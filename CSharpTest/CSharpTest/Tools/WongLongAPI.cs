using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AISSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharpTest.Tools
{
    public class WoLongApi
    {
        private const string Secret = "cscj3gci4yq3a1oaw94slplrizi5dzcj"; //卧龙提供Secret
        private const string AppKey = "9821352024";                       //卧龙提供AppKey


        public static WoLongApi Instance { get; } = new WoLongApi();


        /// <summary>
        ///     卧龙微博任务推送接口
        /// </summary>
        /// <param name="taskId">提交的任务 id（必须唯一）</param>
        /// <param name="keyword">若 type 为 1：推送的搜索关键词，可以重复推送，不同关键词以逗号分割 若 type 为 2：为用户昵称</param>
        /// <returns></returns>
        public JObject CallWlWeiboTaskAdd(int taskId, string keyword)
        {
            CommonTools.Log("访问卧龙微博任务推送接口...");
            var UrlFormat = "weibo/task/add";

            var woLongParameters = new Dictionary<string, object>
            {
                {"task_id", taskId},
                {"keyword", keyword},
                {"app_key", AppKey},
                {"t", CommonTools.GetTimestamp()}
            };

            var    sign      = GenerateWolongSign(woLongParameters, Secret, out string url);
            var    woLongUrl = UrlFormat + url + "&sign=" + sign;
            using (FileStream fs = new FileStream("tp.txt", FileMode.OpenOrCreate))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(woLongUrl);
                sw.Flush();
                sw.Close();
            }

            return CallWlApi(woLongUrl);
        }

        /// <summary>
        ///     卧龙微博任务获取接口
        /// </summary>
        /// <param name="taskId">提交的任务 id（必须唯一）</param>
        /// <param name="page">页数</param>
        /// <returns></returns>
        public JObject CallWlWeiboTaskGet(int taskId, int page)
        {
            CommonTools.Log("访问卧龙微博任务获取接口...");
            var UrlFormat = "weibo/task";

            var woLongParameters = new Dictionary<string, object>
            {
                {"task_id", taskId},
                {"page", page},
                {"app_key", AppKey},
                {"t", CommonTools.GetTimestamp()}
            };

            var sign      = GenerateWolongSign(woLongParameters, Secret, out var url);
            var woLongUrl = UrlFormat + url + "&sign=" + sign;

            return CallWlApi(woLongUrl);
        }

        /// <summary>
        ///     调用卧龙API
        /// </summary>
        /// <param name="apiUrl">api地址</param>
        /// <returns>Json对象</returns>
        private JObject CallWlApi(string apiUrl)
        {
            string baseUrl = "http://api.mts.wolongdata.com:8009/";
            string url = baseUrl + apiUrl;
            CommonTools.Log(url);
            var woLongJson = WebApiInvoke.GetHtml(url);

            if (woLongJson == null)
            {
                CommonTools.Log("访问卧龙接口超时！");
                var tryTimes = 0;
                while (woLongJson == null)
                {
                    tryTimes++;
                    if (tryTimes <= 3)
                    {
                        CommonTools.Log("开始第 {0} 次重试...".FormatStr(tryTimes));
                        woLongJson = WebApiInvoke.GetHtml(url);
                    }
                    else
                    {
                        break;
                    }
                }

                if (woLongJson == null) return new JObject();
            }

            var json = JsonConvert.DeserializeObject(woLongJson);
            return (JObject) JsonConvert.DeserializeObject(woLongJson);
        }

        /// <summary>
        ///     返回参数签名
        /// </summary>
        public static string GenerateWolongSign(Dictionary<string, object> param, string secret, out string url)
        {
            var paraStr = GetParaUrl(param, out url);
            var sign    = CommonTools.Md5Encoding(secret + paraStr + secret);
            return sign;
        }


        /// <summary>
        ///     拼接参数
        /// </summary>
        public static string GetParaUrl(Dictionary<string, object> param, out string url)
        {
            var orderParas = param.OrderBy(x => x.Key);
            var sb         = new StringBuilder(); // 带URL中的链接符
            var sb1        = new StringBuilder(); // 首尾相接，不带连接符
            var n          = 0;

            foreach (var p in orderParas)
            {
                sb1.Append(CommonTools.UrlEncoding(p.Key)).Append(CommonTools.UrlEncoding(p.Value.ToString()));
                if (n == 0)
                    sb.Append("?").Append(CommonTools.UrlEncoding(p.Key)).Append("=")
                        .Append(CommonTools.UrlEncoding(p.Value.ToString()));
                else
                    sb.Append("&").Append(CommonTools.UrlEncoding(p.Key)).Append("=")
                        .Append(CommonTools.UrlEncoding(p.Value.ToString()));
                n++;
            }

            url = sb.ToString();
            return sb1.ToString();
        }
    }
}