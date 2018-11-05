using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTools.Helper
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 替换html中的特殊字符
        /// </summary>
        /// <param name="theString">需要进行替换的文本</param>
        /// <returns>替换完的文本</returns>
        public static string HtmlEncode(this string theString)
        {
            theString = theString.Replace(">", "&gt;");
            theString = theString.Replace("<", "&lt;");
            theString = theString.Replace(" ", "&nbsp;");
            theString = theString.Replace("\"", "&quot;");
            theString = theString.Replace("\'", "&#39;");
            theString = theString.Replace("\n", "<br/>");
            return theString;
        }

        /// <summary>
        /// 恢复html中的特殊字符
        /// </summary>
        /// <param name="theString">需要恢复的文本</param>
        /// <returns>恢复好的文本</returns>
        public static string HtmlDiscode(this string theString)
        {
            theString = theString.Replace("&gt;", ">");
            theString = theString.Replace("&lt;", "<");
            theString = theString.Replace("&nbsp;", " ");
            theString = theString.Replace("&quot;", "\"");
            theString = theString.Replace("&#39;", "\'");
            theString = theString.Replace("<br/>", "\n");
            theString = theString.Replace("<br>", "\n");
            theString = theString.Replace("&#091;", "[");
            theString = theString.Replace("&#093;", "]");
            theString = theString.Replace("&#093;", "·");
            return theString;
        }

        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="format">符合格式的字符串</param>
        /// <param name="objs">参数数组</param>
        /// <returns></returns>
        public static string FormatStr(this string format, params object[] objs)
        {
            return string.Format(format, objs);
        }

        /// <summary>
        /// 判断字符串是否为纯数字
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns></returns>
        public static bool IsNum(this string source)
        {
            foreach (char c in source)
            {
                if ((c < '0' || c > '9') && c != '.')
                    return false;
            }
            return true;
        }
    }
}
