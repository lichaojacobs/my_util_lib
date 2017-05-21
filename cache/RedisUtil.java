// Copyright 2017 Mobvoi Inc. All Rights Reserved.

package com.mobvoi.data.portal.api.cache;

import com.mobvoi.data.portal.api.cache.commands.ListsCommand;
import com.mobvoi.data.portal.api.cache.commands.StringCommands;
import java.util.List;
import java.util.Map;
import java.util.Random;
import java.util.concurrent.ConcurrentHashMap;
import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;

/**
 * Created by lichao on 2017/4/10.
 */
public class RedisUtil {

  private static Map<String, List<JedisPool>> pools = new ConcurrentHashMap<>();

  public static Jedis getResource(String name) {
    if (name == null) {
      throw new RuntimeException("Jedis name is null. Get Jedis resource failed.");
    }

    if (pools == null || pools.isEmpty()) {
      throw new RuntimeException("Jedis pools is empty. Get Jedis resource failed.");
    }

    List<JedisPool> pool = pools.get(name);
    if (pool == null || pool.isEmpty()) {
      throw new RuntimeException(
          "Jedis pools is empty. Get Jedis resource failed. name is " + name);
    }
    return pool.get(new Random().nextInt(pool.size()))
        .getResource();
  }

  protected static Map<String, List<JedisPool>> getPools() {
    return pools;
  }

  protected static void addPool(String poolName, List<JedisPool> jedisPools) {
    pools.put(poolName, jedisPools);
    StringCommands.putJedis(poolName);
    ListsCommand.putJedis(poolName);
  }

  public static void closePool(List<JedisPool> pools) {
    if (pools != null && !pools.isEmpty()) {
      pools.forEach(JedisPool::destroy);
    }
  }
}
