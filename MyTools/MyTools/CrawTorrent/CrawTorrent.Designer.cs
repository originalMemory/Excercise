namespace MyTools.CrawTorrent
{
    partial class CrawTorrent
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
            this.label_name = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label_path = new System.Windows.Forms.Label();
            this.txt_path = new System.Windows.Forms.TextBox();
            this.btn_down = new System.Windows.Forms.Button();
            this.btn_SaveFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(89, 67);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(53, 12);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "动漫名：";
            // 
            // txt_name
            // 
            this.txt_name.Location = new System.Drawing.Point(168, 64);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(146, 21);
            this.txt_name.TabIndex = 1;
            // 
            // label_path
            // 
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(77, 107);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(65, 12);
            this.label_path.TabIndex = 2;
            this.label_path.Text = "保存路径：";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(168, 104);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(146, 21);
            this.txt_path.TabIndex = 3;
            // 
            // btn_down
            // 
            this.btn_down.Location = new System.Drawing.Point(168, 198);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(75, 23);
            this.btn_down.TabIndex = 4;
            this.btn_down.Text = "下载";
            this.btn_down.UseVisualStyleBackColor = true;
            this.btn_down.Click += new System.EventHandler(this.btn_down_Click);
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(337, 102);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFolder.TabIndex = 7;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            this.btn_SaveFolder.Click += new System.EventHandler(this.btn_SaveFolder_Click);
            // 
            // CrawTorrent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 354);
            this.Controls.Add(this.btn_SaveFolder);
            this.Controls.Add(this.btn_down);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.label_path);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.label_name);
            this.Name = "CrawTorrent";
            this.Text = "CrawTorrent";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Button btn_down;
        private System.Windows.Forms.Button btn_SaveFolder;
    }
}