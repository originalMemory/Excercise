using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTest.Model;

namespace CSharpTest.Tools
{
    class D3quadtree
    {
        /// <summary>
        /// 四叉树
        /// </summary>
        public QuadtreeRoot Tree = new QuadtreeRoot();

        /// <summary>
        /// 初始化四叉树
        /// </summary>
        /// <param name="nodes">节点</param>
        /// <returns></returns>
        public QuadtreeRoot quadtree(List<ForceNode> nodes)
        {
            var tree = new QuadtreeRoot();
            if (nodes.Count > 0)
            {

            }
            return tree;
        }

        /// <summary>
        /// 添加所有节点
        /// </summary>
        /// <param name="data">节点列表</param>
        void addAll(List<ForceNode> data)
        {
            int n = data.Count;
            double x = 0.0, y = 0.0;        //当前节点的x,y坐标
            double x0 = double.MaxValue, y0 = double.MaxValue;      //最小的x,y坐标
            double x1 = double.MinValue, y1 = double.MinValue;      //最大的x,y坐标

            var xz = new List<double>();
            var yz = new List<double>();
            for (int i = 0; i < n; i++)
            {
                xz.Add(0.0);
                yz.Add(0.0);
            }
            //初始化及坐标及极值
            for (int i = 0; i < n; i++)
            {
                var d = data[i];
                x = d.x;
                y = d.y;
                if (x < x0)
                    x0 = x;
                if (x > x1)
                    x1 = x;
                if (y < y0)
                    y0 = y;
                if (y > y1)
                    y1 = y;
            }

            //如果极值不存在，则使用已存在的极值
            if (x1 < x0)
            {
                x0 = Tree._x0;
                x1 = Tree._x1;
            }
            if (y1 < y0)
            {
                y0 = Tree._y0;
                y1 = Tree._y1;
            }

            //扩展树以覆盖新节点
            cover(x0, y0);
            cover(x1, y1);

            //添加新节点
        }

        /// <summary>
        /// 扩展树以覆盖新节点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void cover(double x, double y)
        {
            if (x == 0.0 || y == 0.0)
                return;

            double x0 = Tree._x0;
            double x1 = Tree._x1;
            double y0 = Tree._y0;
            double y1 = Tree._y1;

            if (x0 == x1 && y0 == y1)
            {
                x0 = Math.Floor(x);
                x1 = x0 + 1;
                y0 = Math.Floor(y);
                y1 = y0 + 1;
            }
            //判断点是否不在现有四叉树范围内
            else if (x0 > x || x > x1 || y0 > y || y > y1)
            {
                //不再其范围内则扩展4四叉树至包含该点
                var z = x1 - x0;            //叶子节点间隔
                if (Tree._root == null)
                    Tree._root = new QuadtreeNode();
                var node = Tree._root;

                //判断节点类型
                //(y < (y0 + y1) / 2) << 1 | (x < (x0 + x1) / 2)
                bool isY = y < (y0 + y1) / 2;
                bool isX = x < (x0 + x1) / 2;
                int i = 0;
                if (isY)
                    i = 1 << 1;
                else
                    i = 0 << 1;
                if (isX)
                    i = i | 1;
                else
                    i = i | 0;

                //初始化节点分布
                switch (i)
                {
                    case 0:
                        do
                        {
                            QuadtreeNode child = new QuadtreeNode();
                            if (node.children == null)
                            {
                                node.children = new QuadtreeNode[4];
                            }
                            node.children[i] = child;
                            z *= 2;
                            x1 = x0 + z;
                            y1 = y0 + z;
                            
                        } while (x>x1||y>y1);
                        break;
                    case 1:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            if (node.children == null)
                            {
                                node.children = new QuadtreeNode[4];
                            }
                            node.children[i] = parent;
                            z *= 2;
                            x1 = x0 - z;
                            y1 = y0 + z;
                        } while (x > x1 || y > y1);
                        break;
                    case 2:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            if (node.children == null)
                            {
                                node.children = new QuadtreeNode[4];
                            }
                            node.children[i] = parent;
                            z *= 2;
                            x1 = x0 + z;
                            y1 = y0 - z;
                        } while (x > x1 || y > y1);
                        break;
                    case 3:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            if (node.children == null)
                            {
                                node.children = new QuadtreeNode[4];
                            }
                            node.children[i] = parent;
                            z *= 2;
                            x1 = x0 - z;
                            y1 = y0 - z;
                        } while (x > x1 || y > y1);
                        break;
                    default:
                        break;
                }
                Tree._root = node;
            }
            Tree._x0 = x0;
            Tree._x1 = x1;
            Tree._y0 = y0;
            Tree._y1 = y1;
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="x">节点x坐标</param>
        /// <param name="y">节点y坐标</param>
        /// <param name="d">节点数据</param>
        void add(double x, double y, ForceNode d)
        {
            if (x == 0.0 && y == 0.0)
                return;
            var node = Tree._root;
            QuadtreeNode parent = node;
            var leaf = new QuadtreeNode
            {
                IsLeaf = true,
                data = d
            };
            double x0 = Tree._x0, x1 = Tree._x1, y0 = Tree._y0, y1 = Tree._y1;
            double xm;      //四叉树当前区块x轴中点
            double ym;      //四叉树当前区块y轴中点
            double xp;      //数据结点x坐标
            double yp;      //数据结点y坐标
            double right;
            double bottom;
            int i = 0;      //新结点在四叉树子节点列表中的位置
            int j = 0;      //旧结点在四叉树子节点列表中的位置

            //如果根节点为空，初始化根节点为叶结点
            if (node == null)
            {
                Tree._root = leaf;
                return;
            }

            //找出新叶结点应在坐标，为其赋值
            while (!node.IsLeaf)    //当未查找至叶子节点时，不断计算
            {
                //判断叶新节点位于四叉树哪一个区块
                right = x;
                xm = (x0 + x1) / 2;
                if (right >= xm)
                    x0 = xm;
                else
                    x1 = xm;

                bottom = y;
                ym = (y0 + y1) / 2;
                if (bottom >= ym)
                    y0 = ym;
                else
                    y1 = ym;

                i = (int)bottom << 1 | (int)right;  //新结点在子节点中的排序

                //当前坐标节点为空时，添加叶结点
                if (node.children == null)
                {
                    node.children = new QuadtreeNode[4];
                    node.children[i] = leaf;
                    return;
                }
                parent = node;
                node = node.children[i];
            }

            xp = node.data.x;
            yp = node.data.y;

            //判断新结点是否与已存在的节点的坐标完全一致
            if (node.IsLeaf)
            {
                if (x == xp && y == yp)
                {
                    if (parent != null)
                    {
                        if (parent.children == null)
                            parent.children = new QuadtreeNode[4];
                        parent.children[i] = leaf;
                    }
                    else
                        Tree._root = leaf;
                    return;
                }
            }

            //切割叶结点，直到新旧节点并存
            do
            {
                if (parent.children == null)
                    parent.children = new QuadtreeNode[4];

                //判断叶新节点位于四叉树哪一个区块
                right = x;
                xm = (x0 + x1) / 2;
                if (right >= xm)
                    x0 = xm;
                else
                    x1 = xm;

                bottom = y;
                ym = (y0 + y1) / 2;
                if (bottom >= ym)
                    y0 = ym;
                else
                    y1 = ym;

                i = (int)bottom << 1 | (int)right;  //新结点在子节点中的排序
                bool isOldY = yp >= ym;
                bool isOldX = xp >= xm;
                if (isOldY)
                    j = 1 << 1;
                else
                    j = 0 << 1;
                if (isOldX)
                    j = j | 1;
                else
                    j = j | 0;

            } while (i == j);
            //清除原有数据，并赋新值
            parent = node;
            var oldNode = new QuadtreeNode
            {
                IsLeaf = true,
                data = node.data
            };
            parent.data = null;
            parent.IsLeaf = false;
            parent.children[j] = oldNode;
            parent.children[i] = leaf;
        }
    }
}
