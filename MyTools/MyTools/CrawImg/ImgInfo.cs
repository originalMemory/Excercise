using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.CrawImg
{
    /// <summary>
    /// 抓取的图片信息
    /// </summary>
    public class ImgInfo
    {
        /// <summary>
        /// 作者信息
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 主题归属作品名称
        /// </summary>
        public string ACGWork { get; set; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public List<string> Urls { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 作品标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 正文
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 图片类型
        /// </summary>
        public ImgKind Kind { get; set; }
    }

    /// <summary>
    /// 图片类型
    /// </summary>
    public enum ImgKind
    {
        Cosplay, Daily, Illustraion
    }


}
