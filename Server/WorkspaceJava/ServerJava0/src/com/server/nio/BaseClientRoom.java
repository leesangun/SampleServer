package com.server.nio;

import java.nio.ByteBuffer;
import java.util.HashSet;
import java.util.Set;


public abstract class BaseClientRoom extends BaseClient {
	protected final Set<String> _listRoomId = new HashSet<String>();


    public void join(String idRoom){
        Room.join(this, idRoom);
        _listRoomId.add(idRoom);
        
    }
    public void leave(String idRoom){
        if (idRoom == null)
        {
            if (_listRoomId.size() > 0) idRoom = _listRoomId.iterator().next();
            else return;
        }
        Room.leave(this, idRoom);
        _listRoomId.remove(idRoom);
    }


    public void roomWrite(ByteBuffer buffer, String idRoom)
    {
        if (idRoom == null)
        {
            if (_listRoomId.size() > 0) Room.write(_listRoomId.iterator().next(), buffer);
        }
        else Room.write(idRoom, buffer);
    }

    public void roomWrite0(ByteBuffer buffer, String idRoom) //본인제외
    {
    	if (idRoom == null)
        {
            if (_listRoomId.size() > 0) Room.write(_listRoomId.iterator().next(), buffer);
        }
        else Room.write(idRoom, buffer, this);
    }
    
    @Override
    protected void disconnect()
    {
        for (String idRoom : _listRoomId)
        {
            Room.leave(this, idRoom);
        }
        _listRoomId.clear();

       // super.disconnect();
    }
}



