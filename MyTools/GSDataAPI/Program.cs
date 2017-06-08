using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSDataAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string appid = "";              //你的appid
            string appkey = "";        //你的appkey
            string url = "http://open.gsdata.cn/api/sys/sysapi/user_info";      //接口地址
            Dictionary<string, object> postData = new Dictionary<string, object>();     //post参数，按接口要求添加。不用添加appid和appkey
            postData.Add("logintype", "");
            postData.Add("loginname", "");
            GSDataSDK api = new GSDataSDK(appid, appkey);       //初始化接口
            string str = api.Call(postData, url);               //调用接口
            Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}
