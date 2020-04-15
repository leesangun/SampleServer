package com.server.nio;

import java.nio.ByteBuffer;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.Set;

import org.apache.commons.pool.impl.GenericObjectPool;

import com.server.libs.ObjectPoolFactory;

public class Room
{
    private static final Hashtable<String, Room> _listRoom = new Hashtable<String, Room>();
    public static final GenericObjectPool<Room> _poolRoom = new GenericObjectPool<Room>(new ObjectPoolFactory<Room>(Room.class));
	
    private final Set<BaseClientRoom> _listClient = new HashSet<BaseClientRoom>();


    public static void join(BaseClientRoom client, String idRoom)
    {
        // if (_listRoom[idRoom] == null)
        if (!_listRoom.containsKey(idRoom))
        {
            try {
				_listRoom.put(idRoom, _poolRoom.borrowObject());
			} catch (Exception e) {
				e.printStackTrace();
			}
        }
        _listRoom.get(idRoom)._listClient.add(client);
    }


    public static void leave(BaseClientRoom client, String idRoom)
    {
        if (!_listRoom.containsKey(idRoom))
        {
            return;
        }
        _listRoom.get(idRoom)._listClient.remove(client);
        if (Room.countSocket(idRoom) == 0)
        {
            try {
				_poolRoom.returnObject(_listRoom.get(idRoom));
			} catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
            _listRoom.remove(idRoom);
        }
    }

    public static int countSocket(String idRoom)
    {
        return  _listRoom.get(idRoom)._listClient.size();
    }

    public static void write(String idRoom, ByteBuffer buffer)
    {
    	Set<BaseClientRoom> listClient = _listRoom.get(idRoom)._listClient;
        for (BaseClient c : listClient)
        {
            c.write(buffer);
        }
    }
    public static void write(String idRoom, ByteBuffer buffer, BaseClientRoom client)
    {
    	Set<BaseClientRoom> listClient = _listRoom.get(idRoom)._listClient;
    	for (BaseClient c : listClient)
        {
            if (c.equals(client)) continue;
            c.write(buffer);
        }
    }
}
