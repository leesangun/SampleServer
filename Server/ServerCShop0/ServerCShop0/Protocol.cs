using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerCShop0
{
    public enum EnumKey : ushort
    {
        req_login = 0,
        res_login = 1,
        req_room_area_join = 2,
        res_room_area_join = 3,
        req_room_area_message = 4,
        req_message = 10,
        res_message = 11,
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

    

    class Protocol
    {
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        public static ResLogin _resLogin = new ResLogin();
        public static ResRoomAreaJoin _resRoomAreaJoin = new ResRoomAreaJoin();
        public static ResMessage _resMessage = new ResMessage();

        public static ResRoomAreaList _resRoomAreaList = new ResRoomAreaList();

        public void SetData()
        {
            _resRoomAreaList.recordRoomAreas = new RecordRoomArea[3];
            for (int i = 0; i < _resRoomAreaList.recordRoomAreas.Length; i++)
            {
                _resRoomAreaList.recordRoomAreas[i] = new RecordRoomArea();
                _resRoomAreaList.recordRoomAreas[i].idRoom = "-"+(i+1);
            }

            _resRoomAreaList.recordRoomAreas[0].name = "서울";
            _resRoomAreaList.recordRoomAreas[1].name = "경기";
            _resRoomAreaList.recordRoomAreas[2].name = "부산";

            _resLogin.recordRoomAreas = _resRoomAreaList.recordRoomAreas;
        }
    }
}
