﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using VipManager.Helper;

namespace VipManager.FormControl
{
    public partial class EditVipComb : Form
    {
        /// <summary>
        /// 套餐内产品列表
        /// </summary>
        DataTable ProInComb = new DataTable();

        /// <summary>
        /// 所有套餐数据表
        /// </summary>
        DataTable DtComb = new DataTable();

        /// <summary>
        /// 会员Id
        /// </summary>
        public int VipID;

        public delegate void SetCombName(string combName);
        public event SetCombName setCombName;

        public EditVipComb()
        {
            InitializeComponent();
            InitCbComb();
        }

        /// <summary>
        /// 初始化套餐列表下拉框
        /// </summary>
        private void InitCbComb()
        {
            string sqlComb = "select [ID],[CombName],[ProIDs] from [Combination] where [UserId]='{0}'".FormatStr(Config.User._id.ToString());
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComb, Config.con);
            adapter.Fill(DtComb);
            cbComb.DisplayMember = "CombName";
            cbComb.ValueMember = "ID";
            cbComb.DataSource = DtComb;
        }

        //保存修改
        private void btnSaveVipComb_Click(object sender, EventArgs e)
        {
            //获取当前会员套餐编号
            int combID = Convert.ToInt32(cbComb.SelectedValue);
            //将原有套餐映射和产品映射标记为删除
            string sqlDelCombSnap = "update [CombSnap] set [IsDel]={0},[DelAt]=#{1}# where [VipID]={2}".FormatStr(true, DateTime.Now, VipID);
            OleDbCommand combDelComb = new OleDbCommand(sqlDelCombSnap, Config.con);
            combDelComb.ExecuteNonQuery();

            //获取套餐信息
            string sqlGetComb = "select * from [Combination] where [ID]={0}".FormatStr(combID);
            OleDbCommand comGetComb = new OleDbCommand(sqlGetComb, Config.con);
            OleDbDataReader readerComb = comGetComb.ExecuteReader();
            if (readerComb.Read())
            {
                //插入新的套餐映射
                int payNum = Convert.ToInt32(readerComb["PayNum"]) + 1;
                DateTime startAt = DateTime.Now;
                int timeRange = Convert.ToInt32(readerComb["TimeRange"]);
                DateTime endAt = startAt.AddMonths(timeRange);
                string sqlAddCombSnap = @"insert into [CombSnap]([No],[CombName],[Description],[CreateAt],[UserId],[LastPayAt],[PayNum],[Type],[Price],
[Num],[Discount],[IsDel],[StartAt],[EndAt],[VipID],[CombID],[ProIDs]) values(
{0},'{1}','{2}',#{3}#,'{4}',#{5}#,{6},{7},{8},{9},{10},{11},#{12}#,#{13}#,{14},{15},'{16}')"
                    .FormatStr(readerComb["No"], readerComb["CombName"], readerComb["Description"], DateTime.Now, readerComb["UserId"], readerComb["LastPayAt"], payNum
                    , readerComb["Type"], readerComb["Price"], readerComb["Num"], readerComb["Discount"], false, startAt, endAt, VipID, combID, readerComb["ProIDs"]);
                OleDbCommand comAddCombSnap = new OleDbCommand(sqlAddCombSnap, Config.con);
                comAddCombSnap.ExecuteNonQuery();
                readerComb.Close();

                //更新套餐使用信息
                string sqlUpComb = @"update [Combination] set [LastPayAt]=#{0}#,[PayNum]={1} where [ID]={2}".FormatStr(DateTime.Now, payNum, combID);
                OleDbCommand comUpComb = new OleDbCommand(sqlUpComb, Config.con);
                comUpComb.ExecuteNonQuery();
            }
            setCombName(cbComb.Text);
            this.Close();
        }

        private void cbComb_TextChanged(object sender, EventArgs e)
        {
            int combID = Convert.ToInt32(cbComb.SelectedValue);
            DataRow[] rows = DtComb.Select("ID=" + combID);

            string sqlComb = "select * from [Product] where [ID] in({0})".FormatStr(rows[0]["ProIDs"]);
            OleDbDataAdapter adapter = new OleDbDataAdapter(sqlComb, Config.con);
            ProInComb.Clear();
            adapter.Fill(ProInComb);
            lbPro.DataSource = ProInComb;
            lbPro.DisplayMember = "ProName";
            lbPro.ValueMember = "ID";
        }

    }
}
