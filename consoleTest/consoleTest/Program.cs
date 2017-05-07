using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Text.RegularExpressions;
using cPlusPlusTest.Model;

using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Net;

using JiebaNet.Analyser;
using JiebaNet.Segmenter;

namespace cPlusPlusTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "    测试    实验    ";
            Console.WriteLine(str + "a");
            string str2 = str.Trim();
            Console.WriteLine(str2 + "a");
            

            Console.ReadKey();
        }


    }
}
