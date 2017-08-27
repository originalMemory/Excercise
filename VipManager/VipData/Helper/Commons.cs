using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace VipData.Helper
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

    
}
