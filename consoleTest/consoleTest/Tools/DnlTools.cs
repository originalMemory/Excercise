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
using consoleTest.Model;
using consoleTest.Helper;
using AISSystem;

namespace consoleTest.Tools
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
                                Abstract = old.Abstract,
                                Html = old.Html,
                                Domain = old.Domain,
                                TopDomain = old.TopDomain,
                                BaiduVStar = old.BaiduVStar,
                                CreatedAt = old.CreatedAt,
                                Keywords = old.Keywords,
                                LinkUrl = old.LinkUrl,
                                PublishTime = old.PublishTime,
                                Rank = old.Rank,
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
                        ValLinkCount = old.ValLinkCount,
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
                            ValLinkCount = x.ValLinkCount,
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

        public static void DelRepeat()
        {
            

            //var filterPro = Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false);
            //var pros = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).ToList();
            //int i = 0;
            //int count = pros.Count;

            ////获取现在所有关键词
            //var filterKey = Builders<Dnl_Keyword>.Filter.Empty;
            //var keywords = MongoDBHelper.Instance.GetDnl_Keyword().Find(filterKey).ToList();
            //foreach (var pro in pros)
            //{

            //    var proObjId = pro._id;
            //    var userObjId = pro.UsrId;

            //    Log("当前项目[" + i + "/" + count + "] - " + pro.Name);
            //    //if (i < 135)
            //    //{
            //    //    i++;
            //    //    continue;
            //    //}
            //    var builder = Builders<Dnl_KeywordMapping>.Filter;
            //    var filterFind = builder.Ne(x => x.CategoryId, ObjectId.Empty) & builder.Eq(x => x.ProjectId, proObjId);
            //    var col = MongoDBHelper.Instance.GetDnl_KeywordMapping();
            //    var keys = col.Find(filterFind).ToList();
            //    var filterRoot = builder.Eq(x => x.CategoryId, ObjectId.Empty) & builder.Eq(x => x.ProjectId, proObjId);
            //    var roots = col.Find(filterRoot).ToList();
            //    var maps = new List<Dnl_KeywordMapping>();
            //    foreach (var x in keys)
            //    {
            //        var tp = maps.Find(s => s.KeywordId == x.KeywordId);
            //        if (tp != null)
            //            continue;
            //        var root = roots.Find(s => s.KeywordId == x.KeywordId);
            //        if (root == null)
            //        {
            //            var map = new Dnl_KeywordMapping
            //            {
            //                Keyword = x.Keyword,
            //                KeywordId = x.KeywordId,
            //                ProjectId = x.ProjectId,
            //                UserId = x.UserId
            //            };
            //            maps.Add(map);
            //        }
            //    }
            //    if(maps.Count>0)
            //        col.InsertMany(maps);
            //    i++;
            //}
        }

        /// <summary>
        /// 输出时间日志
        /// </summary>
        /// <param name="message">日志信息</param>
        public static void Log(string message)
        {
            Console.WriteLine(DateTime.Now + "：" + message);
        }
    }
}
