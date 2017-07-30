using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using VipManager.Model;
using System.Windows.Forms;
using System.Data;

namespace VipManager.Helper
{
    public class Config
    {
        /// <summary>
        /// 登陆商家信息
        /// </summary>
        public static User userInfo;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source= " + Application.StartupPath + "\\Database\\database.mdb;");
        //public static OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;User ID=admin;Jet OleDb:Database Password=sinofaith; Data Source= " + Application.StartupPath + "\\Database\\database.db;");

        /// <summary>
        /// 系统静态环境初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                //连接数据库
                con.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                Application.Exit();
            }
        }

        /// <summary>
        /// 系统静态环境销毁
        /// </summary>
        public static void Dispose()
        {

            if (con.State != System.Data.ConnectionState.Closed)
                con.Close();
        }
    }
}
