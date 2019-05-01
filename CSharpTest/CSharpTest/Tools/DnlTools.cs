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
using IWSData.Model;
using System.Threading;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;

using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using System.Web;
using NReadability;

namespace CSharpTest.Tools
{
    public class DnlTools
    {
        /// <summary>
        /// 计算百度链接数
        /// </summary>
        public static void CountLinkNum()
        {
            var colKey=MongoDBHelper.Instance.GetDnl_Keyword();
            var keywords = colKey.Find(Builders<Dnl_Keyword>.Filter.Empty).ToList();
            int i = 1;
            foreach (var key in keywords)
            {
                CommonTools.Log("关键词["+i+"/+"+keywords.Count+"]：" + key.Keyword);
                var filterLink = Builders<Dnl_Link_Baidu>.Filter.Eq(x => x.SearchkeywordId, key._id.ToString());
                var queryLink = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => x._id).ToList();
                int num = queryLink.Count;
                var update = new UpdateDocument { { "$set", new QueryDocument { { "LinkCount_Baidu", num } } } };
                var filterKey = Builders<Dnl_Keyword>.Filter.Eq(x => x._id, key._id);
                colKey.UpdateOne(filterKey, update);
                CommonTools.Log("链接数：" + num + "\n");
                i++;
            }
        }

        /// <summary>
        /// 计算微信链接数
        /// </summary>
        public static void CountWXLinkNum(ObjectId projectId, string cateName="")
        {
            var builderKeyCate = Builders<MediaKeywordCategoryMongo>.Filter;
            var colKeyCate = MongoDBHelper.Instance.GetMediaKeywordCategory();
            var filterKeyCate = builderKeyCate.Eq(x => x.ProjectId, projectId) & builderKeyCate.Eq(x => x.Name, cateName);
            var cateId = colKeyCate.Find(filterKeyCate).Project(x => x._id).FirstOrDefault();

            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.IsDel, false);
            if (cateId == ObjectId.Empty)
            {
                filterKeyMap &= builderKeyMap.Eq(x => x.ProjectId, projectId) & builderKeyMap.Eq(x => x.CategoryId, ObjectId.Empty);
            }
            else
            {
                filterKeyMap &= builderKeyMap.Eq(x => x.CategoryId, cateId);
            }

            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);
            foreach (var key in keys)
            {
                if (key.Name == "圣罗兰包 一比一")
                {
                    int adf = 343;
                }
                var filterLink = builderLinkMain.Eq(x => x.KeywordId, key.Id);
                filterLink &= builderLinkMain.Gte(x => x.CreatedAt, new DateTime(2018, 12, 20));
                var linkNum = colLinkMain.Count(filterLink);
                colKey.UpdateOne(builderKey.Eq(x => x._id, new ObjectId(key.Id)), Builders<MediaKeywordMongo>.Update.Set(x => x.WXLinkNum, Convert.ToInt32(linkNum)));
                CommonTools.Log("关键词 - {0}，链接数 - {1}".FormatStr(key.Name, linkNum));
            }
        }

        /// <summary>
        /// 计算百度链接收录数
        /// </summary>
        public static void ComputeDCNum()
        {
            var sw=new System.Diagnostics.Stopwatch();
            sw.Start();
            int computeNum = 0;
            int errorNum = 0;       //连续异常数
            int startPos,length;
            Console.Write("输入起始位置：");
            startPos = Convert.ToInt32(Console.ReadLine());
            Console.Write("输入计算数量：");
            length = Convert.ToInt32(Console.ReadLine());
            //获取现在所有搜索完成的关键词
            var filterKey = Builders<Dnl_Keyword>.Filter.Eq(x => x.BotStatus_Baidu, 2);
            var keywords = MongoDBHelper.Instance.GetDnl_Keyword().Find(filterKey).ToList();
            keywords = keywords.OrderBy(x => x.CreatedAt).ToList();
            int top = startPos + length;
            for (int i = startPos; i <= top; i++)
            {
                if (i == keywords.Count)
                    break;

                //if (computeNum == 5000)
                //{
                //    CommonTools.Log("计算量已接近阈值，暂停30分钟！");
                //    Thread.Sleep(30 * 60 * 1000);
                //}

                CommonTools.Log("当前关键词[" + i + "/" + keywords.Count + "] - " + keywords[i].Keyword);
                try
                {
                    //获取关键词对应链接
                    var filterLink = Builders<Dnl_Link_Baidu>.Filter.Eq(x => x.SearchkeywordId, keywords[i-1]._id.ToString());
                    var col = MongoDBHelper.Instance.GetDnl_Link_Baidu();
                    var links = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => new
                    {
                        Id = x._id,
                        Domain = x.Domain,
                        DcNum=x.DCNum
                    }).ToList();
                    int j = 1;
                    foreach (var x in links)
                    {
                        if (errorNum == 30)
                        {
                            sw.Stop();
                            CommonTools.Log("计算时间：{0}s\t计算链接数{1}".FormatStr(sw.Elapsed, computeNum));
                            //Thread.Sleep(24 * 60 * 60 * 1000);
                        }
                        if (x.DcNum == 0)
                        {
                            string domain = x.Domain.Replace("https://", "");
                            domain = domain.Replace("http://", "");
                            if (string.IsNullOrEmpty(domain))
                                domain = domain.Trim();
                            long num = WebApiInvoke.GetDomainCollectionNum(domain);
                            //判断是否连续异常
                            if (num == 0)
                            {
                                errorNum++;
                            }
                            else
                            {
                                errorNum = 0;
                            }
                            CommonTools.Log("域名 - {0}\t收录量 - {1}".FormatStr(domain, num));
                            computeNum++;
                            var update = new UpdateDocument { { "$set", new QueryDocument { { "DCNum", num } } } };
                            var filterUp = Builders<Dnl_Link_Baidu>.Filter.Eq(s => s._id, x.Id);
                            try
                            {
                                col.UpdateOne(filterUp, update);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            CommonTools.Log("域名 - {0}\t收录量 - {1}".FormatStr(x.Domain, x.DcNum));
                            errorNum = 0;
                        }
                        j++;
                    }
                    CommonTools.Log("本关键词对应链接数 - " + links.Count);
                    Console.WriteLine("\n");
                }
                catch (Exception ex)
                {
                    CommonTools.Log(ex.Message);
                }
            }

            CommonTools.Log("全部迁移完毕！");
        }

        
        /// <summary>
        /// 微信数据分析
        /// </summary>
        public static void AnalysizeWeiXinLink()
        {
            string baseUrl = System.AppDomain.CurrentDomain.BaseDirectory;

            //获取项目
            var builderPro = Builders<IW2S_Project>.Filter;
            string proName = "普世佳音-主内微信影响力评测";
            var filterPro = builderPro.Eq(x => x.Name, proName);
            var queryPro = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).FirstOrDefault();
            //获取项目内所有关键词
            var builderMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterMap = builderMap.Eq(x => x.ProjectId, queryPro._id);
            filterMap &= builderMap.Eq(x => x.IsDel, false);
            var keyObjIds = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterMap).Project(x => x.KeywordId).ToList();

            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var filterKey = builderKey.In(x => x._id, keyObjIds);
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();

            var builderLink = Builders<WxLinkMainMongo>.Filter;
            var WXlinks = new List<WxLinkMainMongo>();

            string filename = "微信5月数据.xls";
            string path = baseUrl + @"ExportFiles\" + filename;

            HSSFWorkbook linkExcel = new HSSFWorkbook();     //Excel表格
            ISheet linkSheet = linkExcel.CreateSheet("5月链接数据");
            IRow RowHead0 = linkSheet.CreateRow(0);
            RowHead0.CreateCell(0).SetCellValue("链接Id");
            RowHead0.CreateCell(1).SetCellValue("关键词");
            RowHead0.CreateCell(2).SetCellValue("关键词Id");
            RowHead0.CreateCell(3).SetCellValue("公众号呢称");
            RowHead0.CreateCell(4).SetCellValue("公众号Id");
            RowHead0.CreateCell(5).SetCellValue("发布时间");
            RowHead0.CreateCell(6).SetCellValue("标题");
            RowHead0.CreateCell(7).SetCellValue("摘要");
            RowHead0.CreateCell(8).SetCellValue("正文长度");
            RowHead0.CreateCell(9).SetCellValue("链接地址");
            RowHead0.CreateCell(10).SetCellValue("阅读量");
            RowHead0.CreateCell(11).SetCellValue("点赞量");
            RowHead0.CreateCell(12).SetCellValue("是否已被删除");

            int i = 1;
            try
            {
                foreach (var key in queryKey)
                {
                    CommonTools.Log("当前获取关键词[{0}/{1}] - {2}".FormatStr(i, queryKey.Count, key.Keyword));
                    if (key.Keyword == "国度")
                    {
                        i++;
                        continue;
                    }
                    int page = 0, pagesize = 5000;
                    var filterLink = builderLink.Eq(x => x.KeywordId, key._id.ToString());
                    DateTime startDate = new DateTime(2017, 5, 1);
                    DateTime endDate = new DateTime(2017, 6, 1);
                    //filterLink &= builderLink.Gte(x => x.PostTime, startDate.AddHours(8));
                    var queryLink = MongoDBHelper.Instance.GetWXLinkMain().Find(filterLink);
                    int topLinkNum = (int)queryLink.Count();
                    int nowLinkNum = 0;
                    while (true)
                    {
                        var sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        var tempLinks = queryLink.Skip(page * pagesize).Limit(pagesize).ToList();
                        sw.Stop();
                        WXlinks.AddRange(tempLinks);
                        nowLinkNum += tempLinks.Count;
                        CommonTools.Log("当前获取链接[{0}/{1}]\t耗时 - {2}s".FormatStr(nowLinkNum, topLinkNum, sw.Elapsed));
                        if (nowLinkNum >= topLinkNum)
                            break;
                        page++;
                        
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                CommonTools.Log(ex.Message);
            }

            int j = 1;
            foreach (var link in WXlinks)
            {
                IRow row = linkSheet.CreateRow(j);
                row.CreateCell(0).SetCellValue(link._id.ToString());
                row.CreateCell(1).SetCellValue(link.Keyword);
                row.CreateCell(2).SetCellValue(link.KeywordId);
                row.CreateCell(3).SetCellValue(link.Nickname);
                row.CreateCell(4).SetCellValue(link.Name);
                row.CreateCell(5).SetCellValue(link.PostTime.ToString());
                row.CreateCell(6).SetCellValue(link.Title);
                row.CreateCell(7).SetCellValue(link.Description);
                row.CreateCell(8).SetCellValue(link.ContentLen);
                row.CreateCell(9).SetCellValue(link.Url);
                row.CreateCell(10).SetCellValue(link.ReadNum);
                row.CreateCell(11).SetCellValue(link.LikeNum);
                row.CreateCell(12).SetCellValue(link.IsDelByAu);
                j++;
            }
            string path3 = @"F:\微信半年数据.xls";
            using (FileStream fileAna = new FileStream(path3, FileMode.Create))
            {
                linkExcel.Write(fileAna);　　//创建Excel文件。
            }
            

            HSSFWorkbook workBook = new HSSFWorkbook();     //Excel表格

            //按关键词分组数据
            
            //导出关键词信息
            ISheet sheet1 = workBook.CreateSheet("关键词统计");
            IRow RowHead1 = sheet1.CreateRow(0);
            RowHead1.CreateCell(0).SetCellValue("关键词");
            RowHead1.CreateCell(1).SetCellValue("公众号数");
            RowHead1.CreateCell(2).SetCellValue("文章数");
            RowHead1.CreateCell(3).SetCellValue("阅读量");
            RowHead1.CreateCell(4).SetCellValue("点赞量");

            int count = 1;
            foreach (var key in queryKey)
            {
                var links = WXlinks.FindAll(x => x.KeywordId == key._id.ToString());
                IRow row = sheet1.CreateRow(count);
                row.CreateCell(0).SetCellValue(key.Keyword);
                var names = links.Select(x => x.Name).Distinct();
                row.CreateCell(1).SetCellValue(names.Count());
                row.CreateCell(2).SetCellValue(links.Count);
                int readNum = links.Sum(x => x.ReadNum);
                row.CreateCell(3).SetCellValue(readNum);
                int likeNum = links.Sum(x => x.LikeNum);
                row.CreateCell(4).SetCellValue(likeNum);
                count = count + 1;
            }

            //获取所有公众号信息
            ISheet sheet2 = workBook.CreateSheet("公众号统计");
            IRow RowHead2 = sheet2.CreateRow(0);
            RowHead2.CreateCell(0).SetCellValue("公众号");
            RowHead2.CreateCell(1).SetCellValue("关键词数");
            RowHead2.CreateCell(2).SetCellValue("文章数");
            RowHead2.CreateCell(3).SetCellValue("阅读量");
            RowHead2.CreateCell(4).SetCellValue("点赞量");
            var allName = WXlinks.Select(x => x.Nickname).ToList();
            allName = allName.Distinct().ToList();
            count = 1;
            foreach (var name in allName)
            {
                //导出所有分组信息
                var keys = new List<string>();
                int readNum = 0, likeNum = 0;
                var links = WXlinks.FindAll(x => x.Nickname == name);
                keys = links.Select(x => x.Keyword).Distinct().ToList();
                IRow row = sheet2.CreateRow(count);
                row.CreateCell(0).SetCellValue(name);
                row.CreateCell(1).SetCellValue(keys.Count);
                links = links.DistinctBy(x => x.Url);
                row.CreateCell(2).SetCellValue(links.Count());
                readNum = links.Sum(x => x.ReadNum);
                likeNum = links.Sum(x => x.LikeNum);
                row.CreateCell(3).SetCellValue(readNum);
                row.CreateCell(4).SetCellValue(likeNum);
                count = count + 1;
            }

            string filename2 = "微信半年数据分析" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
            string path2 = @"F:\" + filename2;
            using (FileStream fileAna = new FileStream(path2, FileMode.Create))
            {
                workBook.Write(fileAna);　　//创建Excel文件。
                fileAna.Close();
            }
            CommonTools.Log("计算完毕！");
        }

        /// <summary>
        /// 计算微信正文
        /// </summary>
        public static void GetWeiXinLinkContent()
        {
            string baseUrl = System.AppDomain.CurrentDomain.BaseDirectory;

            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            var builderMainLink = Builders<WxLinkMainMongo>.Filter;
            var colMainLink = MongoDBHelper.Instance.GetWXLinkMain();
            var colOtherLink = MongoDBHelper.Instance.GetWXLinkOther();
            var colConLink = MongoDBHelper.Instance.GetWXLinkContent();
            try
            {
                int i = 1;
                foreach (var key in queryKey)
                {
                    CommonTools.Log("当前处理关键词[{0}/{1}] - {2}".FormatStr(i, queryKey.Count, key.Keyword));
                    int page = 0, pagesize = 100;
                    //var filterLink = builderMainLink.Eq(x => x.KeywordId, key._id.ToString());
                    var filterLink = builderMainLink.In(x => x.KeywordId, queryKey.Select(x=>x._id.ToString()).ToList());
                    var queryLink = colMainLink.Find(filterLink);
                    int topLinkNum = (int)queryLink.Count();
                    int nowLinkNum = 0;
                    int computeNum = 1;
                    if (i < 10)
                    {
                        i++;
                        continue;
                    }
                    if (i == 5)
                    {
                        page = 145;
                        nowLinkNum = page * pagesize;
                        computeNum = nowLinkNum + 1;
                    }
                    while (true)
                    {
                        var sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        var tempLinks = queryLink.Project(x => new WeiXinLinkDto
                        {
                            PublishTime = x.PostTime,
                            KeywordId = x.KeywordId,
                            Title = x.Title,
                            ContentLen = x.ContentLen,
                            Description = x.Description
                        }).ToList();
                        //var tempLinks = queryLink.Skip(page * pagesize).Limit(pagesize).ToList();
                        sw.Stop();
                        nowLinkNum += tempLinks.Count;
                        CommonTools.Log("当前获取链接[{0}/{1}]\t耗时 - {2}s".FormatStr(nowLinkNum, topLinkNum, sw.Elapsed));

                        //sw.Restart();
                        //var contentLinks = new List<WxLinkContentMongo>();
                        //foreach (var x in tempLinks)
                        //{
                        //    CommonTools.Log("当前计算链接[{0}/{1}]\t标题 - {2}".FormatStr(computeNum, topLinkNum, x.Title));
                        //    //获取原来保存的网页源码和正文
                        //    var filterHtml = builderLink.Eq(s => s.KeywordId, x.KeywordId) & builderLink.Eq(s => s.Url, x.Url);
                        //    var sw2 = new System.Diagnostics.Stopwatch();
                        //    sw2.Start();
                        //    string html = colLink.Find(filterHtml).Project(s => s.Html).FirstOrDefault();
                        //    sw2.Stop();
                        //    var temp = new WxLinkContentMongo
                        //    {
                        //        LinkId = x._id,
                        //        KeywordId = x.KeywordId,
                        //    };
                        //    if (html == null)
                        //    {
                        //        html = WebApiInvoke.GetHtml(x.Url);
                        //        if (html == null)
                        //        {
                        //            CommonTools.Log("网页源码获取出错！");
                        //            Console.ReadLine();
                        //        }
                        //    }
                        //    //提取正文
                        //    temp.Html = html;
                        //    HtmlDocument doc = new HtmlDocument();
                        //    doc.LoadHtml(html);
                        //    HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id=\"js_content\"]");     //定位正文位置
                        //    var filterUp = builderMainLink.Eq(s => s._id, x._id);
                        //    if (node != null)
                        //    {
                        //        string content = CommonTools.RemoveTextTag(node.InnerHtml);
                        //        temp.Content = content;
                        //        //将正文按行拆分
                        //        string text =content.Replace("\r", "");
                        //        var listStr = text.Split('\n').ToList();
                        //        string desc = "";
                        //        for (int j = 0; j < listStr.Count; j++)
                        //        {
                        //            string str = listStr[j];
                        //            //跳过公众号统一的关注话语
                        //            if (j == 0 && str.Contains("关注"))
                        //            {
                        //                continue;
                        //            }
                        //            //限制摘要不超过50个词
                        //            desc += str;
                        //            if (desc.Length >= 50)
                        //            {
                        //                if (desc.Length > 100)
                        //                    desc = desc.Substring(0, 97)+"...";
                        //                break;
                        //            }
                        //        }
                        //        var update = new UpdateDocument { { "$set", new QueryDocument { { "Description", desc } } } };
                        //        colMainLink.UpdateOne(filterUp, update);
                        //    }
                        //    else
                        //    {
                        //        temp.Content = "";
                        //        //if (html.Contains("该内容已被发布者删除") || html.Contains("该公众号已迁移") || html.Contains("此内容因违规无法查看"))
                        //        if (html != null)
                        //        {
                        //            var update = new UpdateDocument { { "$set", new QueryDocument { { "IsDelByAu", true } } } };
                        //            colMainLink.UpdateOne(filterUp, update);
                        //        }
                        //        else
                        //        {
                        //            CommonTools.Log("正文提取获取出错！");
                        //            Console.ReadLine();
                        //        }
                        //    }
                        //    contentLinks.Add(temp);
                        //    computeNum++;
                        //}
                        //if (contentLinks.Count > 0)
                        //    colConLink.InsertMany(contentLinks);
                        //sw.Stop();
                        //CommonTools.Log("当前拆分链接[{0}/{1}]\t耗时 - {2}s".FormatStr(nowLinkNum, topLinkNum, sw.Elapsed));
                        //if (nowLinkNum >= topLinkNum)
                        //    break;
                        page++;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                CommonTools.Log(ex.Message);
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 计算微信公众号信息
        /// </summary>
        public static void ComputeWeiXinName()
        {
            try
            {
                string baseUrl = "http://open.gsdata.cn/";
                string nameUrl = baseUrl + "api/wx/wxapi/nickname_one";       //公众号信息接口
                string appid = "JVEvKn7ghegw984neooX";
                string appkey = "n0TWOaX9gta1dpfVF07hpkKr2";
                GSDataSDK api = new GSDataSDK(appid, appkey);           //接口函数

                var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
                var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
                var builderMainLink = Builders<WxLinkMainMongo>.Filter;
                var colMainLink = MongoDBHelper.Instance.GetWXLinkMain();
                var builderName = Builders<WXNameMongo>.Filter;
                var colName = MongoDBHelper.Instance.GetWXName();
                int i = 1;
                foreach (var key in queryKey)
                {
                    CommonTools.Log("当前处理关键词[{0}/{1}] - {2}".FormatStr(i, queryKey.Count, key.Keyword));
                    var filterLink = builderMainLink.Eq(x => x.KeywordId, key._id.ToString());
                    filterLink &= builderMainLink.Eq(x => x.NameId, ObjectId.Empty);
                    var queryLink = colMainLink.Find(filterLink);
                    int topLinkNum = (int)queryLink.Count();
                    int computeNum = 1;
                    if (key.Keyword == "国度")
                    {
                        i++;
                        continue;
                    }
                    //if (i < 2)
                    //{
                    //    i++;
                    //    continue;
                    //}
                    //if (i == 5)
                    //{
                    //    page = 145;
                    //    nowLinkNum = page * pagesize;
                    //    computeNum = nowLinkNum + 1;
                    //}
                    var sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    var tempLinks = queryLink.Project(x => new
                    {
                        Id = x._id,
                        Name = x.Name,
                        NickName = x.Nickname
                    }).ToList();
                    sw.Stop();
                    var linkByName = tempLinks.GroupBy(x => x.Name).ToList();
                    foreach (var x in linkByName)
                    {
                        CommonTools.Log("当前处理公众号[{0}/{1}] - {2}".FormatStr(computeNum, linkByName.Count(), x.Key));
                        //检查本公众号是否已查询过
                        var filterName = builderName.Eq(s => s.Name, x.Key);
                        var queryName = colName.Find(filterName).FirstOrDefault();
                        ObjectId nameObjId;
                        if (queryName == null)
                        {
                            Dictionary<string, object> postData = new Dictionary<string, object>();                 //post参数
                            postData.Add("wx_name", x.Key);
                            string NameStr = api.Call(postData, nameUrl);       //调用接口，获取返回数据
                            int testNum = 0;
                            //如果返回值为空，重试3次
                            while (NameStr == null)
                            {
                                if (testNum == 3)
                                    break;
                                NameStr = api.Call(postData, nameUrl);
                                testNum++;
                            }

                            //解析Json字符串
                            JObject nameJson = new JObject();
                            try
                            {
                                nameJson = JObject.Parse(NameStr);
                            }
                            catch (Exception ex)
                            {
                                CommonTools.Log("微信搜索出错：" + ex.Message);
                                CommonTools.Log("是否继续重试？（Y/N）");
                                string code = Console.ReadLine().ToUpper();
                                if (code != "Y" && code != "N")
                                {
                                    continue;
                                }
                                else
                                {
                                    if (code == "Y")
                                        continue;
                                    else
                                        break;
                                }
                            }
                            if (nameJson.Property("returnCode") != null && nameJson.Property("returnCode").Value.ToString() == "1001")
                            {
                                CommonTools.Log("公众号信息" + nameJson.Property("returnMsg").Value);
                                JObject nameReturn = (JObject)nameJson["returnData"];
                                //获取常用链接信息
                                if (nameReturn == null)
                                {
                                    computeNum++;
                                    continue;
                                }
                                var nameInfo = new WXNameMongo()
                                {
                                    _id = ObjectId.GenerateNewId(),
                                    CreatedAt = DateTime.Now.AddHours(8),
                                };
                                nameObjId = nameInfo._id;
                                if (nameReturn.Property("id") != null)
                                    nameInfo.GsId = Convert.ToInt32(nameReturn.Property("id").Value);
                                if (nameReturn.Property("wx_name") != null)
                                    nameInfo.Name = nameReturn.Property("wx_name").Value.ToString();
                                if (nameReturn.Property("wx_nickname") != null)
                                    nameInfo.Nickname = nameReturn.Property("wx_nickname").Value.ToString();
                                if (nameReturn.Property("wx_type") != null)
                                    nameInfo.Type = nameReturn.Property("wx_type").Value.ToString();
                                if (nameReturn.Property("wx_biz") != null)
                                    nameInfo.Biz = nameReturn.Property("wx_biz").Value.ToString();
                                if (nameReturn.Property("wx_qrcode") != null)
                                    nameInfo.Qrcode = nameReturn.Property("wx_qrcode").Value.ToString();
                                if (nameReturn.Property("wx_note") != null)
                                    nameInfo.Description = nameReturn.Property("wx_note").Value.ToString();
                                if (nameReturn.Property("wx_vip") != null)
                                    nameInfo.Vip = nameReturn.Property("wx_vip").Value.ToString();
                                if (nameReturn.Property("wx_vip_note") != null)
                                    nameInfo.VipNote = nameReturn.Property("wx_vip_note").Value.ToString();
                                if (nameReturn.Property("wx_country") != null)
                                    nameInfo.Country = nameReturn.Property("wx_country").Value.ToString();
                                if (nameReturn.Property("wx_province") != null)
                                    nameInfo.Province = nameReturn.Property("wx_province").Value.ToString();
                                if (nameReturn.Property("wx_city") != null)
                                    nameInfo.City = nameReturn.Property("wx_city").Value.ToString();
                                if (nameReturn.Property("status") != null)
                                    nameInfo.Status = Convert.ToInt32(nameReturn.Property("status").Value);
                                if (nameReturn.Property("isenable") != null)
                                    nameInfo.IsEnable = Convert.ToInt32(nameReturn.Property("isenable").Value);
                                if (nameReturn.Property("category_id") != null)
                                    nameInfo.CategoryId = nameReturn.Property("category_id").Value.ToString();
                                if (nameReturn.Property("update_status") != null)
                                    nameInfo.UpdateStatus = Convert.ToInt32(nameReturn.Property("update_status").Value);
                                if (nameReturn.Property("wx_district") != null)
                                    nameInfo.District = nameReturn.Property("wx_district").Value.ToString();
                                if (nameReturn.Property("overseas") != null)
                                    nameInfo.Overseas = nameReturn.Property("overseas").Value.ToString();
                                if (nameReturn.Property("link_name") != null)
                                    nameInfo.LinkName = nameReturn.Property("link_name").Value.ToString();
                                if (nameReturn.Property("link_unit") != null)
                                    nameInfo.LinkUnit = nameReturn.Property("link_unit").Value.ToString();
                                if (nameReturn.Property("link_position") != null)
                                    nameInfo.LinkPostion = nameReturn.Property("link_position").Value.ToString();
                                if (nameReturn.Property("link_tel") != null)
                                    nameInfo.LinkTel = nameReturn.Property("link_tel").Value.ToString();
                                if (nameReturn.Property("link_wx") != null)
                                    nameInfo.LinkWX = nameReturn.Property("link_wx").Value.ToString();
                                if (nameReturn.Property("link_qq") != null)
                                    nameInfo.LinkQQ = nameReturn.Property("link_qq").Value.ToString();
                                if (nameReturn.Property("link_email") != null)
                                    nameInfo.LinkEmail = nameReturn.Property("link_email").Value.ToString();

                                try
                                {
                                    colName.InsertOne(nameInfo);
                                    filterLink = builderMainLink.Eq(s => s.Name, x.Key);
                                    var update = new UpdateDocument { { "$set", new QueryDocument { { "NameId", nameObjId } } } };
                                    colMainLink.UpdateMany(filterLink, update);
                                    CommonTools.Log("成功计算公众号 - " + nameInfo.Nickname);
                                }
                                catch (Exception ex)
                                {
                                    CommonTools.Log(ex.Message);
                                    Console.ReadLine();
                                }

                            }
                            else if (nameJson.Property("errcode") != null)
                            {
                                CommonTools.Log("错误信息 - " + nameJson.Property("errmsg").ToString());
                                CommonTools.Log("错误代码 - " + nameJson.Property("errmsg").ToString());
                                while (true)
                                {
                                    CommonTools.Log("是否继续？（Y/N）");
                                    string code = Console.ReadLine().ToUpper();
                                    if (code != "Y" && code != "N")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        if (code == "Y")
                                            break;
                                        else
                                            Environment.Exit(0);
                                    }
                                }
                            }
                        }
                        computeNum++;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 计算微信公众号信息
        /// </summary>
        public static void ResetWeiXinSearchRange()
        {
            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var queryKey = colKey.Find(filterKey).ToList();
            DateTime startTime = new DateTime(2016, 11, 1);
            DateTime endTime = new DateTime(2017, 5, 1);
            var update = new UpdateDocument { { "$set", new QueryDocument { { "WXStartTime", startTime.AddHours(8) }, { "WXEndTime", endTime.AddHours(8) } } } };
            colKey.UpdateMany(filterKey, update);

        }

        /// <summary>
        /// 获取微信前200公众号文章评论
        /// </summary>
        public static void GetWXTop200NameComment()
        {
            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            var builderMainLink = Builders<WxLinkMainMongo>.Filter;
            var colMainLink = MongoDBHelper.Instance.GetWXLinkMain();
            try
            {
                int i = 1;
                var nameInfos = new List<NameStatisticDto>();
                foreach (var key in queryKey)
                {
                    CommonTools.Log("当前处理关键词[{0}/{1}] - {2}".FormatStr(i, queryKey.Count, key.Keyword));
                    int page = 0, pagesize = 1000;
                    var filterLink = builderMainLink.Eq(x => x.KeywordId, key._id.ToString());
                    //var filterLink = builderMainLink.In(x => x.KeywordId, queryKey.Select(x => x._id.ToString()).ToList());
                    var queryLink = colMainLink.Find(filterLink);
                    int topLinkNum = (int)queryLink.Count();
                    int nowLinkNum = 0;
                    //if (i < 10)
                    //{
                    //    i++;
                    //    continue;
                    //}
                    //if (i == 5)
                    //{
                    //    page = 145;
                    //    nowLinkNum = page * pagesize;
                    //    computeNum = nowLinkNum + 1;
                    //}
                    while (true)
                    {
                        //获取Top200公众号的信息
                        var sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        var tempLink = colMainLink.Find(filterLink).Skip(page * pagesize).Limit(pagesize).Project(x => new
                        {
                            _id = x._id.ToString(),
                            Url = x.Url,
                            Name = x.Nickname,
                            Keyword = x.Keyword,
                            KeywordId = x.KeywordId,
                            ReadNum = x.ReadNum,
                            LikeNum = x.LikeNum,
                            CommentNum = 0,
                            NameId = x.NameId.ToString()
                        }).ToList();
                        sw.Stop();
                        nowLinkNum += tempLink.Count;
                        CommonTools.Log("当前获取链接[{0}/{1}]\t耗时 - {2}s".FormatStr(nowLinkNum, topLinkNum, sw.Elapsed));

                        var linkByName = tempLink.GroupBy(x => x.Name);        //按公众号分组
                        foreach (var name in linkByName)
                        {
                            var nameInfo = new NameStatisticDto();
                            nameInfo.Name = name.Key;
                            var allLinks = name.ToList();                       //公众号所有文章
                            nameInfo.NameId = allLinks[0].NameId;
                            nameInfo.KeywordNum = allLinks.Select(x => x.Keyword).Distinct().Count();
                            var trueLinks = allLinks.DistinctBy(x => x.Url);
                            nameInfo.LinkNum = trueLinks.Count;
                            nameInfo.LikeNum = trueLinks.Sum(x => x.LikeNum);
                            nameInfo.ReadNum = trueLinks.Sum(x => x.ReadNum);
                            nameInfo.InfluenceNum = nameInfo.ReadNum + nameInfo.LikeNum * 11 + nameInfo.CommentNum * 99;
                            nameInfos.Add(nameInfo);
                        }
                        page++;
                        if (nowLinkNum >= topLinkNum)
                            break;
                    }
                    i++;
                }
                nameInfos = nameInfos.OrderByDescending(x => x.InfluenceNum).ToList();
                var top200Name = nameInfos.Take(200).ToList();

                //获取Top200公众号内的链接信息
                var nameObjIds = top200Name.Select(x => new ObjectId(x.NameId)).ToList();
                var filterMainLink = builderMainLink.In(x => x.NameId, nameObjIds);
                var num = colMainLink.Find(filterMainLink).Count();
                Console.Write(num);
            }
            catch (Exception ex)
            {
                CommonTools.Log(ex.Message);
                Console.ReadLine();
            }
            
        }

        /// <summary>
        /// 测试清博关键词搜索接口
        /// </summary>
        /// <param name="keyword">关键词</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="startNum">搜索起始位置</param>
        public static void TestGSSearch(string keyword, string startTime = "2016-11-01", string endTime = "2017-10-15",int startNum=0)
        {
            //搜索关键词
            string baseUrl = "http://open.gsdata.cn/";
            string linkUrl = baseUrl + "api/wx/opensearchapi/content_keyword_search";       //关键词获取文章接口地址
            //string baseUrl = "http://api.gsdata.cn/";
            //string linkUrl = baseUrl + "weixin/v1/articles";       //关键词获取文章接口地址
            string appid = "JVEvKn7ghegw984neooX";
            string appkey = "n0TWOaX9gta1dpfVF07hpkKr2";
            GSDataSDK api = new GSDataSDK(appid, appkey);           //接口函数
            Dictionary<string, object> postData = new Dictionary<string, object>();                 //post参数
            postData.Add("keyword", keyword);
            postData.Add("sortname", "likenum");
            postData.Add("sort", "desc");
            //postData.Add("start", startNum);
            postData.Add("startdate", startTime);
            postData.Add("enddate", endTime);
            string LinkStr = api.Call(postData, linkUrl);       //调用接口，获取返回数据
            Console.WriteLine(LinkStr);
        }

        /// <summary>
        /// 抓取5118拓展词
        /// </summary>
        /// <param name="keyword">关键词</param>
        public static void Craw5118(string keyword)
        {
            string url = "http://www.5118.com/seo/words/{0}".FormatStr(keyword);
            string html = WebApiInvoke.GetHtml(url,"utf-8");
            //string keyword = "真爱梦想";
            //string url = "http://www.5118.com/seo/words/真爱梦想";
            //string html = WebApiInvoke.GetHtml(url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//span[@class=\"hoverToHide\"]");
            foreach (var node in nodes)
            {
                Console.WriteLine(node.InnerText);
            }
            //string pw = "123456";
            //byte[] bytes = Encoding.UTF8.GetBytes(pw);
            //string base64 = Convert.ToBase64String(bytes);
            //Console.WriteLine(base64);
        }

        /// <summary>
        /// 修复因服务器搬迁导致的图片无法显示
        /// </summary>
        public static void repairImg()
        {
            string sourceServer = "211.154.6.166:9999";
            string nowServer = "43.240.138.233:9999";
            //更新用户头像
            var builerUser = Builders<IW2SUser>.Filter;
            var filterUser = builerUser.Empty;
            var colUser = MongoDBHelper.Instance.Get_IW2SUser();
            var queryUser = colUser.Find(filterUser).ToList();
            foreach (var user in queryUser)
            {
                if (!string.IsNullOrEmpty(user.PictureSrc) && user.PictureSrc.Contains(sourceServer))
                {
                    string newImg = user.PictureSrc.Replace(sourceServer, nowServer);
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "PictureSrc", newImg } } } };
                    filterUser = builerUser.Eq(x => x._id, user._id);
                    colUser.UpdateOne(filterUser, update);
                    Console.WriteLine(newImg);
                }
            }
            //更新简报图片
            var builerReport = Builders<Dnl_Report>.Filter;
            var filterReport = builerReport.Empty;
            var colReport = MongoDBHelper.Instance.GetDnl_Report();
            var queryReport = colReport.Find(filterReport).ToList();
            foreach (var Report in queryReport)
            {
                if (!string.IsNullOrEmpty(Report.IconUrl) && Report.IconUrl.Contains(sourceServer))
                {
                    string newImg = Report.IconUrl.Replace(sourceServer, nowServer);
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "IconUrl", newImg } } } };
                    filterReport = builerReport.Eq(x => x._id, Report._id);
                    colReport.UpdateOne(filterReport, update);
                    Console.WriteLine(newImg);
                }
            }

            //更新命名实体图片
            var builerEntityTree = Builders<Dnl_EntityTree>.Filter;
            var filterEntityTree = builerEntityTree.Empty;
            var colEntityTree = MongoDBHelper.Instance.GetDnl_EntityTree();
            var queryEntityTree = colEntityTree.Find(filterEntityTree).ToList();
            foreach (var EntityTree in queryEntityTree)
            {
                if (!string.IsNullOrEmpty(EntityTree.PicUrl) && EntityTree.PicUrl.Contains(sourceServer))
                {
                    string newImg = EntityTree.PicUrl.Replace(sourceServer, nowServer);
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "PicUrl", newImg } } } };
                    filterEntityTree = builerEntityTree.Eq(x => x._id, EntityTree._id);
                    colEntityTree.UpdateOne(filterEntityTree, update);
                    Console.WriteLine(newImg);
                }
            }
            Console.WriteLine("更新完毕！");
        }

        /// <summary>
        /// 根据邮箱删除用户
        /// </summary>
        /// <param name="userEmailList">邮箱列表</param>
        public static void DelUser(List<string> userEmailList)
        {
            var builderUser = Builders<IW2SUser>.Filter;
            var filterUser = builderUser.In(x => x.UsrEmail, userEmailList);
            var colUser = MongoDBHelper.Instance.Get_IW2SUser();
            var queryUser = colUser.Find(filterUser).ToList();
            var userObjIdList = queryUser.Select(x => x._id).ToList();

            var buiderPro = Builders<IW2S_Project>.Filter;
            var filterPro = buiderPro.In(x => x.UsrId, userObjIdList);
            var colPro = MongoDBHelper.Instance.GetIW2S_Projects();
            var queryPro = colPro.Find(filterPro).ToList();
            var proObjIdList = queryPro.Select(x => x._id).ToList();
            colPro.DeleteMany(filterPro);

            //删除项目组及映射
            var filterProGr = Builders<Dnl_ProjectCategory>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_ProjectCategory().DeleteMany(filterProGr);
            var fiterProGrMap = Builders<Dnl_ProjectGroup>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_ProjectGroup().DeleteMany(fiterProGrMap);

            //删除项目折线数据
            var filterProLink = Builders<IW2S_ProLinksCount>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetIW2S_ProLinksCount().DeleteMany(filterProLink);

            //删除项目组分享
            var filterShare = Builders<IW2S_ProjectShare>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetIW2S_ProjectShare().DeleteMany(filterShare);

            //删除百度分组及关键词映射
            var filterBaiduCate = Builders<Dnl_KeywordCategory>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetDnl_KeywordCategory().DeleteMany(filterBaiduCate);
            var filterBaiduMap = Builders<Dnl_KeywordMapping>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetDnl_KeywordMapping().DeleteMany(filterBaiduMap);
            var filterBaiduCoPresent = Builders<Dnl_MappingCoPresent>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetDnl_MappingCoPresent().DeleteMany(filterBaiduCoPresent);

            //删除微信分组及关键词映射
            var filterWxCate = Builders<MediaKeywordCategoryMongo>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetMediaKeywordCategory().DeleteMany(filterWxCate);
            var filterWxMap = Builders<MediaKeywordMappingMongo>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetMediaKeywordMapping().DeleteMany(filterWxMap);
            var filterWxCoPresent = Builders<MediaMappingCoPresentMongo>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetMediaMappingCoPresent().DeleteMany(filterWxCoPresent);

            //删除链接映射
            var filterLinkMapping = Builders<Dnl_LinkMapping_Baidu>.Filter.In(x => x.ProjectId, proObjIdList);
            MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().DeleteMany(filterLinkMapping);

            //删除命名实体
            var filterEntity = Builders<Dnl_EntityTree>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_EntityTree().DeleteMany(filterEntity);
            var fiterEnMap = Builders<Dnl_EntityTreeMapping>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_EntityTreeMapping().DeleteMany(fiterEnMap);

            
            //获取简报ID
            var filterReprot = Builders<Dnl_Report>.Filter.In(x => x.UsrId, userObjIdList);
            //var queryReport = MongoDBHelper.Instance.GetDnl_Report().Find(filterReprot).ToList();
            //var reObjIdList = queryReport.Select(x => x._id).ToList();
            MongoDBHelper.Instance.GetDnl_Report().DeleteMany(filterReprot);

            //删除简报及所有相关数据
            var filterReDesc = Builders<Dnl_Report_Description>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_Description().DeleteMany(filterReDesc);
            var filterReDomainChart = Builders<Dnl_Report_DomainChart>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_DomainChart().DeleteMany(filterReDomainChart);
            var filterReKeyword = Builders<Dnl_Report_Keyword>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_Keyword().DeleteMany(filterReKeyword);
            var filterReKeywordCate = Builders<Dnl_Report_KeywordCategory>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_KeywordCategory().DeleteMany(filterReKeywordCate);
            var filterReKeywordChart = Builders<Dnl_Report_keywordChart>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_keywordChart().DeleteMany(filterReKeywordChart);
            var filterReLinkChart = Builders<Dnl_Report_LinkChart>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_LinkChart().DeleteMany(filterReLinkChart);
            var filterReLinkChartCate = Builders<Dnl_Report_LinkChartCategory>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_LinkChartCategory().DeleteMany(filterReLinkChartCate);
            var filterReStatistics = Builders<Dnl_Report_Statistics>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_Statistics().DeleteMany(filterReStatistics);
            var filterReTimeLink = Builders<Dnl_Report_TimeLink>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_TimeLink().DeleteMany(filterReTimeLink);
            var filterReWordTree = Builders<Dnl_Report_WordTree>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_Report_WordTree().DeleteMany(filterReWordTree);
            var filterReShare = Builders<Dnl_ReportShare>.Filter.In(x => x.UsrId, userObjIdList);
            MongoDBHelper.Instance.GetDnl_ReportShare().DeleteMany(filterReShare);


            colUser.DeleteMany(filterUser);
            Console.WriteLine("删除完毕！");





        }

        /// <summary>
        /// 导出未分组域名
        /// </summary>
        public static void ExportUngroupDomain()
        {
            var proObjId = new ObjectId("598a7a4af4b87d07ccbc8d17");
            string filename = "未分组网址.xls";
            string path = @"F:\" + filename;

            HSSFWorkbook linkExcel = new HSSFWorkbook();     //Excel表格
            ISheet linkSheet = linkExcel.CreateSheet();
            IRow RowHead0 = linkSheet.CreateRow(0);
            RowHead0.CreateCell(0).SetCellValue("域名");
            RowHead0.CreateCell(1).SetCellValue("关键词数");
            RowHead0.CreateCell(2).SetCellValue("链接数");

            var buiderMap = Builders<Dnl_KeywordMapping>.Filter;
            var filterMap = buiderMap.Eq(x => x.ProjectId, proObjId) & buiderMap.Eq(x => x.CategoryId, ObjectId.Empty) & buiderMap.Eq(x => x.IsDel, false);
            var colMap = MongoDBHelper.Instance.GetDnl_KeywordMapping();
            var keyIds = colMap.Find(filterMap).Project(x => x.KeywordId.ToString()).ToList();
            keyIds = keyIds.Distinct().ToList();
            CommonTools.Log("获取关键词 - {0}".FormatStr(keyIds.Count));

            var builderLink = Builders<Dnl_Link_Baidu>.Filter;
            var filterLink = builderLink.In(x => x.SearchkeywordId, keyIds);
            var colLink = MongoDBHelper.Instance.GetDnl_Link_Baidu();
            var queryLink = colLink.Find(filterLink).Project(x => new
            {
                Id=x._id.ToString(),
                Domain = x.Domain,
                Keyword = x.Keywords,
                PublishTime = x.PublishTime,
                LinkUrl=x.LinkUrl
            }).ToList();
            var links = new List<Timelevel1linkDto>();
            int years = int.MaxValue;
            int yeare = 0;
            foreach (var item in queryLink)
            {
                DateTime tpTime = new DateTime();
                DateTime.TryParse(item.PublishTime, out tpTime);
                if (tpTime != DateTime.MinValue)
                {
                    if (tpTime.Year > yeare)
                        yeare = tpTime.Year;
                    if (tpTime.Year < years)
                        years = tpTime.Year;
                }
                var link = new Timelevel1linkDto
                {
                    Id = item.Id,
                    Domain = item.Domain,
                    Keywords = item.Keyword,
                    PublishTime = tpTime,
                    LinkUrl=item.LinkUrl
                };
                links.Add(link);
            }

            //创建年份时间段
            int i = 3;
            Dictionary<int, int> yearToIndex = new Dictionary<int, int>();
            int cpyear = years;
            while (cpyear <= yeare)
            {
                RowHead0.CreateCell(i).SetCellValue(cpyear);
                yearToIndex.Add(cpyear, i);
                cpyear++;
                i++;
            }

            //获取已分组域名
            var domainCatBuilder = Builders<IW2S_DomainCategoryData>.Filter;
            var domainCatFilter = domainCatBuilder.Eq(x => x.UsrId, ObjectId.Empty);
            var domainCategoryDatas = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas().Find(domainCatFilter).Project(x => x.DomainName).ToList().Distinct().ToList();
            CommonTools.Log("获取已分组域名 - {0}".FormatStr(domainCategoryDatas.Count));

            //links.RemoveAll(x => domainCategoryDatas.Contains(x.Domain));
            var domains = links.Select(x => x.Domain).ToList();
            domains = domains.Distinct().ToList();
            CommonTools.Log("获取域名 - {0}".FormatStr(domains.Count));

            var linkByDomain = links.GroupBy(x => x.Domain).ToList();

            int j = 1;
            foreach (var x in linkByDomain)
            {
                var tpLinks = x.ToList();
                IRow row = linkSheet.CreateRow(j);
                row.CreateCell(0).SetCellValue(x.Key);
                int keyNum = tpLinks.Select(s => s.Keywords).Distinct().Count();
                int linkNum = tpLinks.Select(s => s.LinkUrl).Distinct().Count();
                row.CreateCell(1).SetCellValue(keyNum);
                row.CreateCell(2).SetCellValue(linkNum);

                var yearList = tpLinks.DistinctBy(s => s.LinkUrl).Select(s => s.PublishTime.Year).ToList();
                i = 3;
                cpyear = years;
                while (cpyear <= yeare)
                {
                    int num = yearList.Count(s => s == cpyear);
                    if (num != 0)
                        row.CreateCell(i).SetCellValue(num);
                    cpyear++;
                    i++;
                }
                j++;
            }

            using (FileStream fileAna = new FileStream(path, FileMode.Create))
            {
                linkExcel.Write(fileAna);　　//创建Excel文件。
            }
            CommonTools.Log("导出完毕！");
        }

        /// <summary>
        /// 导入域名分组
        /// </summary>
        /// <param name="path">分组文件路径</param>
        /// <param name="userObjId">用户ID</param>
        public static void ImportDomain(string path, ObjectId userObjId)
        {
            Dictionary<string, string> domainToCateName = new Dictionary<string, string>(); //域名和分组名词典
            Dictionary<string, ObjectId> CateNameToObjId = new Dictionary<string, ObjectId>(); //分组名和ID词典
            List<string> domains = new List<string>();      //域名列表
            List<string> cateNames = new List<string>();      //域名列表

            //读取Excel文件
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                HSSFWorkbook workbook = new HSSFWorkbook(stream);
                HSSFSheet sheet = workbook.GetSheetAt(0) as HSSFSheet;
                
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    //获取域名及其归属分组
                    HSSFRow row = sheet.GetRow(i) as HSSFRow;
                    if (row.GetCell(0) == null || string.IsNullOrEmpty(row.GetCell(0).StringCellValue))
                    {
                        continue;
                    }
                    string cateName = row.GetCell(0).StringCellValue;
                    string domain = row.GetCell(1).StringCellValue;
                    if (!domainToCateName.ContainsKey(domain))
                    {
                        domainToCateName.Add(domain, cateName);
                        domains.Add(domain);
                        cateNames.Add(cateName);
                    }
                }
            }
            cateNames = cateNames.Distinct().ToList();

            //查询词组是否已存在
            var builderCate = Builders<IW2S_DomainCategory>.Filter;
            var colCate = MongoDBHelper.Instance.GetIW2S_DomainCategorys();
            foreach (var cateName in cateNames)
            {
                var filterCate = builderCate.Eq(x => x.UsrId, userObjId);
                filterCate &= builderCate.Eq(x => x.Name, cateName);
                var queryCate = colCate.Find(filterCate).FirstOrDefault();
                if (queryCate != null)
                {
                    CateNameToObjId.Add(cateName, queryCate._id);
                }
                else
                {
                    //不存在则创建新分组
                    var cate = new IW2S_DomainCategory
                    {
                        _id = ObjectId.GenerateNewId(),
                        Name = cateName,
                        UsrId = userObjId
                    };
                    colCate.InsertOne(cate);
                    CateNameToObjId.Add(cateName, cate._id);
                }
            }
            CommonTools.Log("分组已添加完毕！");

            var builderDomain = Builders<IW2S_DomainCategoryData>.Filter;
            var colDomain = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas();
            int j = 0;
            foreach (var domain in domains)
            {
                j++;
                string cateName = domainToCateName[domain];
                //查询本域名是否已被分组
                var filterDomain = builderDomain.Eq(x => x.UsrId, userObjId);
                filterDomain &= builderDomain.Eq(x => x.DomainName, domain);
                var queryDomain = colDomain.Find(filterDomain).FirstOrDefault();
                if (queryDomain != null)
                    continue;
                else
                {
                    //添加新的域名
                    var newDomain = new IW2S_DomainCategoryData
                    {
                        UsrId = userObjId,
                        DomainName = domain,
                        DomainCategoryId = CateNameToObjId[cateName]
                    };
                    colDomain.InsertOne(newDomain);
                }
                CommonTools.Log("已添加域名[{0}/{1}] - {2}；\t分组 - {3}".FormatStr(j, domains.Count, domain, cateName));
            }
            CommonTools.Log("导入完成");
        }

        /// <summary>
        /// Html表格转Excel
        /// </summary>
        /// <param name="tableHtml">Html表格代码</param>
        public static void ExportExcel(string tableHtml)
        {
            //创建数据表
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            int colIndex = 0;   //当前列位置
            int rowIndex = 0;   //当前行位置

            //解析table数据
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(tableHtml);
            //解析标题栏
            HtmlNodeCollection heads = doc.DocumentNode.SelectNodes("/thead/tr");
            if (heads != null)
            {
                foreach (var x in heads)
                {
                    HtmlNode head = HtmlNode.CreateNode(x.OuterHtml);
                    //获取本行内所有单元格
                    var cells = head.Descendants().Where(s => s.Name == "td" || s.Name == "th").ToList();
                    if (cells.Count() == 0)
                        continue;
                    IRow row = sheet.CreateRow(rowIndex);
                    //遍历所有单元格，为其赋值
                    foreach (var cell in cells)
                    {
                        //判断本单元格是否为合并单元格
                        int colspan = Convert.ToInt32(cell.GetAttributeValue("colspan", 1));
                        int rowspan = Convert.ToInt32(cell.GetAttributeValue("rowspan", 1));
                        if (colspan > 1 || rowspan > 1)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + rowspan - 1, colIndex, colIndex + colspan - 1));
                        }
                        row.CreateCell(colIndex).SetCellValue(cell.InnerText);
                        colIndex += colspan;
                    }
                    rowIndex++;
                    colIndex = 0;
                }
            }

            //解析表格数据
            HtmlNodeCollection bodys = doc.DocumentNode.SelectNodes("/tbody/tr");
            if (bodys != null)
            {
                foreach (var x in bodys)
                {
                    HtmlNode body = HtmlNode.CreateNode(x.OuterHtml);
                    //获取本行内所有单元格
                    var cells = body.Descendants().Where(s => s.Name == "td" || s.Name == "th").ToList();
                    if (cells.Count() == 0)
                        continue;
                    IRow row = sheet.CreateRow(rowIndex);
                    //遍历所有单元格，为其赋值
                    foreach (var cell in cells)
                    {
                        //判断本单元格是否为合并单元格
                        int colspan = Convert.ToInt32(cell.GetAttributeValue("colspan", 1));
                        int rowspan = Convert.ToInt32(cell.GetAttributeValue("rowspan", 1));
                        if (colspan > 1 || rowspan > 1)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + rowspan - 1, colIndex, colIndex + colspan - 1));
                        }
                        row.CreateCell(colIndex).SetCellValue(cell.InnerText);
                        colIndex += colspan;
                    }
                    rowIndex++;
                    colIndex = 0;
                }
            }

            string path = "E:\\test.xls";
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                workbook.Write(file);　　//创建Excel文件。
            }
            CommonTools.Log("导出完毕！");
            
        }

        /// <summary>
        /// 删除无效链接
        /// </summary>
        public static void DelUnuseLink()
        {
            string errStr = "robots.txt";       //异常域名特征
            //var sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //int computeNum = 0;
            //int errorNum = 0;       //连续异常数
            //int startPos, length;
            //Console.Write("输入起始位置：");
            //startPos = Convert.ToInt32(Console.ReadLine());
            //Console.Write("输入计算数量：");
            //length = Convert.ToInt32(Console.ReadLine());
            //获取现在所有搜索完成的关键词
            var builderKey = Builders<Dnl_Keyword>.Filter;
            var filterKey = builderKey.Eq(x => x.BotStatus_Baidu, 2);
            var colKey = MongoDBHelper.Instance.GetDnl_Keyword();
            var keys = colKey.Find(filterKey).ToList();
            keys = keys.OrderBy(x => x.CreatedAt).ToList();
            //int top = startPos + length;
            for (int i = 0; i <= keys.Count; i++)
            {
                //if (i == keys.Count)
                //    break;

                //if (computeNum == 5000)
                //{
                //    CommonTools.Log("计算量已接近阈值，暂停30分钟！");
                //    Thread.Sleep(30 * 60 * 1000);
                //}

                CommonTools.Log("当前关键词[" + (i + 1) + "/" + keys.Count + "] - " + keys[i].Keyword);
                try
                {
                    //获取关键词对应链接
                    var builderLink = Builders<Dnl_Link_Baidu>.Filter;
                    var filterLink = builderLink.Eq(x => x.SearchkeywordId, keys[i]._id.ToString());
                    var colLink = MongoDBHelper.Instance.GetDnl_Link_Baidu();
                    var links = colLink.Find(filterLink).Project(x => new
                    {
                        Id = x._id,
                        Domain = x.Domain,
                    }).ToList();
                    int delNum = 0;
                    List<ObjectId> delLinkIds = new List<ObjectId>();
                    int j = 1;
                    //对比域名，筛选错误链接
                    foreach (var x in links)
                    {
                        if (x.Domain.Contains(errStr))
                        {
                            delLinkIds.Add(x.Id);
                            delNum++;
                        }
                        j++;
                    }

                    if (delLinkIds.Count > 0)
                    {
                        filterLink = builderLink.In(x => x._id, delLinkIds);
                        colLink.DeleteMany(filterLink);

                        int newLinkNum = keys[i].LinkCount_Baidu - delNum;
                        var updateKey = new UpdateDocument { { "$set", new QueryDocument { { "LinkCount_Baidu", newLinkNum } } } };
                        var filterUpKey = builderKey.Eq(x => x._id, keys[i]._id);
                        colKey.UpdateOne(filterUpKey, updateKey);
                    }

                    CommonTools.Log("本关键词错误链接数 - " + delNum);
                    Console.WriteLine("\n");
                }
                catch (Exception ex)
                {
                    CommonTools.Log(ex.Message);
                }
            }

            CommonTools.Log("全部迁移完毕！");
        }

        /// <summary>
        /// 输出标签数据
        /// </summary>
        /// <param name="keys"></param>
        public static void ExportData(List<string> keys)
        {
            //获取所有链接数据
            var builderKey = Builders<Dnl_Keyword>.Filter;
            var filterKey = builderKey.In(x => x.Keyword, keys);
            var colKey = MongoDBHelper.Instance.GetDnl_Keyword();
            var queryKey = colKey.Find(filterKey).ToList();
            var keyIds = queryKey.Select(x => x._id.ToString()).ToList();

            var builderLink = Builders<Dnl_Link_Baidu>.Filter;
            var filterLink = builderLink.In(x => x.SearchkeywordId, keyIds);
            var colLink = MongoDBHelper.Instance.GetDnl_Link_Baidu();
            var queryLink = colLink.Find(filterLink).Project(x => new
            {
                Id = x._id,
                Keyword = x.Keywords,
                KeywordId = x.SearchkeywordId,
                Url = x.LinkUrl,
                Title = x.Title,
                Domain = x.Domain,
                Desc = x.Description,
                PublishTime=x.PublishTime
            }).ToList();

            //筛选无效时间
            var links = new List<BaiduLink>();
            foreach (var x in queryLink)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(x.PublishTime, out dt))
                {
                    if (dt < new DateTime(1978, 1, 1) || dt > DateTime.Now)
                        continue;
                    var link = new BaiduLink
                    {
                        Id = x.Id.ToString(),
                        Keyword = x.Keyword,
                        KeywordId = x.KeywordId,
                        Url = x.Url,
                        Title = x.Title,
                        Domain = x.Domain,
                        Description = x.Desc,
                        PublishTime = dt
                    };
                    links.Add(link);
                }
            }

            //输出逐年时间分布表
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet timeSheet = workbook.CreateSheet("逐年分布");
            int i = 0, j = 0;
            var years = links.Select(x => x.PublishTime.Year).ToList();
            var startDt = years.Min();
            var endDt = years.Max();
            IRow headTime = timeSheet.CreateRow(i);
            while (startDt <= endDt)
            {
                headTime.CreateCell(j).SetCellValue(startDt);
                j++;
                startDt++;
            }
            i++;
            j = 0;
            startDt = years.Min();
            IRow rowTime = timeSheet.CreateRow(i);
            while (startDt <= endDt)
            {
                int num = years.Count(x => x == startDt);
                rowTime.CreateCell(j).SetCellValue(num);
                j++;
                startDt++;
            }

            //输出域名分组出现表
            ISheet domainSheet = workbook.CreateSheet("域名分组出现表");

            //获取域名分组
            var userObjId = ObjectId.Empty;
            var builderDoCate = Builders<IW2S_DomainCategory>.Filter;
            var filterDoCate = builderDoCate.Eq(x => x.UsrId, userObjId);
            var colDoCate = MongoDBHelper.Instance.GetIW2S_DomainCategorys();
            var queryDoCate = colDoCate.Find(filterDoCate).ToList();
            //如果用户无分组，使用公用分组
            if (queryDoCate.Count == 0)
            {
                userObjId = ObjectId.Empty;
                filterDoCate = builderDoCate.Eq(x => x.UsrId, userObjId);
            }
            queryDoCate = colDoCate.Find(filterDoCate).ToList();
            i = 0;
            j = 0;
            IRow headDomain = domainSheet.CreateRow(i);
            foreach (var x in queryDoCate)
            {
                headDomain.CreateCell(j).SetCellValue(x.Name);
                j++;
            }
            headDomain.CreateCell(j).SetCellValue("未分组");
            i++;
            j = 0;

            var linkNums = new int[queryDoCate.Count + 1];

            //获取域名分组数据
            var buiderDo = Builders<IW2S_DomainCategoryData>.Filter;
            var filterDo = buiderDo.Eq(x => x.UsrId, userObjId);
            var colDo = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas();
            var queryDo = colDo.Find(filterDo).ToList();
            //生成域名及其域名类别索引辞典
            Dictionary<string, string> domainToCate = new Dictionary<string, string>();
            foreach (var domain in queryDo)
            {
                if (!domainToCate.ContainsKey(domain.DomainName))
                {
                    var index = queryDoCate.FindIndex(x => x._id == domain.DomainCategoryId);
                    if (index != -1)
                    {
                        domainToCate.Add(domain.DomainName, queryDoCate[index].Name);
                    }
                }
            }
            foreach (var x in links)
            {
                if (domainToCate.ContainsKey(x.Domain))
                {
                    string cateName= domainToCate[x.Domain];
                    int index = queryDoCate.FindIndex(s => s.Name == cateName);
                    linkNums[index]++;
                }
                else
                {
                    linkNums[queryDoCate.Count]++;
                }
            }
            IRow rowDomain = domainSheet.CreateRow(i);
            for (int k = 0; k < linkNums.Length; k++)
            {
                rowDomain.CreateCell(k).SetCellValue(linkNums[k]);
                
            }
            i = 0; j = 0;
            
            //输出链接详情表
            ISheet linkSheetAll = workbook.CreateSheet("链接详情表（全部）");
            IRow headLink = linkSheetAll.CreateRow(i);
            headLink.CreateCell(0).SetCellValue("ID");
            headLink.CreateCell(1).SetCellValue("域名");
            headLink.CreateCell(2).SetCellValue("域名分组");
            headLink.CreateCell(3).SetCellValue("标题");
            headLink.CreateCell(4).SetCellValue("链接");

            ISheet linkSheetPart = workbook.CreateSheet("链接详情表（部分）");
            IRow headLink2 = linkSheetPart.CreateRow(i);
            headLink2.CreateCell(0).SetCellValue("ID");
            headLink2.CreateCell(1).SetCellValue("域名");
            headLink2.CreateCell(2).SetCellValue("域名分组");
            headLink2.CreateCell(3).SetCellValue("标题");
            headLink2.CreateCell(4).SetCellValue("链接");
            i++;
            j = 1;
            foreach (var x in links)
            {
                var row1 = linkSheetAll.CreateRow(i);
                row1.CreateCell(0).SetCellValue(x.Id);
                row1.CreateCell(1).SetCellValue(x.Domain);
                row1.CreateCell(3).SetCellValue(x.Title);
                row1.CreateCell(4).SetCellValue(x.Url);
                if(domainToCate.ContainsKey(x.Domain))
                {
                    string cate = domainToCate[x.Domain];
                    row1.CreateCell(2).SetCellValue(cate);

                    //判断是否为部分表内数据
                    Regex reg = new Regex("门户|新闻|财经|公益|资讯");
                    if (reg.IsMatch(cate))
                    {
                        var row2 = linkSheetPart.CreateRow(j);
                        row2.CreateCell(0).SetCellValue(x.Id);
                        row2.CreateCell(1).SetCellValue(x.Domain);
                        row2.CreateCell(2).SetCellValue(cate);
                        row2.CreateCell(3).SetCellValue(x.Title);
                        row2.CreateCell(4).SetCellValue(x.Url);
                        j++;
                    }
                }
                else
                    row1.CreateCell(2).SetCellValue("未分组");
                i++;
            }

            using (FileStream fs = new FileStream("F:\\Test.xls", FileMode.Create))
            {
                workbook.Write(fs);
            }
            Console.WriteLine("计算完毕");
        }

        /// <summary>
        /// 正文抽取
        /// </summary>
        /// <param name="url">网址</param>
        public static void ExtractContent(string url)
        {
            var transcoder = new NReadabilityTranscoder();
            string html;
            using (var wc = new WebClient())
            {
                html = wc.DownloadString(url);
            }
            TranscodingInput input = new TranscodingInput(html);
            TranscodingResult result = transcoder.Transcode(input);
            string content = result.ExtractedContent;
            string str = content.SubAfter("</head>").SubBefore("</html>");
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            str = doc.DocumentNode.InnerText;
            Console.WriteLine(str);
        }

        /// <summary>
        /// 提取词频
        /// </summary>
        /// <param name="projectIds"></param>
        public static void ExportWordFreqency(string projectIds)
        {
            
            var proObjIds = CommonTools.GetObjIdListFromStr(projectIds);

            //获取项目名称
            var queryPro = MongoDBHelper.Instance.GetIW2S_Projects().Find(Builders<IW2S_Project>.Filter.In(x => x._id, proObjIds)).Project(x => new
            {
                Id = x._id,
                Name = x.Name
            }).ToList();

            JiebaHelper jieba = new JiebaHelper();
            var transcoder = new NReadabilityTranscoder();

            int index = 1;
            foreach (var pro in queryPro)
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                CommonTools.Log("计算项目 - {0}".FormatStr(pro.Name));
                if (index < 5)
                {
                    index++;
                    continue;
                }
                index++;
                //获取关键词信息
                var builderMap = Builders<Dnl_KeywordMapping>.Filter;
                var filterMap = builderMap.Eq(x => x.ProjectId, pro.Id);
                filterMap &= builderMap.Eq(x => x.CategoryId, ObjectId.Empty) & builderMap.Eq(x => x.IsDel, false);
                var queryMap = MongoDBHelper.Instance.GetDnl_KeywordMapping().Find(filterMap).Project(x => new
                {
                    KeywordId = x.KeywordId.ToString(),
                    ProjectId = x.ProjectId
                }).ToList();

                var dic = queryMap.ToLookup(x => x.KeywordId);
                var keyIds = queryMap.Select(x => x.KeywordId).Distinct().ToList();
                CommonTools.Log("获取百度关键词数 - {0}个".FormatStr(keyIds.Count));
                //获取链接信息
                ////获取项目内已删除的链接Id
                //var builderLinkMap = Builders<Dnl_LinkMapping_Baidu>.Filter;
                //var filterLinkMap = builderLinkMap.Eq(x => x.ProjectId, pro.Id) & builderLinkMap.Eq(x => x.DataCleanStatus, (byte)2);
                //filterLinkMap &= builderLinkMap.Eq(x => x.Source, SourceType.Baidu);
                //var exLinkObjIds = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).Project(x => x.LinkId).ToList();       //项目中已删除的链接ID列表

                //以100个关键词为单位，循环获取所有链接信息
                var buiderLink = Builders<Dnl_Link_Baidu>.Filter;
                var colLink = MongoDBHelper.Instance.GetDnl_Link_Baidu();
                var linkList = new List<TagLink>();
                int page = 0, pagesize = 100;
                while (true)
                {
                    var tempKeys = keyIds.Skip(page * pagesize).Take(pagesize).ToList();
                    if (tempKeys.Count > 0)
                    {
                        //以500为单位，循环获取本批次链接信息
                        var filterLink = buiderLink.In(x => x.SearchkeywordId, tempKeys);
                        int pageLink = 0, pagesizeLink = 500;
                        var allLinkNum = colLink.Find(filterLink).Count();      //本批次关键词对应链接总数
                        int getNum = 0;     //本批关键词已获取链接数
                        while (true)
                        {
                            var tempLink = colLink.Find(filterLink).Skip(pageLink * pagesizeLink).Limit(pagesizeLink).Project(x => new TagLink
                            {
                                Id = x._id,
                                Title = x.Title,
                                Html = x.Html,
                                KeywordId = x.SearchkeywordId,
                                Description = x.Description
                            }).ToList();
                            if (tempLink.Count > 0)
                            {
                                linkList.AddRange(tempLink);
                                getNum += tempLink.Count;
                            }
                            else
                                break;
                            pageLink++;
                            CommonTools.Log("获取当前百度链接[{0}/{1}]".FormatStr(getNum, allLinkNum));
                        }
                        page++;
                        CommonTools.Log("获取百度[{0}/{1}]关键词对应链接".FormatStr(page * pagesize < keyIds.Count ? page * pagesize : keyIds.Count, keyIds.Count));
                    }
                    else
                    {
                        break;
                    }
                }

                //合并标题、描述和正文，然后获取名词词频
                string str = "";
                if (linkList.Count > 0)
                {
                    double num = 1;
                    foreach (var link in linkList)
                    {
                        try
                        {
                            TranscodingInput input = new TranscodingInput(link.Html);
                            TranscodingResult result = transcoder.Transcode(input);
                            if (result.ContentExtracted)
                            {
                                string htmlCon = result.ExtractedContent.SubAfter("</head>").SubBefore("</html>");
                                HtmlDocument doc = new HtmlDocument();
                                str += doc.DocumentNode.InnerText + System.Environment.NewLine;
                            }
                            else
                            {
                                str += link.Title + System.Environment.NewLine;
                                str += link.Description + System.Environment.NewLine;
                            }
                        }
                        catch(Exception ex)
                        {
                            CommonTools.Log("错误：{0}".FormatStr(ex.Message));
                            str += link.Title + System.Environment.NewLine;
                            str += link.Description + System.Environment.NewLine;
                        }
                        CommonTools.Log("[{0}/{1}] - {2}".FormatStr(num, linkList.Count, link.Title));
                        num++;

                    }
                    var frequecny = jieba.GetFrequency(str, new List<string>());
                    var sheet = workbook.CreateSheet(pro.Name);
                    var head = sheet.CreateRow(0);
                    head.CreateCell(0).SetCellValue("词名");
                    head.CreateCell(1).SetCellValue("词频");
                    int j = 1;
                    for (int i = 0; i < frequecny.noun.Count || i < 100; i++)
                    {
                        var row = sheet.CreateRow(j);
                        row.CreateCell(0).SetCellValue(frequecny.noun[i]);
                        row.CreateCell(1).SetCellValue(frequecny.nounCount[i]);
                        j++;
                    }
                }
                string filePath = "F:\\{0}.xls".FormatStr(pro.Name);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    workbook.Write(fs);
                }
            }
            CommonTools.Log("词频统计完毕！");

        }

        /// <summary>
        /// 获取链接关系图谱
        /// </summary>
        /// <param name="prjId">项目Id</param>
        /// <param name="timeInterval">时间间隔，0为全部，1为月，2为季，3为年</param>
        /// <returns></returns>
        public static TimeLinkRefer GetLinkReference(string prjId, int timeInterval)
        {
            var result = new TimeLinkRefer();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            if (string.IsNullOrEmpty(prjId))
            {
                return null;
            }

            //获取用户ID
            var userObjId = MongoDBHelper.Instance.GetIW2S_Projects().Find(new QueryDocument { { "_id", new ObjectId(prjId) } }).Project(x => x.UsrId).FirstOrDefault();

            List<ObjectId> cateIds = new List<ObjectId>();
            //获取第1级关键词组
            var builderCate = Builders<Dnl_KeywordCategory>.Filter;
            var filterCate = builderCate.Eq(x => x.ProjectId, new ObjectId(prjId));
            filterCate &= builderCate.Eq(x => x.IsDel, false) & builderCate.Eq(x => x.ParentId, ObjectId.Empty);
            var queryCate = MongoDBHelper.Instance.GetDnl_KeywordCategory().Find(filterCate).Project(x => new
            {
                Id = x._id,
                Name = x.Name
            }).ToList();
            var cateObjIdList = queryCate.Select(x => x.Id).ToList();      //词组ObjectId列表
            //建立分组信息
            var cateInfo = new List<LinkReferCate>();
            foreach (var x in queryCate)
            {
                var cate = new LinkReferCate();
                //判断是否需要裁剪分组名
                if (x.Name.Length <= 15)
                {
                    cate.name = x.Name;
                    cate.baseName = x.Name;
                }
                else
                {
                    cate.name = x.Name.Substring(0, 14) + "…";
                    cate.baseName = x.Name.Substring(0, 14) + "…";
                }
                cate.id = x.Id.ToString();
                cateInfo.Add(cate);
            }

            //获取组内关键词
            var builderMap = Builders<Dnl_KeywordMapping>.Filter;
            var filterMap = builderMap.In(x => x.CategoryId, cateObjIdList) & builderMap.Eq(x => x.IsDel, false);
            var queryKey = MongoDBHelper.Instance.GetDnl_KeywordMapping().Find(filterMap).Project(x => new
            {
                KeywordId = x.KeywordId.ToString(),
                CategoryId = x.CategoryId,
                Keyword = x.Keyword,
            }).ToList();
            var keyIdList = queryKey.Select(x => x.KeywordId).ToList();    //关键词ObjectId列表

            //建议关键词与词组字典
            var keyToCate = new Dictionary<string, string>();
            //var keyGroup = queryKey.GroupBy(x => x.KeywordId).ToList();
            queryKey = queryKey.DistinctBy(x => x.KeywordId).ToList();
            foreach (var key in queryKey)
            {
                var cateId = queryCate.Find(x => x.Id.Equals(key.CategoryId)).Id.ToString();
                keyToCate.Add(key.KeywordId, cateId);
            }

            //获取项目内已删除的链接Id
            var builderLinkMap = Builders<Dnl_LinkMapping_Baidu>.Filter;
            var filterLinkMap = builderLinkMap.Eq(x => x.ProjectId, new ObjectId(prjId)) & builderLinkMap.Eq(x => x.DataCleanStatus, (byte)2);
            filterLinkMap &= builderLinkMap.Eq(x => x.Source, SourceType.Baidu);
            var exLinkObjIdList = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).Project(x => x.LinkId).ToList();       //项目中已删除的链接ID列表

            //获取链接信息
            var builderLink = Builders<Dnl_Link_Baidu>.Filter;
            var filterLink = builderLink.In(x => x.SearchkeywordId, keyIdList);
            filterLink &= builderLink.Nin(x => x._id, exLinkObjIdList);
            var queryLink = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => new
            {
                Title = x.Title,
                Description = x.Description,
                Keword = x.Keywords,
                KeywordId = x.SearchkeywordId,
                PublishTime = x.PublishTime,
                LinkUrl = x.LinkUrl,
                DCNum = x.DCNum,
                Domain = x.Domain
            }).ToList();

            //建立节点信息
            var linkNodeList = new List<LinkReferNode>();         //节点信息
            for (int i = 0; i < queryLink.Count; i++)
            {
                //未存在发布信息时跳过该链接
                DateTime tpdt = new DateTime();
                DateTime.TryParse(queryLink[i].PublishTime, out tpdt);
                if (tpdt < new DateTime(1753, 1, 09) || tpdt > DateTime.Now)
                {
                    continue;
                }
                //获取链接信息
                var link = new LinkReferNode
                {
                    publishTime = tpdt,
                    linkUrl = queryLink[i].LinkUrl,
                    value = 1,
                    SortNum = queryLink[i].DCNum,
                    Domain = queryLink[i].Domain,
                    DomainColNum = queryLink[i].DCNum
                };
                if (queryLink[i].Title != null && queryLink[i].Title.Length > 20)
                    link.name = queryLink[i].Title.Substring(0, 19) + "…";
                else
                    link.name = queryLink[i].Title;
                if (queryLink[i].Description != null && queryLink[i].Description.Length > 50)
                    link.describe = queryLink[i].Description.Substring(0, 49) + "…";
                else
                    link.describe = queryLink[i].Description;

                //获取链接所含关键词及数量
                var repeat = queryLink.FindAll(s => s.Title == queryLink[i].Title).DistinctBy(s => s.KeywordId);

                link.keyWordCount = repeat.Count;
                link.keyWordList = new List<string>();
                link.keyWordIdList = new List<string>();
                //移除重复的链接
                foreach (var y in repeat)
                {
                    link.keyWordList.Add(y.Keword);
                    link.keyWordIdList.Add(y.KeywordId);
                    if (i == queryLink.Count)
                        i--;
                    if (queryLink[i].KeywordId != y.KeywordId)
                    {
                        queryLink.Remove(y);
                    }
                }
                //获取归属组序号
                var cateId = keyToCate[queryLink[i].KeywordId];
                var cateIndex = cateInfo.FindIndex(s => s.id == cateId);
                link.category = cateIndex;
                linkNodeList.Add(link);
            }

            //建立时间坐标并将链接信息按时间分组
            DateTime startTime = linkNodeList.Min(x => x.publishTime);
            DateTime endTime = linkNodeList.Max(x => x.publishTime);
            result.DateTimeList = new List<DateTime>();
            int year = startTime.Year;
            int month = startTime.Month;
            int season = (month - 1) / 3;
            var timeLinkNodeList = new List<List<LinkReferNode>>();        //按时间分组的链接节点信息
            switch (timeInterval)
            {
                case 0:     //一次返回所有时间段
                    result.DateTimeList.Add(startTime);
                    timeLinkNodeList.Add(linkNodeList);
                    break;
                case 1:     //按月返回
                    {
                        DateTime timeCoordinate = new DateTime(year, month, 1);
                        while (timeCoordinate <= endTime)
                        {
                            DateTime temp = timeCoordinate;
                            result.DateTimeList.Add(temp);
                            timeCoordinate = timeCoordinate.AddMonths(1);
                        }
                        //将链接信息按月分组
                        foreach (var time in result.DateTimeList)
                        {
                            var tempNode = new List<LinkReferNode>();
                            tempNode = linkNodeList.Where(x => x.publishTime >= time && x.publishTime < time.AddMonths(1)).ToList();
                            timeLinkNodeList.Add(tempNode);
                        }
                        break;
                    }
                case 2:     //按季返回
                    {
                        DateTime timeCoordinate = new DateTime(year, 1 + 3 * season, 1);
                        while (timeCoordinate < endTime)
                        {
                            DateTime temp = timeCoordinate;
                            result.DateTimeList.Add(temp);
                            timeCoordinate = timeCoordinate.AddMonths(3);
                        }
                        //将链接信息按季分组
                        foreach (var time in result.DateTimeList)
                        {
                            var tempNode = new List<LinkReferNode>();
                            tempNode = linkNodeList.Where(x => x.publishTime >= time && x.publishTime < time.AddMonths(3)).ToList();
                            timeLinkNodeList.Add(tempNode);
                        }
                        break;
                    }
                case 3:     //按年返回
                    {
                        DateTime timeCoordinate = new DateTime(year, 1, 1);
                        while (timeCoordinate < endTime)
                        {
                            DateTime temp = timeCoordinate;
                            result.DateTimeList.Add(temp);
                            timeCoordinate = timeCoordinate.AddYears(1);
                        }
                        //将链接信息按月分组
                        foreach (var time in result.DateTimeList)
                        {
                            var tempNode = new List<LinkReferNode>();
                            tempNode = linkNodeList.Where(x => x.publishTime >= time && x.publishTime < time.AddYears(1)).ToList();
                            timeLinkNodeList.Add(tempNode);
                        }
                        break;
                    }
                default:
                    return null;
            }
            result.ReferList = new List<LinkReferDto>();
            for (int i = 0; i < result.DateTimeList.Count; i++)
            {
                //重标记结点的顺序
                for (int j = 0; j < timeLinkNodeList[i].Count; j++)
                {
                    timeLinkNodeList[i][j].Index = j;
                }
                var referData = ComputerLinkRefer(timeLinkNodeList[i], cateInfo, keyIdList, userObjId);
                result.ReferList.Add(referData);
            }

            sw.Stop();
            return result;

        }

        /// <summary>
        /// 计算节点关系
        /// </summary>
        /// <param name="linkNodeList">节点信息</param>
        /// <param name="cateInfo">分组信息</param>
        /// <param name="keyIdList">关键词Id列表</param>
        /// <param name="userObjId">用户Id</param>
        /// <returns></returns>
        static LinkReferDto ComputerLinkRefer(List<LinkReferNode> linkNodeList, List<LinkReferCate> cateInfo, List<string> keyIdList, ObjectId userObjId)
        {
            LinkReferDto result = new LinkReferDto();
            //建立节点关系
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var linkReferList = new List<LinkReferIn2Node>();        //节点间关系
            //按关键词分组生成小集群列表
            var linkByKeyList = new List<LinkReferByKey>();         //关键词链接信息列表
            //var centerNodeList = new List<LinkReferNode>();         //核心节点列表
            foreach (var keyId in keyIdList)
            {
                var nodeList = linkNodeList.FindAll(x => x.keyWordIdList.Contains(keyId));
                if (nodeList.Count > 0)
                {
                    var linkByKey = new LinkReferByKey
                    {
                        KeywordId = keyId,
                        NodeList = nodeList
                    };
                    linkByKeyList.Add(linkByKey);
                }
            }
            //遍历计算关系
            for (int i = 0; i < linkByKeyList.Count; i++)
            {
                string keyId = linkByKeyList[i].KeywordId;
                var nodeList = linkByKeyList[i].NodeList;
                //计算集群内部关系
                var center = nodeList.OrderByDescending(x => x.SortNum).FirstOrDefault();     //选取核心点
                //centerNodeList.Add(center);
                foreach (var node in nodeList)
                {
                    if (node.linkUrl == center.linkUrl)
                        continue;
                    var refer = new LinkReferIn2Node
                    {
                        source = center.Index,
                        target = node.Index
                    };
                    linkReferList.Add(refer);
                }
            }

            sw.Stop();
            //生成Json对象
            JObject json = new JObject();
            JArray jArrayCate = new JArray();
            JArray jArrayLink = new JArray();
            foreach (var x in linkNodeList)
            {
                JObject link = new JObject();
                JProperty name = new JProperty("id", x.name);
                link.Add(name);
                JProperty category = new JProperty("group", x.category);
                link.Add(category);
                jArrayLink.Add(link);
            }
            JProperty jLink = new JProperty("nodes", jArrayLink);
            json.Add(jLink);

            JArray jArrayRefer = new JArray();
            foreach (var x in linkReferList)
            {
                JObject refer = new JObject();
                JProperty source = new JProperty("source", linkNodeList[x.source].name);
                refer.Add(source);
                JProperty target = new JProperty("target", linkNodeList[x.target].name);
                refer.Add(target);
                JProperty value = new JProperty("value", 1);
                refer.Add(value);
                jArrayRefer.Add(refer);
            }
            JProperty jRefer = new JProperty("links", jArrayRefer);
            json.Add(jRefer);
            result.Json = json.ToString();
            return result;
        }

        /// <summary>
        /// 导出基金数据
        /// </summary>
        public static void ExportFoundation()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheetFound = workbook.CreateSheet("基金");
            int page = 0, pagesize = 500;
            var colFound = MongoDBHelper.Instance.GetFoundation();
            var founds = new List<FoundationMongo>();
            while (true)
            {
                var temp = colFound.Find(Builders<FoundationMongo>.Filter.Empty).Skip(page * pagesize).Limit(pagesize).ToList();
                if (temp.Count == 0)
                    break;
                CommonTools.Log("获取基金数据 - {0}".FormatStr(temp.Count));
                founds.AddRange(temp);
                page++;
            }
            CommonTools.Log("总计获取基金数据 - {0}".FormatStr(founds.Count));
            var rowHead = sheetFound.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("Id");
            rowHead.CreateCell(1).SetCellValue("FoundName");
            rowHead.CreateCell(2).SetCellValue("Location");
            rowHead.CreateCell(3).SetCellValue("NetAssets");
            rowHead.CreateCell(4).SetCellValue("OpenPolitics");
            rowHead.CreateCell(5).SetCellValue("Website");
            rowHead.CreateCell(6).SetCellValue("CreditCode");
            rowHead.CreateCell(7).SetCellValue("Chairman");
            rowHead.CreateCell(8).SetCellValue("Secretary");
            rowHead.CreateCell(9).SetCellValue("EstablishTime");
            rowHead.CreateCell(10).SetCellValue("Tel");
            rowHead.CreateCell(11).SetCellValue("OriginalFund");
            rowHead.CreateCell(12).SetCellValue("RegisterDepart");
            rowHead.CreateCell(13).SetCellValue("Email");
            rowHead.CreateCell(14).SetCellValue("Adress");
            rowHead.CreateCell(15).SetCellValue("Purpose");
            rowHead.CreateCell(16).SetCellValue("CreadAt");
            rowHead.CreateCell(17).SetCellValue("FoundationType");
            int i = 1;
            foreach (var item in founds)
            {
                var row = sheetFound.CreateRow(i);
                row.CreateCell(0).SetCellValue(item._id.ToString());
                row.CreateCell(1).SetCellValue(item.FoundName);
                row.CreateCell(2).SetCellValue(item.Location);
                row.CreateCell(3).SetCellValue(item.NetAssets);
                row.CreateCell(4).SetCellValue(item.OpenPolitics);
                row.CreateCell(5).SetCellValue(item.Website);
                row.CreateCell(6).SetCellValue(item.CreditCode);
                row.CreateCell(7).SetCellValue(item.Chairman);
                row.CreateCell(8).SetCellValue(item.Secretary);
                row.CreateCell(9).SetCellValue(item.EstablishTime);
                row.CreateCell(10).SetCellValue(item.Tel);
                row.CreateCell(11).SetCellValue(item.OriginalFund);
                row.CreateCell(12).SetCellValue(item.RegisterDepart);
                row.CreateCell(13).SetCellValue(item.Email);
                row.CreateCell(14).SetCellValue(item.Adress);
                row.CreateCell(15).SetCellValue(item.Purpose);
                row.CreateCell(16).SetCellValue(item.CreadAt);
                row.CreateCell(17).SetCellValue(item.FoundationType);
                i++;
            }

            ISheet sheetProject = workbook.CreateSheet("项目");
            page = 0;
            var colPro = MongoDBHelper.Instance.GetFoundation_project();
            var projects = new List<Foundation_projectMongo>();
            while (true)
            {
                var temp = colPro.Find(Builders<Foundation_projectMongo>.Filter.Empty).Skip(page * pagesize).Limit(pagesize).ToList();
                if (temp.Count == 0)
                    break;
                CommonTools.Log("获取项目数据 - {0}".FormatStr(temp.Count));
                projects.AddRange(temp);
                page++;
            }
            CommonTools.Log("总计获取项目数据 - {0}".FormatStr(projects.Count));
            var rowHead2 = sheetProject.CreateRow(0);
            rowHead2.CreateCell(0).SetCellValue("Id");
            rowHead2.CreateCell(1).SetCellValue("FoundId");
            rowHead2.CreateCell(1).SetCellValue("FoundName");
            rowHead2.CreateCell(2).SetCellValue("ProjectName");
            rowHead2.CreateCell(3).SetCellValue("ExecutiveYear");
            rowHead2.CreateCell(4).SetCellValue("YearIncome");
            rowHead2.CreateCell(5).SetCellValue("YearExpenditure");
            rowHead2.CreateCell(6).SetCellValue("AttentionField");
            rowHead2.CreateCell(7).SetCellValue("FundUse");
            rowHead2.CreateCell(8).SetCellValue("BenefitGroup");
            rowHead2.CreateCell(9).SetCellValue("ProjectBrief");
            i = 1;
            foreach (var item in projects)
            {
                var row = sheetProject.CreateRow(i);
                row.CreateCell(0).SetCellValue(item._id.ToString());
                row.CreateCell(1).SetCellValue(item.Fid.ToString());
                row.CreateCell(1).SetCellValue(founds.Find(x => x._id == item.Fid).FoundName.ToString());
                row.CreateCell(2).SetCellValue(item.ProjectName);
                row.CreateCell(3).SetCellValue(item.ExecutiveYear);
                row.CreateCell(4).SetCellValue(item.YearIncome);
                row.CreateCell(5).SetCellValue(item.YearExpenditure);
                row.CreateCell(6).SetCellValue(item.AttentionField);
                row.CreateCell(7).SetCellValue(item.FundUse);
                row.CreateCell(8).SetCellValue(item.BenefitGroup);
                row.CreateCell(9).SetCellValue(item.ProjectBrief);
                i++;
            }

            using (FileStream fs = new FileStream("found.xls", FileMode.Create))
            {
                workbook.Write(fs);
            }
            CommonTools.Log("输出完毕");
        }

        /// <summary>
        /// 拆分百度数据
        /// </summary>
        public static void SplitBaiduData()
        {
            Console.Write("起始页数（从0开始）：");
            int page = Convert.ToInt32(Console.ReadLine());
            Console.Write("结束页数：");
            int pageEnd = Convert.ToInt32(Console.ReadLine());
            int  pagesize = 10;
            var keywords = new List<Dnl_Keyword>();
            var colKey = MongoDBHelper.Instance.GetDnl_Keyword();
            var filterKey = Builders<Dnl_Keyword>.Filter.Empty;
            var keyNum = colKey.Find(filterKey).Count();
            int usedNum = page * pagesize;        //已拆分关键词数
            bool IsAllNotMove = false;
            while (usedNum < keyNum && page < pageEnd)
            {
                //获取关键词ID
                var tpKeyIds = colKey.Find(filterKey).SortBy(x => x.CreatedAt).Skip(page * pagesize).Limit(pagesize).Project(x => x._id).ToList();
                usedNum += tpKeyIds.Count;
                page++;
                CommonTools.Log("当前拆分关键词[{0}/{1}/{2}]".FormatStr(usedNum, pageEnd * pagesize, keyNum));
                var keyIds = new List<string>();
                var colLinkMain = MongoDBHelper.Instance.GetBaiduLinkMain();
                var builderLinkMain = Builders<BaiduLinkMainMongo>.Filter;
                if (!IsAllNotMove)
                {
                    foreach (var tpId in tpKeyIds)
                    {
                        var filterLinkMain = builderLinkMain.Eq(x => x.KeywordId, tpId);
                        var count = colLinkMain.Find(filterLinkMain).Count();
                        if (count == 0)
                        {
                            keyIds.Add(tpId.ToString());
                        }
                    }
                    if (keyIds.Count == tpKeyIds.Count)
                    {
                        IsAllNotMove = true;
                    }
                    if (keyIds.Count == 0)
                    {
                        CommonTools.Log("本批已全部拆分！");
                        continue;
                    }
                }
                else
                {
                    keyIds = tpKeyIds.Select(x => x.ToString()).ToList();
                }

                //获取链接及映射详情
                var filterLink = Builders<Dnl_Link_Baidu>.Filter.In(x => x.SearchkeywordId, keyIds);
                var links = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).ToList();

                if (links.Count == 0)
                {
                    CommonTools.Log("本批次无链接");
                    continue;
                }
                else
                {
                    CommonTools.Log("本次链接数 - {0}".FormatStr(links.Count));
                }
                var builderLinkMap=Builders<Dnl_LinkMapping_Baidu>.Filter;
                var filterLinkMap = builderLinkMap.In(x => x.LinkId, links.Select(x => x._id));
                var linkMaps = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).ToList();

                //拆分并转换格式
                var colLinkContent = MongoDBHelper.Instance.GetBaiduLinkContent();
                var colLinkOther = MongoDBHelper.Instance.GetBaiduLinkOther();

                int i = 0;
                var linkMains = new List<BaiduLinkMainMongo>();
                var linkOthers = new List<BaiduLinkOtherMongo>();
                var linkContents = new List<BaiduLinkContentMongo>();
                var newLinkMaps = new List<LinkMappingMongo>();
                foreach (var oldLink in links)
                {
                    i++;

                    var keyObjId=new ObjectId(oldLink.SearchkeywordId);
                    var linkMain = new BaiduLinkMainMongo
                    {
                        _id = ObjectId.GenerateNewId(),
                        DCNum = oldLink.DCNum,
                        CreatedAt = oldLink.CreatedAt,
                        Description = oldLink.Description,
                        Domain = oldLink.Domain,
                        Keyword = oldLink.Keywords,
                        KeywordId = keyObjId,
                        LinkUrl = oldLink.LinkUrl,
                        Title = oldLink.Title,
                    };
                    DateTime publishAt = new DateTime();
                    if (DateTime.TryParse(oldLink.PublishTime, out publishAt))
                    {
                        linkMain.PublishAt = publishAt;
                    }
                    linkMains.Add(linkMain);

                    var linkOther = new BaiduLinkOtherMongo
                    {
                        TopDomain = oldLink.TopDomain,
                        BaiduVStar = oldLink.BaiduVStar,
                        IsPromotion = oldLink.IsPromotion,
                        KeywordId = keyObjId,
                        LinkId = linkMain._id,
                        MatchAt = oldLink.MatchAt
                    };
                    linkOthers.Add(linkOther);

                    var linkContent = new BaiduLinkContentMongo
                    {
                        KeywordId = keyObjId,
                        LinkId = linkMain._id,
                    };

                    //判断网页是否过大
                    if (oldLink.Html.Length > 262144)
                    {
                        CommonTools.Log("网页大小超出5M - {0} - {1}".FormatStr(oldLink.Title, oldLink.LinkUrl));
                    }
                    else
                    {
                        //判断网页编码是否错误，错误则重新获取
                        string html = oldLink.Html;
                        int length = html.Length;
                        if (!Regex.IsMatch(html, "charset=\"utf-8|charset=\"UTF-8|charset=utf-8|charset=UTF-8"))
                        {
                            if (Regex.IsMatch(html, "charset=\"gb2312|charset=\"GB2312|charset=gb2312|charset=GB2312"))
                            {
                                html = WebApiInvoke.GetHtml(oldLink.LinkUrl, "gb2312");
                            }
                            else if (Regex.IsMatch(html, "charset=\"gbk|charset=\"GBK|charset=gbk|charset=GBK"))
                            {
                                html = WebApiInvoke.GetHtml(oldLink.LinkUrl, "gbk");
                            }
                        }
                        if (html == null)
                        {
                            html = oldLink.Html;
                        }

                        if (html.Length > 262144)
                        {
                            CommonTools.Log("网页大小超出5M - {0} - {1}".FormatStr(oldLink.Title, oldLink.LinkUrl));
                        }
                        else
                        {
                            linkContent.Html = html;
                            length = html.Length;

                            //获取网页中文正文
                            string content = "";
                            try
                            {
                                var transcoder = new NReadabilityTranscoder();
                                TranscodingInput input = new TranscodingInput(html);
                                TranscodingResult result = transcoder.Transcode(input);
                                if (result.ContentExtracted)
                                {
                                    string htmlCon = result.ExtractedContent.SubAfter("</head>").SubBefore("</html>");
                                    content = Regex.Replace(htmlCon, "</p>|<br/>|<br>|</h[0-9]>", System.Environment.NewLine);
                                    content = Regex.Replace(content, "<.+?>", "");
                                    content = Regex.Replace(content, " ", " ");
                                }
                            }
                            catch (Exception ex)
                            {
                                CommonTools.Log("正文提取错误，错误原因：{0}".FormatStr(ex.Message));
                                CommonTools.Log("错误链接[{0}/{1}] - {2}".FormatStr(i, links.Count, oldLink.Title));
                            }
                            linkContent.Content = content;
                            linkContents.Add(linkContent);
                        }
                    }

                    var oldMap = linkMaps.Find(x => x.LinkId == oldLink._id);
                    if (oldMap != null)
                    {
                        var newMap = new LinkMappingMongo
                        {
                            DataCleanStatus = oldMap.DataCleanStatus,
                            CreatedAt = oldMap.CreatedAt,
                            InfriLawCode = oldMap.InfriLawCode,
                            LinkId = oldMap.LinkId,
                            ProjectId = oldMap.ProjectId,
                            Source = SourceType.Baidu,
                            UserId = oldMap.UserId
                        };
                        newLinkMaps.Add(newMap);
                    }
                }
                //int inserNum = 0;
                //int insertSize=1;
                //while (inserNum<linkMains.Count)
                //{
                    colLinkMain.InsertMany(linkMains);
                    colLinkOther.InsertMany(linkOthers);
                    //var temp = linkContents.Skip(inserNum).Take(insertSize).ToList();
                    colLinkContent.InsertMany(linkContents);
                    //inserNum += insertSize;
                    //CommonTools.Log("保存拆分链接[{0}/{1}]".FormatStr(inserNum, linkMains.Count));
                //}
                if (newLinkMaps.Count > 0)
                {
                    MongoDBHelper.Instance.GetLinkMapping().InsertMany(newLinkMaps);
                }
                CommonTools.Log("本批拆分完毕");
            }
        }

        /// <summary>
        /// 拆分百度数据
        /// </summary>
        public static void DelBaiduData()
        {
            Console.Write("起始页数（从0开始）：");
            int page = Convert.ToInt32(Console.ReadLine());
            Console.Write("结束页数：");
            int pageEnd = Convert.ToInt32(Console.ReadLine());
            int pagesize = 10;
            var keywords = new List<Dnl_Keyword>();
            var colKey = MongoDBHelper.Instance.GetDnl_Keyword();
            var filterKey = Builders<Dnl_Keyword>.Filter.Empty;
            var keyNum = colKey.Find(filterKey).Count();
            int usedNum = page * pagesize;        //已拆分关键词数
            while (page < pageEnd)
            {
                //获取关键词ID
                var keyIds = colKey.Find(filterKey).SortBy(x => x.CreatedAt).Skip(page * pagesize).Limit(pagesize).Project(x => x._id).ToList();
                usedNum += keyIds.Count;
                page++;
                CommonTools.Log("当前拆分关键词[{0}/{1}/{2}]".FormatStr(usedNum, pageEnd * pagesize, keyNum));
                //获取拆分链接
                //var filterLink = Builders<BaiduLinkMainMongo>.Filter.In(x => x.KeywordId, keyIds);
                //var linkIds = MongoDBHelper.Instance.GetBaiduLinkMain().Find(filterLink).Project(x => x._id).ToList();
                var filterLink = Builders<Dnl_Link_Baidu>.Filter.In(x => x.SearchkeywordId, keyIds.Select(x => x.ToString()));
                var linkIds = MongoDBHelper.Instance.GetDnl_Link_Baidu().Find(filterLink).Project(x => x._id).ToList();
                CommonTools.Log("本次链接数 - {0}".FormatStr(linkIds.Count));
                if (linkIds.Count == 0)
                {
                    break;
                }

                ////拆分并转换格式
                //var colLinkMain = MongoDBHelper.Instance.GetBaiduLinkMain();
                //var colLinkContent = MongoDBHelper.Instance.GetBaiduLinkContent();
                //var colLinkOther = MongoDBHelper.Instance.GetBaiduLinkOther();

                //colLinkMain.DeleteMany(filterLink);
                //colLinkOther.DeleteMany(Builders<BaiduLinkOtherMongo>.Filter.In(x => x.KeywordId, keyIds));
                //colLinkContent.DeleteMany(Builders<BaiduLinkContentMongo>.Filter.In(x => x.KeywordId, keyIds));

                //MongoDBHelper.Instance.GetBaiduLinkMapping().DeleteMany(Builders<BaiduLinkMappingMongo>.Filter.In(x => x.LinkId, linkIds));
                CommonTools.Log("本批已拆分完毕");
            }
            CommonTools.Log("全部已拆分完毕");
        }

        /// <summary>
        /// 合并域名分组
        /// </summary>
        public static void CombineDomainCate()
        {
            var builderCate = Builders<IW2S_DomainCategory>.Filter;
            var colCate = MongoDBHelper.Instance.GetIW2S_DomainCategorys();
            var builder = Builders<IW2S_DomainCategoryData>.Filter;
            var col = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas();

            //获取公用分组及域名
            var filterCate = builderCate.Eq(x => x.UsrId, ObjectId.Empty) & builderCate.Eq(x => x.IsDel, false);
            var publicCates = colCate.Find(filterCate).Project(x => new IdAndName
            {
                Id = x._id,
                Name = x.Name
            }).ToList();
            var filter = builder.In(x => x.DomainCategoryId, publicCates.Select(x => x.Id));
            
            var publicDomains = col.Find(filter).Project(x => new IdAndName
            {
                Id = x._id,
                Name = x.DomainName,
                CateId = x.DomainCategoryId
            }).ToList();

            //获取所有用户信息
            var users = MongoDBHelper.Instance.Get_IW2SUser().Find(Builders<IW2SUser>.Filter.Eq(x => x.IsDel, false)).Project(x => new IdAndName
            {
                Id = x._id,
                Name = x.LoginName
            }).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                var user=users[i];
                CommonTools.Log("当前合并用户[{0}/{1}] - {2}".FormatStr(i + 1, users.Count, user.Name));
                //获取当前用户分组及域名
                filterCate = builderCate.Eq(x => x.UsrId, user.Id) & builderCate.Eq(x => x.IsDel, false);
                var privateCates = colCate.Find(filterCate).Project(x => new IdAndName
                {
                    Id = x._id,
                    Name = x.Name,
                    Weight=x.Weight
                }).ToList();
                if (privateCates.Count == 0)
                {
                    CommonTools.Log("该用户无分组！");
                    continue;
                }
                filter = builder.In(x => x.DomainCategoryId, privateCates.Select(x => x.Id));
                var privateDomains = col.Find(filter).Project(x => new IdAndName
                {
                    Id = x._id,
                    Name = x.DomainName,
                    CateId = x.DomainCategoryId
                }).ToList();

                var newCates = new List<IW2S_DomainCategory>();
                var newDomains = new List<IW2S_DomainCategoryData>();
                //先合并分有分组
                foreach (var item in publicCates)
                {
                    var cate = new IW2S_DomainCategory
                    {
                        _id = ObjectId.GenerateNewId(),
                        Name = item.Name,
                        UsrId = user.Id,
                        Weight = item.Weight
                    };
                    var tpDomains = publicDomains.Where(x => x.CateId == item.Id).Select(x => new IW2S_DomainCategoryData
                    {
                        _id = ObjectId.GenerateNewId(),
                        DomainCategoryId = cate._id,
                        DomainName = x.Name,
                        UsrId = user.Id
                    }).ToList();
                    if (tpDomains.Count > 0)
                    {
                        newDomains.AddRange(tpDomains);
                    }

                    newCates.Add(cate);
                }

                //合并私有分组
                foreach (var item in privateCates)
                {
                    //判断是有否同名分组
                    if (!newCates.Exists(x => x.Name == item.Name))
                    {
                        var cate = new IW2S_DomainCategory
                        {
                            _id = ObjectId.GenerateNewId(),
                            Name = item.Name,
                            UsrId = user.Id,
                            Weight = item.Weight
                        };
                        var tpDomains = publicDomains.Where(x => x.CateId == item.Id).Select(x => new IW2S_DomainCategoryData
                        {
                            _id = ObjectId.GenerateNewId(),
                            DomainCategoryId = cate._id,
                            DomainName = x.Name,
                            UsrId = user.Id
                        }).ToList();
                        if (tpDomains.Count > 0)
                        {
                            newDomains.AddRange(tpDomains);
                        }

                        newCates.Add(cate);
                    }
                    else
                    {
                        //新分组的Id
                        var newCate = newCates.Find(x => x.Name == item.Name);
                        newCates.Remove(newCate);
                        //获取新分组内域名
                        var tpNewDomains = newDomains.FindAll(x => x.DomainCategoryId == newCate._id);
                        if (tpNewDomains.Count > 0)
                        {
                            tpNewDomains.ForEach(x => x.DomainCategoryId = item.Id);
                            var temp = privateDomains.FindAll(x => x.CateId == item.Id);
                            if (temp.Count > 0)
                            {
                                var tpOldDomains = temp.Select(x => x.Name).ToList();//用户本分组下已存在的域名
                                //删除已存在的域名
                                newDomains.RemoveAll(x => x.DomainCategoryId == item.Id && tpOldDomains.Contains(x.DomainName));
                            }
                        }
                    }
                }
                if (newDomains.Count > 0)
                {
                    col.InsertMany(newDomains);
                }
                if (newCates.Count > 0)
                    colCate.InsertMany(newCates);
            }
        }

        public static void GetMultiContent(ObjectId proObjId)
        {
            //获取图表数据
            var builderChart = Builders<ProjectChartMongo>.Filter;
            var filterChart = builderChart.Eq(x => x.ProjectId, proObjId) & builderChart.Eq(x => x.Type, ChartType.D3ForceTable);
            filterChart &= builderChart.Eq(x => x.Source, SourceType.Baidu) & builderChart.Eq(x => x.Name, "默认");
            var colChart = MongoDBHelper.Instance.GetProjectChart();
            var queryChart = colChart.Find(filterChart).FirstOrDefault();
            if (queryChart != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();       //Json序列化与反序列化
                var datas = serializer.Deserialize<List<D3ForceTableDto>>(queryChart.DataJson); 

                //获取核心区结点
                var centers = datas.FindAll(x => x.Percent <= 25);
                var temp = new List<MultiLinkInfo>();
                double totalInflNum = centers.Sum(x => x.LinkReferByDo);
                double totalKeyNum = centers.Sum(x => x.Keyword.Count);
                //var keys = new List<string>();
                //foreach (var item in centers)
                //{
                //    keys.AddRange(item.Keyword);
                //}
                //totalKeyNum = keys.Distinct().Count();
                foreach (var item in centers)
                {
                    var info = new MultiLinkInfo
                    {
                        Id = item.Id,
                    };
                    info.TotalNum = item.Keyword.Count / totalKeyNum + item.LinkReferByDo / totalInflNum;
                    temp.Add(info);
                }
                temp = temp.OrderByDescending(x => x.TotalNum).ToList();
                var contents = new List<string>();

                int i = 1;
                JiebaHelper jieba = new JiebaHelper();
                foreach (var link in temp)
                {
                    var content = MongoDBHelper.Instance.GetBaiduLinkContent().Find(Builders<BaiduLinkContentMongo>.Filter.Eq(x => x.LinkId, new ObjectId(link.Id))).Project(x => x.Content).FirstOrDefault();
                    if (content == null)
                    {
                        content = MongoDBHelper.Instance.GetBingLinkContent().Find(Builders<BingLinkContentMongo>.Filter.Eq(x => x.LinkId, new ObjectId(link.Id))).Project(x => x.Content).FirstOrDefault();
                    }
                    if (content == null || content.Length < 100)
                    {
                        continue;
                    }
                    var abs = jieba.GetSummary(null,null,null,content);
                    foreach (var item in abs)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine();
                }

                
                //foreach (var link in contents)
                //{
                //    try
                //    {
                //        string content = "";
                //        var transcoder = new NReadabilityTranscoder();
                //        TranscodingInput input = new TranscodingInput(htmldetail);
                //        TranscodingResult result = transcoder.Transcode(input);
                //        if (result.ContentExtracted)
                //        {
                //            string htmlCon = result.ExtractedContent.SubAfter("</head>").SubBefore("</html>");
                //            content = Regex.Replace(htmlCon, "</p>|<br/>|<br>|</h[0-9]>", System.Environment.NewLine);
                //            content = Regex.Replace(content, "<.+?>", "");
                //            content = Regex.Replace(content, " ", " ");
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        CommonHelper.Log("正文提取错误，错误原因：{0}".FormatStr(ex.Message));
                //    }
                //}
            }
        }

        /// <summary>
        /// 重置项目图表预计算状态
        /// </summary>
        /// <param name="projectId">项目Id</param>
        public static void ResetChartPreCompute(ObjectId projectId, SourceType sourceType)
        {
            //获取当前项目预计算状态
            var colPro = MongoDBHelper.Instance.GetIW2S_Projects();
            var filterPro = new QueryDocument { { "_id", projectId } };
            var pro = colPro.Find(filterPro).Project(x => new IW2S_Project
            {
                BDPreStatus = x.BDPreStatus,
                BDPreUp = x.BDPreUp,
                BingPreStatus = x.BingPreStatus,
                BingPreUp = x.BingPreUp
            }).FirstOrDefault();

            switch (sourceType)
            {
                case SourceType.Baidu:
                    {
                        //判断项目是否已经在计算中
                        switch (pro.BDPreStatus)
                        {
                            case 1:
                                {
                                    //通知计算程序项目出现新的变动
                                    var updateAdd = Builders<IW2S_Project>.Update.Set(x => x.BDPreUp, 1);
                                    colPro.UpdateOne(filterPro, updateAdd);
                                }
                                break;
                            case 2:
                                {
                                    //删除原有预计算数据将项目标记为需要计算
                                    MongoDBHelper.Instance.GetProjectChart().DeleteMany(Builders<ProjectChartMongo>.Filter.Eq(x => x.ProjectId, projectId));
                                    var updateAdd = Builders<IW2S_Project>.Update.Set(x => x.BDPreStatus, 0);
                                    colPro.UpdateOne(filterPro, updateAdd);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case SourceType.Weixin:
                    break;
                case SourceType.Bing:
                    {
                        //判断项目是否已经在计算中
                        switch (pro.BingPreStatus)
                        {
                            case 1:
                                {
                                    //通知计算程序项目出现新的变动
                                    var updateAdd = Builders<IW2S_Project>.Update.Set(x => x.BingPreUp, 1);
                                    colPro.UpdateOne(filterPro, updateAdd);
                                }
                                break;
                            case 2:
                                {
                                    //删除原有预计算数据将项目标记为需要计算
                                    MongoDBHelper.Instance.GetProjectChart().DeleteMany(Builders<ProjectChartMongo>.Filter.Eq(x => x.ProjectId, projectId));
                                    var updateAdd = Builders<IW2S_Project>.Update.Set(x => x.BingPreStatus, 0);
                                    colPro.UpdateOne(filterPro, updateAdd);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        public static void SearchNewrankWXAccount()
        {
            //搜索关键词
            string linkUrl = "https://api.newrank.cn/api/sync/weixin/account/info";       //关键词获取文章接口地址
            string appkey = "02b6f7c73e9d4814b79f4feb1";
            var proObjId = new ObjectId("5b0d4dc4f4b87d0c1c66f4f1");
            var keyObjIds = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(new QueryDocument { { "ProjectId", proObjId } }).Project(x => x.KeywordId.ToString()).ToList().Distinct().ToList();
            var linkInfos = MongoDBHelper.Instance.GetWXLinkMain().Find(Builders<WxLinkMainMongo>.Filter.In(x => x.KeywordId, keyObjIds)).Project(x => new
            {
                Id = x._id,
                Account = x.Name,
                NickName=x.Nickname,
                NameId=x.NameId
            }).ToList();

            var builder_Account = Builders<WXName_NewrankMongo>.Filter;
            var col_Account = MongoDBHelper.Instance.GetWXName_Newrank();
            int num = 0;
            foreach(var link in linkInfos)
            {
                num++;
                var filter_Account = builder_Account.Eq(x => x.Account, link.Account);
                ObjectId acccoutId = col_Account.Find(filter_Account).Project(x => x._id).FirstOrDefault();
                if (acccoutId!=ObjectId.Empty)
                {
                    CommonTools.Log("公众号：{0} 已查询".FormatStr(link.NickName));
                }
                else
                {
                    CommonTools.Log("当前查询公众号[{0}/{1}]：{2}".FormatStr(num, linkInfos.Count, link.NickName));
                    Thread.Sleep(1000);
                    Dictionary<string, object> postData = new Dictionary<string, object>();                 //post参数
                    postData.Add("account", link.Account);
                    #region 查询请求
                    HttpWebRequest request = WebRequest.Create(linkUrl) as HttpWebRequest;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    request.Headers.Add("Key", appkey);
                    //发送POST数据
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in postData.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, postData[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, postData[key]);
                            i++;
                        }
                    }
                    byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                    string[] value = request.Headers.GetValues("Content-Type");
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Stream myResponseStream = response.GetResponseStream();
                    StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                    string resultStr = myStreamReader.ReadToEnd();
                    myStreamReader.Close();
                    myResponseStream.Close();
                    # endregion

                    //解析Json字符串
                    JObject linkJson = new JObject();
                    try
                    {
                        linkJson = JObject.Parse(resultStr);
                    }
                    catch (Exception ex)
                    {
                        CommonTools.Log("微信搜索出错：" + ex.Message);
                        continue;
                    }

                    if (linkJson.Property("code") != null && linkJson.Property("code").Value.ToString() == "0")
                    {
                        //CommonTools.Log("链接" + linkJson.Property("msg").Value);
                        //CommonTools.Log("从{0}至{1}内第{2}页数据已搜索完成！\n".FormatStr(tpStartDate.ToString("yyyy-MM-dd"), tpEndDate.ToString("yyyy-MM-dd"),page));
                        JArray items = (JArray)linkJson["data"];
                        for (i = 0; i < items.Count; i++)
                        {
                            //获取常用链接信息
                            JObject item = (JObject)items[i];
                            if (item == null)
                                break;
                            var account = new WXName_NewrankMongo()
                            {
                                _id = ObjectId.GenerateNewId(),
                                CreatedAt = DateTime.Now.AddHours(8),
                                Account = link.Account
                            };
                            if (item.Property("name") != null)
                                account.Name = item.Property("name").Value.ToString();
                            if (item.Property("account") != null)
                                account.Account = item.Property("account").Value.ToString();
                            if (item.Property("description") != null)
                                account.Description = item.Property("description").Value.ToString();

                            if (item.Property("certifiedText") != null)
                                account.CertifiedText = item.Property("certifiedText").Value.ToString();
                            if (item.Property("wxId") != null)
                                account.WXId = item.Property("wxId").Value.ToString();
                            if (item.Property("tags") != null)
                            {
                                try
                                {
                                    JArray Jtags = (JArray)item["tags"];
                                    account.Tags = Jtags.ToObject<List<string>>();
                                }
                                catch
                                {
                                    CommonTools.Log(item["tags"].ToString());
                                }
                            }
                            if (item.Property("type") != null)
                                account.Type = item.Property("type").Value.ToString();

                            JToken dsf = item.Property("favorite").Value;
                            string str = dsf.ToString();
                            if (item.Property("favorite") != null && !string.IsNullOrEmpty(item.Property("favorite").Value.ToString()))
                                account.Favorite = Convert.ToInt32(item.Property("favorite").Value);
                            if (item.Property("isOriUser") != null && !string.IsNullOrEmpty(item.Property("isOriUser").Value.ToString()))
                                account.IsOriUser = Convert.ToInt32(item.Property("isOriUser").Value);

                            if (item.Property("headImageUrl") != null)
                                account.HeadImageUrl = item.Property("headImageUrl").Value.ToString();
                            if (item.Property("codeImageUrl") != null)
                                account.CodeImageUrl = item.Property("codeImageUrl").Value.ToString();
                            if (item.Property("weekNri") != null && !string.IsNullOrEmpty(item.Property("weekNri").Value.ToString()))
                                account.WeekNri = Convert.ToDouble(item.Property("weekNri").Value);

                            if (item.Property("miniProgramInfos") != null)
                                account.MiniProgramInfos = item.Property("miniProgramInfos").Value.ToString();
                            if (item.Property("platformInfos") != null)
                                account.PlatformInfos = item.Property("platformInfos").Value.ToString();

                            DateTime dt = new DateTime();
                            if (item.Property("joinDate") != null)
                                DateTime.TryParse(item.Property("joinDate").Value.ToString(), out dt);
                            account.JoinDate = dt.AddHours(8);
                            dt = DateTime.MinValue;

                            col_Account.InsertOne(account);
                            acccoutId = account._id;
                        }
                    }
                    else if (linkJson.Property("code").Value.ToString() == "1203")
                    {
                        CommonTools.Log("数据不存在！\n");
                    }
                    else
                    {
                        CommonTools.Log("错误信息 - " + linkJson.Property("msg").ToString());
                        CommonTools.Log("错误代码 - " + linkJson.Property("code").ToString());
                        break;
                    }

                    //更新链接公众号Id
                    var update = Builders<WxLinkMainMongo>.Update.Set(x => x.NameId, acccoutId);
                    MongoDBHelper.Instance.GetWXLinkMain().UpdateOne(new QueryDocument { { "_id", link.Id } }, update);
                }
            }
                
        }

        public static void FilterWXLink(string projectId)
        {
            var proObjId = new ObjectId(projectId);
            var builderKey=Builders<MediaKeywordMappingMongo>.Filter;
            var filterKey = builderKey.Eq(x => x.ProjectId, proObjId) & builderKey.Eq(x => x.CategoryId, ObjectId.Empty) & builderKey.Eq(x => x.IsDel, false);
            var keyIds = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKey).Project(x => x.KeywordId.ToString()).ToList().Distinct().ToList();
            var builderLink=Builders<WxLinkMainMongo>.Filter;
            var colLink=MongoDBHelper.Instance.GetWXLinkMain();
            var links = colLink.Find(builderLink.In(x => x.KeywordId, keyIds)).Project(x => new
            {
                Id = x._id,
                Url = x.Url
            }).ToList();
            for (int i = 0; i < links.Count; i++)
            {
                if (i < 1644)
                    continue;
                var link = links[i];
                var content = WebApiInvoke.GetHtml(link.Url);
                WXDelLinkType delType = WXDelLinkType.Normal;
                if (content.Contains("此内容因违规无法查看"))
                {
                    delType = WXDelLinkType.Illegal;
                }
                else if (content.Contains("此帐号已被屏蔽, 内容无法查看"))
                {
                    delType = WXDelLinkType.Shield;
                }
                else if (content.Contains("此帐号已自主注销，内容无法查看"))
                {
                    delType = WXDelLinkType.LogOff;
                }
                else if (content.Contains("此内容被投诉且经审核涉嫌侵权，无法查看"))
                {
                    delType = WXDelLinkType.Tort;
                }
                else if (content.Contains("该内容已被发布者删除"))
                {
                    delType = WXDelLinkType.Del;
                }
                var update = Builders<WxLinkMainMongo>.Update.Set(x => x.DelType, delType);
                var filterLink = builderLink.Eq(x => x._id, link.Id);
                colLink.UpdateOne(filterLink, update);
                CommonTools.Log("{0}/{1} - {2}".FormatStr(i, links.Count, delType));
            }
        }

        public static void CheckRepeat(ObjectId projectId)
        {
            var builderCate=Builders<MediaKeywordCategoryMongo>.Filter;
            var cates = MongoDBHelper.Instance.GetMediaKeywordCategory().Find(builderCate.Eq(x => x.ProjectId, projectId) & builderCate.Eq(x => x.IsDel, false) & builderCate.Eq(x => x.ParentId, ObjectId.Empty)).Project(x => new
            {
                Id = x._id,
                Name = x.Name
            }).ToList();

            foreach (var cate in cates)
            {
                //获取当前分组内关键词
                CommonTools.Log("当前检查关键词分组{0}".FormatStr(cate.Name));
                var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
                var filterKeyMap = builderKeyMap.Eq(x => x.CategoryId, cate.Id) & builderKeyMap.Eq(x => x.IsDel, false);
                var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
                {
                    Id = x.KeywordId.ToString(),
                    Name = x.Keyword
                }).ToList();
                //获取当前分组所有链接
                var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
                var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();
                var builderLinkContent = Builders<WxLinkContentMongo>.Filter;
                var colLinkContent = MongoDBHelper.Instance.GetWXLinkContent();
                var links = colLinkMain.Find(builderLinkMain.In(x => x.KeywordId, keys.Select(x => x.Id))).Project(x => new LinkInfo
                {
                    Id = x._id,
                    Title = x.Title,
                    Desc = x.Description,
                    Keyword=x.Keyword,
                    KeywordId=x.KeywordId,
                    Url=x.Url,
                }).ToList();
                //遍历关键词，查找同组内重复的链接
                var builderKey = Builders<MediaKeywordMongo>.Filter;
                var colKey = MongoDBHelper.Instance.GetMediaKeyword();
                foreach (var key in keys)
                {
                    CommonTools.Log("检查关键词：{0}".FormatStr(key.Name));
                    foreach (var link in links)
                    {
                        if (link.Keyword == key.Name)
                            continue;
                        var tpKeys = key.Name.Split(' ');
                        bool isRepeat = true;
                        
                        foreach (var x in tpKeys)
                        {
                            if (!link.Title.Contains(x) && !link.Desc.Contains(x))
                            {
                                isRepeat = false;
                                break;
                            }
                        }
                        if (isRepeat)
                        {
                            if (link.Title.Contains("Gucci古驰彩虹亮片领蝴蝶结圆领T恤"))
                            {
                                int dad = 324;
                            }
                            //判断是否已经保存过这个链接
                            var existLink = links.Find(x => x.Keyword == key.Name && x.Url == link.Url);
                            if (existLink != null)
                                continue;
                            CommonTools.Log("重复关键词 - {0}".FormatStr(link.Keyword));
                            CommonTools.Log("重复链接 - {0}".FormatStr(link.Title));
                            CommonTools.Log("重复描述 - {0}".FormatStr(link.Desc));
                            var linkDetail = colLinkMain.Find(builderLinkMain.Eq(x => x._id, link.Id)).FirstOrDefault();
                            linkDetail._id = ObjectId.GenerateNewId();
                            linkDetail.Keyword = key.Name;
                            linkDetail.KeywordId = key.Id;
                            linkDetail.CreatedAt = DateTime.Now.AddHours(8);
                            colLinkMain.InsertOne(linkDetail);
                            try
                            {
                                var copyLinkContent = colLinkContent.Find(builderLinkContent.Eq(x => x.LinkId, link.Id)).FirstOrDefault();
                                copyLinkContent._id = linkDetail._id;
                                copyLinkContent.KeywordId = linkDetail.KeywordId;
                                colLinkContent.InsertOne(copyLinkContent);
                            }
                            catch (Exception ex)
                            {
                                CommonTools.Log("无正文");
                            }
                            colKey.UpdateOne(builderKey.Eq(x => x._id, new ObjectId(key.Id)), Builders<MediaKeywordMongo>.Update.Inc(x => x.WXLinkNum, 1));
                        }
                    }
                }
            }
        }

        public static void DelRepeat(ObjectId projectId)
        {

            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();
            var builderLinkContent = Builders<WxLinkContentMongo>.Filter;
            var colLinkContent = MongoDBHelper.Instance.GetWXLinkContent();
            var builderCate = Builders<MediaKeywordCategoryMongo>.Filter;

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.ProjectId,projectId) & builderKeyMap.Eq(x => x.IsDel, false);
            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);

            foreach (var key in keys)
            {
                CommonTools.Log("检查关键词：{0}".FormatStr(key.Name));
                //获取当前分组所有链接
                var links = colLinkMain.Find(builderLinkMain.Eq(x => x.KeywordId, key.Id)).Project(x => new LinkInfo
                {
                    Id = x._id,
                    Title = x.Title,
                    Keyword = x.Keyword,
                    KeywordId = x.KeywordId,
                    Url = x.Url,
                    CreatedAt=x.CreatedAt
                }).ToList();

                var linkGroup = links.GroupBy(x => x.Url).Where(x => x.Count() > 1).ToList();
                foreach (var group in linkGroup)
                {
                    var tpLinks = group.ToList();
                    tpLinks = tpLinks.OrderBy(x => x.CreatedAt).ToList();
                    CommonTools.Log("重复链接{0}个 - {1}".FormatStr(tpLinks.Count - 1, tpLinks[0].Title));
                    for (int i = 1; i < tpLinks.Count; i++)
                    {
                        colLinkMain.DeleteOne(builderLinkMain.Eq(x => x._id, tpLinks[i].Id));
                        colLinkContent.DeleteOne(builderLinkContent.Eq(x => x.LinkId, tpLinks[i].Id));
                        colKey.UpdateOne(builderKey.Eq(x => x._id, new ObjectId(key.Id)), Builders<MediaKeywordMongo>.Update.Inc(x => x.WXLinkNum, -1));
                    }

                }
            }
        }


        public static void MoveKey(ObjectId oldProId, ObjectId newProId)
        {
            var builderCate = Builders<MediaKeywordCategoryMongo>.Filter;
            var filterCate = builderCate.Eq(x => x.ProjectId, oldProId);
            var colCate = MongoDBHelper.Instance.GetMediaKeywordCategory();
            var cates = colCate.Find(filterCate).ToList();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.In(x => x.CategoryId, cates.Select(x=>x._id));
            var colKeyMap = MongoDBHelper.Instance.GetMediaKeywordMapping();
            var keyMaps = colKeyMap.Find(filterKeyMap).ToList();
            List<MediaKeywordMappingMongo> newMaps = new List<MediaKeywordMappingMongo>();
            foreach (var map in keyMaps)
            {
                if (map.ProjectId == newProId)
                {
                    newMaps.Add(map);
                }
            }
            filterKeyMap = builderKeyMap.In(x => x._id, newMaps.Select(x => x._id));
            colKeyMap.DeleteMany(filterKeyMap);
            var delCates = newMaps.GroupBy(x => x.CategoryId).ToList();
            foreach (var item in delCates)
            {
                var cateId = item.Key;
                int num = -item.Count();
                var updateCate = Builders<MediaKeywordCategoryMongo>.Update.Inc(x => x.KeywordCount, num);
                colCate.UpdateOne(builderCate.Eq(x => x._id, cateId), updateCate);
            }
            //var keyIds = keyMaps.Select(x => x.KeywordId).ToList();
            //var builderKey = Builders<MediaKeywordMongo>.Filter;
            //var filterKey = builderKey.In(x => x._id, keyIds) & builderKey.Eq(x => x.WXBotStatus, 2);
            //var keys = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            //foreach (var map in keyMaps)
            //{
            //    if (keys.Find(x => x._id== map.KeywordId) != null)
            //    {
            //        newKeyMapIds.Add(map._id);
            //        CommonTools.Log("重复关键词：" + map.Keyword);
            //    }
            //    else
            //    {
            //        CommonTools.Log("新关键词：" + map.Keyword);
            //    }
            //}
            //filterKeyMap = builderKeyMap.In(x => x._id, newKeyMapIds);
            ////var updateKeyMap = Builders<MediaKeywordMappingMongo>.Update.Set(x => x.ProjectId, newProId);
            //colKeyMap.DeleteMany(filterKeyMap);
            
        }

        public static void CountPro(ObjectId proId)
        {
            var cates = MongoDBHelper.Instance.GetMediaKeywordCategory().Find(Builders<MediaKeywordCategoryMongo>.Filter.Eq(x => x.ProjectId, proId)).ToList();
            foreach (var cate in cates)
            {
                var keyIds = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(Builders<MediaKeywordMappingMongo>.Filter.Eq(x => x.CategoryId, cate._id)).Project(x => x.KeywordId).ToList();
                var linkCount = MongoDBHelper.Instance.GetMediaKeyword().Find(Builders<MediaKeywordMongo>.Filter.In(x => x._id, keyIds)).Project(x => x.WXLinkNum).ToList();
                Console.WriteLine("{0};{1};{2}".FormatStr(cate.Name, linkCount.Count,linkCount.Sum()));
            }
        }

        /// <summary>
        /// 修复微信链接
        /// </summary>
        public static void RepairWXLink(ObjectId projectId)
        {
            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.ProjectId, projectId) & builderKeyMap.Eq(x => x.CategoryId, ObjectId.Empty) & builderKeyMap.Eq(x => x.IsDel, false);
            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);
            int n = 0;
            var faild = new HashSet<string>();
            foreach (var key in keys)
            {
                n++;
                //if (n < 33)
                //{
                //    continue;
                //}
                if (key.Name == "圣罗兰包 一比一")
                {
                    int adf = 343;
                }
                var filterLink = builderLinkMain.Eq(x => x.KeywordId, key.Id);
                filterLink &= builderLinkMain.Gt(x => x.CreatedAt, new DateTime(2018, 10, 1));
                filterLink &= builderLinkMain.Regex(x => x.Url, "timestamp");
                var links = colLinkMain.Find(filterLink).Project(x => new
                {
                    Id = x._id,
                    Url = x.Url,
                    Title=x.Title,
                    WxAccount = x.Name,
                    WxNick = x.Nickname
                }).ToList();
                int i = 1;
                foreach (var link in links)
                {
                    if (string.IsNullOrEmpty(link.WxAccount)||faild.Contains(link.WxAccount))
                    {
                        i++;
                        continue;
                    }
                    
                    CommonTools.Log("[{0}/{1}]链接标题[{2}/{3}]：{4}".FormatStr(n,keys.Count,i,links.Count,link.Title));
                    CommonTools.Log("{0}\t{1}".FormatStr(link.WxNick, link.WxAccount));
                    #region 获取真实链接
                    string linkUrl = System.Web.HttpUtility.UrlEncode(link.Url);    //url编码后的链接地址
                    string trueUrl = "";
                    string appid = "33e8773009029e227badd9e8d7477daf";
                    int j = 0;
                    while (string.IsNullOrEmpty(trueUrl))
                    {
                        string reqUrl = "https://api.shenjian.io/?appid={0}&url={1}&account={2}".FormatStr(appid, linkUrl, link.WxAccount);
                        JObject trueUrlJson = JObject.Parse(WebApiInvoke.CreateGetHttpResponse(reqUrl));
                        try
                        {
                            trueUrl = trueUrlJson["data"]["article_origin_url"].ToString();
                            CommonTools.Log(string.Format("真实链接：{0}".FormatStr(trueUrl)));
                        }
                        catch (Exception ex)
                        {
                            CommonTools.Log(trueUrlJson["reason"].ToString());
                            Thread.Sleep(1000);
                            if (trueUrlJson["reason"].ToString() == "链接服务失败")
                                continue;
                            else
                                break;
                        }
                        j++;
                    }
                    #endregion
                    if (!string.IsNullOrEmpty(trueUrl))
                    {
                        colLinkMain.UpdateOne(builderLinkMain.Eq(x => x._id, link.Id), Builders<WxLinkMainMongo>.Update.Set(x => x.Url, trueUrl));
                    }
                    else
                    {
                        faild.Add(link.WxAccount);
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// 检查微信链接是否与关键词相关
        /// </summary>
        /// <param name="projectId">项目Id</param>
        public static void CheckWXLink(ObjectId projectId,string cateName="")
        {
            var fileName = "wxNicks.txt";
            var tpFile = File.OpenText(fileName);
            var wxNicks = tpFile.ReadToEnd().Split(';').ToList();
            tpFile.Close();
            var segmenter = new JiebaSegmenter();

            var builderKeyCate = Builders<MediaKeywordCategoryMongo>.Filter;
            var colKeyCate = MongoDBHelper.Instance.GetMediaKeywordCategory();
            var filterKeyCate = builderKeyCate.Eq(x => x.ProjectId, projectId) & builderKeyCate.Eq(x => x.Name, cateName);
            var cateId = colKeyCate.Find(filterKeyCate).Project(x => x._id).FirstOrDefault();

            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.IsDel, false);
            if (cateId == ObjectId.Empty)
            {
                filterKeyMap &= builderKeyMap.Eq(x => x.ProjectId, projectId) & builderKeyMap.Eq(x => x.CategoryId, ObjectId.Empty);
            }
            else
            {
                filterKeyMap &= builderKeyMap.Eq(x => x.CategoryId, cateId);
            }
            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);
            int n = 0;
            var faild = new HashSet<string>();
            foreach (var key in keys)
            {
                n++;
                //if (n < 15)
                //{
                //    continue;
                //}
                CommonTools.Log("[{0}/{1}]关键词：{2}".FormatStr(n, keys.Count, key.Name));
                if (key.Name == "圣罗兰包 一比一")
                {
                    int adf = 343;
                }
                var filterLink = builderLinkMain.Eq(x => x.KeywordId, key.Id);
                filterLink &= builderLinkMain.Gt(x => x.CreatedAt, new DateTime(2018, 12, 20));
                //filterLink &= builderLinkMain.Regex(x => x.Url, "timestamp");
                var links = colLinkMain.Find(filterLink).Project(x => new
                {
                    Id = x._id,
                    Url = x.Url,
                    Title = x.Title,
                    Des = x.Description,
                    WxAccount = x.Name,
                    WxNick = x.Nickname,
                    DelType = x.DelType,
                }).ToList();
                int i = 0;
                var builderLinkContent = Builders<WxLinkContentMongo>.Filter;
                var colLinkContent=MongoDBHelper.Instance.GetWXLinkContent();
                foreach (var link in links)
                {
                    i++;
                    CommonTools.Log("[{0}/{1}]链接标题[{2}/{3}]：{4}".FormatStr(n, keys.Count, i, links.Count, link.Title));
                    if (link.DelType != WXDelLinkType.Normal || string.IsNullOrEmpty(link.WxAccount))
                        continue;
                    if (link.Title.Contains("B11116"))
                    {
                        int df = 342;
                    }
                    if (wxNicks.Contains(link.WxNick))
                        continue;
                    var checkKeys = segmenter.Cut(key.Name).ToList();
                    string tp = checkKeys[0];
                    for (int j = 0; j < checkKeys.Count; )
                    {
                        if (link.Title.ToLower().Contains(checkKeys[j].ToLower()))
                        {
                            checkKeys.RemoveAt(j);
                            continue;
                        }
                        if (link.Des.ToLower().Contains(checkKeys[j].ToLower()))
                        {
                            checkKeys.RemoveAt(j);
                            continue;
                        }
                        if (tp == checkKeys[j])
                        {
                            j++;
                        }
                        else
                        {
                            tp = checkKeys[j];
                        }
                    }
                    var filterLinkContent = builderLinkContent.Eq(x => x.LinkId, link.Id);
                    bool isSave = true;
                    if (checkKeys.Count > 0)
                    {
                        tp = checkKeys[0];
                        var content = colLinkContent.Find(filterLinkContent).Project(x => x.Content).FirstOrDefault();
                        if (!string.IsNullOrEmpty(content))
                        {
                            for (int j = 0; j < checkKeys.Count; )
                            {
                                if (content.ToLower().Contains(checkKeys[j].ToLower()))
                                {
                                    checkKeys.RemoveAt(j);
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (checkKeys.Count > 0)
                            {
                                isSave = false;
                            }
                        }
                        else
                        {
                            CommonTools.Log("无正文，难以判断！");
                        }
                        if (isSave)
                            continue;
                        CommonTools.Log("链接无关，地址：{0}".FormatStr(link.Url));
                        CommonTools.Log("{0}\t{1}".FormatStr(link.WxNick, link.WxAccount));
                        Console.Write("请确认是否打开该链接（1/0)：");
                        char c = Console.ReadKey().KeyChar;
                        Console.Write('\n');
                        if (c == '1')
                        {
                            System.Diagnostics.Process.Start(link.Url);
                        }
                        Console.Write("请确认是否删除该链接（1/0)：");
                        c = Console.ReadKey().KeyChar;
                        Console.Write('\n');
                        if (c == '1')
                        {
                            //删除完全无关链接
                            filterLink = builderLinkMain.Eq(x => x._id, link.Id);
                            colLinkMain.DeleteOne(filterLink);
                            colLinkContent.DeleteOne(filterLinkContent);
                        }
                        else
                        {
                            wxNicks.Add(link.WxNick);
                        }
                    }
                    
                }
            }
            var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            var sw = new StreamWriter(fs);
            sw.Write(string.Join(";", wxNicks));
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 导出有问题的微信数据
        /// </summary>
        /// <param name="proId">项目Id</param>
        public static void ExportWXLink(ObjectId proId)
        {
            HashSet<ObjectId> usedIds = new HashSet<ObjectId>();
            string filePath = "F:\\errorLink_旧.csv";
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("关键词Id,关键词,链接Id,创建时间,链接标题,公众号,公众号呢称,公众号Id,是否更新公众号,链接状态,链接地址,是否更新链接地址,是否删除");
            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.ProjectId, proId) & builderKeyMap.Eq(x => x.CategoryId, ObjectId.Empty) & builderKeyMap.Eq(x => x.IsDel, false);
            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);
            int n = 0;
            var faild = new HashSet<string>();
            foreach (var key in keys)
            {
                n++;
                //if (n < 33)
                //{
                //    continue;
                //}
                CommonTools.Log("[{0}/{1}]关键词：{2}".FormatStr(n, keys.Count, key.Name));
                if (key.Name == "圣罗兰包 一比一")
                {
                    int adf = 343;
                }
                var filterLink = builderLinkMain.Eq(x => x.KeywordId, key.Id);
                filterLink &= builderLinkMain.Gte(x => x.CreatedAt, new DateTime(2018, 12, 7));
                //filterLink &= builderLinkMain.Regex(x => x.Url, "timestamp");
                var links = colLinkMain.Find(filterLink).Project(x => new
                {
                    Id = x._id,
                    Url = x.Url,
                    Title = x.Title,
                    Des = x.Description,
                    WxAccount = x.Name,
                    WxNick = x.Nickname,
                    WxId=x.NameId,
                    DelType = x.DelType,
                    CreatedAt = x.CreatedAt,
                    PostAt=x.PostTime,
                }).ToList();
                int i = 0;
                var builderLinkContent = Builders<WxLinkContentMongo>.Filter;
                var colLinkContent = MongoDBHelper.Instance.GetWXLinkContent();
                foreach (var link in links)
                {
                    i++;
                    if (!link.Url.Contains("timestamp") && !string.IsNullOrEmpty(link.WxAccount))
                        continue;
                    if (link.DelType != WXDelLinkType.Normal)
                        continue;
                    //写入链接信息
                    CommonTools.Log("[{0}/{1}]链接标题[{2}/{3}]：{4}".FormatStr(n, keys.Count, i, links.Count, link.Title));
                    //关键词Id,关键词,链接Id,创建时间,链接标题,公众号,公众号呢称,公众号Id,是否更新公众号,链接状态,链接地址,是否更新链接地址,是否删除
                    sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},0,{8},{9},0,0".FormatStr(key.Id.ToString(), key.Name, link.Id.ToString(), link.CreatedAt.ToShortDateString(),
                        link.Title.Replace(",", "&"), link.WxAccount, link.WxNick, link.WxId.ToString(), link.DelType, link.Url));

                    //if (usedIds.Contains(link.Id))
                    //    continue;
                    ////查找是否存在同样标题的链接
                    //filterLink = builderLinkMain.Eq(x => x.Title, link.Title) & builderLinkMain.Eq(x => x.Nickname, link.WxNick);
                    //var sameLinks = colLinkMain.Find(filterLink).Project(x => new
                    //{
                    //    Id = x._id,
                    //    Url = x.Url,
                    //    Title = x.Title,
                    //    Des = x.Description,
                    //    WxAccount = x.Name,
                    //    WxNick = x.Nickname,
                    //    WxId = x.NameId,
                    //    DelType = x.DelType,
                    //    Key=x.Keyword,
                    //    KeyId=x.KeywordId,
                    //    CreatedAt = x.CreatedAt,
                    //    PostAt = x.PostTime,
                    //}).ToList();
                    //如果不存在相同链接,且链接为正常链接,则跳过
                    //if (sameLinks.Count == 1 && !link.Url.Contains("timestamp") && !string.IsNullOrEmpty(link.WxAccount))
                    //    continue;

                    ////从新到旧依次删除，保留最老的一个
                    //sameLinks = sameLinks.OrderByDescending(x => x.CreatedAt).ToList();
                    //for (int j = 0; j < sameLinks.Count-1;)
                    //{
                    //    TimeSpan dis = sameLinks[j].PostAt - sameLinks[j + 1].PostAt;
                    //    if (dis.Days > 1)
                    //        sameLinks.RemoveAt(j);
                    //    else
                    //        j++;
                    //}
                    //if (sameLinks.Count == 1)
                    //    continue;

                    //if (key.Name == "Gucci 尾单" && i == 3)
                    //    continue;
                    //for (int j = 0; j < sameLinks.Count - 1; j++)
                    //{
                    //    filterLink = builderLinkMain.Eq(x => x._id, sameLinks[j].Id);
                    //    colLinkMain.DeleteOne(filterLink);
                    //    MongoDBHelper.Instance.GetWXLinkContent().DeleteOne(Builders<WxLinkContentMongo>.Filter.Eq(x => x.LinkId, sameLinks[j].Id));
                    //}

                    
                }
            }
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 导出百度关键词信息表
        /// </summary>
        /// <param name="proId"></param>
        public static void ExportBaiduKeyInfo()
        {
            //获取项目信息
            var pros=MongoDBHelper.Instance.GetIW2S_Projects().Find(Builders<IW2S_Project>.Filter.Eq(x=>x.IsDel,false)).Project(x=>new {
                Id=x._id,
                Name=x.Name
            }).ToList();
            pros.RemoveAll(x => x.Name.Contains("备份") || x.Name.Contains("克隆"));
            //获取分组信息
            var builderCate=Builders<Dnl_KeywordCategory>.Filter;
            var filterCate = builderCate.In(x => x.ProjectId, pros.Select(x => x.Id)) & builderCate.Eq(x => x.ParentId, ObjectId.Empty) & builderCate.Eq(x => x.IsDel, false);
            var cates = MongoDBHelper.Instance.GetDnl_KeywordCategory().Find(filterCate).Project(x => new
            {
                Id = x._id,
                Name = x.Name,
                ProjectId = x.ProjectId
            }).ToList();
            //获取关键词信息
            var builderMap = Builders<Dnl_KeywordMapping>.Filter;
            var filterMap = builderMap.In(x => x.CategoryId, cates.Select(x => x.Id)) & builderMap.Eq(x => x.IsDel, false);
            //filterMap &= builderMap.Eq(x => x.ParentCategoryId, ObjectId.Empty);     //不获取一级分组以下内关键词
            var maps = MongoDBHelper.Instance.GetDnl_KeywordMapping().Find(filterMap).Project(x => new
            {
                KeywordId = x.KeywordId,
                Name = x.Keyword,
                CategoryId = x.CategoryId,
                ProjectId = x.ProjectId,
            }).ToList();    //关键词映射列表

            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("项目");
            IRow rowHead = sheet.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("编号");
            rowHead.CreateCell(1).SetCellValue("名称");
            for (int i = 0; i < pros.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(i);
                row.CreateCell(1).SetCellValue(pros[i].Name);
            }
            sheet = workbook.CreateSheet("分组");
            rowHead = sheet.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("编号");
            rowHead.CreateCell(1).SetCellValue("名称");
            rowHead.CreateCell(2).SetCellValue("项目编号");
            rowHead.CreateCell(3).SetCellValue("项目名称");
            for (int i = 0; i < cates.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(i);
                row.CreateCell(1).SetCellValue(cates[i].Name);
                int idx = pros.FindIndex(x => x.Id == cates[i].ProjectId);
                if (idx != -1)
                {
                    row.CreateCell(2).SetCellValue(idx);
                    row.CreateCell(3).SetCellValue(pros[idx].Name);
                }
            }
            sheet = workbook.CreateSheet("关键词");
            rowHead = sheet.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("编号");
            rowHead.CreateCell(1).SetCellValue("名称");
            rowHead.CreateCell(2).SetCellValue("分组编号");
            rowHead.CreateCell(3).SetCellValue("分组名称");
            rowHead.CreateCell(4).SetCellValue("项目编号");
            rowHead.CreateCell(5).SetCellValue("项目名称");
            for (int i = 0; i < maps.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(i);
                row.CreateCell(1).SetCellValue(maps[i].Name);
                int idx = cates.FindIndex(x => x.Id == maps[i].CategoryId);
                if (idx != -1)
                {
                    row.CreateCell(2).SetCellValue(idx);
                    row.CreateCell(3).SetCellValue(cates[idx].Name);
                }
                idx = pros.FindIndex(x => x.Id == maps[i].ProjectId);
                if (idx != -1)
                {
                    row.CreateCell(4).SetCellValue(idx);
                    row.CreateCell(5).SetCellValue(pros[idx].Name);
                }
            }

            FileStream fs = new FileStream("F:\\关键词.xls", FileMode.OpenOrCreate);
            workbook.Write(fs);
            fs.Close();
            CommonTools.Log("导出完毕");
        }

        HttpHelper HttpHp = new HttpHelper();

        public static void DeleteExtraLink(ObjectId projectId, string cateName, int retainNum)
        {
            var builderKey = Builders<MediaKeywordMongo>.Filter;
            var colKey = MongoDBHelper.Instance.GetMediaKeyword();
            var builderLinkMain = Builders<WxLinkMainMongo>.Filter;
            var colLinkMain = MongoDBHelper.Instance.GetWXLinkMain();
            var builderLinkContent = Builders<WxLinkContentMongo>.Filter;
            var colLinkContent = MongoDBHelper.Instance.GetWXLinkContent();

            var builderKeyCate = Builders<MediaKeywordCategoryMongo>.Filter;
            var colKeyCate = MongoDBHelper.Instance.GetMediaKeywordCategory();
            var filterKeyCate = builderKeyCate.Eq(x => x.ProjectId, projectId) & builderKeyCate.Eq(x => x.Name, cateName);
            var cateId = colKeyCate.Find(filterKeyCate).Project(x => x._id).FirstOrDefault();

            var builderKeyMap = Builders<MediaKeywordMappingMongo>.Filter;
            var filterKeyMap = builderKeyMap.Eq(x => x.CategoryId, cateId) & builderKeyMap.Eq(x => x.IsDel, false);
            var keys = MongoDBHelper.Instance.GetMediaKeywordMapping().Find(filterKeyMap).Project(x => new
            {
                Id = x.KeywordId.ToString(),
                Name = x.Keyword
            }).ToList();

            keys = keys.DistinctBy(x => x.Name);
            var filterLink = builderLinkMain.In(x => x.KeywordId, keys.Select(x => x.Id));
            filterLink &= builderLinkMain.Gt(x => x.CreatedAt, new DateTime(2018, 12, 20));
            var links = colLinkMain.Find(filterLink).SortByDescending(x => x.PostTime).Project(x => new
            {
                Id = x._id,
                Tilte = x.Title,
                PublishAt = x.PostTime,
                KeywordId=x.KeywordId
            }).ToList();
            int delNum = links.Count - retainNum;
            for (int i = links.Count - 1; links.Count - i <= delNum; i--)
            {
                colLinkMain.DeleteOne(builderLinkMain.Eq(x => x._id, links[i].Id));
                colLinkContent.DeleteOne(builderLinkContent.Eq(x => x.LinkId, links[i].Id));
                colKey.UpdateOne(builderKey.Eq(x => x._id, new ObjectId(links[i].KeywordId)), Builders<MediaKeywordMongo>.Update.Inc(x => x.WXLinkNum, -1));
                CommonTools.Log("删除链接[{0}/{1}] - {2}".FormatStr(i - retainNum + 1, links.Count - retainNum, links[i].Tilte));
            }
        }

    }

    public class LinkInfo
    {
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Keyword { get; set; }
        public string KeywordId { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class IdAndName
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public ObjectId CateId { get; set; }
        public int Weight { get; set; }
    }

    /// <summary>
    /// 多文本摘要中链接信息
    /// </summary>
    public class MultiLinkInfo
    {
        public string Id { get; set; }
        /// <summary>
        /// 关键词占比
        /// </summary>
        public double KeywordPercent { get; set; }
        /// <summary>
        /// 链接影响力点比
        /// </summary>
        public double LinkInflPercent { get; set; }
        /// <summary>
        /// 最终权重
        /// </summary>
        public double TotalNum { get; set; }
    }



    
}
