using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VipImgApi.Helper
{
    /// <summary>
    /// 公共函数类
    /// </summary>
    public class Commons
    {
        public static string GetAppSetting(string key)
        {
            var keys = System.Configuration.ConfigurationManager.AppSettings.AllKeys;
            if (keys == null || !keys.Contains(key))
                return null;
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            return value;
        }
    }

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
}
