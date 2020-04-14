package com.server.protocol;

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

