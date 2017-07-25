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

namespace VipManager.FormControl
{
    public partial class EditProInComb : Form
    {

        /// <summary>
        /// 套餐内产品列表
        /// </summary>
        DataTable ProInComb = new DataTable();

        /// <summary>
        /// 所有产品数据表
        /// </summary>
        DataTable DtPro = new DataTable();

        public delegate void SetDataSource(DataTable dt);
        public event SetDataSource ReturnProList;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="proInComb">套餐内产品列表</param>
        public EditProInComb(DataTable proInComb)
        {
            InitializeComponent();

            // 初始化产品列表下拉框
            InitCbPro();

            //初始化产品列表
            this.ProInComb = proInComb.Copy();
            this.lbEditPro.DataSource = this.ProInComb;
            this.lbEditPro.DisplayMember = "ProName";
            this.lbEditPro.ValueMember = "No";
        }

        /// <summary>
        /// 初始化产品列表下拉框
        /// </summary>
        private void InitCbPro()
        {
            string sqlPro = "select [No],[ProName],[Price] from [Product] where [IsDel]=false";
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlPro, Config.con);
            adapter.Fill(this.DtPro);
            this.cbPro.DataSource = this.DtPro;
            this.cbPro.DisplayMember = "ProName";
            this.cbPro.ValueMember = "No";
        }

        //添加产品
        private void btnAddProInComb_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(this.cbPro.SelectedValue);

            //检查该产品是否已添加
            DataRow[] rows = this.ProInComb.Select("No='{0}'".FormatStr(no));
            if (rows.Count() > 0)
            {
                MessageBoxEx.Show("该产品已添加！", "提示");
            }
            else
            {
                rows = this.DtPro.Select("No='{0}'".FormatStr(no));
                if (rows.Count() > 0)
                {
                    this.ProInComb.ImportRow(rows[0]);
                }
            }
        }

        //删除产品
        private void btnDelProInComb_Click(object sender, EventArgs e)
        {
            int no = Convert.ToInt32(lbEditPro.SelectedValue);
            DataRow[] rows = this.ProInComb.Select("No='{0}'".FormatStr(no));
            if (rows.Count() > 0)
            {
                this.ProInComb.Rows.Remove(rows[0]);
            }
        }

        /// <summary>
        /// 初始化产品列表
        /// </summary>
        /// <param name="dt"></param>
        public void InitPro(DataTable dt)
        {
            this.ProInComb = dt.Copy();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ReturnProList(this.ProInComb);
            this.Close();
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
