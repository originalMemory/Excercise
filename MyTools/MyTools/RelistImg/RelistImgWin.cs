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
using MyTools.Tools;
using MyTools.Model;
using MyTools.Helper;

namespace MyTools.RelistImg
{
    public partial class RelistImgWin : Form
    {
        string SourcePath = "";
        string TargetPath = "";
        /// <summary>
        /// ini文件中区块名
        /// </summary>
        string IniSection = "RelistImg";

        public RelistImgWin()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            txtMinImgWidth.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);
            txtLimitNum.KeyPress += new KeyPressEventHandler(ControlEvent.NumLimit);

            //加载上一次目录信息
            txtSourcePath.Text = OperateIni.ReadIniData(IniSection, "sourcePath");
            txtTargetPath.Text = OperateIni.ReadIniData(IniSection, "targetPath");
            txtExcludeFloder.Text = OperateIni.ReadIniData(IniSection, "excludeFloder");
            //加载图片条件
            string strType = OperateIni.ReadIniData(IniSection, "imgType");
            if (!string.IsNullOrEmpty(strType))
            {
                ImgType type = (ImgType)Convert.ToInt32(strType);
                switch (type)
                {
                    case ImgType.Horizon:
                        cmbImgType.Text = "横图";
                        break;
                    case ImgType.Vertical:
                        cmbImgType.Text = "竖图";
                        break;
                    default:
                        break;
                }
            }

            txtMinImgWidth.Text = OperateIni.ReadIniData(IniSection, "minImgWidth");
            string strStartAt = OperateIni.ReadIniData(IniSection, "startAt");
            if (!string.IsNullOrEmpty(strStartAt))
            {
                DateTime startAt = new DateTime();
                DateTime.TryParse(strStartAt, out startAt);
                dtpStart.Value = startAt;
            }
            string strCheckTime = OperateIni.ReadIniData(IniSection, "checkTime");
            if (!string.IsNullOrEmpty(strCheckTime))
                checkTime.Checked = Convert.ToBoolean(strCheckTime);
            string strcheckCopy = OperateIni.ReadIniData(IniSection, "checkCopy");
            if (!string.IsNullOrEmpty(strcheckCopy))
                checkCopy.Checked = Convert.ToBoolean(strcheckCopy);
            string strcheckDeep = OperateIni.ReadIniData(IniSection, "checkDeep");
            if (!string.IsNullOrEmpty(strcheckDeep))
                checkDeep.Checked = Convert.ToBoolean(strcheckDeep);
            txtLimitNum.Text = OperateIni.ReadIniData(IniSection, "limitNum");
            string strSortReg = OperateIni.ReadIniData(IniSection, "txtSortReg");
            if (!string.IsNullOrEmpty(strSortReg))
                txtSortReg.Text = strSortReg;
        }

        //获取原图路径
        private void btn_GetSourcePath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择原图目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtSourcePath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //获取目标路径
        private void btn_GetTargetPath_Click(object sender, EventArgs e)
        {
            dialog.Description = "请选择目标目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtTargetPath.Text = dialog.SelectedPath;
                dialog.Reset();
            }
        }

        //开始重排序图片
        private void btn_Relist_Click(object sender, EventArgs e)
        {
            new Thread(run).Start();
            picRunning.Visible = true;

            //保存本次设置
            OperateIni.WriteIniData(IniSection, "sourcePath", txtSourcePath.Text);
            OperateIni.WriteIniData(IniSection, "targetPath", txtTargetPath.Text);
            OperateIni.WriteIniData(IniSection, "excludeFloder", txtExcludeFloder.Text);
            ImgType type = new ImgType();
            switch (cmbImgType.Text)
            {
                case "横图":
                    type = ImgType.Horizon;
                    break;
                case "竖图":
                    type = ImgType.Vertical;
                    break;
                default:
                    break;
            }
            OperateIni.WriteIniData(IniSection, "imgType", (int)type);
            OperateIni.WriteIniData(IniSection, "minImgWidth", txtMinImgWidth.Text);
            OperateIni.WriteIniData(IniSection, "startAt", dtpStart.Value);
            OperateIni.WriteIniData(IniSection, "checkTime", checkTime.Checked);
            OperateIni.WriteIniData(IniSection, "checkCopy", checkCopy.Checked);
            OperateIni.WriteIniData(IniSection, "checkDeep", checkDeep.Checked);
            OperateIni.WriteIniData(IniSection, "limitNum", txtLimitNum.Text);
            OperateIni.WriteIniData(IniSection, "txtSortReg", txtSortReg.Text);
        }

        void run()
        {
            TargetPath = txtTargetPath.Text;
            SourcePath = txtSourcePath.Text;
            string str = txtExcludeFloder.Text;
            int limitNum = Convert.ToInt32(txtLimitNum.Text);
            var exclude = str.Split(';', '；').Where(x => !string.IsNullOrEmpty(x) && x != "undefined").ToList();
            Regex reg = new Regex(txtSortReg.Text);

            /* 获取原图路径下的图片
             * 判断是搜索子目录
             * 对得到的图片路径进行正则匹配,将匹配出的序号和路径存入ImgInfo类并添加到列表中去
             */
            var imgPathList = new List<ImgInfo>();
            var tempList = new List<string>();
            if (checkDeep.Checked)
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
            int floderCount = 1;      //文件夹起始序号
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
                string folderPath = TargetPath + "\\" + floderCount;
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
                    bool IsMove = true;
                    foreach (var x in exclude)
                    {
                        if (path.Contains(x))
                        {
                            IsMove = false;
                            break;
                        }
                    }
                    if (IsMove)
                        file.MoveTo(newImgPath);
                    else
                        file.CopyTo(newImgPath, false);
                    rtxtImgList.Text = rtxtImgList.Text.Insert(0, floderCount + @"/" + name + Environment.NewLine);
                }

            }
            picRunning.Visible = false;
        }

        class ImgInfo
        {
            public string Path { get; set; }
            public long Num { get; set; }
        }

    }
}
