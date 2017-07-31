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
            InitTabVip();

            ProInComb.Columns.Add("No", System.Type.GetType("System.Int32"));
            ProInComb.Columns.Add("ProName", System.Type.GetType("System.String"));
            ProInComb.Columns.Add("Price", System.Type.GetType("System.Double"));
            lbPro.DataSource = ProInComb;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "No";
        }

        #region 会员管理
        /// <summary>
        /// 初始化会员管理页
        /// </summary>
        private void InitTabVip()
        {
            //绑定生日下拉框
            cbMonth.DataSource = Config.MonthAr;
            InitCbDay(Config.MonthAr[0]);
            //绑定年龄段下拉框
            cbAgeRange.DataSource = Config.AgeRangeList;
            //绑定脸型下拉框
            cbFaceType.DataSource = Config.FaceTypeList;
            //绑定发色下拉框
            cbHairColor.DataSource = Config.HairColorList;
            //绑定发质下拉框
            cbHairQuality.DataSource = Config.HairQualityList;
            //绑定浓密度下拉框
            cbHairDensity.DataSource = Config.HairDensityList;
            //绑定脱发倾向下拉框
            cbHairLossTrend.DataSource = Config.HairLossTrendList;
            //绑定肤色下拉框
            cbSkinColor.DataSource = Config.SkinColorList;
            //绑定身高下拉框
            cbHeight.DataSource = Config.HeightList;
            //绑定体型下拉框
            cbBodySize.DataSource = Config.BodySizeList;
            //绑定性别打扮下拉框
            cbSexDress.DataSource = Config.SexDressList;
            //绑定职业下拉框
            cbProfession.DataSource = Config.ProfessionList;
            //获取当前套餐列表
            IntVipComb();

            InitDgvVip();
            InitFirstVip();
            IsInitVip = true;
        }

        /// <summary>
        /// 会员套餐
        /// </summary>
        DataTable DtVipComb = new DataTable();

        /// <summary>
        /// 初始化会员管理员套餐
        /// </summary>
        private void IntVipComb()
        {
            string sql = "select [No],[CombName] from [Combination]";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            adapter.Fill(DtVipComb);
            cbVipComb.DataSource = DtVipComb;
            cbVipComb.DisplayMember = "CombName";
            cbVipComb.ValueMember = "No";
        }

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
            DataTable dt = new DataTable();
            string sqlLast = @"select a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[No] as [CombNo],[CombName] from [Vip] as a , [CombSnap] as b where a.[No]=b.[VipNo] order by a.[LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            adapter.Fill(dt);
            LoadVipSelectedRow(dt.Rows[0]);
        }

        DataTable DtVip = new DataTable();

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDgvVip()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("VipNo", System.Type.GetType("System.Int32"));
            dt.Columns.Add("VipName", System.Type.GetType("System.String"));
            dt.Columns.Add("CombName", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
            dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Birth", System.Type.GetType("System.String"));
            dt.Columns.Add("Phone", System.Type.GetType("System.String"));
            dt.Columns.Add("Gender", System.Type.GetType("System.String"));
            dt.Columns.Add("CombNo", System.Type.GetType("System.Int32"));

            //获取数据
            string sql = @"select a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[No] as [CombNo],[CombName] from [Vip] as a , [CombSnap] as b where a.[No]=b.[VipNo]";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DtVip.Clear();
            adapter.Fill(DtVip);
            foreach (DataRow oldRow in DtVip.Rows)
            {
                DataRow row = dt.NewRow();
                int no = Convert.ToInt32(oldRow["VipNo"]);
                row["VipNo"] = Convert.ToInt32(oldRow["VipNo"]);
                row["VipName"] = oldRow["VipName"].ToString();
                row["CombName"] = oldRow["CombName"].ToString();
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["LastPayAt"] = (System.DateTime)oldRow["LastPayAt"];
                row["PayNum"] = Convert.ToInt32(oldRow["PayNum"]);
                row["Birth"] = oldRow["Birth"].ToString();
                row["Phone"] = oldRow["Phone"].ToString();
                row["Gender"] = oldRow["Gender"].ToString();
                row["CombNo"] = Convert.ToInt32(oldRow["CombNo"]);
                dt.Rows.Add(row);
            }
            dgvVip.DataSource = dt;
        }

        /// <summary>
        /// 加载选定行的会员数据
        /// </summary>
        /// <param name="row">选定的会员表内某行</param>
        private void LoadVipSelectedRow(DataRow row)
        {
            txtVipNo.Text = row["VipNo"].ToString();
            txtVipName.Text = row["VipName"].ToString();
            txtPhone.Text = row["Phone"].ToString();
            txtVipPayNum.Text = row["PayNum"].ToString();
            dtpVipReg.Value = (DateTime)row["CreateAt"];
            dtpVipLastPay.Value = (DateTime)row["LastPayAt"];
            cbGender.Text = row["Gender"].ToString();
            string[] birth = row["Birth"].ToString().Split('/');
            cbMonth.Text = birth[0];
            cbDay.Text = birth[1];

            cbAgeRange.Text = row["AgeRange"].ToString();
            cbFaceType.Text = row["FaceType"].ToString();
            cbHairColor.Text = row["HairColor"].ToString();
            cbHairQuality.Text = row["HairQuality"].ToString();
            cbHairDensity.Text = row["HairDensity"].ToString();
            cbHairLossTrend.Text = row["HairLossTrend"].ToString();
            cbSkinColor.Text = row["SkinColor"].ToString();
            cbHeight.Text = row["Height"].ToString();
            cbBodySize.Text = row["BodySize"].ToString();
            cbSexDress.Text = row["SexDress"].ToString();
            cbProfession.Text = row["Profession"].ToString();

            //加载当前套餐
            int combNo = Convert.ToInt32(row["CombNo"]);
            DataRow[] rows = DtVipComb.Select("No=" + combNo);
            if (rows.Length > 0)
            {
                cbVipComb.Text = rows[0]["CombName"].ToString();
            }
        }

        /// <summary>
        /// 初始化天数下拉框
        /// </summary>
        /// <param name="month"></param>
        private void InitCbDay(int month)
        {
            var dayList = new List<int>();
            for (int i = 0; i < Config.DayLimitAr[month - 1]; i++)
            {
                dayList.Add(i + 1);
            }
            cbDay.DataSource = dayList;
        }

        //根据选中行刷新上方数据
        private void dgvVip_Click(object sender, EventArgs e)
        {
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            int combNo = Convert.ToInt32(dr.Cells["VipNo"].Value);
            DataRow[] rows = DtVip.Select("VipNo=" + combNo);
            LoadVipSelectedRow(rows[0]);
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
           

            //修改会员信息
            string sqlModVip = @"update [Vip] set [VipName]='{0}',[Phone]='{1}',[Gender]='{2}'".FormatStr(txtVipName.Text, txtPhone.Text, cbGender.Text);
            sqlModVip += " where [No]={0}".FormatStr(txtVipNo.Text);

            OleDbCommand com = new OleDbCommand(sqlModVip, Config.con);
            com.ExecuteNonQuery();
            InitDgvVip();
            MessageBoxEx.Show("修改成功！", "提示");
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
                    
                }
                else
                {
                    
                    
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
            string sqlLast = "select top 1 * from [Product] order by [LastPayAt] desc";
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

        DataTable DtPro = new DataTable();

        /// <summary>
        /// 初始化产品数据表
        /// </summary>
        private void InitDgvPro()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("No", System.Type.GetType("System.Int32"));
            dt.Columns.Add("VipName", System.Type.GetType("System.String"));
            dt.Columns.Add("CombName", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
            dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Birth", System.Type.GetType("System.String"));
            dt.Columns.Add("Phone", System.Type.GetType("System.String"));
            dt.Columns.Add("Gender", System.Type.GetType("System.String"));

            string sql = "select * from [Vip]";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            adapter.Fill(DtPro);
            foreach (DataRow oldRow in DtPro.Rows)
            {
                DataRow row = dt.NewRow();
                row["No"] = Convert.ToInt32(oldRow["No"]);
                row["ProName"] = oldRow["ProName"].ToString();
                row["Description"] = oldRow["Description"].ToString();
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["LastPayAt"] = (System.DateTime)oldRow["LastPayAt"];
                row["PayNum"] = Convert.ToInt32(oldRow["PayNum"]);
                row["Type"] = oldRow["Type"].ToString();
                dt.Rows.Add(row);
            }
            dgvPro.DataSource = dt;
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

        private void dgvPro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //根据选中行刷新上方数据
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            txtProNo.Text = dr.Cells["ProNo"].Value.ToString();
            txtProName.Text = dr.Cells["ProName"].Value.ToString();
            txtProPrice.Text = dr.Cells["ProPrice"].Value.ToString();
            txtProDesc.Text = dr.Cells["ProDesc"].Value.ToString();
            txtProPayNum.Text = dr.Cells["ProPayNum"].Value.ToString();
            dtpProCreate.Value = (DateTime)dr.Cells["ProCreateAt"].Value;
            dtpProLastPay.Value = (DateTime)dr.Cells["ProLastPayAt"].Value;
        }

        //搜索产品信息
        private void btnSearchPro_Click(object sender, EventArgs e)
        {
            string factor = txtSearchPro.Text;

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
            string sqlLast = "select top 1 * from [Combination] order by [LastPayAt] desc";
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
            dt.Columns.Add("No", System.Type.GetType("System.Int32"));
            dt.Columns.Add("CombName", System.Type.GetType("System.String"));
            dt.Columns.Add("Description", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
            dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Type", System.Type.GetType("System.String"));

            //获取源数据
            string sql = "select * from [Combination]";
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
                CombType type = (CombType)oldRow["Type"];
                switch (type)
                {
                    case CombType.Num:
                        row["Type"] = "次数型";
                        break;
                    case CombType.Discount:
                        row["Type"] = "折扣型";
                        break;
                    case CombType.Time:
                        row["Type"] = "时间型";
                        break;
                    default:
                        break;
                }
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
                            double price = Convert.ToDouble(reader["Price"]);
                            int num = Convert.ToInt32(reader["Num"]);
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
                string sqlPro = "select [No],[ProName],[Price] from [Product] where [No] in({0})".FormatStr(proNos);
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
            InitDgvComb();
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

            LoadCombSelectedRow(rows[0]);

        }

        /// <summary>
        /// 加载套餐表中选定行
        /// </summary>
        /// <param name="row"></param>
        private void LoadCombSelectedRow(DataRow row)
        {
            txtCombNo.Text = row["No"].ToString();
            txtCombName.Text = row["CombName"].ToString();
            txtCombDesc.Text = row["Description"].ToString();
            txtCombPayNum.Text = row["PayNum"].ToString();
            dtpCombCreate.Value = (DateTime)row["CreateAt"];
            dtpCombLastPay.Value = (DateTime)row["LastPayAt"];
            CombType type = (CombType)row["Type"];
            switch (type)
            {
                case CombType.Num:
                    {
                        cbCombType.Text = "次数型";
                        SetCombNum();
                        double price = Convert.ToDouble(row["Price"]);
                        int num = Convert.ToInt32(row["Num"]);
                        txtCombDetail.Text = price.ToString();
                        txtCombNum.Text = num.ToString();
                    }
                    break;
                case CombType.Discount:
                    {
                        cbCombType.Text = "折扣型";
                        SetCombDiscount();
                        double discount = Convert.ToDouble(row["Discount"]);
                        txtCombDetail.Text = discount.ToString();
                    }
                    break;
                case CombType.Time:
                    {
                        cbCombType.Text = "时间型";
                        SetCombTime();
                        CombTimeType timeType = (CombTimeType)row["Type"];
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
            string proNos = row["ProNos"].ToString();
            string sqlPro = "select [No],[ProName],[Price] from [Product] where [No] in({0}) ".FormatStr(proNos);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
            ProInComb.Clear();
            adapter.Fill(ProInComb);
        }

        //修改套餐内产品信息
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
                proNos += row["No"].ToString() + ",";
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

        //搜索套餐
        private void btnSearchComb_Click(object sender, EventArgs e)
        {
            string factor = txtSearchComb.Text;
            DataRow[] rowAr = null;
            if (string.IsNullOrEmpty(factor))
            {
                MessageBoxEx.Show("搜索条件不能为空！", "提示");
            }
            else if (factor.IsNum())
            {
                rowAr = DtComb.Select("No=" + factor);
            }
            else
            {
                rowAr = DtComb.Select("CombName like '%{0}%'".FormatStr(factor));
            }

            //判断是否加载数据
            if (rowAr.Length > 0)
            {
                LoadCombSelectedRow(rowAr[0]);
            }
            else
            {
                MessageBoxEx.Show("无搜索结果！", "提示");
            }
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


        

    }
}
