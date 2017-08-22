using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTest.Helper;
using CSharpTest.Model;

namespace CSharpTest.Tools
{
    class VipMaTools
    {
        public static void InsertUser(string userName, string password)
        {
            var guid = EncryptHelper.GetEncryPwd(password);
            VipMaUserMongo user = new VipMaUserMongo
            {
                UserName = userName,
                Password = guid
            };
            MongoDBHelper.Instance.GetVipMaUser().InsertOne(user);
            Console.WriteLine("添加用户成功！");
        }
    }
}
