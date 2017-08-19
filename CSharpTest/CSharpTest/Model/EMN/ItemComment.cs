using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cPlusPlusTest.Models
{
    public class ItemComment
    {
        public string Poster { get; set; }//发布评论的人
        public string Content { get; set; }//评论的内容
        public double? PostAt { get; set; }//发布时间
        public string Location { get; set; }//发布人所在地
        public Guid? EntityID { get; set; }
        public Guid ID { get; set; }
        public int? Stars { get; set; }
        public string PosterContactUrl { get; set; }

    }
    public enum CommentAttitude
    {
        Bad,//差评
        SoSo,//中评
        Good,//好评
    }

    public class ItemCommentSummary
    {
        public string Content { get; set; }//评论汇总内容
        public int Comments { get; set; }//有多少条评论属于该汇总
    }
}
