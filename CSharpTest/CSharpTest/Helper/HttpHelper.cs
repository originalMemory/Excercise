using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using HtmlAgilityPack;
using System.Threading;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace CSharpTest.Tools
{
    public class HttpHelper
    {

        /// <summary>
        /// 当前代理IP
        /// </summary>
        public WebProxy NowProxyIp;
        /// <summary>
        /// IP过期时间
        /// </summary>
        public DateTime ExpireTime;
        public HttpHelper()
        {
            //初始化
            //LoadProxyIP();
            while (true)
            {
                try
                {
                    //判断本机IP是否在白名单内
                    string str = CreateHttpGet("http://ip.11jsq.com/index.php/api/entry?method=proxyServer.generate_api_url&packid=0&fa=0&fetch_key=&qty=1&time=100&pro=&city=&port=1&format=json&ss=5&css=&ipport=1&et=1&dt=1&specialTxt=3&specialJson=");
                    JObject json = JObject.Parse(str);
                    if (json["code"].ToString() == "5")
                    {
                        CommonTools.Log(json["msg"].ToString());
                        CommonTools.Log("添加IP至白名单");
                        //添加白名单
                        string ip = Regex.Match(str, @"(2(5[0-5]{1}|[0-4]\d{1})|[0-1]?\d{1,2})(\.(2(5[0-5]{1}|[0-4]\d{1})|[0-1]?\d{1,2})){3}").Value;
                        json = JObject.Parse(CreateHttpGet(string.Format("http://http.zhiliandaili.cn/Users-whiteIpAddNew.html?appid=2723&appkey=b36d9ff1f6e1f19e6cca8d7a28ae5e9f&whiteip={0}&index=", ip)));
                        if (json["code"].ToString() == "0")
                        {
                            LoadProxyIP();
                            break;
                        }
                    }
                    else
                    {
                        string ipStr = json["data"][0]["IP"].ToString();
                        string timeStr = json["data"][0]["ExpireTime"].ToString();
                        if (!DateTime.TryParse(timeStr, out ExpireTime))
                        {
                            CommonTools.Log("获取出错，等待1s再获取代理IP!");
                            Thread.Sleep(1000);
                            continue;
                        }
                        string[] arr = ipStr.Split(':');
                        NowProxyIp = new WebProxy(arr[0], Convert.ToInt32(arr[1]));
                        CommonTools.Log(string.Format("代理IP：{0}\t到期时间：{1}", NowProxyIp.Address.ToString(), timeStr));
                        if (yanzhen(NowProxyIp))
                            break;
                    }
                }
                catch (Exception ex)
                {
                    CommonTools.Log(ex.Message);
                    Thread.Sleep(1000);
                }

            }
        }

        private string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry IpEntry = Dns.GetHostEntry(hostName);
            for (int i = 0; i < IpEntry.AddressList.Length; i++)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }
            return "";
        }

        public static List<string> _agent = new List<string>
        {
            "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; AcooBrowser; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
            "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; Acoo Browser; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; .NET CLR 3.0.04506)",
            "Mozilla/4.0 (compatible; MSIE 7.0; AOL 9.5; AOLBuild 4337.35; Windows NT 5.1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)",
            "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)",
            "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 2.0.50727; Media Center PC 6.0)",
            "Mozilla/5.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET CLR 1.0.3705; .NET CLR 1.1.4322)",
            "Mozilla/4.0 (compatible; MSIE 7.0b; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727; InfoPath.2; .NET CLR 3.0.04506.30)",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN) AppleWebKit/523.15 (KHTML, like Gecko, Safari/419.3) Arora/0.3 (Change: 287 c9dfb30)",
            "Mozilla/5.0 (X11; U; Linux; en-US) AppleWebKit/527+ (KHTML, like Gecko, Safari/419.3) Arora/0.6",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.2pre) Gecko/20070215 K-Ninja/2.1.1",
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; zh-CN; rv:1.9) Gecko/20080705 Firefox/3.0 Kapiko/3.0",
            "Mozilla/5.0 (X11; Linux i686; U;) Gecko/20070322 Kazehakase/0.4.5",
            "Mozilla/5.0 (X11; U; Linux i686; en-US; rv:1.9.0.8) Gecko Fedora/1.9.0.8-1.fc10 Kazehakase/0.5.6",
            "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_3) AppleWebKit/535.20 (KHTML, like Gecko) Chrome/19.0.1036.7 Safari/535.20",
            "Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; fr) Presto/2.9.168 Version/11.52",
        };

        /// <summary>  
        /// 创建GET方式的HTTP请求  
        /// </summary>  
        string CreateHttpGet(string url, int timeout = 5000, string userAgent = null, CookieCollection cookies = null)
        {
            string resp = "";
            while (true)
            {
                try
                {
                    HttpWebRequest request = null;
                    if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                    {
                        //对服务端证书进行有效性校验（非第三方权威机构颁发的证书，如自己生成的，不进行验证，这里返回true）
                        //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                        request = WebRequest.Create(url) as HttpWebRequest;
                        request.ProtocolVersion = HttpVersion.Version10;    //http版本，默认是1.1,这里设置为1.0
                    }
                    else
                    {
                        request = WebRequest.Create(url) as HttpWebRequest;
                    }
                    request.Method = "GET";

                    //设置代理UserAgent和超时
                    //request.UserAgent = userAgent;
                    request.Timeout = timeout;
                    if (cookies != null)
                    {
                        request.CookieContainer = new CookieContainer();
                        request.CookieContainer.Add(cookies);
                    }
                    StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream());

                    resp = sr.ReadToEnd();
                    break;
                }
                catch (Exception ex)
                {
                    CommonTools.Log(ex.Message);
                    Thread.Sleep(1000);
                }
            }
            return resp;
        }

        /// <summary>
        /// 加载代理IP
        /// </summary>
        void LoadProxyIP()
        {
            while (true)
            {
                //获取余额，当接近0时通知充值
                string str = CreateHttpGet("http://http.zhiliandaili.cn/Users-getBalanceNew.html?appid=2723&appkey=b36d9ff1f6e1f19e6cca8d7a28ae5e9f");
                JObject json = JObject.Parse(str);
                double money = Convert.ToDouble(json["data"]);
                if (json["code"].ToString() == "10000")
                {
                    Thread.Sleep(10000);
                    continue;
                }
                if (money <= 0.003)
                {
                    CommonTools.Log("余额不足，请通知管理员充值代理IP费用！充值后按任意键继续！");
                    CommonTools.SendErrorMail("搜狗BOT", "", "代理IP余额不足！");
                    Console.ReadKey();
                }
                str = CreateHttpGet("http://ip.11jsq.com/index.php/api/entry?method=proxyServer.generate_api_url&packid=0&fa=0&fetch_key=&qty=1&time=100&pro=&city=&port=1&format=json&ss=5&css=&ipport=1&et=1&dt=1&specialTxt=3&specialJson=");
                json = JObject.Parse(str);
                try
                {
                    string ipStr = json["data"][0]["IP"].ToString();
                    string timeStr = json["data"][0]["ExpireTime"].ToString();
                    if (!DateTime.TryParse(timeStr, out ExpireTime))
                    {
                        CommonTools.Log("获取出错，等待1s再获取代理IP!");
                        Thread.Sleep(1000);
                        continue;
                    }
                    string[] arr = ipStr.Split(':');
                    NowProxyIp = new WebProxy(arr[0], Convert.ToInt32(arr[1]));
                    CommonTools.Log(string.Format("代理IP：{0}\t到期时间：{1}", NowProxyIp.Address.ToString(), timeStr));
                    if (yanzhen(NowProxyIp))
                        break;
                }
                catch (Exception ex)
                {
                    CommonTools.Log(ex.Message);
                    CommonTools.Log("代理IP获取失败，重试！");
                    Thread.Sleep(1000);
                }
            }
        }

        bool yanzhen(WebProxy pro)
        {
            try
            {
                HttpWebRequest Req;
                HttpWebResponse Resp;
                //WebProxy proxyObject = new WebProxy(ipStr, port);// port为端口号 整数型
                Req = WebRequest.Create("http://www.baidu.com/s?wd=ip&ie=utf-8&tn=94523140_hao_pg") as HttpWebRequest;
                Req.Proxy = pro; //设置代理
                Req.Timeout = 1000;   //超时
                Resp = (HttpWebResponse)Req.GetResponse();
                Encoding bin = Encoding.GetEncoding("UTF-8");
                using (StreamReader sr = new StreamReader(Resp.GetResponseStream(), bin))
                {
                    string str = sr.ReadToEnd();
                    return true;
                }

            }
            catch (Exception ex)
            {
                CommonTools.Log(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 指定header参数的HTTP Get方法
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="url"></param>
        /// <returns>respondse</returns>
        public string Get(WebHeaderCollection headers, string url, string responseEncoding = "UTF-8", bool isUseProxyIp = false, bool isUseCookie = false)
        {

            string responseText = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 4000;
                if (isUseProxyIp)
                    request.Proxy = NowProxyIp;

                request.Method = "GET";
                //request.Headers = headers;
                foreach (string key in headers.Keys)
                {
                    switch (key.ToLower())
                    {
                        case "user-agent":
                            request.UserAgent = headers[key];
                            break;
                        case "referer":
                            request.Referer = headers[key];
                            break;
                        case "host":
                            request.Host = headers[key];
                            break;
                        case "contenttype":
                            request.ContentType = headers[key];
                            break;
                        case "accept":
                            request.Accept = headers[key];
                            break;
                        default:
                            break;
                    }

                }

                if (string.IsNullOrEmpty(request.Referer))
                {
                    request.Referer = "http://weixin.sogou.com/";
                };
                if (string.IsNullOrEmpty(request.Host))
                {
                    request.Host = "weixin.sogou.com";
                };
                if (string.IsNullOrEmpty(request.UserAgent))
                {
                    Random r = new Random();
                    int index = r.Next(_agent.Count - 1);
                    request.UserAgent = _agent[index];
                }
                if (isUseCookie)
                {
                    CookieContainer objcok = new CookieContainer();
                    //Cookie cook1 = new Cookie();
                    //cook1.Name = "CNZZDATA1257708097";
                    //cook1.Value = "1167517418-1479817270-%7C1488717892";
                    //objcok.Add(cook1);
                    ////objcok.Add(new Cookie("SUV", "0093178BCA6333225AA9E662822AC111", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("SMYUV", "1522842946301271", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("CXID", "7C92DB39D595722E2A8F9D1C71CDCEFE", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("SUID", "4ADACD6F3665860A5AFD4E57000E6731", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("IPLOC", "CN1100", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("sw_uuid", "2744924708", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("dt_ssuid", "3606773377", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("pex", "C864C03270DED3DD8A06887A372DA219231FFAC25A9D64AE09E82AED12E416AC", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("ssuid", "7447060622", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("sg_uuid", "9841370979", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("ABTEST", "0|1532324497|v1", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("ld", "llllllllll2bqQ$5lllllVHNru9lllllzAYWMlllll9lllll4ylll5@@@@@@@@@@", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("LSTMV", "244%2C70", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("LCLKINT", "3560", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("SUIR", "22B2A507686D154969E78F23687CFD0F", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("weixinIndexVisited", "1", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("JSESSIONID", "aaaJTduGhewQ8dtQgPHsw", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("PHPSESSID", "9fg55mratktraq1i077eem6d73", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("sct", "19", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("SNUID", "D44453F29D9BECA916754D6C9EDBE7BA", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("seccodeRight", "success", "/", ".sogou.com"));
                    //////objcok.Add(new Cookie("successCount", "1|Mon, 30 Jul 2018 13:40:29 GMT", "/", ".sogou.com"));
                    ////objcok.Add(new Cookie("refresh", "1", "/", ".sogou.com"));
                    request.CookieContainer = objcok;
                    //CookieCollection cc = Tools.LoadCookieFromCache();
                    //cc.Add()
                    //request.CookieContainer = new CookieContainer();
                    //request.CookieContainer.Add(cc);
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (isUseCookie && response.Cookies.Count > 0)
                {
                    var cookieCollection = response.Cookies;
                }
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();




                // Open the stream using a StreamReader for easy access.
                Encoding encoding;
                switch (responseEncoding.ToLower())
                {
                    case "utf-8":
                        encoding = Encoding.UTF8;
                        break;
                    case "unicode":
                        encoding = Encoding.Unicode;
                        break;
                    case "ascii":
                        encoding = Encoding.ASCII;
                        break;
                    default:
                        encoding = Encoding.Default;
                        break;

                }
                StreamReader reader = new StreamReader(dataStream, encoding);//System.Text.Encoding.Default
                // Read the content.

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("requests status_code error");
                }
                responseText = reader.ReadToEnd();
                reader.Close();

                // Cleanup the streams and the response.
                dataStream.Close();
                response.Close();


            }
            catch (Exception e)
            {
                responseText = "获取网页失败！";
            }

            return responseText;
        }


        public string GetWithProxyIp(string url, List<string> errors, List<string> asserts = null, string responseEncoding = "UTF-8", bool isUseCookie = false)
        {
            string text = "";
            WebHeaderCollection headers = new WebHeaderCollection();
            bool isAgain = false;    //判断是否需要重新获取
            do
            {
                if (DateTime.Now > ExpireTime)
                    LoadProxyIP();
                //获取网页，判断是否为错误信息
                text = Get(headers, url, "utf-8", true);
                if (text == "获取网页失败！")
                {
                    CommonTools.Log("获取网页失败！重试！");
                    LoadProxyIP();
                    isAgain = true;
                    continue;
                }
                bool isFind = false;
                foreach (var x in errors)
                {
                    if (text.Contains(x))
                    {
                        CommonTools.Log(x);
                        CommonTools.Log("被发现，更换代理IP");
                        LoadProxyIP();
                        isFind = true;
                        break;
                    }
                }
                if (isFind)
                    isAgain = true;
                else
                    isAgain = false;

                if (asserts != null)
                {
                    //为警告类型时，返回错误类型
                    foreach (var x in asserts)
                    {
                        text = x;
                        return text;
                    }
                }
            } while (isAgain);

            return text;
        }

        bool yanzhen(string ipStr, int port)
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
