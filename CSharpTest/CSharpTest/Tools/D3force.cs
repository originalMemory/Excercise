using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpTest.Model;

using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CSharpTest.Tools
{
    //回调函数
    public delegate void CallBackHandler(QuadtreeNode quad);
    public delegate bool CallBackHandler5(QuadtreeNode quad, int x0, int y0, int x1, int y1);
    public class D3force
    {
        public D3force()
        {
            StreamReader sr = new StreamReader("miserables.json");
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

        }

        double x(ForceNode node)
        {
            return node.x + node.vx;
        }

        double y(ForceNode node)
        {
            return node.y + node.vy;
        }
        
        #region simnulation，模拟器
        /// <summary>
        /// 节点属性
        /// </summary>
        public List<ForceNode> Nodes = new List<ForceNode>();
        /// <summary>
        /// 衰减值
        /// </summary>
        double alpha = 0;
        /// <summary>
        /// 最小衰减值
        /// </summary>
        double alphaMin = 0.001; 
        /// <summary>
        /// 衰减减小率
        /// </summary>
        double alphaDecay = Math.Pow(0.001, 1 / 300);
        /// <summary>
        /// 目标衰减值
        /// </summary>
        double alphaTarget = 0;
        /// <summary>
        /// 速度减小率
        /// </summary>
        double velocityDecay = 0.6;
        Dictionary<string, string> forces = new Dictionary<string, string>();
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
            alpha += (alphaTarget - alpha) * alphaDecay;
            //计算结点位置和速度
            forceLink(alpha);

            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                node.x += node.vx *= velocityDecay;
                node.y += node.vy *= velocityDecay;
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
        /// <param name="alpha">衰减值</param>
        public void forceLink(double alpha)
        {
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
                    target.vy += y * b;
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
        public void initializeForce()
        {
            int n = Nodes.Count, m = Links.Count;
            //初始化Count
            for (int i = 0; i < n; i++)
            {
                count.Add(0.0);
            }
            for (int i = 0; i < n; i++)
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
            tree.visitAfter(new CallBackHandler(accumulate));

            
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


        public void accumulate(QuadtreeNode quad)
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
                    quad.x = x / strength;
                    quad.y = y / strength;
                }
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

        public bool apply(QuadtreeNode quad)
        {
            return true;
        }
        #endregion

        #region center,计算中心点
        /// <summary>
        /// 计算中心点力
        /// </summary>
        /// <param name="cWidth"></param>
        /// <param name="cHeight"></param>
        public void forceCenter(int cWidth,int cHeight)
        {
            double sx = .0, sy = .0;
            int n = Nodes.Count;
            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                sx += node.x;
                sx += node.y;
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
