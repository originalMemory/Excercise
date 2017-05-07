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
    public class DownloadImg
    {
        public static readonly DownloadImg Instance = new DownloadImg();

        #region 下载图片
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="path">保存路径</param>
        /// <param name="imgInfo">图片链接</param>
        /// <returns></returns>
        public static string GetImages(string path, ImgInfo imgInfo)
        {
            try
            {
                //判断文件夹是否存在，不存在则创建文件夹后继续。若路径名含有不能使用字符，返回错误信息
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception)
                    {
                        return "文件夹名称不符合规范！";
                    }
                }
                //下载图片
                foreach (string x in imgInfo.Urls)
                {
                    //法1
                    ////Http请求
                    //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(x);
                    ////使用GET方法获取链接指向的图片
                    //req.Method = "GET";
                    ////Http响应
                    //HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                    ////获取文件名
                    ////http://img5.bcyimg.com/coser/47840/post/177yb/b170965019f011e69ec30f46e23bf1f9.jpg
                    //Regex reg = new Regex(@"http://.*/.*/.*/.*/.*/(?<info>.+)");
                    //Match mat = reg.Match(x);
                    ////获取图片并保存至本地。
                    //Image img = Image.FromStream(resp.GetResponseStream());
                    //img.Save(path + @"\" + mat.Groups["info"].Value);
                    //img.Dispose();
                    //resp.Close();

                    //法2
                    using (var webClient = new WebClient())   //使用完后释放对象
                    {
                        var imageDate = webClient.DownloadData(x);
                        using (var stream = new MemoryStream(imageDate))
                        {
                            var image = Image.FromStream(stream);
                            Regex reg = new Regex(@"http://.*/(?<info>.+)");
                            Match mat = reg.Match(x);
                            image.Save(path + @"\" + mat.Groups["info"].Value);
                            image.Dispose();
                        }
                    }
                }
                return "保存成功！";
            }
            catch (Exception)
            {
                return "网页连接出错！";
            }
        }
        #endregion
    }
}
