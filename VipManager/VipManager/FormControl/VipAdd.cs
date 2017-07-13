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

namespace VipManager.FormControl
{
    public partial class VipAdd : Skin_Color
    {
        /// <summary>
        /// 新会员编号
        /// </summary>
        int No;

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
            No = maxNo + 1;
            txtNo.Text = No.ToString();
            dtpStart.Value = DateTime.Now;
            dtpEnd.Value = DateTime.Now;

            //初始化为次数型
            labDetail.Text = "次数：";
            txtDetail.Enabled = true;
            dtpStart.Enabled = false;
            dtpEnd.Enabled = false;
            labDiscount.Visible = false;
            txtBalance.Enabled = false;
            txtBalance.Text = "0";
            txtDetail.Text = "0";
        }

        //根据会员类型激活或禁用不同选项
        private void cbType_TextChanged(object sender, EventArgs e)
        {
            ComboBox type = (ComboBox)sender;
            switch (type.Text)
            {
                case "次数型":
                    labDetail.Text = "次数：";
                    txtDetail.Enabled = true;
                    dtpStart.Enabled = false;
                    dtpEnd.Enabled = false;
                    labDiscount.Visible = false;
                    txtBalance.Enabled = false;
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    break;
                case "折扣型":
                    labDetail.Text = "折扣：";
                    txtDetail.Enabled = true;
                    dtpStart.Enabled = false;
                    dtpEnd.Enabled = false;
                    labDiscount.Visible = true;
                    txtBalance.Enabled = true;
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    break;
                case "时间型":
                    txtDetail.Enabled = false;
                    dtpStart.Enabled = true;
                    dtpEnd.Enabled = true;
                    labDiscount.Visible = false;
                    txtBalance.Enabled = false;
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    break;
            }
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
            DateTime startTime = DateTime.Now;          //开始时间
            DateTime endTime = DateTime.Now;            //结束时间
            //VipType type = VipType.Num;
            switch (cbType.Text)
            {
                case "次数型":
                    try
                    {
                        remainNum = Convert.ToInt32(txtDetail.Text);
                    }
                    catch
                    {
                        MessageBoxEx.Show("次数只能为数字！", "提示");
                        return;
                    }
                    //type = VipType.Num;
                    break;
                case "折扣型":
                    try
                    {
                        discount = Convert.ToDouble(txtDetail.Text);
                    }
                    catch
                    {
                        MessageBoxEx.Show("折扣只能为数字！", "提示");
                        return;
                    }
                    try
                    {
                        balance = Convert.ToDouble(txtBalance.Text);
                    }
                    catch
                    {
                        MessageBoxEx.Show("余额只能为数字！", "提示");
                        return;
                    }
                   // type = VipType.Discount;
                    break;
                case "时间型":
                    startTime = dtpStart.Value;
                    endTime = dtpEnd.Value;
                    if (startTime >= endTime)
                    {
                        MessageBoxEx.Show("开始时间必须小于结束时间", "提示");
                        return;
                    }
                    if (startTime < DateTime.Now.Date)
                    {
                        MessageBoxEx.Show("开始时间不能小于当前时间", "提示");
                        return;
                    }
                    //type = VipType.Time;
                    break;
            }

            //插入会员信息
            string sqlAddVip = @"insert into [Vip]([No],[VipName],[Phone],[Type],[Balance],[RemainNum],[Discount],[CreateAt],[DelAt],[IsDel],[Note],[LastPayAt],[Gender],[StartAt],[EndAt],[PayNum],[UserId]) values(
{0},'{1}','{2}','{3}',{4},{5},{6},#{7}#,#{8}#,{9},'{10}',#{11}#,'{12}',#{13}#,#{14}#,{15},'{16}')".FormatStr(txtNo.Text, txtName.Text, txtPhone.Text, cbType.Text, balance, remainNum, discount, DateTime.Now,
                                                                            DateTime.MinValue, false, null, DateTime.MinValue, cbGender.Text, startTime, endTime, 0, "1");
            OleDbCommand com = new OleDbCommand(sqlAddVip, Config.con);
            com.ExecuteNonQuery();
            MessageBoxEx.Show("会员添加成功！", "提示");
            this.Close();
        }


    }
}
