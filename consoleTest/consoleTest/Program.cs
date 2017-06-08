using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSharpTest.Model;
using CSharpTest.Tools;
using System.IO;
using Microsoft.VisualBasic;
using MongoDB.Bson;

using NReadability;
using System.Net;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;


namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //string appid = "2n91H7A2k6yg27dGLcD9";
            //string appkey = "zCziLdek8fKNvHNM6E3kl4C9a";
            //string url = "http://open.gsdata.cn/api/wx/opensearchapi/nickname_keyword_search";
            //Dictionary<string, object> postData = new Dictionary<string, object>();
            //postData.Add("keyword", "天使Angel");
            ////postData.Add("url", "http://mp.weixin.qq.com/s?__biz=MzA4ODQ2MDgxMA==&mid=210176467&idx=6&sn=ede1974d166a8318e27c1b3f093e5223&scene=4#wechat_redirect");
            ////postData.Add("loginname", "15210917689");
            ////postData.Add("password", "kdi1994");
            //GsdataApi api = new GsdataApi(appid, appkey);
            //string str = api.Call(postData, url);
            //Console.WriteLine(str);

            //DnlTools.Temp();

            Dnl_Keyword_Media task = new Dnl_Keyword_Media
            {
                _id = ObjectId.GenerateNewId(),
                Keyword = "耶稣",
                CreatedAt = DateTime.Now.AddHours(8),
            };
            DnlTools.SearchWeiXiArticle(task);
            Console.ReadKey();
        }

    }
}
