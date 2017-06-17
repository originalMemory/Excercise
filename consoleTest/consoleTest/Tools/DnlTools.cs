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

using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

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
                            CommonTools.Log("域名收录量 - " + num);
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
                            CommonTools.Log("域名收录量 - " + x.DcNum);
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
        /// 重计算微信链接状态
        /// </summary>
        public static void ArrangeLink()
        {
            var builder = Builders<OldWeiXinLinkMongo>.Filter;
            var filter = builder.Empty;
            Console.Write("从多少数据开始：");
            string str = Console.ReadLine();
            int page = Convert.ToInt32(str) / 100;
            int pagesize = 100;
            var col = MongoDBHelper.Instance.GetOldWeiXinLink();
            var linkNum = col.Find(filter).Count();
            int nowNum = page * 100 + 1;
            while (true)
            {
                var queryLink = col.Find(filter).Project(x => new OldWeiXinLinkDto
                {
                    _id = x._id,
                    Content = x.Content,
                    GetTime = x.GetTime,
                    GetTimeNewest = x.GetTimeNewest,
                    GetTimePm = x.GetTimePm,
                    LikeNum = x.LikeNum,
                    LikeNumNewest = x.LikeNumNewest,
                    LikeNumPM = x.LikeNumPM,
                    LikeNumWeek = x.LikeNumWeek,
                    ContentLen = 0,
                    ReadNum = x.ReadNum,
                    ReadNumNewest = x.ReadNumNewest,
                    ReadNumPM = x.ReadNumPM,
                    ReadNumWeek = x.ReadNumWeek,
                    RealReadNum = x.RealReadNum,
                    RealReadNumPM = x.RealReadNumPM,
                    RealReadNumWeek = x.RealReadNumWeek,
                    Title = x.Title,
                    Url = x.Url,
                }).Skip(page * pagesize).Limit(pagesize).ToList();
                if (queryLink == null || queryLink.Count == 0)
                    break;
                foreach (var link in queryLink)
                {
                    CommonTools.Log("当前处理链接[{0}/{1}] - {2}".FormatStr(nowNum, linkNum, link.Title));
                    var readNums = new List<int> { link.ReadNum, link.ReadNumPM, link.ReadNumWeek, link.ReadNumNewest, link.RealReadNum, link.RealReadNumPM, link.RealReadNumWeek };
                    int maxReadNum = readNums.Max();
                    var likeNums = new List<int> { link.LikeNum, link.LikeNumPM, link.LikeNumWeek, link.LikeNumNewest };
                    int maxLikeNum = likeNums.Max();
                    var getTimes = new List<DateTime> { link.GetTime, link.GetTimeNewest, link.GetTimePm };
                    DateTime maxGetTime = getTimes.Max().AddHours(8);

                    string content = link.Content;
                    int length = 0;
                    bool isDel = false;
                    var filterUp = builder.Eq(x => x._id, link._id);
                    if (string.IsNullOrEmpty(content))
                    {
                        string html = WebApiInvoke.GetHtml(link.Url);
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);
                        HtmlNode node = doc.DocumentNode.SelectSingleNode("//div[@id=\"js_content\"]");     //定位正文位置

                        if (node != null)
                        {
                            content = CommonTools.RemoveTextTag(node.InnerHtml);
                            length = content.Length;
                        }
                        else
                        {
                            content = "";
                            if (html.Contains("该内容已被发布者删除") || html.Contains("该公众号已迁移"))
                            {
                                isDel = true;
                            }
                        }
                        var update = new UpdateDocument { { "$set", new QueryDocument { { "IsDelByAu", isDel }, { "ReadNum", maxReadNum }, { "LikeNum", maxLikeNum }, { "GetTime", maxGetTime }, { "Content", content }, { "ContentLen", length } } } };
                       col.UpdateOne(filterUp, update);
                    }
                    else
                    {
                        length = content.Length;
                        var update = new UpdateDocument { { "$set", new QueryDocument { { "IsDelByAu", isDel }, { "ReadNum", maxReadNum }, { "LikeNum", maxLikeNum }, { "GetTime", maxGetTime }, { "ContentLen", length } } } };
                       col.UpdateOne(filterUp, update);
                    }
                    nowNum++;
                }
                page++;
            }
        }
        
        /// <summary>
        /// 微信数据分析
        /// </summary>
        public static void AnalysizeWeiXinLink()
        {
            string baseUrl = System.AppDomain.CurrentDomain.BaseDirectory;

            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            var builderLink = Builders<WeiXinLinkMongo>.Filter;
            var WXlinks = new List<WeiXinLinkMongo>();

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
            RowHead0.CreateCell(7).SetCellValue("正文");
            RowHead0.CreateCell(8).SetCellValue("正文长度");
            RowHead0.CreateCell(9).SetCellValue("链接地址");
            RowHead0.CreateCell(10).SetCellValue("阅读量");
            RowHead0.CreateCell(11).SetCellValue("点赞量");
            RowHead0.CreateCell(12).SetCellValue("是否已被删除");
            int i = 1;
            string filename = "微信5月数据.xls";
            string path = baseUrl + @"\ExportFiles\" + filename;
            try
            {
                foreach (var key in queryKey)
                {
                    CommonTools.Log("当前获取关键词[{0}/{1}] - {2}".FormatStr(i, queryKey.Count, key.Keyword));
                    int page = 0, pagesize = 500;
                    var filterLink = builderLink.Eq(x => x.KeywordId, key._id.ToString());
                    DateTime startDate = new DateTime(2017, 5, 1);
                    DateTime endDate = new DateTime(2017, 6, 1);
                    filterLink &= builderLink.Gte(x => x.PostTime, startDate.AddHours(8));
                    var queryLink = MongoDBHelper.Instance.GetWeiXinLink().Find(filterLink);
                    int topLinkNum = (int)queryLink.Count();
                    int nowLinkNum = 0;
                    while (true)
                    {
                        var sw = new System.Diagnostics.Stopwatch();
                        sw.Start();
                        var tempLinks = MongoDBHelper.Instance.GetWeiXinLink().Find(filterLink).Skip(page * pagesize).Limit(pagesize).ToList();
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
            int k = 1;
            foreach (var link in WXlinks)
            {
                IRow row = linkSheet.CreateRow(k);
                row.CreateCell(0).SetCellValue(link._id.ToString());
                row.CreateCell(1).SetCellValue(link.Keyword);
                row.CreateCell(2).SetCellValue(link.KeywordId);
                row.CreateCell(3).SetCellValue(link.Name);
                row.CreateCell(4).SetCellValue(link.WXName);
                row.CreateCell(5).SetCellValue(link.PostTime.ToString());
                row.CreateCell(6).SetCellValue(link.Title);
                row.CreateCell(7).SetCellValue(link.Content);
                row.CreateCell(8).SetCellValue(link.ContentLen);
                row.CreateCell(9).SetCellValue(link.Url);
                row.CreateCell(10).SetCellValue(link.ReadNum);
                row.CreateCell(11).SetCellValue(link.LikeNum);
                bool isDel = link.IsDelByAu;
                if (link.ContentLen < 50)
                    isDel = true;
                row.CreateCell(12).SetCellValue(isDel);
                k++;
            }
            using (FileStream fileAna = new FileStream(path, FileMode.OpenOrCreate))
            {
                linkExcel.Write(fileAna);　　//创建Excel文件。
            }
            //linkExcel.Write(file);　　//创建Excel文件。

            int delNum = WXlinks.RemoveAll(x => x.ContentLen < 50 || x.IsDelByAu == true);

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

            int count = 0;
            foreach (var key in queryKey)
            {
                var links = WXlinks.FindAll(x => x.KeywordId == key._id.ToString());
                IRow row = sheet1.CreateRow(count + 1);
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
            var allName = WXlinks.Select(x => x.Name).ToList();
            allName = allName.Distinct().ToList();
            count = 0;
            foreach (var name in allName)
            {
                //导出所有分组信息
                var keys = new List<string>();
                int readNum = 0, likeNum = 0;
                var links = WXlinks.FindAll(x => x.Name == name);
                keys = links.Select(x => x.Keyword).Distinct().ToList();
                IRow row = sheet2.CreateRow(count + 1);
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

            string filename2 = "微信5月数据分析" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".xls";
            string path2 = baseUrl + @"\ExportFiles\" + filename2;
            using (FileStream fileAna = new FileStream(path2, FileMode.Create))
            {
                workBook.Write(fileAna);　　//创建Excel文件。
                fileAna.Close();
            }
        }

        public static void CreateWXLinkIndex()
        {
            string keyId = "593fa6c91037d43f1038a5c6";
            var builderLink = Builders<WeiXinLinkMongo>.Filter;
            var filterLink = builderLink.Eq(x => x.KeywordId, keyId);
            var col = MongoDBHelper.Instance.GetWeiXinLink();
            var num = col.Find(filterLink).Count();
            CommonTools.Log("本关键词对应链接数 - " + num);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var queryLink = col.Find(filterLink).Project(x => new WeiXinTimelinkDto
            {
                Id = x._id.ToString(),
                PublishTime = x.PostTime,
                Title = x.Title,
                //Content=x.Description,
                LinkUrl = x.Url,
                Name = x.Name,
                Keywords = x.Keyword
            }).ToList();
            sw.Stop();
            CommonTools.Log("第一种计算方法耗时 - {0}s".FormatStr(sw.Elapsed));
            var linkObjIds = queryLink.Select(x => new ObjectId(x.Id)).ToList();
            sw.Restart();
            var filterLink2 = builderLink.In(x => x._id, linkObjIds);
            var queryLink2 = col.Find(filterLink2).Project(x => new WeiXinTimelinkDto
            {
                Id = x._id.ToString(),
                PublishTime = x.PostTime,
                Title = x.Title,
                Content = x.Content,
                LinkUrl = x.Url,
                Name = x.Name,
                Keywords = x.Keyword
            }).ToList();
            sw.Stop();
            CommonTools.Log("第二种计算方法耗时 - {0}s".FormatStr(sw.Elapsed));
        }

        #region 微信图表计算
        /// <summary>
        /// 微信图表计算
        /// </summary>
        public void WeiXinChart()
        {
            //获取所有项目
            var filterPro = Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false);
            var queryPro = MongoDBHelper.Instance.GetIW2S_Projects().Find(filterPro).ToList();
            int i = 1;
            foreach (var pro in queryPro)
            {
                CommonTools.Log("当前计算项目[{0}/{1}] - {2}".FormatStr(i, queryPro.Count, pro.Name));
                if (pro.Name != "二次元音乐")
                {
                    i++;
                    continue;
                }
                i++;

                JavaScriptSerializer serializer = new JavaScriptSerializer();       //Json序列化与反序列化

                //获取图表数据
                var builderChart = Builders<PojectChartMongo>.Filter;
                var filterChart = builderChart.Eq(x => x.ProjectId, pro._id) & builderChart.Eq(x => x.Source, SourceType.Media);
                filterChart &= builderChart.Eq(x => x.Name, "默认");
                var colChart = MongoDBHelper.Instance.GetPojectChart();
                var queryChart = colChart.Find(filterChart).ToList();

                //获取项目内所有关键词组信息
                var cateInfos = WeiXinGetAllFenZhu(pro.UsrId.ToString(), pro._id.ToString());

                #region 折线图及饼图计算
                //获取项目内前8关键词组
                var cate8Ids = cateInfos.Take(8).Select(x => x.id).ToList();
                

                var lineData = WeiXinGetTimeLinkCount(string.Join(";", cate8Ids), pro._id.ToString(), null, null, 0, 10, 25, 1);
                string lineDataStr = serializer.Serialize(lineData).ToString();       //数据Json字符串

                cate8Ids.Remove(ObjectId.Empty.ToString());
                cate8Ids.Sort();

                //生成参数Json
                JObject lineFactorJson = new JObject();
                lineFactorJson.Add(new JProperty("categoryIds", string.Join(";", cate8Ids)));
                lineFactorJson.Add(new JProperty("startTime", null));
                lineFactorJson.Add(new JProperty("endTime", null));
                lineFactorJson.Add(new JProperty("percent", 0));
                lineFactorJson.Add(new JProperty("topNum", 10));
                lineFactorJson.Add(new JProperty("sumNum", 25));
                lineFactorJson.Add(new JProperty("timeInterval", 1));

                var lineChart = queryChart.Find(x => x.Type == ChartType.Line);
                if (lineChart != null) //判断保存方式
                {
                    var filterUp = builderChart.Eq(x => x._id, lineChart._id);
                    //更新数据
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "DataJson", lineDataStr }, { "FactorJson", lineFactorJson.ToString() }, { "CreatedAt", DateTime.Now.AddHours(8) } } } };
                    colChart.UpdateOne(filterUp, update);
                }
                else
                {
                    //创建数据
                    var chart = new PojectChartMongo
                    {
                        CreatedAt = DateTime.Now.AddHours(8),
                        DataJson = lineDataStr,
                        FactorJson = lineFactorJson.ToString(),
                        Name = "默认",
                        ProjectId = pro._id,
                        Type = ChartType.Line,
                        Source=SourceType.Media
                    };
                    colChart.InsertOne(chart);
                }
                CommonTools.Log("折线图计算完毕！");
                #endregion

                #region 气泡图计算
                //获取所有关键词分组Id
                var cateIds = cateInfos.Select(x => x.id).ToList();

                var bubbleData = WeiXinGetDomainStatis(string.Join(";", cateIds), pro._id.ToString());
                string bubbleDataStr = serializer.Serialize(bubbleData).ToString();       //数据Json字符串

                cateIds.Remove(ObjectId.Empty.ToString());
                cateIds.Sort();
                //生成参数Json
                JObject bubbleFactorJson = new JObject();
                bubbleFactorJson.Add(new JProperty("categoryIds", string.Join(";", cateIds)));

                var bubbleChart = queryChart.Find(x => x.Type == ChartType.Bubble);
                if (bubbleChart != null) //判断保存方式
                {
                    var filterUp = builderChart.Eq(x => x._id, bubbleChart._id);
                    //更新数据
                    var update = new UpdateDocument { { "$set", new QueryDocument { { "DataJson", bubbleDataStr }, { "FactorJson", bubbleFactorJson.ToString() }, { "CreatedAt", DateTime.Now.AddHours(8) } } } };
                    colChart.UpdateOne(filterUp, update);
                }
                else
                {
                    //创建数据
                    var chart = new PojectChartMongo
                    {
                        CreatedAt = DateTime.Now.AddHours(8),
                        DataJson = bubbleDataStr,
                        FactorJson = bubbleFactorJson.ToString(),
                        Name = "默认",
                        ProjectId = pro._id,
                        Type = ChartType.Bubble,
                        Source = SourceType.Media
                    };
                    colChart.InsertOne(chart);
                }
                CommonTools.Log("气泡图计算完毕！");
                #endregion


            }

            CommonTools.Log("本次计算完毕！\n");
        }

        /// <summary>
        /// 获取该项目下所有分组(词组文件夹列表)
        /// </summary>
        /// <param name="usr_id">用户ID</param>
        /// <param name="projectId">项目ID</param>
        /// <returns>关键词数组</returns>
        private List<GroupTree2Dto> WeiXinGetAllFenZhu(string usr_id, string projectId)
        {

            List<GroupTree2Dto> list = new List<GroupTree2Dto>();

            GroupTree2Dto result = new GroupTree2Dto();
            result.name = "所有词";
            //根目录ID默认为"000000000000000000000000"
            result.id = "000000000000000000000000";
            result.pId = "0";
            list.Add(result);


            var listGT = WeiXinGetCategoryTree2(usr_id, projectId, new ObjectId("000000000000000000000000"), list);
            if (listGT == null)
                listGT = list;

            return listGT;

        }

        /// <summary>
        /// 获取关键词数组
        /// </summary>
        /// <param name="usr_id">用户ID</param>
        /// <param name="projectId">项目ID</param>
        /// <param name="parentId">父ID</param>
        /// <param name="list">关键词数组</param>
        /// <returns>关键词数组</returns>
        private List<GroupTree2Dto> WeiXinGetCategoryTree2(string usr_id, string projectId, ObjectId parentId, List<GroupTree2Dto> list)
        {
            //获取次级词组名
            var builder = Builders<MediaKeywordCategoryMongo>.Filter;
            var filter = builder.Eq(x => x.ProjectId, new ObjectId(projectId));
            filter &= builder.Eq(x => x.IsDel, false);
            filter &= builder.Eq(x => x.ParentId, parentId);
            var TaskList = MongoDBHelper.Instance.GetMediaKeywordCategory().Find(filter).Project(x => new GroupTree2Dto
            {
                id = x._id.ToString(),
                pId = x.ParentId.ToString(),
                name = x.Name
            }).ToList();

            //若次级词组不存在，返回null，中断递归
            if (TaskList.Count == 0)
                return null;

            //递归调用GetCategoryTree2()，获取次级词组名和当前组内关键词
            foreach (var treedata in TaskList)
            {
                WeiXinGetCategoryTree2(usr_id, projectId, new ObjectId(treedata.id), list);
                // parent.children.Add(treedata);
                GroupTree2Dto gt = new GroupTree2Dto();
                gt.id = treedata.id;
                gt.pId = treedata.pId;
                gt.name = treedata.name;
                list.Add(gt);
            }

            return list;
        }

        //命中关键词域名分布图
        private List<WXDomainStatisDto> WeiXinGetDomainStatis(string categoryIds, string projectId)
        {
            var result = new List<WXDomainStatisDto>();
            if (string.IsNullOrEmpty(categoryIds) || string.IsNullOrEmpty(projectId))
            {
                return result;
            }

            ObjectId proObjId = new ObjectId(projectId);
            JavaScriptSerializer serializer = new JavaScriptSerializer();       //Json序列化与反序列化

            var cateIds = new List<string>();
            if (!string.IsNullOrEmpty(categoryIds))
            {
                cateIds = CommonTools.GetIdListFromStr(categoryIds);
                cateIds.Remove(ObjectId.Empty.ToString());
                cateIds.Sort();
            }
            /* 计算图表数据 */
            //获取项目内所有关键词Id
            var keyIds = new List<string>();
            var usrId = ObjectId.Empty;
            var groupBuilder = Builders<MediaKeywordMappingMongo>.Filter;
            var groupFilter = groupBuilder.Eq(x => x.ProjectId, new ObjectId(projectId)) & groupBuilder.Eq(x => x.IsDel, false);

            if (!string.IsNullOrEmpty(categoryIds))
            {
                var cateObjIds = categoryIds.Split(';').Select(x => new ObjectId(x)).ToList();
                //判断是否有分组
                if (cateObjIds.Count == 1 && cateObjIds[0].Equals(ObjectId.Empty))
                {
                    //无分组时获取所有关键词
                    groupFilter &= groupBuilder.Eq(x => x.CategoryId, ObjectId.Empty);
                }
                else
                {
                    //有分组时仅获取选定分组内关键词
                    cateObjIds.Remove(ObjectId.Empty);      //去除根结点
                    groupFilter &= groupBuilder.In(x => x.CategoryId, cateObjIds);

                }
                var groupCol = MongoDBHelper.Instance.GetMediaKeywordMapping();
                keyIds = groupCol.Find(groupFilter).Project(x => x.KeywordId).ToList().Select(x => x.ToString()).ToList();      //关键词Id组
                usrId = groupCol.Find(groupBuilder.Eq(x => x.ProjectId, proObjId)).Project(x => x.UserId).FirstOrDefault();      //用户Id
            }
            //获取项目内已删除的链接Id
            var builderLinkMap = Builders<Dnl_LinkMapping_Baidu>.Filter;
            var filterLinkMap = builderLinkMap.Eq(x => x.ProjectId, proObjId) & builderLinkMap.Eq(x => x.DataCleanStatus, (byte)2);
            var exLinkObjIds = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).Project(x => x.LinkId).ToList();       //项目中已删除的链接ID列表
            //获取项目内所有符合条件的链接
            var buiderLink = Builders<WeiXinLinkMongo>.Filter;
            var filterLink = buiderLink.In(x => x.KeywordId, keyIds) & buiderLink.Nin(x => x._id, exLinkObjIds);
            var querylink = MongoDBHelper.Instance.GetWeiXinLink().Find(filterLink).Project(x => new WeiXinLinkDto
            {
                _id = x._id.ToString(),
                KeywordId = x.KeywordId,
                PostTime = x.PostTime,
                ReadNum = x.ReadNum,
                LikeNum = x.LikeNum,
                Title = x.Title,
                ContentLen = x.ContentLen,
                Name = x.Name
            }).ToList();

            //如果关键词大于30个，去除只有一个关键词且文本长度小于50的链接
            if (keyIds.Count > 30)
            {
                var filterKey = Builders<MediaKeywordMongo>.Filter.In(x => x._id, keyIds.Select(x => new ObjectId(x)));
                var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).Project(x => new
                {
                    Id = x._id.ToString(),
                    Keyword = x.Keyword
                }).ToList();
                for (int i = 0; i < querylink.Count; i++)
                {
                    foreach (var key in queryKey)
                    {
                        if (querylink[i].Title.Contains(key.Keyword))
                        {
                            querylink[i].Keywords.Add(key.Keyword);
                        }
                    }
                }
                querylink.RemoveAll(x => x.Keywords.Count <= 1 && x.ContentLen < 50);
            }

            //按公众号分组
            var linkByName = querylink.GroupBy(x => x.Name);
            foreach (var links in linkByName)
            {
                var stastic = new WXDomainStatisDto
                {
                    Name = links.Key,
                    Count = links.Count(),
                };
                int hotNum = 0;
                var tempLinks = links.ToList();
                stastic.KeywordTotal = tempLinks.Select(x => x.KeywordId).Distinct().Count();     //获取公众号涉及到的所有关键词数
                tempLinks = tempLinks.DistinctBy(x => x.LinkUrl);
                foreach (var link in tempLinks)
                {
                    hotNum += link.LikeNum * 12 + link.ReadNum;
                }
                stastic.HotNum = hotNum;
                stastic.PublishRatio = 100;
                result.Add(stastic);
            }

            if (result == null || result.Count == 0)
            {
                return result;
            }
            List<string> domainNameList = result.Select(x => x.Name).Distinct().ToList();

            var domainCatBuilder = Builders<IW2S_DomainCategoryData>.Filter;
            var domainCatFilter = domainCatBuilder.Eq(x => x.UsrId, usrId) & domainCatBuilder.In(x => x.DomainName, domainNameList);
            var domainCategoryDatas = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas().Find(domainCatFilter).Project(x => new { DomainCategoryId = x.DomainCategoryId, DomainName = x.DomainName }).ToList().DistinctBy(x => x.DomainName);
            //判断是否有私有公众号分组
            if (domainCategoryDatas.Count == 0)
            {
                usrId = ObjectId.Empty;
                domainCatFilter = domainCatBuilder.Eq(x => x.UsrId, usrId) & domainCatBuilder.In(x => x.DomainName, domainNameList);
                domainCategoryDatas = MongoDBHelper.Instance.GetIW2S_DomainCategoryDatas().Find(domainCatFilter).Project(x => new { DomainCategoryId = x.DomainCategoryId, DomainName = x.DomainName }).ToList().DistinctBy(x => x.DomainName);
            }
            DomainCategoryInfo dicdomainCategoryData = new DomainCategoryInfo();
            dicdomainCategoryData.Domain = new List<string>();
            dicdomainCategoryData.DomainCategoryId = new List<string>();
            dicdomainCategoryData.DomainCategoryName = new List<string>();
            foreach (var domainCategoryData in domainCategoryDatas)
            {
                if (!dicdomainCategoryData.Domain.Contains(domainCategoryData.DomainName))
                {
                    dicdomainCategoryData.Domain.Add(domainCategoryData.DomainName);
                    dicdomainCategoryData.DomainCategoryId.Add(domainCategoryData.DomainCategoryId.ToString());
                    var filter2 = Builders<IW2S_DomainCategory>.Filter.Eq(x => x._id, domainCategoryData.DomainCategoryId);
                    string v = MongoDBHelper.Instance.GetIW2S_DomainCategorys().Find(filter2).Project(x => x.Name).FirstOrDefault();
                    dicdomainCategoryData.DomainCategoryName.Add(v);
                }
            }
            foreach (var r in result)
            {
                if (dicdomainCategoryData.Domain.Contains(r.Name))
                {
                    int index = dicdomainCategoryData.Domain.IndexOf(r.Name);
                    r.DomainCategoryId = dicdomainCategoryData.DomainCategoryId[index];
                    r.DomainCategoryName = dicdomainCategoryData.DomainCategoryName[index];

                }
                else
                {
                    r.DomainCategoryId = null;
                    r.DomainCategoryName = "未分组";
                }
            }
            return result;
        }

        /// <summary>
        /// 有效链接统计图
        /// </summary>
        /// <param name="categoryIds">关键词分组ID,多个用;相连</param>
        /// <param name="prjId">项目ID,多个用;相连</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="percent">显示百分比以上的节点</param>
        /// <param name="topNum">标记前多少位</param>
        /// <param name="sumNum">饼图统计Top数/param>
        /// <param name="timeInterval">坐标点时间间隔</param>
        /// <returns></returns>
        private TimeLinkCountDto WeiXinGetTimeLinkCount(string categoryIds, string prjId, string startTime, string endTime, int percent, int topNum, int sumNum, int timeInterval)
        {
            DateTime tpStart = new DateTime();
            DateTime tpEnd = new DateTime();
            DateTime.TryParse(startTime, out tpStart);
            DateTime.TryParse(endTime, out tpEnd);

            TimeLinkCountDto result = new TimeLinkCountDto();

            List<ObjectId> cateObjIds = new List<ObjectId>();
            var cateIds = new List<string>();
            //拆分categoryId，转为ObjectId数组
            if (!string.IsNullOrEmpty(categoryIds))
            {
                cateObjIds = categoryIds.Split(';').Select(x => new ObjectId(x)).ToList();
                cateIds = cateObjIds.Select(x => x.ToString()).ToList();
                cateIds.Remove(ObjectId.Empty.ToString());
                cateIds.Sort();
            }

            if (string.IsNullOrEmpty(prjId))
            {
                return null;
            }
            var proObjId = new ObjectId(prjId);

            //判断是否为根分组
            bool cateIsRoot = false;
            if (cateObjIds.Count == 1 && cateObjIds[0].Equals(ObjectId.Empty))
            {
                cateIsRoot = true;
            }
            //多个分组时剔除根分组
            cateObjIds.Remove(ObjectId.Empty);

            int years = 3;      //图表时间范围，按天的数据截止时间，按周和按月是5年
            switch (timeInterval)
            {
                case 1:
                    years = 0;
                    break;
                case 7:
                    years = 5;
                    break;
                case 30:
                    years = 5;
                    break;
                default:
                    break;
            }

            var groupBuilder = Builders<MediaKeywordMappingMongo>.Filter;
            var builder = Builders<WeiXinLinkMongo>.Filter;

            //获取关键词列表
            var keyIds = new List<string>();      //关键词列表
            var groupFilter = groupBuilder.Eq(x => x.ProjectId, proObjId) & groupBuilder.Eq(x => x.IsDel, false);
            var groupCol = MongoDBHelper.Instance.GetMediaKeywordMapping();
            var keyToCate = new Dictionary<string, string>();
            /* 判断是否有分组
             * 有则使用原有分组信息
             * 无则仅建立所有词一组数据 */
            if (cateIsRoot)
            {
                groupFilter &= groupBuilder.Eq(x => x.CategoryId, ObjectId.Empty);
            }
            else
            {
                //从分组中获取所有关键词Id
                groupFilter &= groupBuilder.In(x => x.CategoryId, cateObjIds);
            }
            var TaskList = groupCol.Find(groupFilter).Project(x => new
            {
                KeywordId = x.KeywordId.ToString(),
                CategoryId = x.CategoryId.ToString()
            }).ToList();
            foreach (var x in TaskList)
            {
                if (!keyIds.Contains(x.KeywordId) && !keyToCate.ContainsKey(x.KeywordId))
                {
                    keyIds.Add(x.KeywordId);
                    keyToCate.Add(x.KeywordId, x.CategoryId);
                }
            }

            //获取项目内已删除的链接Id
            var builderLinkMap = Builders<Dnl_LinkMapping_Baidu>.Filter;
            var filterLinkMap = builderLinkMap.Eq(x => x.ProjectId, proObjId) & builderLinkMap.Eq(x => x.DataCleanStatus, (byte)2);
            var exLinkObjIds = MongoDBHelper.Instance.GetDnl_LinkMapping_Baidu().Find(filterLinkMap).Project(x => x.LinkId).ToList();       //项目中已删除的链接ID列表

            //获取发表时间
            var filterLink = builder.In(x => x.KeywordId, keyIds) & builder.Ne(x => x.PostTime, DateTime.MinValue);
            filterLink &= builder.Nin(x => x._id, exLinkObjIds);
            var queryDatas = MongoDBHelper.Instance.GetWeiXinLink().Find(filterLink).Project(x => new WeiXinLinkDto
            {
                PublishTime = x.PostTime,
                KeywordId = x.KeywordId,
                Title = x.Title,
                //Content = x.Content,
                Description = x.Description
            }).ToList();

            ////如果关键词大于30个，去除只有一个关键词且文本长度小于50的链接
            //if (keyIds.Count > 30)
            //{
            //    var filterKey = Builders<MediaKeywordMongo>.Filter.In(x => x._id, keyIds.Select(x => new ObjectId(x)));
            //    var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).Project(x => new
            //    {
            //        Id = x._id.ToString(),
            //        Keyword = x.Keyword
            //    }).ToList();
            //    for (int i = 0; i < queryDatas.Count; i++)
            //    {
            //        foreach (var key in queryKey)
            //        {
            //            if (queryDatas[i].Title.Contains(key.Keyword) || queryDatas[i].Content.Contains(key.Keyword))
            //            {
            //                queryDatas[i].Keywords.Add(key.Keyword);
            //            }
            //        }
            //    }
            //    queryDatas.RemoveAll(x => x.Keywords.Count <= 1 && x.Content.Length < 50);
            //}

            //获取包含分组ID的发布时间信息
            List<LinkStatus> linkList = new List<LinkStatus>();
            foreach (var x in queryDatas)
            {

                //DateTime tmpDt = new DateTime();
                //DateTime.TryParse(x.PublishTime, out tmpDt);
                int i = keyIds.IndexOf(x.KeywordId);
                while (i != -1)
                {
                    LinkStatus v = new LinkStatus();
                    //v.PublishTime = tmpDt;
                    if (!cateIsRoot)
                    {
                        v.CategoryId = keyToCate[x.KeywordId];
                    }
                    else
                    {
                        //无分组以空定义为所有分组
                        v.CategoryId = "000000000000000000000000";
                    }
                    v.Title = x.Title;
                    v.Description = x.Description;
                    v.PublishTime = x.PublishTime;
                    linkList.Add(v);
                    i = keyIds.IndexOf(x.KeywordId, i + 1);
                }
            }

            //删除异常时间 如0001-01-01与2063-23-12等时间，并排序
            linkList = linkList.Where(x => x.PublishTime > new DateTime(1753, 1, 09)).Where(x => x.PublishTime <= DateTime.Now).OrderByDescending(x => x.PublishTime).ToList();

            //建立时间坐标
            List<DateTime> xCoordinate = new List<DateTime>();
            //int i = 1;
            if (linkList.Count > 0)
            {
                DateTime now = linkList[0].PublishTime;
                DateTime end = new DateTime();
                if (years == 0)
                {
                    end = linkList.Last().PublishTime;
                }
                else
                {
                    end = now.AddYears(-years);
                }
                while (now >= end)
                {
                    xCoordinate.Add(now);
                    now = now.AddDays(-timeInterval);
                }
            }
            xCoordinate.Reverse();
            result.Times = xCoordinate;

            //获取起止时间位置
            int xStart;
            int xEnd;
            if (string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                for (int i = 0; i < xCoordinate.Count; i++)
                {
                    if (xCoordinate[i] <= tpStart) { xStart = i; }
                    if (xCoordinate[i] <= tpEnd) { xEnd = i; }
                }
            }

            //将发布时间依分组拆分
            List<CategoryList> categoryList = new List<CategoryList>();
            if (!cateIsRoot)
            {
                foreach (var x in cateObjIds)
                {
                    CategoryList v = new CategoryList();
                    v.PublishTime = new List<DateTime>();
                    v.CategoryId = x.ToString();
                    categoryList.Add(v);
                }

                //获取分组名并分配到数据中去
                var namefilter = Builders<MediaKeywordCategoryMongo>.Filter.In(x => x._id, cateObjIds);
                var nameList = MongoDBHelper.Instance.GetMediaKeywordCategory().Find(namefilter).Project(x => new
                {
                    Name = x.Name,
                    CategoryId = x._id.ToString()
                }).ToList();
                foreach (var x in nameList)
                {
                    CategoryList cat = categoryList.Find(s => s.CategoryId.Equals(x.CategoryId));
                    cat.CategoryName = x.Name;
                }
            }
            else
            {
                var cat = new CategoryList
                {
                    CategoryId = "000000000000000000000000",
                    CategoryName = "所有词",
                    PublishTime = new List<DateTime>()
                };
                categoryList.Add(cat);
            }

            //获取各分组内数据的发布时间
            foreach (var x in linkList)
            {
                CategoryList cat = categoryList.Find(s => s.CategoryId.Equals(x.CategoryId));
                cat.PublishTime.Add(x.PublishTime);
            }

            List<LineData> lineData = new List<LineData>();

            //top数据
            List<TopData> topData = new List<TopData>();

            //遍历数组，获取不同分组的数据
            foreach (var categoryData in categoryList)
            {
                LineData link = new LineData();
                link.name = categoryData.CategoryName;

                List<int> linkCounts = new List<int>();
                if (categoryData.PublishTime.Count > 0)
                {
                    DateTime now = linkList[0].PublishTime;
                    DateTime end = new DateTime();
                    if (years == 0)
                    {
                        end = linkList.Last().PublishTime;
                    }
                    else
                    {
                        end = now.AddYears(-years);
                    }
                    while (now >= end)
                    {
                        linkCounts.Add(categoryData.PublishTime.Where(x => x <= now && x > now.AddDays(-timeInterval)).Count());
                        now = now.AddDays(-timeInterval);
                    }
                }
                else
                {
                    continue;
                }
                //将链接数倒序
                linkCounts.Reverse();

                link.LinkCount = linkCounts;
                lineData.Add(link);

                //将坐标添加到临时数据列表中
                List<DateTime> temp = new List<DateTime>();
                for (int i = 0; i < xCoordinate.Count; i++)
                {
                    TopData v = new TopData();
                    v.name = categoryData.CategoryName;
                    v.CategoryId = categoryData.CategoryId;
                    v.X = xCoordinate[i];
                    v.Y = linkCounts[i];
                    topData.Add(v);
                }

            }

            //获取top数据及自动摘要节点

            if (string.IsNullOrEmpty(startTime) && string.IsNullOrEmpty(endTime))
            {
                topData = topData.Where(x => x.X > linkList[0].PublishTime.AddYears(-1)).ToList().OrderByDescending(x => x.Y).ToList<TopData>();

            }
            else
            {
                topData = topData.Where(x => x.X > tpStart).Where(x => x.X < tpEnd).OrderByDescending(x => x.Y).ToList<TopData>();
            }
            //获取限定数量的摘要时间节点
            List<TopData> tempSum = new List<TopData>();
            if (sumNum > 0)
            {
                tempSum = topData.Take(sumNum).ToList();
            }
            else
            {
                tempSum = topData.Take(1).ToList();
            }
            topData = topData.Take(topNum).ToList();

            List<SumData> sumData = new List<SumData>();        //摘要
            //获取摘要节点
            for (var i = 0; i < tempSum.Count; i++)
            {
                SumData sum = new SumData();
                sum.Y = tempSum[i].Y;
                sum.X = tempSum[i].X;
                sum.CategoryName = tempSum[i].name;
                sum.CategoryId = tempSum[i].CategoryId;
                sumData.Add(sum);
            }
            //依节点查询数据库，生成摘要
            var jieba = new JiebaHelper();
            for (var i = 0; i < sumData.Count; i++)
            {
                DateTime time = sumData[i].X;     //当前时间节点
                string source = "";
                foreach (var x in linkList)
                {
                    if (x.PublishTime <= time && x.PublishTime > time.AddDays(-timeInterval))
                        if (x.CategoryId.Equals(sumData[i].CategoryId))
                            source += x.Title + "。" + System.Environment.NewLine + x.Description + "。" + System.Environment.NewLine;
                }
                var tempStr = jieba.GetSummary(sumData[i].CategoryName, time.ToString(), prjId, source);
                if (tempStr.Count > 0 && tempStr[0].Length > 40)
                {
                    tempStr[0] = tempStr[0].Substring(0, 39);
                    tempStr[0] += "…";
                }
                if (tempStr.Count > 0)
                    sumData[i].Summary = tempStr[0];
            }
            result.Sum = sumData;

            //在percent大于0时，获取最大值，将不高于最大值percent百分比的值设为0,topData值删除
            List<TopData> delList = new List<TopData>();
            if (percent > 0)
            {
                int maxCount = topData[0].Y;
                int limit = maxCount * percent / 100;
                for (int i = 0; i < lineData.Count; i++)
                {
                    for (int j = 0; j < lineData[i].LinkCount.Count; j++)
                    {
                        if (lineData[i].LinkCount[j] < limit) lineData[i].LinkCount[j] = 0;
                    }
                }
                foreach (var x in topData)
                {
                    if (x.Y < limit) delList.Add(x);
                }
                foreach (var x in delList)
                {
                    topData.Remove(x);
                }
                //for (int i = 0; i < topData.Count; i++)
                //{
                //    if (topData[i].Y < limit) topData[i].Y = 0;
                //}
            }

            //将top数据分配到各组中去
            for (int i = 0; i < lineData.Count; i++)
            {
                lineData[i].topData = new List<TopData>();
                foreach (var x in topData)
                {
                    if (lineData[i].name.Equals(x.name))
                    {
                        lineData[i].topData.Add(x);
                    }
                }
            }

            result.LineDataList = lineData;
            return result;
        }

        #endregion

    }

    
}
