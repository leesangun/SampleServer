using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientCShop0
{
    public partial class Form1 : Form
    {
        private readonly NetClient _netClient;
        public Form1()
        {
            InitializeComponent();

            _netClient = new NetClient(this);

            labelLog.Text = "Start";
        }

        private int _countLog,_countLogUdp;
        public void Log(string message)
        {
            if (labelLog.InvokeRequired)
            {
                labelLog.Invoke(new MethodInvoker(delegate ()
                {
                    labelLog.Text = (++_countLog) + ":" + message;
                }));
            }
            else
            {
                labelLog.Text = (++_countLog) + ":" + message;
            }
        }

        public void LogUdp(string message)
        {
            if (labelLogUdp.InvokeRequired)
            {
                labelLogUdp.Invoke(new MethodInvoker(delegate ()
                {
                    labelLogUdp.Text = (++_countLogUdp) + ":" + message;
                }));
            }
            else
            {
                labelLogUdp.Text = (++_countLogUdp) + ":" + message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _netClient.ReqMessage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _netClient.End();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            _netClient.ReqLogin("nick");
        }

        private void buttonRoomAreaJoin_Click(object sender, EventArgs e)
        {
            _netClient.ReqRoomAreaJoin("-1");
        }

        private void buttonRoomAreaMessage_Click(object sender, EventArgs e)
        {
            _netClient.ReqRoomAreaMessage("메시지");
        }

        

        private void testUdp_Click(object sender, EventArgs e)
        {
            _netClient.TestUdpSend();
            // clientUdp.Send();
           // ClientUDP.Test();
        }
    }
}
