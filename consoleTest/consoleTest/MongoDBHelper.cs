using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AISSystem;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoV2;

using cPlusPlusTest.Model;

namespace cPlusPlusTest
{
    public class MongoDBHelper:MDB
    {

        static string conn = AppSettingHelper.GetAppSetting("mongoCon");

        static string dbName = AppSettingHelper.GetAppSetting("mongoDB");


        public MongoDBHelper()
            : base(conn, dbName)
        {

        }

        public static readonly MongoDBHelper Instance = new MongoDBHelper();


        public IMongoCollection<IW2S_level1link> GetIW2S_level1links()
        {
            return base.GetCollection<IW2S_level1link>("IW2S_level1link");
        }

        public IMongoCollection<IW2S_BaiduCommend> GetIW2S_BaiduCommends()
        {
            return base.GetCollection<IW2S_BaiduCommend>("IW2S_BaiduCommend");
        }

        public IMongoCollection<IW2S_WX_BaiduCommend> GetIW2S_WX_BaiduCommends()
        {
            return base.GetCollection<IW2S_WX_BaiduCommend>("IW2S_WX_BaiduCommend");
        }

        public IMongoCollection<IW2S_WX_level1link> GetIW2S_WX_level1links()
        {
            return base.GetCollection<IW2S_WX_level1link>("IW2S_WX_level1link");
        }

        public IMongoCollection<IW2S_WB_BaiduCommend> GetIW2S_WB_BaiduCommends()
        {
            return base.GetCollection<IW2S_WB_BaiduCommend>("IW2S_WB_BaiduCommend");
        }

        public IMongoCollection<IW2S_WB_level1link> GetIW2S_WB_level1links()
        {
            return base.GetCollection<IW2S_WB_level1link>("IW2S_WB_level1link");
        }

        public IMongoCollection<IW2S_SG_BaiduCommend> GetIW2S_SG_BaiduCommends()
        {
            return base.GetCollection<IW2S_SG_BaiduCommend>("IW2S_SG_BaiduCommend");
        }

        public IMongoCollection<IW2S_SG_level1link> GetIW2S_SG_level1links()
        {
            return base.GetCollection<IW2S_SG_level1link>("IW2S_SG_level1link");
        }

        public IMongoCollection<IW2S_BotRegister> GetIW2S_BotRegister()
        {
            return GetCollection<IW2S_BotRegister>("IW2S_BotRegister");
        }

        public IMongoCollection<IW2S_Project> GetIW2S_Projects()
        {
            return base.GetCollection<IW2S_Project>("IW2S_Project");
        }

        public IMongoCollection<IW2S_KeywordCategory> GetIW2S_KeywordCategorys()
        {
            return base.GetCollection<IW2S_KeywordCategory>("IW2S_KeywordCategory");
        }
        public IMongoCollection<IW2S_KeywordGroup> GetIW2S_KeywordGroups()
        {
            return base.GetCollection<IW2S_KeywordGroup>("IW2S_KeywordGroup");
        }

        public IMongoCollection<IW2S_DomainCategory> GetIW2S_DomainCategorys()
        {
            return base.GetCollection<IW2S_DomainCategory>("IW2S_DomainCategory");
        }
        public IMongoCollection<IW2S_DomainCategoryData> GetIW2S_DomainCategoryDatas()
        {
            return base.GetCollection<IW2S_DomainCategoryData>("IW2S_DomainCategoryData");
        }

        public IMongoCollection<test> GetTest()
        {
            return base.GetCollection<test>("test");
        }
    }

    public static class MongoExtension
    {
        public static ObjectId ToObjectId(this string txt)
        {
            var bs = Encoding.Default.GetBytes(txt);
            byte[] arr = new byte[12];
            foreach (var b in bs)
            {
                Mutl(arr, 31);
                Add(arr, b);
            }
            string hex = GetHex(arr);
            return ObjectId.Parse(hex);
        }


        static void Mutl(byte[] bytes, byte a)
        {
            uint preAdd = 0;
            for (var i = 0; i < bytes.Length; i++)
            {
                uint b = ((uint)bytes[i]) * ((uint)a) + preAdd;
                preAdd = b / 256;
                bytes[i] = (byte)(b % 256);
            }
        }

        static void Add(byte[] bytes, byte a)
        {
            uint preAdd = a;
            for (var i = 0; i < bytes.Length; i++)
            {
                uint b = ((uint)bytes[i]) + preAdd;
                bytes[i] = (byte)(b % 256);
                if (b < 256)
                    break;
                preAdd = b / 256;
            }
        }

        static string GetHex(byte[] bs)
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < bs.Length; i++)
            {
                sb.Append(bs[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
