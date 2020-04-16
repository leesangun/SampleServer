package com.server.log;

import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.logging.ConsoleHandler;
import java.util.logging.FileHandler;
import java.util.logging.Formatter;
import java.util.logging.Handler;
import java.util.logging.Level;
import java.util.logging.LogRecord;
import java.util.logging.Logger;


public class FormatterCustom extends Formatter {
	private static FormatterCustom _instance;
	public static FormatterCustom getInstance() {
		if(_instance==null)_instance = new FormatterCustom();
		return _instance;
	}
	private FormatterCustom() {
		
	}
	
    public String getHead(Handler h) {
        return "START LOG\n";
    }
    
    public String format(LogRecord rec) {
        StringBuffer buf = new StringBuffer(1000);
 
        buf.append(calcDate(rec.getMillis()));
        
        buf.append(" [");
        buf.append(rec.getLevel());
        buf.append("] ");
        
        buf.append("[");
        buf.append(rec.getSourceMethodName());
        buf.append("] ");
        
        buf.append(rec.getMessage());
        buf.append("\n");
        
        return buf.toString();
    }
 
    public String getTail(Handler h) {
        return "END LOG\n";
    }
    
    private String calcDate(long millisecs) {
        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm");
        Date resultdate = new Date(millisecs);
        return dateFormat.format(resultdate);
    }
    
    private static final SimpleDateFormat _dateFormat = new SimpleDateFormat( "yyyy-MM-dd HH-mm-ss");// ( "yyyy-MM-dd HH:mm:ss");
    public static String getFileName() {
        return String.format("%s message.log",_dateFormat.format(Calendar.getInstance().getTime()));
    }
    
    
}
