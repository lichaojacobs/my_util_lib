package com.mobvoi.data.portal.api.cache.commands;

import com.mobvoi.data.portal.api.cache.RedisUtil;
import java.util.List;
import java.util.Optional;
import redis.clients.jedis.BinaryClient;
import redis.clients.jedis.Jedis;

/**
 * Created by lichao on 2017/3/24.
 */
public class JedisLists implements ListsCommand {

  private String name;

  public JedisLists(String name) {
    this.name = name;
  }

  public List<String> leftPopWithBlocking(int timeout, String... keys) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.blpop(timeout, keys);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String leftPopWithBlocking(int timeout, String key) {
    return Optional.ofNullable(leftPopWithBlocking(timeout, new String[]{key}))
        .map(list -> list.get(1))
        .orElse(null);
  }


  public List<String> rightPopWithBlocking(int timeout, String... keys) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.brpop(timeout, keys);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String rightPopWithBlocking(int timeout, String key) {
    return Optional.ofNullable(rightPopWithBlocking(timeout, new String[]{key}))
        .map(list -> list.get(1))
        .orElse(null);
  }

  @Override
  public String indexFromLeft(String key, long index) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lindex(key, index);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long leftInsert(String key, BinaryClient.LIST_POSITION where,
      String pivot, String value) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.linsert(key, where, pivot, value);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long length(String key) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.llen(key);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String leftPop(String key) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lpop(key);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long leftPush(String key, String... strings) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lpush(key, strings);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long leftPush(String key, List<String> values) {
    String[] strings = values.toArray(new String[0]);
    return leftPush(key, strings);
  }

  @Override
  public Long leftPushIfExist(String key, String... strings) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lpushx(key, strings);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long leftPushIfExist(String key, List<String> values) {
    String[] strings = values.toArray(new String[0]);
    return leftPushIfExist(key, strings);
  }

  @Override
  public List<String> range(String key, long start, long end) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lrange(key, start, end);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long removeFromLeft(String key, long count, String value) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lrem(key, count, value);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String setFromLeft(String key, long index, String value) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.lset(key, index, value);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String trimFromLeft(String key, long start, long end) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.ltrim(key, start, end);
    } finally {
      jedis.close();
    }
  }

  @Override
  public String rightPop(String key) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.rpop(key);
    } finally {
      jedis.close();
    }
  }


  public String rightPopLeftPush(String srckey, String dstkey) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.rpoplpush(srckey, dstkey);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long rightPush(String key, String... strings) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.rpush(key, strings);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long rightPush(String key, List<String> values) {
    String[] strings = values.toArray(new String[values.size()]);
    return rightPush(key, strings);
  }

  @Override
  public Long rightPushIfExist(String key, String... strings) {
    Jedis jedis = RedisUtil.getResource(name);
    try {
      return jedis.rpushx(key, strings);
    } finally {
      jedis.close();
    }
  }

  @Override
  public Long rightPushIfExist(String key, List<String> values) {
    String[] strings = values.toArray(new String[values.size()]);
    return rightPushIfExist(key, strings);
  }
}
