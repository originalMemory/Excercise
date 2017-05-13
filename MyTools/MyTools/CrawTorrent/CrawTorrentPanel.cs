using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MyTools.Tools;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO;


namespace MyTools.CrawTorrent
{
    public partial class CrawTorrentPanel : Form
    {
        public CrawTorrentPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 种子信息列表
        /// </summary>
        List<Torrent> TorList = new List<Torrent>();

        /// <summary>
        /// 判断是否为剧场版、OVA和OAD
        /// </summary>
        List<string> TheaterKey = new List<string> { "剧场版", "OVA", "OAD" };
        /// <summary>
        /// 种子类别
        /// </summary>
        List<string> TorType = new List<string> { "生肉", "音乐", "漫画", "广播剧", "其他", "剧场版", "OVA", "OAD" };
        /// <summary>
        /// 种子类别枚举
        /// </summary>
        enum EnumType { 生肉, 音乐, 漫画, 广播剧, 其他,剧场版, OVA, OAD };

        string FloderPath=@"F:\普罗米修斯\2013\10月";

        #region 打开保存文件夹事件
        /// <summary>
        /// 路径获取对象
        /// </summary>
        FolderBrowserDialog dialog = new FolderBrowserDialog();

        //获取保存文件夹位置
        private void btn_SaveFolder_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择保存路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = FormatFileName(txt_name.Text);
                FloderPath = dialog.SelectedPath;
                txt_path.Text = Path.Combine(dialog.SelectedPath, fileName);
            }
        }

        private void btn_OpenFloder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txt_path.Text);
        }
        #endregion

        #region 下载种子文件
        private void btn_downFile_Click(object sender, EventArgs e)
        {
            btn_getInfo.Enabled = false;
            List<Torrent> downList = new List<Torrent>();
            //获取要下载的种子
            int num = TorList.Count;
            for (int i = 0; i < TorList.Count; i++)
            {
                bool isDown = (bool)dgv_main.Rows[i].Cells["IsDown"].Value;
                if (isDown)
                    downList.Add(TorList[i]);
            }
            Thread parameterThread = new Thread(new ParameterizedThreadStart(DownFile));
            parameterThread.Name = "Thread A:";
            parameterThread.Start(downList);  
            //DownFile();
        }
        #endregion

        //获取种子信息
        private void btn_getInfo_Click(object sender, EventArgs e)
        {
            btn_OpenFloder.Enabled = false;
            TorList.Clear();

            txt_path.Text = Path.Combine(FloderPath, FormatFileName(txt_name.Text));

            //dgv_main.DataSource = null;
            //获取网页源代码
            string baseUrl = "https://share.dmhy.org/topics/list?keyword=";     //搜索页面原始链接
            string pageUrl = "https://share.dmhy.org/topics/list/page/";        //后续页面链接
            string domain = "https://share.dmhy.org";
            string url = baseUrl + txt_name.Text;                               //第一页链接
            double threshold = Convert.ToDouble(txt_threshold.Text);
            //循环抓取数据
            int num = 2;
            while (true)
            {
                var html = WebApi.GetHtml(url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                /* 解析源代码 */
                //HtmlNode tbody = doc.DocumentNode.SelectSingleNode("//tbody");      //第1页搜索结果
                //tbody = HtmlNode.CreateNode(tbody.OuterHtml);
                HtmlNodeCollection tasks = doc.DocumentNode.SelectNodes("//*[@id=\"topic_list\"]/tbody/tr");                      //搜索结果数组
                //判断是否已经抓取完毕
                if (tasks == null)
                    break;
                foreach (var x in tasks)
                {
                    var infos = HtmlNode.CreateNode(x.OuterHtml);
                    Torrent tor = new Torrent();
                    tor.IsDown = true;
                    tor.PublishTime = infos.SelectSingleNode("/tr/td[1]/span").InnerText.Trim();
                    string type = infos.SelectSingleNode("/tr/td[2]/a").InnerText.Trim();
                    tor.Type = Microsoft.VisualBasic.Strings.StrConv(type, Microsoft.VisualBasic.VbStrConv.SimplifiedChinese);      //转换为简体中文
                    HtmlNode link = infos.SelectSingleNode("/tr/td[3]/a");
                    tor.Title = Microsoft.VisualBasic.Strings.StrConv(link.InnerText.Trim(), Microsoft.VisualBasic.VbStrConv.SimplifiedChinese);
                    tor.Url = domain+link.Attributes["href"].Value;
                    string size = infos.SelectSingleNode("/tr/td[5]").InnerText.Trim();
                    tor.Size = ParseSize(size);
                    TorList.Add(tor);
                }
                //判断是否已经抓取完毕
                if (tasks.Count<80)
                    break;
                url = pageUrl + num + "?keyword=" + txt_name.Text;
                num++;
            }
            
            //去除分集种子
            for (int i = 0; i < TorList.Count;i++)
            {
                if (TorList[i].Type != "动画" && TorList[i].Type != "ＲＡＷ")
                {
                    continue;
                }
                bool isDel = true;
                //判断大小，以防误删合集
                if (TorList[i].Size > threshold)
                {
                    continue;
                }
                //判断是否为剧场版、OVA或OAD
                foreach (var x in TheaterKey)
                {   
                    if (TorList[i].Title.ToUpper().Contains(x))
                    {
                        isDel = false;
                        break;
                    }
                }
                
                if (isDel)
                {
                    TorList.Remove(TorList[i]);
                    i--;
                    continue;
                }
            }
            TorList = TorList.OrderByDescending(x => x.Size).ToList();      //按大小排序
            //展示在表格中
            dgv_main.DataSource = TorList;
            if(dgv_main.Rows.Count>0)
                dgv_main.Rows[0].Selected = false; 
        }

        /// <summary>
        /// 转换大小，以MB为单位
        /// </summary>
        /// <param name="sizeStr">大小字符串</param>
        /// <returns></returns>
        private double ParseSize(string sizeStr)
        {
            //正则匹配，获取大小单位
            Regex regUnit = new Regex("GB|MB|KB");
            Regex regNum = new Regex(@"^\d+(\.\d{1,2})?");
            Match mt = regUnit.Match(sizeStr.ToUpper());
            if (mt.Success)
            {
                string unit = mt.Value;
                string temp = regNum.Match(sizeStr).Value;      //匹配大小数字
                double num = Convert.ToDouble(temp);
                switch (unit)
                {
                    case "GB":
                        num *= 1024;
                        break;
                    case "MB":
                        break;
                    case "KB":
                        num /= 1024;
                        break;
                }
                return num;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 下载图片线程
        /// </summary>
        /// <param name="DownList">下载列表</param>
        void DownFile(object para)
        {
            List<Torrent> downList = (List<Torrent>)para;
            int num = downList.Count;
            int i = 0;
            while (true)
            {
                //获取下载链接
                Torrent tor = new Torrent();
                if (i < downList.Count)
                {
                    tor = downList[i];
                    i++;
                }
                else
                {
                    break;
                }
                //解析下载页源码
                string html = WebApi.GetHtml(tor.Url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);
                HtmlNode tasks = doc.DocumentNode.SelectSingleNode("//*[@id=\"tabs-1\"]/a");                      //搜索结果数组
                if (tasks != null)
                {
                    string url = "https:" + tasks.Attributes["href"].Value;
                    string fileName = FormatFileName(tor.Title) + ".torrent";
                    string path = txt_path.Text;       //保存文件夹路径

                    //对种子分类
                    string type = null;
                    string drama = "广播剧";
                    switch (tor.Type)
                    {
                        case "动漫音乐":
                            {
                                //判断是否是广播剧
                                int index = tor.Title.IndexOf(drama);
                                if(index!=-1&&(tor.Title[index-1]!='附'&&tor.Title[index-1]!='&'))
                                    type = TorType[(int)EnumType.广播剧];
                                else
                                    type = TorType[(int)EnumType.音乐];
                                break;
                            }
                        case "ＲＡＷ":
                            type = TorType[(int)EnumType.生肉];
                            break;
                        case "漫画":
                            type = TorType[(int)EnumType.漫画];
                            break;
                        case "日文原版":
                            type = TorType[(int)EnumType.漫画];
                            break;
                        case "港台原版":
                            type = TorType[(int)EnumType.漫画];
                            break;
                        case "季度全集":
                        case "动画":
                            {
                                //判断是否为生肉
                                Regex regRaw = new Regex("raws|reinforce");
                                if (regRaw.IsMatch(tor.Title.ToLower()))
                                {
                                    type = TorType[(int)EnumType.生肉];
                                    break;
                                }
                                //判断是否是广播剧
                                int index = tor.Title.IndexOf(drama);
                                if (index != -1 && (tor.Title[index - 1] != '附' && tor.Title[index - 1] != '&')){
                                    type = TorType[(int)EnumType.广播剧];
                                    break;
                                }
                                //判断是否为剧场版、OVA或OAD
                                for (int j = 0; j < TheaterKey.Count; j++)
                                {
                                    int pos = tor.Title.ToUpper().IndexOf(TheaterKey[j]);
                                    if (pos!=-1)
                                    {
                                        if ((tor.Title[pos - 1] != '附' && tor.Title[pos - 1] != '+') && tor.Title[pos - 2] != '+')
                                        {
                                            type = TorType[(int)(EnumType)(j + 5)];
                                            break;
                                        }
                                    }
                                }
                                break;
                            }
                        case "其他":
                            type = TorType[(int)EnumType.其他];
                            break;
                    }
                    if (type != null)
                    {
                        path = Path.Combine(path, type);
                    }

                    if (!WebApi.DownloadFile(url,path,fileName))
                    {
                        MessageBox.Show(txt_name.Text + " - " + fileName + " 下载出错！", "提示");

                    }
                }
                int percent = i * 100 / num;
                if (bar_down.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它  
                    Action<int> actionDelegate = (x) => { this.bar_down.Value = x; };
                    // 或者  
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };  
                    bar_down.Invoke(actionDelegate, percent);
                }
                else
                {
                    this.bar_down.Value = percent;
                }
            }
            UpdateBtnStatus(true);
         
            this.Invoke((EventHandler)delegate { this.btn_getInfo.Enabled = true; });
            MessageBox.Show(txt_name.Text + " 下载完成！", "提示");
        }

        #region 跨线程修改按扭访问状态
        private delegate void UpdateBtnDelegate(bool status);
        void UpdateBtnStatus(bool status)
        {
            if (label1.InvokeRequired)
            {
                UpdateBtnDelegate md = new UpdateBtnDelegate(UpdateBtnStatus);
                label1.Invoke(md, new object[] { status });
                //label1.BeginInvoke(md, new object[] { message });  
            }
            else
            {
                btn_OpenFloder.Enabled = status;
            }
        }
        #endregion

        private void dgv_main_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (e.Clicks < 2)
            {
                
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                DataGridViewCell isDown = dgv_main.Rows[e.RowIndex].Cells["IsDown"];
                if ((bool)isDown.Value)
                    isDown.Value = false;
                else
                    isDown.Value = true;
            }
        }

        /// <summary>
        /// 去除文件名中非法字符
        /// </summary>
        /// <param name="originNmae">原文件名</param>
        /// <returns>格式化文件名</returns>
        public string FormatFileName(string originNmae)
        {
            string check = "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]";        //检查文件名是否含有非法字符
            return Regex.Replace(originNmae, check, "_");
        }
    }


    /// <summary>
    /// 种子信息类
    /// </summary>
    public class Torrent
    {
        /// <summary>
        /// 是否下载
        /// </summary>
        public bool IsDown { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public string PublishTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public double Size { get; set; }
        /// <summary>
        /// 下载页面地址
        /// </summary>
        public string Url { get; set; }
    }
}
