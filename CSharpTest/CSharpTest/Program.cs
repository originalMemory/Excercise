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
using AISSystem;
using System.Text.RegularExpressions;


namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //string url = "http://localhost:2022/api/Keyword/DelLinkShareComment";
            //Dictionary<string, string> para = new Dictionary<string, string>();
            //para.Add("id", "5a084a77c37a3c28cc0a7ba3");
            //para.Add("userId", "58845bed1e5318078cb01b1a");
            ////para.Add("replyId", "5a084a77c37a3c28cc0a7ba3");
            ////para.Add("content", "测试回复");
            ////para.Add("level", "1");
            //var response = WebApiInvoke.CreatePostHttpResponse(url, para);
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //Console.WriteLine(sr.ReadToEnd());


            string str = "这个女人在娱乐圈也算是大姐大了，当年那场银行包养风波并未将这个女人打垮，现在虽然已经快五十岁了，可是依然过的滋润十足，只不过她这样的女人最需要的就是结交权贵，因此眼见冯楚扬身份不凡，而且长的帅，年轻，首先就过来结交-= 第壹版主小説站官網 =-んττρs://ωωω.dΙyΙьáиzんú.Ιиんττρs://м.dΙyΙьáиzんú.Ιи-= 第壹版主小説站官網 =-んττρs://щщщ.dǐγǐЪáηzんυ.ǐηんττρs://м.dǐγǐЪáηzんυ.ǐη-= 第壹版主小説站官網 =-んττρs://ωωω.dìγìЪаηzんú.ìηんττρs://м.dìγìЪаηzんú.ìη-= 第壹版主小説站官網 =——= м.ｄīｙīｂāńｚｈū.īń =——= 第壹版主小説站官網 =——= щщщ.ｄīｙīｂāńｚｈū.ìň =-发送邮件 ｄīｙīｂāńｚｈū ⊙ ｑｑ.ｃōｍ「是许小姐吧？你好你好……」冯楚扬眼见许晴首先和自己握手，他眼中露出了色迷迷的神色，轻笑着握着她的玉手，笑道，「我可是从小喜欢看许小姐的电视剧和电影啊，尤其是之前上映的那个电影《老炮儿》，我可是最喜欢您在里面和冯小刚前辈那场后入式的动作大战啊……」";
            Regex reg=new Regex(@"= 第壹版主小説([\s\S]*) ｄīｙīｂāńｚｈū ⊙ ｑｑ.ｃōｍ");
            string temp = reg.Replace(str,"");

            Console.WriteLine(temp);

            Console.ReadKey();
        }

    }


}
