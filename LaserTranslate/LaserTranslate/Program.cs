using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace LaserTranslate
{
    class Program
    {
        static int LEN = 101;
        static void Main(string[] args)
        {
            var curDir=Directory.GetCurrentDirectory();
            var filePaths = Directory.GetFiles(curDir, "*.txt");
            int i=0;
            foreach (var filePath in filePaths)
            {
                i++;
                Console.WriteLine(string.Format("当前处理文件[{0}/{1}]：{2}", i, filePaths.Count(), Path.GetFileName(filePath)));
                FileStream fs = new FileStream(filePath, FileMode.Open);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                string data = sr.ReadToEnd();
                sr.Close();
                fs.Close();
                string startStr = "02 80 CE 00 B0 65 00";
                //按指定要求分割文本
                var dataList = Regex.Split(data,startStr).ToList();
                if (dataList.Count == 0)
                {
                    Console.WriteLine("该文件无完整数据，跳过！");
                    continue;
                }
                if (dataList[0].Length < (LEN * 2 + 3) * 3)
                    dataList.RemoveAt(0);
                if (dataList[dataList.Count - 1].Length < (LEN * 2 + 3) * 3)
                    dataList.RemoveAt(dataList.Count - 1);

                //写入csv
                var newFilePath = Path.GetFileNameWithoutExtension(filePath) + ".csv";
                fs = new FileStream(newFilePath, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write("angle/deg");

                for (int j = 0; j < dataList.Count; j++)
                {
                    sw.Write(string.Format(",{0}",j+1));
                }
                sw.WriteLine();
                var result = new List<List<int>>();
                for (int j = 0; j < dataList.Count; j++)
                {
                    var tp = dataList[j].Trim().Split(' ');
                    var list = new List<int>(LEN);
                    for (int k = 0; k < LEN*2; k += 2)
                    {
                        //大端模式
                        list.Add(Convert.ToInt32(tp[k + 1] + tp[k], 16));
                    }
                    result.Add(list);
                }
                for (int j = 0; j < LEN; j++)
                {
                    sw.Write((40 + j));
                    for (int k = 0; k < result.Count; k++)
                    {
                        sw.Write(string.Format(",{0}", result[k][j]));
                    }
                    sw.WriteLine();
                }
                sw.Close();
                fs.Close();
            }
            Console.WriteLine("转换完成！");
            Console.ReadKey();
        }
    }
}
