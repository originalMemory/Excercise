using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VipManager.Model
{
    /// <summary>
    /// 套餐映射
    /// </summary>
    public class CombSnap
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 编号
        /// </summary>
        public int No;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 类型
        /// </summary>
        public CombType Type;
        /// <summary>
        /// 价格
        /// </summary>
        public double Price;
        /// <summary>
        /// 次数
        /// </summary>
        public int Num;
        /// <summary>
        /// 已用次数
        /// </summary>
        public int UsedNum;
        /// <summary>
        /// 折扣
        /// </summary>
        public double Discount;
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndAt;
        /// <summary>
        /// 包含产品信息列表
        /// </summary>
        public List<ProInfo> ProList;
    }

    /// <summary>
    /// 产品映射
    /// </summary>
    public class ProInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ProID;
        /// <summary>
        /// 价格
        /// </summary>
        public double Price;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;
    }
}
