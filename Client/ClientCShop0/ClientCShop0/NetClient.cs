using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
//Install-Package System.Text.Json -Version 4.7.1

using Protocol;

namespace ClientCShop0
{
    class NetClient
    {
        private readonly ClientTCP _clientTCP;
        private readonly ClientUDP _clientUdp;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly Form1 _form1;

        struct DataClient
        {
            public string idRoomArea;
        }
        private DataClient _dataClient = new DataClient();

        public NetClient(Form1 form1)
        {
            _form1 = form1;

            _clientTCP = ClientTCP.getInstance(
                (byte[] bytes) =>
                {
                    var readOnlySpan = new ReadOnlySpan<byte>(bytes);
                    BasePacket basePacket;
                    try
                    {
                        basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);
                    }
                    catch(JsonException e)
                    {
                        Console.WriteLine(Encoding.UTF8.GetString(bytes));
                        return;
                    }
                    
                    switch (basePacket.key)
                    {
                        case EnumKey.resLogin:
                            {
                                this.ResLogin(JsonSerializer.Deserialize<ResLogin>(readOnlySpan));

                                break;
                            }
                        case EnumKey.resRoomAreaJoin:
                            {
                                this.ResRoomAreaJoin(JsonSerializer.Deserialize<ResRoomAreaJoin>(readOnlySpan));

                                break;
                            }
                        case EnumKey.resMessage:
                            {
                                this.ResMessage(JsonSerializer.Deserialize<ResMessage>(readOnlySpan));

                                break;
                            }
                    }

                }
            );

            _clientUdp = ClientUDP.GetInstance(
                (byte[] bytes) => {
                    string receiveString = Encoding.UTF8.GetString(bytes);
                    Console.WriteLine(receiveString);
                    _form1.LogUdp(receiveString);
                }
            );
        }

        public void TestUdpSend()
        {
            _clientUdp.Send();
        }

        public void ReqLogin(string nick)
        {
            ReqLogin req = new ReqLogin();
            req.udpClientPort = _clientUdp.GetLocalPort();
            req.key = EnumKey.reqLogin;
            req.nick = nick;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);
        }
        public void ResLogin(ResLogin res)
        {
            switch (res.result)
            {
                case EnumResResult.success:
                    {
                        foreach(RecordRoomArea record in res.recordRoomAreas)
                        {
                            _form1.Log(record.name);
                            Console.WriteLine(record.name);
                        }
                        break;
                    }
            }
        }
        public void ReqRoomAreaJoin(string idRoom)
        {
            ReqRoomAreaJoin req = new ReqRoomAreaJoin();
            req.key = EnumKey.reqRoomAreaJoin;
            req.idRoom = idRoom;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);

            _dataClient.idRoomArea = idRoom;
        }
        public void ResRoomAreaJoin(ResRoomAreaJoin res)
        {
            switch (res.result)
            {
                case EnumResResult.success:
                    {
                        _form1.Log("ResRoomAreaJoin");
                        break;
                    }
            }
        }

        public void ReqRoomAreaMessage(string message)
        {
            if(_dataClient.idRoomArea == null)
            {
                return;
            }

            ReqRoomAreaMessage req = new ReqRoomAreaMessage();
            req.key = EnumKey.reqRoomAreaMessage;
            req.idRoom = _dataClient.idRoomArea;
            req.message = message;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);
        }


        public void ReqMessage()
        {
            ReqMessage req = new ReqMessage();
            req.key = EnumKey.reqMessage;
            req.message = "한글";


            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);

        }

        public void ResMessage(ResMessage res)
        {
            switch (res.result)
            {
                case EnumResResult.success:
                    {
                        _form1.Log(res.message);
                        Console.WriteLine(res.message);
                        break;
                    }
            }
            
        }


        public void End()
        {
            _clientTCP.end();
        }

    }
}
