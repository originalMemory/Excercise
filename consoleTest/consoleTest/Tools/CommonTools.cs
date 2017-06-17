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
    }
}
