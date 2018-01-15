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
            switch (novel.Kind)
            {
                case NovelWebKind.Diyibanzhu:
                    Title.Text = Regex.Replace(novel.Title + " - " + novel.Author, @"&", @"&&") + "（" + novel.Urls.Count + "章）";
                    break;
                case NovelWebKind.Sexinsex:
                    Title.Text = Regex.Replace(novel.Title + " - " + novel.Author, @"&", @"&&") + "（" + novel.PageCount + "页）";
                    break;
                case NovelWebKind.CaoLiu:
                    Title.Text = Regex.Replace(novel.Title + " - " + novel.Author, @"&", @"&&") + "（" + novel.PageCount + "页）";
                    break;
                default:
                    break;
            }
            Title.Location = new Point((this.Width - 20 - Title.Width) / 2, Title.Location.Y);
            //重置打开文件夹按钮状态
            btn_OpenFloder.Enabled = false;
            btn_SaveNovel.Enabled = false;
            btn_GetNovel.Enabled = false;

            //检查目录和文件名，将不能使用字符替换为“_”
            //正则表达式"[\\u005C/:\\u002A\\u003F\"<>\'\\u007C’‘“”：？]"还包含中文的字符（实际上中文字符是可以使用的）
            //string fileNameCheck = "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]";
            //string pathCheck = "";
            string path = @"D:\Program Files (x86)\DAEMON Tools Lite\bin\和谐文\新建文件夹\";
            //switch (novel.Kind)
            //{
            //    case NovelWebKind.Diyibanzhu:
            //        //pathCheck = Regex.Replace(novel.Title + " - " + novel.Author, fileNameCheck, "_");
            //        path = @"D:\Program Files (x86)\DAEMON Tools Lite\bin\和谐文\新建文件夹\";
            //        break;
            //}
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
                        switch (novel.Kind)
                        {
                            case NovelWebKind.Diyibanzhu:
                                txt_main.Text += novel.ChapterNames[i] + System.Environment.NewLine + GetDiyiChapter(novel.Urls[i]) + System.Environment.NewLine + System.Environment.NewLine;
                                bar.Value = (int)((float)(i + 1) / novel.Urls.Count * 100);
                                break;
                            case NovelWebKind.Sexinsex:
                                GetSISChapter(novel.Urls[i]);
                                break;
                            case NovelWebKind.CaoLiu:
                                GetCLChapter(novel.Urls[i]);
                                break;
                            default:
                                break;
                        }
                    }
                    DownStart = false;
                    btn_SaveNovel.Enabled = true;
                    btn_GetNovel.Enabled = true;
                    Thread.Sleep(1000);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            
        }

        /// <summary>
        /// 获取第一版主章节内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        string GetDiyiChapter(string url)
        {
            string novel = "";
            int num = 1;
            while (true)
            {
                //获取网页
                string respHtml = WebApi.GetHtml(url);
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

            string str = SortNovel(novel, NovelWebKind.Diyibanzhu);
            

            return str;
        }

        /// <summary>
        /// 获取色中色帖子文章
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        void GetSISChapter(string url)
        {

            int page = 1;
            string newUrl = url;
            while (page <= novel.PageCount)
            {
                bar.Value = (int)((float)(page) / novel.PageCount * 100);
                string newHtml = WebApi.GetHtml(newUrl);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(newHtml);
                //获取当前页所有帖子
                HtmlNodeCollection floors = doc.DocumentNode.SelectNodes("//div[@class=\"mainbox viewthread\"]");

                //获取本页小说部分
                string content = "";
                for (int i = 0; i < floors.Count; i++)
                {
                    string tpCon = "";
                    HtmlNode floor = HtmlNode.CreateNode(floors[i].OuterHtml);
                    //获取楼主
                    var posterNode = floor.SelectSingleNode("//td[@class=\"postauthor\"]/cite/a");
                    string poster = "";
                    if (posterNode != null)
                    {
                        poster = posterNode.InnerText;
                    }
                    if (poster == novel.Poster)
                    {
                        HtmlNode conNode = floor.SelectSingleNode("//div[@class=\"t_msgfont\"]/div");
                        tpCon = conNode.InnerText;
                    }
                    else
                    {
                        //判断字数，如果小于500，则认为与作品无关，跳过
                        HtmlNode conNode = floor.SelectSingleNode("//div[@class=\"t_msgfont\"]/div");
                        if(conNode!=null)
                        {
                            string str = conNode.InnerText;
                            if (str.Length > 500)
                            {
                                tpCon = str;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tpCon))
                    {
                        tpCon = tpCon.HtmlDiscode();
                        string temp = SortNovel(tpCon, NovelWebKind.Sexinsex);
                        content += temp;
                    }
                }

                if (page < novel.PageCount)
                {
                    //获取页数
                    HtmlNode pagesNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[1]/div[5]/div[2]");
                    if (pagesNode != null)
                    {
                        //存在多页情况
                        int nowPageIndex = -1;    //当前所在页位置
                        for (int i = 0; i < pagesNode.ChildNodes.Count; i++)
                        {
                            var pageNode = pagesNode.ChildNodes[i];
                            //获取页数
                            string temp = pageNode.InnerText;
                            if (!string.IsNullOrEmpty(temp)&&temp.HtmlDiscode().IsNum())
                            {
                                //判断是否为下一页
                                int num = Convert.ToInt32(temp.HtmlDiscode());
                                if (num == page + 1)
                                {
                                    nowPageIndex = i;
                                    break;
                                }
                            }

                            ////判断是不是最后一页
                            //if (nowPageIndex == i - 1)
                            //{
                            //    break;
                            //}

                            ////判断是不是当前页
                            //if (!pageNode.HasAttributes)
                            //{
                            //    //判断是不是搜索框
                            //    string temp = pageNode.InnerText;
                            //    if (!string.IsNullOrEmpty(temp))
                            //    {
                            //        //判断是否是总帖数
                            //        int num = Convert.ToInt32(temp);
                            //        if (num <= novel.PageCount)
                            //        {
                            //            nowPageIndex = i;
                            //            break;
                            //        }
                            //    }
                            //}
                        }
                        string baseUrl = System.IO.Path.GetDirectoryName(url).Replace("\\", "/") + "/";
                        newUrl = baseUrl + pagesNode.ChildNodes[nowPageIndex].GetAttributeValue("href", "");
                        newUrl = newUrl.Replace("http:/", "http://");
                        newUrl = newUrl.Replace("https:/", "https://");
                    }
                }
                page++;
                if (content.Length > 0)
                    txt_main.Text += content + System.Environment.NewLine + System.Environment.NewLine;
            }
        }

        /// <summary>
        /// 获取草榴帖子文章
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        void GetCLChapter(string url)
        {

            int page = 1;
            string newUrl = url;
            string pageId = Path.GetFileNameWithoutExtension(url);
            string baseUrl = "http://cl.soze.pw/read.php?tid={0}&page=".FormatStr(pageId);
            while (page <= novel.PageCount)
            {
                bar.Value = (int)((float)(page) / novel.PageCount * 100);
                string newHtml = WebApi.GetHtml(newUrl);
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(newHtml);
                //获取当前页所有帖子
                HtmlNodeCollection floors = doc.DocumentNode.SelectNodes("//*[@class=\"t t2\"]");

                //获取本页小说部分
                string content = "";
                for (int i = 0; i < floors.Count; i++)
                {
                    string tpCon = "";
                    HtmlNode floor = HtmlNode.CreateNode(floors[i].OuterHtml);
                    //获取楼主
                    var posterNode = floor.SelectSingleNode("//th[@class=\"r_two\"]/b");
                    string poster = "";
                    if (posterNode != null)
                    {
                        poster = posterNode.InnerText;
                    }
                    if (poster == novel.Poster)
                    {
                        HtmlNode conNode = floor.SelectSingleNode("//div[@class=\"tpc_content do_not_catch\"]");
                        if (conNode == null)
                        {
                            conNode = floor.SelectSingleNode("//div[@class=\"tpc_content\"]");
                        }
                        tpCon = conNode.InnerText;
                    }
                    else
                    {
                        //判断字数，如果小于500，则认为与作品无关，跳过
                        HtmlNode conNode = floor.SelectSingleNode("//div[@class=\"tpc_content do_not_catch\"]");
                        if (conNode == null)
                        {
                            conNode = floor.SelectSingleNode("//div[@class=\"tpc_content\"]");
                        }
                        if (conNode != null)
                        {
                            string str = conNode.InnerText;
                            if (str.Length > 500)
                            {
                                tpCon = str;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(tpCon))
                    {
                        tpCon = tpCon.HtmlDiscode();
                        tpCon = Regex.Replace(tpCon, @"\[ 此貼.+\]", "");
                        string temp = SortNovel(tpCon, NovelWebKind.CaoLiu);
                        content += temp;
                    }
                }
                page++;
                if (content.Length > 0)
                    txt_main.Text += content + System.Environment.NewLine + System.Environment.NewLine;
                newUrl = baseUrl + page;
            }
        }

        /// <summary>
        /// 整理小说正文
        /// </summary>
        /// <param name="novel">小说正文</param>
        /// <param name="kind">小说网站类型</param>
        /// <returns>整理后的小说正文</returns>
        string SortNovel(string novel,NovelWebKind kind)
        {
            //将文章按行分列
            var source = novel.Replace("\r", "");
            var txtList = source.Split('\n').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            if (txtList.Count <= 1)
            {
                txtList = source.Split('　').Select(x => x.Trim()).Where(x => !String.IsNullOrEmpty(x)).ToList();
            }
            //合并异常换行
            //Regex regChapter = new Regex("(?<chapter>" + txt_regChapter.Text + ")\r\n");
            for (int i = 0; i < txtList.Count; )
            {
                string txt = txtList[i];
                Regex saveReg = new Regex("字数|作者|排版|首发|发表|日期");
                Regex dateReg = new Regex("(20\\d{2}[-/]\\d{1,2}[-/]\\d{1,2})|(20\\d{2}年\\d{1,2}月\\d{1,2}日)");
                if (i < 10 && (saveReg.IsMatch(txt) || dateReg.IsMatch(txt)))
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

                if (Regex.IsMatch(txt, "[＊*]"))
                {
                    i++;
                    continue;
                }

                switch (kind)
                {
                    case NovelWebKind.Diyibanzhu:
                        {
                            //╮找?回╘网ξ址?请ㄨ百喥▼索∴弟?—╮板?zんù¤综╝合◆社╕区
                            Regex regDel3 = new Regex("网.+址.+.社.区|地.+址.+.社.区|第.一.版.主|小.说.+版.主|０１ｂｚ|快.看.更.新|〇１Вｚ．ｎеｔ|ｄｉｙｉｂａｎｚｈｕ|最.新.地.址");
                            if (regDel3.IsMatch(txt))
                            {
                                txtList.RemoveAt(i);
                                continue;
                            }
                        }
                        break;
                    case NovelWebKind.Sexinsex:
                        {
                            Regex regDel = new Regex("本帖最后由.+编辑");
                            if (regDel.IsMatch(txt))
                            {
                                txtList.RemoveAt(i);
                                continue;
                            }
                        }
                        break;
                    case NovelWebKind.CaoLiu:
                        break;
                    default:
                        break;
                }

                //判断是否为一句结束符
                char end = txtList[i][txtList[i].Length - 1];
                if (end == '。' || end == '」' || end == '】' || end == '！' || end == '”'||end == '…' || end == '？')
                {
                    if (end == '…')
                    {
                        string temp = txtList[i];
                        Regex reg = new Regex("「|“|【");
                        if (!reg.IsMatch(temp))
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
                    else
                    {
                        i++;
                        continue;
                    }
                }
                else if (i < txtList.Count - 1)
                {
                    txtList[i] += txtList[i + 1];
                    txtList.RemoveAt(i + 1);
                }
                else
                {
                    i++;
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
            switch (novel.Kind)
            {
                case NovelWebKind.Diyibanzhu:
                    break;
                case NovelWebKind.Sexinsex:
                    if (string.IsNullOrEmpty(novel.Author))
                    {
                        fileName = "{0}.txt".FormatStr(novel.Title);
                    }
                    break;
                default:
                    break;
            }
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
