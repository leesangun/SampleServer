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
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        private ResMessage _resMessage = new ResMessage();

        public void ReqMessage(Client client,ReqMessage req)
        {
            Console.WriteLine(req.message);
            this.ResMessage(client);
        }

        private void ResMessage(Client client)
        {
            _resMessage.key = EnumKey.RES_MESSAGE;
            _resMessage.message = "응답";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(_resMessage, _jsonSerializerOptions);


            client.Send(bytes);
        }
    }

    class Client : BaseClient
    {
        private readonly List<string> _listRoomId = new List<string>();
        private readonly OnReceiveFn _onReceiveFn = new OnReceiveFn();

        public Client(Socket socket) : base(socket)
        {
            
        }
        protected override void OnReceive(byte[] bytes, int size)
        {

            byte[] bs = new byte[size];
            Array.Copy(bytes, bs, bs.Length);
            var readOnlySpan = new ReadOnlySpan<byte>(bs);
            BasePacket basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);
            

            switch (basePacket.key)
            {
                case EnumKey.REQ_MESSAGE:
                    {
                        _onReceiveFn.ReqMessage(this, JsonSerializer.Deserialize<ReqMessage>(readOnlySpan));
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

        protected override void Disconnect()
        {
            foreach (string roomId in _listRoomId)
            {
                this.Leave(roomId);
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
            if (_listRoom[idRoom] == null)
            {
                _listRoom[idRoom] = new Room();
            }
            _listRoom[idRoom]._listClient.Add(client);
        }


        public static void Leave(Client client, string idRoom)
        {
            if (_listRoom[idRoom] == null)
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
