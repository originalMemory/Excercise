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
using System.Net.Mail;
using System.Configuration;
using System.Security.Cryptography;
using System.Web;
using AISSystem;

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
        ///  获取时间戳，从1970年1月1日到现在的秒数
        /// </summary>
        public static int GetTimestamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            var cstName  = (int)timeSpan.TotalSeconds;
            return cstName;
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="timestamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddSeconds(timestamp);
        }

        /// <summary>
        /// URL 编码
        /// </summary>
        public static string UrlEncoding(string para)
        {
            string str = HttpUtility.UrlEncode(para);
            if (string.IsNullOrEmpty(str)) return null;

            if (para.Contains("/"))
            {
                str = str.Replace("%2f", "/");
            }

            return !para.Equals(str) ? str.ToUpper() : str;
        }


        /// <summary>
        /// 计算参数拼接后的MD5值
        /// </summary>
        public static string Md5Encoding(string para)
        {
            var md5      = MD5.Create();
            var bytes    = Encoding.UTF8.GetBytes(para);
            var md5Bytes = md5.ComputeHash(bytes);
            // var s = Convert.ToBase64String(md5Bytes);
            var s = string.Empty;
            foreach (var b in md5Bytes)
            {
                s += b.ToString("X2");
            }

            return s.ToLower();
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

        /// <summary>
        /// 发送错误邮件
        /// </summary>
        /// <param name="proName">出错程序名称</param>
        /// <param name="stack">出错代码位置</param>
        /// <param name="msg">错误内容</param>
        /// <param name="recivers">收件人列表</param>
        /// <param name="ccs">抄送人列表</param>
        public static void SendErrorMail(string proName, string stack, string msg, List<string> recivers = null, List<string> ccs = null)
        {
            var emailAcount = ConfigurationManager.AppSettings["ems_usr_acc"];
            var emailPassword = ConfigurationManager.AppSettings["ems_usr_pwd"];
            MailMessage message = new MailMessage();
            //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
            MailAddress fromAddr = new MailAddress(emailAcount);
            message.From = fromAddr;
            //设置收件人,可添加多个,添加方法与下面的一样
            if (recivers != null)
            {
                foreach (var item in recivers)
                {
                    message.To.Add(item);
                }
            }
            else
            {
                message.To.Add("kdi1994@163.com");
            }
            //设置抄送人
            if (ccs != null)
            {
                foreach (var item in ccs)
                {
                    message.CC.Add(item);
                }
            }
            //设置邮件标题
            message.Subject = "BOT错误";
            //设置邮件内容
            string content = @"{0}程序出错！
错误位置：{1}
错误原因：{2}";
            content = content.FormatStr(proName, stack, msg);
            message.Body = content;
            //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看,下面是QQ的
            var smtp = ConfigurationManager.AppSettings["ems_smtp"];
            int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["ems_smtp_port"]);
            SmtpClient client = new SmtpClient(smtp, smtpPort);
            //设置发送人的邮箱账号和密码
            client.Credentials = new System.Net.NetworkCredential(emailAcount, emailPassword);
            //启用ssl,也就是安全发送
            client.EnableSsl = true;
            //发送邮件
            client.Send(message);
        }
    }
}
