using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Text.RegularExpressions;
using MyTools.Tools;
using HtmlAgilityPack;

namespace MyTools.CrawNovel
{
    public class GetNovel
    {
        /// <summary>
        /// 获取小说信息及章节链接
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns></returns>
        public NovelInfo GetNovelUrls(string url)
        {
            var novel = new NovelInfo();
            try
            {
                //获取网页
                string respHtml = WebApi.GetHtml(url, "gb2312");
                //对网页进行分类
                novel.Kind = NovelWebKind.Diyibanzhu;
                switch (novel.Kind)
                {
                    case NovelWebKind.Diyibanzhu:
                        novel = ExtractDiyiChapter(url, respHtml);
                        break;
                }

                return novel;
            }
            catch (Exception ex)
            {
                novel.Error = ex.Message;
                return novel;
            }
        }

        /// <summary>
        /// 抽取第一版主章节列表
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <param name="html">网页源码</param>
        /// <returns></returns>
        NovelInfo ExtractDiyiChapter(string url,string html)
        {
            var novel = new NovelInfo();
            //获取文章名称与作者
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode task = doc.DocumentNode.SelectSingleNode("//div[@class=\"j_box\"]");
            if (task == null)
                return null;
            var authInfo = HtmlNode.CreateNode(task.OuterHtml);
            novel.Title = authInfo.SelectSingleNode("/div/div[1]").InnerText.Trim();
            novel.Author = authInfo.SelectSingleNode("/div/div[2]/ul/li[1]/a").InnerText.Trim();

            //获取章节列表
            HtmlNodeCollection tasks = doc.DocumentNode.SelectNodes("//div[@class=\"list_box\"]/ul/li");
            if (tasks == null)
                return null;

            string fileName = System.IO.Path.GetFileName(url);      //原页面名
            novel.Urls = new List<string>();
            novel.ChapterNames = new List<string>();
            foreach (var x in tasks)
            {
                var info = HtmlNode.CreateNode(x.OuterHtml);
                //<li><a title=【我的性奴家族】（1-3） href="108732.html">【我的性奴家族】（1-3）</a></li>
                string chapterUrl = url.Replace(fileName,"") + info.FirstChild.Attributes["href"].Value;    //下一章节页面名称
                //<li><a href="javascript:;" onclick="tourl('114593.html')">【我的性奴家族】（5）</a></li>
                if (chapterUrl.Contains("javascript"))
                {
                    string temp = info.FirstChild.Attributes["onclick"].Value;
                    Regex reg = new Regex("[0-9]*.html");
                    var str = reg.Match(temp);
                    chapterUrl = url.Replace(fileName, "") + str.Value;
                }
                novel.Urls.Add(chapterUrl);
                novel.ChapterNames.Add(info.InnerText);
            }
            

            return novel;
        }

    }
}
