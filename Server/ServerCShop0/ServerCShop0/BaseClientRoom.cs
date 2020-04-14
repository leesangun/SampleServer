using Lib;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace ServerCShop0
{
    abstract class BaseClientRoom : BaseClient
    {
        protected readonly List<string> _listRoomId = new List<string>();

        public BaseClientRoom() { }

        public BaseClientRoom(Socket socket) : base(socket)
        {
        }

        public virtual void Join(string idRoom)
        {
            Room.Join(this, idRoom);
            if (!_listRoomId.Contains(idRoom))
            {
                _listRoomId.Add(idRoom);
            }
            
        }
        public virtual void Leave(string idRoom = null)
        {
            if (idRoom == null)
            {
                if (_listRoomId.Count > 0) idRoom = _listRoomId[0];
                else return;
            }
            Room.Leave(this, idRoom);
            _listRoomId.Remove(idRoom);
        }


        public void RoomSend(byte[] bytes, string idRoom = null)
        {
            if (idRoom == null)
            {
                if (_listRoomId.Count > 0) Room.Send(_listRoomId[0], bytes);
            }
            else Room.Send(idRoom, bytes);
        }

        public void RoomSend0(byte[] bytes, string idRoom = null) //본인제외
        {
            if (idRoom == null)
            {
                if (_listRoomId.Count > 0) Room.Send(_listRoomId[0], bytes);
            }
            else Room.Send(idRoom, bytes, this);
        }
        protected override void Disconnect()
        {
            foreach (string idRoom in _listRoomId)
            {
                Room.Leave(this, idRoom);
            }
            _listRoomId.Clear();

            base.Disconnect();
        }

    }
    class Room
    {
        private static readonly Dictionary<string, Room> _listRoom = new Dictionary<string, Room>();
        private static readonly ObjectPool<Room> _poolRoom = new ObjectPool<Room>(() => new Room());

        private readonly List<BaseClientRoom> _listClient = new List<BaseClientRoom>();


        public static void Join(BaseClientRoom client, string idRoom)
        {
            // if (_listRoom[idRoom] == null)
            if (!_listRoom.ContainsKey(idRoom))
            {
                //_listRoom[idRoom] = new Room();
                _listRoom[idRoom] = _poolRoom.GetObject();
            }
            if (!_listRoom[idRoom]._listClient.Contains(client))
            {
                _listRoom[idRoom]._listClient.Add(client);
            }
        }


        public static void Leave(BaseClientRoom client, string idRoom)
        {
            // if (_listRoom[idRoom] == null)
            if (!_listRoom.ContainsKey(idRoom))
            {
                return;
            }
            _listRoom[idRoom]._listClient.Remove(client);
            if (Room.CountSocket(idRoom) == 0)
            {
                _poolRoom.PutObject(_listRoom[idRoom]);
                _listRoom.Remove(idRoom);
            }
        }

        public static int CountSocket(string idRoom)
        {
            return _listRoom[idRoom]._listClient.Count();
        }

        public static void Send(string idRoom, byte[] bytes)
        {
            foreach (BaseClientRoom c in _listRoom[idRoom]._listClient)
            {
                c.Send(bytes);
            }
        }
        public static void Send(string idRoom, byte[] bytes, BaseClientRoom client)
        {
            foreach (BaseClientRoom c in _listRoom[idRoom]._listClient)
            {
                if (c == client) continue;
                c.Send(bytes);
            }
        }
    }
}
