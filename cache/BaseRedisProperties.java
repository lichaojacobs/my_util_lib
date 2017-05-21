// Copyright 2017 Mobvoi Inc. All Rights Reserved.

package com.mobvoi.data.portal.api.cache;

import java.util.function.Supplier;
import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.PropertySource;
import org.springframework.stereotype.Component;

/**
 * Created by lichao on 2017/5/12.
 */
@Data
@Component
@PropertySource(value = "classpath:application.properties")
public class BaseRedisProperties {

  public static final int DEFAULT_TIMEOUT = 2000;

  public static final String DEFAULT_PASSWORD = null;

  public static final int DEFAULT_DB = 0;

  public static final Supplier<BaseRedisPoolProperties> DEFAULT_POOL = BaseRedisPoolProperties::new;

  //redis name
  @Value("${redis.name}")
  private String name;
  // 连接地址 ip:port
  @Value("${redis.connection}")
  private String connection;

  // 超时时间
  private int timeout = DEFAULT_TIMEOUT;

  // 密码
  @Value("${redis.password}")
  private String password = DEFAULT_PASSWORD;

  // 数据库
  private int db = DEFAULT_DB;

  // 连接池配置
  private BaseRedisPoolProperties pool = DEFAULT_POOL.get();
}
