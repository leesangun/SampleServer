module ServerJava0 {
	requires com.google.gson;
	requires com.fasterxml.jackson.databind;
	requires commons.pool;
	requires jedis;
	requires commons.pool2;
	requires commons.dbcp;
	requires java.sql;
	requires java.desktop;
	requires java.logging;
	requires org.apache.logging.log4j.core;
	requires org.apache.logging.log4j;
	exports com.server.protocol;
}

