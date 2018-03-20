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
            //string url = "http://localhost:2022/api/News/SaveNews";
            //Dictionary<string, object> postData = new Dictionary<string, object>();
            //postData.Add("id", null);
            //postData.Add("userId", "58845bed1e5318078cb01b1a");
            //postData.Add("title", "测试标题2232");
            //postData.Add("sourceType", 0);
            //postData.Add("tags", "标签1;标签2");
            //postData.Add("content", "sdfjeifhskdfhojfilsdjflsdijfilejflsijfi;");
            //var response = WebApiInvoke.CreatePostHttpResponse(url, postData);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string reStr = sr.ReadToEnd();
            //Console.WriteLine(reStr);


            //string url = "http://localhost:2022/api/AnaPro/ReStartTagBindSearch?anaProCateId=5a445dc5f4b87d04984d4aec&sourceType=0";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            MyTools.SortDir(@"G:\新建文件夹 (2)\本子");
           

            Console.ReadKey();
        }

        

    }


}
