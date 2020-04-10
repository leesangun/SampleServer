﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;
//Install-Package System.Text.Json -Version 4.7.1
namespace ClientCShop0
{
    class NetClient
    {
        private readonly ClientTCP _clientTCP;

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
                    BasePacket basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);
                    switch (basePacket.key)
                    {
                        case EnumKey.res_login:
                            {
                                this.ResLogin(JsonSerializer.Deserialize<ResLogin>(readOnlySpan));

                                break;
                            }
                        case EnumKey.res_room_area_join:
                            {
                                this.ResRoomAreaJoin(JsonSerializer.Deserialize<ResRoomAreaJoin>(readOnlySpan));

                                break;
                            }
                        case EnumKey.res_message:
                            {
                                this.ResMessage(JsonSerializer.Deserialize<ResMessage>(readOnlySpan));

                                break;
                            }
                    }

                }
            );
        }

        public void ReqLogin(string nick)
        {
            ReqLogin req = new ReqLogin();
            req.key = EnumKey.req_login;
            req.nick = nick;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);
        }
        public void ResLogin(ResLogin res)
        {
            switch (res.result)
            {
                case EnumResResult.SUCCESS:
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
            req.key = EnumKey.req_room_area_join;
            req.idRoom = idRoom;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);

            _dataClient.idRoomArea = idRoom;
        }
        public void ResRoomAreaJoin(ResRoomAreaJoin res)
        {
            switch (res.result)
            {
                case EnumResResult.SUCCESS:
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
            req.key = EnumKey.req_room_area_message;
            req.idRoom = _dataClient.idRoomArea;
            req.message = message;
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);
        }


        public void ReqMessage()
        {
            ReqMessage req = new ReqMessage();
            req.key = EnumKey.req_message;
            req.message = "한글";


            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, _jsonSerializerOptions);
            _clientTCP.write(bytes);

        }

        public void ResMessage(ResMessage res)
        {
            switch (res.result)
            {
                case EnumResResult.SUCCESS:
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
