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
using System.Data.OleDb;
using VipManager.Helper;
using VipManager.Model;
using System.Text.RegularExpressions;

namespace VipManager.FormControl
{
    public partial class VipAdd : Form
    {


        public VipAdd()
        {
            InitializeComponent();
            //获取原有会员编号，计算新会员编号
            string sqlVip = "select top 1 [No] from [Vip] where [IsDel]=false order by [No] desc";
            OleDbCommand comVip = new OleDbCommand(sqlVip, Config.con);
            OleDbDataReader reader = comVip.ExecuteReader();
            int maxNo = 0;
            if (reader.Read())
            {
                maxNo = reader.GetInt32(0);
            }
            maxNo++;
            txtNo.Text = maxNo.ToString();

            //绑定年龄段下拉框
            cbAge.DataSource = Config.AgeList;
            cbAge.DisplayMember = "Display";
            cbAge.ValueMember = "Value";
        }

        //根据会员类型激活或禁用不同选项
        private void cbType_TextChanged(object sender, EventArgs e)
        {
            ComboBox type = (ComboBox)sender;

        }

        private void btnAddVip_Click(object sender, EventArgs e)
        {
            //验证数据是否填写完整
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBoxEx.Show("姓名未填写！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                MessageBoxEx.Show("联系方式未填写！", "提示");
                return;
            }
            double balance = 0.0;                         //余额
            double discount = 0.0;                      //折扣
            int remainNum = 0;                            //次数


//            //插入会员信息
//            string sqlAddVip = @"insert into [Vip]([No],[VipName],[Phone],[Type],[Balance],[RemainNum],[Discount],[CreateAt],[DelAt],[IsDel],[Note],[LastPayAt],[Gender],[StartAt],[EndAt],[PayNum],[UserId]) values(
//{0},'{1}','{2}','{3}',{4},{5},{6},#{7}#,#{8}#,{9},'{10}',#{11}#,'{12}',#{13}#,#{14}#,{15},'{16}')".FormatStr(txtNo.Text, txtName.Text, txtPhone.Text, cbType.Text, balance, remainNum, discount, DateTime.Now,
//                                                                            DateTime.MinValue, false, null, DateTime.MinValue, cbGender.Text, startTime, endTime, 0, "1");
//            OleDbCommand com = new OleDbCommand(sqlAddVip, Config.con);
//            com.ExecuteNonQuery();
//            MessageBoxEx.Show("会员添加成功！", "提示");
            this.Close();
        }

    }
}
