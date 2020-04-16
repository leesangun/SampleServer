package com.server.protocol;

public class Protocol {
	/*
	public enum EnumKey
    {
        reqLogin(0),
        resLogin(1),
        reqRoomAreaJoin(2),
        resRoomAreaJoin(3),
        reqRoomAreaMessage(4),
        reqMessage(10),
        resMessage(11);
        
        private final int _value; 
        private EnumKey(int value) { 
        	this._value = value; 
        } 
        public int getValue() {
        	return _value; 
        }
        
        public static EnumKey valueToEnum(int value)
        {
        	EnumKey[] e = EnumKey.values();
            for(int i = 0; i < e.length; i++)
            {
                if(e[i].getValue() == value)
                    return e[i];
            }
            return null;
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
	*/
    
    public class EnumKey
    {
        public static final int 
        reqLogin = 0,
        resLogin = 1,
        reqRoomAreaJoin = 2,
        resRoomAreaJoin = 3,
        reqRoomAreaMessage = 4,
        reqMessage = 10,
        resMessage = 11;
    }
    public class EnumResResult
    {
    	public static final int 
    	success = 0;
    }
}

