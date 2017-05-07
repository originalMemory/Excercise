using IprseeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Models
{
    public class Link
    {
        public string Url { get; set; }
        public string PostData { get; set; }
        public string QueryName { get; set; }

        public string Domain { get; set; }
        public Guid? EntityID { get; set; }
         
    }

    public class XTask
    {
        public Guid _id { get; set; }
        public string QueryName { get; set; }
        public string CompanyName { get; set; }
        public bool? BotComments { get; set; }
        public bool? BotBuyerList { get; set; }
        public string Url { get; set; }
        public string PostData { get; set; }
        public string SiteName { get; set; }
        public DateTime? LastBotAt { get; set; }   
		public string LastUpdatedBy{get;set;}  
		public int? TotalItems{get;set;}
		public double? MAP{get;set;}
        public string ProductName{get;set;}
        public string BrandName{get;set;}
        public int?  WeeklySoldNum{get;set;}
        public int? TotalComments { get; set; }
        public bool? IsECMGroup { get; set; }
        public string CreatedBy { get; set; }
        public BotStatus BotStatus { get; set; }
        public string DetailQueryName { get; set; }//
        public string CommentQueryName { get; set; }
        public string BuyerListQueryName { get; set; }
        public Guid? ItemID { get; set; }
        public Guid? ShopID { get; set; }

        public BotTask ToBotTask()
        {
            BotTask tsk = new BotTask
            {
                ID = _id,
                SiteName = SiteName,
                AddtionalParamters = Url,
                QueryName = QueryName, 

            };
            return tsk;
        }

        public void SetID()
        {
            _id = AISSystem.IDHelper.GetGuid(string.Format("{0}/{1}", CompanyName, Url));
        }
    }
}
