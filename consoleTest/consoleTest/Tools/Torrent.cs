using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTorrent;
using System.Net;

namespace CSharpTest.Tools
{
    /// <summary>
    /// 种子相关类
    /// </summary>
    class Torrent
    {
        /// <summary>
        /// 根据链接下载种子
        /// </summary>
        /// <param name="url">种子下载链接</param>
        /// <param name="path">保存位置</param>
        public static void Download(string url,string path)
        {
            WebClient client = new WebClient();
            client.DownloadFile(url, path + System.IO.Path.GetFileName(url));
        }
    }
}
