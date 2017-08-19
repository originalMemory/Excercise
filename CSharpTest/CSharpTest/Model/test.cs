using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    public class test
    {
        public UserTypes type { get; set; }
    }

    /// <summary>
    /// 用户类型
    /// </summary>
    public enum UserTypes
    {
        /// <summary>
        /// 工程师
        /// </summary>
        Engineer,
        /// <summary>
        /// 免费用户
        /// </summary>
        Free,
        /// <summary>
        /// 付费用户
        /// </summary>
        Vip
    }
}
