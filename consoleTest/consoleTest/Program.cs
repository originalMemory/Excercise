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
using System.Web;

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var emailList = new List<string>();
            emailList.Add("353702907@qq.com");
            emailList.Add("dersenf@163.com");
            DnlTools tool = new DnlTools();
            tool.DelUser(emailList);
            Console.ReadKey();
        }

    }
}
