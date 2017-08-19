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
