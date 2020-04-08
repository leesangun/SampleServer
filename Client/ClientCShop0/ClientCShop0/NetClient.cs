using System;
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

        public NetClient()
        {
            _clientTCP = ClientTCP.getInstance(
                (byte[] bytes) =>
                {
                    var readOnlySpan = new ReadOnlySpan<byte>(bytes);
                    BasePacket basePacket = JsonSerializer.Deserialize<BasePacket>(readOnlySpan);
                    switch (basePacket.key)
                    {
                        case EnumKey.RES_MESSAGE:
                            {
                                this.ResMessage(JsonSerializer.Deserialize<ResMessage>(readOnlySpan));

                                break;
                            }
                    }

                }
            );
        }

        public void ReqMessage()
        {
            ReqMessage req = new ReqMessage();
            req.key = EnumKey.REQ_MESSAGE;
            req.message = "한글";
            req.test = new char[2]{'a','b' };


            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(req, options);
            _clientTCP.write(bytes);

        }

        public void ResMessage(ResMessage r)
        {
            Console.WriteLine(r.message);
        }


        public void End()
        {
            _clientTCP.end();
        }

    }
}
