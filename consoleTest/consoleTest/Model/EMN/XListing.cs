
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AISSystem;
using IprseeData;
using MongoDB.Bson;

namespace cPlusPlusTest.Models
{
    public class XListing : INotifyPropertyChanged
    {
        public int Index { get; set; }
        public string SiteName { get; set; }
        public string CompanyName { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string DetailQueryName { get; set; }//
        public string CommentQueryName { get; set; }
        public string BuyerListQueryName { get; set; }
        public bool? BotDetails { get; set; }
        public bool? BotComments { get; set; }//
        public bool? BotBuyerList { get; set; }
        public string RealBrandName { get; set; }
        public string RealProductName { get; set; }
        public Nullable<int> usrid { get; set; }

        public ObjectId UId { get; set; }

        public Nullable<int> PageNum { get; set; }
        public Nullable<int> Position { get; set; }


        public ObjectId ProjectId { get; set; }

        #region Item Info

        /// <summary>
        /// 宝贝名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 宝贝价格
        /// </summary>
        public double? ItemPrice { get; set; }

        /// <summary>
        /// 所有价格，例如：US $20 - 40 / Kilogram
        /// </summary>
        public string ItemAllPrice { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        public string ItemPriceCurrency { get; set; }

        /// <summary>        
        /// 最近宝贝最近30天确认收货，
        /// </summary>
        public int? ItemSold30Days { get; set; }

        /// <summary>        
        /// 海外宝贝最近30天确认收货，
        /// </summary>
        public int? Recent30DaySoldNum { get; set; }      

        /// <summary>        
        /// 最近宝贝最近30天成交预定数（有可能有退货）
        /// </summary>
        public int? Item30DaysOrderedNum { get; set; }
                
        /// <summary>
        /// 宝贝评论数
        /// </summary>
        public int? ItemTotalCommentCount { get; set; }

        /// <summary>
        /// 宝贝好评率
        /// </summary>
        public float? ItemGoodCommentPercent { get; set; }

        /// <summary>
        /// 宝贝浏览次数 
        /// </summary>
        public int? ItemReviews { get; set; }

        /// <summary>
        /// 宝贝在网站中的编号
        /// </summary>
        public string ItemID { get; set; }

        /// <summary>
        /// 宝贝详情链接
        /// </summary>
        public string ItemDetailUrl { get; set; }

        /// <summary>
        /// 宝贝库存数量
        /// </summary>
        public int? ItemQuantityInStock { get; set; }

        /// <summary>
        /// 宝贝图片链接
        /// </summary>
        public string ItemImgUrl { get; set; }

        /// <summary>
        /// 宝贝品牌名称
        /// </summary>
        public string ItemBrandName { get; set; }

        /// <summary>
        /// 宝贝在网站的产品名称  型号
        /// </summary>
        public string ItemProductName { get; set; }

        /// <summary>
        /// 宝贝新旧
        /// </summary>
        public string ItemCondition { get; set; } 

        /// <summary>
        /// 宝贝是否包退 
        /// </summary>
        public bool? ItemCanReturn { get; set; }

        /// <summary>
        /// 宝贝是否保证是正品
        /// </summary>
        public bool? ItemIsGenuines { get; set; }

        /// <summary>
        /// 宝贝所在地
        /// </summary>
        public string ItemLocation { get; set; }

        /// <summary>
        /// 原产地
        /// </summary>
        public string PlaceOfOrigin { get; set; }

        /// <summary>
        /// QQ号
        /// </summary>
        public long? QQ { get; set; }

        /// <summary>
        /// 宝贝icon
        /// </summary>
        public string Itempic { get; set; }

        public ObjectId taskid { get; set; }
        public string taskName { get; set; }

        BotStatus itemBotStatus = BotStatus.StructureChanged;
        public BotStatus ItemBotStatus { get { return itemBotStatus; } set { itemBotStatus = value;  } }
        public int? BotSpentMilliseconds;
        
        public Dictionary<string, string> ItemPropertyNameValues = new Dictionary<string, string>();
        public string ItemNameValueText { get { return string.Join("; ", ItemPropertyNameValues ); } }

        public List<XSalesRecord> SalesRecords = new List<XSalesRecord>();

        public List<SubVersion> SubVersions = new List<SubVersion>();

        public List<ItemComment> ItemCommentList = new List<ItemComment>();
        BotStatus itemCommentBotStatus= BotStatus.StructureChanged ;
        public BotStatus ItemCommentBotStatus { get { return itemCommentBotStatus; } set { itemCommentBotStatus = value;   } }

        public List<string> ProdImgs = new List<string>();

        public byte? InfringmentType { get; set; }

        #endregion 

        #region Shop Info

        /// <summary>
        /// 店铺名称
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// 主营
        /// </summary>
        public string ShopSalesProducts { get; set; }
        /// <summary>
        /// 店铺所在地
        /// </summary>
        public string ShopLocation { get; set; }
        /// <summary>
        /// 店铺所在市
        /// </summary>
        public string ShopCity { get; set; }
        /// <summary>
        /// 店铺详细地址
        /// </summary>
        public string ShopAddress { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ShopZipCode { get; set; }
        /// <summary>
        /// 宝贝数
        /// </summary>
        public int? ShopItemCount { get; set; }
        /// <summary>
        /// 月成交数
        /// </summary>
        public int? ShopMonthDeals { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        public string ShopContactUrl { get; set; }
        /// <summary>
        /// 信用级别
        /// </summary>
        public string ShopCredit { get; set; }
        /// <summary>
        /// 好评率
        /// </summary>
        public double? ShopGoodCommentRate { get; set; }

        /// <summary>
        /// 是不是天猫
        /// </summary>
        public bool? ShopIsTmall { get; set; }

        /// <summary>
        /// 是不是已经被授权加盟该品牌
        /// </summary>
        public bool? ShopIsAuthorized { get; set; }

        /// <summary>
        ///宝贝描述符合度
        /// </summary>
        public double? ShopItemMatchedScore { get; set; }

        /// <summary>
        /// 店铺服务满意度
        /// </summary>
        public double? ShopServiceScore { get; set; }

        /// <summary>
        /// 是不是被验证过 
        /// </summary>
        public bool? ShopIsCertified { get; set; }
        
       
        /// <summary>
        /// 认证
        /// </summary>
        public string Certification{get;set;}
        /// <summary>
        /// 创店时间
        /// </summary>
        public double? ShopSetupAt { get; set; }

        /// <summary>
        /// 店铺在网站上的id
        /// </summary>
        public string ShopID { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ShopTelephone { get; set; }

        ///<summary>
        ///联系人手机
        ///</summary>
        public string ShopPhone { get; set; }

        /// <summary>
        /// 店铺对应的公司
        /// </summary>
        public string ShopCompanyName { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ShotpContacts { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string ShopFax { get; set; }

        /// <summary>
        /// 最小订货量
        /// </summary>
        public string MOQ { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectNumber { get; set; }

        /// <summary>
        /// 招标方
        /// </summary>
        public string TenderSide { get; set; }

        /// <summary>
        /// 招标产品
        /// </summary>
        public string BiddingProducts { get; set; }

        /// <summary>
        /// 采购金额
        /// </summary>
        public string TenderMoney { get; set; }

        /// <summary>
        /// 投标截至时间
        /// </summary>
        public string DeadlineForSubmissionOfBids { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string ShopEmail { get; set; }


        BotStatus shopBotStatus = BotStatus.StructureChanged;
        public BotStatus ShopBotStatus { get { return shopBotStatus; } set { shopBotStatus = value; } }


        public Dictionary<string, string> ShopNameValues = new Dictionary<string, string>();

        public string ShopNameValueText { get { return string.Join("; ", ShopNameValues); } }

        #endregion 

        #region kunlun
        public string ShopCrc { get; set; }
        public string ItemCrc { get; set; }
        public string taskname { get; set; }
        #endregion 

        public Guid? SiteID { get; set; }
        public Guid? CompanyID { get; set; }
        public Guid? IBP_BrandID { get; set; }
        public Guid? BotItemID { get; set; }
        public Guid? BotShopID { get; set; }

     

        public void UpdateField(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            if (json.IsContain("{Status:")) 
            {
                if (json.IsContain("Removed"))
                    itemBotStatus = BotStatus.Removed;
                else if (json.IsContain("Timeout"))
                    itemBotStatus = BotStatus.Timeout;
                else if (json.IsContain("ServerError"))
                    itemBotStatus = BotStatus.Timeout;
                else if (json.IsContain("Ok"))
                    itemBotStatus = BotStatus.Ok;
                else if (json.IsContain("StructureChanged"))
                    itemBotStatus = BotStatus.StructureChanged;
            }

            if (json.IsContain("SoldNum:"))
                ItemSold30Days = json.SubAfter("SoldNum").ExInt();

            if (json.IsContain("Buyer:"))
            {
                string buyer = json.SubAfter(":").SubBefore("Product:").GetTrimed();
                if (string.IsNullOrEmpty(buyer))
                    return;
                string package = json.SubAfter("Product:").SubBefore("Price:").GetTrimed();
                if (string.IsNullOrEmpty(package))
                    return;
                double? price = json.SubAfter("Price:").SubBefore("Num:").ExDouble();
                if (!price.HasValue)
                    return;
                int? num = json.SubAfter("Num:").SubBefore("At:").ExInt();
                string at = json.SubAfter("At:").SubBefore("}").GetTrimed();
                if(string.IsNullOrEmpty(at))
                    return;
                DateTime dt=DateTime.ParseExact(at, "yyyy-MM-dd HH:mm:ss", null);
                if (SalesRecords.Any(x => x.Buyer == buyer && x.SettleDT == dt))
                    return;
                SalesRecords.Add(new XSalesRecord
                {
                    Buyer = buyer,
                    Count = num.Value,
                    Price = price.Value,
                    PakageName = package,
                    SettleDT=dt 
                });                
            }

            if (json.IsContain("Img:"))
            {
                string img = json.SubAfter("Img:").SubBefore("}").GetTrimed();
                if (!ProdImgs.Contains(img))
                    ProdImgs.Add(img);
            }

            if (json.IsContain("Poster:"))
            {
                string poster = json.SubAfter("Poster:").SubBefore("Content:").GetTrimed(); 
                if (string.IsNullOrEmpty(poster))
                    return;
                string content = json.SubAfter("Content:").SubAfter("At:");
                if (string.IsNullOrEmpty(content))
                    return;
                int? at = json.SubAfter("At:").SubBefore("}").GetDigital().GetTrimedStart('0').ToInt();
                if (!at.HasValue)
                    return;
                if (at < 10000)
                    at = DateTime.Now.Year * 10000 + at;
                if (ItemCommentList.Any(x => x.Poster == poster && x.PostAt == at))
                    return;
                ItemCommentList.Add(new ItemComment { PostAt=at , Poster=poster,Content=content });
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public string ShopContacts;
        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void NotifyAllChanges()
        {
            foreach (var p in this.GetType().GetProperties())
            {
                OnPropertyChanged(p.Name);
            }
        }
    }

    public class SubVersion
    {
        public string ItemID { get; set; }
        public string DetailUrl { get; set; }
        public string Version { get; set; }
        public string SiteName { get; set; }
    }
     
}
