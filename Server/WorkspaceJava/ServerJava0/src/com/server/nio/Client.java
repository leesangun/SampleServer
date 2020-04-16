package com.server.nio;

import java.nio.ByteBuffer;
import java.nio.channels.AsynchronousSocketChannel;
import java.util.ArrayList;
import java.util.List;

import org.apache.commons.pool.impl.GenericObjectPool;

import com.server.libs.JsonJackson;
import com.server.libs.ObjectPoolFactory;

import com.server.protocol.*;
import com.server.protocol.Protocol.EnumKey;
import com.server.protocol.Protocol.EnumResResult;

public class Client extends BaseClientRoom{
	public static final GenericObjectPool<Client> _poolClient = new GenericObjectPool<Client>(new ObjectPoolFactory<Client>(Client.class));
	public static final JsonJackson _json = new JsonJackson();
	public static final List<Client> _listClient = new ArrayList<Client>();
	
	private final OnReceiveFn _onReceiveFn;
	//	public Client(AsynchronousSocketChannel channel) {
	//		super(channel);
	//		
	//		System.out.println("connect");
	//	}

	public Client() {
		_onReceiveFn = new OnReceiveFn(this);
	}

	@Override
	public void init(AsynchronousSocketChannel channel) {
		super.init(channel);

		_listClient.add(this);

		System.out.println("connect");
	}


	@Override
	protected void disconnect() {
		super.disconnect();
		System.out.println("disconnect");
		
		_listClient.remove(this);
		try {
			_poolClient.returnObject(this);
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	@Override
	protected void read(ByteBuffer buffer) {
		String msg = new String(buffer.array());
		System.out.println(msg);		

		byte[] bytes = buffer.array();
		int key = (int)_json.byteToMap(bytes).get("key");
		switch (key) {
		case Protocol.EnumKey.reqLogin:
		{
			_onReceiveFn.reqLogin(_json.bytesToObject(bytes, ReqLogin.class));
			break;
		}
		case Protocol.EnumKey.reqRoomAreaJoin:
		{
			_onReceiveFn.reqRoomAreaJoin(_json.bytesToObject(bytes, ReqRoomAreaJoin.class));
			break;
		}
		case Protocol.EnumKey.reqRoomAreaMessage:
		{
			_onReceiveFn.reqRoomAreaMessage(_json.bytesToObject(bytes, ReqRoomAreaMessage.class));
			break;
		}
		case Protocol.EnumKey.reqMessage:
		{
			_onReceiveFn.reqMessage(_json.bytesToObject(bytes, ReqMessage.class));
			break;
		}

		default:{

		}
		}

		/*
		switch (EnumKey.valueToEnum(key))
        {
            case reqLogin:
                {
                    _onReceiveFn.reqLogin(_json.bytesToObject(bytes, ReqLogin.class));
                    break;
                }
            case reqRoomAreaJoin:
                {
                	_onReceiveFn.reqRoomAreaJoin(_json.bytesToObject(bytes, ReqRoomAreaJoin.class));
                    break;
                }
            case reqRoomAreaMessage:
                {
                	_onReceiveFn.reqRoomAreaMessage(_json.bytesToObject(bytes, ReqRoomAreaMessage.class));
                    break;
                }
            case reqMessage:
                {
                	_onReceiveFn.reqMessage(_json.bytesToObject(bytes, ReqMessage.class));
                    break;
                }

             default:{

             }
        }
		 */
	}

}

class OnReceiveFn
{
	class DataClient
	{
		public String nick;
	}
	private DataClient _dataClient = new DataClient();

	private Client _client;



	public OnReceiveFn(Client client)
	{
		_client = client;
	}


	public void reqLogin(ReqLogin req)
	{
		System.out.println(req.nick);
		_dataClient.nick = req.nick;

		ProtocolObject._resLogin.key = EnumKey.resLogin;
		ProtocolObject._resLogin.result = EnumResResult.success;

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resLogin));

		_client.write(bytes);
	}

	public void reqRoomAreaJoin(ReqRoomAreaJoin req)
	{
		System.out.println(req.idRoom);

		_client.join(req.idRoom);

		ProtocolObject._resRoomAreaJoin.key = EnumKey.resRoomAreaJoin;
		ProtocolObject._resRoomAreaJoin.result = EnumResResult.success;

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resRoomAreaJoin));

		_client.write(bytes);
	}

	public void reqRoomAreaMessage(ReqRoomAreaMessage req)
	{
		System.out.println(req.idRoom);

		ProtocolObject._resMessage.key = EnumKey.resMessage;
		ProtocolObject._resMessage.result = EnumResResult.success;
		ProtocolObject._resMessage.message = req.message;

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resMessage));

		_client.roomWrite(bytes, req.idRoom);
	}

	public void reqMessage(ReqMessage req)
	{
		System.out.println(req.message);

		ProtocolObject._resMessage.key = EnumKey.resMessage;
		ProtocolObject._resMessage.result = EnumResResult.success;
		ProtocolObject._resMessage.message = "ÀÀ´ä";

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resMessage));

		_client.write(bytes);
	}

	public void resLeave(String idRoom)
	{
		ProtocolObject._resMessage.key = EnumKey.resMessage;
		ProtocolObject._resMessage.result = EnumResResult.success;
		ProtocolObject._resMessage.message = _dataClient.nick + "°¡ ³ª°¨";

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resMessage));

		_client.roomWrite0(bytes, idRoom);
	}

	public void resDisconnect(String idRoom)
	{
		ProtocolObject._resMessage.key = EnumKey.resMessage;
		ProtocolObject._resMessage.result = EnumResResult.success;
		ProtocolObject._resMessage.message = _dataClient.nick + "°¡ ²÷±è";

		ByteBuffer bytes = ByteBuffer.wrap(Client._json.objectToBytes(ProtocolObject._resMessage));

		_client.roomWrite0(bytes,idRoom);
	}
}
