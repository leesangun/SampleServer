package com.server.db;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import javax.sql.DataSource;

import org.apache.commons.dbcp.ConnectionFactory;
import org.apache.commons.dbcp.DriverManagerConnectionFactory;
import org.apache.commons.dbcp.PoolableConnectionFactory;
import org.apache.commons.dbcp.PoolingDataSource;
import org.apache.commons.pool.impl.GenericObjectPool;

import com.server.Config;

public class ConnMySql {
	private static ConnMySql _instance;
	public static ConnMySql getInstance() {
		if(_instance==null)_instance = new ConnMySql();
		return _instance;
	}
	

	private GenericObjectPool _gPool;
	private DataSource _dataSource;
	
	private ConnMySql() {
		try {
			Class.forName(Config.OptionMySql.JDBC_DRIVER);
		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		}

		// Creates an Instance of GenericObjectPool That Holds Our Pool of Connections Object!
		_gPool = new GenericObjectPool();
		_gPool.setMaxActive(Config.OptionMySql.POOL_MAX_ACTIVE);

		// Creates a ConnectionFactory Object Which Will Be Use by the Pool to Create the Connection Object!
		ConnectionFactory cf = new DriverManagerConnectionFactory(Config.OptionMySql.JDBC_DB_URL, Config.OptionMySql.JDBC_USER, Config.OptionMySql.JDBC_PASS);

		// Creates a PoolableConnectionFactory That Will Wraps the Connection Object Created by the ConnectionFactory to Add Object Pooling Functionality!
		PoolableConnectionFactory pcf = new PoolableConnectionFactory(cf, _gPool, null, null, false, true);
		_dataSource = new PoolingDataSource(_gPool);
	}

	// This Method Is Used To Print The Connection Pool Status
	private void printDbStatus() {
		System.out.println("Max.: " + _gPool.getMaxActive() + "; Active: " + _gPool.getNumActive() + "; Idle: " + _gPool.getNumIdle());
	}
	
    private void select(OnResultMySql onResult,String sql)
    {
    	this.printDbStatus();
    	Connection connection = null;
    	PreparedStatement preparedStatement = null;
    	ResultSet resultSet = null;
    	try {
			 connection = _dataSource.getConnection();
			 preparedStatement = connection.prepareStatement(sql);
			 resultSet = preparedStatement.executeQuery();
			 onResult.onResult(resultSet);
		} catch (SQLException e) {
			e.printStackTrace();
		}finally {
			try {
				if(resultSet != null) {
					resultSet.close();
				}
				if(preparedStatement != null) {
					preparedStatement.close();
				}
				if(connection != null) {
					connection.close();
				}
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
    	this.printDbStatus();
    }
    public void select0(OnResultMySql onResult){
    	this.select(onResult, "SELECT * FROM logPortfolio LIMIT 0,5");
    }

	public static void test() {
		ConnMySql conn = ConnMySql.getInstance();
		conn.select0(new OnResultMySql() {
			@Override
			public void onResult(ResultSet resultSet) {
				try {
					while (resultSet.next()) {
						System.out.println(resultSet.getString("time"));
					}
				} catch (SQLException e) {
					e.printStackTrace();
				}
			}
		});
	}
}
