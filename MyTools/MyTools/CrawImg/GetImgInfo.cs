using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

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
                //Http请求
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //使用GET方法获取链接指向的网页
                req.Method = "GET";
                //设置用户代理，防止网站判定为用户代理
                req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
                //如果方法验证网页来源就加上这一句如果不验证那就可以不写了
                //req.Referer = "http://sufei.cnblogs.com";
                //添加Cookie，用来抓取需登陆可见的网页
                CookieContainer objcok = new CookieContainer();
                Cookie cook1 = new Cookie();
                //cook1.Name = "CNZZDATA1257708097";
                //cook1.Value = "1167517418-1479817270-%7C1488717892";
                //objcok.Add(cook1);
                objcok.Add(new Cookie("LOGGED_USER", "2Gsu8lGqckigfryi4J%2BxqQ%3D%3D%3AEPYACL1Ic4QgUm9bW2hOXg%3D%3D", "/", ".bcy.net"));
                req.CookieContainer = objcok;
                //设置超时
                req.Timeout = 3000;
                //Http响应
                HttpWebResponse resp;
                int time = 0;   //服务器无响应里重试次数
                while (true)
                {
                    time++;
                    resp = (HttpWebResponse)req.GetResponse();
                    if (resp.StatusCode != HttpStatusCode.OK)
                    {
                        if (time < 5)
                            continue;
                        else
                            break;
                    }
                    break;
                }
                //使用utf-8去解码网页
                string htmlCharset = "utf-8";
                Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
                StreamReader sr = new StreamReader(resp.GetResponseStream(), htmlEncoding);
                //读取返回的网页
                string respHtml = sr.ReadToEnd();
                //对网页进行分类
                Regex kindReg = new Regex(@"http://bcy.net/(?<info>.+?)/");
                Match kindMat = kindReg.Match(url);
                string kind = kindMat.Groups["info"].Value;
                switch (kind)
                {
                    case "coser":
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
                resp.Close();

                return img;
            }
            catch (Exception)
            {
                img.Error = "网页连接出错！";
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
            img.Urls = new List<string>();
            //获取图片归属作品名称
            Match ACGWork = Regex.Match(respHtml, @"<meta name=""keywords"" content="".+,.+,(?<info>.+?),.+,.+,.+,.+,.+,.+,.+"" />");
            img.ACGWork = ACGWork.Groups["info"].Value;
            //获取Author信息
            Regex cnReg = new Regex(@"cn:&nbsp;&nbsp;<h3><a href="".*"" class=""blue1"">(?<info>.+?)</a></h3>");
            MatchCollection cn = cnReg.Matches(respHtml);
            if (cn.Count == 0)
            {
                cnReg = new Regex(@"cn:&nbsp;&nbsp;(?<info>.+?)</span>");
                cn = cnReg.Matches(respHtml);
            }
            //判断是单人还是多人cosplay，多人cosplay以角色名开头，单人cosplay以标题开头
            if (cn.Count > 1)
            {
                //获取coser名字
                string cosers = "";
                foreach (Match x in cn)
                {
                    cosers += x.Groups["info"].Value + "&";
                }
                cosers = cosers.Substring(0, cosers.Length - 1);
                img.Author = cosers;
                //获取角色名字
                Regex roleReg = new Regex("<h2.*>\n(?<info>.+?) </a>");
                MatchCollection rolesMatch = roleReg.Matches(respHtml);
                string roles = "";
                foreach (Match x in rolesMatch)
                {
                    roles += x.Groups["info"].Value + "&";
                }
                img.Title = roles.Substring(0, roles.Length - 1);
            }
            else
            {
                img.Author = cn[0].Groups["info"].Value;
                //获取标题
                Match titleMat = Regex.Match(respHtml, "<h1.*>\n(?<info>.+?) </h1>");
                try
                {
                    if (titleMat.Value == null || cn.Count == 0)
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
            }
            //获取正文
            Match descriptionMatch = Regex.Match(respHtml, @"<div class=""post__content js-content-img-wrap js-fullimg js-maincontent mb20"">\s(?<info>.+?)<br/>");
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
            Match ACGWork = Regex.Match(respHtml, @"<meta name=""keywords"" content="".+,.+,(?<info>.+?),.+,.+,.+,.+,.+,.+,.+"" />");
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
