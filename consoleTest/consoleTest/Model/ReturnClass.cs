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
}
