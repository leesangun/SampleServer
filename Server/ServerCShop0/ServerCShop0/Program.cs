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
            base.Bind(new IPEndPoint(IPAddress.Any, 4000));
            base.Listen(0);
            BeginAccept(Accept, this);

            Protocol protocol = new Protocol();
            protocol.SetData();
        }

        private void Accept(IAsyncResult result)
        {
            var client = new Client(EndAccept(result));
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
