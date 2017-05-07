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

namespace cPlusPlusTest
{
    class MongoTest
    {
        public void BotQuery()
        {
            var userid = new ObjectId("58845bed1e5318078cb01b1a");
            var projectid = new ObjectId("588712927c08250b487fcfd1");
            var builderKey = Builders<IW2S_BaiduCommend>.Filter;
            var filterKey = builderKey.Eq(x => x.ProjectId, projectid) & builderKey.Eq(x => x.IsRemoved, false);
            var queryKey = MongoDBHelper.Instance.GetIW2S_BaiduCommends().Find(filterKey).ToList();
            var keyBotId = queryKey.Select(x => x.BotId).ToList();

            var filterBot = Builders<IW2S_BotRegister>.Filter.In(x => x.BotId, keyBotId);
            var queryBot = MongoDBHelper.Instance.GetIW2S_BotRegister().Find(filterBot).ToList();
            foreach (var bot in queryBot)
            {
                Console.WriteLine(bot.BotId+" "+bot.ProcessId);
            }
        }

        /// <summary>
        /// 获取公用域名分组
        /// </summary>
        /// <param name="userId">来源用户Id</param>
        public void GetCommonDomainCate(string userId)
        {
            var userObjId = new ObjectId(userId);
            //获取原有域名分组
            var builderCate = Builders<IW2S_DomainCategory>.Filter;
            var filterCate = builderCate.Eq(x => x.UsrId, userObjId);
            filterCate &=builderCate.Eq(x=>x.IsDel,false);
            var colCate=MongoDBHelper.Instance.GetIW2S_DomainCategorys();
            var oldCateList = colCate.Find(filterCate).ToList();
            //写入公用域名分组
            var newCateList = new List<IW2S_DomainCategory>();
            Console.WriteLine("写入分组：");
            foreach (var old in oldCateList)
            {
                var cate = new IW2S_DomainCategory
                {
                    IsDel = false,
                    Name = old.Name,
                    ParentId = old.ParentId,
                    ParentName = old.ParentName,
                    UsrId = ObjectId.Empty
                };
                Console.WriteLine(old.Name);
                newCateList.Add(cate);
            }
            colCate.InsertMany(newCateList);
            //获取新分组
            var filterNewCate=builderCate.Eq(x=>x.UsrId,ObjectId.Empty);
            var queryCate = colCate.Find(filterNewCate).ToList();
            //建立新旧分组Id词典
            var oldToNew = new Dictionary<ObjectId, ObjectId>();
            foreach (var old in oldCateList)
            {
                var newId = queryCate.Find(x => x.Name == old.Name)._id;
                oldToNew.Add(old._id, newId);
            }
            //获取原有域名数据
            var builderDate = Builders<IW2S_DomainCategoryData>.Filter;
            var filterDate = builderDate.Eq(x => x.UsrId, userObjId);
            var colDate = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas();
            var oldDateList = colDate.Find(filterDate).ToList();
            //写入公用域名分组数据
            var newDateList = new List<IW2S_DomainCategoryData>();
            Console.WriteLine("写入域名：");
            foreach (var old in oldDateList)
            {
                if(oldToNew.ContainsKey(old.DomainCategoryId))
                {
                    var date = new IW2S_DomainCategoryData
                    {
                        DomainCategoryId = oldToNew[old.DomainCategoryId],
                        DomainName = old.DomainName,
                        UsrId = ObjectId.Empty
                    };
                    Console.WriteLine(old.DomainName);
                    newDateList.Add(date);
                }
            }
            colDate.InsertMany(newDateList);
        }
    }
}
