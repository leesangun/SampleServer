package com.server;

import java.nio.ByteBuffer;

import org.apache.commons.pool.BasePoolableObjectFactory;
import org.apache.commons.pool.impl.GenericObjectPool;

import com.server.db.ConnMySql;
import com.server.db.ConnRedis;
import com.server.gui.MainFrame;
import com.server.libs.BaseJsonSerializer;
import com.server.libs.ObjectPoolFactory;
import com.server.log.FormatterCustom;
import com.server.log.Log;
import com.server.log.Log4j;
import com.server.nio.Client;
import com.server.nio.ServerAsync;

public class Main  {

	public static void main(String[] args) {
		//Executors.newSingleThreadExecutor().execute(new Server("localhost", 4000));
		
		new ServerAsync();
		new MainFrame();

		//BaseJsonSerializer.test();
		

		//ObjectPoolFactory.test();
	
		//ConnRedis.test();
		//ConnMySql.test();
	
		//Log4j.test();
	}
}


