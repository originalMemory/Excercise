using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Models
{
    public class XItem:XBase 
    {
        /// <summary>
        /// 宝贝名称
        /// </summary>
        public string ItemName { get; set; }

        public double? Price { get; set; }

        public int? Sold30Days { get; set; }

        public int? CommentCount { get; set; }

        public string DetailUrl { get; set; }

        public string Brand { get; set; }

        public string Product { get; set; }

        public string ImgUrl { get; set; }

        public bool? CanReturn { get; set; }

        public bool? IsGenuines { get; set; }
    }

    public class Item
    {
        public Guid _id { get; set; }//Singnature 
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string DetailUrl { get; set; }
        public string ItemId { get; set; }
        public Guid? ShopID { get; set; }
        public string SiteName { get; set; }
        public Dictionary<string, string> NameValues { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public string Loc { get; set; }
        public double? Price { get; set; }
        public bool? BotBuyerList { get; set; }
        public int? BuyerCount { get; set; }
        public DateTime? LastBuyerListBotAt { get; set; }//
        public string BuyerListQueryName { get; set; }
        public bool? BotComments { get; set; }//
        public int? CommentsCount { get; set; }//
        public string CommentQueryName { get; set; }
        public DateTime? LastCommentBotAt { get; set; }//
        public byte BotStatus { get; set; }//
        public DateTime? LastDetailBotAt { get; set; }//
        public string DetailQueryName { get; set; }//
        public DateTime? CreatedAt { get; set; }//
        public DateTime? ClosedAt { get; set; }
        public string Img { get; set; }
        public string BrandName { get; set; }
        public string ProductName { get; set; }
        public string RealBrandName { get; set; }
        public string RealProductName { get; set; }

    }

    public class Buyer
    {
        public Guid _id { get; set; }
        public Guid? ItemID { get; set; }
        public string BuyerName { get; set; }
        public DateTime? BuyAt { get; set; }
        public int? Num { get; set; }
        public double? Price { get; set; }
        public string CompanyName { get; set; }
        public string BuyerContactUrl { get; set; }      
    }
    public class BuyerLike
    {
        public Guid _id { get; set; }
        public Guid? BuyerEntityID { get; set; }
        public string LikeItemTitle { get; set; }
        public double? LikeItemPrice { get; set; }
        public string LikeAt { get; set; }
        public string LikeItemUrl { get; set; }
        public void SetID()
        {
            _id = AISSystem.IDHelper.GetGuid(string.Format("{0},{1}", BuyerEntityID, LikeItemTitle));
        }
    }

    public class BuyerEntity  
    {
        public Guid _id { get; set; }         
        public string BuyerName { get; set; }      
        public string BuyerContactUrl { get; set; }  
        public string SiteName { get; set; }
        public string QueryName { get; set; }
        public Dictionary<string, string> NameValues { get; set; }
        public List<BuyerLike> Likes { get; set; }
        public DateTime? LastBotAt { get; set; }
        public string AvatarImg { get; set; }
        public void SetID()
        {
            if (!string.IsNullOrEmpty(BuyerName) && !string.IsNullOrEmpty(SiteName))
                _id = AISSystem.IDHelper.GetGuid(string.Format("{0},{1}", BuyerName, SiteName));
            else if (!string.IsNullOrEmpty(BuyerContactUrl))
                _id = AISSystem.IDHelper.GetGuid(BuyerContactUrl); 
        }
    }

    public class Comment
    {
        public Guid _id { get; set; }
        public Guid? ItemID { get; set; }
        public string Poster { get; set; }
        public DateTime? PostAt { get; set; }
        public string PostContent { get; set; }
        public string CompanyName { get; set; }
        public string PosterContactUrl { get; set; }
        public int? Stars { get; set; }
    }
}
