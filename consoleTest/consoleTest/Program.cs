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
            string str = "http://www.2dhgame.org/s.asp?k=%CE%D2%B5%C4%CA%D5%B2%D8%BC%D0&f=&c=&paixu=2&page=4";
            string str2 = HttpUtility.UrlDecode(str, Encoding.GetEncoding("gbk"));
            Console.WriteLine(str2);
            Console.ReadKey();
        }

    }
}
