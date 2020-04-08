using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCShop0
{
    abstract class BaseClient
    {
        protected readonly Socket _socket;
        private readonly byte[] _bytes = new byte[1024];


        public BaseClient(Socket socket)
        {
  
            this._socket = socket;
            if (_socket.Connected)
            {
                this._socket.BeginReceive(_bytes, 0, _bytes.Length, SocketFlags.None, Receive, this);
                var remoteAddr = (IPEndPoint)socket.RemoteEndPoint;
                Console.WriteLine($"Client:(From:{remoteAddr.Address.ToString()}:{remoteAddr.Port},Connection time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")})");
            }
        }

        private void Receive(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                try
                {
                    int size = this._socket.EndReceive(result);
                    this.OnReceive(_bytes,size);
                    this._socket.BeginReceive(_bytes, 0, _bytes.Length, SocketFlags.None, Receive, this);

                }
                catch (SocketException e)
                {
                    Console.WriteLine("disconnect");

                    this.Disconnect();

                }


            }
        }

        public void Send(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            //this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, Send, this);
            _socket.Send(data, data.Length, SocketFlags.None);
        }
        public void Send(byte[] bytes)
        {
            _socket.Send(bytes, bytes.Length, SocketFlags.None);
        }
        private void Send(IAsyncResult result)
        {
            if (_socket.Connected)
            {
                this._socket.EndSend(result);
            }
        }

        protected abstract void Disconnect();
        protected abstract void OnReceive(byte[] bytes,int size);
    }
}
