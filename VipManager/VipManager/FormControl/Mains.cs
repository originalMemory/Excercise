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

namespace VipManager.FormControl
{
    public partial class Mains : Skin_Color
    {
        public Mains()
        {
            InitializeComponent();
            string sql = "select * from [Vip]";
            OleDbCommand com = new OleDbCommand(sql, Config.con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(com);
            OleDbCommandBuilder cb = new OleDbCommandBuilder(adapter);
            adapter.InsertCommand = cb.GetInsertCommand();
            adapter.UpdateCommand = cb.GetUpdateCommand();
            adapter.DeleteCommand = cb.GetDeleteCommand();
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvVip.DataSource = dt;
        }

        private void Mains_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnAddVip_Click(object sender, EventArgs e)
        {
            VipAdd add = new VipAdd();
            add.ShowDialog();
        }

        private void Mains_Load(object sender, EventArgs e)
        {
            //默认加载最近一次支付的会员的信息
            string sqlLast = "select top 1 * from [Vip] order by [LastPayAt] desc";
            OleDbCommand com = new OleDbCommand(sqlLast, Config.con);
            OleDbDataReader reader = com.ExecuteReader();
            if (reader.Read())
            {
                txtNo.Text = reader["No"].ToString();
                txtName.Text = reader["VipName"].ToString();
                txtPhone.Text = reader["Phone"].ToString();
                dtpRegAt.Value = (DateTime)reader["CreateAt"];
                cbGender.Text = reader["Gender"].ToString();
                string type = reader["Type"].ToString();
                cbType.Text = type;
                switch (type)
                {
                    case "次数型":
                        SetNum();
                        txtDetail.Text = reader["RemainNum"].ToString();
                        break;
                    case "折扣型":
                        SetDiscount();
                        txtDetail.Text = reader["Discount"].ToString();
                        txtBalance.Text = reader["Balance"].ToString();
                        break;
                    case "时间型":
                        SetTime();
                        dtpStart.Value = (DateTime)reader["StartAt"];
                        dtpEnd.Value = (DateTime)reader["EndAt"];
                        break;
                    default:
                        break;
                }
            }
        }

        #region 控件显示状态
        /// <summary>
        /// 显示次数型数据
        /// </summary>
        private void SetNum()
        {
            labDetail.Text = "次数：";
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
        private void SetDiscount()
        {
            labDetail.Text = "折扣：";
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
        private void SetTime()
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
    }
}
