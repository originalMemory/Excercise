using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Data;
using JiebaNet.Analyser;
using JiebaNet.Segmenter;
using JiebaNet.Segmenter.PosSeg;
using System.Text.RegularExpressions;
using CSharpTest.Model;
using CSharpTest.Helper;
using AISSystem;
using HtmlAgilityPack;

using Newtonsoft.Json.Linq;

namespace CSharpTest.Tools
{
    public static class DnlTools
    {
        public static void MigrateKeyLink()
        {
            string fileName = "错误项目.txt";

            var errorPros = new List<IW2S_Project>();
            #region 迁移数据
            var filterPro = Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false);
            var pros = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).ToList();
            int i = 0;
            int count = pros.Count;
            foreach (var pro in pros)
            {
                var proObjId = pro._id;
                var userObjId = pro.UsrId;

                Log("当前迁移项目[" + i + "/" + count + "] - " + pro.Name);

                //迁移关键词
                var oldFilterKey = Builders<IW2S_BaiduCommend>.Filter.Eq(x => x.ProjectId, proObjId);
                var oldKeys = MongoDBHelper.Instance.GetIW2S_BaiduCommends().Find(oldFilterKey).ToList();
                int level = oldKeys.Count / 2;
                //检测关键词是否已迁移
                var keywords = oldKeys.Select(x => x.CommendKeyword).ToList();
                var filterNewKey = Builders<Dnl_Keyword>.Filter.In(x => x.Keyword, keywords);
                var queryKey = MongoDBHelper.Instance.GetDnl_Keyword().Find(filterNewKey).ToList();
                if (queryKey.Count > level)
                {
                    i++;
                    continue;
                }

                var newKeys = new List<Dnl_Keyword>();
                var oldKeyToNew = new Dictionary<ObjectId, ObjectId>();     //原有关键词Id和新关键词Id对照词典
                foreach (var old in oldKeys)
                {
                    var newKey = new Dnl_Keyword
                    {
                        _id = ObjectId.GenerateNewId(),
                        Keyword = old.CommendKeyword,
                        BotIntervalHours = 7 * 24,
                        BotStatus_Baidu = 2,
                        CreatedAt = old.CreatedAt,
                        LastBotEndAt_Baidu = DateTime.Now.AddHours(8),
                        ValLinkCount_Baidu = old.ValLinkCount
                    };
                    oldKeyToNew.Add(old._id, newKey._id);
                    newKeys.Add(newKey);
                }
                if (newKeys.Count == 0)
                {
                    i++;
                    continue;
                }
                MongoDBHelper.Instance.GetDnl_Keyword().InsertMany(newKeys);
                Log("关键词迁移结束，共迁移 " + newKeys.Count + " 个");
                //迁移链接
                var keyIds = oldKeys.Select(x => x._id.ToString()).ToList();
                var filterLink = Builders<IW2S_level1link>.Filter.In(x => x.SearchkeywordId, keyIds);
                int page = 0;
                int pagesize = 100;
                int linkCount = 0;
                try
                {
                    while (true)
                    {
                        var oldLinks = MongoDBHelper.Instance.GetIW2S_level1links().Find(filterLink).Skip((page) * pagesize).Limit(pagesize).ToList();
                        var newLinks = new List<Dnl_Link_Baidu>();
                        if (oldLinks.Count == 0 || oldLinks == null)
                            break;
                        foreach (var old in oldLinks)
                        {
                            var newLink = new Dnl_Link_Baidu
                            {
                                _id = ObjectId.GenerateNewId(),
                                Title = old.Title,
                                Description = old.Description,
                                Html = old.Html,
                                Domain = old.Domain,
                                TopDomain = old.TopDomain,
                                BaiduVStar = old.BaiduVStar,
                                CreatedAt = old.CreatedAt,
                                Keywords = old.Keywords,
                                LinkUrl = old.LinkUrl,
                                PublishTime = old.PublishTime,
                                SearchkeywordId = oldKeyToNew[new ObjectId(old.SearchkeywordId)].ToString(),
                                IsPromotion = old.IsMarket,
                                MatchAt = Convert.ToInt32(old.MatchAt)
                            };
                            newLinks.Add(newLink);
                        }
                        linkCount += newLinks.Count;
                        MongoDBHelper.Instance.GetDnl_Link_Baidu().InsertMany(newLinks);
                        page++;
                    }
                    Log("链接迁移结束，共迁移 " + linkCount + " 个");
                }
                catch (Exception)
                {
                    errorPros.Add(pro);
                    Log("链接迁移出错");
                }

                //迁移关键词词组
                var builderCate = Builders<IW2S_KeywordCategory>.Filter;
                var filterCate = builderCate.Eq(x => x.ProjectId, proObjId) & builderCate.Eq(x => x.IsDel, false);
                var oldCates = MongoDBHelper.Instance.GetIW2S_KeywordCategorys().Find(filterCate).ToList();
                var newCates = new List<Dnl_KeywordCategory>();
                var oldCateToNew = new Dictionary<ObjectId, ObjectId>();
                foreach (var old in oldCates)
                {
                    var newCate = new Dnl_KeywordCategory
                    {
                        _id = ObjectId.GenerateNewId(),
                        IsDel = false,
                        GroupNumber = old.GroupNumber,
                        KeywordCount = old.KeywordTotal,
                        InfriLawCode = old.InfriLawCode,
                        Name = old.Name,
                        ParentId = old.ParentId,
                        ProjectId = old.ProjectId,
                        UserId = old.UsrId,
                        Weight = old.Weight
                    };
                    newCates.Add(newCate);
                    oldCateToNew.Add(old._id, newCate._id);
                }
                if (newCates.Count > 0)
                    MongoDBHelper.Instance.GetDnl_KeywordCategory().InsertMany(newCates);
                if (!oldCateToNew.ContainsKey(ObjectId.Empty))
                {
                    oldCateToNew.Add(ObjectId.Empty, ObjectId.Empty);
                }
                Log("关键词组迁移结束，共迁移 " + newCates.Count + " 个");

                //迁移词组内关键词
                var oldGroupObjIds = oldCates.Select(x => x._id).ToList();
                var filterGroup = Builders<IW2S_KeywordGroup>.Filter.In(x => x.CommendCategoryId, oldGroupObjIds) & Builders<IW2S_KeywordGroup>.Filter.Eq(x => x.IsDel, false);
                var oldGroups = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterGroup).ToList();
                var newMaps = new List<Dnl_KeywordMapping>();
                foreach (var old in oldGroups)
                {
                    var newMap = new Dnl_KeywordMapping
                    {
                        _id = ObjectId.GenerateNewId(),
                        IsDel = false,
                        CategoryId = oldCateToNew[old.CommendCategoryId],
                        ParentCategoryId = oldCateToNew[old.ParentCategoryId],
                        Keyword = old.BaiduCommend,
                        KeywordId = oldKeyToNew[old.BaiduCommendId],
                        ProjectId = old.ProjectId,
                        UserId = old.UsrId
                    };
                    newMaps.Add(newMap);
                }
                if (newMaps.Count > 0)
                    MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newMaps);
                Log("关键词映射迁移结束，共迁移 " + newMaps.Count + " 个");
                //迁移所有词映射
                var newKeyObjIds = newKeys.Select(x => x._id).ToList();
                var filter = Builders<Dnl_Keyword>.Filter.In(x => x._id, newKeyObjIds);
                var keys = MongoDBHelper.Instance.GetDnl_Keyword().Find(filter).ToList();
                var newRootKeys = new List<Dnl_KeywordMapping>();
                foreach (var x in keys)
                {
                    var y = new Dnl_KeywordMapping
                    {
                        _id = ObjectId.GenerateNewId(),
                        Keyword = x.Keyword,
                        KeywordId = x._id,
                        ProjectId = proObjId,
                        UserId = userObjId
                    };
                    newRootKeys.Add(y);
                }
                if (newRootKeys.Count > 0)
                    MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newRootKeys);
                Log("所有词映射迁移结束，共迁移 " + newRootKeys.Count + " 个");
                Log(pro.Name + " 迁移完毕" + System.Environment.NewLine);
                i++;
            }

            #endregion

            if (!File.Exists(fileName))
            {
                FileStream fs1 = new FileStream("F:\\TestTxt.txt", FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs1);
                foreach (var pro in errorPros)
                {
                    sw.WriteLine(pro._id.ToString() + "：" + pro.Name);//开始写入值
                }
                sw.Close();
                fs1.Close();
            }
            else
            {
                FileStream fs = new FileStream("F:\\TestTxt.txt", FileMode.Open, FileAccess.Write);
                StreamWriter sr = new StreamWriter(fs);
                foreach (var pro in errorPros)
                {
                    sr.WriteLine(pro._id.ToString() + "：" + pro.Name);//开始写入值
                }
                sr.Close();
                fs.Close();
            }
            Log("全部迁移完毕！");
        }

        public static void RecoverMapping()
        {
            var errorPros = new List<IW2S_Project>();
            #region 迁移数据
            var filterPro = Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false);
            var pros = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).ToList();
            int i = 0;
            int count = pros.Count;

            //获取现在所有关键词
            var filterKey = Builders<Dnl_Keyword>.Filter.Empty;
            var keywords = MongoDBHelper.Instance.GetDnl_Keyword().Find(filterKey).ToList();
            foreach (var pro in pros)
            {

                var proObjId = pro._id;
                var userObjId = pro.UsrId;

                Log("当前项目[" + i + "/" + count + "] - " + pro.Name);
                if (i < 135)
                {
                    i++;
                    continue;
                }

                //获取原项目内所有关键词
                var builderKey = Builders<IW2S_BaiduCommend>.Filter;
                var filterBd = builderKey.Eq(x => x.ProjectId, proObjId) & builderKey.Eq(x => x.IsRemoved, false);
                var oldKeys = MongoDBHelper.Instance.GetIW2S_BaiduCommends().Find(filterBd).ToList();

                //获取原关键词词组
                var builderCate = Builders<IW2S_KeywordCategory>.Filter;
                var filterCate = builderCate.Eq(x => x.ProjectId, proObjId) & builderCate.Eq(x => x.IsDel, false);
                var oldCates = MongoDBHelper.Instance.GetIW2S_KeywordCategorys().Find(filterCate).ToList();
                if (oldKeys.Count == 0)
                    continue;
                var oldCateToNew = new Dictionary<ObjectId, ObjectId>();

                
                //获取新关键词词组
                var newBuiderCate = Builders<Dnl_KeywordCategory>.Filter;
                var newFilterCate = newBuiderCate.Eq(x => x.ProjectId, proObjId) & newBuiderCate.Eq(x => x.IsDel, false);
                var newCates = MongoDBHelper.Instance.GetDnl_KeywordCategory().Find(newFilterCate).ToList();
                var newCateInsert = new List<Dnl_KeywordCategory>();
                foreach (var x in oldCates)
                {
                    var temp = newCates.Where(s => s.ProjectId.Equals(x.ProjectId)).ToList().Find(s => s.Name == x.Name);
                    if (temp != null && !oldCateToNew.ContainsKey(x._id))
                        oldCateToNew.Add(x._id, temp._id);
                    if (temp == null && !oldCateToNew.ContainsKey(x._id))
                    {
                        var temp2 = new Dnl_KeywordCategory
                        {
                            _id = ObjectId.GenerateNewId(),
                            Name = x.Name,
                            InfriLawCode = x.InfriLawCode,
                            KeywordCount = x.KeywordTotal,
                            Weight = x.Weight,
                            ProjectId = proObjId,
                            UserId = userObjId
                        };
                        newCateInsert.Add(temp2);
                        oldCateToNew.Add(x._id, temp2._id);
                    }
                }
                for(int j=0;j<newCateInsert.Count;j++)
                {
                    var old = oldCates.Find(s => s.Name == newCateInsert[j].Name);
                    if (old != null && oldCateToNew.ContainsKey(old.ParentId))
                        newCateInsert[j].ParentId = oldCateToNew[old.ParentId];
                }
                if (newCateInsert.Count > 0)
                    MongoDBHelper.Instance.GetDnl_KeywordCategory().InsertMany(newCateInsert);
                oldCateToNew.Add(ObjectId.Empty,ObjectId.Empty);
                

                //迁移词组内关键词映射
                var oldGroupObjIds = oldCates.Select(x => x._id).ToList();
                var filterGroup = Builders<IW2S_KeywordGroup>.Filter.In(x => x.CommendCategoryId, oldGroupObjIds) & Builders<IW2S_KeywordGroup>.Filter.Eq(x => x.IsDel, false);
                var oldGroups = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterGroup).ToList();
                var newMaps = new List<Dnl_KeywordMapping>();
                foreach (var x in oldGroups)
                {
                    var map = new Dnl_KeywordMapping
                    {
                        CategoryId = oldCateToNew[x.CommendCategoryId],
                        ParentCategoryId = oldCateToNew[x.ParentCategoryId],
                        ProjectId = proObjId,
                        UserId = userObjId,
                        Keyword = x.BaiduCommend
                    };
                    var temp = keywords.Find(s => s.Keyword == map.Keyword);
                    if (temp == null)
                        continue;
                    else
                        map.KeywordId = temp._id;
                    newMaps.Add(map);
                }
                if(newMaps.Count>0)
                    MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newMaps);


                Log("关键词映射迁移结束，共迁移 " + newMaps.Count + " 个");

               

                //迁移所有词映射
                var newRootKeys = new List<Dnl_KeywordMapping>();
                foreach (var x in oldKeys)
                {
                    var y = new Dnl_KeywordMapping
                    {
                        _id = ObjectId.GenerateNewId(),
                        Keyword = x.Keyword,
                        ProjectId = proObjId,
                        UserId = userObjId
                    };
                    var temp = keywords.Find(s => s.Keyword == y.Keyword);
                    if (temp == null)
                        continue;
                    else
                        y.KeywordId = temp._id;
                    newRootKeys.Add(y);
                }
                if (newRootKeys.Count > 0)
                    MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newRootKeys);
                Log("所有词映射迁移结束，共迁移 " + newRootKeys.Count + " 个");
                Log(pro.Name + " 迁移完毕" + System.Environment.NewLine);
                i++;
            }

            #endregion

            Log("全部迁移完毕！");
        }

        /// <summary>
        /// 计算链接数
        /// </summary>
        public static void CountLinkNum()
        {
            var colKey=MongoDBHelper.Instance.GetDnl_Keyword();
            var keywords = colKey.Find(Builders<Dnl_Keyword>.Filter.Empty).ToList();
            int i = 1;
            foreach (var key in keywords)
            {
                Log("关键词["+i+"/+"+keywords.Count+"]：" + key.Keyword);
                var filterLink = Builders<Dnl_Link_Baidu>.Filter.Eq(x => x.SearchkeywordId, key._id.ToString());
                var queryLink = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => x._id).ToList();
                int num = queryLink.Count;
                var update = new UpdateDocument { { "$set", new QueryDocument { { "LinkCount_Baidu", num } } } };
                var filterKey = Builders<Dnl_Keyword>.Filter.Eq(x => x._id, key._id);
                colKey.UpdateOne(filterKey, update);
                Log("链接数：" + num + "\n");
                i++;
            }
        }

        public static void SearchWeiXiArticle(Dnl_Keyword_Media task)
        {
            Log("关键词 - " + task.Keyword);
            string url = "http://open.gsdata.cn/api/wx/opensearchapi/content_keyword_search";
            //string url = "http://open.gsdata.cn/api/sys/sysapi/check_user";
            Dictionary<string, object> postData = new Dictionary<string, object>();

            string appid = "JVEvKn7ghegw984neooX";
            string appkey = "n0TWOaX9gta1dpfVF07hpkKr2";

            postData.Add("keyword", task.Keyword);
            postData.Add("start", 0);
            postData.Add("num", 10);
            //postData.Add("startdate", "2005-01-01");
            //postData.Add("enddate", "2017-06-01");
            postData.Add("sortname", "likenum");
            postData.Add("sort", "asc");

            GsdataApi api = new GsdataApi(appid, appkey);
            string str = api.Call(postData, url);
            Console.WriteLine(str);
            JObject json = JObject.Parse(str);
            if (json.Property("returnCode") != null & json["returnCode"].ToString() == "1001")
            {
                Log("获取成功！");
                JObject returnData = (JObject)json["returnData"];
                task.WXArticleNumTotal = Convert.ToInt32(returnData["total"]);
                task.WXArticleNumNow += Convert.ToInt32(returnData["num"]);
                JArray items = (JArray)returnData["items"];
                var articles = new List<Dnl_Link_WeiXi>();
                for (int i = 0; i < items.Count; i++)
                {
                    JObject item = (JObject)items[i];
                    var article = new Dnl_Link_WeiXi()
                    {
                        Keyword = task.Keyword,
                        KeywordId = task._id.ToString(),
                        Id = Convert.ToInt32(item["id"]),
                        Table_name = item["table_name"].ToString(),
                        Talbe_id = Convert.ToInt32(item["talbe_id"]),
                        Name = item["name"].ToString(),
                        Wx_name = item["wx_name"].ToString(),
                        Nickname_id = Convert.ToInt32(item["nickname_id"]),
                        Posttime = item["posttime"].ToString(),
                        Title = item["title"].ToString(),
                        Description = item["content"].ToString(),
                        Url = item["url"].ToString(),
                        Status = item["status"].ToString(),
                        Add_time = item["add_time"].ToString(),
                        Get_time = item["get_time"].ToString(),
                        Readnum = Convert.ToInt32(item["readnum"]),
                        Likenum = Convert.ToInt32(item["likenum"]),
                        Get_time_pm = item["get_time_pm"].ToString(),
                        Readnum_pm = Convert.ToInt32(item["readnum_pm"]),
                        Likenum_pm = Convert.ToInt32(item["likenum_pm"]),
                        Get_time_week = item["get_time_week"].ToString(),
                        Readnum_week = Convert.ToInt32(item["readnum_week"]),
                        Likenum_week = Convert.ToInt32(item["likenum_week"]),
                        Top = Convert.ToInt32(item["top"]),
                        Ispush = Convert.ToInt32(item["ispush"]),
                        Picurl = item["picurl"].ToString(),
                        Sourceurl = item["sourceurl"].ToString(),
                        Author = item["author"].ToString(),
                        Copyright = item["copyright"].ToString(),
                        Index_name = item["index_name"].ToString(),
                    };
                    articles.Add(article);
                }
                MongoDBHelper.Instance.GetDnl_Keyword_Media().InsertOne(task);
                MongoDBHelper.Instance.GetDnl_Link_WeiXi().InsertMany(articles);
                Log("保存成功！");
            }
            else
            {
                Log("请求出错！");
            }
        }

        public static void Temp()
        {
            //var filterPro = Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false);
            //var pros = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).ToList();
            //int count = pros.Count;

            //获取现在所有关键词
            var filterKey = Builders<Dnl_Keyword>.Filter.Empty;
            var keywords = MongoDBHelper.Instance.GetDnl_Keyword().Find(filterKey).ToList();
            for (int i = 0; i < keywords.Count; i++)
            {

                //var proObjId = pro._id;
                //var userObjId = pro.UsrId;

                Log("当前关键词[" + i + "/" + keywords.Count + "] - " + keywords[i].Keyword);
                //获取当前项目内所有关键词Id
                //var filterCate = Builders<Dnl_KeywordMapping>.Filter.Eq(x => x.ProjectId, proObjId);
                //var keyIds = MongoDBHelper.Instance.GetDnl_KeywordMapping().Find(filterCate).Project(x => x.KeywordId.ToString()).ToList().Distinct();
                var filterLink = Builders<Dnl_Link_Baidu>.Filter.Eq(x => x.SearchkeywordId, keywords[i]._id.ToString());
                var col = MongoDBHelper.Instance.GetDnl_Link_Baidu();
                var links = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => new
                {
                    Id = x._id,
                    Domain = x.Domain
                }).ToList();
                int j = 1;
                foreach (var x in links)
                {
                    string num = GetDomainCollectionNum(x.Domain);
                    if (num.Contains("亿"))
                    {
                        if (num.Contains("万"))
                        {
                            int p1 = num.IndexOf("亿");
                            int p2 = num.IndexOf("万");
                            long a = Convert.ToInt32(num.SubBefore("亿"));
                            long b = Convert.ToInt32(num.SubAfter("亿").SubBefore("万"));
                            num = (a * 100000000 + b * 10000).ToString();
                        }
                        else
                        {
                            int p1 = num.IndexOf("亿");
                            int p2 = num.IndexOf("万");
                            long a = Convert.ToInt32(num.SubBefore("亿"));
                            long b = Convert.ToInt32(num.SubAfter("亿"));
                            num = (a * 100000000 + b).ToString();
                        }
                    }
                    else if (num.Contains("万"))
                    {
                        int p2 = num.IndexOf("万");
                        long a = Convert.ToInt32(num.SubBefore("万"));
                        long b = Convert.ToInt32(num.SubAfter("万"));
                        num = (a * 10000 + b).ToString();
                    }
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "DomainCollectionNum", num } } } };
                    var filterUp = Builders<Dnl_Link_Baidu>.Filter.Eq(s => s._id, x.Id);
                    col.UpdateOne(filterUp, update);
                    Log("当前关键词[" + i + "/" + keywords.Count + "] - " + keywords[i].Keyword + "   -   [" + j + "/" + links.Count + "]");
                    j++;
                }
                Console.WriteLine("\n");
                i++;
            }

            Log("全部迁移完毕！");
        }

        /// <summary>
        /// 输出时间日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void Log(string message)
        {
            Console.WriteLine(DateTime.Now.ToString() + "：" + message);
        }

        /// <summary>
        /// 获取域名收录量
        /// </summary>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        private static string GetDomainCollectionNum(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                return "0";
            }
            string url = "http://www.baidu.com/s?ie=utf-8&wd=site:{0}";
            url = url.FormatStr(domain.GetUrlEncodedString("utf-8"));
            string html = WebApiInvoke.GetHtml(url);        //获取网页源码
            if (string.IsNullOrEmpty(html))
            {
                return "0";
            }
            //解析并获取域名收录量
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode collection = doc.DocumentNode.SelectSingleNode("//*[@id=\"1\"]/div/div[1]/div/p[3]/span/b");
            if (collection != null)
            {
                string numStr = collection.InnerText;
                if (!string.IsNullOrEmpty(numStr))
                {
                    numStr = numStr.Trim().Replace(",", "");
                }
                return numStr;
            }
            else
            {
                return "0";
            }
        }
    }

    
}
