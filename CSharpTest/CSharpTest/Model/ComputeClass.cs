using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CSharpTest.Model
{
    /// <summary>
    /// 域名分类信息
    /// </summary>
    public class DomainCategoryInfo
    {
        public List<string> Domain { get; set; }
        public List<string> DomainCategoryId { get; set; }
        public List<string> DomainCategoryName { get; set; }
    }


    /// <summary>
    /// 折线图单条线数据
    /// </summary>
    public class LineData
    {
        public string name { get; set; }
        public List<int> LinkCount { get; set; }
        public List<TopData> topData { get; set; }
        public string CategoryId { get; set; }
    }

    /// <summary>
    /// Top数据
    /// </summary>
    public class TopData
    {
        public DateTime X { get; set; }
        public int Y { get; set; }
        public string name { get; set; }
        public string CategoryId { get; set; }
    }

    /// <summary>
    /// 摘要数据
    /// </summary>
    public class SumData
    {
        public DateTime X { get; set; }
        public int Y { get; set; }
        public string Summary { get; set; }
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
    }

    /// <summary>
    /// 链接信息
    /// </summary>
    public class LinkStatus
    {
        public DateTime PublishTime { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
    }

    /// <summary>
    /// 链接分组信息
    /// </summary>
    public class CategoryList
    {
        public List<DateTime> PublishTime { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    /// <summary>
    /// 分组树（圆形d3图）
    /// </summary>
    public class GroupTree2Dto
    {
        public string id { get; set; }
        /// <summary>
        /// 父结点Id
        /// </summary>
        public string pId { get; set; }
        /// <summary>
        /// 结点名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 是否为非叶子结点
        /// </summary>
        public bool isNode { get; set; }

    }

    /// <summary>
    /// 百度链接数据
    /// </summary>
    public class BaiduLink
    {
        public string Id;
        public string Url;
        public string Title;
        public string Description;
        public string Domain;
        public string Keyword;
        public string KeywordId;
        public DateTime PublishTime;
    }

    /// <summary>
    /// 标签搜索使用的链接数据
    /// </summary>
    public class TagLink
    {
        public ObjectId Id { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Html源码，微信时为正文
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 关键词Id
        /// </summary>
        public string KeywordId { get; set; }
    }
}
