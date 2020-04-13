package com.server;

public class Protocol {
	public enum EnumKey
    {
        reqLogin(0),
        resLogin(1),
        reqRoomAreaJoin(2),
        resRoomAreaJoin(3),
        reqRoomAreaMessage(4),
        reqMessage(10),
        resMessage(11);
        
        final private int name; 
        private EnumKey(int name) { 
        	this.name = name; 
        } 
        public int getName() {
        	return name; 
        }
    }
	
	//for(Type type : Type.values()){ System.out.println(type.getName()); }

	
    public enum EnumResResult
    {
        SUCCESS(0);
        final private int name; 
        private EnumResResult(int name) { 
        	this.name = name; 
        } 
        public int getName() {
        	return name; 
        }
    }

    
}

class BasePacket
{
    public Protocol.EnumKey key;
}
class BasePacketRes extends BasePacket
{
    public Protocol.EnumResResult result;
}

 class ReqLogin extends BasePacket
{
    public String nick;
}
 class ResLogin extends ResRoomAreaList
{

}
 class ReqRoomAreaJoin extends BasePacket
{
    public String idRoom;
}
 class ResRoomAreaJoin extends BasePacketRes
{

}


 class ResRoomAreaList extends BasePacketRes
{
    public RecordRoomArea[] recordRoomAreas;
}
 class RecordRoomArea
{
    public String idRoom;
    public String name;
}

 class ReqRoomAreaMessage extends BasePacket
{
    public String idRoom;
    public String message;
}

 class ReqMessage extends BasePacket
{
    public String message;
}
 class ResMessage extends BasePacketRes
{
    public String message;
}