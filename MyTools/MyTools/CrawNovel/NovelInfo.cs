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
        #region 对分章节小说网站
        /// <summary>
        /// 章节链接
        /// </summary>
        public List<string> Urls { get; set; }
        /// <summary>
        /// 章节名
        /// </summary>
        public List<string> ChapterNames { get; set; }
        #endregion
        #region 对论坛贴子
        /// <summary>
        /// 楼主名称
        /// </summary>
        public string Poster { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        #endregion
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
        Diyibanzhu,
        Sexinsex,
        CaoLiu,
    }


}
