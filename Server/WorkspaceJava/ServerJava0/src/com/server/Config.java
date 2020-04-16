package com.server;

public class Config {
	public static class OptionRedis{
		public static final String HOST = "127.0.0.1";
		public static final int PORT = 6379;
		public static final int TIMEOUT = 3000;
	}

	public class OptionMySql{
		public static final String 
			JDBC_DRIVER = "com.mysql.jdbc.Driver",  
			JDBC_DB_URL = "jdbc:mysql://lsu3.cafe24.com:3306/lsu3",
			JDBC_USER = "lsu3",
			JDBC_PASS = "ghtmxld1";
		
		public static final int POOL_MAX_ACTIVE = 5;
	}


	public static final int SEC_USER_SESSION = 60;
}
