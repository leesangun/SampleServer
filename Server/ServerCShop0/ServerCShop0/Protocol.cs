namespace Protocol
{
    public enum EnumKey : ushort
    {
        reqLogin = 0,
        resLogin = 1,
        reqRoomAreaJoin = 2,
        resRoomAreaJoin = 3,
        reqRoomAreaMessage = 4,
        reqMessage = 10,
        resMessage = 11,
    }
    public enum EnumResResult : byte
    {
        SUCCESS = 0,
    }

    public class BasePacket
    {
        public EnumKey key { get; set; }
    }
    public class BasePacketRes : BasePacket
    {
        public EnumResResult result { get; set; }
    }

    public class ReqLogin : BasePacket{
        public string nick { get; set; }
    }
    public class ResLogin : ResRoomAreaList
    {
        
    }
    public class ReqRoomAreaJoin : BasePacket
    {
        public string idRoom { get; set; }
    }
    public class ResRoomAreaJoin : BasePacketRes
    {

    }


    public class ResRoomAreaList : BasePacketRes
    {
        public RecordRoomArea[] recordRoomAreas { get; set; }
    }
    public class RecordRoomArea
    {
        public string idRoom { get; set; }
        public string name { get; set; }
    }

    public class ReqRoomAreaMessage : BasePacket
    {
        public string idRoom { get; set; }
        public string message { get; set; }
    }

    public class ReqMessage : BasePacket
    {
        public string message { get; set; }
    }
    public class ResMessage : BasePacketRes
    {
        public string message { get; set; }
    }
}
