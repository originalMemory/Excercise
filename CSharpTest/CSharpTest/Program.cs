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
            //DnlTools.GetLinkReference("598e893df4b87d0b5055ccb6", 0);
            D3force force = new D3force("test.json", 900 / 2, 600 / 2);
            force.StartCompute();
            Console.ReadKey();
        }

    }

}
