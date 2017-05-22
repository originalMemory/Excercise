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
using NReadability;

namespace FormTest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            var transcoder = new NReadabilityWebTranscoder();
            bool success; string transcodedContent =
               transcoder.Transcode("http://news.163.com/17/0522/16/CL294BVU000187VE.html", out success);
            richTextBox1.Text = transcodedContent;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string html = GetMainContentHelper.getDataFromUrl(textBox1.Text);
            string data = GetMainContentHelper.GetMainContent(html);
            richTextBox1.Text = data;
        }
    }
}
