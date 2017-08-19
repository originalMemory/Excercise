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
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;

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

            //限制文本框输入范围
            txtCombDetail.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.DoubleLimit);
            txtCombNum.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);
            txtSearchComb.SkinTxt.KeyPress += new KeyPressEventHandler(this.txtSearchComb_KeyPress);

            //产品初始设置
            DtProInComb.Columns.Add("ID", System.Type.GetType("System.Int32"));
            DtProInComb.Columns.Add("ProName", System.Type.GetType("System.String"));
            DtProInComb.Columns.Add("Price", System.Type.GetType("System.Double"));
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "ID";
            lbPro.DataSource = DtProInComb;
            txtProPrice.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.DoubleLimit);
            txtSearchPro.SkinTxt.KeyPress += new KeyPressEventHandler(this.txtSearchPro_KeyPress);

            txtShopPhone.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);

            //消息记录初始设置
            DtPayPro.Columns.Add("ID", System.Type.GetType("System.Int32"));
            DtPayPro.Columns.Add("ProName", System.Type.GetType("System.String"));
            lbPayPro.DisplayMember = "PayName";
            lbPayPro.ValueMember = "ID";
            lbPayPro.DataSource = DtPayPro;
            txtSearchPay.SkinTxt.KeyPress += new KeyPressEventHandler(this.txtSearchPay_KeyPress);
        }

        /// <summary>
        /// 计算会员数量
        /// </summary>
        public void ComputeVipNum()
        {

        }

        #region 会员管理
        /// <summary>
        /// 初始化会员管理页
        /// </summary>
        private void InitTabVip()
        {
            //输入框限制事件
            txtPhone.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);
            //搜索框绑定搜索事件
            txtSearchVip.SkinTxt.KeyPress += new KeyPressEventHandler(this.txtSearchVip_KeyPress);

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
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[ID]=b.[VipID] and b.[IsDel]=false and a.[UserId]='{0}' order by a.[LastPayAt] desc".FormatStr(Config.User._id.ToString());
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
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[ID]=b.[VipID] and b.[IsDel]=false and a.[UserId]='{0}'".FormatStr(Config.User._id.ToString());
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
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(factor))
            {
                //判断是搜索编号还是姓名或联系方式
                if (factor.IsNum())
                {
                    //搜索编号
                    string sqlVipNo = @"select top 1 a.[ID] as [VipID], a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[No]={0} and a.[ID]=b.[VipID] and b.[IsDel]=false and a.[UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comVipNo = new OleDbCommand(sqlVipNo, Config.con);
                    OleDbDataAdapter adapterVipNo = new OleDbDataAdapter(comVipNo);
                    adapterVipNo.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadVipSelectedRow(dt.Rows[0]);
                    else
                    {
                        //搜索联系方式
                        string sqlVipPhone = @"select top 1 a.[ID] as [VipID], a.[No] as [VipNo],a.[vipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[Phone]='{0}' and a.[ID]=b.[VipID] and b.[IsDel]=false and a.[UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                        OleDbCommand comVipPhone = new OleDbCommand(sqlVipPhone, Config.con);
                        OleDbDataAdapter adapterVipPhone = new OleDbDataAdapter(comVipPhone);
                        adapterVipPhone.Fill(dt);
                        if (dt.Rows.Count > 0)
                            LoadVipSelectedRow(dt.Rows[0]);
                        else
                            MessageBoxEx.Show("无符合条件数据！", "提示");
                    }
                }
                else
                {
                    //搜索姓名
                    string sqlVipName = @"select top 1 a.[ID] as [VipID], a.[No] as [VipNo],a.[VipName],a.[CreateAt],a.[LastPayAt],a.[PayNum],[Birth],[Phone],[Gender],[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress],
            b.[ID] as [CombSnapID],[CombID],[CombName] from [Vip] as a , [CombSnap] as b where a.[VipName]='{0}' and a.[ID]=b.[VipID] and b.[IsDel]=false and a.[UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comVipName = new OleDbCommand(sqlVipName, Config.con);
                    OleDbDataAdapter adapterVipName = new OleDbDataAdapter(comVipName);
                    adapterVipName.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadVipSelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
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

        /// <summary>
        /// 设置会员套餐名
        /// </summary>
        /// <param name="combName">套餐名</param>
        public void SetVipComb(string combName)
        {
            txtVipComb.Text = combName;
        }

        //收银
        private void btnPay_Click(object sender, EventArgs e)
        {
            VipPay pay = new VipPay();
            pay.SetVipInfo(txtVipNo.Text, txtVipName.Text, CurVipID);
            pay.ShowDialog();
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
            string sqlLast = "select top 1 * from [Product] where [UserId]='{0}' order by [LastPayAt] desc".FormatStr(Config.User._id.ToString());
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

            string sql = "select * from [Product] where [UserId]='{0}'".FormatStr(Config.User._id.ToString());
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
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(factor))
            {
                if (factor.IsNum())
                {
                    //搜索产品编号
                    string sqlProNo = "select top 1 * from [Product] where [No]={0} and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comProNo = new OleDbCommand(sqlProNo, Config.con);
                    OleDbDataAdapter adapterProNo = new OleDbDataAdapter(comProNo);
                    adapterProNo.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadProSelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
                }
                else
                {
                    //搜索产品名称
                    string sqlProName = "select top 1 * from [Product] where [ProName] like '%{0}%' and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comProName = new OleDbCommand(sqlProName, Config.con);
                    OleDbDataAdapter adapterProName = new OleDbDataAdapter(comProName);
                    adapterProName.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadProSelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
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
                btnSearchPro_Click(sender, e);
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
            string sqlLast = "select top 1 * from [Combination] where [UserId]='{0}' order by [LastPayAt] desc".FormatStr(Config.User._id.ToString());
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
            string sql = "select * from [Combination] where [UserId]='{0}'".FormatStr(Config.User._id.ToString());
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
        DataTable DtProInComb = new DataTable();

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
            DtProInComb.Clear();
            adapter.Fill(DtProInComb);
        }

        //修改套餐内产品信息
        private void btnEditProInComb_Click(object sender, EventArgs e)
        {
            EditProInComb edit = new EditProInComb(DtProInComb);
            edit.ReturnProList += this.SetProInCmob;
            edit.ShowDialog();
        }

        /// <summary>
        /// 重设套餐内产品列表
        /// </summary>
        /// <param name="dt"></param>
        public void SetProInCmob(DataTable dt)
        {
            DtProInComb = dt.Copy();
            lbPro.DataSource = DtProInComb;
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

            foreach (DataRow row in DtProInComb.Rows)
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
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(factor))
            {
                if (factor.IsNum())
                {
                    //搜索编号
                    string sqlCombNo = "select top 1 * from [Combination] where [No]={0} and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comCombNo = new OleDbCommand(sqlCombNo, Config.con);
                    OleDbDataAdapter adapterCombNo = new OleDbDataAdapter(comCombNo);
                    adapterCombNo.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadCombSelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
                }
                else
                {
                    //搜索名称
                    string sqlCombName = "select top 1 * from [Combination] where [CombName] like '%{0}%' and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    OleDbCommand comCombName = new OleDbCommand(sqlCombName, Config.con);
                    OleDbDataAdapter adapterCombName = new OleDbDataAdapter(comCombName);
                    adapterCombName.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadCombSelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
                }
            }
            else
            {
                MessageBoxEx.Show("搜索条件不能为空！", "提示");
            }
        }

        private void txtSearchComb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearchComb_Click(sender, e);
            }
        }
        #endregion

        //标签页切换时加载
        bool IsInitVip = false;
        bool IsInitPro = false;
        bool IsInitComb = false;
        bool IsInitShop = false;
        bool IsInitPay = false;
        private void tabMain_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tabVip&&!IsInitVip)
            {
                InitDgvVip();
                InitFirstVip();
                IsInitVip = true;
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

            if (e.TabPage == tabShop && !IsInitShop)
            {
                InitShop();
                IsInitShop = true;
                return;
            }

            if (e.TabPage == tabPay && !IsInitPay)
            {
                InitDgvPay();
                InitFirstPay();
                IsInitPay = true;
                return;
            }
        }      

        #region 数据管理
        //备份数据
        private void btnBackupData_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "备份数据";
            save.Filter = "数据文件|*.mdb";
            if (save.ShowDialog() == DialogResult.OK)
            {
                string backupFilePath = save.FileName;
                FileInfo file = new FileInfo(Config.DbPath);
                file.CopyTo(backupFilePath, true);
            }
        }

        //恢复数据
        private void btnRecoverData_Click(object sender, EventArgs e)
        {
            OpenFileDialog recover = new OpenFileDialog();
            recover.Title = "恢复数据";
            recover.Filter = "数据文件|*.mdb";
            recover.Multiselect = false;
            if (recover.ShowDialog() == DialogResult.OK)
            {
                FileInfo oldFile = new FileInfo(Config.DbPath);
                Config.Dispose();
                oldFile.Delete();

                string backupFilePath = recover.FileName;
                FileInfo newFile = new FileInfo(backupFilePath);
                newFile.CopyTo(Config.DbPath);
                Config.Init();
            }
        }
        #endregion

        #region 商铺信息
        void InitShop()
        {
            picLicence.ImageLocation = Config.User.PicLicenUrl;
            picLogo.ImageLocation = Config.User.LogoUrl;
            txtShopAlipay.Text = Config.User.Alipay;
            txtShopEmail.Text = Config.User.Email;
            txtShopName.Text = Config.User.Name;
            txtShopNo.Text = Config.User.ShopNo;
            txtShopPhone.Text = Config.User.Phone;
            txtShopWeixin.Text = Config.User.Weixin;
            txtAddress.Text = Config.User.Address;
        }
        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtShopNo.Text) || string.IsNullOrEmpty(txtShopName.Text) || string.IsNullOrEmpty(txtShopPhone.Text)
                || string.IsNullOrEmpty(txtShopEmail.Text) || string.IsNullOrEmpty(txtShopWeixin.Text) || string.IsNullOrEmpty(txtShopAlipay.Text))
            {
                MessageBoxEx.Show("内容不能为空！", "提示");
            }
            bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(txtShopEmail.Text, @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?");
            if (!isEmail)
            {
                MessageBoxEx.Show("邮箱格式错误！", "提示");
            }

            try
            {
                var buider = Builders<UserMongo>.Filter;
                var filter = buider.Eq(x => x._id, Config.User._id);
                var col = MongoDBHelper.Instance.GetUser();
                var update = new UpdateDocument{{"$set",new QueryDocument{{"Address",txtAddress.Text},{"ShopNo",txtShopNo.Text},{"Name",txtShopName.Text},{"Phone",txtShopPhone.Text}
                ,{"Email",txtShopEmail.Text},{"Weixin",txtShopWeixin.Text},{"Alipay",txtShopAlipay.Text}}}};
                col.UpdateOne(filter, update);
                var user = col.Find(filter).FirstOrDefault();
                Config.User = user;
                InitShop();
            }
            catch
            {
                MessageBoxEx.Show("网络不佳,请重试！", "提示");
            }
            
        }
        #endregion

        #region 交易记录
        /// <summary>
        /// 加载最近一次支付的交易
        /// </summary>
        private void InitFirstPay()
        {
            string sqlLast = "select top 1 * from [Product] where [UserId]='{0}' order by [LastPayAt] desc".FormatStr(Config.User._id.ToString());
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
                LoadPaySelectedRow(dt.Rows[0]);
        }


        /// <summary>
        /// 支付信息数据表
        /// </summary>
        DataTable DtPay = new DataTable();
        /// <summary>
        /// 当前选中支付信息ID
        /// </summary>
        int CurPayID = 0;

        /// <summary>
        /// 初始化支付记录表
        /// </summary>
        private void InitDgvPay()
        {
            //第一次时设置数据表结构
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("VipName", System.Type.GetType("System.String"));
            dt.Columns.Add("ToalPrice", System.Type.GetType("System.Double"));
            dt.Columns.Add("CreateAt", System.Type.GetType("System.DateTime"));
            dt.Columns.Add("IsUseComb", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("PayPrice", System.Type.GetType("System.Double"));
            

            string sql = "select * from [PayRecord] where [UserId]='{0}'".FormatStr(Config.User._id.ToString());
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            DtPay.Clear();
            adapter.Fill(DtPay);
            foreach (DataRow oldRow in DtPay.Rows)
            {
                DataRow row = dt.NewRow();
                row["ID"] = Convert.ToInt32(oldRow["ID"]);
                row["VipName"] = oldRow["VipName"].ToString();
                row["ToalPrice"] = Convert.ToDouble(oldRow["ToalPrice"]);
                row["PayPrice"] = Convert.ToDouble(oldRow["PayPrice"]);
                row["CreateAt"] = (System.DateTime)oldRow["CreateAt"];
                row["IsUseComb"] = (System.Boolean)oldRow["IsUseComb"];
                dt.Rows.Add(row);
            }
            dgvPay.DataSource = dt;
        }

        private void dgvPay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //根据选中行刷新上方数据
            DataGridView dgt = (DataGridView)sender;
            DataGridViewRow dr = dgt.CurrentRow;
            int id = Convert.ToInt32(dr.Cells["PayID"].Value);
            DataRow[] rows = DtPay.Select("ID=" + id);
            LoadPaySelectedRow(rows[0]);
        }

        DataTable DtPayPro = new DataTable();

        /// <summary>
        /// 展示当前选中的支付记录行
        /// </summary>
        /// <param name="row"></param>
        private void LoadPaySelectedRow(DataRow row)
        {
            CurPayID = Convert.ToInt32(row["ID"]);
            txtPayVipNo.Text = row["VipNo"].ToString();
            txtPayVipName.Text = row["VipName"].ToString();
            txtTotalPrice.Text = row["TotalPrice"].ToString();
            txtPayPrice.Text = row["PayPrice"].ToString();
            dtpPayAt.Value = (DateTime)row["CreateAt"];

            //判断是否使用套餐
            bool isUseComb = (System.Boolean)row["IsUseComb"];
            if (isUseComb)
            {
                rbUseComb.Checked = true;
                txtPayVipComb.Text = row["CombSnapName"].ToString();
            }
            else
            {
                rbUnUseComb.Checked = true;
                txtPayVipComb.Text = null;
            }

            //加载已使用支付记录列表
            string sqlPay = "select [ID],[ProName] from [ProSnap] where [PayID] ={0} ".FormatStr(CurPayID);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPay, Config.con);
            DtPayPro.Clear();
            adapter.Fill(DtPayPro);
        }

        //搜索支付记录信息
        private void btnSearchPay_Click(object sender, EventArgs e)
        {
            string factor = txtSearchPay.Text;
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(factor))
            {
                if (factor.IsNum())
                {
                    //搜索支付记录会员编号
                    string sqlPayNo = "select top 1 * from [PayRecord] where [VipNo]={0} and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    if (cbFilterPayAt.Checked)
                    {
                        DateTime timeFactor=dtpSearchPayAt.Value.Date;
                        sqlPayNo += " and [CreateAt]>#{0}# and [CreateAt]<#{1}".FormatStr(timeFactor, timeFactor.AddDays(1).AddSeconds(-1));
                    }
                    sqlPayNo += " order by [CreateAt] desc";
                    OleDbCommand comPayNo = new OleDbCommand(sqlPayNo, Config.con);
                    OleDbDataAdapter adapterPayNo = new OleDbDataAdapter(comPayNo);
                    adapterPayNo.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadPaySelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
                }
                else
                {
                    //搜索支付记录名称
                    string sqlPayName = "select top 1 * from [PayRecord] where [VipName] like '%{0}%' and [UserId]='{1}'".FormatStr(factor, Config.User._id.ToString());
                    if (cbFilterPayAt.Checked)
                    {
                        DateTime timeFactor = dtpSearchPayAt.Value.Date;
                        sqlPayName += " and [CreateAt]>#{0}# and [CreateAt]<#{1}".FormatStr(timeFactor, timeFactor.AddDays(1).AddSeconds(-1));
                    }
                    sqlPayName += " order by [CreateAt] desc";
                    OleDbCommand comPayName = new OleDbCommand(sqlPayName, Config.con);
                    OleDbDataAdapter adapterPayName = new OleDbDataAdapter(comPayName);
                    adapterPayName.Fill(dt);
                    if (dt.Rows.Count > 0)
                        LoadPaySelectedRow(dt.Rows[0]);
                    else
                        MessageBoxEx.Show("无符合条件数据！", "提示");
                }
            }
            else
            {
                MessageBoxEx.Show("搜索条件不能为空！", "提示");
            }

        }

        private void txtSearchPay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSearchPay_Click(sender, e);
            }
        }
        #endregion

    }
}
