package com.ricebook.rhllor.courier.utils;

import com.google.common.collect.ImmutableMap;
import com.google.common.collect.Lists;

import org.apache.commons.collections.CollectionUtils;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Optional;
import java.util.function.Predicate;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

/**
 * Created by chao li on 16/12/14.
 */

public class SqlWhereBuffer {

  public static SqlWhereBuffer.Where builder() {
    return new Builder();
  }

  static class Builder implements SqlWhereBuffer.Order, SqlWhereBuffer.Where {
    private List<String> sql = new ArrayList<>();
    private Map<String, Object> params = new HashMap<>();
    private String orderBy = " ";
    private String limit = " ";
    private String in = " ";
    private List<SqlMapperPlugin> sqlMapperPlugins;

    public Builder() {
      //添加基本类型默认的插件
      sqlMapperPlugins = Lists.newArrayList();
      this.mapperPlugins(SqlMapperPlugin.EnumSqlPlugin)
          .mapperPlugins(SqlMapperPlugin.BooleanSqlPlugin);
    }

    public Builder mapperPlugins(SqlMapperPlugin sqlMapperPlugin) {
      if (CollectionUtils.isEmpty(sqlMapperPlugins)) {
        sqlMapperPlugins = Lists.newArrayList();
      }
      sqlMapperPlugins.add(sqlMapperPlugin);
      return this;
    }

    @Override
    public SqlWhereBuffer.Where ifPresent(Object value, String sql) {
      Optional.ofNullable(value)
          .ifPresent(val -> {
            this.sql.add(sql);
            Pattern pattern = Pattern.compile(":([a-z,A-Z,\\w,_]*)");
            Matcher matcher = pattern.matcher(sql);
            if (!matcher.find()) {
              throw new IllegalArgumentException(sql + " don't include :name");
            }

            Optional<SqlMapperPlugin> mapper = sqlMapperPlugins.stream()
                .filter(sqlMapperPlugin -> sqlMapperPlugin.test(val))
                .findFirst();
            if (mapper.isPresent()) {
              params.putAll(mapper.get()
                  .getParams(matcher.group(1), value));
            } else {
              params.put(matcher.group(1), value);
            }
          });
      return this;
    }

    @Override
    public SqlWhereBuffer.Order orderBy(String field) {
      orderBy += "ORDER BY " + field;
      return this;
    }

    @Override
    public SqlWhereBuffer.Where limit(int offset, int count) {
      limit += "limit " + offset + "," + count;
      return this;
    }

    @Override
    public Where in(List<Object> values, String sql) {
      return null;
    }

    @Override
    public String sql() {
      if (sql.isEmpty() || params.isEmpty()) {
        return "";
      } else {
        return " WHERE " + sql.stream()
            .collect(Collectors.joining(" AND ")) + orderBy + limit;
      }
    }

    @Override
    public ImmutableMap<String, Object> params() {
      return ImmutableMap.copyOf(params);
    }

    @Override
    public SqlWhereBuffer.Where desc() {
      orderBy += " DESC";
      return this;
    }

    @Override
    public SqlWhereBuffer.Where asc() {
      return this;
    }
  }

  public static class SqlMapperPlugin {

    private final Predicate<Object> predicate;
    private final ParamValue paramValue;

    private SqlMapperPlugin(Predicate<Object> predicate,
        ParamValue paramValue) {
      this.predicate = predicate;
      this.paramValue = paramValue;
    }

    static SqlMapperPlugin EnumSqlPlugin = of(Enum.class).columnValue((value, sql) ->
        Collections.singletonMap(sql, getEnumValue(value))
    );

    static SqlMapperPlugin BooleanSqlPlugin = of(Boolean.class).columnValue((value, sql) ->
        Collections.singletonMap(sql, ((boolean) value) ? 1 : 0)
    );

    public static SqlMapperPlugin.MapperPluginsBuilder of(Predicate<Object> predicate) {
      return new MapperPluginsBuilder(predicate);
    }

    public static SqlMapperPlugin.MapperPluginsBuilder of(Class clazz) {
      return of((pd) -> {
        return clazz.isAssignableFrom(pd.getClass());
      });
    }

    boolean test(Object pd) {
      return this.predicate.test(pd);
    }

    Map<String, Object> getParams(String sql, Object value) {
      return paramValue.getParams(value, sql);
    }

    public static class MapperPluginsBuilder {

      Predicate<Object> predicate;

      public MapperPluginsBuilder(Predicate<Object> predicate) {
        this.predicate = predicate;
      }

      public SqlMapperPlugin columnValue(SqlMapperPlugin.ParamValue paramValue) {
        return new SqlMapperPlugin(this.predicate, paramValue);
      }
    }

    @FunctionalInterface
    public interface ParamValue {
      Map<String, Object> getParams(Object var1, String var2);
    }
  }

  public static String getEnumValue(Object value) {
    Method method;
    try {
      method = value.getClass()
          .getMethod("name");
      return String.valueOf(method.invoke(value));
    } catch (NoSuchMethodException e) {
      throw new RuntimeException("NoSuchMethodException", e);
    } catch (IllegalAccessException e) {
      throw new RuntimeException("IllegalAccessException", e);
    } catch (InvocationTargetException e) {
      throw new RuntimeException("InvocationTargetException", e);
    }
  }

  public interface Order {
    Where desc();

    Where asc();
  }

  public interface Where {
    Where ifPresent(Object value, String sql);

    Order orderBy(String field);

    Where limit(int begin, int end);

    Where in(List<Object> values, String sql);

    String sql();

    Map<String, Object> params();
  }

}
