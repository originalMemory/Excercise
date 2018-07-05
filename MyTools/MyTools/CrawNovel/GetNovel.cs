using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Text.RegularExpressions;
using MyTools.Tools;
using HtmlAgilityPack;
using MyTools.Helper;

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
                //对网页进行分类
                Regex kindReg = new Regex(@"//.*?/");
                var match = kindReg.Match(url);
                string kindStr = match.Value;
                kindStr = kindStr.Replace("/", "");
                //获取网页
                string respHtml = WebApi.GetHtml(url);
                switch (kindStr)
                {
                    case "www.diyibanzhu.xyz":
                        novel = ExtractDiyiChapter(url, respHtml);
                        novel.Kind = NovelWebKind.Diyibanzhu;
                        break;
                    case "www.tangzhekan.net":
                        novel = ExtractDiyiChapter(url, respHtml);
                        novel.Kind = NovelWebKind.Diyibanzhu;
                        break;
                    case "sexinsex.net":
                        novel = ExtractSisChapter(url, respHtml);
                        novel.Kind = NovelWebKind.Sexinsex;
                        break;
                    case "cl.soze.pw":
                        novel = ExtractCLChapter(url, respHtml);
                        novel.Kind = NovelWebKind.CaoLiu;
                        break;
                    case "sis001.com":
                        novel = ExtractSis001Chapter(url, respHtml);
                        novel.Kind = NovelWebKind.sis001;
                        break;
                    default:
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
            HtmlNodeCollection tasks = doc.DocumentNode.SelectNodes("//div[@class=\"dd\"]//a");
            if (tasks == null)
                return null;

            string fileName = System.IO.Path.GetFileName(url);      //原页面名
            string newBaseUrl = "";
            if (string.IsNullOrEmpty(fileName))
            {
                newBaseUrl = url;
            }
            else
            {
                newBaseUrl = url.Replace(fileName, "");
            }
            novel.Urls = new List<string>();
            novel.ChapterNames = new List<string>();
            foreach (var info in tasks)
            {
                //<li><a title=【我的性奴家族】（1-3） href="108732.html">【我的性奴家族】（1-3）</a></li>
                string chapterUrl = newBaseUrl + info.Attributes["href"].Value;    //下一章节页面名称
                //<li><a href="javascript:;" onclick="tourl('114593.html')">【我的性奴家族】（5）</a></li>
                if (chapterUrl.Contains("javascript"))
                {
                    string temp = info.Attributes["onclick"].Value;
                    Regex reg = new Regex("[0-9]*.html");
                    var str = reg.Match(temp);
                    chapterUrl = newBaseUrl + str.Value;
                }
                novel.Urls.Add(chapterUrl);
                novel.ChapterNames.Add(info.InnerText);
            }
            

            return novel;
        }

        /// <summary>
        /// 抽取色中色小说
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <param name="html">网页源码</param>
        /// <returns></returns>
        NovelInfo ExtractSisChapter(string url, string html)
        {
            var novel = new NovelInfo();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            novel.Author = "";

            //获取文章名称
            novel.Title = doc.DocumentNode.SelectSingleNode("//div[@class=\"postmessage defaultpost\"]/h2").InnerText.Trim();

            //获取楼主
            HtmlNode authorNode = doc.DocumentNode.SelectSingleNode("//td[@class=\"postauthor\"]/cite/a");
            string firstPoster = authorNode.InnerText;
            novel.Poster = firstPoster;
            //string uid = authorNode.GetAttributeValue("href", "");
            //uid = uid.Replace("space.php?uid=", "");

            //遍历获取小说正文
            //获取楼主
            string poster = doc.DocumentNode.SelectSingleNode("//td[@class=\"postauthor\"]").InnerText;
            HtmlNode conNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"t_msgfont\"]/div[@class=\"t_msgfont\"]");
            string firstNovel = conNode.InnerText;


            //获取总页数
            novel.Urls = new List<string>();
            novel.Urls.Add(url);
            HtmlNode pagesNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[1]/div[5]/div[2]");
            if (pagesNode != null)
            {
                //存在多页情况
                int totalCount = pagesNode.ChildNodes.Count;
                for (int i = totalCount - 1; i < totalCount; i--)
                {
                    var pageNode = pagesNode.ChildNodes[i];
                    string temp = pageNode.InnerText;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        temp = temp.Replace(".", "").Trim();
                        if (temp.IsNum())
                        {
                            novel.PageCount = Convert.ToInt32(temp);
                            break;
                        }
                    }
                }
            }
            else
            {
                novel.PageCount = 1;
            }

            //匹配作者
            var authorMatch = Regex.Match(firstNovel, "作者[：:](?<info>.+)");
            if (authorMatch.Success)
            {
                string author = authorMatch.Groups[0].Value;
                author = author.Replace("\r", "");
                novel.Author = author;
            }

            return novel;
        }

        /// <summary>
        /// 抽取第一会所小说
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <param name="html">网页源码</param>
        /// <returns></returns>
        NovelInfo ExtractSis001Chapter(string url, string html)
        {
            var novel = new NovelInfo();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            novel.Author = "";

            //获取文章名称
            novel.Title = doc.DocumentNode.SelectSingleNode("//div[@class=\"postmessage defaultpost\"]/h2").InnerText.Trim();

            //获取楼主
            HtmlNode authorNode = doc.DocumentNode.SelectSingleNode("//td[@class=\"postauthor\"]/cite/a");
            string firstPoster = authorNode.InnerText;
            string postId = authorNode.GetAttributeValue("id", "").Replace("userinfo", "");
            novel.Poster = firstPoster;
            //string uid = authorNode.GetAttributeValue("href", "");
            //uid = uid.Replace("space.php?uid=", "");

            //遍历获取小说正文
            //获取楼主
            string poster = doc.DocumentNode.SelectSingleNode("//td[@class=\"postauthor\"]//a").InnerText;
            HtmlNode conNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"postmessage_{0}\"]".FormatStr(postId));
            string firstNovel = conNode.InnerText;


            //获取总页数
            novel.Urls = new List<string>();
            novel.Urls.Add(url);
            HtmlNode pagesNode = doc.DocumentNode.SelectSingleNode("//*[@id=\"wrapper\"]/div[1]/div[5]/div[2]");
            if (pagesNode != null)
            {
                //存在多页情况
                int totalCount = pagesNode.ChildNodes.Count;
                for (int i = totalCount - 1; i < totalCount; i--)
                {
                    var pageNode = pagesNode.ChildNodes[i];
                    string temp = pageNode.InnerText;
                    if (!string.IsNullOrEmpty(temp))
                    {
                        temp = temp.Replace(".", "").Trim();
                        if (temp.IsNum())
                        {
                            novel.PageCount = Convert.ToInt32(temp);
                            break;
                        }
                    }
                }
            }
            else
            {
                novel.PageCount = 1;
            }

            //匹配作者
            var authorMatch = Regex.Match(firstNovel, "作者[：:](?<info>.+)】?");
            if (authorMatch.Success)
            {
                string author = authorMatch.Groups[0].Value;
                author = author.Replace("\r", "");
                novel.Author = author;
            }

            return novel;
        }
        /// <summary>
        /// 抽取草榴小说
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <param name="html">网页源码</param>
        /// <returns></returns>
        NovelInfo ExtractCLChapter(string url, string html)
        {
            var novel = new NovelInfo();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            novel.Author = "";

            //获取文章名称
            novel.Title = doc.DocumentNode.SelectSingleNode("//tr[@class=\"tr1 do_not_catch\"]//h4").InnerText.Trim();

            //获取楼主
            HtmlNode authorNode = doc.DocumentNode.SelectSingleNode("//th[@class=\"r_two\"]/b");
            string firstPoster = authorNode.InnerText;
            novel.Poster = firstPoster;
            //string uid = authorNode.GetAttributeValue("href", "");
            //uid = uid.Replace("space.php?uid=", "");

            //遍历获取小说正文
            //获取楼主
            HtmlNode conNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"tpc_content do_not_catch\"]");
            string firstNovel = conNode.InnerText;


            //获取总页数
            novel.Urls = new List<string>();
            novel.Urls.Add(url);
            HtmlNode pagesNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"pages\"]");
            if (pagesNode != null)
            {
                //存在多页情况
                var lastpageNode = doc.DocumentNode.SelectSingleNode("//*[@class=\"pages\"]//input");
                string lastpageStr = lastpageNode.GetAttributeValue("value","");
                lastpageStr = lastpageStr.Replace("1/", "");
                novel.PageCount = Convert.ToInt32(lastpageStr);
            }
            else
            {
                novel.PageCount = 1;
            }

            //匹配作者
            var authorMatch = Regex.Match(firstNovel, "作者[：:](?<info>.+)");
            if (authorMatch.Success)
            {
                string author = authorMatch.Groups[0].Value;
                author = author.Replace("\r", "");
                novel.Author = author;
            }
            else
            {
                novel.Author = firstPoster;
            }

            return novel;
        }

    }
}
