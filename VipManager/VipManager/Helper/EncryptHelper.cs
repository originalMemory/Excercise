using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VipManager.Helper
{

    public class EncryptHelper
    {
        /// <summary>
        /// 生成MD5编码
        /// </summary>
        /// <param name="myString">源字符串</param>
        /// <returns>MD5字符串</returns>
        public static string GetMD5(string myString)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(myString);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x2");
            }
            return sTemp.ToLower();
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string EncordBase64(string source)
        {

            string encode = "";
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

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
