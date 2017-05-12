using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Model
{
    /// <summary>
    /// 公有百度链接类
    /// </summary>
    public class Dnl_Link_Baidu
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// 搜索关键词Id
        /// </summary>
        public string SearchkeywordId { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 一级域名
        /// </summary>
        public string TopDomain { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述，从百度搜索页面抓取
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime CreatedAt { get; set; }
        /// <summary>
        /// 网页源代码
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 网页摘要，从网页源代码获取
        /// </summary>
        public string Abstract { get; set; }
        /// <summary>
        /// 百度蓝V认证
        /// </summary>
        public int BaiduVStar { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PublishTime { get; set; }
        /// <summary>
        /// 抓取时在搜索页面的排序所在位置
        /// </summary>
        public int Rank { get; set; }
    }
    /// <summary>
    /// 私有百度链接与公有百度链接映射类，包含用户对链接的分类和设置
    /// </summary>
    public class Dnl_LinkMapping_Baidu
    {
        public ObjectId _id { get; set; }
        public ObjectId LinkId { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId ProjectId { get; set; }
        /// <summary>
        /// 数据清洗状态：1，收藏;2,排除
        /// </summary>
        public Nullable<byte> DataCleanStatus { get; set; }
        /// <summary>
        /// 数据类型Id
        /// </summary>
        public ObjectId InfriLawCode { get; set; }
        
    }

    /// <summary>
    /// 百度链接类
    /// </summary>
    public class Dnl_Link_BaiduDto
    {
        public string _id { get; set; }
        /// <summary>
        /// 搜索关键词Id
        /// </summary>
        public string SearchkeywordId { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 主域名
        /// </summary>
        public string TopDomain { get; set; }
        public string LinkUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 数据清洗状态：1，收藏
        /// </summary>
        public Nullable<byte> DataCleanStatus { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime CreatedAt { get; set; }
        public string Abstract { get; set; }
        public int BaiduVStar { get; set; }
        public string InfriLawCode { get; set; }
        public string InfriLawCodeStr { get; set; }
        public DateTime PublishTime { get; set; }

    }
}
