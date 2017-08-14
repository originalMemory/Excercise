using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; 

namespace CSharpTest.Helper
{
    public static class IDHelper
    {
        [ThreadStatic]
        private static MD5CryptoServiceProvider md5Service;

        private static MD5CryptoServiceProvider MD5
        {
            get
            {
                if (md5Service == null)
                    md5Service = new MD5CryptoServiceProvider();
                return md5Service;
            }
        }

        public static byte[] ComputeMD5ForString(string input)
        {
            byte[] bytes = UTF8Encoding.UTF8.GetBytes(input);
            return MD5.ComputeHash(bytes);
        }

        public static Guid GetGuid(string input)
        {
            if (string.IsNullOrEmpty(input))
                return Guid.Empty;
            byte[] bytes = ComputeMD5ForString(input);
            return new Guid(bytes);
        }
    }

    public class EncryptHelper
    {
        const string scret_key = "&*(TIYTYF&%^*(^^";
        public static string GetOrderToken(string order_code)
        {
            return IDHelper.GetGuid(string.Format("{0}{1}", order_code, scret_key)).ToString();
        }

        public static bool VerifyOrderToken(string token, string order_code)
        {
            string _token = GetOrderToken(order_code);
            return _token == token;
        }

        const string pwd_key = "&*^%%*(%%&**^^";

        public static Guid GetEncryPwd(string pwd)
        {
            return IDHelper.GetGuid(pwd + pwd_key);
        }
    }
}
