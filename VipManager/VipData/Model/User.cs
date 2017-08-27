using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace VipData.Model
{
    /// <summary>
    /// 登陆商家信息
    /// </summary>
    public class UserMongo
    {
        /// <summary>
        /// MongoID
        /// </summary>
        public ObjectId _id { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateAt { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public Guid Password { get; set; }
        /// <summary>
        /// 地理位置
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 本次登陆IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        public DateTime LastLoginAt { get; set; }
        /// <summary>
        /// 总计登陆次数
        /// </summary>
        public int LoginNum { get; set; }
        /// <summary>
        /// 商号
        /// </summary>
        public string ShopNo { get; set; }
        /// <summary>
        /// 营业执照图片地址
        /// </summary>
        public string PicLicenUrl { get; set; }
        /// <summary>
        /// Logo图片地址
        /// </summary>
        public string LogoUrl { get; set; }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 联系人微信
        /// </summary>
        public string Weixin { get; set; }
        /// <summary>
        /// 联系人支付宝
        /// </summary>
        public string Alipay { get; set; }
    }
}
