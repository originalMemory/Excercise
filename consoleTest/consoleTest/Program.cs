using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using consoleTest.Model;
using consoleTest.Tools;


namespace consoleTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string url = "https://dl.dmhy.org/2017/05/12/f798e55610cb7d7d6caa91d3cfc2f8187245c4de.torrent";
            string path = "E:\\";
            Torrent.Download(url, path);
            Console.ReadKey();
        }

    }
}
