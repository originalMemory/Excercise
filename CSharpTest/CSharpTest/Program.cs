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


namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string url = "http://localhost:2022/api/Keyword/DelLinkShareComment";
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("id", "5a084a77c37a3c28cc0a7ba3");
            para.Add("userId", "58845bed1e5318078cb01b1a");
            //para.Add("replyId", "5a084a77c37a3c28cc0a7ba3");
            //para.Add("content", "测试回复");
            //para.Add("level", "1");
            var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            Console.WriteLine(sr.ReadToEnd());


            Console.ReadKey();
        }

    }


}
