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
using MyTools.Tools;
using HtmlAgilityPack;
using System.IO;

namespace MyTools.CrawNovel
{
    public partial class CrawNovelPanel : Form
    {
        public CrawNovelPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 网页中解析到的小说信息
        /// </summary>
        NovelInfo novel = new NovelInfo();

        private void btn_GetNovel_Click(object sender, EventArgs e)
        {
            var getNovel = new GetNovel();
            string url = txt_Url.Text;
            novel = getNovel.GetNovelUrls(url);
            //判断抓取中是否遇到错误！
            if (!string.IsNullOrEmpty(novel.Error))
            {
                MessageBox.Show(novel.Error);
                return;
            }
            //获取标题并居中
            Title.Text = Regex.Replace(novel.Title + " - " + novel.Author, @"&", @"&&") + "（" + novel.Urls.Count + "章）";
            Title.Location = new Point((this.Width - 20 - Title.Width) / 2, Title.Location.Y);
            //重置打开文件夹按钮状态
            btn_OpenFloder.Enabled = false;

            //检查目录和文件名，将不能使用字符替换为“_”
            //正则表达式"[\\u005C/:\\u002A\\u003F\"<>\'\\u007C’‘“”：？]"还包含中文的字符（实际上中文字符是可以使用的）
            string fileNameCheck = "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]";
            string pathCheck = "";
            string path = "";
            switch (novel.Kind)
            {
                case NovelWebKind.Diyibanzhu:
                    pathCheck = Regex.Replace(novel.Title + " - " + novel.Author, fileNameCheck, "_");
                    path = @"D:\Program Files (x86)\DAEMON Tools Lite\bin\和谐文\新建文件夹\" + pathCheck;
                    break;
            }
            txt_FloderPath.Text = path;

            //获取章节正文
            string txtMain = "";
            for (int i = 0; i < novel.Urls.Count; i++)
            {
                txtMain += novel.ChapterNames[i] + System.Environment.NewLine + GetChapter(novel.Urls[i]) + System.Environment.NewLine + System.Environment.NewLine;
                
            }
            foreach (var x in novel.Urls)
            {
            }
            txt_main.Text = txtMain;
        }

        string GetChapter(string url)
        {
            string novel = "";
            int num = 0;
            while (true)
            {
                //获取网页
                string respHtml = WebApi.GetHtml(url, "utf-8");
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(respHtml);
                HtmlNode task = doc.DocumentNode.SelectSingleNode("//div[@class=\"box_box\"]");
                if (task == null)
                    break;
                string temp = task.InnerText;
                if (string.IsNullOrEmpty(temp))
                    break;
                else
                {
                    novel += temp;
                    //获取下一分页列表
                    HtmlNodeCollection tasks = doc.DocumentNode.SelectNodes("//div[@class=\"list_box\"]/ul/li");
                    if (tasks == null || num >= tasks.Count)
                        break;
                    string name = tasks[num].Attributes["href"].Value;
                    url = Path.GetDirectoryName(url) + name;
                    num++;
                }
            }
            //将文章按行分列
            var source = novel.Replace("\r", "");
            var txtList = source.Split('\n').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            //合并异常换行
            //Regex regChapter = new Regex("(?<chapter>" + txt_regChapter.Text + ")\r\n");
            for (int i = 0; i < txtList.Count; )
            {
                string txt = txtList[i];
                if (txt.Contains("字数") || txt.Contains("作者"))
                {
                    i++;
                    continue;
                }
                var publishAt = new DateTime();
                bool isTime = DateTime.TryParse(txt, out publishAt);
                if (isTime)
                {
                    i++;
                    continue;
                }
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
                    if (index == -1 || index == (txtList[i].Length - 1))
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
            string str = "";
            for (int i = 0; i < txtList.Count; i++)
            {
                str += "    " + txtList[i] + Environment.NewLine + Environment.NewLine;
            }

            return str;
        }
    }
}
