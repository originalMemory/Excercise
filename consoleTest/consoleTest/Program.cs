using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using consoleTest.Model;
using consoleTest.Tools;
using System.IO;
using Microsoft.VisualBasic;

using NReadability;
using System.Net;


namespace consoleTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var transcoder = new NReadabilityTranscoder();
            string content;
            using (var wc = new WebClient())
            {
                content = wc.DownloadString("http://news.163.com/17/0522/16/CL294BVU000187VE.html");
            }
            bool success;
            string transcodedContent =
              transcoder.Transcode(content, out success);
            Console.WriteLine(transcodedContent);
            Console.ReadKey();
        }

    }
}
