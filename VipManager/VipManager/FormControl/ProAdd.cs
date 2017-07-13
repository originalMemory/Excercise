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
    public partial class ProAdd : Skin_Color
    {
        /// <summary>
        /// 新会员编号
        /// </summary>
        int No;

        public ProAdd()
        {
            InitializeComponent();
            //获取原有会员编号，计算新会员编号
            string sqlVip = "select top 1 [No] from [Product] where [IsDel]=false order by [No] desc";
            OleDbCommand comVip = new OleDbCommand(sqlVip, Config.con);
            OleDbDataReader reader = comVip.ExecuteReader();
            int maxNo = 0;
            if (reader.Read())
            {
                maxNo = reader.GetInt32(0);
            }
            No = maxNo + 1;
            txtNo.Text = No.ToString();
            txtPrice.Text = "0";
        }

        private void btnAddVip_Click(object sender, EventArgs e)
        {
            //验证数据是否填写完整且符合要求
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBoxEx.Show("姓名未填写！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                MessageBoxEx.Show("描述未填写！", "提示");
                return;
            }
            double price = 0.0;
            if (string.IsNullOrEmpty(txtPrice.Text))
            {
                MessageBoxEx.Show("价格未填写！", "提示");
                return;
            }
            else
            {
                try
                {
                    price = Convert.ToDouble(txtPrice.Text);
                }
                catch
                {
                    MessageBoxEx.Show("价格只能为数字！", "提示");
                    return;
                }
            }
            //插入会员信息
            string sqlAddPro = @"insert into [Product]([No],[ProName],[ProDesc],[Price],[CreateAt],[IsDel],[DelAt],[LastPayAt],[UserId],[PayNum]) values(
{0},'{1}','{2}',{3},#{4}#,{5},#{6}#,#{7}#,'{8}',{9})"
                .FormatStr(txtNo.Text, txtName.Text, txtDesc.Text, price, DateTime.Now,false,DateTime.MinValue,DateTime.MinValue,"1",0);
            OleDbCommand com = new OleDbCommand(sqlAddPro, Config.con);
            com.ExecuteNonQuery();
            MessageBoxEx.Show("产品添加成功！", "提示");
            this.Close();
        }


    }
}
