// Copyright 2017 Mobvoi Inc. All Rights Reserved.

package com.mobvoi.data.portal.api.cache;

import lombok.Data;

/**
 * Created by lichao on 2017/4/10.
 */
@Data
//@ConfigurationProperties(prefix = "redis")
public class BaseRedisPoolProperties {

  public static final int DEFAULT_MAX_TOTAL = 256;
  public static final int DEFAULT_MAX_IDLE = 32;
  public static final int DEFAULT_MIN_IDLE = 8;
  public static final int DEFAULT_MAX_WAIT_MILLIS = 1000;
  public static final boolean DEFAULT_TEST_ON_BORROW = true;

  // 最大连接数
  private int maxTotal = DEFAULT_MAX_TOTAL;

  // 最大空闲数
  private int maxIdle = DEFAULT_MAX_IDLE;

  // 最小空闲数
  private int minIdle = DEFAULT_MIN_IDLE;

  // 最大等待时间
  private int maxWaitMillis = DEFAULT_MAX_WAIT_MILLIS;
  // 是否测试连接
  private boolean testOnBorrow = DEFAULT_TEST_ON_BORROW;
}
