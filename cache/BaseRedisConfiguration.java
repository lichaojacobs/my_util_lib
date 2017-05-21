// Copyright 2017 Mobvoi Inc. All Rights Reserved.

package com.mobvoi.data.portal.api.cache;

import static com.google.common.base.Preconditions.checkArgument;

import com.google.common.base.Strings;
import java.util.List;
import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.BeansException;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.config.BeanPostProcessor;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Configuration;
import redis.clients.jedis.JedisPool;

/**
 * Created by lichao on 2017/4/10.
 */
@Configuration
@Slf4j
public class BaseRedisConfiguration implements BeanPostProcessor {

  @Autowired
  BaseRedisProperties baseRedisProperties;

  @PostConstruct
  public void init() {
    String connection = baseRedisProperties.getConnection();
    checkArgument(!Strings.isNullOrEmpty(connection), "redis connnection is null");
    List<JedisPool> jedisPools = RedisPool.getJedisPool(baseRedisProperties);
    RedisUtil.addPool(baseRedisProperties.getName(), jedisPools);
  }

  @PreDestroy
  public void destroy() {
    if (RedisUtil.getPools() != null && !RedisUtil.getPools()
        .isEmpty()) {
      RedisUtil.getPools()
          .forEach((name, pool) -> {
            log.info("close redis pool, name is {}", name);
            RedisUtil.closePool(pool);
          });
    }
  }

  @Override
  public Object postProcessBeforeInitialization(Object bean, String beanName)
      throws BeansException {
    return bean;
  }

  @Override
  public Object postProcessAfterInitialization(Object bean, String beanName) throws BeansException {
    return bean;
  }
}
