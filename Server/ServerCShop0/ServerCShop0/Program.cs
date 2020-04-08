using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerCShop0
{

    class Server : Socket
    {
        private readonly List<Client> _listClient = new List<Client>();

        public Server() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            // 포트는 10000을 Listen한다.
            base.Bind(new IPEndPoint(IPAddress.Any, 4000));
            base.Listen(0);
            // 비동기 소켓으로 Accept 클래스로 대기한다.
            BeginAccept(Accept, this);
        }
        // 클라이언트가 접속하면 함수가 호출된다.
        private void Accept(IAsyncResult result)
        {
            // EndAccept로 접속 Client Socket을 받는다. EndAccept는 대기를 끝나는 것이다.
            // Client 클래스를 생성한다.
            var client = new Client(EndAccept(result));
            _listClient.Add(client);
            // 비동기 소켓으로 Accept 클래스로 대기한다.
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
        }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread t = new Thread(new ThreadStart(ThreadServer));
            t.Start();
            t.Join();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
