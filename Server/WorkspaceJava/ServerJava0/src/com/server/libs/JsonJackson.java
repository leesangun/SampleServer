package com.server.libs;

import java.io.IOException;
import java.util.Map;

import com.fasterxml.jackson.core.JsonParseException;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;


public class JsonJackson implements BaseJsonSerializer {
	private ObjectMapper _objectMapper = new ObjectMapper();

	@Override
	public <T> T bytesToObject(byte[] bytes, Class<T> classOfT) {
		try {
			return _objectMapper.readValue(bytes, classOfT);
		} catch (JsonParseException e) {
			e.printStackTrace();
		} catch (JsonMappingException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		return null;
	}

	@Override
	public String objectToString(Object obj) {
		try {
			return _objectMapper.writeValueAsString(obj);
		} catch (JsonProcessingException e) {
			e.printStackTrace();
			return null;
		}
	}

	@Override
	public byte[] objectToBytes(Object obj) {
		try {
			return _objectMapper.writeValueAsBytes(obj);
		} catch (JsonProcessingException e) {
			e.printStackTrace();
			return null;
		}
	}

	public Map<?, ?> byteToMap(byte[] bytes) {
		try {
			return _objectMapper.readValue(bytes, Map.class);
		} catch (IOException e) {
			e.printStackTrace();
		}
		return null;
	}
}
