using Lib;
using Protocol;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerCShop0
{
    class ProtocolObject
    {
        //비동기서버이기 때문에 이방식은 테스트 필요
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
                _resRoomAreaList.recordRoomAreas[i].idRoom = "-" + (i + 1);
            }

            _resRoomAreaList.recordRoomAreas[0].name = "서울";
            _resRoomAreaList.recordRoomAreas[1].name = "경기";
            _resRoomAreaList.recordRoomAreas[2].name = "부산";

            _resLogin.recordRoomAreas = _resRoomAreaList.recordRoomAreas;
        }
    }

    class Server : Socket
    {
        private readonly List<Client> _listClient = new List<Client>();
        private readonly ObjectPool<Client> _poolClient = new ObjectPool<Client>(() => new Client());

        public Server() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            base.Bind(new IPEndPoint(IPAddress.Any, 4000));
            base.Listen(0);
            BeginAccept(Accept, this);

            ProtocolObject protocol = new ProtocolObject();
            protocol.SetData();
        }

        private void Accept(IAsyncResult result)
        {
            // var client = new Client(EndAccept(result));

            var client = _poolClient.GetObject();
            client.Init(EndAccept(result), () => {
                _listClient.Remove(client);
                _poolClient.PutObject(client);
            });
            
            _listClient.Add(client);
            BeginAccept(Accept, this);
        }

        public void SendAll(byte[] message)
        {
            foreach (Client c in _listClient)
            {
                c.Send(message);
            }
        }
    }
    static class Program
    {
        public static Server _server;
        public static void ThreadServer()
        {
            _server = new Server();

            //RedisRedis.TestReset();
            //RedisRedis.Test();
            //LibMySql.Test();
            LibLog.Test();
            //ConnRedis.Test();
        }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread threadServer = new Thread(new ThreadStart(ThreadServer));
            threadServer.Start();
            threadServer.Join();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
