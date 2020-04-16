package com.server.log;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.core.Logger;
import org.apache.logging.log4j.core.config.xml.XmlConfigurationFactory;

public class Log4j {

	public static void init() {
		System.setProperty(XmlConfigurationFactory.CONFIGURATION_FILE_PROPERTY,"log4j.xml");
		
	}
	
	public static void test() {
		Log4j.init();
		
		Logger logger = (Logger) LogManager.getLogger(Log4j.class);
		logger.fatal("fatal log");
		logger.error("error log");
		logger.warn("warn log");
		logger.info("info log");
		logger.debug("debug log");
	}
}
