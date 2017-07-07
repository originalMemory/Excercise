using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;

namespace VipManager.FormControl
{
    public partial class Login : Skin_Color
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = "123";
            string password = "123";
            if (userName == txtUserName.Text && password == txtPassword.Text)
            {
                Mains main = new Mains();
                main.Show();
                this.Hide();
            }
        }
    }
}
