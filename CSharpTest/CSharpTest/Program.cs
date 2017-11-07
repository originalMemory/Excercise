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
            string url = "http://localhost:2022/api/Account/Regist";
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("uName", "213142");
            para.Add("uPwd1", "213142");
            para.Add("uPwd2", "213142");
            para.Add("email", "34234234234@qq.com");
            para.Add("ip", "119.253.58.170");
            var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            StreamReader sr = new StreamReader(response.GetResponseStream());
            Console.WriteLine(sr.ReadToEnd());


            Console.ReadKey();
        }

    }


}
