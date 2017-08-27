using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace VipData.Model
{
    /// <summary>
    /// 用户已输入命令
    /// </summary>
    public class UserCommand
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public ObjectId UserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateAt { get; set; }
        /// <summary>
        /// Access命令
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DelAt { get; set; }
    }
}
