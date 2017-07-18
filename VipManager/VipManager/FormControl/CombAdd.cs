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
using VipManager.UControl;

namespace VipManager.FormControl
{
    public partial class CombAdd : Skin_Color
    {
        /// <summary>
        /// 新会员编号
        /// </summary>
        int No;

        /// <summary>
        /// 套餐内产品信息列表
        /// </summary>
        List<ProInfo> ProInComb = new List<ProInfo>();

        /// <summary>
        /// 所有产品数据表
        /// </summary>
        DataTable dtPro = new DataTable();

        public CombAdd()
        {
            InitializeComponent();
            //获取原有会员编号，计算新会员编号
            string sqlVip = "select top 1 [No] from [Combination] where [IsDel]=false order by [No] desc";
            OleDbCommand comVip = new OleDbCommand(sqlVip, Config.con);
            OleDbDataReader reader = comVip.ExecuteReader();
            int maxNo = 0;
            if (reader.Read())
            {
                maxNo = reader.GetInt32(0);
            }
            No = maxNo + 1;
            txtNo.Text = No.ToString();

            InitCbPro();

            //初始化为减价型
            UCombNum uNum = new UCombNum();
            panelType.Controls.Add(uNum);
            
        }

        /// <summary>
        /// 初始化产品列表下拉框
        /// </summary>
        private void InitCbPro()
        {
            string sqlPro = "select [No],[ProName],[Price] from [Product] where [IsDel]=false";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
            adapter.Fill(dtPro);
            cbPro.DataSource = dtPro;
            cbPro.DisplayMember = "ProName";
            cbPro.ValueMember = "No"; 
        }

        //根据会员类型激活或禁用不同选项
        private void cbType_TextChanged(object sender, EventArgs e)
        {
            ComboBox type = (ComboBox)sender;
            switch ((CombType)type.SelectedValue)
            {
                case CombType.Num:
                    {
                        panelType.Controls.Clear();
                        UCombNum uNum = new UCombNum();
                        panelType.Controls.Add(uNum);
                    }
                    break;
                case CombType.Discount:
                    {

                    }
                    break;
            }
        }

        private void btnAddVip_Click(object sender, EventArgs e)
        {
            string userId="1";
            //验证数据是否填写完整
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBoxEx.Show("名称未填写！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtDesc.Text))
            {
                MessageBoxEx.Show("描述未填写！", "提示");
                return;
            }
            if (lbComb.Items.Count == 0)
            {
                MessageBoxEx.Show("套餐不能为空！", "提示");
                return;
            }
            double sourcePrice = Convert.ToDouble(txtSPrice.Text);
            //计算折扣
            double discount = 0.0;                      //折扣
            if (cbType.Text=="折扣型")
            {
                discount = Convert.ToDouble(txtDetail.Text);
            }

            string proNos = string.Join(";", ProInComb.Select(x=>x.No));
            string proNames = string.Join(";", ProInComb.Select(x=>x.Name));

            //插入会员信息
            string sqlAddVip = @"insert into [Combination]([No],[CombName],[Description],[ProNos],[ProNames],[CreateAt],[UserId],[IsDel],[DelAt],[LastPayAt],[PayNum],[Type],[SourcePrice],[TruePrice],[Discout]) values(
{0},'{1}','{2}','{3}','{4}',#{5}#,'{6}',{7},#{8}#,#{9}#,{10},'{11}',{12},{13},{14}"
                .FormatStr(txtNo.Text, txtName.Text, txtDesc.Text, proNos, proNames, DateTime.Now, userId, false, DateTime.MinValue, DateTime.MinValue, 0, cbType.Text, txtSPrice.Text, txtTPrice, Text, discount);
            OleDbCommand com = new OleDbCommand(sqlAddVip, Config.con);
            com.ExecuteNonQuery();
            MessageBoxEx.Show("套餐添加成功！", "提示");
            this.Close();
        }

        
        //添加产品
        private void btnAddPro_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(cbPro.SelectedValue);
            string name = cbPro.SelectedText;

            //获取产品的价格
            double price = 0.0;
            DataColumn col = dtPro.Columns["No"];
            foreach (DataGridViewRow row in dtPro.Rows)
            {
                if (Convert.ToInt32(row.Cells["No"].Value) == no)
                {
                    price = Convert.ToDouble(row.Cells["Price"].Value);
                    break;
                }
            }

            name += "/" + price;

            //检查该产品是否已添加
            if (ProInComb.Find(x=>x.No==no)==null)
            {
                ProInfo pro = new ProInfo
                {
                    No = no,
                    Name = name,
                    Price = price
                };
                ProInComb.Add(pro);
                //重计算原总价
                double sourcePrice = ProInComb.Sum(x => x.Price);
                txtSPrice.Text = sourcePrice.ToString();

                //重计算真实价格
                double reduce = 0.0;                         //减价
                double discount = 0.0;                      //折扣
                double truePrice = 0.0;                     //真实价格
                switch (cbType.Text)
                {
                    case "减价型":
                        reduce = Convert.ToInt32(txtDetail.Text);
                        truePrice = sourcePrice - reduce;
                        break;
                    case "折扣型":
                        discount = Convert.ToDouble(txtDetail.Text);
                        truePrice = sourcePrice * (1 - discount / 100);
                        break;
                }
                txtTPrice.Text = truePrice.ToString();
            }
            else
            {
                MessageBoxEx.Show("该产品已添加！", "提示");
            }

            //初始化下拉菜单
            lbComb.DataSource = ProInComb;
            lbComb.DisplayMember = "Name";
            lbComb.ValueMember = "No";
        }

        //删除产品
        private void btnDelProInComb_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(cbPro.SelectedValue);
            ProInComb.RemoveAll(x => x.No == no);
        }

        //在输入折扣或减免参数后验证是否为数字并重计算真实价格
        private void txtDetail_Leave(object sender, EventArgs e)
        {
            if (txtDetail.Text.IsNum())
            {
                double sourcePrice = ProInComb.Sum(x => x.Price);

                //重计算真实价格
                double reduce = 0.0;                         //减价
                double discount = 0.0;                      //折扣
                double truePrice = 0.0;                     //真实价格
                switch (cbType.Text)
                {
                    case "减价型":
                        reduce = Convert.ToInt32(txtDetail.Text);
                        truePrice = sourcePrice - reduce;
                        break;
                    case "折扣型":
                        discount = Convert.ToDouble(txtDetail.Text);
                        truePrice = sourcePrice * (1 - discount / 100);
                        break;
                }
                txtTPrice.Text = truePrice.ToString();
            }
            else
            {
                switch (cbType.Text)
                {
                    case "减价型":
                        MessageBoxEx.Show("减价只能为数字！", "提示");
                        txtDetail.Focus();
                        break;
                    case "折扣型":

                        MessageBoxEx.Show("折扣只能为数字！", "提示");
                        txtDetail.Focus();
                        break;
                }
            }
            
        }

        private void lbComb_Leave(object sender, EventArgs e)
        {
            if (txtDetail.Text.IsNum())
            {
                double sourcePrice = ProInComb.Sum(x => x.Price);

                //重计算真实价格
                double reduce = 0.0;                         //减价
                double discount = 0.0;                      //折扣
                double truePrice = 0.0;                     //真实价格
                switch (cbType.Text)
                {
                    case "减价型":
                        reduce = Convert.ToInt32(txtDetail.Text);
                        truePrice = sourcePrice - reduce;
                        break;
                    case "折扣型":
                        discount = Convert.ToDouble(txtDetail.Text);
                        truePrice = sourcePrice * (1 - discount / 100);
                        break;
                }
                txtTPrice.Text = truePrice.ToString();
            }
            else
            {
                switch (cbType.Text)
                {
                    case "减价型":
                        MessageBoxEx.Show("减价只能为数字！", "提示");
                        txtDetail.Focus();
                        break;
                    case "折扣型":

                        MessageBoxEx.Show("折扣只能为数字！", "提示");
                        txtDetail.Focus();
                        break;
                }
            }
        }


    }
}
