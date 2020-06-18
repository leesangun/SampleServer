using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

//https://docs.microsoft.com/ko-kr/dotnet/core/tools/

namespace ServerCShop1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            new Server();
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
        }
    }

    class Server : Socket
    {

        private readonly List<Socket> _listSocket = new List<Socket>();

        public Server() : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            base.Bind(new IPEndPoint(IPAddress.Any, 4000));
            base.Listen(0);
            BeginAccept(Accept, this);

        }

        private void Accept(IAsyncResult result)
        {
            
            _listSocket.Add(EndAccept(result));

            Console.WriteLine("Connect");

            BeginAccept(Accept, this);

            
        }

        public void SendAll(byte[] message)
        {
            foreach (Socket s in _listSocket)
            {
                s.Send(message);
            }
        }

    }
}
