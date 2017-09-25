using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTest.Model;

using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using AISSystem;

namespace CSharpTest.Tools
{
    //回调函数
    public delegate bool CallBackHandler3(QuadtreeNode quad, double x1, double x2);
    public delegate void CallBackHandler5(QuadtreeNode quad, double x0, double y0, double x1, double y1);
    public class D3force
    {
        /// <summary>
        /// 析构函数
        /// </summary>
        /// <param name="filePath">Json文件地址</param>
        /// <param name="cWidth">绘图宽度</param>
        /// <param name="cHeight">绘图高度</param>
        public D3force(string filePath,double cWidth,double cHeight)
        {
            this.cWidth = cWidth;
            this.cHeight = cHeight;
            StreamReader sr = new StreamReader(filePath);
            string str = sr.ReadToEnd();
            sr.Close();
            JObject json = JObject.Parse(str);
            JArray jnodes = (JArray)json["nodes"];
            int i=0;
            foreach (var x in jnodes)
            {
                ForceNode node = new ForceNode
                {
                    group = Convert.ToInt32(x["group"]),
                    id = x["id"].ToString(),
                    index=i,
                };
                Nodes.Add(node);
                i++;
            }
            JArray jlinks = (JArray)json["links"];
            int j=0;
            foreach (var x in jlinks)
            {
                ForceLink link = new ForceLink
                {
                    index = j,
                    value = Convert.ToInt32(x["value"]),
                };
                string source = x["source"].ToString();
                ForceNode sn = Nodes.Find(s => s.id == source);
                if (sn != null)
                {
                    link.source = sn;
                }
                string target = x["target"].ToString();
                ForceNode tn = Nodes.Find(s => s.id == target);
                if (tn != null)
                {
                    link.target = tn;
                }
                Links.Add(link);
                j++;
            }

            //初始化结点
            initializeNodes();
            initializeForceLink();
            initializeManyBody();
        }

        public void StartCompute()
        {
            //计算数据
            alpha += (alphaTarget - alpha) * alphaDecay;
            tick();
            //生成结点和链接坐标表
            HSSFWorkbook workBook = new HSSFWorkbook();
            ISheet nodeSheet = workBook.CreateSheet("结点");
            IRow rowHead = nodeSheet.CreateRow(0);
            rowHead.CreateCell(0).SetCellValue("结点序号");
            rowHead.CreateCell(1).SetCellValue("结点名称");
            rowHead.CreateCell(2).SetCellValue("结点分组");
            rowHead.CreateCell(3).SetCellValue("结点X坐标");
            rowHead.CreateCell(4).SetCellValue("结点Y坐标");
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                var row = nodeSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(node.index);
                row.CreateCell(1).SetCellValue(node.id);
                row.CreateCell(2).SetCellValue(node.group);
                row.CreateCell(3).SetCellValue(node.x);
                row.CreateCell(4).SetCellValue(node.y);
            }

            ISheet linkSheet = workBook.CreateSheet("链接");
            IRow rowHead2 = linkSheet.CreateRow(0);
            rowHead2.CreateCell(0).SetCellValue("链接序号");
            rowHead2.CreateCell(1).SetCellValue("链接起始点序号");
            rowHead2.CreateCell(2).SetCellValue("链接起始点名称");
            rowHead2.CreateCell(3).SetCellValue("链接结点点序号");
            rowHead2.CreateCell(4).SetCellValue("链接结束点名称");
            rowHead2.CreateCell(5).SetCellValue("链接强度");
            for (int i = 0; i < Links.Count; i++)
            {
                var link = Links[i];
                var row = linkSheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(link.index);
                row.CreateCell(1).SetCellValue(link.source.index);
                row.CreateCell(2).SetCellValue(link.source.id);
                row.CreateCell(3).SetCellValue(link.target.index);
                row.CreateCell(4).SetCellValue(link.target.id);
                row.CreateCell(5).SetCellValue(link.value);
            }
            var filePath = "result.xls";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                workBook.Write(fs);
                CommonTools.Log("生成完毕");
            }
        }

        //double x(ForceNode node)
        //{
        //    return node.x + node.vx;
        //}

        //double y(ForceNode node)
        //{
        //    return node.y + node.vy;
        //}
        
        #region simnulation，模拟器
        /// <summary>
        /// 节点属性
        /// </summary>
        public List<ForceNode> Nodes = new List<ForceNode>();
        /// <summary>
        /// 衰减值
        /// </summary>
        double alpha = 1;
        /// <summary>
        /// 最小衰减值
        /// </summary>
        double alphaMin = 0.001; 
        /// <summary>
        /// 衰减减小率
        /// </summary>
        double alphaDecay = 1 - Math.Pow(0.001, (double)1 / 300);
        /// <summary>
        /// 目标衰减值
        /// </summary>
        double alphaTarget = 0;
        /// <summary>
        /// 速度减小率
        /// </summary>
        double velocityDecay = 0.6;
        /// <summary>
        /// 初始半径
        /// </summary>
        double initialRadius = 10;
        /// <summary>
        /// 初始角度
        /// </summary>
        double initialAngle = Math.PI * (3 - Math.Sqrt(5));

        /// <summary>
        /// 初始化节点
        /// </summary>
        public void initializeNodes()
        {
            for (int i = 0,n=Nodes.Count; i < n; i++)
            {
                var node = Nodes[i];
                var radius = initialRadius * Math.Sqrt(i);
                var angle = i * initialAngle;
                node.x = radius * Math.Cos(angle);
                node.y = radius * Math.Sin(angle);
            }
        }

        /// <summary>
        /// 监听函数，不断调用计算新位置
        /// </summary>
        public void tick()
        {
            //重计算衰减值
            while (alpha > alphaMin)
            {
                alpha += (alphaTarget - alpha) * alphaDecay;
                //计算结点位置和速度
                forceLink();
                forceManyBody();
                forceCenter();

                for (int i = 0; i < Nodes.Count; i++)
                {
                    var node = Nodes[i];
                    node.x += node.vx *= velocityDecay;
                    node.y += node.vy *= velocityDecay;
                }
                CommonTools.Log("当前衰减率 - {0}".FormatStr(alpha));
            }
        }

        /// <summary>
        /// 返回与距离位置<x，y> 给定搜索半径最接近的节点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius">半径</param>
        /// <returns></returns>
        ForceNode find(double x, double y, double radius)
        {
            ForceNode closest = null;
            if (radius == null)
                radius = double.MaxValue;
            else
                radius *= radius;
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                double dx = x - node.x;
                double dy = y - node.y;
                double d2 = dx * dx + dy * dy;
                if (d2 < radius)
                {
                    closest = node;
                    radius = d2;
                }
            }
            if (closest != null)
                return closest;
            else
                return null;
        }
        #endregion

        #region  forceLink,结点链接相关
        /// <summary>
        /// 节点间链接
        /// </summary>
        List<ForceLink> Links = new List<ForceLink>();
        /// <summary>
        /// 节点Index索引
        /// </summary>
        Dictionary<ForceNode, int> nodeById = new Dictionary<ForceNode, int>();
        /// <summary>
        /// 节点链接个数
        /// </summary>
        List<double> count = new List<double>();
        List<double> bias = new List<double>();
        /// <summary>
        /// 链接强度
        /// </summary>
        List<double> strengthsLink = new List<double>();
        /// <summary>
        /// 链接距离
        /// </summary>
        List<double> distancesLink = new List<double>();
        /// <summary>
        /// 默认距离
        /// </summary>
        double defaultDistanceLink = 30.0;
        int iterations = 1;

        /// <summary>
        /// 获取节点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int index(ForceNode node)
        {
            return node.index;
        }

        /// <summary>
        /// 获取链接的默认距离
        /// </summary>
        /// <param name="link">链接</param>
        /// <returns>默认距离</returns>
        public double defaultStrength(ForceLink link)
        {
            return 1 / Math.Min(count[link.source.index], count[link.target.index]);
        }

        /// <summary>
        /// 计算链接上结点速度
        /// </summary>
        public void forceLink()
        {
            alpha = Math.Round(alpha, 15);
            for (int k = 0; k < iterations; k++)
            {
                for (int i = 0; i < Links.Count; i++)
                {
                    ForceLink link = Links[i];
                    ForceNode source = link.source, target = link.target;
                    double x, y, l;
                    //计算下一步的节点速度
                    x = target.x + target.vx - source.x - source.vx;
                    if (x == 0)     //此处有疑问
                        x = jiggle();
                    y = target.y + target.vy - source.y - source.vy;
                    if (y == 0)
                        y = jiggle();
                    l = Math.Sqrt(x * x + y * y);
                    l = (l - distancesLink[i]) / l * alpha * strengthsLink[i];
                    x *= l;
                    y *= l;
                    double b = bias[i];
                    target.vx -= x * b;
                    target.vy -= y * b;
                    b = 1 - b;
                    source.vx += x * b;
                    source.vy += y * b;
                }
            }
        }

        public double jiggle()
        {
            Random r = new Random();
            double dr = (r.NextDouble() - 0.5) * 0.000006;
            return dr;
        }

        /// <summary>
        /// 初始化力链接
        /// </summary>
        public void initializeForceLink()
        {
            int n = Nodes.Count, m = Links.Count;
            //初始化Count
            for (int i = 0; i < n; i++)
            {
                count.Add(0.0);
            }
            for (int i = 0; i < m; i++)
            {
                var link = Links[i];
                count[link.source.index]++;
                count[link.target.index]++;
            }
            //初始化bias
            for (int i = 0; i < m; i++)
            {
                bias.Add(0.0);
            }
            for (int i = 0; i < m; i++)
            {
                var link = Links[i];
                bias[i] = count[link.source.index] / (count[link.source.index] + count[link.target.index]);
            }
            initializeStrength();
            initializeDistance();

        }

        /// <summary>
        /// 初始化链接强度
        /// </summary>
        public void initializeStrength()
        {
            for (int i = 0; i < Links.Count; i++)
            {
                double strength = defaultStrength(Links[i]);
                strengthsLink.Add(strength);
            }
        }

        /// <summary>
        /// 初始化链接距离
        /// </summary>
        public void initializeDistance()
        {
            for (int i = 0; i < Links.Count; i++)
            {
                double distance = defaultDistanceLink;
                distancesLink.Add(distance);
            }
        }
        #endregion

        #region forceManyBody,计算结点间相互作用力
        /// <summary>
        /// 链接强度
        /// </summary>
        List<double> strengthsManyBody = new List<double>();
        /// <summary>
        /// 链接距离
        /// </summary>
        List<double> distancesManyBody = new List<double>();
        double defaultStrengthManyBody = -30.0;
        double distanceMin2 = 1;
        double distanceMax2 = Double.MaxValue;
        double theta2 = .81;

        public void forceManyBody()
        {
            int n = Nodes.Count;
            D3quadtree tree = new D3quadtree(Nodes);
            tree.visitAfter(new CallBackHandler5(accumulate));
            for (int i = 0; i < n; i++)
            {
                applyNode = Nodes[i];
                tree.visit(new CallBackHandler3(apply));
            }
            
        }

        public void initializeManyBody()
        {
            if (Nodes.Count == 0)
                return;
            for (int i = 0; i < Nodes.Count; i++)
            {
                strengthsManyBody.Add(0.0);
            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                strengthsManyBody[node.index] = +defaultStrengthManyBody;
            }
        }


        public void accumulate(QuadtreeNode quad, double x0, double y0, double x1, double y1)
        {
            double x = 0, y = 0, strength = 0, c;
            QuadtreeNode q;
            //对于内部节点，积累来自子象限的力。
            if (quad.children != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (quad.children[i] != null)
                    {
                        q = quad.children[i];
                        c = q.value;
                        strength += c;
                        x += c * q.x;
                        y += c * q.y;
                    }
                }
                quad.x = x / strength;
                quad.y = y / strength;
            }
            //对叶结点，积累来自重合象限的力
            else
            {
                q = quad;
                q.x = q.data.x;
                q.y = q.data.y;
                strength += strengthsManyBody[q.data.index];
            }
            quad.value = strength;
        }

        ForceNode applyNode;
        public bool apply(QuadtreeNode quad, double x1, double x2)
        {
            if (quad.value == 0)
                return true;

            double x = quad.x - applyNode.x,
             y = quad.y - applyNode.y,
             w = x2 - x1,
             l = x * x + y * y;

            //如果可能则使用Barnes-Hut近似
            //限制距离很近的节点，对重合的结点使用随机方向
            if (w * w / theta2 < 1)
            {
                if (l < distanceMax2)
                {
                    if (x == 0)
                    {
                        x = jiggle();
                        l += x * x;
                    }
                    if (y == 0)
                    {
                        y = jiggle();
                        l += y * y;
                    }
                    if (l < distanceMin2)
                    {
                        l = Math.Sqrt(distanceMin2 * l);
                    }
                    applyNode.vx += x * quad.value * alpha / l;
                    applyNode.vy += y * quad.value * alpha / l;
                }
                return true;
            }
            //否则，直接处理点
            else if (quad.children != null || l >= distanceMax2)
                return false;

            //限制距离很近的节点，对重合的结点使用随机方向
            if (!IsEqual(quad.data, applyNode))
            {
                if (x == 0)
                {
                    x = jiggle();
                    l += x * x;
                }
                if (y == 0)
                {
                    y = jiggle();
                    l += y * y;
                }
                if (l < distanceMin2)
                {
                    l = Math.Sqrt(distanceMin2 * l);
                }
                w = strengthsManyBody[quad.data.index] * alpha / l;
                applyNode.vx += x * w;
                applyNode.vy += y * w;
            }
            return false;
        }

        bool IsEqual(ForceNode source, ForceNode target)
        {
            if (source.group == target.group && source.id == target.id && source.index == target.index &&
                source.vx == target.vx && source.vy == target.vy && source.x == target.x && source.y == target.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region center,计算中心点
        /// <summary>
        /// 区域宽度
        /// </summary>
        double cWidth;
        /// <summary>
        /// 区域高度
        /// </summary>
        double cHeight;
        /// <summary>
        /// 计算中心点力
        /// </summary>
        public void forceCenter()
        {
            double sx = .0, sy = .0;
            int n = Nodes.Count;
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                sx += node.x;
                sy += node.y;
            }
            sx = sx / n - cWidth;
            sy = sy / n - cHeight;
            for (int i = 0; i < n; i++)
            {
                var node = Nodes[i];
                node.x -= sx;
                node.y -= sy;
            }
        }
        
        #endregion

        #region x$2
        

        #endregion
    }
}
