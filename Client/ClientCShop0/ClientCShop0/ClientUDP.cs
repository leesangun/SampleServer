using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientCShop0
{
    public class ClientUDP
    {
        private ReceiveUdpState _receiveUdpState = new ReceiveUdpState();
        private struct ReceiveUdpState
        {
            public UdpClient udpClient;
            public IPEndPoint iPEndPoint;
        }

        private readonly string IP = "127.0.0.1";
        private readonly int PORT = 7777;


        public delegate void Ack(byte[] data);
        private Ack _ack;
        private static ClientUDP _instance;
        public static ClientUDP GetInstance(Ack ack)
        {
            if (_instance == null)
            {
                _instance = new ClientUDP(ack);
            }
            return _instance;
        }


        public ClientUDP(Ack ack)
        {
            _ack = ack;

            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            _receiveUdpState.udpClient = new UdpClient(iPEndPoint);
            _receiveUdpState.iPEndPoint = iPEndPoint;

            Console.WriteLine("listening for messages");
            _receiveUdpState.udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), _receiveUdpState);
        }
        

        public void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient udpClient = ((ReceiveUdpState)(ar.AsyncState)).udpClient;
            IPEndPoint iPEndPoint = ((ReceiveUdpState)(ar.AsyncState)).iPEndPoint;

            byte[] receiveBytes = udpClient.EndReceive(ar, ref iPEndPoint);
            _ack(receiveBytes);
          //  string receiveString = Encoding.UTF8.GetString(receiveBytes);
          //  Console.WriteLine($"Received: {receiveString} {iPEndPoint}");

            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), _receiveUdpState);
        }

        public void Send()
        {
            string msg = "안녕하세요";
            byte[] datagram = Encoding.UTF8.GetBytes(msg);
            _receiveUdpState.udpClient.Send(datagram, datagram.Length, IP, PORT);
            Console.WriteLine("[Send] 127.0.0.1:7777 로 {0} 바이트 전송", datagram.Length);
        }

        public int GetLocalPort()
        {
            return ((IPEndPoint)_receiveUdpState.udpClient.Client.LocalEndPoint).Port;
        }

        public void End()
        {
            _receiveUdpState.udpClient.Close();
        }

        private static string GetLocalIP() // IP주소를 가져오는 메소드 , IPv4 형식으로 주소를 가져오는 방법입니다.
        {
            IPHostEntry host; // 인터넷 호스트 주소 정보를 가져와 변수에 저장
            host = Dns.GetHostEntry(Dns.GetHostName());// 단순 도메인 이름 확인기능
            foreach (IPAddress ip in host.AddressList) //호스트와 연결된 ip주소 목록을 가져옵니다.
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) //만약 ip의 주소와 가져온 ip의 주소가 같다면 
                {
                    return ip.ToString(); //인터넷 주소를 표준 표기법으로 변환합니다.
                }
            }
            return "127.0.0.1";
        }

        public static void Test()
        {
            // (1) UdpClient 객체 성성
            UdpClient cli = new UdpClient();

            string msg = "안녕하세요";
            byte[] datagram = Encoding.UTF8.GetBytes(msg);

            // (2) 데이타 송신
            cli.Send(datagram, datagram.Length, "127.0.0.1", 7777);
            Console.WriteLine("[Send] 127.0.0.1:7777 로 {0} 바이트 전송", datagram.Length);

            // (3) 데이타 수신
            IPEndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytes = cli.Receive(ref epRemote);
            Console.WriteLine("[Receive] {0} 로부터 {1} 바이트 수신", epRemote.ToString(), bytes.Length);

            // (4) UdpClient 객체 닫기
            cli.Close();
        }

    }
}
