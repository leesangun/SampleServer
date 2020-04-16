package com.server;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.charset.StandardCharsets;
import java.util.Iterator;

public class Server  implements Runnable{
	InetSocketAddress _inetSocketAddress;
	
	public Server(String ip, int port) {
		_inetSocketAddress = new InetSocketAddress(ip, port);
	}

	@Override
	public void run() {
		try (Selector selector = Selector.open();						//셀렉터 오픈
				ServerSocketChannel serverSocketChannel = ServerSocketChannel.open();) //서버소켓 오픈
		{
			serverSocketChannel.configureBlocking(false);				
			serverSocketChannel.socket().bind(this._inetSocketAddress);				
			serverSocketChannel.register(selector, SelectionKey.OP_ACCEPT);	//연결요청 수락모드로 설정

			while (selector.select() > 0) {							//입출력 소켓이 있는 경우
				handleSelectedKeys(selector);						//선택된 소켓 처리
			}

		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	//selectedKeys에 있는 모든 key의 상태를 보고 그에 따른 처리를 한다.
	private void handleSelectedKeys(Selector selector) throws IOException {
		Iterator<SelectionKey> iter = selector.selectedKeys().iterator(); //입출력 소켓 콜렉션
		while (iter.hasNext()) {
			SelectionKey key = (SelectionKey) iter.next();
			iter.remove();
			if (key.isValid()) {
				if (key.isAcceptable()) {						//클라이언트의 연결 요청이 왔을 때
					acceptSocket(selector, key);
				} 
				else if (key.isConnectable()) {
					System.out.format("Connectable is for client.%n");
				} 
				else if (key.isReadable()) {					//데이터가 도착 했을 때
					this.read(selector,key);
				} 
				else if (key.isWritable()) {
					this.write(selector,key);
				}
			}
		}
	}

	//key.isAcceptable()일때 key의 서버소켓으로 연결요청이 온 소켓을 받아 들이고 모드를 OP_READ로 바꾼다.
	private void acceptSocket(Selector selector, SelectionKey key) throws IOException {
		ServerSocketChannel ssc = (ServerSocketChannel)key.channel();
		SocketChannel socketChannel = ssc.accept();				
		socketChannel.configureBlocking(false);					
		
		//여기서 클라이언트객체 생성
		
		StringBuffer sb = new StringBuffer();
		//socketChannel.register(selector, SelectionKey.OP_READ,sb);	//읽기 모드로 등록
		sb.append("Welcome server!\r\n>");
		socketChannel.register(selector, SelectionKey.OP_WRITE, sb);
		
		System.out.format("Accepted: %s%n",socketChannel.socket().getRemoteSocketAddress().toString());
	}

	//key.isReadable()일때 데이터를 읽어서 출력한다.
	private void read(Selector selector, SelectionKey key) {
		ByteBuffer buf = ByteBuffer.allocate(1024);

		SocketChannel socketChannel = (SocketChannel)key.channel();
		buf.clear();								//buf초기화

		try{
			int size = socketChannel.read(buf);	//소켓채널에서 읽음
			
			if (size == -1) {						//연결이 끊어졌을 때
				this.disconnect(key);
				return;
			}
			buf.flip();									//limit=position, position=0
			
			
			String msg = StandardCharsets.UTF_8.decode(buf).toString();
			System.out.format("MESSAGE RECEIVE(%d): %s%n", size, msg);

			StringBuffer sb = (StringBuffer) key.attachment();
			sb.append(msg);
			// Socket 채널을 channel에 송신 등록한다
			socketChannel.register(selector, SelectionKey.OP_WRITE, sb);
		}catch (IOException e) {
			//e.printStackTrace();
			System.out.println(e.toString());
			this.disconnect(key);
		}
	}

	private void write(Selector selector, SelectionKey key) {
		try {
			// 키 채널을 가져온다.
			SocketChannel channel = (SocketChannel) key.channel();
			// 채널 Non-blocking 설정
			channel.configureBlocking(false);
			// StringBuffer 취득
			StringBuffer sb = (StringBuffer) key.attachment();
			String data = sb.toString();
			// StringBuffer 초기화
			sb.setLength(0);
			// byte 형식으로 변환
			ByteBuffer buffer = ByteBuffer.wrap(data.getBytes());
			// ***데이터 송신***
			channel.write(buffer);
			// Socket 채널을 channel에 수신 등록한다
			channel.register(selector, SelectionKey.OP_READ, sb);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	private void disconnect(SelectionKey key) {
		System.out.println("disconnect");
		
		SocketChannel socketChannel = (SocketChannel)key.channel();
		try {
			socketChannel.close();
			socketChannel.socket().close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		key.cancel();
	}
}
