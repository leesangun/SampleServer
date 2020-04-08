using System;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace ClientCShop0
{
    
    public class ClientTCP
    {

        private Socket _socket;

        public readonly string SERVER_IP = "127.0.0.1";
        public readonly int SERVER_PORT = 4000;

        private byte[] _byteReceive = new byte[1024];    // Receive data by this array to save.

        public delegate void Ack(byte[] data);
        private Ack _ack;

        private static ClientTCP _clientTCP;
        public static ClientTCP getInstance(Ack ack)
        {
            if (_clientTCP == null)
            {
                _clientTCP = new ClientTCP();
                _clientTCP.start(ack);
            }
            return _clientTCP;
        }


        private void start(Ack ack)
        {

            _ack = ack;

            //=======================================================
            // Socket create.
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);

            //=======================================================
            // Socket connect.
            try
            {
                IPAddress ipAddr = System.Net.IPAddress.Parse(SERVER_IP);
                IPEndPoint ipEndPoint = new System.Net.IPEndPoint(ipAddr, SERVER_PORT);
                _socket.Connect(ipEndPoint);
            }
            catch (SocketException SCE)
            {
                Console.WriteLine("Socket connect error! : " + SCE.ToString());
                return;
            }

            this.beginReceive();

        }

        public void end()
        {
            _socket.Close();
            _socket = null;
        }

        public void write(string message)
        {
            try
            {
                //int SenddataLength = Encoding.Default.GetByteCount(message);
                //byte[] bytes = Encoding.Default.GetBytes(message);
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                _socket.Send(bytes, bytes.Length, 0);
            }
            catch (SocketException err)
            {
                Console.WriteLine("Socket send or receive error! : " + err.ToString());
            }
        }

        public void write(byte[] bytes)
        {
            try
            {
                _socket.Send(bytes, bytes.Length, 0);
            }
            catch (SocketException err)
            {
                Console.WriteLine("Socket send or receive error! : " + err.ToString());
            }
        }

        /*
        public void read()
        {
            try
            {
                _socket.Receive(Receivebyte);
                ReceiveString = Encoding.Default.GetString(Receivebyte);
                Debug.Log("Receive Data : " + ReceiveString + "(" + Encoding.Default.GetByteCount(ReceiveString.ToString()) + ")");
            }
            catch (SocketException err)
            {
                Debug.Log("Socket send or receive error! : " + err.ToString());
            }
        }
        */

        private int _recieved;
        private void ReceiveCallback(IAsyncResult AR)
        {
            if (_socket == null) return;
            _recieved = _socket.EndReceive(AR);

            if (_recieved <= 0) return;

            byte[] recData = new byte[_recieved];
            Buffer.BlockCopy(_byteReceive, 0, recData, 0, _recieved);

            //this._interfaceClient.read(recData);
            //Debug.Log(Encoding.Default.GetString(recData));
            //_ack(Encoding.UTF8.GetString(recData));
            _ack(recData);
            _recieved = 0;

            this.beginReceive();
        }

        private void beginReceive()
        {
            _socket.BeginReceive(_byteReceive, 0, _byteReceive.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }

        public void update()
        {


        }
    }
}
