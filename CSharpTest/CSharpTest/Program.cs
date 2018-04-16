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
            string url = "http://localhost:2022/api/Blog/SaveExpert";
            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("id", null);
            postData.Add("userId", "58845bed1e5318078cb01b1a");
            postData.Add("name", "张风");
            postData.Add("industry", "航天");
            postData.Add("title", "中国工程院院士");
            postData.Add("abs", "中国航空学常务理事，中国航空工业集团公司科技委专职委员，航空科学技术与系统工程专家。");
            postData.Add("record", "杨凤田（1941.06.14 —）,飞机总体设计专家。生于辽宁省义县，原籍辽宁省义县。1964年毕业于哈尔滨军事工程学院（哈军工）。现任沈阳飞机设计研究所研究员。曾任西工大、北航、南航兼职教授");
            postData.Add("description", "杨凤田是歼8D/F总设计师，据杨凤田院士介绍，受油机技术是成熟的技术，属于战略性技术。作战飞机“腿要长”，就飞得远，就要空中受油。");
            postData.Add("achievement", "多年来，他主持的国家多项重点工程的研制，均已设计定型并批量装备部队。");
            postData.Add("refBook", "《凤舞蓝天--记中国工程院院士杨凤田》");
            postData.Add("level", "4.5");
            postData.Add("coverUrl", null);
            var response = WebApiInvoke.CreatePostHttpResponse(url, postData);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            string reStr = sr.ReadToEnd();
            Console.WriteLine(reStr);


            //string url = "http://localhost:2022/api/Blog/DelExpert?id=5ad4a2828bd04c0f2c066edd";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            //MyTools.SortDir(@"C:\新建文件夹 (2)");

            //var dirs = @"G:\123\我好期待音乐推荐\";
            //var files = Directory.GetFiles(dirs, "*.*", SearchOption.AllDirectories);

            //foreach (var x in files)
            //{
            //    Console.WriteLine(x);
            //    var name = Path.GetFileName(x);
            //    File.Move(x, dirs + name);
            //}

            Console.ReadKey();
        }

        

    }


}
