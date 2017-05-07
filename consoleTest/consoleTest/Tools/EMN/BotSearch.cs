using System.Reflection.Emit;
using IprseeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using AISSystem;
using cPlusPlusTest.Models;
using System.Data;
using IprseeData.EF_Sql;
using cPlusPlusTest.Template;
using MongoDB.Driver;
using cPlusPlusTest.Helper;
using MongoDB.Driver.Builders;
using MongoDB.Bson;


namespace cPlusPlusTest.Search
{
    public class BotSearch
    {
        public static readonly BotSearch Instance = new BotSearch();
        string com = AISSystem.AppSettingHelper.GetAppSetting("commonsMySqlCon");

        public delegate void UpdateBotStatus();

        public event UpdateBotStatus SetBusy;
        public event UpdateBotStatus SetReady;

        public void Run()
        {
            while (true)
            {
                BotTaskService bt = new BotTaskService();
                Random r = new Random();
                Thread.Sleep(2000);
                FreeTask keyTask = bt.GetBotTask(BotTypes.ItemSnapshot);  //get_task();
                if (keyTask == null || keyTask.TaskName == "" || keyTask.TaskName == null)
                {
                    SetReady();
                    log("No search task !");
                    Thread.Sleep(1000);
                    var sssH = DateTime.Now.Hour;
                    if (sssH == 5)
                    {
                        var miuH = DateTime.Now.Minute;
                        if (miuH < 3)
                        {
                            var queryTask = new QueryDocument { { "BotIntervalHours", 24 }, { "IsStarted", true } };
                            var colH = MongoDBHelper.Instance.Get_FreeTask();
                            var resultH = colH.Find(queryTask).SortByDescending(x => x.CreatedAt).ToList();
                            foreach (var item in resultH)
                            {
                                var update1 = new UpdateDocument { { "$set", new QueryDocument { { "IsBot", false } } } };
                                var Hresult = MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", item._id } }, update1);
                            }
                        }
                    }
                    var Zhou = DateTime.Now.DayOfWeek.ToString();
                    if (Zhou == "Sunday")
                    {
                        var Zsss = DateTime.Now.Hour;
                        if (Zsss == 5)
                        {
                            var miuZ = DateTime.Now.Minute;
                            if (miuZ < 3)
                            {
                                var queryTask = new QueryDocument { { "BotIntervalHours", 168 }, { "IsStarted", true } };
                                var colZ = MongoDBHelper.Instance.Get_FreeTask();
                                var resultZ = colZ.Find(queryTask).SortByDescending(x => x.CreatedAt).ToList();
                                foreach (var item in resultZ)
                                {
                                    var update2 = new UpdateDocument { { "$set", new QueryDocument { { "IsBot", false } } } };
                                    var Zresult = MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", item._id } }, update2);
                                }

                            }
                        }
                    }
                    var Tian = DateTime.Now.Day;
                    if (Tian ==30)
                    {
                        var Tsss = DateTime.Now.Hour;
                        if (Tsss == 5)
                        {
                            var miuT = DateTime.Now.Minute;
                            if (miuT < 3)
                            {
                                var queryTask = new QueryDocument { { "BotIntervalHours", 720 }, { "IsStarted", true } };
                                var colT = MongoDBHelper.Instance.Get_FreeTask();
                                var resultT = colT.Find(queryTask).SortByDescending(x => x.CreatedAt).ToList();
                                foreach (var item in resultT)
                                {
                                    var update3 = new UpdateDocument { { "$set", new QueryDocument { { "IsBot", false } } } };
                                    var Tresult = MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", item._id } }, update3);
                                }
                            }
                        }
                    }
                    DateTime start = Convert.ToDateTime(DateTime.Now.AddDays(0).AddHours(8).ToShortDateString());
                    DateTime end = Convert.ToDateTime(DateTime.Now.AddDays(1).AddHours(8));
                    continue;
                }

                SetBusy();
                //string sql = @"update taskparameters set LastBotStartAt ='{0}',IsBot=1 where Id={1}".FormatStr(DateTime.Now, keyTask.Id);
                //MySqlDbHelper.ExecuteSql(com, sql);
                int? LinksNum = 0;
                LinksNum = keyTask.recordNum + 1;
                var update = new UpdateDocument { { "$set", new QueryDocument { { "IsBot", true }, { "LastBotStartAt", DateTime.Now.AddHours(8) }, 
                { "recordNum", LinksNum }  } } };
                //   var result = MongoDBHelper<FreeTask>.Update("FreeTask", new QueryDocument { { "_id", keyTask._id } }, update);
                var result = MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", keyTask._id } }, update);
                List<FreeTaskRecord> list = new List<FreeTaskRecord>();
                FreeTaskRecord ftr = new FreeTaskRecord();
                ftr._id = new ObjectId();
                ftr.IsStarted = keyTask.IsStarted;
                ftr.IsBot = true;
                ftr.Taskid = keyTask._id;
                ftr.TaskName = keyTask.TaskName;
                ftr.UsrId = keyTask.UsrId;
                ftr.ServiceState = "Normal";
                ftr.CreatedAt = DateTime.Now.AddHours(8);
                ftr.LastBotStartAt = DateTime.Now.AddHours(8);
                // ftr.LastBotEndAt = DateTime.Now.AddHours(8);
                ftr.Dataquantity = 0;
                ftr.LinksNum = 0;
                ftr.ShopsNum = 0;
                ftr.SiteId = Guid.Parse("A00A672B-DD05-65FB-4EE0-CFA26EBF2ED5");
                ftr.SiteName = "taobao";
                ftr.LanIP = "";//TaobaoWebHelper.GetLanIP();
                ftr.UId = keyTask.UId;
                ftr.ProjectId = keyTask.ProjectId;
                //  var savedq = MongoDBHelper<FreeTaskRecord>.InsertData(ftr, "FreeTaskRecord");
                list.Add(ftr);
                var col = MongoDBHelper.Instance.Get_FreeTaskRecord();
                col.InsertMany(list);
                string recordId = ftr._id.ToString();
                Snapshot(keyTask, com, recordId);
                try
                {
                    // update = new UpdateDocument { { "$set", new QueryDocument { { "LastBotEndAt", DateTime.Now.AddHours(8) } } } };
                    // result = MongoDBHelper<FreeTask>.Update("FreeTask", new QueryDocument { { "_id", keyTask._id } }, update);
                    update = new UpdateDocument { { "$set", new QueryDocument { { "LastBotEndAt", DateTime.Now.AddHours(8) } } } };
                    result = MongoDBHelper.Instance.Get_FreeTask().UpdateOne(new QueryDocument { { "_id", keyTask._id } }, update);

                    update = new UpdateDocument { { "$set", new QueryDocument { { "LastBotEndAt", DateTime.Now.AddHours(8) } } } };
                    // result = MongoDBHelper<FreeTaskRecord>.Update("FreeTaskRecord", new QueryDocument { { "_id", new ObjectId(recordId)} }, update);
                    result = MongoDBHelper.Instance.Get_FreeTaskRecord().UpdateOne(new QueryDocument { { "_id", new ObjectId(recordId) } }, update);

                }
                catch (Exception ex)
                {
                    log("get_proj_to_qry ERROR ." + ex.Message);
                    Thread.Sleep(5000);
                }



                //}


            }
        }

        void Snapshot(FreeTask task, string conn, string recordId)
        {

            int itemCount = 0;
            List<XListing> xListings = new List<XListing>();

            // TaoBaoSnapshotQuery tbq = new TaoBaoSnapshotQuery();
            TaoBaoQuery tbq = new TaoBaoQuery(task.TaskName);
            SnapSearchResult result = null;
            result = tbq.Query(task, recordId) as SnapSearchResult;
            xListings = result.Listings;
            //int pageSize = 2000;
            //xListings = xListings.OrderByDescending(x => x.ItemSold30Days).ToList();
            //for (int page = 0; page * pageSize < xListings.Count; page++)
            //{
            //    SaveResult(xListings.Skip(page * pageSize).Take(pageSize).ToList(), conn, BotTypes.ItemSnapshot);
            //}
            // SaveKeyRecord(task, xListings);
        }

        BotTask GetSubTask(string url, string postData, string queryName, BotTask currentTask)
        {
            var newTask = new BotTask
            {
                QueryName = queryName ?? currentTask.QueryName,
                BotType = currentTask.BotType,
                UserCompanyID = currentTask.UserCompanyID,
                EntityID = currentTask.EntityID,
                SiteID = currentTask.SiteID,
                SiteName = currentTask.SiteName,
                MaxItemSizePeerLink = currentTask.MaxItemSizePeerLink,
                UserCompanyBrandID = currentTask.UserCompanyBrandID,
                BotBrandID = currentTask.BotBrandID,
                BotItemID = currentTask.BotItemID,
                BotProductID = currentTask.BotProductID,
                BotShopID = currentTask.BotShopID,
                AddtionalParamters = string.IsNullOrEmpty(postData) ? url : url + PostDataPrefix + postData,
                ID = currentTask.ID,
                FriendlyName = currentTask.FriendlyName,
                IsSnapshotBot = currentTask.IsSnapshotBot,
                UserCompanyProductID = currentTask.UserCompanyProductID,
                IsTempSubTask = true,
            };
            return newTask;
        }

        public const string PostDataPrefix = "\r\nPost Data:";
        Dictionary<string, IQuery> queryList = new Dictionary<string, IQuery>();
        StackQueue<string> msges = new StackQueue<string>(30);
        public StackQueue<string> Messages { get { return msges; } }
        public Dispatcher Dispatcher { get; set; }
        public Dictionary<BotTypes, Dispatcher> dispatches = new Dictionary<BotTypes, Dispatcher>();
        public Dictionary<BotTypes, StackQueue<string>> MessageQueues = new Dictionary<BotTypes, StackQueue<string>>();
        public event EventHandler<string> MessageHandler;
        IQuery GetIQuery(string typeName)
        {
            if (!typeName.StartsWith("QueryLib"))
                typeName = "QueryLib." + typeName;
            if (queryList.ContainsKey(typeName))
                return queryList[typeName];
            var t = this.GetType().Assembly.GetType(typeName);
            if (t == null)
                t = this.GetType().Assembly.GetTypes().FirstOrDefault(x => x.FullName == typeName);
            IQuery query = Activator.CreateInstance(t) as IQuery;
            queryList.Add(typeName, query);
            return query;
        }
        void OnMsg(string msg, BotTypes botType)
        {
            if (MessageHandler != null)
                MessageHandler(this, msg);
            var dis = dispatches[botType];
            if (dis == null)
                return;
            var msgqu = MessageQueues[botType];
            if (msgqu == null)
                return;
            dis.Invoke(() => { msgqu.Enqueue(string.Format("{0}: {1}", DateTime.Now, msg)); });
        }

        Dictionary<Guid, string> conns = new Dictionary<Guid, string>();

        #region Save Result

        //void SaveResult(List<XListing> listings, string con, BotTypes botType)
        //{
        //    listings.ToList().ForEach(x =>
        //    {
        //        if (string.IsNullOrEmpty(x.ShopName) && !string.IsNullOrEmpty(x.SiteName) && !x.BotShopID.HasValue
        //            && !x.SiteName.ToLower().Contains("taobao") && !x.SiteName.ToLower().Contains("alibaba"))
        //        {
        //            x.ShopName = x.SiteName;
        //            x.BotShopID = x.SiteID;
        //        }
        //        if (!x.BotShopID.HasValue && !string.IsNullOrEmpty(x.ShopName))
        //            x.BotShopID = IDHelper.GetGuid(string.Format("{0},{1}", x.ShopID ?? x.ShopName, x.SiteName));
        //    });
        //    var shopList = listings;
        //    shopList = shopList.DistinctBy(x => x.BotShopID);
        //    //var exists_ids = MySqlDbHelper.GetExsitsIds<Guid?>(com, "bot_shops", "Shop_id", shopList.Select(x => x.BotShopID).ToArray());
        //    //if (exists_ids != null && exists_ids.Count > 0)
        //    //{
        //    //    shopList = shopList.Where(x => !exists_ids.Contains(x.BotShopID)).ToList();
        //    //}

        //    FieldsDocument shopfd = new FieldsDocument();
        //    shopfd.Add("BotShopID", 1);
        //    MongoCollection<Guid> shopcol = MongoDBHelper<Guid>.GetMongoDB().GetCollection<Guid>("FreeBotShop");
        //    List<Guid?> BotShopID = shopList.Select(x => x.BotShopID).ToList();
        //    var existsshop_objs = shopcol.Find(Query.In("BotShopID", new BsonArray(BotShopID))).SetFields(shopfd);
        //    List<Guid?> exists_ids = new List<Guid?>();
        //    foreach (var result in existsshop_objs)
        //    {
        //        exists_ids.Add(result);
        //    }
        //    if (exists_ids != null && exists_ids.Count > 0)
        //    {
        //        shopList = shopList.Where(x => !exists_ids.Contains(x.BotShopID)).ToList();
        //    }
        //    if (shopList == null || shopList.Count == 0)
        //        return;

        //    if (shopList.Count > 0)
        //    {
        //        var dt = GetShopList(shopList);
        //        // DBHelper.RunTVP(con, "usp_InsertMDMShop", "@TVP", dt);
        //        //  var saved = MySqlDbHelper.BatchInsert(con, "bot_shops", dt);
        //        var saved = MongoDBHelper<FreeBotShop>.BatchInsertData(dt, "FreeBotShop");
        //        log("to save " + saved + " bot_shops");
        //        log("Done");

        //    }
        //    listings.ToList().ForEach(x =>
        //    {
        //        if (!x.BotItemID.HasValue && !string.IsNullOrEmpty(x.ItemName))
        //            x.BotItemID = IDHelper.GetGuid(string.Format("{0},{1}", x.ItemID ?? x.ItemName, x.SiteName));
        //    });
        //    var itemList = listings;
        //    itemList = itemList.DistinctBy(x => x.BotItemID);
        //    //var exists_itemids = MySqlDbHelper.GetExsitsIds<Guid?>(com, "bot_items", "ItemId", itemList.Select(x => x.BotItemID).ToArray());
        //    //if (exists_itemids != null && exists_itemids.Count > 0)
        //    //{
        //    //    itemList = itemList.Where(x => !exists_itemids.Contains(x.BotItemID)).ToList();
        //    //}

        //    FieldsDocument fd = new FieldsDocument();
        //    fd.Add("BotItemID", 1);
        //    MongoCollection<Guid> col = MongoDBHelper<Guid>.GetMongoDB().GetCollection<Guid>("FreeBotItem");
        //    List<Guid?> BotItemID = itemList.Select(x => x.BotItemID).ToList();
        //    var exists_objs = col.Find(Query.In("BotItemID", new BsonArray(BotItemID))).SetFields(fd);
        //    List<Guid?> existsitem_ids = new List<Guid?>();
        //    foreach (var result in exists_objs)
        //    {
        //        existsitem_ids.Add(result);
        //    }
        //    if (existsitem_ids != null && existsitem_ids.Count > 0)
        //    {
        //        itemList = itemList.Where(x => !existsitem_ids.Contains(x.BotItemID)).ToList();
        //    }
        //    if (itemList == null || itemList.Count == 0)
        //        return;

        //    var itemdt = GetItemList(itemList);
        //    //  var savedListings = MySqlDbHelper.BatchInsert(con, "bot_items", itemdt);
        //    var savedListings = MongoDBHelper<FreeBotItem>.BatchInsertData(itemdt, "FreeBotItem");
        //    log("to save " + savedListings + " listings");
        //    log("Done");
        //}

        public string GetNameValues(Dictionary<string, string> dic, int maxLen)
        {
            StringBuilder sb = new StringBuilder();
            if (dic != null && dic.Count > 0)
                dic.ToList().ForEach(x => sb.AppendLine(string.Format("{0}:{1}", x.Key, x.Value)));
            if (sb.Length > maxLen)
                sb.Remove(maxLen - 1, sb.Length - maxLen);
            return sb.ToString();
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
                Itemlist.Add(fb);
            }
            return Itemlist;
        }


        #endregion
        #endregion



        void log(string msg)
        {
            Console.WriteLine(DateTime.Now + "  :  " + msg);
        }

    }

    public class ModelsConstants
    {
        public const string MDMSalt = "MDMSalt_1q2w3e!@#";
        public const string MDMKey = "MDMCryp_1q2w3e$%^";
    }

}
