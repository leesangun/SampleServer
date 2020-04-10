using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using System.Text.Json;
//Install-Package System.Text.Json -Version 4.7.1

namespace ServerCShop0
{
    class OnReceiveFn
    {
        struct DataClient
        {
            public string nick;
        }
        private DataClient _dataClient = new DataClient();

        private Client _client;
        public OnReceiveFn(Client client)
        {
            _client = client;
        }
        

        public void ReqLogin(ReqLogin req)
        {
            Console.WriteLine(req.nick);
            _dataClient.nick = req.nick;

            Protocol._resLogin.key = EnumKey.res_login;
            Protocol._resLogin.result = EnumResResult.SUCCESS;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resLogin, Protocol._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ReqRoomAreaJoin(ReqRoomAreaJoin req)
        {
            Console.WriteLine(req.idRoom);

            _client.Join(req.idRoom);

            Protocol._resRoomAreaJoin.key = EnumKey.res_room_area_join;
            Protocol._resRoomAreaJoin.result = EnumResResult.SUCCESS;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resRoomAreaJoin, Protocol._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ReqRoomAreaMessage(ReqRoomAreaMessage req)
        {
            Console.WriteLine(req.idRoom);

            Protocol._resMessage.key = EnumKey.res_message;
            Protocol._resMessage.result = EnumResResult.SUCCESS;
            Protocol._resMessage.message = req.message;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resMessage, Protocol._jsonSerializerOptions);

            _client.RoomSend(bytes, req.idRoom);
        }

        public void ReqMessage(ReqMessage req)
        {
            Console.WriteLine(req.message);


            Protocol._resMessage.key = EnumKey.res_message;
            Protocol._resMessage.result = EnumResResult.SUCCESS;
            Protocol._resMessage.message = "응답";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resMessage, Protocol._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ResLeave(string idRoom)
        {
            Protocol._resMessage.key = EnumKey.res_message;
            Protocol._resMessage.result = EnumResResult.SUCCESS;
            Protocol._resMessage.message = _dataClient.nick + "가 나감";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resMessage, Protocol._jsonSerializerOptions);


            _client.RoomSend0(bytes, idRoom);
        }

        public void ResDisconnect(string idRoom)
        {
            Protocol._resMessage.key = EnumKey.res_message;
            Protocol._resMessage.result = EnumResResult.SUCCESS;
            Protocol._resMessage.message = _dataClient.nick + "가 끊김";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(Protocol._resMessage, Protocol._jsonSerializerOptions);


            _client.RoomSend0(bytes,idRoom);
        }

    }

    class Client : BaseClient
    {
        private readonly List<string> _listRoomId = new List<string>();
        private readonly OnReceiveFn _onReceiveFn;

        public Client(Socket socket) : base(socket)
        {
            _onReceiveFn = new OnReceiveFn(this);
        }
        protected override void OnReceive(byte[] bytes, int size)
        {

            byte[] bs = new byte[size];
            Array.Copy(bytes, bs, bs.Length);
            var readOnlySpan = new ReadOnlySpan<byte>(bs);
            BasePacket basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);
            

            switch (basePacket.key)
            {
                case EnumKey.req_login:
                    {
                        _onReceiveFn.ReqLogin(JsonSerializer.Deserialize<ReqLogin>(readOnlySpan));
                        break;
                    }
                case EnumKey.req_room_area_join:
                    {
                        _onReceiveFn.ReqRoomAreaJoin(JsonSerializer.Deserialize<ReqRoomAreaJoin>(readOnlySpan));
                        break;
                    }
                case EnumKey.req_room_area_message:
                    {
                        _onReceiveFn.ReqRoomAreaMessage(JsonSerializer.Deserialize<ReqRoomAreaMessage>(readOnlySpan));
                        break;
                    }
                case EnumKey.req_message:
                    {
                        _onReceiveFn.ReqMessage(JsonSerializer.Deserialize<ReqMessage>(readOnlySpan));
                        break;
                    }
            }
        }

  
        public void Join(string idRoom)
        {
            Room.Join(this, idRoom);
            _listRoomId.Add(idRoom);
        }
        public void Leave(string idRoom = null)
        {
            this.Leave0(idRoom);
            if (idRoom == null)
            {
                if (_listRoomId.Count > 0) idRoom = _listRoomId[0];
                else return;
            }

            _onReceiveFn.ResLeave(idRoom);
        }

        private void Leave0(string idRoom = null)
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
            _onReceiveFn.ResDisconnect(_listRoomId[_listRoomId.Count-1]);
            string[] listRoomId = _listRoomId.ToArray();
            foreach (string roomId in listRoomId)
            {
                this.Leave0(roomId);
            }

            if(_socket != null)
            {
                // Release the socket.
                _socket.Shutdown(SocketShutdown.Both);

                _socket.Disconnect(false);
                if (_socket.Connected)
                    Console.WriteLine("We're still connnected");
                else
                    Console.WriteLine("We're disconnected");
            }
        }
    }

    class Room
    {
        private static readonly Dictionary<string, Room> _listRoom = new Dictionary<string, Room>();
        private readonly List<Client> _listClient = new List<Client>();

        public static void Join(Client client, string idRoom)
        {
           // if (_listRoom[idRoom] == null)
            if(!_listRoom.ContainsKey(idRoom))
            {
                _listRoom[idRoom] = new Room();
            }
            _listRoom[idRoom]._listClient.Add(client);
        }


        public static void Leave(Client client, string idRoom)
        {
            // if (_listRoom[idRoom] == null)
            if (!_listRoom.ContainsKey(idRoom))
            {
                return;
            }
            _listRoom[idRoom]._listClient.Remove(client);
            if (Room.CountSocket(idRoom) == 0)
            {
                _listRoom.Remove(idRoom);
            }
        }

        public static int CountSocket(string idRoom)
        {
            return _listRoom[idRoom]._listClient.Count();
        }

        public static void Send(string idRoom, byte[] bytes)
        {
            foreach (Client c in _listRoom[idRoom]._listClient)
            {
                c.Send(bytes);
            }
        }
        public static void Send(string idRoom, byte[] bytes, Client client)
        {
            foreach (Client c in _listRoom[idRoom]._listClient)
            {
                if (c == client) continue;
                c.Send(bytes);
            }
        }
    }
}
