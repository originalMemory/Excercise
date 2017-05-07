using ProxyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AISSystem;
using IprseeBot.Models;
using IprseeData.EF_Sql;
using System.Xml.Linq;
using IprseeData;
using System.Web;
using IprseeBot.Helper;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Threading;
using System.Text.RegularExpressions;

namespace IprseeBot.Template
{
    public class TaoBaoQuery
    {
        WebHelperNoCookieProxy proxy = new WebHelperNoCookieProxy();
        HTML.WebHelper web = new HTML.WebHelper();
        string nick_name;
        static int count = 0;
        static object baidu_token = new object();
        int pages = 0;
        public List<XListing> Listings = new List<XListing>();
        public List<Link> SubTasks = new List<Link>();
        public int NextPage { get; set; }

        public TaoBaoQuery(string _nick_name)
        {
            lock (baidu_token)
            {
                count++;
                nick_name = _nick_name ?? ("anonymous_" + count);
            }
        }


        public object Query(FreeTask tsk, string recordId)
        {
            SnapSearchResult result = null;
            result = new SnapSearchResult();


            result = GetLinks(result, tsk, recordId);

            return result;
        }



        SnapSearchResult GetLinks(SnapSearchResult result, FreeTask tsk, string recordId)
        {
            List<XListing> xListings = new List<XListing>();

            string link = "https://s.taobao.com/search?q={0}&ie=utf8&sort=default".FormatStr(tsk.TaskName);
            //   string link = "https://s.taobao.com/search?q={0}&ie=utf8&sort=default".FormatStr("连衣裙冬");
            int nohist_pages = 0;
            int quried_pages = 1;
            int Position = 1;
            //最多搜索20页 
            while (!string.IsNullOrEmpty(link) && quried_pages <= 20)
            {
                log(link);
                var html = TaobaoWebHelper.GetSnapshotHtml(link); ;
                try
                {
                    if (html != null)
                    {
                        var tagslist = html.SubAfter("itemlist").SubBefore("recommendAuctions");
                        var tags = tagslist.SubAfter("p4pTags").SplitWith("p4pTags");

                        if (tags == null || tags.Length == 0)
                        {
                            log("BLOCKED " + tsk);
                            break;
                        }
                        bool nohit = true;

                        foreach (var tag in tags)
                        {

                            try
                            {
                                if (!tag.Contains("raw_title"))
                                {
                                    Console.WriteLine(DateTime.Now);
                                    break;
                                }
                                if (tag == null || tag == "" || tag.Trim().Length == 0)
                                {
                                    Console.WriteLine(DateTime.Now);
                                    break;
                                }
                                var a = tag.SubAfter("raw_title").SubBefore("pic_url");
                                string title = RemoveChar(a.GetLower());
                                string nid = RemoveChar(tag.SubAfter("nid").SubBefore("category"));
                                string pic_url = RemoveChar(tag.SubAfter("pic_url").SubBefore("detail_url"));
                                string detail_url = "https:" + RemoveChar(tag.SubAfter("detail_url").SubBefore("view_price"));
                                detail_url = Regex.Unescape(detail_url);
                                var view_price = RemoveChar(tag.SubAfter("view_price").SubBefore("view_fee"));
                                double price = 0;
                                if (!string.IsNullOrEmpty(view_price))
                                {
                                    price = Convert.ToDouble(view_price);
                                }
                                string item_loc = RemoveChar(tag.SubAfter("item_loc").SubBefore("reserve_price"));
                                string view_sales = RemoveChar(tag.SubAfter("view_sales").SubBefore("comment_count")).Replace("人收货", "").Replace("人付款", "");
                                int days30 = 0;
                                if (!string.IsNullOrEmpty(view_sales))
                                {
                                    days30 = Convert.ToInt32(view_sales);
                                }
                                string comment_count = RemoveChar(tag.SubAfter("comment_count").SubBefore("user_id"));
                                int commentcount = 0;
                                if (!string.IsNullOrEmpty(comment_count))
                                {
                                    commentcount = Convert.ToInt32(comment_count);
                                }
                                //shop
                                string user_id = RemoveChar(tag.SubAfter("user_id").SubBefore("nick"));
                                string nick = RemoveChar(tag.SubAfter("nick").SubBefore("shopcard"));
                                string isTmall = RemoveChar(tag.SubAfter("isTmall").SubBefore("delivery"));
                                string delivery = RemoveChar(tag.SubAfter("delivery").SubBefore("description"));
                                string description = RemoveChar(tag.SubAfter("description").SubBefore("service"));
                                string sellerCredit = RemoveChar(tag.SubAfter("sellerCredit").SubBefore("totalRate"));
                                string siteName = "taobao";
                                Guid siteId = Guid.Parse("A00A672B-DD05-65FB-4EE0-CFA26EBF2ED5");
                                var totalRate = RemoveChar(tag.SubAfter("totalRate").SubBefore("icon").GetLower());
                                var shopLink = RemoveChar(tag.SubAfter("shopLink").SubBefore("}"));
                                shopLink = "https:" + shopLink;
                                shopLink = Regex.Unescape(shopLink);
                                XListing listing = new XListing
                                {
                                    ShopContactUrl = shopLink,
                                    ItemDetailUrl = detail_url,
                                    ItemPrice = price,
                                    ItemName = title,
                                    ItemID = nid,
                                    ItemLocation = item_loc,
                                    ItemSold30Days = days30,
                                    Itempic = pic_url,
                                    ItemTotalCommentCount = commentcount,
                                    UId = tsk.UId,
                                    ShopID = user_id,
                                    ShopName = nick,
                                    ShopLocation = item_loc,
                                    ShopIsTmall = isTmall == "true" ? true : false,
                                    taskid = tsk._id,
                                    taskName = tsk.TaskName,
                                    SiteName = siteName,
                                    SiteID = siteId,
                                    usrid = tsk.UsrId,
                                    ShopIsAuthorized = false,
                                    Position = Position,
                                    PageNum = quried_pages,
                                    ProjectId = tsk.ProjectId
                                };
                                if (listing.ItemDetailUrl != null && listing.ItemName != null)
                                    listing.ItemBotStatus = BotStatus.Ok;
                                result.Listings.Add(listing);
                                xListings.Add(listing);
                                nohit = false;
                                nohist_pages = 0;
                                Position++;
                            }
                            catch (Exception we)
                            {
                                Console.WriteLine(DateTime.Now + "错误：" + we.Message);
                                break;
                            }
                        }

                        if (nohit)
                            nohist_pages++;
                        //如果连续3页都没有结果，就跳出
                        if (nohist_pages > 3)
                            break;

                        quried_pages++;
                        pages++;
                        NextPage = NextPage + 44;

                        //  link = "https://s.taobao.com/search?q={0}&sort=sale-desc&s={1}".FormatStr(tsk.TaskName, NextPage);
                        //
                        link = "https://s.taobao.com/search?q={0}&ie=utf8&sort=default&s={1}".FormatStr(tsk.TaskName, NextPage);
                        Console.WriteLine(DateTime.Now + "任务名：" + tsk.TaskName + "；开始搜索第" + quried_pages + "页");

                        SaveResult(xListings, BotTypes.ItemSnapshot, recordId, tsk);
                        xListings.Clear();

                        int n = new Random().Next(3000, 6000);
                        Thread.Sleep(n);

                    }
                    else
                    {
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(DateTime.Now + "错误：" + ex.Message);
                    break;
                }
            }
            return result;
        }

        void SaveResult(List<XListing> listings, BotTypes botType, string recordId, FreeTask tsk)
        {
            listings.ToList().ForEach(x =>
            {
                if (string.IsNullOrEmpty(x.ShopName) && !string.IsNullOrEmpty(x.SiteName) && !x.BotShopID.HasValue
                    && !x.SiteName.ToLower().Contains("taobao") && !x.SiteName.ToLower().Contains("alibaba"))
                {
                    x.ShopName = x.SiteName;
                    x.BotShopID = x.SiteID;
                }
                if (!x.BotShopID.HasValue && !string.IsNullOrEmpty(x.ShopName))
                    x.BotShopID = IDHelper.GetGuid(string.Format("{0},{1},{2},{3}", x.ShopName, x.SiteName, tsk._id, x.ShopID));
            });
            var shopList = listings;
            shopList = shopList.DistinctBy(x => x.BotShopID);
            //var exists_ids = MySqlDbHelper.GetExsitsIds<Guid?>(com, "bot_shops", "Shop_id", shopList.Select(x => x.BotShopID).ToArray());
            //if (exists_ids != null && exists_ids.Count > 0)
            //{
            //    shopList = shopList.Where(x => !exists_ids.Contains(x.BotShopID)).ToList();
            //}

            FieldsDocument shopfd = new FieldsDocument();
            shopfd.Add("BotShopID", 1);
            // MongoCollection<Guid> shopcol = MongoDBHelper<Guid>.GetMongoDB().GetCollection<Guid>("FreeBotShop");
            var shopcol = MongoDBHelper.Instance.Get_FreeBotShop();
            var builder = Builders<FreeBotShop>.Filter;

            List<Guid?> BotShopID = shopList.Select(x => x.BotShopID).ToList();
            //  var existsshop_objs = shopcol.Find(MongoDB.Driver.Builders.Query.In("BotShopID", new BsonArray(BotShopID))).SetFields(shopfd);
            var existsshop_objs = shopcol.Find(builder.In(x => x.Shop_id, BotShopID)).Project(x => x.Shop_id).ToList();
            List<Guid?> exists_ids = new List<Guid?>();
            foreach (var result in existsshop_objs)
            {
                exists_ids.Add(result);
            }
            if (exists_ids != null && exists_ids.Count > 0)
            {
                shopList = shopList.Where(x => !exists_ids.Contains(x.BotShopID)).ToList();
            }
            if (shopList == null || shopList.Count == 0)
                return;
            List<FreeBotShop> dt = null;
            if (shopList.Count > 0)
            {
                dt = GetShopList(shopList);
                //  var saved = MongoDBHelper<FreeBotShop>.BatchInsertData(dt, "FreeBotShop");
                shopcol.InsertMany(dt);
                log("to save bot_shops");
                log("Done");
            }
            listings.ToList().ForEach(x =>
            {
                if (!x.BotItemID.HasValue && !string.IsNullOrEmpty(x.ItemName))
                    x.BotItemID = IDHelper.GetGuid(string.Format("{0},{1},{2},{3}", x.ItemName, x.SiteName, tsk._id, x.ItemID));
            });
            var itemList = listings;
            itemList = itemList.DistinctBy(x => x.BotItemID);
            //var exists_itemids = MySqlDbHelper.GetExsitsIds<Guid?>(com, "bot_items", "ItemId", itemList.Select(x => x.BotItemID).ToArray());
            //if (exists_itemids != null && exists_itemids.Count > 0)
            //{
            //    itemList = itemList.Where(x => !exists_itemids.Contains(x.BotItemID)).ToList();
            //}

            FieldsDocument fd = new FieldsDocument();
            fd.Add("BotItemID", 1);
            //   MongoCollection<Guid> col = MongoDBHelper<Guid>.GetMongoDB().GetCollection<Guid>("FreeBotItem");
            var col = MongoDBHelper.Instance.Get_FreeBotItem();
            var itemBuilder = Builders<FreeBotItem>.Filter;
            List<Guid?> BotItemID = itemList.Select(x => x.BotItemID).ToList();
            //  var exists_objs = col.Find(MongoDB.Driver.Builders.Query.In("BotItemID", new BsonArray(BotItemID))).SetFields(fd);
            var exists_objs = col.Find(itemBuilder.In(x => x.ItemId, BotItemID)).Project(x => x.ItemId).ToList();
            List<Guid?> existsitem_ids = new List<Guid?>();
            foreach (var result in exists_objs)
            {
                existsitem_ids.Add(result);
            }
            if (existsitem_ids != null && existsitem_ids.Count > 0)
            {
                itemList = itemList.Where(x => !existsitem_ids.Contains(x.BotItemID)).ToList();
            }

            List<XListing> updatelinks = new List<XListing>();
            if (existsitem_ids != null && existsitem_ids.Count > 0)
            {
                updatelinks = updatelinks.Where(x => existsitem_ids.Contains(x.BotItemID)).ToList();
            }
            update_level1_links(updatelinks, botType, recordId, tsk);
            if (itemList == null || itemList.Count == 0)
                return;

            var itemdt = GetItemList(itemList);
            //  var savedListings = MySqlDbHelper.BatchInsert(con, "bot_items", itemdt);
            // var savedListings = MongoDBHelper<FreeBotItem>.BatchInsertData(itemdt, "FreeBotItem");
            col.InsertMany(itemdt);

            // var wequery = new QueryDocument { { "_id", new ObjectId(recordId) } };
            // FreeTaskRecord TaskList = MongoDBHelper<FreeTaskRecord>.Find1("FreeTaskRecord", wequery);
            var colRecord = MongoDBHelper.Instance.Get_FreeTaskRecord();
            var RecordBuilder = Builders<FreeTaskRecord>.Filter;
            FreeTaskRecord TaskList = colRecord.Find(RecordBuilder.Eq(x => x._id, new ObjectId(recordId))).FirstOrDefault();
            int LinksNum = 0;
            int ShopsNum = 0;
            LinksNum = TaskList.LinksNum + itemdt.Count;
            ShopsNum = TaskList.ShopsNum + dt.Count;
            var updateWebsiteCount = new UpdateDocument { { "$set", new QueryDocument { { "LinksNum", LinksNum }, { "ShopsNum", ShopsNum } } } };
            //  MongoDBHelper<FreeTaskRecord>.Update("FreeTaskRecord", wequery, updateWebsiteCount);
            MongoDBHelper.Instance.Get_FreeTaskRecord().UpdateOne(new QueryDocument { { "_id", new ObjectId(recordId) } }, updateWebsiteCount);


            var colTask = MongoDBHelper.Instance.Get_FreeTask();
            var TaskBuilder = Builders<FreeTask>.Filter;
            FreeTask Task2List = colTask.Find(TaskBuilder.Eq(x => x._id, itemdt[0].taskId)).FirstOrDefault();
            int TaskLinksNum = 0;
            int TaskShopsNum = 0;
            TaskLinksNum = Task2List.LinksNum + itemdt.Count;
            TaskShopsNum = Task2List.ShopsNum + dt.Count;
            var TaskupdateWebsiteCount = new UpdateDocument { { "$set", new QueryDocument { { "LinksNum", TaskLinksNum }, { "ShopsNum", TaskShopsNum } } } };
            //  MongoDBHelper<FreeTaskRecord>.Update("FreeTaskRecord", wequery, updateWebsiteCount);
            MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", itemdt[0].taskId } }, TaskupdateWebsiteCount);

            log("to save listings");
            log("Done");
        }


        private void update_level1_links(List<XListing> listings, BotTypes botType, string recordId, FreeTask tsk)
        {
            if (listings == null || listings.Count == 0)
            {
                log("SUCCESS update 0 apps Links for " + tsk.TaskName);
                return;
            }
            var itemdt = GetItemList(listings);
            for (int i = 0; i < itemdt.Count; i++)
            {
                var update = new UpdateDocument { { "$set", new QueryDocument { { "PageNum", itemdt[i].PageNum },{"TotalComments",itemdt[i].TotalComments},{"Price",itemdt[i].Price},
                {"Recent30DaysSoldNum",itemdt[i].Recent30DaysSoldNum} ,{ "Position", itemdt[i].Position }  } } };
                var result = MongoDBHelper.Instance.Get_FreeBotItem().UpdateOne(new QueryDocument { { "ItemId", itemdt[i].ItemId } }, update);
            }

        }




        #region TVP
        MySqlTable GetShops(List<XListing> list)
        {

            MySqlTable table = new MySqlTable
            {
                cols = new string[]{
                        "Shop_id","SiteName","ShopName",
                        "Location","Credit","CreatedAt","LastUpdatedAt",
                        "taobaoId","ContactUrl","IsTmall","Siteid"
                        
                    }
            };

            table.data = new object[list.Count, table.cols.Length];
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                table.data[i, 0] = l.BotShopID;
                table.data[i, 1] = l.SiteName;
                table.data[i, 2] = l.ShopName;
                table.data[i, 3] = l.ShopLocation;
                table.data[i, 4] = l.ShopCredit;
                table.data[i, 5] = DateTime.Now;
                table.data[i, 6] = DateTime.Now;
                table.data[i, 7] = l.ShopID;
                table.data[i, 8] = l.ShopContactUrl;
                table.data[i, 9] = l.ShopIsTmall;
                table.data[i, 10] = l.SiteID;
            }
            return table;
        }

        MySqlTable GetListings(List<XListing> list)
        {
            MySqlTable table = new MySqlTable
            {
                cols = new string[]{
                        "ItemId","ShopID",
                        "ShopName","ItemName","CreatedAt","LastUpdatedAt",
                        "TotalBotPackages","taobaoID","Recent30DaysSoldNum","Price",
                        "DetailUrl","SiteName","LastBotAt","QuantityInStock",
                        "BotStatus","TotalComments","BadComments","Reviews","BotBrand",
                        "Recent30DaysOrderedNum","IsTmall","ItemGoodCommentPercent",
                        "pic_url","taskId","taskName","SiteId"
                    }
            };
            table.data = new object[list.Count, table.cols.Length];
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                table.data[i, 0] = l.BotItemID;
                table.data[i, 1] = l.BotShopID;
                table.data[i, 2] = l.ShopName;
                table.data[i, 3] = l.ItemName;
                table.data[i, 4] = DateTime.Now;
                table.data[i, 5] = DateTime.Now;
                table.data[i, 6] = null;
                table.data[i, 7] = l.ItemID;
                table.data[i, 8] = l.ItemSold30Days;
                table.data[i, 9] = l.ItemPrice;
                table.data[i, 10] = l.ItemDetailUrl;
                table.data[i, 11] = l.SiteName;
                table.data[i, 12] = DateTime.Now;
                table.data[i, 13] = 0;
                table.data[i, 14] = l.ItemBotStatus;
                table.data[i, 15] = l.ItemTotalCommentCount;
                table.data[i, 16] = 0;
                table.data[i, 17] = l.ItemReviews;
                table.data[i, 18] = "";
                table.data[i, 19] = l.Item30DaysOrderedNum;
                table.data[i, 20] = l.ShopIsTmall;
                table.data[i, 21] = l.ItemGoodCommentPercent;
                table.data[i, 22] = l.Itempic;
                table.data[i, 23] = l.taskid;
                table.data[i, 24] = l.taskName;
                table.data[i, 25] = l.SiteID;

            }

            return table;
        }

        List<FreeBotShop> GetShopList(List<XListing> list)
        {
            List<FreeBotShop> Itemlist = new List<FreeBotShop>();
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                FreeBotShop fb = new FreeBotShop();
                fb.Shop_id = l.BotShopID;
                fb.SiteName = l.SiteName;
                fb.ShopName = l.ShopName;
                fb.Location = l.ShopLocation;
                fb.Credit = l.ShopCredit;
                fb.CreatedAt = DateTime.Now;
                fb.LastUpdatedAt = DateTime.Now;
                fb.taobaoId = l.ShopID;
                fb.ContactUrl = l.ShopContactUrl;
                fb.IsTmall = l.ShopIsTmall;
                fb.Siteid = l.SiteID;
                fb.usrId = l.usrid;
                fb.taskId = l.taskid;
                fb.taskName = l.taskName;
                fb.ArbitraryPriceCount = 0;
                fb.ChuanhuoCount = 0;
                fb.FakeCount = 0;
                fb.IsAuthorized = false;
                fb.UId = l.UId;
                fb.ProjectId = l.ProjectId;
                Itemlist.Add(fb);
            }
            return Itemlist;
        }

        List<FreeBotItem> GetItemList(List<XListing> list)
        {
            List<FreeBotItem> Itemlist = new List<FreeBotItem>();
            for (int i = 0; i < list.Count; i++)
            {
                var l = list[i];
                FreeBotItem fb = new FreeBotItem();
                fb.ItemId = l.BotItemID;
                fb.ShopID = l.BotShopID;
                fb.ShopName = l.ShopName;
                fb.Location = l.ShopLocation;
                fb.ItemName = l.ItemName;
                fb.CreatedAt = DateTime.Now;
                fb.LastUpdatedAt = DateTime.Now;
                fb.TotalBotPackages = null;
                fb.taobaoID = l.ItemID;
                fb.Recent30DaysSoldNum = l.ItemSold30Days;
                fb.Price = l.ItemPrice;
                fb.DetailUrl = l.ItemDetailUrl;
                fb.SiteName = l.SiteName;
                fb.LastBotAt = DateTime.Now;
                fb.QuantityInStock = 0;
                fb.BotStatus = 0;
                fb.TotalComments = l.ItemTotalCommentCount;
                fb.BadComments = 0;
                fb.Reviews = l.ItemReviews;
                fb.BotBrand = "";
                fb.Recent30DaysOrderedNum = l.Item30DaysOrderedNum;
                fb.IsTmall = l.ShopIsTmall;
                fb.ItemGoodCommentPercent = l.ItemGoodCommentPercent;
                fb.pic_url = l.Itempic;
                fb.taskId = l.taskid;
                fb.taskName = l.taskName;
                fb.SiteId = l.SiteID;
                fb.usrId = l.usrid;
                fb.DataCleaningState = 0;
                fb.UId = l.UId;
                fb.Position = l.Position;
                fb.PageNum = l.PageNum;
                fb.ProjectId = l.ProjectId;
                Itemlist.Add(fb);
            }
            return Itemlist;
        }


        #endregion

        static HashSet<char> chinese_commas = new HashSet<char> { '？', '。', '，', '！', '》', '《', '‘', '“', '；' };
        public static string RemoveInivalidChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (var c in input)
            {
                if (chinese_commas.Contains(c))
                    continue;
                if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c > 255)
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string RemoveChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            input = input.Replace(':', ' ').Replace('"', ' ').Replace(",", "");
            return input.Trim();
        }
        public static string RemoveDis(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;
            input = input.Replace(':', ' ').Replace('"', ' ');
            return input.Trim();
        }
        string get_html(string url)
        {
            //var html = proxy.GetFastHtmlWithProxyIpAndARE(url, "utf-8").RemoveSpace();
            //if (!html.IsContains("c-container"))
            var html = web.GetHtml(url, null);
            return html;
        }


        void log(string msg)
        {
            Console.WriteLine(DateTime.Now.ToDateKey2() + "[" + nick_name + "/" + pages + "]:" + msg);
        }



    }
}
