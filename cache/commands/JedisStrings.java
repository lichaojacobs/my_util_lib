package com.mobvoi.data.portal.api.cache.commands;

import com.mobvoi.data.portal.api.cache.RedisUtil;
import redis.clients.jedis.Jedis;

/**
 * Created by lichao on 2017/3/24.
 */
public class JedisStrings implements StringCommands {

  private String name;

  public JedisStrings(String name) {
    this.name = name;
  }

  @Override
  public String get(String key) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.get(key);
    } finally {
      jedis.close();
    }
  }

  @Override
  public void setWithExpire(String key, int seconds, String value) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      jedis
          .setex(key, seconds, value);
    } finally {
      jedis.close();
    }
  }

  @Override
  public void del(String key) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      jedis.del(key);
    } finally {
      jedis.close();
    }
  }
}
