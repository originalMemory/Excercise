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
            this.checkMoreSpace = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txt_Url
            // 
            this.txt_Url.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_Url.Location = new System.Drawing.Point(415, 30);
            this.txt_Url.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_Url.Name = "txt_Url";
            this.txt_Url.Size = new System.Drawing.Size(484, 25);
            this.txt_Url.TabIndex = 13;
            this.txt_Url.Text = "http://cl.soze.pw/htm_data/20/1604/1920038.html";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(352, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "网址：";
            // 
            // btn_GetNovel
            // 
            this.btn_GetNovel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_GetNovel.Location = new System.Drawing.Point(908, 30);
            this.btn_GetNovel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_GetNovel.Name = "btn_GetNovel";
            this.btn_GetNovel.Size = new System.Drawing.Size(100, 29);
            this.btn_GetNovel.TabIndex = 11;
            this.btn_GetNovel.Text = "获取小说";
            this.btn_GetNovel.UseVisualStyleBackColor = true;
            this.btn_GetNovel.Click += new System.EventHandler(this.btn_GetNovel_Click);
            // 
            // bar
            // 
            this.bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bar.Location = new System.Drawing.Point(16, 129);
            this.bar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(1372, 29);
            this.bar.TabIndex = 15;
            // 
            // btn_OpenFloder
            // 
            this.btn_OpenFloder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_OpenFloder.Enabled = false;
            this.btn_OpenFloder.Location = new System.Drawing.Point(739, 919);
            this.btn_OpenFloder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_OpenFloder.Name = "btn_OpenFloder";
            this.btn_OpenFloder.Size = new System.Drawing.Size(100, 29);
            this.btn_OpenFloder.TabIndex = 20;
            this.btn_OpenFloder.Text = "打开文件夹";
            this.btn_OpenFloder.UseVisualStyleBackColor = true;
            this.btn_OpenFloder.Click += new System.EventHandler(this.btn_OpenFloder_Click);
            // 
            // btn_SaveNovel
            // 
            this.btn_SaveNovel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveNovel.Enabled = false;
            this.btn_SaveNovel.Location = new System.Drawing.Point(559, 919);
            this.btn_SaveNovel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_SaveNovel.Name = "btn_SaveNovel";
            this.btn_SaveNovel.Size = new System.Drawing.Size(100, 29);
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
            this.FloderPathInfo.Location = new System.Drawing.Point(375, 889);
            this.FloderPathInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.FloderPathInfo.Name = "FloderPathInfo";
            this.FloderPathInfo.Size = new System.Drawing.Size(93, 17);
            this.FloderPathInfo.TabIndex = 18;
            this.FloderPathInfo.Text = "保存位置：";
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(936, 885);
            this.btn_SaveFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(100, 29);
            this.btn_SaveFolder.TabIndex = 17;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            // 
            // txt_FloderPath
            // 
            this.txt_FloderPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txt_FloderPath.Location = new System.Drawing.Point(479, 885);
            this.txt_FloderPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_FloderPath.Name = "txt_FloderPath";
            this.txt_FloderPath.Size = new System.Drawing.Size(448, 25);
            this.txt_FloderPath.TabIndex = 16;
            this.txt_FloderPath.Text = "E:\\图片\\Cosplay";
            // 
            // Title
            // 
            this.Title.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Title.Location = new System.Drawing.Point(568, 80);
            this.Title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(213, 27);
            this.Title.TabIndex = 21;
            this.Title.Text = "标题 - 作者（章节数）";
            // 
            // checkLine
            // 
            this.checkLine.AutoSize = true;
            this.checkLine.Location = new System.Drawing.Point(1016, 36);
            this.checkLine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkLine.Name = "checkLine";
            this.checkLine.Size = new System.Drawing.Size(59, 19);
            this.checkLine.TabIndex = 22;
            this.checkLine.Text = "分行";
            this.checkLine.UseVisualStyleBackColor = true;
            // 
            // txt_main
            // 
            this.txt_main.Location = new System.Drawing.Point(17, 166);
            this.txt_main.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt_main.Name = "txt_main";
            this.txt_main.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txt_main.Size = new System.Drawing.Size(1369, 702);
            this.txt_main.TabIndex = 23;
            this.txt_main.Text = "";
            // 
            // checkMoreSpace
            // 
            this.checkMoreSpace.AutoSize = true;
            this.checkMoreSpace.Checked = true;
            this.checkMoreSpace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkMoreSpace.Location = new System.Drawing.Point(1083, 36);
            this.checkMoreSpace.Margin = new System.Windows.Forms.Padding(4);
            this.checkMoreSpace.Name = "checkMoreSpace";
            this.checkMoreSpace.Size = new System.Drawing.Size(89, 19);
            this.checkMoreSpace.TabIndex = 24;
            this.checkMoreSpace.Text = "空行隔开";
            this.checkMoreSpace.UseVisualStyleBackColor = true;
            // 
            // CrawNovelPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1404, 962);
            this.Controls.Add(this.checkMoreSpace);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
        private System.Windows.Forms.CheckBox checkMoreSpace;
    }
}