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
        /// 月份数组
        /// </summary>
        public static int[] MonthAr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        /// <summary>
        /// 每月天数数组
        /// </summary>
        public static int[] DayLimitAr = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        /// <summary>
        /// 年龄段
        /// </summary>
        public static List<string> AgeRangeList = new List<string> { "儿童", "青年", "中年", "退休" };
        /// <summary>
        /// 脸型
        /// </summary>
        public static List<string> FaceTypeList = new List<string> { "未知", "圆型", "长方形", "正方型", "三角型", "瓜子脸" };
        /// <summary>
        /// 发色
        /// </summary>
        public static List<string> HairColorList = new List<string> { "未知", "偏黑", "偏黄", "黑白混杂", "白发" };
        /// <summary>
        /// 发质
        /// </summary>
        public static List<string> HairQualityList = new List<string> { "未知", "柔细", "自然卷", "偏硬" };
        /// <summary>
        /// 头发稀疏度
        /// </summary>
        public static List<string> HairDensityList = new List<string> { "未知", "稀疏", "中等", "浓密" };
        /// <summary>
        /// 脱发倾向
        /// </summary>
        public static List<string> HairLossTrendList = new List<string> { "未知", "无", "有", "严重" };
        /// <summary>
        /// 发色
        /// </summary>
        public static List<string> SkinColorList = new List<string> { "未知", "白皙", "中等", "较黑" };
        /// <summary>
        /// 身高
        /// </summary>
        public static List<string> HeightList = new List<string> { "未知", "165以下", "165~175", "175以上" };
        /// <summary>
        /// 体型
        /// </summary>
        public static List<string> BodySizeList = new List<string> { "未知", "偏瘦", "中等", "偏胖", "较胖" };
        /// <summary>
        /// 性别打扮
        /// </summary>
        public static List<string> SexDressList = new List<string> { "未知", "男", "中性", "女" };
        /// <summary>
        /// 职业
        /// </summary>
        public static List<string> ProfessionList = new List<string> { "未知", "公务员", "企业主", "职员", "学生", "其他" };


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
