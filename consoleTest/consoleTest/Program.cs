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
using NReadability;

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            DnlTools tools = new DnlTools();
            tools.TestGSSearch("耶稣", "2016-11-01", "2016-11-17");
            Console.ReadKey();
        }

    }
}
