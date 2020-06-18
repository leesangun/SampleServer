using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;


using System.Windows.Forms;

//nuget
//SuperSocket
//SuperSocket.SocketEngine  or SuperSocket.SocketEngine-unofficial

namespace ServerSuperSocket1x
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MyAppServer server = new MyAppServer();

            server.Setup(new RootConfig(), new ServerConfig()
            {
                Port = 4000,
                Ip = "Any",
            });

            server.Start();
            /*
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.WriteLine();
                continue;
            }
            */

            /*
            //AppServer 연결 포트 설정하기
            if (!server.Setup(4000)) //성공 시 true, 실패 시 false 반환
            {
                Console.WriteLine();
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //AppServer 시작하기
            if (!server.Start()) //성공 시 true, 실패 시 false 반환
            {
                Console.WriteLine("시작하지 못했습니다.");
                Console.ReadKey();
                return;
            }
            */
        }
    }

    /*
    //ReceiveFilter
    public class MyReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public MyReceiveFilter()
        : base(6) // 해더의 길이
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 4] + (int)header[offset + 5] * 256 - 6;
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            var byteTmp = bodyBuffer.CloneRange(offset, length);

            return new BinaryRequestInfo(BitConverter.ToUInt32(header.Array, 0).ToString(), MyReceiveFilter.Combine(header.Array, byteTmp));
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }
    }
    */
    class MyReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo>
    {
        public MyReceiveFilter()
            : base(6)
        {
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 4] * 256 + (int)header[offset + 5];
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4), bodyBuffer.CloneRange(offset, length));
        }

        /// <summary>
        /// 메시지가오면 여기에 걸린다.
        /// </summary>
        /// <param name="readBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="toBeCopied"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public override BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            //데이터가 있는지?
            if (null == readBuffer)
            {
                return NullRequestInfo;
            }

            //데이터가 있다.

            //리턴할 객체
            BinaryRequestInfo briReturn;
            //자른 데이터
            byte[] byteCutData = new byte[length];

            //데이터를 오프셋을 기준으로 자른다.
            Buffer.BlockCopy(readBuffer, offset, byteCutData, 0, length);

            //이 코드는 유니코드를 받기위한 것이므로 유니코로 테스트를 한다.
            briReturn = new BinaryRequestInfo(Encoding.Unicode.GetString(byteCutData)
                                                , byteCutData);

            return briReturn;
        }
    }

    //AppServer
    public class MyAppServer : AppServer<MyAppSession, BinaryRequestInfo>
    {
        public MyAppServer() : base(new DefaultReceiveFilterFactory<MyReceiveFilter, BinaryRequestInfo>())
        {
            this.NewSessionConnected += new SessionHandler<MyAppSession>(MyServer_NewSessionConnected);
            this.SessionClosed += new SessionHandler<MyAppSession, SuperSocket.SocketBase.CloseReason>(MyServer_SessionClosed);
            this.NewRequestReceived += new RequestHandler<MyAppSession, BinaryRequestInfo>(MyServer_NewRequestReceived);
        }

        private void MyServer_NewRequestReceived(MyAppSession session, BinaryRequestInfo requestinfo)
        {
            Console.WriteLine("MyServer_NewRequestReceived");

            //session.Send(requestinfo.Body, 0, requestinfo.Body.Length);
            session.Send("Test");
        }

        private void MyServer_SessionClosed(MyAppSession session, SuperSocket.SocketBase.CloseReason value)
        {
            Console.WriteLine("MyServer_UserClosed");
        }

        private void MyServer_NewSessionConnected(MyAppSession session)
        {
            Console.WriteLine("MyServer_NewUserConnected");
        }
    }

    //AppSession
    public class MyAppSession : AppSession<MyAppSession, BinaryRequestInfo>
    {

    }
}
