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

        /// <summary>
        /// 节点属性
        /// </summary>
        public List<ForceNode> Nodes = new List<ForceNode>();
        /// <summary>
        /// 节点间链接
        /// </summary>
        public List<ForceLink> Links = new List<ForceLink>();
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
        double initialRadius = 10;
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
    }
}
