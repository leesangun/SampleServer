package com.server.nio;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousSocketChannel;
import java.nio.channels.CompletionHandler;

public abstract class BaseClient implements CompletionHandler<Integer, ByteBuffer>{	
	private AsynchronousSocketChannel _channel;
	private int _limitMax;

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
			//----- flip 관련 이 부분 수정 필요
			//buffer.flip();
			buffer.clear();
			if(result > _limitMax)_limitMax = result;
			byte[] bb = buffer.array();
			for (int i=result;i<_limitMax;i++) bb[i] = 0;
			//----------
			
			this.read(buffer);
//			String msg = new String(buffer.array());
//			System.out.println("echo: " + msg);
//			
//			Charset charset = Charset.forName("UTF-8");
//			String data = "네이버 블로그는 부르곰";
//			ByteBuffer byteBuffer = charset.encode(data);
//			_channel.write(byteBuffer);
			
			//buffer = ByteBuffer.allocate(1024);
			

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
/*	
	CompletionHandler<Integer, ByteBuffer> _write = new CompletionHandler<Integer, ByteBuffer>() {
		@Override
		public void completed(Integer result, ByteBuffer attachment) {
			System.out.println("1111");
		}

		@Override
		public void failed(Throwable exc, ByteBuffer attachment) {
			System.out.println("222");
		}
	};
	*/
	public void write(ByteBuffer buffer) {
		//_channel.write(buffer,buffer,_write);
		_channel.write(buffer);
	}
	
	protected abstract void disconnect();
	protected abstract void read(ByteBuffer buffer);
}




