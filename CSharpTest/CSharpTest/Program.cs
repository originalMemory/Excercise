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

namespace CSharpTest
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //D3force force = new D3force();
            //force.initializeNodes();
            DnlTools dnl = new DnlTools();
            dnl.DelUnuseLink();

            //string userName = "test";
            //string pw = "123";
            //string md5 = EncryptHelper.GetMD5(pw);
            //string base64 = EncryptHelper.EncordBase64(md5);
            //VipMaUserMongo user = new VipMaUserMongo
            //{
            //    UserName = userName,
            //    Password = base64
            //};
            //MongoDBHelper.Instance.GetVipMaUser().InsertOne(user);

           

            Console.ReadKey();
        }

    }

}
