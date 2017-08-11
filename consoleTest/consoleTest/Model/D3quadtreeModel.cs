using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    public class QuadtreeRoot
    {
        public QuadtreeNode _root;
        /// <summary>
        /// 最小的x坐标
        /// </summary>
        public double _x0;
        /// <summary>
        /// 最小的y坐标
        /// </summary>
        public double _y0;
        /// <summary>
        /// 最大的x坐标
        /// </summary>
        public double _x1;
        /// <summary>
        /// 最大的y坐标
        /// </summary>
        public double _y1;
    }

    /// <summary>
    /// 四叉树节点
    /// </summary>
    public class QuadtreeNode
    {
        /// <summary>
        /// 子节点
        /// </summary>
        public QuadtreeNode[] children;
        /// <summary>
        /// 节点数据
        /// </summary>
        public ForceNode[] data;
        //public QuadtreeNode()
        //{
        //    children = new QuadtreeNode[4];
        //    data = new ForceNode[4];
        //}
        /// <summary>
        /// 判断本节点是否有值
        /// </summary>
        public bool IsValue;
    }
}
