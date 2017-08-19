﻿using MongoDB.Bson;
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

namespace CSharpTest.Tools
{
    public class DnlTools
    {
        /// <summary>
        /// 计算百度链接数
        /// </summary>
        public void CountLinkNum()
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
        public void ComputeDCNum()
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
        public void AnalysizeWeiXinLink()
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

            var builderLink = Builders<WXLinkMainMongo>.Filter;
            var WXlinks = new List<WXLinkMainMongo>();

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
        public void GetWeiXinLinkContent()
        {
            string baseUrl = System.AppDomain.CurrentDomain.BaseDirectory;

            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            var builderMainLink = Builders<WXLinkMainMongo>.Filter;
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
                        //var contentLinks = new List<WXLinkContentMongo>();
                        //foreach (var x in tempLinks)
                        //{
                        //    CommonTools.Log("当前计算链接[{0}/{1}]\t标题 - {2}".FormatStr(computeNum, topLinkNum, x.Title));
                        //    //获取原来保存的网页源码和正文
                        //    var filterHtml = builderLink.Eq(s => s.KeywordId, x.KeywordId) & builderLink.Eq(s => s.Url, x.Url);
                        //    var sw2 = new System.Diagnostics.Stopwatch();
                        //    sw2.Start();
                        //    string html = colLink.Find(filterHtml).Project(s => s.Html).FirstOrDefault();
                        //    sw2.Stop();
                        //    var temp = new WXLinkContentMongo
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
        public void ComputeWeiXinName()
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
                var builderMainLink = Builders<WXLinkMainMongo>.Filter;
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
        public void ResetWeiXinSearchRange()
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
        public void GetWXTop200NameComment()
        {
            var filterKey = Builders<MediaKeywordMongo>.Filter.Empty;
            var queryKey = MongoDBHelper.Instance.GetMediaKeyword().Find(filterKey).ToList();
            var builderMainLink = Builders<WXLinkMainMongo>.Filter;
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
        public void TestGSSearch(string keyword, string startTime = "2016-11-01", string endTime = "2017-07-01",int startNum=0)
        {
            //搜索关键词
            string baseUrl = "http://open.gsdata.cn/";
            string linkUrl = baseUrl + "api/wx/opensearchapi/content_keyword_search";       //关键词获取文章接口地址
            string appid = "JVEvKn7ghegw984neooX";
            string appkey = "n0TWOaX9gta1dpfVF07hpkKr2";
            GSDataSDK api = new GSDataSDK(appid, appkey);           //接口函数
            Dictionary<string, object> postData = new Dictionary<string, object>();                 //post参数
            postData.Add("keyword", keyword);
            postData.Add("sortname", "likenum");
            postData.Add("sort", "desc");
            postData.Add("start", startNum);
            postData.Add("startdate", startTime);
            postData.Add("enddate", endTime);
            string LinkStr = api.Call(postData, linkUrl);       //调用接口，获取返回数据
            Console.WriteLine(LinkStr);
        }

        /// <summary>
        /// 抓取5118拓展词
        /// </summary>
        /// <param name="keyword">关键词</param>
        public void Craw5118(string keyword)
        {
            string url = "http://www.5118.com/seo/words/{0}".FormatStr(keyword);
            string html = WebApiInvoke.GetHtml(url);
            //string keyword = "真爱梦想";
            //string url = "http://www.5118.com/seo/words/真爱梦想";
            //string html = WebApiInvoke.GetHtml(url);
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//span[@class=\"hoverToHide\"]");
            //foreach (var node in nodes)
            //{
            //    Console.WriteLine(node.InnerText);
            //}
            string pw = "123456";
            byte[] bytes = Encoding.UTF8.GetBytes(pw);
            string base64 = Convert.ToBase64String(bytes);
            Console.WriteLine(base64);
        }

        /// <summary>
        /// 修复因服务器搬迁导致的图片无法显示
        /// </summary>
        public void repairImg()
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
        public void DelUser(List<string> userEmailList)
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
        public void ExportUngroupDomain()
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
            var links = new List<IW2S_Timelevel1linkDto>();
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
                var link = new IW2S_Timelevel1linkDto
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
        public void ImportDomain(string path, ObjectId userObjId)
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
        public void ExportExcel(string tableHtml)
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
        /// 删除无效罗拉
        /// </summary>
        public void DelUnuseLink()
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
    }

    
}