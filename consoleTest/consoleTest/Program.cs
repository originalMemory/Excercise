using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpTest.Model;
using CSharpTest.Tools;
using CSharpTest.Helper;
using System.IO;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using IWSData.Model;

using NReadability;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JiebaNet.Segmenter;
using NReadability;

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var builder = Builders<MediaKeywordMappingMongo>.Filter;
            var filter = builder.Eq(x => x.ProjectId, new ObjectId("595afd02237cbe063c4f0b08"));
            filter &= builder.Eq(x => x.CategoryId, ObjectId.Empty);
            var col = MongoDBHelper.Instance.GetMediaKeywordMapping();
            var query = col.Find(filter).ToList();
            query.RemoveAll(x => x.Keyword == "真爱梦想");
            var keyObjIds = query.Select(x => x.KeywordId);
            var filterKey = Builders<MediaKeywordMongo>.Filter.In(x => x._id, keyObjIds);
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var update = new UpdateDocument { { "$set", new QueryDocument { { "WXBotStatus", 0 } } } };
            colKey.UpdateMany(filterKey, update);
            
            DnlTools tools = new DnlTools();
            //tools.AnalysizeWeiXinLink();
            Console.ReadKey();
        }

    }
}
