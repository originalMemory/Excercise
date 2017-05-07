using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Model
{
    public class IW2S_Project
    {
        public ObjectId _id { get; set; }
        public ObjectId UsrId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsDel { get; set; }
        public int BaiduLinkCount { get; set; }
        public int LinksCount { get; set; }
        /// <summary>
        /// 任务间隔小时
        /// </summary>
        public int BotIntervalHours { get; set; }
        /// <summary>
        /// 下次搜索开始时间
        /// </summary>
        public DateTime NextBotStartAt { get; set; }

        /// <summary>
        /// 上次搜索开始时间
        /// </summary>
        public DateTime LastBotEndAt { get; set; }

        /// <summary>
        /// 1:搜索中,2:搜索完成
        /// </summary>
        public int BotStatus { get; set; }
        /// <summary>
        /// IP.ProcessId
        /// </summary>
        public string BotTag { get; set; }
        public string BotId { get; set; }
    }

    public class IW2S_ProjectDto
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int KeywordCount { get; set; }
        public List<string> SharerEmail { get; set; }
        public DateTime ShareTime { get; set; }
        public int LinkCount { get; set; }
        public List<int> CountList { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class IW2S_ProLinksCount
    {
        public ObjectId _id { get; set; }
        public ObjectId UsrId { get; set; }
        public ObjectId ProjectId { get; set; }
        public int LinksCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
