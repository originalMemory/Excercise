using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    /// <summary>
    /// 微信时间标题数据
    /// </summary>
    public class WeiXinTimelinkDto
    {
        /// <summary>
        /// 链接Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 微信公众号呢称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }

    }
}
