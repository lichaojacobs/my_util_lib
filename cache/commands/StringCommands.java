package com.mobvoi.data.portal.api.cache.commands;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Created by lichao on 2017/5/14.
 */
public interface StringCommands {

  Map<String, JedisStrings> map = new ConcurrentHashMap<>();

  static JedisStrings getJedis(final String name) {
    return map.get(name);
  }

  static void putJedis(final String name) {
    map.put(name, new JedisStrings(name));
  }

  String get(final String key);

  void setWithExpire(final String key, final int seconds, final String value);

  void del(final String key);
}
