using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using CSharpTest.Model;
using CSharpTest.Helper;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace CSharpTest.Tools
{
    public class CommonTools
    {
        /// <summary>
        /// 去除文本HTML代码中的HTML标签
        /// </summary>
        /// <param name="html">文本HTML代码</param>
        /// <returns></returns>
        public static string RemoveTextTag(string html)
        {
            //转换换行标签
            Regex reg = new Regex("</p>|<br>|</br>");
            html = reg.Replace(html, System.Environment.NewLine);
            //html = html.Replace("</p>", System.Environment.NewLine);

            //转换带属性的br标签，如：
            //<br style="max-width: 100%; color: rgb(115, 250, 121); font-size: 20px; line-height: 35.5556px; box-sizing: border-box !important; word-wrap: break-word !important;"  />
            int pos = -3;
            while (true)
            {
                pos = html.IndexOf("<br", pos + 3);
                if (pos != -1)
                {
                    html = html.Insert(pos, System.Environment.NewLine);
                }
                else
                {
                    break;
                }
            }
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            string content = node.InnerText;
            content = System.Web.HttpUtility.HtmlDecode(content);
            //去除多余空行
            content = RemoveSpaceLine(content);

            if (content.Length > 2)
                content = content.Substring(0, content.Length - 2);             //去除末尾换行
            return content;
        }

        /// <summary>
        /// 去除多余换行和空白行
        /// </summary>
        /// <param name="text">源文本</param>
        /// <returns></returns>
        public static string RemoveSpaceLine(string text)
        {
            text = text.Replace(" ", "");       //该处为普通空格
            text = text.Replace(" ", "");       //该处为将形如&nbsp;等字符转义后的空格
            //按段落拆分
            text = text.Replace("\r", "");
            var listStr = text.Split('\n').ToList();
            StringBuilder bu = new StringBuilder();
            foreach (var str in listStr)
            {
                if (!string.IsNullOrEmpty(str))
                    bu.Append(str + System.Environment.NewLine);
            }
            return bu.ToString();
        }

        /// <summary>
        /// 拆分为字符串Id列表
        /// </summary>
        /// <param name="idStr">源字符串</param>
        /// <returns></returns>
        public static List<string> GetIdListFromStr(string idStr)
        {
            var idArray = idStr.Split(';');
            return idArray.Where(x => !string.IsNullOrEmpty(x) && x != "undefined").ToList();
        }

        /// <summary>
        /// 拆分为ObjectId列表
        /// </summary>
        /// <param name="idStr">源字符串</param>
        /// <returns></returns>
        public static List<ObjectId> GetObjIdListFromStr(string idStr)
        {
            var idArray = idStr.Split(';');
            return idArray.Where(x => !string.IsNullOrEmpty(x) && x != "undefined").Select(x => new ObjectId(x)).ToList();
        }

        /// <summary>
        /// 输出时间日志
        /// </summary>
        /// <param name="msg">日志信息</param>
        public static void Log(string msg)
        {
            Console.WriteLine(DateTime.Now.ToString() + "  :  " + msg);
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<param name="useNum">是否包含数字，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，默认为不包含</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, string custom = null, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
    }
}
