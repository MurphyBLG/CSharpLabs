using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Lab5
{
    public partial class MethodsCheckPage : Form
    {
        public string Token { get; set; }
        public MethodsCheckPage()
        {
            InitializeComponent();
        }

        private void Logs_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string token = Token;
            var client = new WebClient { Encoding = Encoding.UTF8 };
            string json = client.DownloadString($"https://api.vk.com/method/{MethodName.Text}?{MethodParam.Text}&access_token={token}&v=5.126");

            JObject jsonObject = JObject.Parse(json);
            Logs.Text = jsonObject.ToString();
        }
    }
}
