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
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using AISSystem;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //string url = "http://localhost:2022/api/Blog/SaveExpert";
            //Dictionary<string, object> postData = new Dictionary<string, object>();
            //postData.Add("id", null);
            //postData.Add("userId", "58845bed1e5318078cb01b1a");
            //postData.Add("name", "张风");
            //postData.Add("industry", "航天");
            //postData.Add("title", "中国工程院院士");
            //postData.Add("abs", "中国航空学常务理事，中国航空工业集团公司科技委专职委员，航空科学技术与系统工程专家。");
            //postData.Add("record", "杨凤田（1941.06.14 —）,飞机总体设计专家。生于辽宁省义县，原籍辽宁省义县。1964年毕业于哈尔滨军事工程学院（哈军工）。现任沈阳飞机设计研究所研究员。曾任西工大、北航、南航兼职教授");
            //postData.Add("description", "杨凤田是歼8D/F总设计师，据杨凤田院士介绍，受油机技术是成熟的技术，属于战略性技术。作战飞机“腿要长”，就飞得远，就要空中受油。");
            //postData.Add("achievement", "多年来，他主持的国家多项重点工程的研制，均已设计定型并批量装备部队。");
            //postData.Add("refBook", "《凤舞蓝天--记中国工程院院士杨凤田》");
            //postData.Add("level", "4.5");
            //postData.Add("coverUrl", null);
            //var response = WebApiInvoke.CreatePostHttpResponse(url, postData);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string reStr = sr.ReadToEnd();
            //Console.WriteLine(reStr);


            //string url = "http://localhost:2022/api/Media/GetLevelLinks?user_id=587b5379cba12d035c51ce23&categoryId=&projectId=59242f261037d47990e7b6d3&keywordId=593fa6c21037d43f1038a586&Title=&domain=&infriLawCode=&status=&page=0&pagesize=2";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            MyTools.SortDir(@"C:\下载\绯月");

            //Console.WriteLine(GetDistance(39.944094107166663, 116.27698527749999, 39.944652798500002, 116.27679663166667));
            //DnlTools.SearchNewrankWXAccount();
            Console.ReadKey();
        }

        private const double EARTH_RADIUS = 6378.137;
        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }  
    }


}
