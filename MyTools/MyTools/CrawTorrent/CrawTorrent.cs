using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTools.CrawTorrent
{
    public partial class CrawTorrent : Form
    {
        public CrawTorrent()
        {
            InitializeComponent();
        }

        #region 下载种子文件
        private void btn_down_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region 打开保存文件夹事件
        private void btn_SaveFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txt_path.Text);
        }
        #endregion
    }
}
