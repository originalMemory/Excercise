using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipData.Model
{
    /// <summary>
    /// 备份用户已执行命令
    /// </summary>
    public class BackupPost
    {
        /// <summary>
        /// 命令来源用户
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 命令内容
        /// </summary>
        public string command { get; set; }
        /// <summary>
        /// 命令类型
        /// </summary>
        public string type { get; set; }
    }
}
