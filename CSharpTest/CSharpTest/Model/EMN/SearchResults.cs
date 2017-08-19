using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AISSystem;
using IprseeData;

namespace cPlusPlusTest.Models
{
    public class SnapSearchResult
    { 
        public List<XListing> Listings = new List<XListing>();
        public List<Link> SubTasks = new List<Link>();
        public Link NextPage { get; set; }

        public void SetID(XTask tsk)
        { 
            Listings.ToList().ForEach(x =>
            {
                if (tsk != null)
                {
                    x.CompanyName = tsk.CompanyName;
                    x.SiteName = tsk.SiteName;
                    x.DetailQueryName = tsk.DetailQueryName;
                    x.CommentQueryName = tsk.CommentQueryName;
                    x.BuyerListQueryName = tsk.BuyerListQueryName;
                    x.BotComments = tsk.BotComments;
                    x.BotBuyerList = tsk.BotBuyerList;
                    x.RealBrandName = tsk.BrandName;
                    x.RealProductName = tsk.ProductName;
                    if (tsk.ItemID.HasValue)
                        x.BotItemID = tsk.ItemID.Value;
                    if (tsk.ShopID.HasValue)
                        x.BotShopID = tsk.ShopID.Value;
                }

                if (!x.BotShopID.HasValue && !string.IsNullOrEmpty(x.ShopName))
                    x.BotShopID = IDHelper.GetGuid(string.Format("{0},{1}", x.ShopID ?? x.ShopName, x.SiteName));
                if (!x.BotItemID.HasValue && !string.IsNullOrEmpty(x.ItemName))
                {
                    if(!string.IsNullOrEmpty( x.ItemID))
                         x.BotItemID = IDHelper.GetGuid(string.Format("{0},{1}",x.ItemID ,x.SiteName));
                    else if(x.BotShopID .HasValue )
                         x.BotItemID = IDHelper.GetGuid(string.Format("{0},{1}",x.ItemName ,x.BotShopID ));
                    else if(!string.IsNullOrEmpty(x.ItemDetailUrl))
                        x.BotItemID =IDHelper .GetGuid (x.ItemDetailUrl);
                    else 
                        x.BotItemID =IDHelper.GetGuid(string.Format("{0},{1}",x.ItemName ,x.SiteName));
                }
                if (x.ItemBotStatus == BotStatus.Removed)
                    x.ClosedAt = DateTime.Now;
                x.ItemCommentList.ForEach(y => 
                {
                    y.EntityID = x.BotItemID;
                    y.ID = IDHelper.GetGuid(string.Format("{0},{1},{2}", x.BotItemID, y.Poster, y.PostAt));
                });
                x.SalesRecords.ForEach(y => 
                {
                    y.PackageID = x.BotItemID; 
                });
            });
        }

        public List<Store> GetStores()
        {
            List<Store> stores = new List<Store>();
            var gs = Listings.Where(x => x.BotShopID.HasValue).GroupBy(x => x.BotShopID);
            if (gs == null)
                return stores;
            foreach (var g in gs)
            {
                Store s = new Store
                {
                    _id = g.Key.Value,
                    CompanyName = g.Select(x => x.CompanyName).FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                    ContactUrl = g.Select(x => x.ShopContactUrl).FirstOrDefault(x => !string.IsNullOrEmpty(x)).ReplaceWith("amp;","").GetTrimed(),
                    Loc = g.SelectMany(x => new string[] { x.ItemLocation, x.ShopLocation }).FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                    ShopName = g.Select(x => x.ShopName).FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                    SiteName = g.Select(x => x.SiteName).FirstOrDefault(x => !string.IsNullOrEmpty(x)),
                    Credit=g.Select(x=>x.ShopCredit ).FirstOrDefault(x=>!string.IsNullOrEmpty(x)),
                };
                s.NameValues = new Dictionary<string, string>();
                g.SelectMany(x => x.ShopNameValues).ToList().ForEach(kvp =>
                {
                    if (!s.NameValues.ContainsKey(kvp.Key))
                        s.NameValues.Add(kvp.Key, kvp.Value);
                });
                foreach (var i in g)
                {
                    TryAddNameValue(s.NameValues, "Phone", i.ShopPhone);
                    TryAddNameValue(s.NameValues, "Address", i.ShopAddress);
                    TryAddNameValue(s.NameValues, "City", i.ShopCity);
                    TryAddNameValue(s.NameValues, "Contacts", i.ShopContacts);
                    TryAddNameValue(s.NameValues, "Credit", i.ShopCredit);
                    TryAddNameValue(s.NameValues, "Fax", i.ShopFax);
                    TryAddNameValue2(s.NameValues, "GoodCommentRate", i.ShopGoodCommentRate );
                    TryAddNameValue2(s.NameValues, "ItemCount", i.ShopItemCount);
                    TryAddNameValue2(s.NameValues, "ItemMatchedScore", i.ShopItemMatchedScore);
                    TryAddNameValue2(s.NameValues, "MonthDeals", i.ShopMonthDeals);
                    TryAddNameValue2(s.NameValues, "ServiceScore", i.ShopServiceScore);
                    TryAddNameValue2(s.NameValues, "SetupAt", i.ShopSetupAt);
                }
                stores.Add(s);
            }
            return stores;
        }

        public List<Item> GetItems()
        {
            List<Item> result = new List<Item>();
            foreach (var l in Listings)
            {
                if(!l.BotItemID.HasValue )
                    continue ;
                Item it = new Item
                {
                    _id = l.BotItemID.Value,
                    BotBuyerList = l.BotBuyerList,
                    BotComments = l.BotComments,
                    BotStatus = (byte)(l.ItemBotStatus == BotStatus.Removed ? 0 : l.ItemBotStatus == BotStatus.ServerError ? 3 : 4),
                    BuyerCount = l.ItemSold30Days,
                    BuyerListQueryName = l.BuyerListQueryName,
                    ClosedAt = l.ClosedAt,
                    CommentQueryName = l.CommentQueryName,
                    CommentsCount = l.ItemTotalCommentCount,
                    CompanyName = l.CompanyName,
                    DetailQueryName = l.DetailQueryName,
                    DetailUrl = l.ItemDetailUrl.ReplaceWith("amp;", "").GetTrimed(),
                    Img = l.ItemImgUrl,
                    ItemId = l.ItemID,
                    Loc = l.ItemLocation ?? l.ShopLocation,
                    Price = l.ItemPrice,
                    ShopID = l.BotShopID,
                    SiteName = l.SiteName,
                    Title = l.ItemName,
                    BrandName =l.ItemBrandName,
                    ProductName =l.ItemProductName ,
                    RealBrandName =l.RealBrandName ,
                    RealProductName =l.RealProductName ,                    
                };
                it.NameValues = new Dictionary<string, string>();
                TryAddNameValue2(it.NameValues, "IsGenuines", l.ItemIsGenuines);
                TryAddNameValue2(it.NameValues, "CanReturn", l.ItemCanReturn);
                TryAddNameValue2(it.NameValues, "Condition", l.ItemCondition);
                TryAddNameValue2(it.NameValues, "GoodCommentPeercent", l.ItemGoodCommentPercent);
                TryAddNameValue2(it.NameValues, "Currency", l.ItemPriceCurrency);
                TryAddNameValue2(it.NameValues, "Quantity", l.ItemQuantityInStock);
                TryAddNameValue2(it.NameValues, "Reviews", l.ItemReviews);
                foreach (var kvp in l.ItemPropertyNameValues)
                    TryAddNameValue(it.NameValues, kvp.Key, kvp.Value);
                result.Add(it);

            }
            return result;
        }

        public List<Buyer> GetBuyers()
        {
            List<Buyer> buyers = new List<Buyer>();
            foreach (var l in Listings )
            {
                l.SalesRecords.ForEach(x =>
                {
                    Buyer by = new Buyer
                    {
                        _id = x.GetID(),
                        BuyAt = x.SettleDT,
                        BuyerContactUrl = x.BuyerContactUrl,
                        BuyerName = x.Buyer,
                        CompanyName = l.CompanyName,
                        ItemID = l.BotItemID ?? x.PackageID ,
                        Num = x.Count,
                        Price = x.Price,
                    };
                    buyers .Add (by);
                });
            }
            return buyers;
        }

        public List<Comment> GetComments()
        {
            List<Comment> coms = new List<Comment>();
            foreach (var l in Listings )
            {
                l.ItemCommentList.ForEach(x =>
                {
                    Comment com = new Comment
                    {
                        _id = x.ID,
                        CompanyName = l.CompanyName,
                        ItemID = l.BotItemID,
                        PostAt = x.PostAt.ToDateTime3(),
                        PostContent = x.Content,
                        Poster = x.Poster,
                        PosterContactUrl = x.PosterContactUrl ,
                        Stars =x.Stars ,
                    };
                    coms.Add(com);
                });
            }
            return coms;
        }

        void TryAddNameValue2(Dictionary<string, string> nameValues, string key, object value)
        {
            TryAddNameValue(nameValues, key, string.Format("{0}", value));
        }

        void TryAddNameValue(Dictionary<string, string> nameValues, string key, string value) 
        { 
            if(string.IsNullOrEmpty(value))
                return ;
            if (!nameValues.ContainsKey(key))
            {
                nameValues.Add(key, value);
                return;
            }
            if (!nameValues[key].IsContain(value))
                nameValues[key] = string.Format("{0},{1}", nameValues[key], value);
        }
    }
}
