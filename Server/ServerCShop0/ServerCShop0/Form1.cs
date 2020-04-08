using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerCShop0
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResMessage r = new ResMessage
            {
                key = EnumKey.RES_MESSAGE,
                message = "전체응답"
            };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(r, options);

            Program._server.SendAll(bytes);
        }
    }
}
