using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTest.Model
{
    /// <summary>
    /// 四叉树
    /// </summary>
    public class QuadtreeRoot
    {
        /// <summary>
        /// 根结点
        /// </summary>
        public QuadtreeNode _root;
        /// <summary>
        /// x坐标左值
        /// </summary>
        public double _x0;
        /// <summary>
        /// y坐标下值
        /// </summary>
        public double _y0;
        /// <summary>
        /// x坐标右值
        /// </summary>
        public double _x1;
        /// <summary>
        /// y坐标上值
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
        public ForceNode data;
        //public QuadtreeNode()
        //{
        //    children = new QuadtreeNode[4];
        //    data = new ForceNode[4];
        //}
        /// <summary>
        /// 判断本节点是分支结点还是叶子结点
        /// </summary>
        public bool IsLeaf;
    }
}
