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
            
            // Send("Welcome server!\r\n>");
        }
        // 메시지가 오면 호출된다.
        private void Receive(IAsyncResult result)
        {
            // 접속이 연결되어 있으면...
            if (_socket.Connected)
            {
                // EndReceive를 호출하여 데이터 사이즈를 받는다.
                // EndReceive는 대기를 끝내는 것이다.
                try
                {
                    int size = this._socket.EndReceive(result);
                    // 데이터를 string으로 변환한다.
                    // string message = Encoding.ASCII.GetString(buffer, 0, size);
                    // Console.WriteLine(message);
                    //byte[] b = buffer;
                    //Array.Resize(ref b, size);
                    this.OnReceive(_bytes,size);
                    // ReqMessage req = (ReqMessage)LibMarshal.ByteToObject(buffer, typeof(ReqMessage));
                    // Console.WriteLine(req.message);

                    this._socket.BeginReceive(_bytes, 0, _bytes.Length, SocketFlags.None, Receive, this);

                }
                catch (SocketException e)
                {
                    Console.WriteLine("disconnect");

                    this.Disconnect();

                }


            }
        }
        // Send도 비동기 식으로 만들 수 있는데.. 굳이 send는 그럴 필요가 없습니다.
        // 메시지를 보내는 함수
        public void Send(string msg)
        {
            byte[] data = Encoding.ASCII.GetBytes(msg);
            //this.socket.BeginSend(data, 0, data.Length, SocketFlags.None, Send, this);
            // Client로 메시지 전송
            _socket.Send(data, data.Length, SocketFlags.None);
        }
        public void Send(byte[] bytes)
        {
            _socket.Send(bytes, bytes.Length, SocketFlags.None);
        }
        // Send 비동기 식임.. 현재는 미사용.
        private void Send(IAsyncResult result)
        {
            // 접속이 연결되어 있으면...
            if (_socket.Connected)
            {
                this._socket.EndSend(result);
            }
        }

        protected abstract void Disconnect();
        protected abstract void OnReceive(byte[] bytes,int size);
    }
}
