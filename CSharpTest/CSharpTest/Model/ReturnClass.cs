using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    /// <summary>
    /// 气泡图结果
    /// </summary>
    public class DomainStatisDto
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 有效链接数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 关键词数
        /// </summary>
        public int KeywordTotal { get; set; }
        /// <summary>
        /// 域名收录量
        /// </summary>
        public long DomainColNum { get; set; }
        /// <summary>
        /// 含发布时间比
        /// </summary>
        public string PublishRatio { get; set; }
        /// <summary>
        /// 域名分组Id
        /// </summary>
        public string DomainCategoryId { get; set; }
        /// <summary>
        /// 域名分组名
        /// </summary>
        public string DomainCategoryName { get; set; }
    }

    /// <summary>
    /// 折线图及饼图数据
    /// </summary>
    public class TimeLinkCountDto
    {
        /// <summary>
        /// 时间坐标
        /// </summary>
        public List<DateTime> Times { get; set; }
        /// <summary>
        /// 折线图数据，每个对象为一条线
        /// </summary>

        public List<LineData> LineDataList { get; set; }
        /// <summary>
        /// 精简版自动摘要，用于在折线图生成提示及生成饼图
        /// </summary>
        public List<SumData> Sum { get; set; }
    }

    /// <summary>
    /// 微信气泡图结果
    /// </summary>
    public class WXDomainStatisDto
    {
        /// <summary>
        /// 公众号名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 链接数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 关键词数
        /// </summary>
        public int KeywordTotal { get; set; }
        /// <summary>
        /// 热度
        /// </summary>
        public int HotNum { get; set; }
        /// <summary>
        /// 域名分组Id
        /// </summary>
        public string DomainCategoryId { get; set; }
        /// <summary>
        /// 域名分组名
        /// </summary>
        public string DomainCategoryName { get; set; }
        /// <summary>
        /// 含发布时间比
        /// </summary>
        public float PublishRatio { get; set; }
    }

    #region 公众号热度分析
    /// <summary>
    /// 公众号热度分析
    /// </summary>
    public class NameStatisticDto
    {
        /// <summary>
        /// 公众号名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公众号Id
        /// </summary>
        public string NameId { get; set; }
        /// <summary>
        /// 发布文章数
        /// </summary>
        public int LinkNum { get; set; }
        ///// <summary>
        ///// 文章信息列表
        ///// </summary>
        //public List<WXLinkInfo> LinkInfoList { get; set; }
        /// <summary>
        /// 累计评论数
        /// </summary>
        public int CommentNum { get; set; }
        /// <summary>
        /// 累计点赞数
        /// </summary>
        public int LikeNum { get; set; }
        /// <summary>
        /// 累计阅读数
        /// </summary>
        public int ReadNum { get; set; }
        /// <summary>
        /// 命中关键词数
        /// </summary>
        public int KeywordNum { get; set; }
        ///// <summary>
        ///// 关键词信息列表
        ///// </summary>
        //public List<WXKeywordInfo> KeyInfoList { get; set; }
        /// <summary>
        /// 影响力指数
        /// </summary>
        public int InfluenceNum { get; set; }
    }

    /// <summary>
    /// 微信文章信息
    /// </summary>
    public class WXLinkInfo
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentNum { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int LikeNum { get; set; }
        /// <summary>
        /// 阅读数
        /// </summary>
        public int ReadNum { get; set; }
        /// <summary>
        /// 命中关键词数
        /// </summary>
        public int KeywordNum { get; set; }
        /// <summary>
        /// 影响力指数
        /// </summary>
        public int InfluenceNum { get; set; }
    }

    /// <summary>
    /// 微信关键词信息
    /// </summary>
    public class WXKeywordInfo
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 命中次数
        /// </summary>
        public int MatchNum { get; set; }
        /// <summary>
        /// 链接信息
        /// </summary>
        public List<LinkTitleAUrl> LinkList { get; set; }
        /// <summary>
        /// 影响力指数
        /// </summary>
        public int InfluenceNum { get; set; }
    }

    public class LinkTitleAUrl
    {
        /// <summary>
        /// 链接地址，排重使用
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
    #endregion

    /// <summary>
    /// 列表数据及总数类
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class QueryResult<T>
    {
        public List<T> Result { get; set; }
        public long Count { get; set; }
    }
}
