using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipManager.Helper
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 格式化字符串
        /// </summary>
        /// <param name="format">符合格式的字符串</param>
        /// <param name="objs">参数数组</param>
        /// <returns></returns>
        public static string FormatStr(this string format, params object[] objs)
        {
            return string.Format(format, objs);
        }
    }
}
