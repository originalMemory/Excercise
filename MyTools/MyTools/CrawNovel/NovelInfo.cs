using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.CrawNovel
{
    /// <summary>
    /// 抓取的图片信息
    /// </summary>
    public class NovelInfo
    {
        /// <summary>
        /// 作者信息
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 章节链接
        /// </summary>
        public List<string> Urls { get; set; }
        /// <summary>
        /// 章节名
        /// </summary>
        public List<string> ChapterNames { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 作品标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 网站类型
        /// </summary>
        public NovelWebKind Kind { get; set; }
    }

    /// <summary>
    /// 小说网站类型
    /// </summary>
    public enum NovelWebKind
    {
        Diyibanzhu
    }


}
