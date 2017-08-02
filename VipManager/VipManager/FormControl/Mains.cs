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
            //获取当前套餐列表
            //IntVipComb();

            InitDgvVip();
            InitFirstVip();
            IsInitVip = true;

            
        }

        #region 会员管理
        /// <summary>
        /// 初始化会员管理页
        /// </summary>
        private void InitTabVip()
        {
            //输入框限制事件
            txtPhone.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);

            cbGender.Text = "男";
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
        }

        ///// <summary>
        ///// 会员套餐
        ///// </summary>
        //DataTable DtVipComb = new DataTable();

        ///// <summary>
        ///// 初始化会员套餐
        ///// </summary>
        //private void IntVipComb()
        //{
        //    string sql = "select [ID],[CombName] from [Combination]";
        //    OleDbCommand com = new OleDbCommand(sql, Config.con);
        //    OleDbDataAdapter adapter = new OleDbDataAdapter(com);
        //    adapter.Fill(DtVipComb);
        //    cbVipComb.DataSource = DtVipComb;
        //    cbVipComb.DisplayMember = "CombName";
        //    cbVipComb.ValueMember = "ID";
        //}

        //添加会员
        private void btnAddVip_Click(object sender, EventArgs e)
        {
            VipAdd add = new VipAdd();
            add.ShowDialog();
            InitDgvVip();
        }

        /// <summary>
        /// 加载最近一次支付的会员的信息
        /// </summary>
        private void InitFirstVip()
        {
            DataTable dt = new DataTable();
            string sqlLast = @"select top 1 a.[ID] as [VipID], a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[ID]=b.[VipID] and b.[IsDel]=false order by a.[LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
                LoadVipSelectedRow(dt.Rows[0]);
        }

        /// <summary>
        /// 会员数据表
        /// </summary>
        DataTable DtVip = new DataTable();
        /// <summary>
        /// 当前选中会员Id
        /// </summary>
        int CurVipID = 0;

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDgvVip()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("VipID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("VipNo", System.Type.GetType("System.Int32"));
            dt.Columns.Add("VipName", System.Type.GetType("System.String"));
            dt.Columns.Add("CombName", System.Type.GetType("System.String"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
            dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));
            dt.Columns.Add("Birth", System.Type.GetType("System.String"));
            dt.Columns.Add("Phone", System.Type.GetType("System.String"));
            dt.Columns.Add("Gender", System.Type.GetType("System.String"));
            dt.Columns.Add("CombSnapID", System.Type.GetType("System.Int32"));

            //获取数据
            string sql = @"select a.[ID] as [VipID],a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[ID]=b.[VipID] and b.[IsDel]=false";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DtVip.Clear();
            adapter.Fill(DtVip);
            foreach (DataRow oldRow in DtVip.Rows)
            {
                DataRow row = dt.NewRow();
                row["VipID"] = Convert.ToInt32(oldRow["VipID"]);
                row["VipNo"] = Convert.ToInt32(oldRow["VipNo"]);
                row["VipName"] = oldRow["VipName"].ToString();
                row["CombName"] = oldRow["CombName"].ToString();
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["LastPayAt"] = (System.DateTime)oldRow["LastPayAt"];
                row["PayNum"] = Convert.ToInt32(oldRow["PayNum"]);
                row["Birth"] = oldRow["Birth"].ToString();
                row["Phone"] = oldRow["Phone"].ToString();
                row["Gender"] = oldRow["Gender"].ToString();
                row["CombSnapID"] = Convert.ToInt32(oldRow["CombSnapID"]);
                dt.Rows.Add(row);
            }
            dgvVip.DataSource = dt;
        }

        /// <summary>
        /// 当前套餐映射ID
        /// </summary>
        int CurCombSnapID = 0;

        /// <summary>
        /// 加载选定行的会员数据
        /// </summary>
        /// <param name="row">选定的会员表内某行</param>
        private void LoadVipSelectedRow(DataRow row)
        {
            CurVipID = Convert.ToInt32(row["VipID"]);
            CurCombSnapID = Convert.ToInt32(row["CombSnapID"]);
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
            txtVipComb.Text = row["CombName"].ToString();
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
            int vipID = Convert.ToInt32(dr.Cells["VipID"].Value);
            DataRow[] rows = DtVip.Select("VipID=" + vipID);
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
            string birth = cbMonth.Text + "/" + cbDay.Text;

            //修改会员信息
            string sqlModVip = @"update [Vip] set [VipName]='{0}',[Gender]='{1}',[Phone]='{2}',[Birth]='{3}',[AgeRange]='{4}',[FaceType]='{5}',
[HairColor]='{6}',[HairQuality]='{7}',[HairDensity]='{8}',[HairLossTrend]='{9}',[Height]='{10}',[BodySize]='{11}',[SkinColor]='{12}',[Profession]='{13}',[SexDress]='{14}'"
                .FormatStr(txtVipName.Text, cbGender.Text, txtPhone.Text,birth,cbAgeRange.Text,cbFaceType.Text,cbHairColor.Text,cbHairQuality.Text,cbHairDensity,
                cbHairLossTrend.Text,cbHeight.Text,cbBodySize.Text,cbSkinColor.Text,cbProfession.Text,cbSexDress.Text);
            sqlModVip += " where [ID]={0}".FormatStr(CurVipID);

            OleDbCommand com = new OleDbCommand(sqlModVip, Config.con);
            com.ExecuteNonQuery();
            InitDgvVip();
            MessageBoxEx.Show("修改成功！", "提示");
        }

        //删除会员
        private void btnDelVip_Click(object sender, EventArgs e)
        {
            //删除会员表信息
            string sqlDelVip = "delete from [Vip] where [ID]={0}".FormatStr(CurVipID);
            OleDbCommand comDelVip = new OleDbCommand(sqlDelVip, Config.con);
            comDelVip.ExecuteNonQuery();

            //删除套餐映射表信息
            string sqlDelComb = "delete from [CombSnap] where [VipID]={0}".FormatStr(CurVipID);
            OleDbCommand comDelComb = new OleDbCommand(sqlDelComb, Config.con);
            comDelComb.ExecuteNonQuery();
            //删除产品映射表信息
            string sqlDelPro = "delete from [ProSnap] where [VipID]={0}".FormatStr(CurVipID);
            OleDbCommand comDelPro = new OleDbCommand(sqlDelPro, Config.con);
            comDelPro.ExecuteNonQuery();

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
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
                LoadProSelectedRow(dt.Rows[0]);
        }


        /// <summary>
        /// 产品数据表
        /// </summary>
        DataTable DtPro = new DataTable();
        /// <summary>
        /// 当前选中产品ID
        /// </summary>
        int CurProID = 0;

        /// <summary>
        /// 初始化产品数据表
        /// </summary>
        private void InitDgvPro()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("No", System.Type.GetType("System.Int32"));
            dt.Columns.Add("ProName", System.Type.GetType("System.String"));
            dt.Columns.Add("Description", System.Type.GetType("System.String"));
            dt.Columns.Add("Price", System.Type.GetType("System.Double"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("LastPayAt", System.Type.GetType("System.String"));
            dt.Columns.Add("PayNum", System.Type.GetType("System.Int32"));

            string sql = "select * from [Product]";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DtPro.Clear();
            adapter.Fill(DtPro);
            foreach (DataRow oldRow in DtPro.Rows)
            {
                DataRow row = dt.NewRow();
                row["ID"] = Convert.ToInt32(oldRow["ID"]);
                row["No"] = Convert.ToInt32(oldRow["No"]);
                row["ProName"] = oldRow["ProName"].ToString();
                row["Description"] = oldRow["Description"].ToString();
                row["Price"] = Convert.ToDouble(oldRow["Price"]);
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["LastPayAt"] = (System.DateTime)oldRow["LastPayAt"];
                row["PayNum"] = Convert.ToInt32(oldRow["PayNum"]);
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
            string sqlModPro = @"update [Product] set [ProName]='{0}',[Description]='{1}',[Price]={2} where [id]={3}".FormatStr(txtProName.Text, txtProDesc.Text, price, CurProID);
            OleDbCommand com = new OleDbCommand(sqlModPro, Config.con);
            com.ExecuteNonQuery();
            InitDgvPro();
            MessageBoxEx.Show("修改成功！", "提示");
        }

        //删除产品
        private void btnDelPro_Click(object sender, EventArgs e)
        {
            string sqlDelPro = "delete from [Product] where [id]={0}".FormatStr(CurProID);
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
            int id = Convert.ToInt32(dr.Cells["ProId"].Value);
            DataRow[] rows = DtPro.Select("ID=" + id);
            LoadProSelectedRow(rows[0]);
        }

        /// <summary>
        /// 展示当前选中的产品行
        /// </summary>
        /// <param name="row"></param>
        private void LoadProSelectedRow(DataRow row)
        {
            CurProID = Convert.ToInt32(row["ID"]);
            txtProNo.Text = row["No"].ToString();
            txtProName.Text = row["ProName"].ToString();
            txtProPrice.Text = row["Price"].ToString();
            txtProDesc.Text = row["Description"].ToString();
            txtProPayNum.Text = row["PayNum"].ToString();
            dtpProCreate.Value = (DateTime)row["CreateAt"];
            dtpProLastPay.Value = (DateTime)row["LastPayAt"];
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
        /// 当前选中套餐ID
        /// </summary>
        int CurCombID = 0;
        /// <summary>
        /// 加载最近一次使用的套餐的信息
        /// </summary>
        private void InitFirstComb()
        {
            string sqlLast = "select top 1 * from [Combination] order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
                LoadCombSelectedRow(dt.Rows[0]);
        }

        /// <summary>
        /// 初始化数据表
        /// </summary>
        private void InitDgvComb()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
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
                row["ID"] = Convert.ToInt32(oldRow["ID"]);
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
            labCombNum.Text = "次数";
            txtCombDetail.Text = "0.0";
            txtCombNum.Text = "0";
            cbCombTime.Visible = false;
            labCombNum.Visible = true;
            labCombUnit.Visible = true;
            txtCombDetail.Visible = true;
            txtCombNum.Visible = true;
            cbDiscountTime.Visible = false;
        }

        /// <summary>
        /// 切换为折扣型展示
        /// </summary>
        private void SetCombDiscount()
        {
            labCombDetail.Text = "折扣：";
            labCombNum.Text = "时间";
            labCombUnit.Text = "%";
            txtCombDetail.Text = "0.0";
            cbCombTime.Visible = false;
            labCombNum.Visible = true;
            labCombUnit.Visible = true;
            txtCombDetail.Visible = true;
            txtCombNum.Visible = false;
            cbDiscountTime.Visible = true;
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
            cbDiscountTime.Visible = false;
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
            int id = Convert.ToInt32(dr.Cells["CombID"].Value);

            //获取详细套餐信息
            var rows = DtComb.Select("ID=" + id);

            LoadCombSelectedRow(rows[0]);

        }

        /// <summary>
        /// 加载套餐表中选定行
        /// </summary>
        /// <param name="row"></param>
        private void LoadCombSelectedRow(DataRow row)
        {
            CurCombID = Convert.ToInt32(row["ID"]);
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
                        int timeType = Convert.ToInt32(row["TimeRange"]);
                        timeType /= 3;
                        switch ((CombTimeType)timeType)
                        {
                            case CombTimeType.Month:
                                cbDiscountTime.Text = "月卡";
                                break;
                            case CombTimeType.Season:
                                cbDiscountTime.Text = "季卡";
                                break;
                            case CombTimeType.HalfYear:
                                cbDiscountTime.Text = "半年卡";
                                break;
                            case CombTimeType.Year:
                                cbDiscountTime.Text = "年卡";
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case CombType.Time:
                    {
                        cbCombType.Text = "时间型";
                        SetCombTime();
                        int timeType = Convert.ToInt32(row["TimeRange"]);
                        timeType /= 3;
                        switch ((CombTimeType)timeType)
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
            string proIDs = row["ProIDs"].ToString();
            string sqlPro = "select [ID],[ProName] from [Product] where [ID] in({0}) ".FormatStr(proIDs);
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
            lbPro.ValueMember = "ID";
        }

        //删除套餐
        private void btnDelComb_Click(object sender, EventArgs e)
        {
            string sqlDelComb = "delete from [Combination] where [ID]={0}".FormatStr(CurCombID);
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

            string proIDs = "";

            foreach (DataRow row in ProInComb.Rows)
            {
                proIDs += row["ID"].ToString() + ",";
            }
            if (proIDs.Length > 0)
                proIDs = proIDs.Substring(0, proIDs.Length - 1);

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
                    {
                        type = CombType.Discount;
                        discount = Convert.ToDouble(txtCombDetail.Text);
                        switch (cbDiscountTime.Text)
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

            string sqlEditComb = @"update [Combination] set [CombName]='{0}',[Description]='{1}',[ProIDs]='{2}',[Type]='{3}',[Price]={4},[Num]={5},[Discount]={6},[TimeRange]={7} where [ID]={8}"
                .FormatStr(txtCombName.Text, txtCombDesc.Text, proIDs, (int)type, price, num, discount, timeRange, CurCombID);
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
                //限制文本框输入范围
                txtCombDetail.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.DoubleLimit);
                txtCombNum.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);
                return;
            }
            if (e.TabPage == tabComb && !IsInitComb)
            {
                ProInComb.Columns.Add("ID", System.Type.GetType("System.Int32"));
                ProInComb.Columns.Add("ProName", System.Type.GetType("System.String"));
                ProInComb.Columns.Add("Price", System.Type.GetType("System.Double"));
                lbPro.DisplayMember = "ProName";
                lbPro.ValueMember = "ID";
                lbPro.DataSource = ProInComb;
                InitDgvComb();
                InitFirstComb();
                IsInitComb = true;
                txtProPrice.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.DoubleLimit);
                return;
            }
        }

        //修改产品套餐
        private void btnEditVipComb_Click(object sender, EventArgs e)
        {
            EditVipComb edit = new EditVipComb();
            edit.VipID = CurVipID;
            edit.setCombName += this.SetVipComb;
            edit.ShowDialog();
            InitDgvVip();
        }

        /// <summary>
        /// 设置会员套餐名
        /// </summary>
        /// <param name="combName">套餐名</param>
        public void SetVipComb(string combName)
        {
            txtVipComb.Text = combName;
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            VipPay pay = new VipPay();
            pay.SetVipInfo(txtVipNo.Text, txtVipName.Text, CurVipID, CurCombSnapID);
            pay.ShowDialog();
        }

       

    }
}
