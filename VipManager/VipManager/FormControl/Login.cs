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
using System.Net.FtpClient;
using System.Net;
using System.IO;
using System.Threading;

namespace VipManager.FormControl
{
    public partial class Login : Skin_Color
    {
        public Login()
        {
            InitializeComponent();
            txtPassword.KeyPress += new KeyPressEventHandler(this.txtPassword_KeyPress);
            //读取用户Logo
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            string url = OperateIni.ReadIniData("Enveronment", "logoUrl", iniPath);
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
                    Thread upload = new Thread(BackUpOnlineDB);
                    upload.Start();
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
            string iniPath = AppDomain.CurrentDomain.BaseDirectory + "Config.ini";
            OperateIni.WriteIniData("Enveronment", "logoUrl", logoUrl, iniPath);
        }

        /// <summary>
        /// 往服务器上传备份文件
        /// </summary>
        void BackUpOnlineDB()
        {
            string backupFloder = Application.StartupPath + "\\Backup\\";
            if (!Directory.Exists(backupFloder))
            {
                Directory.CreateDirectory(backupFloder);
            }
            string backupfile = backupFloder + "upload.mdb";
            File.Copy(Config.DbPath, backupfile, true);
            string uploadPath = "/" + Config.User._id.ToString() + "/";
            FTPUpload(uploadPath, backupfile);
            File.Delete(backupfile);
        }

        /// <summary>
        /// FTP上传文件
        /// </summary>
        /// <param name="savepath">服务器用于保存的文件夹路径，不是服务器根路径,例如： "/UploadDocumentsSave/"</param>
        /// <param name="filePath">本地文件全路径</param>
        public void FTPUpload(string savepath, string filePath)
        {
            FtpClient ftp = new FtpClient();
            ftp.Host = Config.FtpIP;
            ftp.Credentials = new NetworkCredential(Config.FtpUser, Config.FtpPassword);
            ftp.Connect();
            if (!ftp.DirectoryExists(savepath))
            {
                ftp.CreateDirectory(savepath);
            }
            using (var fileStream = File.OpenRead(filePath))
            using (var ftpStream = ftp.OpenWrite(savepath + Path.GetFileName(filePath)))
            {
                var buffer = new byte[8 * 1024];
                int count;
                while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, count);
                }
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
