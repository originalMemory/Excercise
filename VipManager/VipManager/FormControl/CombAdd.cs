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
        /// 套餐内产品列表
        /// </summary>
        DataTable ProInComb = new DataTable();

        /// <summary>
        /// 所有产品数据表
        /// </summary>
        DataTable dtPro = new DataTable();

        public CombAdd()
        {
            InitializeComponent();
            //获取原有套餐编号，计算新会员编号
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

            //初始化产品列表
            ProInComb = dtPro.Clone();
            lbPro.DataSource = ProInComb;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "No";
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
            switch (type.Text)
            {
                case "次数型":
                    SetCombNum();
                    break;
                case "折扣型":
                    SetCombDiscount();
                    break;
                case "时间型":
                    SetCombTime();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 切换为次数型展示
        /// </summary>
        private void SetCombNum()
        {
            labCombDetail.Text = "价格：";
            labCombUnit.Text = "元";
            txtCombDetail.Text = "0.0";
            txtCombNum.Text = "0";
            cbCombTime.Visible = false;
            labCombNum.Visible = true;
            labCombUnit.Visible = true;
            txtCombDetail.Visible = true;
            txtCombNum.Visible = true;
        }

        /// <summary>
        /// 切换为折扣型展示
        /// </summary>
        private void SetCombDiscount()
        {
            labCombDetail.Text = "折扣：";
            labCombUnit.Text = "%";
            txtCombDetail.Text = "0.0";
            cbCombTime.Visible = false;
            labCombNum.Visible = false;
            labCombUnit.Visible = true;
            txtCombDetail.Visible = true;
            txtCombNum.Visible = false;
        }

        /// <summary>
        /// 切换为时间型展示
        /// </summary>
        private void SetCombTime()
        {
            labCombDetail.Text = "时间：";
            cbCombTime.Visible = true;
            labCombNum.Visible = false;
            txtCombNum.Visible = false;
            labCombUnit.Visible = false;
            txtCombDetail.Visible = false;
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
            if (lbPro.Items.Count == 0)
            {
                MessageBoxEx.Show("产品不能为空！", "提示");
                return;
            }

            string proNos = "";

            foreach (DataRow row in ProInComb.Rows)
            {
                proNos += row["No"].ToString()+",";
            }
            if (proNos.Length > 0)
                proNos = proNos.Substring(0, proNos.Length - 1);

            double price = 0.0;
            int num = 0;
            double discount = 0.0;
            CombType type = new CombType();
            int timeRange = 0;
            switch (cbType.Text)
            {
                case "次数型":
                    type = CombType.Num;
                    price = Convert.ToDouble(txtCombDetail.Text);
                    num = Convert.ToInt32(txtCombDetail.Text);
                    break;
                case "折扣型":
                    type = CombType.Discount;
                    discount = Convert.ToDouble(txtCombDetail.Text);
                    break;
                case "时间型":
                    {
                        switch (cbCombTime.Text)
                        {
                            case "月卡":
                                timeRange = 1;
                                break;
                            case "季卡":
                                timeRange = 3;
                                break;
                            case "半年卡":
                                timeRange = 6;
                                break;
                            case "年卡":
                                timeRange = 12;
                                break;
                            default:
                                break;
                        }
                    }
                    type = CombType.Time;
                    break;
            }

            //插入套餐信息
            string sqlAddComb = @"insert into [Combination]([No],[CombName],[Description],[ProNos],[CreateAt],[UserId],[IsDel],[DelAt],[LastPayAt],[PayNum],[Type],[Price],[Num],[Discount],[TimeRange]) values(
{0},'{1}','{2}','{3}',#{4}#,'{5}',{6},#{7}#,#{8}#,{9},{10},{11},{12},{13},{14})"
                .FormatStr(txtNo.Text, txtName.Text, txtDesc.Text, proNos, DateTime.Now, userId, false, DateTime.MinValue, DateTime.MinValue, 0, (int)type,price,num,discount,timeRange);
            OleDbCommand com = new OleDbCommand(sqlAddComb, Config.con);
            com.ExecuteNonQuery();
            MessageBoxEx.Show("套餐添加成功！", "提示");
            this.Close();
        }

        
        //添加产品
        private void btnAddPro_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(cbPro.SelectedValue);

            //检查该产品是否已添加
            DataRow[] rows = ProInComb.Select("No='{0}'".FormatStr(no));
            if (rows.Count() > 0)
            {
                MessageBoxEx.Show("该产品已添加！", "提示");
            }
            else
            {
                rows = dtPro.Select("No='{0}'".FormatStr(no));
                if (rows.Count() > 0)
                {
                    ProInComb.ImportRow(rows[0]);
                }
            }
        }

        //删除产品
        private void btnDelProInComb_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(lbPro.SelectedValue);
            DataRow[] rows = ProInComb.Select("No='{0}'".FormatStr(no));
            if (rows.Count() > 0)
            {
                ProInComb.Rows.Remove(rows[0]);
            }
        }



    }
}
