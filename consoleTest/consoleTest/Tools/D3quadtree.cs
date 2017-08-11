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
        public QuadtreeRoot tree = new QuadtreeRoot();

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
                x0 = tree._x0;
                x1 = tree._x1;
            }
            if (y1 < y0)
            {
                y0 = tree._y0;
                y1 = tree._y1;
            }

            //扩展树以覆盖新节点
        }

        void cover(double x, double y)
        {
            if (x == 0.0 || y == 0.0)
                return;

            double x0 = tree._x0;
            double x1 = tree._x1;
            double y0 = tree._y0;
            double y1 = tree._y1;

            if (x0 == 0.0 && y0 == 0.0)
            {
                x0 = Math.Floor(x);
                x1 = x0 + 1;
                y0 = Math.Floor(y);
                y1 = y0 + 1;
            }
            else if (x0 > x || x > x1 || y0 > y || y > y1)
            {
                var z = x1 - x0;
                var node = tree._root;

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

                switch (i)
                {
                    case 0:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            node.children[i] = parent;
                            node.IsValue = true;
                            z *= 2;
                            x1 = x0 + z;
                            y1 = y0 + z;
                        } while (x>x1||y>y1);
                        break;
                    case 1:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            node.children[i] = parent;
                            node.IsValue = true;
                            z *= 2;
                            x1 = x0 - z;
                            y1 = y0 + z;
                        } while (x > x1 || y > y1);
                        break;
                    case 2:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            node.children[i] = parent;
                            node.IsValue = true;
                            z *= 2;
                            x1 = x0 + z;
                            y1 = y0 - z;
                        } while (x > x1 || y > y1);
                        break;
                    case 3:
                        do
                        {
                            QuadtreeNode parent = new QuadtreeNode();
                            node.children[i] = parent;
                            node.IsValue = true;
                            z *= 2;
                            x1 = x0 - z;
                            y1 = y0 - z;
                        } while (x > x1 || y > y1);
                        break;
                    default:
                        break;
                }
            }
            if (!tree._root.IsValue)
            {

            }
        }
    }
}
