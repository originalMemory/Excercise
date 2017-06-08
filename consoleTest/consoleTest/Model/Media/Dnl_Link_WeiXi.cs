using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CSharpTest.Model
{
    public class Dnl_Link_WeiXi
    {
        /// <summary>
        /// mongodb唯一标识Id
        /// </summary>
        public ObjectId _id { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 搜索关键词Id
        /// </summary>
        public string KeywordId { get; set; }
        /// <summary>
        /// 平台内公众号Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string Table_name { get; set; }
        /// <summary>
        /// 数据表Id
        /// </summary>
        public int Talbe_id { get; set; }
        /// <summary>
        /// 公众号官方呢称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 微信公众号账号
        /// </summary>
        public string Wx_name { get; set; }
        /// <summary>
        /// 平台内公众号Id
        /// </summary>
        public int Nickname_id { get; set; }
        /// <summary>
        /// 微信文章发布时间
        /// </summary>
        public string Posttime { get; set; }
        /// <summary>
        /// 微信文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 微信文章正文摘要
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 微信文章正文
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 微信文章url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据获取状态[1表示已获取,0表示未获取]
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 微信文章入库时间
        /// </summary>
        public string Add_time { get; set; }
        /// <summary>
        /// 微信文章获取时间
        /// </summary>
        public string Get_time { get; set; }
        /// <summary>
        /// 微信文章阅读数
        /// </summary>
        public int Readnum { get; set; }
        /// <summary>
        /// 微信文章点赞数
        /// </summary>
        public int Likenum { get; set; }
        /// <summary>
        /// 微信文章获取时间（下午）
        /// </summary>
        public string Get_time_pm { get; set; }
        /// <summary>
        /// 微信文章阅读数（下午）
        /// </summary>
        public int Readnum_pm { get; set; }
        /// <summary>
        /// 微信文章点赞数（下午）
        /// </summary>
        public int Likenum_pm { get; set; }
        /// <summary>
        /// 微信周阅读数获取时间
        /// </summary>
        public string Get_time_week { get; set; }
        /// <summary>
        /// 微信文章阅读数（周）
        /// </summary>
        public int Readnum_week { get; set; }
        /// <summary>
        /// 微信文章点赞数（周）
        /// </summary>
        public int Likenum_week { get; set; }
        /// <summary>
        /// 文章位置
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 是否同步
        /// </summary>
        public int Ispush { get; set; }
        /// <summary>
        /// 微信文章中图片地址
        /// </summary>
        public string Picurl { get; set; }
        /// <summary>
        /// 原文地址
        /// </summary>
        public string Sourceurl { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 是否原创（原创|非原创|未知）
        /// </summary>
        public string Copyright { get; set; }
        /// <summary>
        /// 搜索引擎名称
        /// </summary>
        public string Index_name { get; set; }

    }
}
