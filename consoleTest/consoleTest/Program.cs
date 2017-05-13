using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using consoleTest.Model;
using consoleTest.Tools;
using System.IO;
using Microsoft.VisualBasic;


namespace consoleTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string str1 = "茉語星夢&自由字幕組】[路人女主的养成方法♭/Saenai Heroine no Sodatekata Fla";
            string str2 = Strings.StrConv(str1, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese);
            Console.WriteLine(str2);
            Console.ReadKey();
        }

    }
}
