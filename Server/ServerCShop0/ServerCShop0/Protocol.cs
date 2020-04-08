using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServerCShop0
{
    public class BasePacket
    {
        public EnumKey key { get; set; }
    }
    public class BasePacketRes : BasePacket
    {
        public byte result { get; set; }
    }

    public class ReqMessage : BasePacket
    {
        public byte sizeMessage { get; set; }
        public string message { get; set; }
        public char[] test { get; set; }
    }
    public class ResMessage : BasePacket
    {
        public byte sizeMessage { get; set; }
        public string message { get; set; }
    }

    public enum EnumKey : ushort
    {
        REQ_MESSAGE = 0,
        RES_MESSAGE = 1,
    }

    class Protocol
    {
    }
}
