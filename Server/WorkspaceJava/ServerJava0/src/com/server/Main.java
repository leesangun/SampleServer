package com.server;


import java.util.concurrent.Executors;


public class Main  {

	

	public static void main(String[] args) {
		Executors.newSingleThreadExecutor().execute(new Server("localhost", 4000));
		
		
	}

}
