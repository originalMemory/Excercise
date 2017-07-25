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
    public partial class Mains : Form
    {
        public Mains()
        {
            InitializeComponent();
            
        }

        private void Mains_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Mains_Load(object sender, EventArgs e)
        {
            UTabVip vip = new UTabVip();
            InitDgvVip();
            InitFirstVip();
            IsInitVip = true;

            ProInComb.Columns.Add("No", System.Type.GetType("System.Int32"));
            ProInComb.Columns.Add("ProName", System.Type.GetType("System.String"));
            ProInComb.Columns.Add("Price", System.Type.GetType("System.Double"));
            lbPro.DataSource = ProInComb;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "No";
        }

        #region 会员管理
        //添加会员
        private void btnAddVip_Click(object sender, EventArgs e)
        {
            VipAdd add = new VipAdd();
            add.ShowDialog();
            InitDgvVip();
            InitFirstVip();
        }

        /// <summary>
        /// 加载最近一次支付的会员的信息
        /// </summary>
        private void InitFirstVip()
        {
            string sqlLast = "select top 1 * from [Vip] where [IsDel]=false order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataReader reader = com.ExecuteReader();
            LoadVipInfo(reader);
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDgvVip()
        {
            string sql = "select * from [Vip] where [IsDel]=false";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvVip.DataSource = dt;
        }

        /// <summary>
        /// 将获取的数据显示到界面
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool LoadVipInfo(OleDbDataReader reader)
        {
            if (reader.Read())
            {
                txtVipNo.Text = reader["No"].ToString();
                txtVipName.Text = reader["VipName"].ToString();
                txtPhone.Text = reader["Phone"].ToString();
                txtVipPayNum.Text = reader["PayNum"].ToString();
                dtpVipReg.Value = (DateTime)reader["CreateAt"];
                dtpVipLastPay.Value = (DateTime)reader["LastPayAt"];
                cbGender.Text = reader["Gender"].ToString();
                string type = reader["Type"].ToString();
                cbType.Text = type;
                switch (type)
                {
                    case "次数型":
                        SetVipNum();
                        txtDetail.Text = reader["RemainNum"].ToString();
                        break;
                    case "折扣型":
                        SetVipDiscount();
                        txtDetail.Text = reader["Discount"].ToString();
                        txtBalance.Text = reader["Balance"].ToString();
                        break;
                    case "时间型":
                        SetVipTime();
                        dtpStart.Value = (DateTime)reader["StartAt"];
                        dtpEnd.Value = (DateTime)reader["EndAt"];
                        break;
                    default:
                        break;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 控件显示状态
        /// <summary>
        /// 显示次数型数据
        /// </summary>
        private void SetVipNum()
        {
            labDetail.Text = "次数：";
            labDetail.Visible = true;
            txtDetail.Visible = true;
            labDiscount.Visible = false;
            labBalance.Visible = false;
            txtBalance.Visible = false;
            labYuan.Visible = false;
            labStart.Visible = false;
            labEnd.Visible = false;
            dtpEnd.Visible = false;
            dtpStart.Visible = false;
        }
        /// <summary>
        /// 显示折扣型数据
        /// </summary>
        private void SetVipDiscount()
        {
            labDetail.Text = "折扣：";
            labDetail.Visible = true;
            txtDetail.Visible = true;
            labDiscount.Visible = true;
            labBalance.Visible = true;
            txtBalance.Visible = true;
            labYuan.Visible = true;
            labStart.Visible = false;
            labEnd.Visible = false;
            dtpEnd.Visible = false;
            dtpStart.Visible = false;
        }
        /// <summary>
        /// 显示时间型数据
        /// </summary>
        private void SetVipTime()
        {
            labDetail.Visible = false;
            txtDetail.Visible = false;
            labDiscount.Visible = false;
            labBalance.Visible = false;
            txtBalance.Visible = false;
            labYuan.Visible = false;
            labStart.Visible = true;
            labEnd.Visible = true;
            dtpEnd.Visible = true;
            dtpStart.Visible = true;
        }
        #endregion

        private void dgvVip_Click(object sender, EventArgs e)
        {
            //根据选中行刷新上方数据
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            txtVipNo.Text = dr.Cells["VipNo"].Value.ToString();
            txtVipName.Text = dr.Cells["VipName"].Value.ToString();
            txtPhone.Text = dr.Cells["VipPhone"].Value.ToString();
            txtVipPayNum.Text = dr.Cells["VipPayNum"].ToString();
            dtpVipReg.Value = (DateTime)dr.Cells["VipCreateAt"].Value;
            cbGender.Text = dr.Cells["VipGender"].Value.ToString();
            string type = dr.Cells["VipType"].Value.ToString();
            cbType.Text = type;
            switch (type)
            {
                case "次数型":
                    SetVipNum();
                    txtDetail.Text = dr.Cells["VipRemainNum"].Value.ToString();
                    break;
                case "折扣型":
                    SetVipDiscount();
                    txtDetail.Text = dr.Cells["VipDiscount"].Value.ToString();
                    txtBalance.Text = dr.Cells["VipBalance"].Value.ToString();
                    break;
                case "时间型":
                    SetVipTime();
                    dtpStart.Value = (DateTime)dr.Cells["VipStartAt"].Value;
                    dtpEnd.Value = (DateTime)dr.Cells["VipEndAt"].Value;
                    break;
                default:
                    break;
            }
        }

        //修改会员信息
        private void btnModVip_Click(object sender, EventArgs e)
        {
            //验证数据是否填写完整
            if (string.IsNullOrEmpty(txtVipName.Text))
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
                    break;
            }

            //修改会员信息
            string sqlModVip = @"update [Vip] set [VipName]='{0}',[Phone]='{1}',[Gender]='{2}'".FormatStr(txtVipName.Text, txtPhone.Text, cbGender.Text);
            switch (cbType.Text)
            {
                case "次数型":
                    sqlModVip += ",[Type]='{0}',[RemainNum]={1}".FormatStr(cbType.Text, txtDetail.Text);
                    break;
                case "折扣型":
                    sqlModVip += ",[Type]='{0}',[Discount]={1},[Balance]={2}".FormatStr(cbType.Text, txtDetail.Text, txtBalance.Text);
                    break;
                case "时间型":
                    sqlModVip += ",[Type]='{0}',[StartAt]={1},[EndAt]={2}".FormatStr(cbType.Text, startTime, endTime);
                    break;
            }
            sqlModVip += " where [No]={0}".FormatStr(txtVipNo.Text);

            OleDbCommand com = new OleDbCommand(sqlModVip, Config.con);
            com.ExecuteNonQuery();
            InitDgvVip();
            MessageBoxEx.Show("修改成功！", "提示");
        }

        //类型状态修改
        private void cbType_TextChanged(object sender, EventArgs e)
        {
            switch (cbType.Text)
            {
                case "次数型":
                    SetVipNum();
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    break;
                case "折扣型":
                    SetVipDiscount();
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    break;
                case "时间型":
                    SetVipTime();
                    txtBalance.Text = "0";
                    txtDetail.Text = "0";
                    dtpStart.Value = DateTime.Now;
                    dtpEnd.Value = DateTime.Now;
                    break;
                default:
                    break;
            }
        }

        //删除会员
        private void btnDelVip_Click(object sender, EventArgs e)
        {
            string sqlDelVip = "delete from [Vip] where [No]={0}".FormatStr(txtVipNo.Text);
            OleDbCommand com = new OleDbCommand(sqlDelVip, Config.con);
            com.ExecuteNonQuery();
            InitDgvVip();
            InitFirstVip();
            MessageBoxEx.Show("删除成功！", "提示");
        }

        //搜索会员
        private void btnSearchVip_Click(object sender, EventArgs e)
        {
            string factor = txtSearchVip.Text;
            if (!string.IsNullOrEmpty(factor))
            {
                //判断是搜索编号还是姓名或联系方式
                if (factor.IsNum())
                {
                    string sql = "select top 1 * from [Vip] where [IsDel]=false and [No]={0}".FormatStr(factor);
                    OleDbCommand com = new OleDbCommand(sql, Config.con);
                    OleDbDataReader reader = com.ExecuteReader();
                    if (!LoadVipInfo(reader))
                    {
                        MessageBoxEx.Show("无符合条件会员！", "提示");
                    }
                }
                else
                {
                    string sql = "select top 1 * from [Vip] where [IsDel]=false and [VipName]='{0}'".FormatStr(factor);
                    OleDbCommand com = new OleDbCommand(sql, Config.con);
                    OleDbDataReader reader = com.ExecuteReader();
                    if (!LoadVipInfo(reader))
                    {
                        sql = "select top 1 * from [Vip] where [IsDel]=false and [Phone]='{0}'".FormatStr(factor);
                        com = new OleDbCommand(sql, Config.con);
                        reader = com.ExecuteReader();
                        if (!LoadVipInfo(reader))
                        {
                            MessageBoxEx.Show("无符合条件会员！", "提示");
                        }
                    }
                }
            }
            else
            {
                MessageBoxEx.Show("搜索条件不能为空！", "提示");
            }
        }

        private void txtSearchVip_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearchVip_Click(sender, e);
            }
        }
        #endregion

        #region 产品管理
        private void btnAddPro_Click(object sender, EventArgs e)
        {
            ProAdd add = new ProAdd();
            add.ShowDialog();
            InitDgvPro();
        }

        /// <summary>
        /// 加载最近一次支付的产品的信息
        /// </summary>
        private void InitFirstPro()
        {
            string sqlLast = "select top 1 * from [Product] where [IsDel]=false order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataReader reader = com.ExecuteReader();
            LoadProInfo(reader);
        }

        /// <summary>
        /// 将获取的产品数据显示到界面
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool LoadProInfo(OleDbDataReader reader)
        {
            if (reader.Read())
            {
                txtProNo.Text = reader["No"].ToString();
                txtProName.Text = reader["ProName"].ToString();
                txtProPrice.Text = reader["Price"].ToString();
                txtProDesc.Text = reader["ProDesc"].ToString();
                txtProPayNum.Text = reader["PayNum"].ToString();
                dtpProCreate.Value = (DateTime)reader["CreateAt"];
                dtpProLastPay.Value = (DateTime)reader["LastPayAt"];
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化产品数据表
        /// </summary>
        private void InitDgvPro()
        {
            string sql = "select * from [Product] where [IsDel]=false";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvPro.DataSource = dt;
        }

        private void tabPro_Enter(object sender, EventArgs e)
        {
            string sqlLast = "select top 1 * from [Product] where [IsDel]=false order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataReader reader = com.ExecuteReader();
            LoadProInfo(reader);
        }

        //修改产品信息
        private void btnModPro_Click(object sender, EventArgs e)
        {
            //验证数据是否填写完整
            if (string.IsNullOrEmpty(txtProName.Text))
            {
                MessageBoxEx.Show("名称未填写！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtProDesc.Text))
            {
                MessageBoxEx.Show("描述未填写！", "提示");
                return;
            }
            double price = 0.0;
            if (string.IsNullOrEmpty(txtProPrice.Text))
            {
                MessageBoxEx.Show("价格未填写！", "提示");
                return;
            }
            else
            {
                try
                {
                    price = Convert.ToDouble(txtProPrice.Text);
                }
                catch
                {
                    MessageBoxEx.Show("价格只能为数字！", "提示");
                    return;
                }
            }

            //修改产品信息
            string sqlModPro = @"update [Product] set [ProName]='{0}',[ProDesc]='{1}',[Price]={2} where [No]={3}".FormatStr(txtProName.Text, txtProDesc.Text, price, txtProNo.Text);
            OleDbCommand com = new OleDbCommand(sqlModPro, Config.con);
            com.ExecuteNonQuery();
            InitDgvPro();
            MessageBoxEx.Show("修改成功！", "提示");
        }

        //删除产品
        private void btnDelPro_Click(object sender, EventArgs e)
        {
            string sqlDelPro = "delete from [Product] where [No]={0}".FormatStr(txtProNo.Text);
            OleDbCommand com = new OleDbCommand(sqlDelPro, Config.con);
            com.ExecuteNonQuery();
            InitDgvPro();
            InitFirstPro();
            MessageBoxEx.Show("删除成功！", "提示");
        }


        private void dgvPro_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //根据选中行刷新上方数据
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            txtProNo.Text = dr.Cells["No"].Value.ToString();
            txtProName.Text = dr.Cells["ProName"].Value.ToString();
            txtProPrice.Text = dr.Cells["Price"].Value.ToString();
            txtProDesc.Text = dr.Cells["ProDesc"].Value.ToString();
            txtProPayNum.Text = dr.Cells["PayNum"].Value.ToString();
            dtpProCreate.Value = (DateTime)dr.Cells["CreateAt"].Value;
            dtpProLastPay.Value = (DateTime)dr.Cells["LastPayAt"].Value;
        }

        //搜索产品信息
        private void btnSearchPro_Click(object sender, EventArgs e)
        {
            string factor = txtSearchPro.Text;
            if (!string.IsNullOrEmpty(factor))
            {
                //判断是搜索编号还是名称
                if (factor.IsNum())
                {
                    string sql = "select top 1 * from [Product] where [IsDel]=false and [No]={0}".FormatStr(factor);
                    OleDbCommand com = new OleDbCommand(sql, Config.con);
                    OleDbDataReader reader = com.ExecuteReader();
                    if (!LoadProInfo(reader))
                    {
                        MessageBoxEx.Show("无符合条件产品！", "提示");
                    }
                }
                else
                {
                    string sql = "select top 1 * from [Product] where [IsDel]=false and [ProName]='{0}'".FormatStr(factor);
                    OleDbCommand com = new OleDbCommand(sql, Config.con);
                    OleDbDataReader reader = com.ExecuteReader();
                    if (!LoadProInfo(reader))
                    {
                        MessageBoxEx.Show("无符合条件产品！", "提示");
                    }
                }
            }
            else
            {
                MessageBoxEx.Show("搜索条件不能为空！", "提示");
            }
        }

        private void txtSearchPro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //btnSearchVip_Click(sender, e);
            }
        }
        #endregion

        #region 套餐管理

        /// <summary>
        /// 已有套餐信息
        /// </summary>
        DataTable DtComb = new DataTable();
        /// <summary>
        /// 加载最近一次支付的会员的信息
        /// </summary>
        private void InitFirstComb()
        {
            string sqlLast = "select top 1 * from [Combination] where [IsDel]=false order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataReader reader = com.ExecuteReader();
            LoadCombInfo(reader);
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDgvComb()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            if (!IsInitComb)
            {
                dt.Columns.Add("No", System.Type.GetType("System.Int32"));
                dt.Columns.Add("CombName", System.Type.GetType("System.String"));
                dt.Columns.Add("Description", System.Type.GetType("System.String"));
                dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
                dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
                dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));
                dt.Columns.Add("Type", System.Type.GetType("System.String"));
            }

            //获取源数据
            string sql = "select * from [Combination] where [IsDel]=false";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DtComb.Clear();
            adapter.Fill(DtComb);
            foreach (DataRow oldRow in DtComb.Rows)
            {
                DataRow row = dt.NewRow();
                row["No"] = Convert.ToInt32(oldRow["No"]);
                row["CombName"] = oldRow["CombName"].ToString();
                row["Description"] = oldRow["Description"].ToString();
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["LastPayAt"] = (System.DateTime)oldRow["LastPayAt"];
                row["PayNum"] = Convert.ToInt32(oldRow["PayNum"]);
                row["Type"] = oldRow["Type"].ToString();
                dt.Rows.Add(row);
            }
            dgvComb.DataSource = dt;
        }

        /// <summary>
        /// 将获取的数据显示到界面
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool LoadCombInfo(OleDbDataReader reader)
        {
            if (reader.Read())
            {
                txtCombNo.Text = reader["No"].ToString();
                txtCombName.Text = reader["CombName"].ToString();
                txtCombDesc.Text = reader["Description"].ToString();
                txtCombPayNum.Text = reader["PayNum"].ToString();
                dtpCombCreate.Value = (DateTime)reader["CreateAt"];
                dtpCombLastPay.Value = (DateTime)reader["LastPayAt"];
                CombType type = (CombType)reader["Type"];
                switch (type)
                {
                    case CombType.Num:
                        {
                            cbCombType.Text = "次数型";
                            SetCombNum();
                            double price = (double)reader["Price"];
                            int num = (int)reader["Num"];
                            txtCombDetail.Text = price.ToString();
                            txtCombNum.Text = num.ToString();
                        }
                        break;
                    case CombType.Discount:
                        {
                            cbCombType.Text = "折扣型";
                            SetCombDiscount();
                            double discount = Convert.ToDouble(reader["Discount"]);
                            txtCombDetail.Text = discount.ToString();
                        }
                        break;
                    case CombType.Time:
                        {
                            cbCombType.Text = "时间型";
                            SetCombTime();
                            CombTimeType timeType = (CombTimeType)reader["Type"];
                            switch (timeType)
                            {
                                case CombTimeType.Month:
                                    cbCombTime.Text = "月卡";
                                    break;
                                case CombTimeType.Season:
                                    cbCombTime.Text = "季卡";
                                    break;
                                case CombTimeType.HalfYear:
                                    cbCombTime.Text = "半年卡";
                                    break;
                                case CombTimeType.Year:
                                    cbCombTime.Text = "年卡";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                }
                //获取套餐内产品信息
                string proNos = reader["ProNos"].ToString();
                string sqlPro = "select [No],[ProName],[Price] from [Product] where [IsDel]=false";
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
                adapter.Fill(ProInComb);
                
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 套餐内产品信息列表
        /// </summary>
        DataTable ProInComb = new DataTable();

        private void cbCombType_TextChanged(object sender, EventArgs e)
        {
            switch (cbCombType.Text)
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
        # region 套餐类型展示变更
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
        #endregion

        //添加套餐
        private void btnAddComb_Click(object sender, EventArgs e)
        {
            CombAdd add = new CombAdd();
            add.ShowDialog();
        }

        //根据选中行刷新上方数据
        private void dgvComb_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //获取选中套餐编号
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            int no = Convert.ToInt32(dr.Cells["CombNo"].Value);

            //获取详细套餐信息
            var rows = DtComb.Select("No=" + no);


            txtCombNo.Text = rows[0]["No"].ToString();
            txtCombName.Text = rows[0]["CombName"].ToString();
            txtCombDesc.Text = rows[0]["Description"].ToString();
            txtCombPayNum.Text = rows[0]["PayNum"].ToString();
            dtpCombCreate.Value = (DateTime)rows[0]["CreateAt"];
            dtpCombLastPay.Value = (DateTime)rows[0]["LastPayAt"];
            CombType type = (CombType)rows[0]["Type"];
            switch (type)
            {
                case CombType.Num:
                    {
                        cbCombType.Text = "次数型";
                        SetCombNum();
                        double price = Convert.ToDouble(rows[0]["Price"]);
                        int num = Convert.ToInt32(rows[0]["Num"]);
                        txtCombDetail.Text = price.ToString();
                        txtCombNum.Text = num.ToString();
                    }
                    break;
                case CombType.Discount:
                    {
                        cbCombType.Text = "折扣型";
                        SetCombDiscount();
                        double discount = Convert.ToDouble(rows[0]["Discount"]);
                        txtCombDetail.Text = discount.ToString();
                    }
                    break;
                case CombType.Time:
                    {
                        cbCombType.Text = "时间型";
                        SetCombTime();
                        CombTimeType timeType = (CombTimeType)rows[0]["Type"];
                        switch (timeType)
                        {
                            case CombTimeType.Month:
                                cbCombTime.Text = "月卡";
                                break;
                            case CombTimeType.Season:
                                cbCombTime.Text = "季卡";
                                break;
                            case CombTimeType.HalfYear:
                                cbCombTime.Text = "半年卡";
                                break;
                            case CombTimeType.Year:
                                cbCombTime.Text = "年卡";
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
            //获取套餐内产品信息
            string proNos = rows[0]["ProNos"].ToString();
            string sqlPro = "select [No],[ProName],[Price] from [Product] where [IsDel]=false and [No] in({0}) ".FormatStr(proNos);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
            ProInComb.Clear();
            adapter.Fill(ProInComb);
        }


        private void btnEditProInComb_Click(object sender, EventArgs e)
        {
            EditProInComb edit = new EditProInComb(ProInComb);
            edit.ReturnProList += this.SetProInCmob;
            edit.ShowDialog();
        }

        /// <summary>
        /// 重设套餐内产品列表
        /// </summary>
        /// <param name="dt"></param>
        public void SetProInCmob(DataTable dt)
        {
            ProInComb = dt.Copy();
            lbPro.DataSource = ProInComb;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "No";
        }
        #endregion

        //标签页切换时加载
        bool IsInitVip = false;
        bool IsInitPro = false;
        bool IsInitComb = false;
        private void tabMain_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabVip&&!IsInitVip)
            {
                return;
            }
            if (e.TabPage == tabPro&&!IsInitPro)
            {
                InitDgvPro();
                InitFirstPro();
                IsInitPro = true;
                return;
            }
            if (e.TabPage == tabComb && !IsInitComb)
            {
                InitDgvComb();
                InitFirstComb();
                IsInitComb = true;
                return;
            }
        }

        //删除套餐
        private void btnDelComb_Click(object sender, EventArgs e)
        {
            string sqlDelComb = "delete from [Combination] where [No]={0}".FormatStr(txtCombNo.Text);
            OleDbCommand com = new OleDbCommand(sqlDelComb, Config.con);
            com.ExecuteNonQuery();
            InitDgvComb();
            InitFirstComb();
            MessageBoxEx.Show("删除成功！", "提示");
        }

        //修改套餐
        private void btnEditComb_Click(object sender, EventArgs e)
        {
            //验证数据是否填写完整
            if (string.IsNullOrEmpty(txtCombName.Text))
            {
                MessageBoxEx.Show("名称未填写！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtCombDesc.Text))
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
            switch (cbCombType.Text)
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

            string sqlEditComb = @"update [Combination] set [CombName]='{0}',[Description]='{1}',[ProNos]='{2}',[Type]='{3}',[Price]={4},[Num]={5},[Discount]={6},[TimeRange]={7} where [No]={8}"
                .FormatStr(txtCombName.Text, txtCombDesc.Text, proNos, (int)type, price, num, discount, timeRange, txtCombNo.Text);
            OleDbCommand com = new OleDbCommand(sqlEditComb, Config.con);
            com.ExecuteNonQuery();
            InitDgvComb();
            MessageBoxEx.Show("修改成功！", "提示");
        }


        

    }
}
