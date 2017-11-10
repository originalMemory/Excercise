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
                        novel = ExtractDiyi(url, respHtml);
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

        NovelInfo ExtractDiyi(string url,string html)
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

            string fileName = System.IO.Path.GetFileName(url);
            novel.Urls = new List<string>();
            novel.ChapterNames = new List<string>();
            foreach (var x in tasks)
            {
                var info = HtmlNode.CreateNode(x.OuterHtml);
                string chapterUrl = url.Replace(fileName,"") + info.FirstChild.Attributes["href"].Value;
                novel.Urls.Add(chapterUrl);
                novel.ChapterNames.Add(info.InnerText);
            }
            

            return novel;
        }

    }
}
