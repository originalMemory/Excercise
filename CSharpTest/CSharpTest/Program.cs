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

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //D3force force = new D3force();
            //force.initializeNodes();
            //DnlTools dnl = new DnlTools();
            //dnl.DelUnuseLink();

            //string userName = "test";
            //string pw = "123";
            //VipMaTools.InsertUser(userName, pw);

            string url = "http://localhost:2012/api/AnaPro/GetLinkReport";
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("userId", "58845bed1e5318078cb01b1a");
            data.Add("tagCateIds", "59aa841fde93dc37647b7692");
            data.Add("anaProCateId", "59aa8236de93dc41ec3136c9");
            data.Add("tagIds", "59aa839ede93dc37647b7691");
            data.Add("projectIds", "588712927c08250b487fcfd1");
            //data.Add("proParaIds", null);
            //data.Add("paraLevel", "0");
            data.Add("sourceType", "0");
            //data.Add("startTime", null);
            //data.Add("endTime", null);
            //data.Add("percent", "0");
            //data.Add("topNum", "15");
            //data.Add("sumNum", "10");
            //data.Add("timeInterval", "1");
            var resp = WebApiInvoke.CreatePostHttpResponse(url, data);
            string htmlCharset = "utf-8";
            Encoding htmlEncoding = Encoding.GetEncoding(htmlCharset);
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream(), htmlEncoding);
            Console.WriteLine(sr.ReadToEnd());

            //DnlTools.ExtractContent("http://politics.people.com.cn/n1/2017/0908/c1024-29524528.html");
            //DnlTools.ExportWordFreqency("598e893df4b87d0b5055ccb6;598c3729f4b87d0c387c7ad1;598c3635f4b87d0c387c797e;5987b6e4f4b87d07107956af;596d801a237cbd0e08f87fd5");
            Console.ReadKey();
        }

    }

}
