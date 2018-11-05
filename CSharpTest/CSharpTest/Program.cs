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
            //postData.Add("userId", "58845bed1e5318078cb01b1a");
            //postData.Add("name", "张风");
            //postData.Add("industry", "航天");
            //postData.Add("title", "中国工程院院士");
            //postData.Add("abs", "中国航空学常务理事，中国航空工业集团公司科技委专职委员，航空科学技术与系统工程专家。");
            //postData.Add("record", "杨凤田（1941.06.14 —）,飞机总体设计专家。生于辽宁省义县，原籍辽宁省义县。1964年毕业于哈尔滨军事工程学院（哈军工）。现任沈阳飞机设计研究所研究员。曾任西工大、北航、南航兼职教授");
            //postData.Add("description", "杨凤田是歼8D/F总设计师，据杨凤田院士介绍，受油机技术是成熟的技术，属于战略性技术。作战飞机“腿要长”，就飞得远，就要空中受油。");
            //postData.Add("achievement", "多年来，他主持的国家多项重点工程的研制，均已设计定型并批量装备部队。");
            //postData.Add("refBook", "《凤舞蓝天--记中国工程院院士杨凤田》");
            //postData.Add("level", "4.5");
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
            //DnlTools.CountWXLinkNum(new ObjectId("5bc9bc97eaf66807248ee4c8"));
            //DnlTools.CountPro(new ObjectId("5bc9bc97eaf66807248ee4c8"));
            //DnlTools.ExportWXLink(new ObjectId("5bc9bdc7eaf66807248ee54c"));
            //DnlTools dnl = new DnlTools();
            //dnl.ImportWXLink("F:\\errorLink_旧.csv");
            //DnlTools.CheckWXLink(new ObjectId("5bc9bdc7eaf66807248ee54c"));
            //DnlTools.RepairWXLink(new ObjectId("5bc9bc97eaf66807248ee4c8"));
            //DnlTools.MoveKey(new ObjectId("5bc9bc97eaf66807248ee4c8"), new ObjectId("5bc9bdc7eaf66807248ee54c")); 
            //var ex = new List<string>() { "游戏" };
            //MyTools.SortDir(@"C:\下载\绯月", ex);

            string str="window.__ssr_data = JSON.parse(\"{\"detail\":{\"post_data\":{\"item_id\":\"6553160850834718980\",\"uid\":\"1364904\",\"ctime\":\"1525776658\",\"type\":\"note\",\"title\":\"\",\"summary\":\"\",\"content\":\"\",\"plain\":\"#Fate\\u002FGrand Order# #Saber Lily##FGO# \\u003Cbr\\u003E\\u003Cbr\\u003E出镜： Erin \\u003Cbr\\u003E\\u003Cbr\\u003E摄影&后期：@Even丶弥音苏 搓搓这个冻僵手的摄影&#091;二哈&#093;\\u003Cbr\\u003E\\u003Cbr\\u003E\\u003Cbr\\u003E寒假拍的 最开始以为会很冷 开拍之后发现阳光居然如此温暖(*´∀｀*)感觉浑身充满力量了x\\u003Cbr\\u003E\\u003Cbr\\u003E服装@喵屋小铺 嗷嗷嗷这套无敌好看了&#091;心&#093;\\u003Cbr\\u003E\\u003Cbr\\u003E特别感谢@女神家的小宠物 大冷天的一起战两套的 仙女！&#091;爱你&#093;\\u003Cbr\\u003E @喵了个汪工作室 棚主太好了帮忙解决了燃眉之急&#091;羞嗒嗒&#093;\\u003Cbr\\u003E\\u003Cbr\\u003E抄送@西安Cos站 @西安cos化妆协会 @水色动漫社公共主页 @Fate系列cos相关推送号 @半次元cosplay频道 @革子 @弥音丶nanami\\u003Cbr\\u003Esaber cn: 腿间挂件艾利安\",\"word_count\":0,\"multi\":[{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Ff1lifr6uvveeqtipflwsng2hsfrwvdt8.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234827\",\"w\":5304,\"h\":7952},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Faglfau1fzqiadwd0efevi6e77ttybzby.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234836\",\"w\":7952,\"h\":4472},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fpo2nksc7r0zgnbt5jvt2ytr2wbajmvnm.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234843\",\"w\":7952,\"h\":4472},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fatigjqhfcbmfkzdwonpmakytomwvmgom.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234852\",\"w\":7952,\"h\":4472},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002F5o0ajwavwdhl6l6gknvbh6lug6ncznnt.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234860\",\"w\":6897,\"h\":3536},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fdzvgyiiaoirdwp6hqrogqd7sltn24xjn.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234869\",\"w\":7952,\"h\":4472},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fkbs0jqyh6p9fkpfgwtymvm2xwyyjmf1h.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234878\",\"w\":5304,\"h\":7952},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fy8qbqgwmaa28sivcuewzqsad9oy0rkk9.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234887\",\"w\":5304,\"h\":7952},{\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F108702\\u002Fpost\\u002Fc0jdo\\u002Fnzz1ofwmozdhsspgt4sd9h7zk0hpnqjn.jpg\\u002Fw650\",\"type\":\"image\",\"mid\":\"9234897\",\"w\":7952,\"h\":5304}],\"pic_num\":9,\"work\":\"Fate\\u002FGrand Order\",\"wid\":\"5623\",\"work_real_name\":\"Fate\\u002FGrand Order\",\"post_tags\":[{\"tag_id\":\"399\",\"tag_name\":\"COS\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcore\\u002Ftags\\u002Fflag\\u002F179t0\\u002Fbc35e9d0b98c11e89533e7273e251f0e.png\\u002F2X2\",\"post_count\":\"387741\"},{\"tag_id\":\"92\",\"tag_name\":\"saber\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fuser\\u002F6949\\u002Fitem\\u002Fc0js1\\u002Fxwhxf92cn4c3zdosxy9fdrelrfvsprgk.jpg\\u002F2X2\",\"post_count\":\"3476\"},{\"tag_id\":\"63\",\"tag_name\":\"黑丝\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcore\\u002Ftags\\u002Fflag\\u002F1789d\\u002Feb360570796611e68e995d4396cfe68f.jpg\\u002F2X2\",\"post_count\":\"18797\"},{\"tag_id\":\"57\",\"tag_name\":\"御姐\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcore\\u002Fwork\\u002Fflag\\u002Fbzwp4\\u002Fb13aecc088e711e5a0d8c3251bf0b82d.jpg\\u002F2X2\",\"post_count\":\"61951\"},{\"tag_id\":\"51\",\"tag_name\":\"萝莉\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcore\\u002Fwork\\u002Fflag\\u002Fbzwp4\\u002Fc2e129e088e611e5a8ab516039cd7c57.jpg\\u002F2X2\",\"post_count\":\"116401\"},{\"tag_id\":\"23840\",\"tag_name\":\"阿尔托利亚·潘德拉贡\",\"type\":\"tag\",\"cover\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Fcoser\\u002F10814\\u002Fpost\\u002F4bqs\\u002Fbb772660376611e8a7d5736c84e332c4.jpg\\u002F2X2\",\"post_count\":\"5853\"}],\"like_count\":1973,\"user_liked\":true,\"reply_count\":76,\"share_count\":17,\"at_user_infos\":[{\"at_uname\":\"Even丶弥音苏\",\"uid\":\"0\"},{\"at_uname\":\"喵屋小铺\",\"uid\":\"1143383\",\"uname\":\"喵屋小铺\"},{\"at_uname\":\"女神家的小宠物\",\"uid\":\"0\"},{\"at_uname\":\"喵了个汪工作室\",\"uid\":\"0\"},{\"at_uname\":\"西安Cos站\",\"uid\":\"0\"},{\"at_uname\":\"西安cos化妆协会\",\"uid\":\"0\"},{\"at_uname\":\"水色动漫社公共主页\",\"uid\":\"0\"},{\"at_uname\":\"Fate系列cos相关推送号\",\"uid\":\"0\"},{\"at_uname\":\"半次元cosplay频道\",\"uid\":\"0\"},{\"at_uname\":\"革子\",\"uid\":\"2263378\",\"uname\":\"革子\"},{\"at_uname\":\"弥音丶nanami\",\"uid\":\"780671\",\"uname\":\"弥音丶nanami\"}],\"editor_status\":\"all_public\",\"forbidden_right_click\":true,\"view_need_login\":false,\"view_need_fans\":false,\"no_trans\":true,\"no_modify\":true,\"postStatus\":\"normal\"},\"detail_user\":{\"uid\":\"1364904\",\"avatar\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002FPublic\\u002FUpload\\u002Favatar\\u002F1364904\\u002Fcc396f40c6b7416088dde0e5065c6bbf\\u002Ffat.jpg\\u002Fabig\",\"uname\":\"腿间挂件艾利安\",\"self_intro\":\"微博 腿间挂件艾利安 求大家多多关注啦\",\"sex\":0,\"following\":\"16\",\"follower\":\"2872\",\"followstate\":\"havefollow\",\"value_user\":0,\"show_utags\":true,\"utags\":[{\"ut_id\":\"1\",\"ut_name\":\"Coser\"}]},\"top_lists\":[{\"id\":\"649923\",\"ttype\":\"coser\",\"sub_type\":\"lastday\",\"stime\":\"20180511\",\"rank\":\"3\"},{\"id\":\"651429\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180513\",\"rank\":\"9\"},{\"id\":\"653830\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180516\",\"rank\":\"10\"},{\"id\":\"652230\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180514\",\"rank\":\"10\"},{\"id\":\"653032\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180515\",\"rank\":\"12\"},{\"id\":\"650632\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180512\",\"rank\":\"12\"},{\"id\":\"649139\",\"ttype\":\"coser\",\"sub_type\":\"lastday\",\"stime\":\"20180510\",\"rank\":\"19\"},{\"id\":\"649853\",\"ttype\":\"coser\",\"sub_type\":\"week\",\"stime\":\"20180511\",\"rank\":\"33\"}],\"detail_banners\":[{\"link\":\"https:\\u002F\\u002Fbcy.net\\u002Fhuodong\\u002F199\",\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Feditor\\u002Fflag\\u002Fc0jrs\\u002Fd141c960d0f311e8b2acb57683eff268.jpg\",\"title\":\"\"},{\"link\":\"https:\\u002F\\u002Fbcy.net\\u002Fhuodong\\u002F201\",\"path\":\"https:\\u002F\\u002Fimg-bcy-qn.pstatp.com\\u002Feditor\\u002Fflag\\u002Fc0js2\\u002F586c3860d8f311e8bce245caf4e164cb.png\",\"title\":\"\"}],\"self\":false,\"currentStyle\":{\"bgColor\":\"white\",\"fontSize\":\"m\",\"indent\":\"noindent\"}},\"user\":{\"uid\":\"351913\"}}\");";
            string str2 = Regex.Unescape(str);
            string regStr = "JSON.parse(?<url>.+);";
            Match mat = Regex.Match(str2, regStr);
            Console.WriteLine(mat.Value);
            Regex regUrl = new Regex("\"path\":\"(?<url>.+?)/w650");
            MatchCollection mc = regUrl.Matches(str2);
            foreach (Match ma in mc)
            {
                Console.WriteLine(ma.Groups["url"].Value);
            }

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
