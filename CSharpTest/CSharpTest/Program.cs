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
            //string url = "http://localhost:11763/api/HtmlEdit/InsertNewsHtml";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //para.Add("title", "测试标题");
            //para.Add("sourceType", "0");
            //para.Add("tags", "测试，临时");
            //para.Add("content", "森远股份分手顶替有封建时代村夺有夺");
            //var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());


            //string url = "http://localhost:2022/api/AnaPro/ReStartTagBindSearch?anaProCateId=5a445dc5f4b87d04984d4aec&sourceType=0";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            MyTools.SortDir(@"C:\baidudown\绯月");
           

            Console.ReadKey();
        }

        

    }


}
