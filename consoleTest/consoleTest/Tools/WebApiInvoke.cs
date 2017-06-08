using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace CSharpTest.Tools
{
    public class WebApiInvoke
    {
        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ////对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
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
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout=5000, string userAgent=null, CookieCollection cookies=null)
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
        public static string GetHtml(string url)
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
            req.Timeout = 5000;
            //Http响应
            HttpWebResponse resp = null;
            int time = 0;   //服务器无响应时重试次数
            while (true)
            {
                time++;
                try
                {
                    resp = (HttpWebResponse)req.GetResponse();
                }
                catch
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
            string respHtml = null;
            if (resp != null)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), htmlEncoding);
                //读取返回的网页
                respHtml = sr.ReadToEnd();
                resp.Close();
            }
            return respHtml;
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
