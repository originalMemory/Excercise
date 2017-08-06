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
            //D3force force = new D3force();
            //force.initializeNodes();
            
            var paths=Directory.GetFiles(@"E:\图片\临时\yande.re");
            string direct = @"E:\图片\临时\新建文件夹\";
            int i = 1;
            foreach (var path in paths)
            {
                string newname = Path.GetFileName(path.Replace("yande.re", "yande"));
                FileInfo file = new FileInfo(path);
                file.MoveTo(direct+newname);
                Console.WriteLine("[{0}/{1}]:{2}".FormatStr(i,paths.Length,newname));
                i++;
            }
            Console.WriteLine("重命名结束！");
            Console.ReadKey();
        }

    }
}
