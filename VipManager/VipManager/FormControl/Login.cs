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
            string userName = txtUserName.Text;
            string password = txtPassword.Text;
            if (userName == null || password == null)
            {
                MessageBoxEx.Show("用户名或密码不能为空！", "提示");
            }

            //获取用户信息
            Guid pwGuid = EncryptHelper.GetEncryPwd(password.ToLower());        //加密密码
            try
            {
                var builder = Builders<UserMongo>.Filter;
                var filter = builder.Eq(x => x.userName, userName);
                filter &= builder.Eq(x => x.Password, pwGuid);
                var col = MongoDBHelper.Instance.GetUser();
                var user = col.Find(filter).FirstOrDefault();

                if (user != null)
                {
                    Config.User = user;
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "LastLoginAt", DateTime.Now.AddHours(8) }, { "LoginNum", user.LoginNum + 1 } } } };
                    col.UpdateOne(filter, update);

                    //更新用户登陆Logo地址


                    Mains main = new Mains();
                    main.Show();
                    this.Hide();
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
    }
}
