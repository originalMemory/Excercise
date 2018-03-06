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

            //MyTools.SortDir(@"C:\新建文件夹 (2)\新建文件夹 (2)");

            string url = "http://www.9moe.com/search.php?";
            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("step", "2");
            para.Add("method", "AND");
            para.Add("sch_area", "0");
            para.Add("s_type", "forum");
            para.Add("f_fid", "41");
            para.Add("orderway", "lastpost");
            para.Add("asc", "DESC");
            para.Add("keyword", "イケない人妻た");
            //para.Add("keyword", "イケない人妻た");
            CookieCollection cookies = new CookieCollection();
            cookies.Add(new Cookie("2ed4e_ck_info", "%2F%09", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_ipstate", "1520300450", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_lastpos", "index", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_lastvisit", "63656%091520339059%09%2Findex.php%3F", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_ol_offset", "4268", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_online", "deleted", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_skinco", "default", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_threadlog", "%2C67%2C102%2C41%2C57%2C", "/", "www.9moe.com"));
            cookies.Add(new Cookie("2ed4e_winduser", "BgRSXFQAOw4PD1dUA1dTVwVRUAVQVwFbAwlRB1BTUldUBwICCwRVbA", "/", "www.9moe.com"));
            cookies.Add(new Cookie("Hm_lvt_f83dc734066b108cb0068c6118d230ce", "1518485765", "/", ".9moe.com"));
            cookies.Add(new Cookie("Hm_lvt_f83dc734066b108cb0068c6118d230ce", "1518485765", "/", "www.9moe.com"));
            cookies.Add(new Cookie("Hm_lpvt_f83dc734066b108cb0068c6118d230ce", "1520339061", "/", "www.9moe.com"));
            cookies.Add(new Cookie("PHPSESSID", "3gio291ar6j7i1kfib85msc107", "/", "www.9moe.com"));
            string html = WebApiInvoke.GetHtml(url, "gbk", 1, para, cookies);
            Console.WriteLine(html);
            Console.ReadKey();
        }

        

    }


}
