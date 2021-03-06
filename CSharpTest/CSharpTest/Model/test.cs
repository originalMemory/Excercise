﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    public class test
    {
        public ObjectId _id { get; set; }
        public int type { get; set; }
        public DateTime delAt { get; set; }
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

    /// <summary>
    /// 公有搜索引擎S.E.链接类
    /// </summary>
    public class Dnl_Link_Baidu
    {
        public ObjectId _id { get; set; }
        /// <summary>
        /// 搜索关键词Id
        /// </summary>
        public string SearchkeywordId { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 一级域名
        /// </summary>
        public string TopDomain { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string LinkUrl { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述，从搜索引擎S.E.搜索页面抓取
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime CreatedAt { get; set; }
        /// <summary>
        /// 网页源代码
        /// </summary>
        public string Html { get; set; }
        /// <summary>
        /// 网页正文，从网页源代码获取
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 搜索引擎S.E.蓝V认证
        /// </summary>
        public int BaiduVStar { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PublishTime { get; set; }
        /// <summary>
        /// 域名收录量
        /// </summary>
        public long DCNum { get; set; }
        /// <summary>
        /// 是否为推广链接
        /// </summary>
        public bool IsPromotion { get; set; }
        /// <summary>
        /// 关键词匹配状态。0为无匹配，1为网页源码匹配，2为标题匹配，3为描述匹配，4为标题及描述匹配，5为网址匹配
        /// </summary>
        public int MatchAt { get; set; }
    }
    /// <summary>
    /// 私有搜索引擎S.E.链接与公有搜索引擎S.E.链接映射类，包含用户对链接的分类和设置
    /// </summary>
    public class Dnl_LinkMapping_Baidu
    {
        public ObjectId _id { get; set; }
        public ObjectId LinkId { get; set; }
        public ObjectId UserId { get; set; }
        public ObjectId ProjectId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public System.DateTime CreatedAt { get; set; }
        /// <summary>
        /// 数据清洗状态：1，收藏;2,排除
        /// </summary>
        public int DataCleanStatus { get; set; }
        /// <summary>
        /// 数据类型Id
        /// </summary>
        public ObjectId InfriLawCode { get; set; }
        /// <summary>
        /// 信源类型
        /// </summary>
        public IWSData.Model.SourceType Source { get; set; }
    }
}
