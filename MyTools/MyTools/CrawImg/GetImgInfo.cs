using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using MyTools.Tools;
using HtmlAgilityPack;
using MyTools.Helper;

namespace MyTools.CrawImg
{
    public class GetImgInfo
    {
        #region 获取图片信息及链接
        /// <summary>
        /// 获取图片信息及链接
        /// </summary>
        /// <param name="url">半次元图片链接</param>
        /// <returns></returns>
        public ImgInfo GetImgUrls(string url)
        {
            ImgInfo img = new ImgInfo();
            try
            {
                //半次元cookies
                CookieContainer objcok = new CookieContainer();
                objcok.Add(new Cookie("__tea_sdk__ssid", "d0cbb440-3366-4c4e-972b-8694b4c19e76", "/", ".bcy.net"));
                objcok.Add(new Cookie("__tea_sdk__user_unique_id", "6548691244520080900", "/", ".bcy.net"));
                objcok.Add(new Cookie("CNZZDATA1257708097", "1707831137-1520559900-%7C1526728284", "/", "bcy.net"));
                objcok.Add(new Cookie("_ga", "GA1.2.1951345541.1531376347", "/", ".bcy.net"));
                objcok.Add(new Cookie("lang_set", "zh", "/", ".bcy.net"));
                objcok.Add(new Cookie("tt_webid", "6599603821759776263", "/", ".bcy.net"));
                objcok.Add(new Cookie("ccid", "87d5218b58ad1cf2471c3ed722233f66", "/", ".bcy.net"));
                objcok.Add(new Cookie("sid_guard", "df0dd15351f0d723b151bf1169ea6bfd%7C1538656608%7C5184000%7CMon%2C+03-Dec-2018+12%3A36%3A48+GMT", "/", ".bcy.net"));
                objcok.Add(new Cookie("uid_tt", "e2b1d6d3b15165205114387e1693f776", "/", ".bcy.net"));
                objcok.Add(new Cookie("sid_tt", "df0dd15351f0d723b151bf1169ea6bfd", "/", ".bcy.net"));
                objcok.Add(new Cookie("sessionid", "df0dd15351f0d723b151bf1169ea6bfd", "/", ".bcy.net"));
                objcok.Add(new Cookie("PHPSESSID", "f2dcf83c81402ec4210080ac85c6bf19", "/", "bcy.net"));
                objcok.Add(new Cookie("mobile_set", "no", "/", ".bcy.net"));
                objcok.Add(new Cookie("Hm_lvt_330d168f9714e3aa16c5661e62c00232", "1540977951%2C1540995336%2C1541252789%2C1541336092", "/", ".bcy.net"));
                objcok.Add(new Cookie("_gid", "GA1.2.169433111.1541336095", "/", ".bcy.net"));
                objcok.Add(new Cookie("Hm_lpvt_330d168f9714e3aa16c5661e62c00232", "1541398442", "/", ".bcy.net"));
                objcok.Add(new Cookie("_csrf_token", "d4500ff829403edc9bcd75348761f489", "/", ".bcy.net"));
                objcok.Add(new Cookie("_bcy_user_id", "U2FsdGVkX1+/Zqi2ucICyz9tj4Y9o9VXE6ZqoxDQGd1nhlSbAxGOWiaROCTsKstXoMFeCphhUhGRGkjgN3szzA==", "/", ".bcy.net"));
                //获取网页
                string respHtml = WebApi.GetHtml(url,objcok);
                //对网页进行分类
                Regex kindReg = new Regex(@"bcy.net/(?<info>.+?)/");
                Match kindMat = kindReg.Match(url);
                string kind = kindMat.Groups["info"].Value;
                switch (kind)
                {
                    case "item":
                        img.Kind = ImgKind.Cosplay;
                        break;
                    case "daily":
                        img.Kind = ImgKind.Daily;
                        break;
                    case "illust":
                        img.Kind = ImgKind.Illustraion;
                        break;
                }
                switch (img.Kind)
                {
                    case ImgKind.Cosplay:
                        img = ExctractCos(respHtml);
                        break;
                    case ImgKind.Daily:
                        img = ExctractDaily(respHtml);
                        break;
                    case ImgKind.Illustraion:
                        img = ExctractACGImg(respHtml);
                        break;
                }
                //判断是否为关注可见图片，是则返回错误信息
                if (img.Urls.Count == 0)
                {
                    img.Error = "该页面关注可见，无法下载图片！";
                }

                return img;
            }
            catch (Exception ex)
            {
                img.Error = ex.Message;
                return img;
            }
        }
        #endregion

        #region 对不同类型页面的提取函数
        /// <summary>
        /// 抽取Cosplay图片
        /// </summary>
        /// <param name="respHtml">网页源代码</param>
        /// <returns></returns>
        private ImgInfo ExctractCos(string respHtml)
        {
            ImgInfo img = new ImgInfo();
            img.Kind = ImgKind.Cosplay;
            if (respHtml.Contains("/tags/name/绘画"))
                img.Kind = ImgKind.Illustraion;
            img.Urls = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(respHtml);
            //获取图片归属作品名称
            string con = doc.DocumentNode.SelectSingleNode("//meta[@name=\"keywords\"]").GetAttributeValue("content", "");
            img.ACGWork = con.Split(',')[0].Replace("。ACG", "");
            //获取coser信息
            img.Author = doc.DocumentNode.SelectSingleNode("//a[@class=\"cut\"][1]").InnerText;
            //改版后的半次元无自定义标题，也没有单独CN显示，故取最后一个标签为标题
            HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"tag-group\"]").LastChild;
            img.Title = System.Net.WebUtility.HtmlDecode(titleNode.InnerText);
            
            //获取正文
            HtmlNode descNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"content\"]/div");
            if(descNode!=null)
            {
                string desc = descNode.InnerHtml;
                Regex reg = new Regex("</p>|<br>|</br>");
                desc = reg.Replace(desc, System.Environment.NewLine);
                desc = Regex.Replace(desc, "<a href=.+?>|</a>", "");
                img.Description = System.Net.WebUtility.HtmlDecode(desc);
            }
            //获取图片链接
            string json = Regex.Match(respHtml, "JSON.parse.+;").Value;
            json = Regex.Unescape(Regex.Unescape(json));
            MatchCollection urls = Regex.Matches(json, "\"path\":\"(?<url>.+?)~tplv-banciyuan-w650.image");
            foreach (Match x in urls)
            {
                string str;
                str = x.Groups["url"].Value;
                if (!string.IsNullOrEmpty(str))
                {
                    str = Regex.Replace(str, "p.-bcy.byteimg.com/img/banciyuan", "img-bcy-qn.pstatp.com");
                    img.Urls.Add(str);
                }
            }
            return img;
        }

        /// <summary>
        /// 抽取Cosplay图片
        /// </summary>
        /// <param name="respHtml">网页源代码</param>
        /// <returns></returns>
        private ImgInfo ExctractDaily(string respHtml)
        {
            ImgInfo img = new ImgInfo();
            img.Kind = ImgKind.Daily;
            img.Urls = new List<string>();
            //获取图片归属作品名称
            Match ACGWork = Regex.Match(respHtml, @"<meta name=""keywords"" content=""(?<info>.+?),+.*"" />");
            img.ACGWork = ACGWork.Groups["info"].Value;
            //获取Author信息
            Regex cnReg = new Regex(@"<img.+alt=""(?<info>.+?)""/>");
            Match cn = cnReg.Match(respHtml);
            img.Author = cn.Groups["info"].Value;
            //获取标题
            Match titleMat = Regex.Match(respHtml, "<h1.*>\n(?<info>.+?) </h1>");
            try
            {
                if (titleMat.Value == null)
                    img.Title = "";
                else
                    img.Title = titleMat.Groups["info"].Value;
            }
            catch (Exception)
            {
                string tilte = titleMat.Value;
                int index1 = tilte.IndexOf('\n');
                int index2 = tilte.IndexOf('<', index1);
                img.Title = tilte.Substring(index1 + 1, index2 - index1);

            }
            //获取正文
            Match descriptionMatch = Regex.Match(respHtml, @"<div id=""larticleArticle"" class=""l-clearfix larticleArticle"">(?<info>.+?)</div>");
            string description = descriptionMatch.Groups["info"].Value;
            //获取<a>标签内链接和内容
            Regex desreg = new Regex(@"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection des = desreg.Matches(description);
            foreach (Match x in des)
            {
                description = description.Replace(x.Value, x.Groups["text"].Value);
            }
            description = description.Replace("<br>", System.Environment.NewLine);
            img.Description = description;
            //获取链接
            Regex linkreg = new Regex(@"<img class='detail_std detail_clickable' src='(?<info>.+?)/w650' />");
            MatchCollection urls = linkreg.Matches(respHtml);
            foreach (Match x in urls)
            {
                string str;
                str = x.Groups["info"].Value;
                if (!string.IsNullOrEmpty(str))
                {
                    img.Urls.Add(str);
                }
            }
            return img;
        }

        /// <summary>
        /// 抽取二次元图片
        /// </summary>
        /// <param name="respHtml">网页源代码</param>
        /// <returns></returns>
        private ImgInfo ExctractACGImg(string respHtml)
        {
            ImgInfo img = new ImgInfo();
            img.Kind = ImgKind.Illustraion;
            img.Urls = new List<string>();
            //获取图片归属作品名称
            Match ACGWork = Regex.Match(respHtml, @"<meta name=""keywords"" content="".+,.+,(?<info>.+?),.+,.+,.+,.+,.+,.+,.+"" />");
            img.ACGWork = ACGWork.Groups["info"].Value;
            //获取画师信息
            Regex artReg = new Regex(@"<img.+alt=""(?<info>.+?)""/>");
            Match artMat = artReg.Match(respHtml);
            img.Author = artMat.Groups["info"].Value;
            //获取标题
            Match title = Regex.Match(respHtml, "<h1.*>\n(?<info>.+?) </h1>");
            if (title.Value != null)
            { img.Title = title.Groups["info"].Value; }
            //获取正文
            Match descriptionMatch = Regex.Match(respHtml, @"<div class=""post__content js-content-img-wrap js-fullimg js-maincontent mb0"">\s(?<info>.+?)<br/>");
            string description = descriptionMatch.Groups["info"].Value;
            //获取<a>标签内链接和内容
            Regex desreg = new Regex(@"(?is)<a(?:(?!href=).)*href=(['""]?)(?<url>[^""\s>]*)\1[^>]*>(?<text>(?:(?!</?a\b).)*)</a>");
            MatchCollection des = desreg.Matches(description);
            foreach (Match x in des)
            {
                description = description.Replace(x.Value, x.Groups["text"].Value);
            }
            description = description.Replace("<br>", System.Environment.NewLine);
            img.Description = description;
            //获取链接
            Regex linkreg = new Regex(@"<img class='detail_std detail_clickable' src='(?<info>.+?)/w650' />");
            MatchCollection urls = linkreg.Matches(respHtml);
            foreach (Match x in urls)
            {
                string str;
                str = x.Groups["info"].Value;
                if (!string.IsNullOrEmpty(str))
                {
                    img.Urls.Add(str);
                }
            }
            return img;
        }
        #endregion

    }
}
