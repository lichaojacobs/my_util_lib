package com.mobvoi.data.utils;

import com.google.common.collect.Lists;
import com.google.gson.Gson;
import com.mongodb.client.MongoCursor;
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import java.util.List;
import java.util.Optional;
import lombok.extern.slf4j.Slf4j;
import org.bson.Document;

/**
 * Created by lichao on 2017/7/31.
 */
@Slf4j
public class MongoMapper<T> {

  private static final Gson gson = new Gson();

  public static <T> List<T> getModelListFromMongo(MongoCursor<Document> resultCursor,
      ParameterizedType type) {
    List<T> results = Lists.newArrayList();
    try {
      while (resultCursor.hasNext()) {
        results.add(gson.fromJson(gson.toJson(resultCursor.next()), type));
      }
    } catch (Exception ex) {
      log.error("getModelListFromMongo error: ", ex);
    } finally {
      resultCursor.close();
    }

    return results;
  }

  public static <T> Optional<T> getModelFromMongo(MongoCursor<Document> resultCursor,
      ParameterizedType type) {

    try {
      if (resultCursor.hasNext()) {
        return Optional.of(gson.fromJson(gson.toJson(resultCursor.next()), type));
      }
    } catch (Exception ex) {
      log.error("getModelFromMongo error: ", ex);
    } finally {
      resultCursor.close();
    }

    return null;
  }

  public static ParameterizedType type(final Class raw, final Type... args) {
    return new ParameterizedType() {
      public Type getRawType() {
        return raw;
      }

      public Type[] getActualTypeArguments() {
        return args;
      }

      public Type getOwnerType() {
        return null;
      }
    };
  }
}
