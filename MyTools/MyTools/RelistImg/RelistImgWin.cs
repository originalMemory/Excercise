using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace MyTools.RelistImg
{
    public partial class RelistImgWin : Form
    {
        string SourcePath = "";
        string TargetPath = "";

        public RelistImgWin()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //获取原图路径
        private void btn_GetSourcePath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择原图目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_SourcePath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //获取目标路径
        private void btn_GetTargetPath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择目标目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_TargetPath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //开始重排序图片
        private void btn_Relist_Click(object sender, EventArgs e)
        {
            new Thread(run).Start();
            pic_Working.Visible = true;
        }

        void run()
        {
            TargetPath = txt_TargetPath.Text;
            SourcePath = txt_SourcePath.Text;
            string str = txt_ExcludeFloder.Text;
            int limitNum = Convert.ToInt32(txt_LimitNum.Text);
            var exclude = str.Split(';', '；').Where(x => !string.IsNullOrEmpty(x) && x != "undefined").ToList();
            Regex reg=new Regex(txt_reg.Text);

            /* 获取原图路径下的图片
             * 判断是搜索子目录
             * 对得到的图片路径进行正则匹配,将匹配出的序号和路径存入ImgInfo类并添加到列表中去
             */
            var imgPathList = new List<ImgInfo>();
            var tempList = new List<string>();
            if (cb_IsDeep.Checked)
                tempList = Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories).ToList();
            else
                tempList = Directory.GetFiles(SourcePath, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach (var x in tempList)
            {
                long num = Convert.ToInt64(reg.Match(x).Groups["num"].Value);
                ImgInfo img = new ImgInfo
                {
                    Path = x,
                    Num = num
                };
                imgPathList.Add(img);
            }
            int count = imgPathList.Count;      //图片总数

            imgPathList = imgPathList.OrderBy(x => x.Num).ToList();     //按序号大小排序
            int floderCount=1;      //文件夹起始序号
            int imgCount = 0;       //图片起始序号
            for (int i = 0; i < count; i++)
            {
                if (imgCount == limitNum)
                {
                    floderCount++;
                    imgCount = 0;
                }
                imgCount++;
                string path = imgPathList[i].Path;      //原图路径
                FileInfo file = new FileInfo(path);
                string name = file.Name;
                string folderPath = TargetPath +"\\"+ floderCount;
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string newImgPath = folderPath + "\\" + name;

                int percent = i * 100 / count;
                proBar.Value = percent + 1;

                if (File.Exists(newImgPath))
                {
                    continue;
                }
                else
                {
                    bool IsMove=true;
                    foreach (var x in exclude)
                    {
                        if(path.Contains(x))
                        {
                            IsMove = false;
                            break;
                        }
                    }
                    if (IsMove)
                        file.MoveTo(newImgPath);
                    else
                        file.CopyTo(newImgPath, false);
                    richTextBox1.Text = richTextBox1.Text.Insert(0, floderCount+@"/"+ name + Environment.NewLine);
                }
                
            }
            pic_Working.Visible = false;
        }

        class ImgInfo
        {
            public string Path { get; set; }
            public long Num { get; set; }
        }

        private void RelistImgWin_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

    }
}
