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
            int time = 0;   //服务器无响应时重试次数
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
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), htmlEncoding);
            //读取返回的网页
            string respHtml = sr.ReadToEnd();
            resp.Close();
            return respHtml;
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
