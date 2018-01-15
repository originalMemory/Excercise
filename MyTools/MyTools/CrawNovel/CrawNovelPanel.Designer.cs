namespace MyTools.CrawNovel
{
    partial class CrawNovelPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_Url = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_GetNovel = new System.Windows.Forms.Button();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.btn_OpenFloder = new System.Windows.Forms.Button();
            this.btn_SaveNovel = new System.Windows.Forms.Button();
            this.FloderPathInfo = new System.Windows.Forms.Label();
            this.btn_SaveFolder = new System.Windows.Forms.Button();
            this.txt_FloderPath = new System.Windows.Forms.TextBox();
            this.Title = new System.Windows.Forms.Label();
            this.checkLine = new System.Windows.Forms.CheckBox();
            this.txt_main = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // txt_Url
            // 
            this.txt_Url.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_Url.Location = new System.Drawing.Point(311, 24);
            this.txt_Url.Name = "txt_Url";
            this.txt_Url.Size = new System.Drawing.Size(364, 21);
            this.txt_Url.TabIndex = 13;
            this.txt_Url.Text = "http://cl.soze.pw/htm_data/20/1604/1920038.html";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "网址：";
            // 
            // btn_GetNovel
            // 
            this.btn_GetNovel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_GetNovel.Location = new System.Drawing.Point(681, 24);
            this.btn_GetNovel.Name = "btn_GetNovel";
            this.btn_GetNovel.Size = new System.Drawing.Size(75, 23);
            this.btn_GetNovel.TabIndex = 11;
            this.btn_GetNovel.Text = "获取小说";
            this.btn_GetNovel.UseVisualStyleBackColor = true;
            this.btn_GetNovel.Click += new System.EventHandler(this.btn_GetNovel_Click);
            // 
            // bar
            // 
            this.bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bar.Location = new System.Drawing.Point(12, 103);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(1029, 23);
            this.bar.TabIndex = 15;
            // 
            // btn_OpenFloder
            // 
            this.btn_OpenFloder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_OpenFloder.Enabled = false;
            this.btn_OpenFloder.Location = new System.Drawing.Point(554, 735);
            this.btn_OpenFloder.Name = "btn_OpenFloder";
            this.btn_OpenFloder.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFloder.TabIndex = 20;
            this.btn_OpenFloder.Text = "打开文件夹";
            this.btn_OpenFloder.UseVisualStyleBackColor = true;
            this.btn_OpenFloder.Click += new System.EventHandler(this.btn_OpenFloder_Click);
            // 
            // btn_SaveNovel
            // 
            this.btn_SaveNovel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveNovel.Enabled = false;
            this.btn_SaveNovel.Location = new System.Drawing.Point(419, 735);
            this.btn_SaveNovel.Name = "btn_SaveNovel";
            this.btn_SaveNovel.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveNovel.TabIndex = 19;
            this.btn_SaveNovel.Text = "保存小说";
            this.btn_SaveNovel.UseVisualStyleBackColor = true;
            this.btn_SaveNovel.Click += new System.EventHandler(this.btn_SaveNovel_Click);
            // 
            // FloderPathInfo
            // 
            this.FloderPathInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FloderPathInfo.AutoSize = true;
            this.FloderPathInfo.BackColor = System.Drawing.Color.Transparent;
            this.FloderPathInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.FloderPathInfo.Location = new System.Drawing.Point(281, 711);
            this.FloderPathInfo.Name = "FloderPathInfo";
            this.FloderPathInfo.Size = new System.Drawing.Size(77, 14);
            this.FloderPathInfo.TabIndex = 18;
            this.FloderPathInfo.Text = "保存位置：";
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(702, 708);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFolder.TabIndex = 17;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            // 
            // txt_FloderPath
            // 
            this.txt_FloderPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txt_FloderPath.Location = new System.Drawing.Point(359, 708);
            this.txt_FloderPath.Name = "txt_FloderPath";
            this.txt_FloderPath.Size = new System.Drawing.Size(337, 21);
            this.txt_FloderPath.TabIndex = 16;
            this.txt_FloderPath.Text = "E:\\图片\\Cosplay";
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(426, 64);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(171, 22);
            this.Title.TabIndex = 21;
            this.Title.Text = "标题 - 作者（章节数）";
            // 
            // checkLine
            // 
            this.checkLine.AutoSize = true;
            this.checkLine.Location = new System.Drawing.Point(762, 29);
            this.checkLine.Name = "checkLine";
            this.checkLine.Size = new System.Drawing.Size(48, 16);
            this.checkLine.TabIndex = 22;
            this.checkLine.Text = "分行";
            this.checkLine.UseVisualStyleBackColor = true;
            // 
            // txt_main
            // 
            this.txt_main.Location = new System.Drawing.Point(13, 133);
            this.txt_main.Name = "txt_main";
            this.txt_main.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txt_main.Size = new System.Drawing.Size(1028, 562);
            this.txt_main.TabIndex = 23;
            this.txt_main.Text = "";
            // 
            // CrawNovelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 770);
            this.Controls.Add(this.txt_main);
            this.Controls.Add(this.checkLine);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.btn_OpenFloder);
            this.Controls.Add(this.btn_SaveNovel);
            this.Controls.Add(this.FloderPathInfo);
            this.Controls.Add(this.btn_SaveFolder);
            this.Controls.Add(this.txt_FloderPath);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.txt_Url);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_GetNovel);
            this.Name = "CrawNovelPanel";
            this.Text = "CrawNovelPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Url;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_GetNovel;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Button btn_OpenFloder;
        private System.Windows.Forms.Button btn_SaveNovel;
        private System.Windows.Forms.Label FloderPathInfo;
        private System.Windows.Forms.Button btn_SaveFolder;
        private System.Windows.Forms.TextBox txt_FloderPath;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.CheckBox checkLine;
        private System.Windows.Forms.RichTextBox txt_main;
    }
}