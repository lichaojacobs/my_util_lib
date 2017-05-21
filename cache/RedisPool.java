// Copyright 2014 Mobvoi Inc. All Rights Reserved.

package com.mobvoi.data.portal.api.cache;

import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;
import lombok.extern.slf4j.Slf4j;
import org.apache.commons.pool2.impl.GenericObjectPoolConfig;
import org.springframework.util.StringUtils;
import redis.clients.jedis.HostAndPort;
import redis.clients.jedis.JedisPool;

@Slf4j
public class RedisPool {

  private static Set<HostAndPort> getRedisConnection(String connection) {
    if (StringUtils.isEmpty(connection)) {
      return Collections.EMPTY_SET;
    }

    return Arrays.stream(connection.split("\\|")).filter(s -> !StringUtils.isEmpty(s))
        .map(s -> s.split(":")).filter(s -> s.length == 2)
        .map(tokens ->
            new HostAndPort(tokens[0].trim(), Integer.parseInt(tokens[1].trim())
            ))
        .collect(
            Collectors.toSet());
  }

  public static List<JedisPool> getJedisPool(BaseRedisProperties baseRedisProperties) {

    BaseRedisPoolProperties poolProperties = baseRedisProperties.getPool();
    GenericObjectPoolConfig poolConfig = new GenericObjectPoolConfig();
    poolConfig.setMaxIdle(poolProperties.getMaxIdle());
    poolConfig.setMinIdle(poolProperties.getMinIdle());
    poolConfig.setMaxTotal(poolProperties.getMaxTotal());
    poolConfig.setTestOnBorrow(poolProperties.isTestOnBorrow());
    poolConfig.setMaxWaitMillis(poolProperties.getMaxWaitMillis());

    Set<HostAndPort> nodes = getRedisConnection(baseRedisProperties.getConnection());
    return nodes.stream()
        .map(node ->
            new JedisPool(
                poolConfig,
                node.getHost(),
                node.getPort(),
                baseRedisProperties.getTimeout(),
                StringUtils.isEmpty(baseRedisProperties.getPassword()) ? null
                    : baseRedisProperties.getPassword(),
                baseRedisProperties.getDb()
            )
        )
        .collect(Collectors.toList());
  }
}
