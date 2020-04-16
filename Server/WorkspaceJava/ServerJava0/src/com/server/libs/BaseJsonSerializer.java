package com.server.libs;

import com.server.protocol.Protocol;
import com.server.protocol.ResMessage;

//porm.xml
/*
<dependencies>
	<dependency>
		<groupId>com.google.code.gson</groupId>
		<artifactId>gson</artifactId>
		<version>2.8.6</version>
	</dependency>
		
	<dependency>
		<groupId>com.fasterxml.jackson.core</groupId>
		<artifactId>jackson-databind</artifactId>
		<version>2.10.0</version>
  	</dependency>
</dependencies> 
*/

//module-info.java
//exports com.server.protocol; ����ȭ �� Ŭ�������� �ִ� ��Ű������ �����Ѵ�.
//public�� �Ǿ�� �ν��ϹǷ� �� Ŭ������ �����Ϸ� ���� �� ����.

//Gson null�� ������   {"message":"1111","key":"reqMessage"}   ������ �����ε�
//JsonJackson null ��  {"key":"reqMessage","result":null,"message":"1111"}

public interface BaseJsonSerializer {
	public <T> T bytesToObject(byte[] bytes,Class<T> classOfT);

	public String objectToString(Object obj);
	public byte[] objectToBytes(Object obj);
	
	public static void test() {
		ResMessage res = new ResMessage();
		res.key = Protocol.EnumKey.reqMessage;
		res.result = Protocol.EnumResResult.success;
		res.message = "1111";
		
		BaseJsonSerializer json = new JsonGson();
		byte[] bytes = json.objectToBytes(res);
		String str = json.objectToString(res);
		res = json.bytesToObject(bytes, ResMessage.class);
		System.out.println(str + " " + res.message);
		
		json = new JsonJackson();
		bytes = json.objectToBytes(res);
		str = json.objectToString(res);
		res = json.bytesToObject(bytes, ResMessage.class);
		System.out.println(str + " " + res.message);
	}
}
