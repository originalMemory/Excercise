using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VipData.Model
{
    /// <summary>
    /// 文件类，用于保存图片
    /// </summary>
    public class UserImgMongo
    {

        public ObjectId _id { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] Bytes { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string Extention { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public Nullable<int> Size { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public System.DateTime CreateAt { get; set; }
        /// <summary>
        /// 用户图片类型
        /// </summary>
        public UserImgType Type { get; set; }
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