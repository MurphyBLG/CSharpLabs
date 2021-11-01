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

namespace Lab5
{
    public partial class AuthorizePage : Form
    {
        public AuthorizePage()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void authorizeButton_Click(object sender, EventArgs e)
        {
            string token = null;
            this.Hide();

            using (AuthorisationForm authForm = new AuthorisationForm())
            {
                if (authForm.ShowDialog() == DialogResult.Yes) token = authForm.Token;
            }

            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Ошибка авторизации", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (MethodsCheckPage mcp = new MethodsCheckPage())
            {
                mcp.Token = token;
                mcp.ShowDialog();
            }

            this.Close();
        }
    }
}
