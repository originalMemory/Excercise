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

            var keys = new List<string> { "公益筹款人", "筹款人", "劝募人" };
            var key = keys.Skip(4).Take(10).ToList();
            if(key.Count>0)
                Console.WriteLine(key.First());

            Console.ReadKey();
        }

    }

}
