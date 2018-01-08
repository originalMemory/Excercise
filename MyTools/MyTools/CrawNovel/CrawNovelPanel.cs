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
using MyTools.Helper;
using System.Threading;

namespace MyTools.CrawNovel
{
    public partial class CrawNovelPanel : Form
    {
        public CrawNovelPanel()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(DownNovels).Start();
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
            //string fileNameCheck = "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]";
            //string pathCheck = "";
            string path = "";
            switch (novel.Kind)
            {
                case NovelWebKind.Diyibanzhu:
                    //pathCheck = Regex.Replace(novel.Title + " - " + novel.Author, fileNameCheck, "_");
                    path = @"D:\Program Files (x86)\DAEMON Tools Lite\bin\和谐文\新建文件夹\";
                    break;
            }
            txt_FloderPath.Text = path;

            //获取章节正文
            bar.Value = 0;
            DownStart = true;
        }

        /// <summary>
        /// 判断是否开始获取文章
        /// </summary>
        bool DownStart = false;

        /// <summary>
        /// 下载小说正文
        /// </summary>
        void DownNovels()
        {
            
            while (true)
            {
                if (DownStart)
                {
                    txt_main.Text = "";
                    for (int i = 0; i < novel.Urls.Count; i++)
                    {
                        txt_main.Text += novel.ChapterNames[i] + System.Environment.NewLine + GetChapter(novel.Urls[i]) + System.Environment.NewLine + System.Environment.NewLine;
                        int percent = (int)((float)(i + 1) / novel.Urls.Count * 100);
                        bar.Value = percent;
                    }
                    DownStart = false;
                    Thread.Sleep(1000);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            
        }

        /// <summary>
        /// 获取章节内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetChapter(string url)
        {
            string novel = "";
            int num = 1;
            while (true)
            {
                //获取网页
                string respHtml = WebApi.GetHtml(url, "gb2312");
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(respHtml);
                HtmlNode task = doc.DocumentNode.SelectSingleNode("//div[@class=\"box_box\"]");
                if (task == null)
                    return "";
                string temp = task.InnerHtml;
                if (string.IsNullOrEmpty(temp))
                    return "";
                else
                {
                    //移除无用标签
                    Regex regDel1 = new Regex("<script.*?</script>|<tr([\\s\\S]*?)</tr>|<div.*?>|</div.*?>");
                    temp = regDel1.Replace(temp, "");
                    temp = temp.HtmlDiscode();
                    
                    novel += temp;
                    //获取下一分页列表
                    HtmlNodeCollection tasks = doc.DocumentNode.SelectNodes("//div[@class=\"chapterPages\"]/a");
                    if (tasks == null || num >= tasks.Count)
                        break;
                    string newName = tasks[num].Attributes["href"].Value;
                    string oldName = Path.GetFileName(url);
                    url = url.Replace(oldName, "") + newName;
                    num++;
                    //Thread.Sleep(500);
                }
            }

            Regex regDel2 = new Regex(@"= 第壹版主([\s\S]*?) ｑｑ.ｃōｍ|= 第壹版主([\s\S]*?)īn|【第[一壹]版主.*?】|<font.*>|</font.*>");
            novel = regDel2.Replace(novel, "");
            

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

                if (Regex.IsMatch(txt, "第.+章.{0,50}"))
                {
                    i++;
                    continue;
                }

                if (Regex.IsMatch(txt, "＊＊＊　"))
                {
                    i++;
                    continue;
                }
                
                if (i == txtList.Count - 1) break;

                //╮找?回╘网ξ址?请ㄨ百喥▼索∴弟?—╮板?zんù¤综╝合◆社╕区
                Regex regDel3 = new Regex("网.+址.+.社.区|地.+址.+.社.区|第.一.版.主|小.说.+版.主|０１ｂｚ|快.看.更.新|〇１Вｚ．ｎеｔ|ｄｉｙｉｂａｎｚｈｕ|最.新.地.址");
                string nextTxt = txtList[i + 1];
                if (regDel3.IsMatch(txt))
                {
                    txtList.RemoveAt(i);
                    continue;
                }

                //判断是否为一句结束符
                char end = txtList[i][txtList[i].Length - 1];
                if (end == '。' || end == '」' || end == '】' || end == '！' || end == '”' || end == '…' || end == '？')
                {
                    i++;
                    continue;
                }
                else
                {
                    txtList[i] += txtList[i + 1];
                    txtList.RemoveAt(i + 1);
                }
            }
            
            string str = "";
            for (int i = 0; i < txtList.Count; i++)
            {
                str += "    " + txtList[i] + Environment.NewLine + Environment.NewLine;
            }

            return str;
        }

        //保存小说
        private void btn_SaveNovel_Click(object sender, EventArgs e)
        {
            //替换无法作为文件名的字符
            Regex regName = new Regex("[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]");
            string fileName = "{0} - {1}.txt".FormatStr(novel.Title, novel.Author);
            fileName = regName.Replace(fileName, "_");

            string dir = txt_FloderPath.Text;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            FileStream fs = new FileStream(dir+fileName, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(txt_main.Text);
            sw.Flush();
            sw.Close();
            fs.Close();

            MessageBox.Show("保存完毕！", "提示");
            //重置打开文件夹按钮状态
            btn_OpenFloder.Enabled = true;

        }

        private void btn_OpenFloder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txt_FloderPath.Text);
        }
    }
}
