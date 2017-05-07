using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyTools.FormatConvert
{
    public partial class FormatConvertPanel : Form
    {
        public FormatConvertPanel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pos = 0;
            string source = textBox1.Text;
            string target="";
            source = source.Replace("\r", "");
            string prefix;
            string con;
            var strs = source.Split('\n').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            for (int i = 0; i < strs.Count; i++)
            {
                if (strs[i].Length <= 0)
                    continue;
                //去除注释
                pos = strs[i].IndexOf('/');
                if(pos!=-1)
                    strs[i] = strs[i].Substring(0, pos);
                //去除空格
                strs[i] = strs[i].Trim();
                //将前缀转为后缀
                pos = strs[i].IndexOf(' ');
                if (pos != -1)
                {
                    prefix = strs[i].Substring(0, pos);
                    con = strs[i].Substring(pos + 1, strs[i].Length - pos - 1).Replace(";", " : ");
                }
                else
                {
                    prefix = "";
                    con = strs[i].Replace(";", "");
                }
                //添加属性
                if (checkBox1.Checked)
                {
                    target += "-";
                }
                else
                {
                    target += "+";
                }
                target += con + prefix + System.Environment.NewLine;
            }
            textBox1.Text = target;
        }
    }
}
