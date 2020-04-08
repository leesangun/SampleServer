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
        private NetClient _netClient;
        public Form1()
        {
            InitializeComponent();

            _netClient = new NetClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _netClient.ReqMessage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _netClient.End();
        }
    }
}
