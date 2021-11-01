using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class AuthorisationForm : Form
    {
        private string _authorizeUri = "https://oauth.vk.com/authorize";
        private string _redirectUri = "https://oauth.vk.com/blank.html";

        // Необходимо зарегистрировать свое приложение на https://vk.com/apps?act=manage
        // Затем получить ID приложения и записать его в переменную _clientId:

        private int _clientId = 7985612;

        public string Token { get; private set; }
        public AuthorisationForm()
        {
            InitializeComponent();
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string uri = e.Url.ToString();
            if (uri.StartsWith(_authorizeUri)) return;

            if (!uri.StartsWith(_redirectUri))
            {
                DialogResult = DialogResult.No;
                return;
            }

            var parameters = (from param in uri.Split('#')[1].Split('&')
                              let parts = param.Split('=')
                              select new
                              {
                                  Name = parts[0],
                                  Value = parts[1]
                              }
                             ).ToDictionary(v => v.Name, v => v.Value);

            Token = parameters["access_token"];
            DialogResult = DialogResult.Yes;
        }

        private void AuthorisationForm_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(
                new Uri("https://oauth.vk.com/authorize?"
                        + $"client_id={_clientId}&display=page&redirect_uri={_redirectUri}&"
                        + "response_type=token&v=5.131"));
        }
    }
}
