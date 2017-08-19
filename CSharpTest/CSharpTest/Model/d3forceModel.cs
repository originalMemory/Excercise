using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    /// <summary>
    /// 力导向图结点
    /// </summary>
    public class ForceNode
    {
        /// <summary>
        /// 分组
        /// </summary>
        public int group;
        /// <summary>
        /// 节点名
        /// </summary>
        public string id;
        /// <summary>
        /// 节点序号
        /// </summary>
        public int index;
        /// <summary>
        /// 节点X方向速度
        /// </summary>
        public double vx;
        /// <summary>
        /// 节点Y方向速度
        /// </summary>
        public double vy;
        /// <summary>
        /// 节点X坐标
        /// </summary>
        public double x;
        /// <summary>
        /// 节点Y坐标
        /// </summary>
        public double y;
    }

    //力导向图链接
    public class ForceLink
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int index;
        /// <summary>
        /// 关联度
        /// </summary>
        public int value;
        /// <summary>
        /// 源节点
        /// </summary>
        public ForceNode source;
        /// <summary>
        /// 目标节点名
        /// </summary>
        public ForceNode target;
    }

    //json内节点属性
    public class SourceNode
    {
        /// <summary>
        /// 分组
        /// </summary>
        public int group;
        /// <summary>
        /// 节点名
        /// </summary>
        public string id;
    }

    //json内链接属性
    public class SourceLink
    {
        /// <summary>
        /// 关联度
        /// </summary>
        public int value;
        /// <summary>
        /// 源节点名
        /// </summary>
        public string source;
        /// <summary>
        /// 目标节点名
        /// </summary>
        public string target;
    }
}
