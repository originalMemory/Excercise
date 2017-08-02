using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using CCWin;
using VipManager.Helper;
using VipManager.Model;

namespace VipManager.FormControl
{
    public partial class VipPay : Form
    {
        public VipPay()
        {
            InitializeComponent();

            InitCbPro();

            //初始化产品列表
            DtUsedPro = DtAllPro.Clone();
            lbPro.DataSource = DtUsedPro;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "ID";
        }

        /// <summary>
        /// 消费产品列表
        /// </summary>
        DataTable DtUsedPro = new DataTable();

        /// <summary>
        /// 所有产品数据表
        /// </summary>
        DataTable DtAllPro = new DataTable();

        /// <summary>
        /// 会员ID
        /// </summary>
        int VipID = 0;

        /// <summary>
        /// 套餐信息
        /// </summary>
        CombSnap CombSnapInfo = new CombSnap();

        /// <summary>
        /// 设置会员信息
        /// </summary>
        /// <param name="vipNo">会员编号</param>
        /// <param name="vipName">会员姓名</param>
        /// <param name="vipID">会员ID</param>
        /// <param name="combSnapID">套餐映射Id</param>
        public void SetVipInfo(string vipNo, string vipName, int vipID,int combSnapID)
        {
            txtVipName.Text = vipName;
            txtVipNo.Text = vipName;
            VipID = vipID;

            //获取套餐映射信息
            string sqlCombSnap = @"select c.[ID] as [CombSnapID],[CombName],[Type],[Num],c.[Price] as [CombPrice],[UsedNum],[Discount],[EndAt],
[ProID],p.[Price] as [ProPrice],[ProName] from [CombSnap] as c,[ProSnap] as p where c.[ID]={0} and p.[CombSnapID]={1}".FormatStr(combSnapID, combSnapID);
            OleDbCommand comCombSnap = new OleDbCommand(sqlCombSnap, Config.con);
            OleDbDataReader readerCombSnap = comCombSnap.ExecuteReader();
            int i = 0;      //重复次数
            CombSnapInfo.ProList = new List<ProSnap>();
            while (readerCombSnap.Read())
            {
                if (i == 0)
                {
                    CombSnapInfo.ID = Convert.ToInt32(readerCombSnap["CombSnapID"]);
                    //CombSnapInfo.No = Convert.ToInt32(readerCombSnap["No"]);
                    CombSnapInfo.Name = readerCombSnap["CombName"].ToString();
                    CombSnapInfo.Type = (CombType)readerCombSnap["Type"];
                    CombSnapInfo.Price = Convert.ToDouble(readerCombSnap["CombPrice"]);
                    CombSnapInfo.Num = Convert.ToInt32(readerCombSnap["Num"]);
                    CombSnapInfo.UsedNum = Convert.ToInt32(readerCombSnap["UsedNum"]);
                    CombSnapInfo.Discount = Convert.ToDouble(readerCombSnap["Discount"]);
                    CombSnapInfo.EndAt = (DateTime)readerCombSnap["EndAt"];
                }
                ProSnap pro = new ProSnap
                {
                    ProID = Convert.ToInt32(readerCombSnap["ProID"]),
                    Name = readerCombSnap["ProName"].ToString(),
                    Price = Convert.ToDouble(readerCombSnap["ProPrice"])
                };
                CombSnapInfo.ProList.Add(pro);
            }
            rbUseComb.Checked = true;
        }

        /// <summary>
        /// 初始化产品列表下拉框
        /// </summary>
        private void InitCbPro()
        {
            string sqlPro = "select [ID],[No],[ProName],[Price] from [Product]";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
            adapter.Fill(DtAllPro);
            cbPro.DataSource = DtAllPro;
            cbPro.DisplayMember = "ProName";
            cbPro.ValueMember = "ID";
        }

        private void btnAddProInComb_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(cbPro.SelectedValue);

            //检查该产品是否已添加
            DataRow[] rows = DtUsedPro.Select("ID='{0}'".FormatStr(id));
            if (rows.Count() > 0)
            {
                MessageBoxEx.Show("该产品已添加！", "提示");
            }
            else
            {
                rows = DtAllPro.Select("ID='{0}'".FormatStr(id));
                if (rows.Count() > 0)
                {
                    DtUsedPro.ImportRow(rows[0]);
                    ComputePrice();
                }
            }
        }

        private void btnDelProInComb_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lbPro.SelectedValue);
            DataRow[] rows = DtUsedPro.Select("ID='{0}'".FormatStr(id));
            if (rows.Count() > 0)
            {
                DtUsedPro.Rows.Remove(rows[0]);
                ComputePrice();
            }
        }

        private void ComputePrice()
        {
            double totalPrice = 0.0;
            foreach (DataRow row in DtUsedPro.Rows)
            {
                int proID = Convert.ToInt32(row["ID"]);
                double price = 0.0;
                price = Convert.ToDouble(row["Price"]);
                //判断是否使用套餐并获取套餐内的实际价格
                if (rbUseComb.Checked)
                {
                    var pro = CombSnapInfo.ProList.Find(x => x.ProID == proID);
                    if (pro != null)
                    {
                        switch (CombSnapInfo.Type)
                        {
                            case CombType.Num:
                                price = 0;
                                break;
                            case CombType.Discount:
                                price = CombSnapInfo.Price * CombSnapInfo.Discount / 100;
                                break;
                            case CombType.Time:
                                price = 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
                totalPrice += price;
            }
            txtTruePrice.Text = totalPrice.ToString("f2");
        }

        private void rbUseComb_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUseComb.Checked)
            {
                //检查套餐是否已用满次数或到期
                switch (CombSnapInfo.Type)
                {
                    case CombType.Num:
                        if (CombSnapInfo.Num <= CombSnapInfo.UsedNum)
                        {
                            rbUnUseComb.Checked = true;
                        }
                        break;
                    case CombType.Discount:
                        if (CombSnapInfo.EndAt < DateTime.Now)
                        {
                            rbUnUseComb.Checked = true;
                        }
                        break;
                    case CombType.Time:
                        if (CombSnapInfo.EndAt < DateTime.Now)
                        {
                            rbUnUseComb.Checked = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
