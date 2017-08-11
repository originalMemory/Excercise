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
using System.IO;
using System.Threading;

namespace MyTools.SelectImg
{
    public partial class SelectImgPanel : Form
    {
        string SourcePath = "";
        string TargetPath = "";
        FolderBrowserDialog dialog = new FolderBrowserDialog();
        public SelectImgPanel()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            richTextBox1.Text = "";
        }

        //获取源目录
        private void btn_SourcePath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择源目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_SourcePath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //获取目标目录
        private void btn_TargetPath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择目标目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_TargetPath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //开始复制图片
        private void btn_Start_Click(object sender, EventArgs e)
        {
            //判断目标文件夹是否存在，不存在则创建文件
            if (!Directory.Exists(txt_TargetPath.Text))
            {
                Directory.CreateDirectory(txt_TargetPath.Text);
            }
            new Thread(run).Start();
            pictureBox1.Visible = true;
        }

        void run()
        {
            TargetPath = txt_TargetPath.Text;
            SourcePath = txt_SourcePath.Text;
            string str = txt_ExcludeFloder.Text;
            var exclude = str.Split(';', '；').Where(x => !string.IsNullOrEmpty(x)).ToList();
            bool horizon = false;
            switch (cmb_ImgType.Text)
            {
                case "横图": horizon = true; break;
                case "树图": horizon = false; break;
            }
            int miniWidth = Convert.ToInt32(txt_MiniWidth.Text);
            SelectCopy(horizon, miniWidth, exclude);
            pictureBox1.Visible = false;
        }

        /// <summary>
        /// 条件复制图片
        /// </summary>
        /// <param name="horizon">图片是否横向</param>
        /// <param name="miniWidth">最小宽度</param>
        /// <param name="exclude">排除文件夹名</param>
        void SelectCopy(bool horizon, int miniWidth,List<string> exclude)
        {
            string temp = SourcePath + @"\(?<name>.+)";
            temp = temp.Replace(@"\", @"\\");
            Regex regFileNovel = new Regex(temp);                        //匹配文件名及其上级目录
            Regex regFileName = new Regex(@".+\\(?<info>.+)");           //匹配文件名
            var imgPathList = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpeg") || s.EndsWith(".jpg") || s.EndsWith(".bmp") || s.EndsWith(".png") || s.EndsWith(".gif")).ToList();
            int count = imgPathList.Count;
            for (int i = 0; i < count;i++ )
            {
                string imgPath = imgPathList[i];
                //判断是否是排除文件夹内的图片
                bool ex = false;
                foreach (var x in exclude)
                {
                    if (imgPath.Contains(x))
                    {
                        ex = true;
                        break;
                    }
                }
                if (ex) continue;
                using (Image img = Image.FromFile(imgPath))
                {
                    //根据图片尺寸筛选
                    if (img.Width < miniWidth) continue;
                    if (horizon)
                    {
                        if (img.Width < img.Height) continue;
                    }
                    else
                    {
                        if (img.Width > img.Height) continue;
                    }
                    if (check_Time.Checked)
                    {
                        //判断开始时间
                        DateTime startTime = dt_Start.Value;
                        FileInfo file = new FileInfo(imgPath);
                        if (file.CreationTime <= startTime && file.LastWriteTime <= startTime)
                        {
                            continue;
                        }
                    }
                    string fileName = regFileName.Match(imgPath).Groups["info"].Value;
                    string fileNovel = regFileNovel.Match(imgPath).Groups["name"].Value;
                    CopyImg(imgPath, TargetPath, fileName, 0);
                    int percent = i * 100 / count;
                    progressBar1.Value = percent + 1;
                    richTextBox1.Text = richTextBox1.Text.Insert(0, fileNovel + Environment.NewLine);
                }
            }
            MessageBox.Show("图片复制完毕!", "提示");
        }

        /// <summary>
        /// 非覆盖式复制图片
        /// </summary>
        /// <param name="imgPath">图片源路径</param>
        /// <param name="TargetPath">目标文件夹</param>
        /// <param name="fileName">文件名</param>
        /// <param name="no">编号</param>
        void CopyImg(string imgPath,string TargetPath,string fileName,int no)
        {
            FileInfo file = new FileInfo(imgPath);
            string newName;
            //K:\Cosplay\Fate_Stay Night\saber Alter - 南宫
            if (no == 0)
            {
                newName = TargetPath + "\\" + fileName;
            }
            else
            {
                string append = "-" + no;
                newName = TargetPath + "\\" + fileName.Insert(fileName.LastIndexOf('.'), append);
            }
            //判断目标文件名是否已存在
            if (File.Exists(newName))
            {
                //比对二者大小，如果不一致则改名存储
                FileInfo comp = new FileInfo(newName);
                if (file.Length != comp.Length)
                {
                    no++;
                    CopyImg(imgPath, TargetPath, fileName, no);
                }
                else
                {
                    return;
                }
            }
            else
            {
                file.CopyTo(newName, false);
            }
        }
    }
}
