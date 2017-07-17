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
        /// 年龄段下拉框数据源
        /// </summary>
        public static DataTable AgeList = new DataTable();

        /// <summary>
        /// 系统静态环境初始化
        /// </summary>
        public static void Init()
        {
            try
            {
                //连接数据库
                con.Open();

                //生成年龄段数据源
                AgeList.Columns.Add("Display",System.Type.GetType("System.String"));
                AgeList.Columns.Add("Value",System.Type.GetType("System.Object"));

                DataRow child = AgeList.NewRow();
                child["Display"] = "儿童";
                child["Value"] = AgeRange.Child;
                AgeList.Rows.Add(child);
                DataRow youth = AgeList.NewRow();
                youth["Display"] = "青年";
                youth["Value"] = AgeRange.Youth;
                AgeList.Rows.Add(youth);
                DataRow middle = AgeList.NewRow();
                middle["Display"] = "中年";
                middle["Value"] = AgeRange.Middle;
                AgeList.Rows.Add(middle);
                DataRow retire = AgeList.NewRow();
                retire["Display"] = "青年";
                retire["Value"] = AgeRange.Retire;
                AgeList.Rows.Add(retire);
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
