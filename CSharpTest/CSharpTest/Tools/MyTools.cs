using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.RegularExpressions;
using AISSystem;

namespace CSharpTest.Tools
{
    public static class MyTools
    {
        /// <summary>
        /// 排除yande图包中的编号重复图片
        /// </summary>
        public static void ExcludeRepeatYande()
        {
            string path=@"K:\yande";
            var fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            List<string> exist = new List<string>();
            List<string> del = new List<string>();
            //yande 320131
            Regex reg = new Regex(@"yande (?<num>\d+?) ");
            foreach (var file in fileList)
            {
                string num = reg.Match(file).Groups["num"].Value;
                if (exist.Contains(num))
                {
                    Console.WriteLine(file);
                    del.Add(file);
                }
                else
                {
                    exist.Add(num);
                }
            }
            foreach (var file in del)
            {
                if (File.Exists(file)) File.Delete(file);
            }
        }

        /// <summary>
        /// 重命名yande图片
        /// </summary>
        public static void RenameYande()
        {
            string path = @"L:\yande";
            var fileList = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
            for (int i = 0; i < fileList.Count; i++)
            {
                string old = fileList[i];
                if (!old.Contains("yande.re")) continue;
                string newPath=old.Replace("yande.re","yande");
                FileInfo file = new FileInfo(old);
                try{
                file.CopyTo(newPath);
                }
                catch
                {
                    i--;
                    continue;
                }
                Console.WriteLine(file.Name);
                File.Delete(old);
            }
        }

        /// <summary>
        /// 重命名文件夹、文件名称
        /// </summary>
        public static void RemoveFileName()
        {
            string path = @"L:\H\图片\Cosplay\Flameworks";
            var fileList = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            var directoryList = Directory.GetDirectories(path);
            Regex reg = new Regex("\\(.{1,16}\\)");
            //foreach (var x in fileList)
            //{
            //    FileInfo file = new FileInfo(x);
            //    string oldName = file.Name;
            //    string str = reg.Match(oldName).Value;
            //    string newName = oldName.Replace(str, "").Trim();
            //    newName = Regex.Replace(newName, @"\..*", "");
            //    string newPath = file.DirectoryName + "\\" + newName+" "+str + file.Extension;
            //    file.MoveTo(newPath);
            //}
            foreach (var x in directoryList)
            {
                DirectoryInfo dir = new DirectoryInfo(x);
                if (!dir.Exists) continue;
                string oldName = dir.Name;
                string str = reg.Match(oldName).Value;
                if (string.IsNullOrEmpty(str)) continue;
                string newName = oldName.Replace(str, "").Trim();
                string newPath = path + "\\" + newName + " " + str;
                dir.MoveTo(newPath);
            }
        }
        public static void AddPassword(string dirPath, string password)
        {
            var fileList = Directory.GetFiles(dirPath, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach (var filePath in fileList)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                if (extension == ".txt")
                    continue;
                string newfileName = fileName + "[密码：{0}]".FormatStr(password);
                File.Move(filePath, dirPath + newfileName + extension);
            }
            CommonTools.Log("重命名完毕！");
        }
    }
}
