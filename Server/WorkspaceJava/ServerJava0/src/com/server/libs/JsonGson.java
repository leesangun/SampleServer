package com.server.libs;

import java.io.UnsupportedEncodingException;

import com.google.gson.Gson;

public class JsonGson implements BaseJsonSerializer {
	private Gson _gson = new Gson();

	@Override
	public <T> T bytesToObject(byte[] bytes, Class<T> classOfT) {
		try {
			return _gson.fromJson(new String(bytes,"utf-8"), classOfT);
		}  catch (UnsupportedEncodingException e) {
			e.printStackTrace();
			return null;
		}
	}

	@Override
	public String objectToString(Object obj) {
		return _gson.toJson(obj);
	}

	@Override
	public byte[] objectToBytes(Object obj) {
		try {
			return this.objectToString(obj).getBytes("utf-8");
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
			return null;
		}
	}

}
