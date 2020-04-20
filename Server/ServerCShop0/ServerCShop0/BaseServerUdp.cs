using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCShop0
{
    abstract class BaseServerUdp
    {

        protected UdpClient _udpClient = new UdpClient(Lib.Config.PORT_UDP);
        public BaseServerUdp()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var udpReceiveResult = await _udpClient.ReceiveAsync();
                    // Console.WriteLine("[Receive] {0} 로부터 {1} 바이트 수신", receivedResults.RemoteEndPoint, receivedResults.Buffer.Length);

                    // Send(udpReceiveResult.RemoteEndPoint);
                     this.Receive(udpReceiveResult);
                    
                }

            });
        }

        protected abstract void Receive(UdpReceiveResult udpReceiveResult);
        public virtual void Send(IPEndPoint remoteEP, byte[] bytes)
        {
            _udpClient.Send(bytes, bytes.Length, remoteEP);
        }
    }

}
