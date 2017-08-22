using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Threading;
using MyTools.Tools;

namespace MyTools.CrawImg
{
    public partial class CrawImgPanel : Form
    {
        //通过窗口的类名或者窗口标题的名字来查找窗口句柄
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        //向窗口传递命令
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        //关闭窗口指令
        public const int WM_CLOSE = 0x10;

        /// <summary>
        /// 判断是否开始下载
        /// </summary>
        bool downStart = true;

        public CrawImgPanel()
        {
            InitializeComponent();
            ThreadPool.SetMaxThreads(1, 1);
            Control.CheckForIllegalCrossThreadCalls = false;
            new Thread(DownImg).Start();
        }

        /// <summary>
        /// 网页中解析到的所有图片信息
        /// </summary>
        ImgInfo img = new ImgInfo();
        /// <summary>
        /// 待下载的图片列表
        /// </summary>
        List<ImgList> imgList = new List<ImgList>();        

        #region 获取图片链接，并显示标题和预览
        private void GetImages_Click(object sender, EventArgs e)
        {
            GetImgInfo getImgInfo = new GetImgInfo();
            string url = txt_Url.Text;
            img = getImgInfo.GetImgUrls(url);
            //判断抓取中是否遇到错误！
            if (!string.IsNullOrEmpty(img.Error))
            {
                MessageBox.Show(img.Error);
                return;
            }
            //获取标题并居中
            Title.Text = img.ACGWork + "  " + Regex.Replace(img.Title+" - "+img.Author, @"&", @"&&") + "（" + img.Urls.Count + "P）";
            Title.Location = new Point((this.Width - 20 - Title.Width) / 2, Title.Location.Y);
            //重置打开文件夹按钮状态
            btn_OpenFloder.Enabled = false;
            //清空已有图片
            flp_ImgPriview.Controls.Clear();

            //加载描述
            TextBox description = new TextBox();
            description.Multiline = true;
            description.Margin = new System.Windows.Forms.Padding(3);
            description.Name = "description";
            description.Size = new Size(300, 300);
            //description.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            description.Text = img.Description;
            flp_ImgPriview.Controls.Add(description);

            //加载预览图片
            for (int i = 0; i < img.Urls.Count; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Margin = new Padding(3);
                pic.Name = "pic" + i.ToString();
                pic.Size = new System.Drawing.Size(300, 300);
                //图片边框重绘
                pic.Paint += new PaintEventHandler(pic_Paint);

                ////设置填充方式  flowlayout表格中不能设置为Fill
                //pic.Dock = System.Windows.Forms.DockStyle.Fill;

                //设置图片显示方式及链接位置
                pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                pic.ImageLocation = img.Urls[i] + "/w650";
                //更换等待动画
                pic.InitialImage = global::MyTools.Properties.Resources.loading;
                //鼠标单击事件
                pic.MouseDown += new MouseEventHandler(pic_MouseDown);
                //将图片添加到flowlayoutpanel
                flp_ImgPriview.Controls.Add(pic);

                ////将图片添加到tablelayoutpanel
                //ImagesPriview.Controls.Add(pic, i % 3, i / 3);
                ////鼠标点击事件
                //pic.MouseDown += new MouseEventHandler(pic_MouseDown);

            }
            //检查目录和文件名，将不能使用字符替换为“_”
            //正则表达式"[\\u005C/:\\u002A\\u003F\"<>\'\\u007C’‘“”：？]"还包含中文的字符（实际上中文字符是可以使用的）
            string fileNameCheck = "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]";
            string titleCheck = Regex.Replace(img.ACGWork, fileNameCheck, "_");
            string pathCheck = "";
            string path = "";
            switch (img.Kind)
            {
                case ImgKind.Cosplay:
                    pathCheck = Regex.Replace(img.Title + " - " + img.Author, fileNameCheck, "_");
                    path = @"E:\图片\Cosplay\" + titleCheck + @"\" + pathCheck;
                    break;
                case ImgKind.Daily:
                    pathCheck = Regex.Replace(img.Author, fileNameCheck, "_");
                    path = @"E:\图片\Cosplay\日常\"+ pathCheck;
                    break;
                case ImgKind.Illustraion:
                    pathCheck = Regex.Replace(img.Author, fileNameCheck, "_");
                    path = @"E:\图片\画师\" + pathCheck;
                    break;
            }
            txt_FloderPath.Text = path;
            flp_ImgPriview.Select();
        }
        #endregion

        #region 图片边框重绘事件
        private void pic_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ((PictureBox)sender).ClientRectangle,
     Color.LightGray, 1, ButtonBorderStyle.Solid, //左边
     Color.LightGray, 1, ButtonBorderStyle.Solid, //上边
     Color.Gray, 1, ButtonBorderStyle.Solid, //右边
     Color.Gray, 1, ButtonBorderStyle.Solid);
        }
        #endregion

        #region 鼠标左键预览大图，右键预览原图
        private void pic_MouseDown(object sender, MouseEventArgs e)
        {
            ////获取网上图片尺寸
            //string str = ((PictureBox)sender).ImageLocation;
            //using (var webClient = new WebClient())   //使用完后释放对象
            //{
            //    var imageDate = webClient.DownloadData(str.Substring(0, str.Length - 5));
            //    using (var stream = new MemoryStream(imageDate)) 
            //    {
            //        var image = Image.FromStream(stream);
            //        BigImage priview = new BigImage();
            //        priview.pictureBox1.ImageLocation = str.Substring(0, str.Length - 5);
            //        priview.pictureBox1.Size = image.Size;
            //        priview.ShowDialog();
            //    }
            //}
            
            PictureBox pic = new PictureBox();
            Form bigImage = new Form();
            string url = ((PictureBox)sender).ImageLocation;
            pic.ImageLocation = url.Substring(0, url.Length - 5);
            pic.InitialImage = global::MyTools.Properties.Resources.loading;
            if (e.Button == MouseButtons.Left)
            {
                pic.Dock = System.Windows.Forms.DockStyle.Fill;
                pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                bigImage.AutoScroll = false;
            }
            //新建窗口，打开原图
            else if (e.Button == MouseButtons.Right)
            {
                pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                bigImage.AutoScroll = true;
            }
            bigImage.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            bigImage.KeyPress += new KeyPressEventHandler(bigImage_keyPress);
            bigImage.Controls.Add(pic);
            bigImage.ShowDialog();
        }

        private void bigImage_keyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                ((Form)sender).Close();
            }
        }
        #endregion
        
        #region 记录图片所在tablelayoutpanel单元格位置
        //TableLayoutPanelCellPosition pos = new TableLayoutPanelCellPosition();

        ////鼠标点击事件，左键放大或缩小图片(tablelayoutpanel)，右键打开新窗口预览原图
        //private void pic_MouseDown(object sender, MouseEventArgs e)
        //{
        //    PictureBox pVideo = (PictureBox)sender;
        //    //点左键放大或缩小图片，右键打开新窗口查看原图
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        if (ImagesPriview.GetColumnSpan(pVideo) == 1)
        //        {
        //            //隐藏其它控件
        //            foreach (Control ctr in ImagesPriview.Controls)
        //            {
        //                if (ctr.Name != pVideo.Name)
        //                    ctr.Visible = false;
        //            }
        //            pos = ImagesPriview.GetCellPosition(pVideo);
        //            ImagesPriview.SetCellPosition(pVideo, new TableLayoutPanelCellPosition(0, 0));
        //            ImagesPriview.SetRowSpan(pVideo, ImagesPriview.RowCount);
        //            ImagesPriview.SetColumnSpan(pVideo, ImagesPriview.ColumnCount);
        //        }
        //        else
        //        {
        //            //显示其它控件
        //            foreach (Control ctr in ImagesPriview.Controls)
        //            {
        //                if (ctr.Name != pVideo.Name)
        //                    ctr.Visible = true;
        //            }
        //            ImagesPriview.SetCellPosition(pVideo, pos);
        //            ImagesPriview.SetRowSpan(pVideo, 1);
        //            ImagesPriview.SetColumnSpan(pVideo, 1);
        //        }
        //    }
        //    else if (e.Button == MouseButtons.Right)
        //    {
        //        //创建新窗口并设置窗口大小
        //        Form BigImage = new Form();
        //        BigImage.Name = "BigImage";
        //        BigImage.Text = "预览原图";
        //        BigImage.Size = new System.Drawing.Size(1280, 720);
        //        //BigImage.AutoScroll = true;
        //        //设置按键事件，按ESC关闭预览窗口
        //        BigImage.KeyPress += new KeyPressEventHandler(BigImage_KeyPress);
        //        //获取图片链接并添加至新窗口中
        //        PictureBox pic = new PictureBox();
        //        pic.ImageLocation = pVideo.ImageLocation.Substring(0, pVideo.ImageLocation.Length - 5);
        //        pic.Margin = new Padding(3);
        //        width = pic.Width;
        //        //设置图片显示方式
        //        pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        //        //更换等待动画
        //        pic.InitialImage = global::CrawImg.Properties.Resources.loading;
        //        pic.MouseWheel += new MouseEventHandler(pic_MouseWheel);
        //        BigImage.Controls.Add(pic);
        //        BigImage.Show();
        //        PictureBox x = (PictureBox)BigImage.Controls[0];
        //        x.Select();
        //    }
        //}
        #endregion
        
        #region 选择保存位置，返回路径
        private void SaveFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择保存路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string titleCheck = Regex.Replace(img.ACGWork, "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]", "_");
                string pathCheck = Regex.Replace(img.Author, "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]", "_");
                txt_FloderPath.Text = dialog.SelectedPath + @"\" + titleCheck + @"\" + pathCheck;
            }
            flp_ImgPriview.Select();
        }
        #endregion

        #region 保存文件，路径不存在则创建路径后再保存，全部保存后激活打开文件夹按钮
        private void SaveImages_Click(object sender, EventArgs e)
        {
            //移除空白字符
            string path = txt_FloderPath.Text.Trim();

            ////定时关闭弹窗
            //Timer timer = new Timer();
            //timer.Interval = 3000;
            //timer.Tick += new EventHandler(Timer_Tick);
            //timer.Start();
            //MessageBox.Show("确认后开始保存图片。全部保存后可打开保存文件夹！\n该窗口3秒后自动关闭！", "确认信息");

            ////开始下载，并计算耗时
            //System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //string result = DownloadImg.GetImages(path, img);
            //stopwatch.Stop();

            ////下载完成后显示耗时
            //MessageBox.Show(result + " 任务耗时：" + stopwatch.ElapsedTicks / 60000000 + "分" + stopwatch.ElapsedTicks / 1000000 % 60 + "秒");

            //保存描述
            //string titleCheck = Regex.Replace(img.Author, "[\\u005C/:\\u002A\\u003F\"<>\'\\u007C]", "_");

            switch (img.Kind)
            {
                case ImgKind.Cosplay:
                    DownCosplay(path);
                    break;
                case ImgKind.Daily:
                    DownCosplay(path);
                    break;
                case ImgKind.Illustraion:
                    DownIllustration(path);
                    break;
            }

            //激活打开文件夹按钮
            btn_OpenFloder.Enabled = true;
            flp_ImgPriview.Select();
            
        }

        /// <summary>
        /// 下载Cosplay和日常图片
        /// </summary>
        /// <param name="path">下载路径</param>
        private void DownCosplay(string path)
        {
            //保存描述
            //string title = Regex.Match(path, ".*\\\\(?<name>.+)").Groups["name"].Value.ToString();
            string title = Path.GetFileNameWithoutExtension(path);
            string infoPath = Path.Combine(path, title);
            string value = img.Description;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream fs = new FileStream(infoPath + ".txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
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

            foreach (var x in img.Urls)
            {
                ImgList temp = new ImgList();
                temp.url = x;
                temp.path = path;
                imgList.Add(temp);
            }
        }

        /// <summary>
        /// 下载插画
        /// </summary>
        /// <param name="path">下载路径</param>
        private void DownIllustration(string path)
        {
            foreach (var x in img.Urls)
            {
                ImgList temp = new ImgList();
                temp.url = x;
                temp.path = path;
                imgList.Add(temp);
            }
        }
        #endregion

        #region 定时关闭弹窗事件
        ////3秒后关闭弹窗
        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    IntPtr ptr = FindWindow(null, "确认信息");
        //    if (ptr != IntPtr.Zero)
        //    {
        //        PostMessage(ptr, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        //    }
        //    ((Timer)sender).Stop();
        //}
        #endregion

        #region 打开保存文件夹事件
        private void OpenFloder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", txt_FloderPath.Text);
            flp_ImgPriview.Select();
        }
        #endregion

        #region 网址栏回车键与获取按钮绑定
        private void Url_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                GetImages_Click(sender,e);
            }
        }
        #endregion

        #region 路径栏回车键与保存按钮绑定
        private void FloderPath_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SaveImages_Click(sender, e);
            }
        }
        #endregion

        #region 鼠标滑过时flp自动聚焦
        private void PicPriview_MouseMove(object sender, MouseEventArgs e)
        {
            ((FlowLayoutPanel)sender).Select();
        }
        #endregion

        private void Mains_FormClosing(object sender, FormClosingEventArgs e)
        {
            downStart = false;
        }


        int status = 0;
        bool isFirst = true;

        /// <summary>
        /// 下载图片线程
        /// </summary>
        void DownImg()
        {

            while (downStart)
            {
                ImgList img = GetUrl();
                if (img == null)
                {
                    if (!isFirst && status == 2)
                    {
                        MessageBox.Show("本次下载任务已完成！", "提示");
                        status = 0;
                        isFirst = true;
                    }
                    Thread.Sleep(3000);
                    continue;
                }
                if (!WebApi.DownloadFile(img.url, img.path))
                {
                    string fileName = Path.GetFileName(img.url);
                    string dirName = Path.GetFileName(img.path);
                    MessageBox.Show(dirName + " - " + fileName + " 下载出错！", "提示");
                }         
            }         
        }

        /// <summary>
        /// 从待下载列表中获取一个下载链接
        /// </summary>
        /// <returns></returns>
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

        private void btn_codeChange_Click(object sender, EventArgs e)
        {
            AddressDown ad = new AddressDown();
            ad.Show();
        }
    }

    /// <summary>
    /// 图片下载信息类
    /// </summary>
    public class ImgList
    {
        /// <summary>
        /// 链接地址
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string path { get; set; }
    }
}
