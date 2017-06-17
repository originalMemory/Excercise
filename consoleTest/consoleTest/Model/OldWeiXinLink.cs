using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CSharpTest.Model
{
    /// <summary>
    /// 公有微信链接类
    /// </summary>
    public class OldWeiXinLinkMongo
    {
        /// <summary>
        /// mongodb唯一标识Id
        /// </summary>
        public ObjectId _id { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 搜索关键词Id
        /// </summary>
        public string KeywordId { get; set; }
        /// <summary>
        /// 公众号官方呢称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 微信公众号账号
        /// </summary>
        public string WXName { get; set; }
        /// <summary>
        /// 微信文章发布时间
        /// </summary>
        public DateTime PostTime { get; set; }
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
        /// 微信文章正文长度
        /// </summary>
        public int ContentLen { get; set; }
        /// <summary>
        /// 微信文章网页源码
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 微信文章url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 数据获取状态[1表示已获取,0表示未获取]
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 微信文章入库时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 微信文章获取时间
        /// </summary>
        public DateTime GetTime { get; set; }
        /// <summary>
        /// 微信文章阅读数
        /// </summary>
        public int ReadNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNum { get; set; }
        /// <summary>
        /// 微信文章点赞数
        /// </summary>
        public int LikeNum { get; set; }
        /// <summary>
        /// 微信文章获取时间（下午）
        /// </summary>
        public DateTime GetTimePm { get; set; }
        /// <summary>
        /// 微信文章阅读数（下午）
        /// </summary>
        public int ReadNumPM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNumPM { get; set; }
        /// <summary>
        /// 微信文章点赞数（下午）
        /// </summary>
        public int LikeNumPM { get; set; }
        /// <summary>
        /// 微信文章阅读数（周）
        /// </summary>
        public int ReadNumWeek { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNumWeek { get; set; }
        /// <summary>
        /// 微信文章点赞数（周）
        /// </summary>
        public int LikeNumWeek { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ReadNumNewest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LikeNumNewest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime GetTimeNewest { get; set; }
        /// <summary>
        /// 文章位置
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 微信文章中图片地址
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 原文地址
        /// </summary>
        public string SourceUrl { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// 是否原创（原创|非原创|未知）
        /// </summary>
        public string Copyright { get; set; }
        public bool IsDelByAu { get; set; }
    }


    /// <summary>
    /// 公有微信链接类
    /// </summary>
    public class OldWeiXinLinkDto
    {
        /// <summary>
        /// mongodb唯一标识Id
        /// </summary>
        public ObjectId _id { get; set; }
        /// <summary>
        /// 微信文章标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 微信文章正文
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 微信文章正文长度
        /// </summary>
        public int ContentLen { get; set; }
        /// <summary>
        /// 微信文章url地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 微信文章获取时间
        /// </summary>
        public DateTime GetTime { get; set; }
        /// <summary>
        /// 微信文章阅读数
        /// </summary>
        public int ReadNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNum { get; set; }
        /// <summary>
        /// 微信文章点赞数
        /// </summary>
        public int LikeNum { get; set; }
        /// <summary>
        /// 微信文章获取时间（下午）
        /// </summary>
        public DateTime GetTimePm { get; set; }
        /// <summary>
        /// 微信文章阅读数（下午）
        /// </summary>
        public int ReadNumPM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNumPM { get; set; }
        /// <summary>
        /// 微信文章点赞数（下午）
        /// </summary>
        public int LikeNumPM { get; set; }
        /// <summary>
        /// 微信文章阅读数（周）
        /// </summary>
        public int ReadNumWeek { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RealReadNumWeek { get; set; }
        /// <summary>
        /// 微信文章点赞数（周）
        /// </summary>
        public int LikeNumWeek { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ReadNumNewest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int LikeNumNewest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime GetTimeNewest { get; set; }
        public bool IsDelByAu { get; set; }
    }
}
