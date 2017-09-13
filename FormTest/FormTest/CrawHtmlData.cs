using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;
using System.Threading;

namespace FormTest
{
    public partial class CrawHtmlData : Form
    {
        public CrawHtmlData()
        {
            InitializeComponent();
            webBrowser1.Navigate("http://www.5118.com/seo/words/%E7%9C%9F%E7%88%B1%E6%A2%A6%E6%83%B3");
            //Thread.Sleep(3000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(webBrowser1.DocumentText);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@class=\"Fn-ui-list dig-list\"]");
            richTextBox1.Text = node.InnerText;
        }
    }
}
