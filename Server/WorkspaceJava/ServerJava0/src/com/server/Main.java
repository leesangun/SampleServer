package com.server;

import org.apache.commons.pool.BasePoolableObjectFactory;
import org.apache.commons.pool.impl.GenericObjectPool;

import com.server.libs.BaseJsonSerializer;
import com.server.libs.ObjectPoolFactory;
import com.server.nio.Client;
import com.server.nio.ServerAsync;

public class Main  {

	public static void main(String[] args) {
		//Executors.newSingleThreadExecutor().execute(new Server("localhost", 4000));
		
		new ServerAsync();

		//BaseJsonSerializer.test();
		

		//ObjectPoolFactory.test();
	}
	
}


