using Lib;
using Protocol;
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

            ProtocolObject._resLogin.key = EnumKey.resLogin;
            ProtocolObject._resLogin.result = EnumResResult.success;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resLogin, Config._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ReqRoomAreaJoin(ReqRoomAreaJoin req)
        {
            Console.WriteLine(req.idRoom);

            _client.Join(req.idRoom);

            ProtocolObject._resRoomAreaJoin.key = EnumKey.resRoomAreaJoin;
            ProtocolObject._resRoomAreaJoin.result = EnumResResult.success;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resRoomAreaJoin, Config._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ReqRoomAreaMessage(ReqRoomAreaMessage req)
        {
            Console.WriteLine(req.idRoom);

            ProtocolObject._resMessage.key = EnumKey.resMessage;
            ProtocolObject._resMessage.result = EnumResResult.success;
            ProtocolObject._resMessage.message = req.message;

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resMessage, Config._jsonSerializerOptions);

            _client.RoomSend(bytes, req.idRoom);
        }

        public void ReqMessage(ReqMessage req)
        {
            Console.WriteLine(req.message);


            ProtocolObject._resMessage.key = EnumKey.resMessage;
            ProtocolObject._resMessage.result = EnumResResult.success;
            ProtocolObject._resMessage.message = "응답";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resMessage, Config._jsonSerializerOptions);


            _client.Send(bytes);
        }

        public void ResLeave(string idRoom)
        {
            ProtocolObject._resMessage.key = EnumKey.resMessage;
            ProtocolObject._resMessage.result = EnumResResult.success;
            ProtocolObject._resMessage.message = _dataClient.nick + "가 나감";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resMessage, Config._jsonSerializerOptions);


            _client.RoomSend0(bytes, idRoom);
        }

        public void ResDisconnect(string idRoom)
        {
            ProtocolObject._resMessage.key = EnumKey.resMessage;
            ProtocolObject._resMessage.result = EnumResResult.success;
            ProtocolObject._resMessage.message = _dataClient.nick + "가 끊김";

            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(ProtocolObject._resMessage, Config._jsonSerializerOptions);


            _client.RoomSend0(bytes,idRoom);
        }

    }

    class Client : BaseClientRoom
    {
        
        private readonly OnReceiveFn _onReceiveFn;

        public Client() {
            _onReceiveFn = new OnReceiveFn(this);
        }

        public Client(Socket socket) : base(socket)
        {
            _onReceiveFn = new OnReceiveFn(this);
        }
        protected override void OnReceive(byte[] bytes, int size)
        {
         //   Console.WriteLine(System.Text.Encoding.UTF8.GetString(bytes));


            var readOnlySpan = new ReadOnlySpan<byte>(bytes, 0, size);
            BasePacket basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);

            switch (basePacket.key)
            {
                case EnumKey.reqLogin:
                    {
                        _onReceiveFn.ReqLogin(JsonSerializer.Deserialize<ReqLogin>(readOnlySpan));
                        break;
                    }
                case EnumKey.reqRoomAreaJoin:
                    {
                        _onReceiveFn.ReqRoomAreaJoin(JsonSerializer.Deserialize<ReqRoomAreaJoin>(readOnlySpan));
                        break;
                    }
                case EnumKey.reqRoomAreaMessage:
                    {
                        _onReceiveFn.ReqRoomAreaMessage(JsonSerializer.Deserialize<ReqRoomAreaMessage>(readOnlySpan));
                        break;
                    }
                case EnumKey.reqMessage:
                    {
                        _onReceiveFn.ReqMessage(JsonSerializer.Deserialize<ReqMessage>(readOnlySpan));
                        break;
                    }
            }
        }


        public override void Leave(string idRoom = null)
        {
            base.Leave(idRoom);
            _onReceiveFn.ResLeave(idRoom);
        }

        protected override void Disconnect()
        {
            if(_listRoomId.Count > 0)
            {
                _onReceiveFn.ResDisconnect(_listRoomId[_listRoomId.Count - 1]);
            }

            base.Disconnect();
        }
    }

    
}
