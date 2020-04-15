package com.server.nio;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousSocketChannel;
import java.nio.channels.CompletionHandler;

public abstract class BaseClient implements CompletionHandler<Integer, ByteBuffer>{	
	private AsynchronousSocketChannel _channel;
	

	public void init(AsynchronousSocketChannel channel) {
		this._channel = channel;
	}
	
//	public BaseClient(AsynchronousSocketChannel channel){
//		this._channel = channel;
//	}

	@Override
	public void completed(Integer result, ByteBuffer buffer){
		if (result == -1){
			this.disconnect();
			try{
				_channel.close(); 
			}catch (IOException e){
				e.printStackTrace();
			}
		}else if (result > 0){
			buffer.flip();
			
			this.read(buffer);
			
//			String msg = new String(buffer.array());
//			System.out.println("echo: " + msg);
//			
//			Charset charset = Charset.forName("UTF-8");
//			String data = "네이버 블로그는 부르곰";
//			ByteBuffer byteBuffer = charset.encode(data);
//			_channel.write(byteBuffer);
			
			_channel.read(buffer, buffer, this);
		}
	}

	@Override
	public void failed(Throwable exc, ByteBuffer attachment) {
		this.disconnect();
		try{
			_channel.close(); 
		}catch (IOException e){
			e.printStackTrace();
		}
	}
	
	public void write(ByteBuffer buffer) {
		_channel.write(buffer);
	}
	
	protected abstract void disconnect();
	protected abstract void read(ByteBuffer buffer);
}