using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;

namespace MyTools.CrawImg
{
    public partial class AddressDown : Form
    {
        bool downStart = true;
        List<ImgList> imgList = new List<ImgList>();        

        public AddressDown()
        {
            InitializeComponent();
            ThreadPool.SetMaxThreads(1, 1);
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(DownImg).Start();
        }

        #region 选择保存位置，返回路径
        private void btn_SaveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择保存路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_FloderPath.Text = dialog.SelectedPath;
            }
        }
        #endregion

        #region 打开保存文件夹事件
        private void btn_OpenFloder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txt_FloderPath.Text);
        }
        #endregion

        #region 保存文件，路径不存在则创建路径后再保存，全部保存后激活打开文件夹按钮
        private void btn_SaveImages_Click(object sender, EventArgs e)
        {
            //移除空白字符
            string path = txt_FloderPath.Text.Trim();

            //判断文件夹是否存在，不存在则创建文件夹后继续。若路径名含有不能使用字符，返回错误信息
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //保存描述
            //string titleCheck = Regex.Replace(img.Author, "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]", "_");
            string title = Regex.Match(path, ".*\\\\(?<name>.+)").Groups["name"].Value.ToString();
            string infoPath = path + "\\" + title;
            string value = txt_words.Text;
            FileStream fs = new FileStream(infoPath + ".txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteAsync(value);
            sw.Flush();
            sw.Close();
            fs.Close();

            //保存链接
            using (StreamWriter writer = new StreamWriter(infoPath + ".url"))
            {
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine("URL=" + txt_Url.Text);
                writer.WriteLine(@"IconFile=C:\Windows\system32\SHELL32.dll");
                writer.WriteLine("IconIndex=13");
                writer.Flush();
            }

            //获取链接
            string source = txt_address.Text;
            Regex linkreg = new Regex(@"<img class='detail_std detail_clickable' src='(?<info>.+?)/w650' />");
            MatchCollection urls = linkreg.Matches(source);

            foreach (Match x in urls)
            {
                string str;
                str = x.Groups["info"].Value;
                if (!string.IsNullOrEmpty(str))
                {
                    ImgList temp = new ImgList();
                    temp.url = str;
                    temp.path = path;
                    imgList.Add(temp);
                }
            }

            //激活打开文件夹按钮
            btn_OpenFloder.Enabled = true;
        }
        #endregion

        int status = 0;
        bool isFirst = true;

        void DownImg()
        {

            while (downStart)
            {
                ImgList img = GetUrl();
                if (img == null)
                {
                    if (!isFirst && status == 2)
                    {
                        MessageBox.Show("全部下载任务已完成！", "提示");
                        status = 0;
                        isFirst = true;
                    }
                    Thread.Sleep(3000);
                    continue;
                }
                string name = Regex.Match(img.url, ".*/(?<name>.+)").Groups["name"].ToString();
                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(img.url), img.path + "\\" + name);
                }
            }
        }

        ImgList GetUrl()
        {
            if (imgList.Count > 0)
            {
                ImgList url = imgList[0];
                imgList.Remove(url);
                if (isFirst)
                {
                    status = 1;
                    isFirst = false;
                }
                return url;
            }
            else
            {
                if (!isFirst)
                {
                    status = 2;
                }
                return null;
            }
        }
    }
}
