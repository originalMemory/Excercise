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
        int[] MonthAr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        int[] DayLimitAr = { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

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
            cbComb.DataSource = DtComb;
            cbComb.DisplayMember = "CombName";
            cbComb.ValueMember = "No";

            //绑定生日下拉框
            cbMonth.DataSource = MonthAr;
            InitCbDay(MonthAr[0]);

            //绑定年龄段下拉框
            List<string> ageRangeList = new List<string> { "儿童", "青年", "中年", "退休" };
            cbAge.DataSource = ageRangeList;
            //绑定脸型下拉框
            List<string> faceTypeList = new List<string> { "未知", "圆型", "长方形", "正方型", "三角型", "瓜子脸" };
            cbFaceType.DataSource = faceTypeList;
            //绑定发色下拉框
            List<string> hairColorList = new List<string> { "未知", "偏黑", "偏黄", "黑白混杂", "白发" };
            cbHairColor.DataSource = hairColorList;
            //绑定发质下拉框
            List<string> hairQualityList = new List<string> { "未知", "柔细", "自然卷", "偏硬" };
            cbHairQuality.DataSource = hairQualityList;
            //绑定浓密度下拉框
            List<string> hairDensityList = new List<string> { "未知", "稀疏", "中等", "浓密" };
            cbHairDensity.DataSource = hairDensityList;
            //绑定脱发倾向下拉框
            List<string> hairLossTrendList = new List<string> { "未知", "无", "有", "严重" };
            cbHairLossTrend.DataSource = hairLossTrendList;
            //绑定肤色下拉框
            List<string> skinColorList = new List<string> { "未知", "白皙", "中等", "较黑" };
            cbSkinColor.DataSource = skinColorList;
            //绑定身高下拉框
            List<string> heightList = new List<string> { "未知", "165以下", "165~175", "175以上" };
            cbHeight.DataSource = heightList;
            //绑定体型下拉框
            List<string> bodySizeList = new List<string> { "未知", "偏瘦", "中等", "偏胖", "较胖" };
            cbBodySize.DataSource = bodySizeList;
            //绑定性别打扮下拉框
            List<string> sexDressList = new List<string> { "未知", "男", "中性", "女" };
            cbSexDress.DataSource = sexDressList;
            //绑定职业下拉框
            List<string> professionList = new List<string> { "未知", "公务员", "企业主", "职员", "学生", "其他" };
            cbProfession.DataSource = professionList;
        }

        private void InitCbDay(int month)
        {
            var dayList=new List<int>();
            for (int i = 0; i < DayLimitAr[month - 1]; i++)
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
                .FormatStr(txtNo.Text, txtName.Text, cbGender.Text, txtPhone.Text, birth, 0, DateTime.MinValue, DateTime.MinValue, 0, userId,
                cbAge.Text, cbFaceType.Text, cbHairColor.Text, cbHairQuality.Text, cbHairDensity.Text, cbHairLossTrend.Text, cbHeight.Text, cbBodySize.Text,
                cbSkinColor.Text, cbProfession.Text, cbSexDress.Text);
            OleDbCommand comAddVip = new OleDbCommand(sqlAddVip, Config.con);
            comAddVip.ExecuteNonQuery();

            //插入套餐映射
            int combNo = Convert.ToInt32(cbComb.SelectedValue);
            var combRows = DtComb.Select("No=" + combNo);
            DataRow combRow = combRows[0];
            int payNum = Convert.ToInt32(combRow["PayNum"]) + 1;
            DateTime startAt = DateTime.Now;
            int timeRange = Convert.ToInt32(combRow["TimeRange"]);
            DateTime endAt = startAt.AddMonths(timeRange);
            string sqlAddCombSnap = @"insert into [CombSnap]([No],[CombName],[Description],[ProNos],[CreateAt],[UserId],[LastPayAt],[PayNum],[Type],[Price],
[Num],[Discount],[IsDel],[StartAt],[EndAt],[VipNo]) values(
{0},'{1}','{2}','{3}',#{4}#,'{5}',#{6}#,{7},{8},{9},{10},{11},{12},#{13}#,#{14}#,{15})"
                .FormatStr(combRow["No"], combRow["CombName"], combRow["Description"], combRow["ProNos"], DateTime.Now, combRow["UserId"], combRow["LastPayAt"], payNum
                , combRow["Type"], combRow["Price"], combRow["Num"], combRow["Discount"],false,startAt,endAt,txtNo.Text);
            OleDbCommand comAddCombSnap = new OleDbCommand(sqlAddCombSnap, Config.con);
            comAddCombSnap.ExecuteNonQuery();

            //更新套餐使用信息
            string sqlUpComb = @"update [Combination] set [LastPayAt]=#{0}#,[PayNum]={1} where [No]={2}".FormatStr(DateTime.Now, payNum, combNo);
            OleDbCommand comUpComb = new OleDbCommand(sqlUpComb, Config.con);
            comUpComb.ExecuteNonQuery();

            //获取套餐内产品信息
            DataTable dtPro = new DataTable();
            string sqlGetPro = "select * from [Product] where [No] in({0})".FormatStr(combRow["ProNos"]);
            OleDbDataAdapter adapterPro = new OleDbDataAdapter(sqlGetPro, Config.con);
            adapterPro.Fill(dtPro);

            foreach (DataRow row in dtPro.Rows)
            {
                //插入产品映射
                string sqlAddPro = @"insert into [ProSnap]([No],[ProName],[ProDesc],[Price],[CreateAt],[LastPayAt],[UserId],[PayNum],[IsDel],[VipNo],[CombSnapNo]) values(
{0},'{1}','{2}',{3},#{4}#,#{5}#,'{6}',{7},{8},{9},{10})"
                    .FormatStr(row["No"], row["ProName"], row["ProDesc"], row["Price"], DateTime.Now, DateTime.Now, row["UserId"], Convert.ToInt32(row["PayNum"]) + 1, false, txtNo.Text, combNo);
                OleDbCommand comAddPro = new OleDbCommand(sqlAddPro, Config.con);
                comAddPro.ExecuteNonQuery();
            }



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
