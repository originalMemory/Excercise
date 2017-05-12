using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using cPlusPlusTest.Model;

using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Net;

using JiebaNet.Analyser;
using JiebaNet.Segmenter;

namespace cPlusPlusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 迁移数据
            var proObjId = new ObjectId("588712927c08250b487fcfd1");
            ////迁移关键词
            //var oldFilterKey = Builders<IW2S_BaiduCommend>.Filter.Eq(x=>x.ProjectId,proObjId);
            //var oldKeys = MongoDBHelper.Instance.GetIW2S_BaiduCommends().Find(oldFilterKey).ToList();
            //var newKeys = new List<Dnl_Keyword>();
            //var oldKeyToNew = new Dictionary<ObjectId, ObjectId>();     //原有关键词Id和新关键词Id对照词典
            //foreach (var old in oldKeys)
            //{
            //    var newKey = new Dnl_Keyword
            //    {
            //        _id = ObjectId.GenerateNewId(),
            //        Keyword = old.CommendKeyword,
            //        BotIntervalHours = 7 * 24,
            //        BotStatus_Baidu = 2,
            //        CreatedAt = old.CreatedAt,
            //        LastBotEndAt_Baidu = DateTime.Now.AddHours(8),
            //        ValLinkCount_Baidu = old.ValLinkCount
            //    };
            //    oldKeyToNew.Add(old._id, newKey._id);
            //    newKeys.Add(newKey);
            //}
            //MongoDBHelper.Instance.GetDnl_Keyword().InsertMany(newKeys);
            ////迁移链接
            //var keyIds = oldKeys.Select(x => x._id.ToString()).ToList();
            //var filterLink = Builders<IW2S_level1link>.Filter.In(x => x.SearchkeywordId, keyIds);
            //var oldLinks = MongoDBHelper.Instance.GetIW2S_level1links().Find(filterLink).ToList();
            //var newLinks = new List<Dnl_Link_Baidu>();
            //foreach (var old in oldLinks)
            //{
            //    var newLink = new Dnl_Link_Baidu
            //    {
            //        _id = ObjectId.GenerateNewId(),
            //        Title = old.Title,
            //        Description = old.Description,
            //        Abstract = old.Abstract,
            //        Html = old.Html,
            //        Domain = old.Domain,
            //        TopDomain = old.TopDomain,
            //        BaiduVStar = old.BaiduVStar,
            //        CreatedAt = old.CreatedAt,
            //        Keywords = old.Keywords,
            //        LinkUrl = old.LinkUrl,
            //        PublishTime = old.PublishTime,
            //        Rank = old.Rank,
            //        SearchkeywordId = oldKeyToNew[new ObjectId(old.SearchkeywordId)].ToString()
            //    };
            //    newLinks.Add(newLink);
            //}
            //MongoDBHelper.Instance.GetDnl_Link_Baidu().InsertMany(newLinks);
            ////迁移关键词词组
            //var builderCate=Builders<IW2S_KeywordCategory>.Filter;
            //var filterCate = builderCate.Eq(x => x.ProjectId, proObjId) & builderCate.Eq(x => x.IsDel, false);
            //var oldCates = MongoDBHelper.Instance.GetIW2S_KeywordCategorys().Find(filterCate).ToList();
            //var newCates = new List<Dnl_KeywordCategory>();
            //var oldCateToNew = new Dictionary<ObjectId, ObjectId>();
            //foreach (var old in oldCates)
            //{
            //    var newCate = new Dnl_KeywordCategory
            //    {
            //        _id = ObjectId.GenerateNewId(),
            //        ValLinkCount = old.ValLinkCount,
            //        IsDel = false,
            //        GroupNumber = old.GroupNumber,
            //        KeywordCount = old.KeywordTotal,
            //        InfriLawCode = old.InfriLawCode,
            //        Name = old.Name,
            //        ParentId = old.ParentId,
            //        ProjectId = old.ProjectId,
            //        UserId = old.UsrId,
            //        Weight = old.Weight
            //    };
            //    newCates.Add(newCate);
            //    oldCateToNew.Add(old._id, newCate._id);
            //}
            //MongoDBHelper.Instance.GetDnl_KeywordCategory().InsertMany(newCates);
            //if (!oldCateToNew.ContainsKey(ObjectId.Empty))
            //{
            //    oldCateToNew.Add(ObjectId.Empty, ObjectId.Empty);
            //}
            ////迁移词组内关键词
            //var filterGroup = Builders<IW2S_KeywordGroup>.Filter.Eq(x => x.ProjectId, proObjId);
            //var oldGroups = MongoDBHelper.Instance.GetIW2S_KeywordGroups().Find(filterGroup).ToList();
            //var newGroups = new List<Dnl_KeywordMapping>();
            //foreach (var old in oldGroups)
            //{
            //    var newGroup = new Dnl_KeywordMapping
            //    {
            //        _id = ObjectId.GenerateNewId(),
            //        IsDel = false,
            //        CategoryId = oldCateToNew[old.CommendCategoryId],
            //        ParentCategoryId = oldCateToNew[old.ParentCategoryId],
            //        Keyword = old.BaiduCommend,
            //        KeywordId = oldKeyToNew[old.BaiduCommendId],
            //        ProjectId = old.ProjectId,
            //        UserId = old.UsrId
            //    };
            //    newGroups.Add(newGroup);
            //}
            //MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newGroups);
            #endregion

            var userObjId = new ObjectId("58845bed1e5318078cb01b1a");
            var filter = Builders<Dnl_Keyword>.Filter.Ne(x => x._id, ObjectId.Empty);
            var keys = MongoDBHelper.Instance.GetDnl_Keyword().Find(filter).ToList();
            var newKeys = new List<Dnl_KeywordMapping>();
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
                newKeys.Add(y);
            }
            MongoDBHelper.Instance.GetDnl_KeywordMapping().InsertMany(newKeys);

            Console.ReadKey();
        }


    }
}
