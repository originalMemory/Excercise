using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipManager.Model
{
    /// <summary>
    /// 会员类型
    /// </summary>
    public enum VipType
    {
        /// <summary>
        /// 次数型
        /// </summary>
        Num,
        /// <summary>
        /// 折扣型
        /// </summary>
        Discount,
        /// <summary>
        /// 时间型
        /// </summary>
        Time
    }

    /// <summary>
    /// 会员年龄段
    /// </summary>
    public enum AgeRange
    {
        /// <summary>
        /// 儿童
        /// </summary>
        Child,
        /// <summary>
        /// 青年
        /// </summary>
        Youth,
        /// <summary>
        /// 中年
        /// </summary>
        Middle,
        /// <summary>
        /// 退休
        /// </summary>
        Retire
    }
}
