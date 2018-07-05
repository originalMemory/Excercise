using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace MyTools.Tools
{
    /// <summary>
    /// 网页处理类
    /// </summary>
    public static class WebApi
    {
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
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
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
            string html = "";       //网页内容

            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

                //字节读取网页内容
                MemoryStream memStream = new MemoryStream();
                using (Stream stream = resp.GetResponseStream())
                {

                    byte[] buffer = new byte[1024];
                    int byteCount;
                    do
                    {
                        byteCount = stream.Read(buffer, 0, buffer.Length);
                        memStream.Write(buffer, 0, byteCount);
                    } while (byteCount > 0);
                }

                //默认以UTF8格式解析
                Encoding encoding = Encoding.UTF8;
                var charset = GetEncoding(resp.CharacterSet);
                if (charset == null)
                {
                    html = Encoding.UTF8.GetString(memStream.ToArray());
                    //判断是否的确为UTF-8格式
                    var charsetStr = "";
                    var charsetReg = new System.Text.RegularExpressions.Regex("<meta [^>]*charset=(.*?)(?=(;|\b|\"))");
                    var match = charsetReg.Match(html);
                    if (match.Groups.Count > 1)
                    {
                        charsetStr = match.Groups[1].Value;
                        if (charsetStr.Trim().ToLower() == "gbk" || charsetStr.Trim().ToLower() == "gb2312")
                        {
                            html = Encoding.GetEncoding("gb2312").GetString(memStream.ToArray());
                            encoding = Encoding.GetEncoding("gb2312");
                        }
                    }
                }
                else
                {
                    html = charset.GetString(memStream.ToArray());
                    //encoding = Encoding.GetEncoding("utf-8");
                }
                resp.Close();
            }
            catch (WebException ex)
            {
                using (var sr = new StreamReader(ex.Response.GetResponseStream(), Encoding.GetEncoding("gb2312")))
                {
                    html = sr.ReadToEnd();
                }
           
            }
            return html;
        }

        /// <summary>
        /// 根据字符串返回编码格式
        /// </summary>
        /// <param name="CharacterSet">编码字符串</param>
        /// <returns>编码格式</returns>
        public static Encoding GetEncoding(string CharacterSet)
        {
            switch (CharacterSet.ToLower())
            {
                case "gb2312": return Encoding.GetEncoding("gb2312");
                case "utf-8": return Encoding.UTF8;
                case "iso-8859-1": return Encoding.GetEncoding("gb2312");

                default: return null;
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="urls">文件链接</param>
        /// <param name="path">保存路径</param>
        /// <param name="fileName">为空时从链接读取文件名</param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string path,string fileName=null)
        {
            try
            {
                //判断文件夹是否存在，不存在则创建文件夹后继续。若路径名含有不能使用字符，返回错误信息
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (fileName == null)
                    fileName = Path.GetFileName(url);             //获取文件名
                string filePath = Path.Combine(path, fileName);             //保存文件路径
                //如果文件已存在，删除该文件
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                //下载文件
                using (var webClient = new WebClient())   //使用完后释放对象
                {
                    webClient.DownloadFile(url, filePath);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
