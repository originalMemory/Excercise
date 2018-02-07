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
using IWSData.Model;

using CSharpTest.Model;

namespace CSharpTest.Helper
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

        public IMongoCollection<IW2S_BotRegister> GetIW2S_BotRegister()
        {
            return GetCollection<IW2S_BotRegister>("IW2S_BotRegister");
        }

        public IMongoCollection<IW2SUser> Get_IW2SUser()
        {
            return base.GetCollection<IW2SUser>("IW2SUser");
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
        #region 数据简报
        public IMongoCollection<Dnl_Report> GetDnl_Report()
        {
            return base.GetCollection<Dnl_Report>("Dnl_Report");
        }

        public IMongoCollection<Dnl_ReportShare> GetDnl_ReportShare()
        {
            return base.GetCollection<Dnl_ReportShare>("Dnl_ReportShare");
        }

        public IMongoCollection<Dnl_Report_Keyword> GetDnl_Report_Keyword()
        {
            return base.GetCollection<Dnl_Report_Keyword>("Dnl_Report_Keyword");
        }

        public IMongoCollection<Dnl_Report_KeywordCategory> GetDnl_Report_KeywordCategory()
        {
            return base.GetCollection<Dnl_Report_KeywordCategory>("Dnl_Report_KeywordCategory");
        }

        public IMongoCollection<Dnl_Report_Statistics> GetDnl_Report_Statistics()
        {
            return base.GetCollection<Dnl_Report_Statistics>("Dnl_Report_Statistics");
        }

        public IMongoCollection<Dnl_Report_keywordChart> GetDnl_Report_keywordChart()
        {
            return base.GetCollection<Dnl_Report_keywordChart>("Dnl_Report_keywordChart");
        }

        public IMongoCollection<Dnl_Report_LinkChart> GetDnl_Report_LinkChart()
        {
            return base.GetCollection<Dnl_Report_LinkChart>("Dnl_Report_LinkChart");
        }

        public IMongoCollection<Dnl_Report_LinkChartCategory> GetDnl_Report_LinkChartCategory()
        {
            return base.GetCollection<Dnl_Report_LinkChartCategory>("Dnl_Report_LinkChartCategory");
        }

        public IMongoCollection<Dnl_Report_Description> GetDnl_Report_Description()
        {
            return base.GetCollection<Dnl_Report_Description>("Dnl_Report_Description");
        }

        public IMongoCollection<Dnl_Report_DomainChart> GetDnl_Report_DomainChart()
        {
            return base.GetCollection<Dnl_Report_DomainChart>("Dnl_Report_DomainChart");
        }

        public IMongoCollection<Dnl_WordTree> GetDnl_WordTree()
        {
            return base.GetCollection<Dnl_WordTree>("Dnl_WordTree");
        }

        public IMongoCollection<Dnl_Report_WordTree> GetDnl_Report_WordTree()
        {
            return base.GetCollection<Dnl_Report_WordTree>("Dnl_Report_WordTree");
        }

        public IMongoCollection<Dnl_Report_TimeLink> GetDnl_Report_TimeLink()
        {
            return base.GetCollection<Dnl_Report_TimeLink>("Dnl_Report_TimeLink");
        }
        #endregion

        public IMongoCollection<Dnl_EntityTree> GetDnl_EntityTree()
        {
            return base.GetCollection<Dnl_EntityTree>("Dnl_EntityTree");
        }

        public IMongoCollection<Dnl_EntityTreeMapping> GetDnl_EntityTreeMapping()
        {
            return base.GetCollection<Dnl_EntityTreeMapping>("Dnl_EntityTreeMapping");
        }

        #region 独立关键词与链接库
        public IMongoCollection<Dnl_Keyword> GetDnl_Keyword()
        {
            return base.GetCollection<Dnl_Keyword>("Dnl_Keyword");
        }

        public IMongoCollection<Dnl_KeywordCategory> GetDnl_KeywordCategory()
        {
            return base.GetCollection<Dnl_KeywordCategory>("Dnl_KeywordCategory");
        }

        public IMongoCollection<Dnl_KeywordMapping> GetDnl_KeywordMapping()
        {
            return base.GetCollection<Dnl_KeywordMapping>("Dnl_KeywordMapping");
        }

        public IMongoCollection<Dnl_Link_Baidu> GetDnl_Link_Baidu()
        {
            return base.GetCollection<Dnl_Link_Baidu>("Dnl_Link_Baidu");
        }

        public IMongoCollection<BingLinkMainMongo> GetBingLinkMain()
        {
            return base.GetCollection<BingLinkMainMongo>("BingLinkMain");
        }

        public IMongoCollection<BingLinkContentMongo> GetBingLinkContent()
        {
            return base.GetCollection<BingLinkContentMongo>("BingLinkContent");
        }

        public IMongoCollection<BingLinkOtherMongo> GetBingLinkOther()
        {
            return base.GetCollection<BingLinkOtherMongo>("BingLinkOther");
        }
        public IMongoCollection<Dnl_LinkMapping_Baidu> GetDnl_LinkMapping_Baidu()
        {
            return base.GetCollection<Dnl_LinkMapping_Baidu>("Dnl_LinkMapping_Baidu");
        }
        #endregion

        #region 社交媒体关键词与链接库
        public IMongoCollection<MediaKeywordMongo> GetMediaKeyword()
        {
            return base.GetCollection<MediaKeywordMongo>("MediaKeyword");
        }

        public IMongoCollection<MediaKeywordCategoryMongo> GetMediaKeywordCategory()
        {
            return base.GetCollection<MediaKeywordCategoryMongo>("MediaKeywordCategory");
        }

        public IMongoCollection<MediaKeywordMappingMongo> GetMediaKeywordMapping()
        {
            return base.GetCollection<MediaKeywordMappingMongo>("MediaKeywordMapping");
        }

        public IMongoCollection<WXLinkMainMongo> GetWXLinkMain()
        {
            return base.GetCollection<WXLinkMainMongo>("WXLinkMain");
        }

        public IMongoCollection<WXLinkOtherMongo> GetWXLinkOther()
        {
            return base.GetCollection<WXLinkOtherMongo>("WXLinkOther");
        }

        public IMongoCollection<WXLinkContentMongo> GetWXLinkContent()
        {
            return base.GetCollection<WXLinkContentMongo>("WXLinkContent");
        }

        public IMongoCollection<WXNameMongo> GetWXName()
        {
            return base.GetCollection<WXNameMongo>("WXName");
        }
        #endregion

        #region 百度链接库
        public IMongoCollection<BaiduLinkMainMongo> GetBaiduLinkMain()
        {
            return base.GetCollection<BaiduLinkMainMongo>("BaiduLinkMain");
        }
        public IMongoCollection<BaiduLinkOtherMongo> GetBaiduLinkOther()
        {
            return base.GetCollection<BaiduLinkOtherMongo>("BaiduLinkOther");
        }
        public IMongoCollection<BaiduLinkContentMongo> GetBaiduLinkContent()
        {
            return base.GetCollection<BaiduLinkContentMongo>("BaiduLinkContent");
        }
        #endregion

        public IMongoCollection<LinkMappingMongo> GetLinkMapping()
        {
            return base.GetCollection<LinkMappingMongo>("LinkMapping");
        }

        

        public IMongoCollection<ProjectChartMongo> GetPojectChart()
        {
            return base.GetCollection<ProjectChartMongo>("PojectChart");
        }

        public IMongoCollection<ProductMongo> GetProduct()
        {
            return base.GetCollection<ProductMongo>("Product");
        }

        public IMongoCollection<OrderMongo> GetOrder()
        {
            return base.GetCollection<OrderMongo>("Order");
        }

        public IMongoCollection<Dnl_ProjectGroup> GetDnl_ProjectGroup()
        {
            return base.GetCollection<Dnl_ProjectGroup>("Dnl_ProjectGroup");
        }

        public IMongoCollection<Dnl_ProjectCategory> GetDnl_ProjectCategory()
        {
            return base.GetCollection<Dnl_ProjectCategory>("Dnl_ProjectCategory");
        }

        public IMongoCollection<IW2S_ProLinksCount> GetIW2S_ProLinksCount()
        {
            return base.GetCollection<IW2S_ProLinksCount>("IW2S_ProLinksCount");
        }

        public IMongoCollection<IW2S_ProjectShare> GetIW2S_ProjectShare()
        {
            return base.GetCollection<IW2S_ProjectShare>("IW2S_ProjectShare");
        }

        public IMongoCollection<Dnl_MappingCoPresent> GetDnl_MappingCoPresent()
        {
            return base.GetCollection<Dnl_MappingCoPresent>("Dnl_MappingCoPresent");
        }
        public IMongoCollection<MediaMappingCoPresentMongo> GetMediaMappingCoPresent()
        {
            return base.GetCollection<MediaMappingCoPresentMongo>("MediaMappingCoPresent");
        }

        public IMongoCollection<VipMaUserMongo> GetVipMaUser()
        {
            return base.GetCollection<VipMaUserMongo>("User");
        }

        public IMongoCollection<FoundationMongo> GetFoundation()
        {
            return base.GetCollection<FoundationMongo>("Foundation");
        }

        public IMongoCollection<Foundation_projectMongo> GetFoundation_project()
        {
            return base.GetCollection<Foundation_projectMongo>("Foundation_project");
        }

        #region 分析项目
        public IMongoCollection<AnaProCateMongo> GetAnaProCate()
        {
            return base.GetCollection<AnaProCateMongo>("AnaProCate");
        }
        public IMongoCollection<AnaProMappingMongo> GetAnaProMapping()
        {
            return base.GetCollection<AnaProMappingMongo>("AnaProMapping");
        }
        public IMongoCollection<AnaProBindTagMongo> GetAnaProBind()
        {
            return base.GetCollection<AnaProBindTagMongo>("AnaProBind");
        }
        #endregion

        #region 标签
        public IMongoCollection<TagMongo> GetTag()
        {
            return base.GetCollection<TagMongo>("Tag");
        }
        public IMongoCollection<TagLinkMappingMongo> GetTagLinkMapping()
        {
            return base.GetCollection<TagLinkMappingMongo>("TagLinkMapping");
        }
        public IMongoCollection<TagCategoryMongo> GetTagCategory()
        {
            return base.GetCollection<TagCategoryMongo>("TagCategory");
        }
        public IMongoCollection<TagMappingMongo> GetTagMapping()
        {
            return base.GetCollection<TagMappingMongo>("TagMapping");
        }
        public IMongoCollection<TagChartMongo> GetTagChart()
        {
            return base.GetCollection<TagChartMongo>("TagChart");
        }
        #endregion

        public IMongoCollection<ProjectChartMongo> GetProjectChart()
        {
            return base.GetCollection<ProjectChartMongo>("ProjectChart");
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
