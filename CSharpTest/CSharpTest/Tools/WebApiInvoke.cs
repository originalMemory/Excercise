﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using AISSystem;
using HtmlAgilityPack;

namespace CSharpTest.Tools
{
    public class WebApiInvoke
    {
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static string CreateGetHttpResponse(string url, int timeout = 5, string userAgent = null, CookieCollection cookies = null)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());

            return sr.ReadToEnd();
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, object> parameters, CookieCollection cookies = null, int timeout = 5000, string userAgent = null)
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] value = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取网页源代码
        /// </summary>
        /// <param name="url">网页链接</param>
        /// <returns></returns>
        public static string GetHtml(string url, string htmlCharset = "utf-8", int type = 0, IDictionary<string, object> parameters = null, CookieCollection cookies = null)
        {
            //Http请求
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            //设置用户代理，防止网站判定为用户代理
            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)";
            //如果方法验证网页来源就加上这一句如果不验证那就可以不写了
            //req.Referer = "http://sufei.cnblogs.com";
            string html = "";
            
            try
            {
                switch (type)
                {
                    case 0:
                        {
                            //使用GET方法获取链接指向的网页
                            req.Method = "GET";
                            //添加Cookie，用来抓取需登陆可见的网页
                            CookieContainer objcok = new CookieContainer();
                            Cookie cook1 = new Cookie();
                            //cook1.Name = "CNZZDATA1257708097";
                            //cook1.Value = "1167517418-1479817270-%7C1488717892";
                            //objcok.Add(cook1);
                            objcok.Add(new Cookie("LOGGED_USER", "2Gsu8lGqckigfryi4J%2BxqQ%3D%3D%3AEPYACL1Ic4QgUm9bW2hOXg%3D%3D", "/", ".bcy.net"));
                            req.CookieContainer = objcok;
                            //设置超时
                            req.Timeout = 5000;
                            //Http响应
                            HttpWebResponse resp;
                            resp = (HttpWebResponse)req.GetResponse();
                            Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
                            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), htmlEncoding);
                            //读取返回的网页
                            html = sr.ReadToEnd();
                            resp.Close();
                        }
                        break;
                    case 1:
                        {
                            HttpWebResponse resp = CreatePostHttpResponse(url, parameters, cookies);
                            Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
                            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), htmlEncoding);
                            //读取返回的网页
                            html = sr.ReadToEnd();
                            resp.Close();
                        }
                        break;
                    default:
                        break;
                }

                return html;
            }
            catch (Exception ex)
            {
                CommonTools.Log(ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 获取域名收录量
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public static long GetDomainCollectionNum(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                return (long)0;
            }
            string url = "http://www.baidu.com/s?ie=utf-8&wd=site:{0}";
            url = url.FormatStr(domain.GetUrlEncodedString("utf-8"));
            string html = GetHtml(url, "utf-8");        //获取网页源码
            //解析并获取域名收录量
            HtmlDocument doc = new HtmlDocument();

            if (html != null)
            {
                doc.LoadHtml(html);
                HtmlNode collection = doc.DocumentNode.SelectSingleNode("//*[@id=\"1\"]/div/div[1]/div/p[3]/span/b");
                long num = 0;        //域名收录量
                if (collection != null)
                {
                    string numStr = collection.InnerText;
                    if (!string.IsNullOrEmpty(numStr))
                    {
                        numStr = numStr.Trim().Replace(",", "");
                        if (numStr.Contains("亿"))
                        {
                            if (numStr.Contains("万"))
                            {
                                int p1 = numStr.IndexOf("亿");
                                int p2 = numStr.IndexOf("万");
                                long a = Convert.ToInt32(numStr.SubBefore("亿"));
                                long b = Convert.ToInt32(numStr.SubAfter("亿").SubBefore("万"));
                                num = a * 100000000 + b * 10000;
                            }
                            else
                            {
                                int p1 = numStr.IndexOf("亿");
                                int p2 = numStr.IndexOf("万");
                                long a = Convert.ToInt32(numStr.SubBefore("亿"));
                                long b = Convert.ToInt32(numStr.SubAfter("亿"));
                                num = a * 100000000 + b;
                            }
                        }
                        else if (numStr.Contains("万"))
                        {
                            int p2 = numStr.IndexOf("万");
                            long a = Convert.ToInt32(numStr.SubBefore("万"));
                            long b = Convert.ToInt32(numStr.SubAfter("万"));
                            num = a * 10000 + b;
                        }
                        else
                        {
                            num = Convert.ToInt64(numStr);
                        }
                    }
                    return num;
                }
            }
            return (long)0;
        }

        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }
    }
}
