package com.server.nio;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousChannelGroup;
import java.nio.channels.AsynchronousServerSocketChannel;
import java.nio.channels.AsynchronousSocketChannel;
import java.nio.channels.CompletionHandler;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import com.server.protocol.ProtocolObject;


public class ServerAsync {
	private static int PORT = 4000;
	private static int threadPoolSize = 8; 
	private static int initialSize = 4;
	private static int backlog = 50; 

	public ServerAsync(){
		ProtocolObject.setData();
		
		ExecutorService executor = Executors.newFixedThreadPool(threadPoolSize); 

		try{
			AsynchronousChannelGroup group = AsynchronousChannelGroup.withCachedThreadPool(executor, initialSize);
			//AsynchronousChannelGroup group = AsynchronousChannelGroup.withFixedThreadPool(1, Executors.defaultThreadFactory());
			AsynchronousServerSocketChannel serverSocketChannel = AsynchronousServerSocketChannel.open(group);
			serverSocketChannel.bind(new InetSocketAddress(PORT), backlog);
			serverSocketChannel.accept(serverSocketChannel, new Dispatcher());
		}catch (IOException e){
			e.printStackTrace();
		}
	}
	
	public static void writeAll(ByteBuffer buffer) {
		for(Client c : Client._listClient) {
			c.write(buffer);
		}
	}
}

class Dispatcher implements CompletionHandler<AsynchronousSocketChannel, AsynchronousServerSocketChannel>{
	private static final int DATA_SIZE = 1024;

	@Override
	public void completed(AsynchronousSocketChannel channel, AsynchronousServerSocketChannel serverSocketChannel) {
		//BaseClient client = new Client(channel);
		BaseClient client = null;
		try {
			client = Client._poolClient.borrowObject();
			client.init(channel);
		} catch (Exception e) {
			e.printStackTrace();
			return;
		}
		
		serverSocketChannel.accept(serverSocketChannel, this);
		ByteBuffer buffer = ByteBuffer.allocate(DATA_SIZE);
		//channel.read(buffer, buffer, new EchoHandler(channel));
		channel.read(buffer, buffer, client);
		
	}

	@Override
	public void failed(Throwable exc, AsynchronousServerSocketChannel listener) {
		System.out.println("dddd");
	}
	
	
}
/*
class EchoHandler implements CompletionHandler<Integer, ByteBuffer>{	
	private AsynchronousSocketChannel _channel;
	public EchoHandler(AsynchronousSocketChannel channel){
		this._channel = channel;
	}

	@Override
	public void completed(Integer result, ByteBuffer buffer){
		if (result == -1){
			System.out.println("close");
			try{
				_channel.close(); 
			}catch (IOException e){
				e.printStackTrace();
			}
		}else if (result > 0){
			buffer.flip();
			
			
			String msg = new String(buffer.array());
			System.out.println("echo: " + msg);
			
			Charset charset = Charset.forName("UTF-8");
			String data = "네이버 블로그는 부르곰";
			ByteBuffer byteBuffer = charset.encode(data);
			_channel.write(byteBuffer);
			
			_channel.read(buffer, buffer, this);
		}
	}

	@Override
	public void failed(Throwable exc, ByteBuffer attachment) {
		System.out.println("disconnects");
		try{
			_channel.close(); 
		}catch (IOException e){
			e.printStackTrace();
		}
	}
	
}
*/