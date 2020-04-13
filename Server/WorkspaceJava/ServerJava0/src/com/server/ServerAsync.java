package com.server;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousChannelGroup;
import java.nio.channels.AsynchronousServerSocketChannel;
import java.nio.channels.AsynchronousSocketChannel;
import java.nio.channels.CompletionHandler;
import java.nio.charset.Charset;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ServerAsync {

	private static int PORT = 4000;
	private static int threadPoolSize = 8; 
	private static int initialSize = 4;
	private static int backlog = 50; 

	public ServerAsync()
	{
		ExecutorService executor = Executors.newFixedThreadPool(threadPoolSize); 

		try
		{
			AsynchronousChannelGroup group = AsynchronousChannelGroup.withCachedThreadPool(executor, initialSize);
			//AsynchronousChannelGroup group = AsynchronousChannelGroup.withFixedThreadPool(1, Executors.defaultThreadFactory());
			AsynchronousServerSocketChannel listener = AsynchronousServerSocketChannel.open(group);
			listener.bind(new InetSocketAddress(PORT), backlog);
			listener.accept(listener, new Dispatcher());
		}
		catch (IOException e)
		{
			e.printStackTrace();
		}
	}
}

class Dispatcher implements CompletionHandler<AsynchronousSocketChannel, AsynchronousServerSocketChannel>
{
	private int DATA_SIZE = 1024;

	@Override
	public void completed(AsynchronousSocketChannel channel, AsynchronousServerSocketChannel listener) 
	{
		System.out.println("connect");
		listener.accept(listener, this);
		ByteBuffer buffer = ByteBuffer.allocate(DATA_SIZE);
		channel.read(buffer, buffer, new EchoHandler(channel));
	}

	@Override
	public void failed(Throwable exc, AsynchronousServerSocketChannel listener) 
	{
		System.out.println("dddd");
	}
}

class EchoHandler implements CompletionHandler<Integer, ByteBuffer>
{
	private AsynchronousSocketChannel channel;
	public EchoHandler(AsynchronousSocketChannel channel)
	{
		this.channel = channel;
	}

	@Override
	public void completed(Integer result, ByteBuffer buffer)
	{
		if (result == -1)
		{
			System.out.println("close");
			try
			{
				channel.close(); 
			}
			catch (IOException e)
			{
				e.printStackTrace();
			}
		}
		else if (result > 0)
		{
			buffer.flip();
			String msg = new String(buffer.array());
			System.out.println("echo: " + msg);
			
			Charset charset = Charset.forName("UTF-8");
			String data = "네이버 블로그는 부르곰";
			ByteBuffer byteBuffer = charset.encode(data);
			channel.write(byteBuffer);
			
			channel.read(buffer, buffer, this);
		}
	}

	@Override
	public void failed(Throwable exc, ByteBuffer attachment) {
		System.out.println("disconnect");
	}

}
