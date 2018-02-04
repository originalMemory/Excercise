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
            //string url = "http://localhost:2022/api/ProCategory/InsertProjectMaDataCate";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //para.Add("userId", "58845bed1e5318078cb01b1a");
            //para.Add("id", "5a5ca4228e2bd623004f6170");
            ////para.Add("name", "项目管理组");
            ////para.Add("description", "测试");
            //para.Add("type", "0");
            //para.Add("projectIds", "59c1d4f5f4b87d02ac45c92e");
            //var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());


            //string url = "http://localhost:2022/api/AnaPro/ReStartTagBindSearch?anaProCateId=5a445dc5f4b87d04984d4aec&sourceType=0";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //var response = WebApiInvoke.CreateGetHttpResponse(url);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());

            var pros = MongoDBHelper.Instance.GetIW2S_Projects().Find(Builders<IW2S_Project>.Filter.Eq(x => x.IsDel, false)).SortByDescending(x => x.CreatedAt).Limit(10).Project(x => x._id).ToList();
            foreach (var item in pros)
            {
                DnlTools.ResetChartPreCompute(item, SourceType.Baidu);
            }

            Console.ReadKey();
        }

        

    }


}
