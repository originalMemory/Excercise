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

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //D3force force = new D3force();
            //force.initializeNodes();

            //string path = @"F:\全部网址-Jacky标注了471个.xls";
            //var userObjId = new ObjectId("571e283e6ce8b80cb8963e84");
            //DnlTools dnl = new DnlTools();
            //dnl.ImportDomain(path, userObjId);

            string userName = "test";
            string pw = "123";
            Guid pwGuid = EncryptHelper.GetEncryPwd(userName + pw);
            VipMaUserMongo user = new VipMaUserMongo
            {
                UserName = userName,
                Password = pwGuid
            };
            MongoDBHelper.Instance.GetVipMaUser().InsertOne(pwGuid);
            
            Console.ReadKey();
        }

    }

}
