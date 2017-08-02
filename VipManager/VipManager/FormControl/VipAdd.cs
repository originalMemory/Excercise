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
using System.Text.RegularExpressions;

namespace VipManager.FormControl
{
    public partial class VipAdd : Form
    {
        

        /// <summary>
        /// 套餐表
        /// </summary>
        DataTable DtComb = new DataTable();

        public VipAdd()
        {
            InitializeComponent();
            //获取原有会员编号，计算新会员编号
            string sqlVip = "select top 1 [No] from [Vip] order by [No] desc";
            OleDbCommand comVip = new OleDbCommand(sqlVip, Config.con);
            OleDbDataReader reader = comVip.ExecuteReader();
            int maxNo = 0;
            if (reader.Read())
            {
                maxNo = reader.GetInt32(0);
            }
            maxNo++;
            txtNo.Text = maxNo.ToString();

            //获取套餐信息，绑定下拉框
            string sqlComb = "select * from [Combination]";
            OleDbCommand comComb = new OleDbCommand(sqlComb, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(comComb);
            adapter.Fill(DtComb);
            cbVipComb.DataSource = DtComb;
            cbVipComb.DisplayMember = "CombName";
            cbVipComb.ValueMember = "ID";

            cbGender.Text = "男";

            //绑定生日下拉框
            cbMonth.DataSource = Config.MonthAr;
            InitCbDay(Config.MonthAr[0]);
            //绑定年龄段下拉框
            cbAge.DataSource = Config.AgeRangeList;
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

            //输入框限制事件
            txtPhone.SkinTxt.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);
        }

        /// <summary>
        /// 初始化天数下拉框
        /// </summary>
        /// <param name="month"></param>
        private void InitCbDay(int month)
        {
            var dayList=new List<int>();
            for (int i = 0; i < Config.DayLimitAr[month - 1]; i++)
            {
                dayList.Add(i + 1);
            }
            cbDay.DataSource = dayList;
        }

        //根据会员类型激活或禁用不同选项
        private void cbType_TextChanged(object sender, EventArgs e)
        {
            ComboBox type = (ComboBox)sender;

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
            string userId="1";
            string birth = cbMonth.Text + "/" + cbDay.Text;

            //插入会员信息
            string sqlAddVip = @"insert into [Vip]([No],[VipName],[Gender],[Phone],[Birth],[Balance],[CreateAt],[LastPayAt],[PayNum],[UserId]
,[AgeRange],[FaceType],[HairColor],[HairQuality],[HairDensity],[HairLossTrend],[Height],[BodySize],[SkinColor],[Profession],[SexDress]) values(
{0},'{1}','{2}','{3}','{4}',{5},#{6}#,#{7}#,{8},'{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}')"
                .FormatStr(txtNo.Text, txtName.Text, cbGender.Text, txtPhone.Text, birth, 0, DateTime.Now, DateTime.MinValue, 0, userId,
                cbAge.Text, cbFaceType.Text, cbHairColor.Text, cbHairQuality.Text, cbHairDensity.Text, cbHairLossTrend.Text, cbHeight.Text, cbBodySize.Text,
                cbSkinColor.Text, cbProfession.Text, cbSexDress.Text);
            OleDbCommand comAddVip = new OleDbCommand(sqlAddVip, Config.con);
            comAddVip.ExecuteNonQuery();

            //获取插入的会员编号
            string sqlGetVip = "select [ID] from [vip] where [no]={0}".FormatStr(txtNo.Text);
            OleDbCommand combGetVip = new OleDbCommand(sqlGetVip, Config.con);
            OleDbDataReader readerVip = combGetVip.ExecuteReader();
            int vipID = 0;
            if (readerVip.Read())
            {
                vipID = Convert.ToInt32(readerVip[0]);
            }
            readerVip.Close();

            //插入套餐映射
            int combID = Convert.ToInt32(cbVipComb.SelectedValue);
            var combRows = DtComb.Select("ID=" + combID);
            DataRow combRow = combRows[0];
            int payNum = Convert.ToInt32(combRow["PayNum"]) + 1;
            DateTime startAt = DateTime.Now;
            int timeRange = Convert.ToInt32(combRow["TimeRange"]);
            DateTime endAt = startAt.AddMonths(timeRange);
            string sqlAddCombSnap = @"insert into [CombSnap]([No],[CombName],[Description],[CreateAt],[UserId],[LastPayAt],[PayNum],[Type],[Price],
[Num],[Discount],[IsDel],[StartAt],[EndAt],[VipID],[CombID],[ProIDs]) values(
{0},'{1}','{2}',#{3}#,'{4}',#{5}#,{6},{7},{8},{9},{10},{11},#{12}#,#{13}#,{14},'{15}')"
                .FormatStr(combRow["No"], combRow["CombName"], combRow["Description"], DateTime.Now, combRow["UserId"], combRow["LastPayAt"], payNum
                , combRow["Type"], combRow["Price"], combRow["Num"], combRow["Discount"], false, startAt, endAt, vipID, combID, combRow["ProIDs"]);
            OleDbCommand comAddCombSnap = new OleDbCommand(sqlAddCombSnap, Config.con);
            comAddCombSnap.ExecuteNonQuery();

            //更新套餐使用信息
            string sqlUpComb = @"update [Combination] set [LastPayAt]=#{0}#,[PayNum]={1} where [ID]={2}".FormatStr(DateTime.Now, payNum, combID);
            OleDbCommand comUpComb = new OleDbCommand(sqlUpComb, Config.con);
            comUpComb.ExecuteNonQuery();

            MessageBoxEx.Show("会员添加成功！", "提示");
            this.Close();
        }

        //随月份变化变化天数
        private void cbMonth_TextChanged(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(((ComboBox)sender).Text);
            InitCbDay(month);
        }

    }
}
