package com.server.db;


import com.server.Config;
import com.server.Config.OptionRedis;

import redis.clients.jedis.Jedis;
import redis.clients.jedis.JedisPool;
import redis.clients.jedis.JedisPoolConfig;

public class ConnRedis {
	private static ConnRedis _instance;
	public static ConnRedis getInstance() {
		if(_instance==null)_instance = new ConnRedis();
		return _instance;
	}
	
    private JedisPool _pool0,_pool1;
	
	private ConnRedis() {
		JedisPoolConfig jedisPoolConfig = new JedisPoolConfig();
        
		_pool0 = new JedisPool(jedisPoolConfig,Config.OptionRedis.HOST,OptionRedis.PORT,OptionRedis.TIMEOUT,null,0);
		_pool1 = new JedisPool(jedisPoolConfig,Config.OptionRedis.HOST,OptionRedis.PORT,OptionRedis.TIMEOUT,null,1);
       // _pool0.close();
	}
	
	public void setUserKey(String userKey,String userId)
    {
		Jedis jedis = _pool0.getResource();
		jedis.setex(userKey, Config.SEC_USER_SESSION, userId);
		jedis.close();
    }
    public void setUserKeyExpire(String userKey)
    {
    	Jedis jedis = _pool0.getResource();
        jedis.expire(userKey, Config.SEC_USER_SESSION);
        jedis.close();
    }
    public String GetUserKey(String userKey)
    {
    	Jedis jedis = _pool0.getResource();
        String result = jedis.get(userKey);
        jedis.close();
        return result;
    }

    public static void test()
    {
        ConnRedis conn = ConnRedis.getInstance();
        conn.setUserKey("user0Key","user0");

        String result = conn.GetUserKey("user0Key");
        if(result == null)
        {
            System.out.println("User not found");
        }
        else
        {
        	System.out.println(result);
        }
    }
}