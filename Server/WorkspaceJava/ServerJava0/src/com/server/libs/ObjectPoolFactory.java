package com.server.libs;

import org.apache.commons.pool.BasePoolableObjectFactory;
import org.apache.commons.pool.impl.GenericObjectPool;

//생성자는 디폴트로 하고 따로 Init메소드를 만든다.

//pom.xml
/*
   	<!-- https://mvnrepository.com/artifact/commons-pool/commons-pool -->
		<dependency>
		    <groupId>commons-pool</groupId>
		    <artifactId>commons-pool</artifactId>
		    <version>1.6</version>
		</dependency>

*/

public class ObjectPoolFactory<T> extends BasePoolableObjectFactory<T>{
	private Class<T> clazzOfT;

	public ObjectPoolFactory(Class<T> cls) {
		clazzOfT = cls;
	}
	@Override
	public T makeObject() throws Exception {
		return clazzOfT.getDeclaredConstructor().newInstance();
	}
	
	public static void test() {
		GenericObjectPool<StringBuffer> genericObjectPool = new GenericObjectPool<StringBuffer>(new ObjectPoolFactory<StringBuffer>(StringBuffer.class));
		try {
			for(int i=0;i<10;i++) {
				StringBuffer s = genericObjectPool.borrowObject();
				s.append(i+"");
				System.out.println(s.toString());
				genericObjectPool.returnObject(s);
			}
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
}

