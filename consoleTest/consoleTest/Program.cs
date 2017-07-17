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
using AISSystem;

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //string keyword = "真爱梦想";
            //string url = "http://www.5118.com/seo/words/真爱梦想";
            //string html = WebApiInvoke.GetHtml(url);
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(html);
            //HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//span[@class=\"hoverToHide\"]");
            //foreach (var node in nodes)
            //{
            //    Console.WriteLine(node.InnerText);
            //}
            string pw = "123456";
            byte[] bytes = Encoding.UTF8.GetBytes(pw);
            string base64 = Convert.ToBase64String(bytes);
            Console.WriteLine(base64);
            Console.ReadKey();
        }

    }
}
