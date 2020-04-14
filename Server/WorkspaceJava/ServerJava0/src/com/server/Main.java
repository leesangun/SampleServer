package com.server;

import com.server.libs.BaseJsonSerializer;

public class Main  {
	public static void main(String[] args) {
		//Executors.newSingleThreadExecutor().execute(new Server("localhost", 4000));
		
		new ServerAsync();

		BaseJsonSerializer.test();
		

	}
	
}


