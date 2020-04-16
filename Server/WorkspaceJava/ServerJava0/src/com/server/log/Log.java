package com.server.log;

import java.io.IOException;
import java.util.logging.ConsoleHandler;
import java.util.logging.FileHandler;
import java.util.logging.Handler;
import java.util.logging.Level;
import java.util.logging.Logger;

public class Log {
	private static Logger _logger;
	public static void init() {
		//=============================================
		// 기본 로그 제거
		//------------
		Logger rootLogger = Logger.getLogger("");
		Handler[] handlers = rootLogger.getHandlers();
		if (handlers[0] instanceof ConsoleHandler) {
			rootLogger.removeHandler(handlers[0]);
		}
		//=============================================

		_logger = Logger.getGlobal();
		_logger.setLevel(Level.INFO);
		
		//Handler handler = new ConsoleHandler();
		FileHandler handler = null;
		try {
			handler = new FileHandler(FormatterCustom.getFileName(), true);
		} catch (SecurityException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}

		handler.setFormatter(FormatterCustom.getInstance());
		_logger.addHandler(handler);
	}

	public static void log(String log) {
		_logger.info(log);
	}

	public static void test() {
		Logger logger = Logger.getGlobal();
		logger.setLevel(Level.INFO);
		logger.severe("severe Log");
		logger.warning("warning Log");
		logger.info("info Log");



		Log.init();

		Log.log("test");
	}
}
