using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleTest.Model
{
    public class TimeLinkCountDto
    {
        public List<DateTime> Times { get; set; }
        
        public List<LinkInfo> linkInfo { get; set; }
        /// <summary>
        /// 精简版自动摘要
        /// </summary>
        public List<SumData> Sum { get; set; }
        
    }

    public class LinkInfo
    {
        public string name { get; set; }
        public List<int> LinkCount { get; set; }
        public List<TopData> topData { get; set; }
        public string CategoryId { get; set; }
    }

    public class TopData
    {
        public DateTime X { get; set; }
        public int Y { get; set; }
        public string name { get; set; }
        public string CategoryId { get; set; }
    }

    public class SumData
    {
        public DateTime X { get; set; }
        public int Y { get; set; }
        public string Summary { get; set; }
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        
    }

    public class CategoryTime
    {
        public DateTime PublishTime { get; set; }
        public string CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
    }

    public class CategoryList
    {
        public List<DateTime> PublishTime { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
