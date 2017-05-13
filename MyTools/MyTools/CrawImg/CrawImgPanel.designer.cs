namespace MyTools.CrawImg
{
    partial class CrawImgPanel
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.UrlInfo = new System.Windows.Forms.Label();
            this.txt_Url = new System.Windows.Forms.TextBox();
            this.btn_GetImages = new System.Windows.Forms.Button();
            this.txt_FloderPath = new System.Windows.Forms.TextBox();
            this.btn_SaveFolder = new System.Windows.Forms.Button();
            this.FloderPathInfo = new System.Windows.Forms.Label();
            this.btn_SaveImages = new System.Windows.Forms.Button();
            this.Pictures = new System.Windows.Forms.GroupBox();
            this.flp_ImgPriview = new System.Windows.Forms.FlowLayoutPanel();
            this.Title = new System.Windows.Forms.Label();
            this.btn_OpenFloder = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_codeChange = new System.Windows.Forms.Button();
            this.Pictures.SuspendLayout();
            this.SuspendLayout();
            // 
            // UrlInfo
            // 
            this.UrlInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UrlInfo.AutoSize = true;
            this.UrlInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.UrlInfo.Location = new System.Drawing.Point(291, 14);
            this.UrlInfo.Name = "UrlInfo";
            this.UrlInfo.Size = new System.Drawing.Size(49, 14);
            this.UrlInfo.TabIndex = 0;
            this.UrlInfo.Text = "网址：";
            // 
            // txt_Url
            // 
            this.txt_Url.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_Url.Location = new System.Drawing.Point(342, 11);
            this.txt_Url.Name = "txt_Url";
            this.txt_Url.Size = new System.Drawing.Size(299, 21);
            this.txt_Url.TabIndex = 1;
            this.txt_Url.Text = "http://bcy.net/coser/detail/2929/569930";
            this.txt_Url.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Url_KeyPress);
            // 
            // btn_GetImages
            // 
            this.btn_GetImages.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_GetImages.Location = new System.Drawing.Point(647, 9);
            this.btn_GetImages.Name = "btn_GetImages";
            this.btn_GetImages.Size = new System.Drawing.Size(75, 23);
            this.btn_GetImages.TabIndex = 2;
            this.btn_GetImages.Text = "获取图片";
            this.btn_GetImages.UseVisualStyleBackColor = true;
            this.btn_GetImages.Click += new System.EventHandler(this.GetImages_Click);
            // 
            // txt_FloderPath
            // 
            this.txt_FloderPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txt_FloderPath.Location = new System.Drawing.Point(335, 590);
            this.txt_FloderPath.Name = "txt_FloderPath";
            this.txt_FloderPath.Size = new System.Drawing.Size(337, 21);
            this.txt_FloderPath.TabIndex = 5;
            this.txt_FloderPath.Text = "E:\\图片\\Cosplay";
            this.txt_FloderPath.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FloderPath_KeyPress);
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(678, 590);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFolder.TabIndex = 6;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            this.btn_SaveFolder.Click += new System.EventHandler(this.SaveFolder_Click);
            // 
            // FloderPathInfo
            // 
            this.FloderPathInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FloderPathInfo.AutoSize = true;
            this.FloderPathInfo.BackColor = System.Drawing.Color.Transparent;
            this.FloderPathInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.FloderPathInfo.Location = new System.Drawing.Point(257, 593);
            this.FloderPathInfo.Name = "FloderPathInfo";
            this.FloderPathInfo.Size = new System.Drawing.Size(77, 14);
            this.FloderPathInfo.TabIndex = 7;
            this.FloderPathInfo.Text = "保存位置：";
            // 
            // btn_SaveImages
            // 
            this.btn_SaveImages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveImages.Location = new System.Drawing.Point(395, 617);
            this.btn_SaveImages.Name = "btn_SaveImages";
            this.btn_SaveImages.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveImages.TabIndex = 8;
            this.btn_SaveImages.Text = "保存图片";
            this.btn_SaveImages.UseVisualStyleBackColor = true;
            this.btn_SaveImages.Click += new System.EventHandler(this.SaveImages_Click);
            // 
            // Pictures
            // 
            this.Pictures.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Pictures.Controls.Add(this.flp_ImgPriview);
            this.Pictures.Location = new System.Drawing.Point(30, 85);
            this.Pictures.Name = "Pictures";
            this.Pictures.Size = new System.Drawing.Size(950, 497);
            this.Pictures.TabIndex = 12;
            this.Pictures.TabStop = false;
            this.Pictures.Text = "图片预览";
            // 
            // flp_ImgPriview
            // 
            this.flp_ImgPriview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flp_ImgPriview.AutoScroll = true;
            this.flp_ImgPriview.BackColor = System.Drawing.SystemColors.Control;
            this.flp_ImgPriview.Location = new System.Drawing.Point(5, 15);
            this.flp_ImgPriview.Name = "flp_ImgPriview";
            this.flp_ImgPriview.Size = new System.Drawing.Size(940, 477);
            this.flp_ImgPriview.TabIndex = 0;
            this.flp_ImgPriview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicPriview_MouseMove);
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(383, 37);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(224, 22);
            this.Title.TabIndex = 13;
            this.Title.Text = "原作名 标题 - 作者（图片数）";
            // 
            // btn_OpenFloder
            // 
            this.btn_OpenFloder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_OpenFloder.Enabled = false;
            this.btn_OpenFloder.Location = new System.Drawing.Point(530, 617);
            this.btn_OpenFloder.Name = "btn_OpenFloder";
            this.btn_OpenFloder.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFloder.TabIndex = 15;
            this.btn_OpenFloder.Text = "打开文件夹";
            this.btn_OpenFloder.UseVisualStyleBackColor = true;
            this.btn_OpenFloder.Click += new System.EventHandler(this.OpenFloder_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(263, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "鼠标左键预览大图，右键预览原图，ESC退出预览";
            // 
            // btn_codeChange
            // 
            this.btn_codeChange.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_codeChange.Location = new System.Drawing.Point(731, 8);
            this.btn_codeChange.Name = "btn_codeChange";
            this.btn_codeChange.Size = new System.Drawing.Size(75, 23);
            this.btn_codeChange.TabIndex = 17;
            this.btn_codeChange.Text = "代码抓取";
            this.btn_codeChange.UseVisualStyleBackColor = true;
            this.btn_codeChange.Click += new System.EventHandler(this.btn_codeChange_Click);
            // 
            // CrawImgPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 661);
            this.Controls.Add(this.btn_codeChange);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_OpenFloder);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.btn_SaveImages);
            this.Controls.Add(this.FloderPathInfo);
            this.Controls.Add(this.btn_SaveFolder);
            this.Controls.Add(this.txt_FloderPath);
            this.Controls.Add(this.btn_GetImages);
            this.Controls.Add(this.txt_Url);
            this.Controls.Add(this.UrlInfo);
            this.Controls.Add(this.Pictures);
            this.Name = "CrawImgPanel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "抓取半次元图片";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mains_FormClosing);
            this.Pictures.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label UrlInfo;
        private System.Windows.Forms.TextBox txt_Url;
        private System.Windows.Forms.Button btn_GetImages;
        private System.Windows.Forms.TextBox txt_FloderPath;
        private System.Windows.Forms.Button btn_SaveFolder;
        private System.Windows.Forms.Label FloderPathInfo;
        private System.Windows.Forms.Button btn_SaveImages;
        private System.Windows.Forms.GroupBox Pictures;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button btn_OpenFloder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flp_ImgPriview;
        private System.Windows.Forms.Button btn_codeChange;
    }
}

