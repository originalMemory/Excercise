using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.Threading;

namespace MyTools.TextEdit
{
    public partial class TextEditPanel : Form
    {
        public TextEditPanel()
        {
            InitializeComponent();
        }

        private void btn_merge_Click(object sender, EventArgs e)
        {
            new Thread(Merge).Start();
            BtnState(false);
        }

        /// <summary>
        /// 合并文章
        /// </summary>
        void Merge()
        {            
            string source = txt_main.Text;
            //匹配章节名
            List<string> chapterName = new List<string>();
            Regex regChapter = new Regex("(?<chapter>"+txt_regChapter.Text+")\r\n");
            MatchCollection mc = regChapter.Matches(source);
            foreach (Match x in mc)
            {
                string temp =x.Groups["chapter"].Value;
                chapterName.Add(temp);
                
            }
            if (chapterName.Count <= 0) MessageBox.Show("章节正则表达式错误，未匹配上章节名", "提示", MessageBoxButtons.OK);
            //将文章按行分列
            source = RemoveTarget(source).Replace("\r", "");
            var txtList = source.Split('\n').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            //合并异常换行
            for (int i = 0; i < txtList.Count; )
            {
                if (i == txtList.Count - 1) break;
                char end = txtList[i][txtList[i].Length - 1];
                if (end == '。' || end == '」' || end == '】' || end == '！' || end == '”' || end == '…' || end == '？')
                {
                    i++;
                    continue;
                }
                else
                {
                    txtList[i] += txtList[i + 1];
                    txtList.Remove(txtList[i + 1]);
                }
                bar.Value = i / (txtList.Count - 1) * 10;

            }
            //分行
            if (checkLine.Checked)
            {
                int oldpos = 0;
                for (int i = 0; i < txtList.Count; )
                {
                    if (oldpos > (txtList[i].Length - 1))
                        oldpos = 0;
                    int index = txtList[i].IndexOf('。', oldpos);
                    if (index == -1||index == (txtList[i].Length - 1))
                    {
                        i++;
                        continue;
                    }
                    if (txtList[i][index + 1] != '”' && txtList[i][index + 1] != '\'' && txtList[i][index + 1] != '」')
                    {
                        string newline = txtList[i].Substring(index + 1);
                        txtList[i] = txtList[i].Substring(0, index + 1);
                        txtList.Insert(++i, newline);
                        oldpos = 0;
                    }
                    else
                    {
                        if (index >= (txtList[i].Length - 2))
                        {
                            oldpos = 0;
                            i++;
                        }
                        else
                        {
                            oldpos = index + 1;
                        }
                    }

                }
                oldpos = 0;
                for (int i = 0; i < txtList.Count; )
                {
                    int index = txtList[i].IndexOf('”', oldpos);
                    if (index == -1 || index == (txtList[i].Length - 1))
                    {
                        i++;
                        continue;
                    }
                    if (txtList[i][index + 1] != '”' && txtList[i][index + 1] != '\'')
                    {
                        string newline = txtList[i].Substring(index + 1);
                        txtList[i] = txtList[i].Substring(0, index + 1);
                        txtList.Insert(++i, newline);
                        oldpos = 0;
                    }
                    else
                    {
                        if (index >= (txtList[i].Length - 2))
                        {
                            oldpos = 0;
                            i++;
                        }
                        else
                        {
                            oldpos = index + 1;
                        }
                    }
                }
                oldpos = 0;
                for (int i = 0; i < txtList.Count; )
                {
                    int index = txtList[i].IndexOf('’', oldpos);
                    if (index == -1 || index == (txtList[i].Length - 1))
                    {
                        i++;
                        continue;
                    }
                    if (txtList[i][index + 1] != '”' && txtList[i][index + 1] != '\'')
                    {
                        string newline = txtList[i].Substring(index + 1);
                        txtList[i] = txtList[i].Substring(0, index + 1);
                        txtList.Insert(++i, newline);
                        oldpos = 0;
                    }
                    else
                    {
                        if (index >= (txtList[i].Length - 2))
                        {
                            oldpos = 0;
                            i++;
                        }
                        else
                        {
                            oldpos = index + 1;
                        }
                    }
                }

                oldpos = 0;
                for (int i = 0; i < txtList.Count; )
                {
                    int index = txtList[i].IndexOf('」', oldpos);
                    if (index == -1 || index == (txtList[i].Length - 1))
                    {
                        i++;
                        continue;
                    }
                    if (txtList[i][index + 1] != '”' && txtList[i][index + 1] != '\'')
                    {
                        string newline = txtList[i].Substring(index + 1);
                        txtList[i] = txtList[i].Substring(0, index + 1);
                        txtList.Insert(++i, newline);
                        oldpos = 0;
                    }
                    else
                    {
                        if (index >= (txtList[i].Length - 2))
                        {
                            oldpos = 0;
                            i++;
                        }
                        else
                        {
                            oldpos = index + 1;
                        }
                    }
                }
            }
            //章节名单独一行
            string str = "";
            int num = 0;
            for (int i = 0; i < txtList.Count;i++ )
            {
                if (chapterName.Count > 0 && txtList[i].Contains(chapterName[num]))
                {
                    txtList[i]=txtList[i].Replace(chapterName[num], "");
                    if (num == 0) str += chapterName[num] + Environment.NewLine + "    " + txtList[i] + Environment.NewLine;
                    else str += Environment.NewLine + Environment.NewLine + chapterName[num] + Environment.NewLine + "    " + txtList[i] + Environment.NewLine;
                    if (num < chapterName.Count - 1) num++;
                }
                else { str += "    " + txtList[i] + Environment.NewLine + Environment.NewLine; }
                bar.Value = (int)((float)i / (txtList.Count - 1) * 90 + 10);
             //   bar.Value = (int)((float)i / (txtList.Count - 1) * 90 + 10);
            }
            txt_main.Text = null;
            txt_main.Text = str;
            BtnState(true);
        }

        /// <summary>
        /// 移除网页标签
        /// </summary>
        /// <param name="source">原文</param>
        /// <returns></returns>
        string RemoveTarget(string source)
        {
            Regex reg = new Regex("<div.+>{1}|</div>|<DIV.+>{1}|</DIV>|<BODY.+>{1}|</BODY>|<FONT.+>{1}|</FONT>");
            string result = reg.Replace(source, "");
            Regex reg2 = new Regex("<br>|<BR/>|<BR>|<br/>|<br />|<p>|</P>|<P>|</p>");
            result = reg2.Replace(result, "\r\n");
            result = result.Replace(" ", "");
            return result;
        }

        /// <summary>
        /// 替换功能
        /// </summary>
        /// <param name="search">查找字符串</param>
        /// <param name="replace">替换字符串</param>
        /// <param name="isReg">true为正则替换，false字符串替换</param>
        void ReplaceText(string search, string replace, bool isReg)
        {
            if (replace.Contains(@"\n"))
            {
                replace = replace.Replace(@"\n", Environment.NewLine);
            }
            string text = txt_main.Text;
            if (isReg)
            {
                Regex reg = new Regex(search);
                text = reg.Replace(text, replace);
            }
            else
            {
                text = text.Replace(search, replace);
            }
            txt_main.Text = null;
            txt_main.Text = text;
        }

        /// <summary>
        /// 添加空行
        /// </summary>
        /// <param name="source">原文</param>
        /// <returns></returns>
        string AddSpaceLine(string source)
        {
            source = RemoveTarget(source).Replace("\r", "");
            var txtList = source.Split('\n').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            for (int i = 1; i < txtList.Count; i++)
            {
                if (!string.IsNullOrEmpty(txtList[i]))
                {
                    string temp = Environment.NewLine;
                    txtList.Insert(i, temp);
                    i++;
                }
            }
            string result = "";
            foreach (var x in txtList)
            {
                if (x.Equals(Environment.NewLine))
                    result += x;
                else
                {
                    result += x + Environment.NewLine;
                }
            }
            return result;
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="finish"></param>
        void BtnState(bool finish)
        {
            if (finish)
            {
                btn_merge.Enabled = true;
                btn_regex.Enabled = true;
                btn_replace.Enabled = true;
            }
            else
            {
                btn_merge.Enabled = false;
                btn_regex.Enabled = false;
                btn_replace.Enabled = false;
            }
        }

        private void txtEdit_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btn_replace_Click(object sender, EventArgs e)
        {
            ReplaceText(txt_Search.Text, txt_replace.Text, false);
        }

        private void btn_regex_Click(object sender, EventArgs e)
        {
            ReplaceText(txt_Search.Text, txt_replace.Text, true);
        }

        private void btn_addSpaceLine_Click(object sender, EventArgs e)
        {
            txt_main.Text = AddSpaceLine(txt_main.Text);
        }
    }
}
