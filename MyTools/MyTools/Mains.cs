using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MyTools.CrawImg;
using MyTools.TextEdit;
using MyTools.SelectImg;
using MyTools.RelistImg;
using MyTools.FormatConvert;

namespace MyTools
{
    public partial class Mains : Form
    {
        public Mains()
        {
            InitializeComponent();
        }

        private void btn_CrawImg_Click(object sender, EventArgs e)
        {
            CrawImgPanel CrawImg = new CrawImgPanel();
            CrawImg.Show();
        }

        private void btn_TextEdit_Click(object sender, EventArgs e)
        {
            TextEditPanel textEdit = new TextEditPanel();
            textEdit.Show();
        }

        private void btn_SelectImg_Click(object sender, EventArgs e)
        {
            SelectImgPanel selectImg = new SelectImgPanel();
            selectImg.Show();
        }

        private void Mains_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
        }

        private void btn_RelistImg_Click(object sender, EventArgs e)
        {
            RelistImgWin relist = new RelistImgWin();
            relist.Show();
        }

        private void btn_dmhyCraw_Click(object sender, EventArgs e)
        {
            MyTools.CrawTorrent.CrawTorrentPanel torrent = new CrawTorrent.CrawTorrentPanel();
            torrent.Show();
        }

        private void btn_CrawNovel_Click(object sender, EventArgs e)
        {
            CrawNovel.CrawNovelPanel novel = new CrawNovel.CrawNovelPanel();
            novel.Show();
        }
    }
}
