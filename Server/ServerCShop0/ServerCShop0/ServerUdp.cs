using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCShop0
{
    class ServerUdp : BaseServerUdp
    {
        protected override void Receive(UdpReceiveResult udpReceiveResult)
        {
            Console.WriteLine("[Receive] {0} 로부터 {1} 바이트 수신", udpReceiveResult.RemoteEndPoint, udpReceiveResult.Buffer.Length);
            Send(udpReceiveResult.RemoteEndPoint, udpReceiveResult.Buffer);
        }

        public override void Send(IPEndPoint remoteEP, byte[] bytes)
        {
            base.Send(remoteEP,bytes);
            Console.WriteLine("[Send] {0}로 {1} 바이트 전송", remoteEP, bytes.Length);
        }



        public static void Test()
        {
            /*
            // (1) UdpClient 객체 성성. 포트 7777 에서 Listening
            UdpClient srv = new UdpClient(7777);


            // 클라이언트 IP를 담을 변수
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
 
           
            while (true)
            {
                // (2) 데이타 수신
                byte[] dgram = srv.Receive(ref remoteEP);
                Console.WriteLine("[Receive] {0} 로부터 {1} 바이트 수신", remoteEP.ToString(), dgram.Length);

                // (3) 데이타 송신
                srv.Send(dgram, dgram.Length, remoteEP);
                Console.WriteLine("[Send] {0} 로 {1} 바이트 송신", remoteEP.ToString(), dgram.Length);
            }
            */

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Task.Run(async () =>
            {
                using (var udpClient = new UdpClient(7777))
                {
                    
                    while (true)
                    {
                        var receivedResults = await udpClient.ReceiveAsync();
                        remoteEP = receivedResults.RemoteEndPoint;
                        Console.WriteLine("[Receive] {0} 로부터 {1} 바이트 수신", remoteEP.ToString(), receivedResults.Buffer.Length);

                        byte[] dgram = receivedResults.Buffer;
                        await udpClient.SendAsync(dgram, dgram.Length, remoteEP);
                        Console.WriteLine("[Send] {0} 로 {1} 바이트 송신", remoteEP.ToString(), dgram.Length);
                    }
                }
            });
            //https://ichiroku11.hatenablog.jp/entry/2017/02/12/224822

            //srv.Close();
        }
    }


}
