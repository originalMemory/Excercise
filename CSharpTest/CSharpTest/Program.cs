using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpTest.Model;
using CSharpTest.Tools;
using CSharpTest.Helper;
using System.IO;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Driver;
using IWSData.Model;

using NReadability;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JiebaNet.Segmenter;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
//using AISSystem;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using MongoDB.Driver.Builders;

//using JiebaNet.Analyser;
//using JiebaNet.Segmenter.PosSeg;


namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //string url = "http://localhost:2022/api/Blog/SaveExpert";
            //Dictionary<string, object> postData = new Dictionary<string, object>();
            //postData.Add("id", null);
            //postData.Add("coverUrl", null);
            //var response = WebApiInvoke.CreatePostHttpResponse(url, postData);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string reStr = sr.ReadToEnd();
            //Console.WriteLine(reStr);


            //string linkUrl = "https://mp.weixin.qq.com/s?src=3&timestamp=1536644778&ver=1&signature=pl4vJAFcu*2F4ZIkp8Ho4nT9LuFTKrEQHIPuGBRQFn1YwGFX2kVQQcXGESn55HIPzVWLz4V79092QTu00E7ZapJHdrq6QB*pM8L01N0W49HdPdeCfAdKG0tlMFpfUCfqS5AZ-BAHpsrarU0wG2HGQ3Tw4GXMbnieBl48TVx7OYs=";
            //linkUrl = HttpUtility.UrlEncode(linkUrl);
            //string account="erciy66";
            //string appid="33e8773009029e227badd9e8d7477daf";
            //string url = "https://api.shenjian.io/?appid={0}&url={1}&account={2}".FormatStr(appid, linkUrl, account);
            //var result = WebApiInvoke.CreateGetHttpResponse(url);
            //Console.WriteLine(result);



            //DnlTools.FilterWXLink("5b547bcdf4b87d0c88a54ab2");
            //DnlTools.CountWXLinkNum(new ObjectId("5c62bf39eaf66804a822f66d"), "Gucci");
            //DnlTools.CountPro(new ObjectId("5c62bf39eaf66804a822f66d"));
            //DnlTools.CheckWXLink(new ObjectId("5c62bf39eaf66804a822f66d"), "tangle teezer");
            //DnlTools.ExportWXLink(new ObjectId("5c62bf39eaf66804a822f66d"));
            //DnlTools.DeleteExtraLink(new ObjectId("5c62bf39eaf66804a822f66d"), "Parajumpers", 54);

//            CommonTools.Log(WoLongApi.Instance.CallWlWeiboTaskAdd( 1, "beatsaber").ToString());
//              CommonTools.Log(WoLongApi.Instance.CallWlWeiboTaskGet(1,0).ToString());
//            var ex = new List<string>() { "整理" };
//            MyTools.SortDir(@"C:\下载\绯月", ex);

            var query = new QueryDocument();
            var ex = new List<String> {"祖娅纳惜百度百科", "祖娅纳惜现场"};
            var bson = new BsonDocument();
            bson.Add("$regex", new BsonRegularExpression("/娅纳惜/"));
            bson.Add("$nin", new BsonArray(ex));
//            query.Add("Keyword", bson);
            query.Add("Keyword", "祖娅纳惜百度百科");
            var col = MongoDBHelper.Instance.GetDnl_Keyword();
            var r = col.Find(query).ToList();
            r.ForEach(x=> Console.WriteLine(x.Keyword));

        //Console.WriteLine(str);
        Console.ReadKey();
        }


        
        

        static TreeNode FindNode(TreeNode root, int value)
        {
            if (root == null)
            {
                return null;
            }
            if (root.value == value)
            {
                return root;
            }
            else
            {
                var r = FindNode(root.child, value);
                if (r == null)
                {
                    r = FindNode(root.brother, value);
                }
                return r;
            }
        }

        public class TreeNode
        {
            public TreeNode child = null;
            public TreeNode brother = null;
            public int value;
            public TreeNode(int v)
            {
                this.value = v;
            }
        }
        static public int Height(TreeNode root)
        {
            if (root == null)
            {
                return 0;
            }
            int left = 0, right = 0;
            if (root.child != null)
            {
                left = Height(root.child);
            }
            if (root.brother != null)
            {
                right = Height(root.brother);
            }
            return (left >= right) ? left+1 : right;
        }

        static string GetNewRankSearchData(string url, Dictionary<string, object> postData)
        {
            string appkey = "108bf3090c0747feac154d685";
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Console.WriteLine("appKey:{0}".FormatStr(appkey));
            //Console.WriteLine("url:{0}".FormatStr(url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            request.Headers.Add("Key", appkey);
            //发送POST数据
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in postData.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, postData[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, postData[key]);
                    i++;
                }
            }
            byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            string[] value = request.Headers.GetValues("Content-Type");
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string resultStr = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return resultStr;
        }

        //存放所有抓取的代理
        public static List<proxy> masterPorxyList = new List<proxy>();
        //代理IP类
        public class proxy
        {
            public string ip;

            public string port;
            public int speed;

            public proxy(string pip, string pport, int pspeed)
            {
                this.ip = pip;
                this.port = pport;
                this.speed = pspeed;
            }


        }
        //抓去处理方法
        static void getProxyList(object pageIndex)
        {

            string urlCombin = "http://www.xicidaili.com/wt/" + pageIndex.ToString();
            string catchHtml = catchProxIpMethord(urlCombin, "UTF8");


            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(catchHtml);


            HtmlNode table = doc.DocumentNode.SelectSingleNode("//div[@id='wrapper']//div[@id='body']/table[1]");

            HtmlNodeCollection collectiontrs = table.SelectNodes("./tr");



            for (int i = 0; i < collectiontrs.Count; i++)
            {
                HtmlAgilityPack.HtmlNode itemtr = collectiontrs[i];


                HtmlNodeCollection collectiontds = itemtr.ChildNodes;
                //table中第一个是能用的代理标题，所以这里从第二行TR开始取值
                if (i > 0)
                {
                    HtmlNode itemtdip = (HtmlNode)collectiontds[3];

                    HtmlNode itemtdport = (HtmlNode)collectiontds[5];

                    HtmlNode itemtdspeed = (HtmlNode)collectiontds[13];

                    string ip = itemtdip.InnerText.Trim();
                    string port = itemtdport.InnerText.Trim();


                    string speed = itemtdspeed.InnerHtml;
                    int beginIndex = speed.IndexOf(":", 0, speed.Length);
                    int endIndex = speed.IndexOf("%", 0, speed.Length);

                    int subSpeed = int.Parse(speed.Substring(beginIndex + 1, endIndex - beginIndex - 1));
                    //如果速度展示条的值大于90,表示这个代理速度快。
                    if (subSpeed > 90)
                    {
                        proxy temp = new proxy(ip, port, subSpeed);

                        masterPorxyList.Add(temp);
                        Console.WriteLine("当前是第:" + masterPorxyList.Count.ToString() + "个代理IP");
                    }

                }


            }

        }

        //抓网页方法
        static string catchProxIpMethord(string url, string encoding)
        {

            string htmlStr = "";
            try
            {
                if (!String.IsNullOrEmpty(url))
                {
                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream datastream = response.GetResponseStream();
                    Encoding ec = Encoding.Default;
                    if (encoding == "UTF8")
                    {
                        ec = Encoding.UTF8;
                    }
                    else if (encoding == "Default")
                    {
                        ec = Encoding.Default;
                    }
                    StreamReader reader = new StreamReader(datastream, ec);
                    htmlStr = reader.ReadToEnd();
                    reader.Close();
                    datastream.Close();
                    response.Close();
                }
            }
            catch { }
            return htmlStr;
        }

        static bool yanzhen(string ipStr, int port)
        {
            try
            {
                HttpWebRequest Req;
                HttpWebResponse Resp;
                WebProxy proxyObject = new WebProxy(ipStr, port);// port为端口号 整数型
                Req = WebRequest.Create("http://www.baidu.com/s?wd=ip&ie=utf-8&tn=94523140_hao_pg") as HttpWebRequest;
                Req.Proxy = proxyObject; //设置代理
                Req.Timeout = 1000;   //超时
                Resp = (HttpWebResponse)Req.GetResponse();
                Encoding bin = Encoding.GetEncoding("UTF-8");
                using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), bin))
                {
                    string str = sr.ReadToEnd();
                    if (str.Contains(ipStr))
                    {
                        Resp.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }


}
