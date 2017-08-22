using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipData.Model
{
    /// <summary>
    /// 会员类型
    /// </summary>
    public enum CombType
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

    /// <summary>
    /// 套餐有效时间
    /// </summary>
    public enum CombTimeType
    {
        /// <summary>
        /// 月卡
        /// </summary>
        Month,
        /// <summary>
        /// 季卡
        /// </summary>
        Season,
        /// <summary>
        /// 半年卡
        /// </summary>
        HalfYear,
        /// <summary>
        /// 年卡
        /// </summary>
        Year,
    }

    /// <summary>
    /// 用户头像类型
    /// </summary>
    public enum UserImgType
    {
        /// <summary>
        /// 营业执照
        /// </summary>
        License,
        /// <summary>
        /// Logo
        /// </summary>
        Logo
    }
}
