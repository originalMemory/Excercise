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
        public void SetVipInfo(string vipNo, string vipName, int vipID)
        {
            txtVipName.Text = vipName;
            txtVipNo.Text = vipNo;
            this.VipID = vipID;

            //获取套餐映射信息
            string sqlCombSnap = @"select * from [CombSnap] where [VipID]={0} and [IsDel]=false".FormatStr(vipID);
            OleDbCommand comCombSnap = new OleDbCommand(sqlCombSnap, Config.con);
            OleDbDataReader readerCombSnap = comCombSnap.ExecuteReader();
            CombSnapInfo.ProList = new List<ProInfo>();
            string proIDs="";
            if (readerCombSnap.Read())
            {
                CombSnapInfo.ID = Convert.ToInt32(readerCombSnap["ID"]);
                //CombSnapInfo.No = Convert.ToInt32(readerCombSnap["No"]);
                CombSnapInfo.Name = readerCombSnap["CombName"].ToString();
                CombSnapInfo.Type = (CombType)readerCombSnap["Type"];
                CombSnapInfo.Price = Convert.ToDouble(readerCombSnap["Price"]);
                CombSnapInfo.Num = Convert.ToInt32(readerCombSnap["Num"]);
                CombSnapInfo.UsedNum = Convert.ToInt32(readerCombSnap["UsedNum"]);
                CombSnapInfo.Discount = Convert.ToDouble(readerCombSnap["Discount"]);
                CombSnapInfo.EndAt = (DateTime)readerCombSnap["EndAt"];
                proIDs = readerCombSnap["ProIDs"].ToString();
            }

            //获取套餐内产品信息
            DataRow[] rows = DtAllPro.Select("ID in ({0})".FormatStr(proIDs));
            foreach (var row in rows)
            {
                ProInfo pro = new ProInfo
                {
                    ProID = Convert.ToInt32(row["ID"]),
                    Name = row["ProName"].ToString(),
                    Price = Convert.ToDouble(row["Price"])
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

        /// <summary>
        /// 总价
        /// </summary>
        double TotalPrice = 0.0;
        /// <summary>
        /// 实际支付价格
        /// </summary>
        double PayPrice = 0.0;

        private void ComputePrice()
        {
            TotalPrice = 0.0;
            PayPrice = 0.0;
            foreach (DataRow row in DtUsedPro.Rows)
            {
                int proID = Convert.ToInt32(row["ID"]);
                double price = Convert.ToDouble(row["Price"]);
                TotalPrice += price;
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
                PayPrice += price;
            }
            txtTruePrice.Text = PayPrice.ToString("f2");
        }

        //判断是否使用套餐
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
            ComputePrice();
        }

        //确认支付
        private void btnPay_Click(object sender, EventArgs e)
        {
            //生成交易记录
            string sqlAddPay = @"insert into [PayRecord]([VipID],[IsUseComb],[CombSnapID],[CombSnapName],[CreateAt],[TotalPrice],[PayPrice]) values(
{0},{1},'{2}','{3}',#{4}#,{5},{6})"
                .FormatStr(VipID, rbUseComb.Checked, CombSnapInfo.ID, CombSnapInfo.Name, DateTime.Now, TotalPrice, PayPrice);
            OleDbCommand comAddPay = new OleDbCommand(sqlAddPay, Config.con);
            comAddPay.ExecuteNonQuery();

            //获取交易ID
            int payID = 0;
            string sqlGetPay = @"select top 1 [ID] from [PayRecord] where [VipID]={0} order by [CreateAt] desc".FormatStr(VipID);
            OleDbCommand comGetPay = new OleDbCommand(sqlGetPay, Config.con);
            OleDbDataReader readerGetPay = comGetPay.ExecuteReader();
            if (readerGetPay.Read())
            {
                payID = readerGetPay.GetInt32(0);
            }

            //判断是否启用次数套餐
            if (rbUseComb.Checked && CombSnapInfo.Type == CombType.Num)
            {
                //更新使用次数
                string sqlUpCombSnap = @"update [CombSnap] set [UsedNum]=[UsedNum]+1 where [ID]={0}".FormatStr(CombSnapInfo.ID);
                OleDbCommand comUpComb = new OleDbCommand(sqlUpCombSnap, Config.con);
                comUpComb.ExecuteNonQuery();
            }

            //获取产品信息
            var proIDList = new List<string>();
            foreach (DataRow row in DtUsedPro.Rows)
            {
                proIDList.Add(row["ID"].ToString());
            }
            string proIDs = string.Join(",", proIDList);
            string sqlGetPro = @"select * from [Product] where [ID] in ({0})".FormatStr(proIDs);
            OleDbCommand comGetPro = new OleDbCommand(sqlGetPro, Config.con);
            OleDbDataReader readerGetPro = comGetPro.ExecuteReader();
            while (readerGetPro.Read())
            {
                int proID = Convert.ToInt32(readerGetPro["ID"]);
                var proInfo = CombSnapInfo.ProList.Find(x => x.ProID == proID);
                int combSnapID = 0;
                if (proInfo != null)
                {
                    combSnapID = proInfo.ProID;
                }
                //插入产品映射
                string sqlAddProSnap = @"insert into [ProSnap]([No],[ProName],[Description],[Price],[CreateAt],[LastPayAt],[UserId],[PayNum],[VipID],[CombSnapID],[ProID],[PayID]) values(
{0},'{1}','{2}',{3},#{4}#,#{5}#,'{6}',{7},{8},{9},{10},{11})"
                   .FormatStr(readerGetPro["No"], readerGetPro["ProName"], readerGetPro["Description"], readerGetPro["Price"], DateTime.Now, DateTime.Now
                   , readerGetPro["UserId"], Convert.ToInt32(readerGetPro["PayNum"]), VipID, combSnapID, readerGetPro["ID"], payID);
                OleDbCommand comAddProSnap = new OleDbCommand(sqlAddProSnap, Config.con);
                comAddProSnap.ExecuteNonQuery();
            }

            //更新产品消费记录
            string sqlUpPro = @"update [Product] set [LastPayAt]=#{0}#,[PayNum]=[PayNum]+1 where [ID] in ({1})".FormatStr(DateTime.Now, proIDs);
            OleDbCommand combUpPro = new OleDbCommand(sqlUpPro, Config.con);
            combUpPro.ExecuteNonQuery();

            string sqlUpVip = @"update [Vip] set [LastPayAt]=#{0}#,[PayNum]=[PayNum]+1 where [ID]={1}".FormatStr(DateTime.Now, VipID);
            OleDbCommand combUpVip = new OleDbCommand(sqlUpVip, Config.con);
            combUpVip.ExecuteNonQuery();

            MessageBoxEx.Show("消费成功!", "提示");
            this.Close();
        }
    }
}
