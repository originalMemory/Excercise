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
using MongoDB.Bson;
using MongoDB.Driver;
using VipManager.Helper;
using VipManager.Model;
using VipData.Model;
using VipData.Helper;

namespace VipManager.FormControl
{
    public partial class Login : Skin_Color
    {
        public Login()
        {
            InitializeComponent();
            txtPassword.KeyPress += new KeyPressEventHandler(this.txtPassword_KeyPress);
            //读取用户Logo
            string url = OperateIni.ReadIniData("Enveronment", "logoUrl", Commons.GetAppSetting("config"));
            picLogo.ImageLocation = url;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var sw1 = new System.Diagnostics.Stopwatch();
            sw1.Start();
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            if (userName == null || password == null)
            {
                MessageBoxEx.Show("用户名或密码不能为空！", "提示");
            }

            //加密密码
            //string md5 = EncryptHelper.GetMD5(password);
            //string base64 = EncryptHelper.EncordBase64(md5);
            var guid = EncryptHelper.GetEncryPwd(password);

            //获取用户信息
            try
            {
                var builder = Builders<UserMongo>.Filter;
                var filter = builder.Eq(x => x.UserName, userName);
                filter &= builder.Eq(x => x.Password, guid);
                var col = MongoDBHelper.Instance.GetUser();
                var user = col.Find(filter).FirstOrDefault();

                if (user != null)
                {
                    Config.User = user;
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "LastLoginAt", DateTime.Now.AddHours(8) }, { "LoginNum", user.LoginNum + 1 } } } };
                    col.UpdateOne(filter, update);

                    sw1.Stop();
                    var sw2 = new System.Diagnostics.Stopwatch();
                    sw2.Start();
                    //更新用户登陆Logo地址
                    RefreshLogo();

                    Mains main = new Mains();
                    main.Show();
                    this.Hide();

                    sw2.Stop();
                    
                }
                else
                {
                    MessageBoxEx.Show("用户名或密码错误！", "提示");

                }
            }
            catch
            {
                MessageBoxEx.Show("网络不佳,请重试！", "提示");
            }
        }

        /// <summary>
        /// 更新用户登陆Logo地址
        /// </summary>
        void RefreshLogo()
        {
            string logoUrl = Config.User.LogoUrl;
            string iniPath = Commons.GetAppSetting("config");
            OperateIni.WriteIniData("Enveronment", "logoUrl", logoUrl, iniPath);
        }

        /// <summary>
        /// 备份本地数据文件
        /// </summary>
        void BackUpLocalDB()
        {
            string backupFloder = Application.StartupPath + "\\Backup\\";
            string backupfile = backupFloder + "backup.mdb";
            var files = System.IO.Directory.GetFiles(backupFloder);
            if (files.Length > 0)
            {

            }
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
