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
            //string url = "http://localhost:2022/api/Keyword/RecoverKeyword";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //para.Add("userId", "58845bed1e5318078cb01b1a");
            //para.Add("id", "");
            //para.Add("projectId", "59c1d4f5f4b87d02ac45c92e");
            //var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());


            //string url = "http://localhost:2022/api/AnaPro/ReStartTagBindSearch?anaProCateId=5a445dc5f4b87d04984d4aec&sourceType=0";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            WebClient wc = new WebClient();
            string upload_file_url = "http://localhost:2022/api/Export/UpLoadFile";
            string path = @"E:\00.jpg";
            byte[] sendData = System.Text.Encoding.UTF8.GetBytes(path);
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            wc.Headers.Add("ContentLength", sendData.Length.ToString());
            byte[] recData = wc.UploadFile(upload_file_url, "POST", path);
            var success = (Encoding.GetEncoding("GB2312").GetString(recData));
            Console.WriteLine(success);

            Console.ReadKey();
        }

        
    }


}
