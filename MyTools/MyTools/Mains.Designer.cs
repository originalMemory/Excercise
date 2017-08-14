namespace MyTools
{
    partial class Mains
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mains));
            this.btn_CrawImg = new System.Windows.Forms.Button();
            this.btn_TextEdit = new System.Windows.Forms.Button();
            this.btn_SelectImg = new System.Windows.Forms.Button();
            this.btn_RelistImg = new System.Windows.Forms.Button();
            this.btn_dmhyCraw = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_CrawImg
            // 
            this.btn_CrawImg.Location = new System.Drawing.Point(115, 72);
            this.btn_CrawImg.Name = "btn_CrawImg";
            this.btn_CrawImg.Size = new System.Drawing.Size(105, 23);
            this.btn_CrawImg.TabIndex = 0;
            this.btn_CrawImg.Text = "抓取半次元图片";
            this.btn_CrawImg.UseVisualStyleBackColor = true;
            this.btn_CrawImg.Click += new System.EventHandler(this.btn_CrawImg_Click);
            // 
            // btn_TextEdit
            // 
            this.btn_TextEdit.Location = new System.Drawing.Point(274, 72);
            this.btn_TextEdit.Name = "btn_TextEdit";
            this.btn_TextEdit.Size = new System.Drawing.Size(75, 23);
            this.btn_TextEdit.TabIndex = 1;
            this.btn_TextEdit.Text = "文本处理";
            this.btn_TextEdit.UseVisualStyleBackColor = true;
            this.btn_TextEdit.Click += new System.EventHandler(this.btn_TextEdit_Click);
            // 
            // btn_SelectImg
            // 
            this.btn_SelectImg.Location = new System.Drawing.Point(115, 122);
            this.btn_SelectImg.Name = "btn_SelectImg";
            this.btn_SelectImg.Size = new System.Drawing.Size(105, 23);
            this.btn_SelectImg.TabIndex = 2;
            this.btn_SelectImg.Text = "条件复制图片";
            this.btn_SelectImg.UseVisualStyleBackColor = true;
            this.btn_SelectImg.Click += new System.EventHandler(this.btn_SelectImg_Click);
            // 
            // btn_RelistImg
            // 
            this.btn_RelistImg.Location = new System.Drawing.Point(274, 122);
            this.btn_RelistImg.Name = "btn_RelistImg";
            this.btn_RelistImg.Size = new System.Drawing.Size(105, 23);
            this.btn_RelistImg.TabIndex = 3;
            this.btn_RelistImg.Text = "图片重排序";
            this.btn_RelistImg.UseVisualStyleBackColor = true;
            this.btn_RelistImg.Click += new System.EventHandler(this.btn_RelistImg_Click);
            // 
            // btn_dmhyCraw
            // 
            this.btn_dmhyCraw.Location = new System.Drawing.Point(115, 166);
            this.btn_dmhyCraw.Name = "btn_dmhyCraw";
            this.btn_dmhyCraw.Size = new System.Drawing.Size(105, 23);
            this.btn_dmhyCraw.TabIndex = 4;
            this.btn_dmhyCraw.Text = "花园种子抓取";
            this.btn_dmhyCraw.UseVisualStyleBackColor = true;
            this.btn_dmhyCraw.Click += new System.EventHandler(this.btn_dmhyCraw_Click);
            // 
            // Mains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 313);
            this.Controls.Add(this.btn_dmhyCraw);
            this.Controls.Add(this.btn_RelistImg);
            this.Controls.Add(this.btn_SelectImg);
            this.Controls.Add(this.btn_TextEdit);
            this.Controls.Add(this.btn_CrawImg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Mains";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mains";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mains_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_CrawImg;
        private System.Windows.Forms.Button btn_TextEdit;
        private System.Windows.Forms.Button btn_SelectImg;
        private System.Windows.Forms.Button btn_RelistImg;
        private System.Windows.Forms.Button btn_dmhyCraw;
    }
}