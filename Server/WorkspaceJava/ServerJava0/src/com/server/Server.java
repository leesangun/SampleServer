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
		try (Selector selector = Selector.open();						//������ ����
				ServerSocketChannel serverSocketChannel = ServerSocketChannel.open();) //�������� ����
		{
			serverSocketChannel.configureBlocking(false);				
			serverSocketChannel.socket().bind(this._inetSocketAddress);				
			serverSocketChannel.register(selector, SelectionKey.OP_ACCEPT);	//�����û �������� ����

			while (selector.select() > 0) {							//����� ������ �ִ� ���
				handleSelectedKeys(selector);						//���õ� ���� ó��
			}

		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	//selectedKeys�� �ִ� ��� key�� ���¸� ���� �׿� ���� ó���� �Ѵ�.
	private void handleSelectedKeys(Selector selector) throws IOException {
		Iterator<SelectionKey> iter = selector.selectedKeys().iterator(); //����� ���� �ݷ���
		while (iter.hasNext()) {
			SelectionKey key = (SelectionKey) iter.next();
			iter.remove();
			if (key.isValid()) {
				if (key.isAcceptable()) {						//Ŭ���̾�Ʈ�� ���� ��û�� ���� ��
					acceptSocket(selector, key);
				} 
				else if (key.isConnectable()) {
					System.out.format("Connectable is for client.%n");
				} 
				else if (key.isReadable()) {					//�����Ͱ� ���� ���� ��
					this.read(selector,key);
				} 
				else if (key.isWritable()) {
					this.write(selector,key);
				}
			}
		}
	}

	//key.isAcceptable()�϶� key�� ������������ �����û�� �� ������ �޾� ���̰� ��带 OP_READ�� �ٲ۴�.
	private void acceptSocket(Selector selector, SelectionKey key) throws IOException {
		ServerSocketChannel ssc = (ServerSocketChannel)key.channel();
		SocketChannel socketChannel = ssc.accept();				
		socketChannel.configureBlocking(false);					
		
		//���⼭ Ŭ���̾�Ʈ��ü ����
		
		StringBuffer sb = new StringBuffer();
		//socketChannel.register(selector, SelectionKey.OP_READ,sb);	//�б� ���� ���
		sb.append("Welcome server!\r\n>");
		socketChannel.register(selector, SelectionKey.OP_WRITE, sb);
		
		System.out.format("Accepted: %s%n",socketChannel.socket().getRemoteSocketAddress().toString());
	}

	//key.isReadable()�϶� �����͸� �о ����Ѵ�.
	private void read(Selector selector, SelectionKey key) {
		ByteBuffer buf = ByteBuffer.allocate(1024);

		SocketChannel socketChannel = (SocketChannel)key.channel();
		buf.clear();								//buf�ʱ�ȭ

		try{
			int size = socketChannel.read(buf);	//����ä�ο��� ����
			
			if (size == -1) {						//������ �������� ��
				this.disconnect(key);
				return;
			}
			buf.flip();									//limit=position, position=0
			
			
			String msg = StandardCharsets.UTF_8.decode(buf).toString();
			System.out.format("MESSAGE RECEIVE(%d): %s%n", size, msg);

			StringBuffer sb = (StringBuffer) key.attachment();
			sb.append(msg);
			// Socket ä���� channel�� �۽� ����Ѵ�
			socketChannel.register(selector, SelectionKey.OP_WRITE, sb);
		}catch (IOException e) {
			//e.printStackTrace();
			System.out.println(e.toString());
			this.disconnect(key);
		}
	}

	private void write(Selector selector, SelectionKey key) {
		try {
			// Ű ä���� �����´�.
			SocketChannel channel = (SocketChannel) key.channel();
			// ä�� Non-blocking ����
			channel.configureBlocking(false);
			// StringBuffer ���
			StringBuffer sb = (StringBuffer) key.attachment();
			String data = sb.toString();
			// StringBuffer �ʱ�ȭ
			sb.setLength(0);
			// byte �������� ��ȯ
			ByteBuffer buffer = ByteBuffer.wrap(data.getBytes());
			// ***������ �۽�***
			channel.write(buffer);
			// Socket ä���� channel�� ���� ����Ѵ�
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
