using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CSharpTest.Model
{
    /// <summary>
    /// 公用关键词库类
    /// </summary>
    public class Dnl_Keyword_Media
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 任务间隔小时
        /// </summary>
        public int BotIntervalHours { get; set; }
        /// <summary>
        /// 微信上次搜索时间
        /// </summary>
        public DateTime LastBotEndAt_WX { get; set; }
        /// <summary>
        /// 微信文章总数
        /// </summary>
        public int WXArticleNumTotal { get; set; }
        /// <summary>
        /// 微信已搜索文章数
        /// </summary>
        public int WXArticleNumNow { get; set; }

    }
}
