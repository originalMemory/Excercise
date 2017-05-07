using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Models
{
    public class XShop:XBase 
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 主营
        /// </summary>
        public string SalesProducts { get; set; }
        /// <summary>
        /// 店铺所在地
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 宝贝数
        /// </summary>
        public int? ItemCount { get; set; }
        /// <summary>
        /// 月成交数
        /// </summary>
        public int? MonthDeals { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public string ContactUrl { get; set; }
        /// <summary>
        /// 信用级别
        /// </summary>
        public string Credit { get; set; }
        /// <summary>
        /// 好评率
        /// </summary>
        public double? GoodCommentRate { get; set; }

        public bool? IsTmall { get; set; }

        public bool? IsAuthorized { get; set; }  
    }

    public class Store
    {
        public Guid _id { get; set; } 
        public string CompanyName { get; set; }
        public string ContactUrl { get; set; }
        public string ShopName { get; set; }
        public string SiteName { get; set; }
        public Dictionary<string, string> NameValues { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Loc { get; set; }
        public string Credit { get; set; }
    }
}
