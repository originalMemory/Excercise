namespace MyTools.CrawImg
{
    partial class AddressDown
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
            this.UrlInfo = new System.Windows.Forms.Label();
            this.txt_address = new System.Windows.Forms.TextBox();
            this.txt_words = new System.Windows.Forms.TextBox();
            this.btn_OpenFloder = new System.Windows.Forms.Button();
            this.btn_SaveImages = new System.Windows.Forms.Button();
            this.FloderPathInfo = new System.Windows.Forms.Label();
            this.btn_SaveFolder = new System.Windows.Forms.Button();
            this.txt_FloderPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txt_Url
            // 
            this.txt_Url.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_Url.Location = new System.Drawing.Point(209, 18);
            this.txt_Url.Name = "txt_Url";
            this.txt_Url.Size = new System.Drawing.Size(299, 21);
            this.txt_Url.TabIndex = 3;
            this.txt_Url.Text = "http://bcy.net/coser/detail/2929/569930";
            // 
            // UrlInfo
            // 
            this.UrlInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.UrlInfo.AutoSize = true;
            this.UrlInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.UrlInfo.Location = new System.Drawing.Point(158, 21);
            this.UrlInfo.Name = "UrlInfo";
            this.UrlInfo.Size = new System.Drawing.Size(49, 14);
            this.UrlInfo.TabIndex = 2;
            this.UrlInfo.Text = "网址：";
            // 
            // txt_address
            // 
            this.txt_address.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_address.Location = new System.Drawing.Point(39, 60);
            this.txt_address.Multiline = true;
            this.txt_address.Name = "txt_address";
            this.txt_address.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_address.Size = new System.Drawing.Size(615, 184);
            this.txt_address.TabIndex = 4;
            // 
            // txt_words
            // 
            this.txt_words.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_words.Location = new System.Drawing.Point(39, 271);
            this.txt_words.Multiline = true;
            this.txt_words.Name = "txt_words";
            this.txt_words.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_words.Size = new System.Drawing.Size(615, 184);
            this.txt_words.TabIndex = 5;
            // 
            // btn_OpenFloder
            // 
            this.btn_OpenFloder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_OpenFloder.Enabled = false;
            this.btn_OpenFloder.Location = new System.Drawing.Point(376, 513);
            this.btn_OpenFloder.Name = "btn_OpenFloder";
            this.btn_OpenFloder.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFloder.TabIndex = 20;
            this.btn_OpenFloder.Text = "打开文件夹";
            this.btn_OpenFloder.UseVisualStyleBackColor = true;
            this.btn_OpenFloder.Click += new System.EventHandler(this.btn_OpenFloder_Click);
            // 
            // btn_SaveImages
            // 
            this.btn_SaveImages.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveImages.Location = new System.Drawing.Point(241, 513);
            this.btn_SaveImages.Name = "btn_SaveImages";
            this.btn_SaveImages.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveImages.TabIndex = 19;
            this.btn_SaveImages.Text = "保存图片";
            this.btn_SaveImages.UseVisualStyleBackColor = true;
            this.btn_SaveImages.Click += new System.EventHandler(this.btn_SaveImages_Click);
            // 
            // FloderPathInfo
            // 
            this.FloderPathInfo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.FloderPathInfo.AutoSize = true;
            this.FloderPathInfo.BackColor = System.Drawing.Color.Transparent;
            this.FloderPathInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.FloderPathInfo.Location = new System.Drawing.Point(103, 489);
            this.FloderPathInfo.Name = "FloderPathInfo";
            this.FloderPathInfo.Size = new System.Drawing.Size(77, 14);
            this.FloderPathInfo.TabIndex = 18;
            this.FloderPathInfo.Text = "保存位置：";
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(524, 486);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFolder.TabIndex = 17;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            this.btn_SaveFolder.Click += new System.EventHandler(this.btn_SaveFolder_Click);
            // 
            // txt_FloderPath
            // 
            this.txt_FloderPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txt_FloderPath.Location = new System.Drawing.Point(181, 486);
            this.txt_FloderPath.Name = "txt_FloderPath";
            this.txt_FloderPath.Size = new System.Drawing.Size(337, 21);
            this.txt_FloderPath.TabIndex = 16;
            this.txt_FloderPath.Text = "E:\\图片\\Cosplay";
            // 
            // AddressDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 567);
            this.Controls.Add(this.btn_OpenFloder);
            this.Controls.Add(this.btn_SaveImages);
            this.Controls.Add(this.FloderPathInfo);
            this.Controls.Add(this.btn_SaveFolder);
            this.Controls.Add(this.txt_FloderPath);
            this.Controls.Add(this.txt_words);
            this.Controls.Add(this.txt_address);
            this.Controls.Add(this.txt_Url);
            this.Controls.Add(this.UrlInfo);
            this.Name = "AddressDown";
            this.Text = "AddressDown";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_Url;
        private System.Windows.Forms.Label UrlInfo;
        private System.Windows.Forms.TextBox txt_address;
        private System.Windows.Forms.TextBox txt_words;
        private System.Windows.Forms.Button btn_OpenFloder;
        private System.Windows.Forms.Button btn_SaveImages;
        private System.Windows.Forms.Label FloderPathInfo;
        private System.Windows.Forms.Button btn_SaveFolder;
        private System.Windows.Forms.TextBox txt_FloderPath;
    }
}