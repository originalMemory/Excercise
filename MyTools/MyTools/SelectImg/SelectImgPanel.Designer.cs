namespace MyTools.SelectImg
{
    partial class SelectImgPanel
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
            this.btn_SourcePath = new System.Windows.Forms.Button();
            this.btn_TargetPath = new System.Windows.Forms.Button();
            this.txt_SourcePath = new System.Windows.Forms.TextBox();
            this.txt_TargetPath = new System.Windows.Forms.TextBox();
            this.txt_MiniWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_ImgType = new System.Windows.Forms.ComboBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_ExcludeFloder = new System.Windows.Forms.TextBox();
            this.btn_Start = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dt_Start = new System.Windows.Forms.DateTimePicker();
            this.check_Time = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_SourcePath
            // 
            this.btn_SourcePath.Location = new System.Drawing.Point(306, 21);
            this.btn_SourcePath.Name = "btn_SourcePath";
            this.btn_SourcePath.Size = new System.Drawing.Size(75, 23);
            this.btn_SourcePath.TabIndex = 0;
            this.btn_SourcePath.Text = "选择源目录";
            this.btn_SourcePath.UseVisualStyleBackColor = true;
            this.btn_SourcePath.Click += new System.EventHandler(this.btn_SourcePath_Click);
            // 
            // btn_TargetPath
            // 
            this.btn_TargetPath.Location = new System.Drawing.Point(306, 52);
            this.btn_TargetPath.Name = "btn_TargetPath";
            this.btn_TargetPath.Size = new System.Drawing.Size(90, 23);
            this.btn_TargetPath.TabIndex = 1;
            this.btn_TargetPath.Text = "选择目标目录";
            this.btn_TargetPath.UseVisualStyleBackColor = true;
            this.btn_TargetPath.Click += new System.EventHandler(this.btn_TargetPath_Click);
            // 
            // txt_SourcePath
            // 
            this.txt_SourcePath.Location = new System.Drawing.Point(94, 22);
            this.txt_SourcePath.Name = "txt_SourcePath";
            this.txt_SourcePath.Size = new System.Drawing.Size(200, 21);
            this.txt_SourcePath.TabIndex = 2;
            this.txt_SourcePath.Text = "M:\\H\\图片\\Cosplay\\Flameworks";
            // 
            // txt_TargetPath
            // 
            this.txt_TargetPath.Location = new System.Drawing.Point(94, 52);
            this.txt_TargetPath.Name = "txt_TargetPath";
            this.txt_TargetPath.Size = new System.Drawing.Size(200, 21);
            this.txt_TargetPath.TabIndex = 3;
            this.txt_TargetPath.Text = "E:\\图片\\Cosplay\\壁纸";
            // 
            // txt_MiniWidth
            // 
            this.txt_MiniWidth.Location = new System.Drawing.Point(257, 92);
            this.txt_MiniWidth.Name = "txt_MiniWidth";
            this.txt_MiniWidth.Size = new System.Drawing.Size(75, 21);
            this.txt_MiniWidth.TabIndex = 4;
            this.txt_MiniWidth.Text = "600";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "源目录：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "目标目录：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "图片类型：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "最小宽度：";
            // 
            // cmb_ImgType
            // 
            this.cmb_ImgType.FormattingEnabled = true;
            this.cmb_ImgType.Items.AddRange(new object[] {
            "横图",
            "竖图"});
            this.cmb_ImgType.Location = new System.Drawing.Point(94, 92);
            this.cmb_ImgType.Name = "cmb_ImgType";
            this.cmb_ImgType.Size = new System.Drawing.Size(75, 20);
            this.cmb_ImgType.TabIndex = 10;
            this.cmb_ImgType.Text = "横图";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 174);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(713, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MyTools.Properties.Resources.jiance;
            this.pictureBox1.Location = new System.Drawing.Point(41, 220);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(278, 259);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(362, 220);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(346, 259);
            this.richTextBox1.TabIndex = 13;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(416, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "排除文件夹名：";
            // 
            // txt_ExcludeFloder
            // 
            this.txt_ExcludeFloder.Location = new System.Drawing.Point(511, 22);
            this.txt_ExcludeFloder.Name = "txt_ExcludeFloder";
            this.txt_ExcludeFloder.Size = new System.Drawing.Size(200, 21);
            this.txt_ExcludeFloder.TabIndex = 14;
            this.txt_ExcludeFloder.Text = "新建文件夹";
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(306, 131);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(103, 30);
            this.btn_Start.TabIndex = 16;
            this.btn_Start.Text = "开始复制";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(356, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "开始日期：";
            // 
            // dt_Start
            // 
            this.dt_Start.Location = new System.Drawing.Point(428, 92);
            this.dt_Start.Name = "dt_Start";
            this.dt_Start.Size = new System.Drawing.Size(127, 21);
            this.dt_Start.TabIndex = 19;
            this.dt_Start.Value = new System.DateTime(2016, 11, 12, 0, 0, 0, 0);
            // 
            // check_Time
            // 
            this.check_Time.AutoSize = true;
            this.check_Time.Location = new System.Drawing.Point(569, 95);
            this.check_Time.Name = "check_Time";
            this.check_Time.Size = new System.Drawing.Size(96, 16);
            this.check_Time.TabIndex = 20;
            this.check_Time.Text = "是否筛选时间";
            this.check_Time.UseVisualStyleBackColor = true;
            // 
            // SelectImgPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 515);
            this.Controls.Add(this.check_Time);
            this.Controls.Add(this.dt_Start);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_Start);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_ExcludeFloder);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.cmb_ImgType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_MiniWidth);
            this.Controls.Add(this.txt_TargetPath);
            this.Controls.Add(this.txt_SourcePath);
            this.Controls.Add(this.btn_TargetPath);
            this.Controls.Add(this.btn_SourcePath);
            this.Name = "SelectImgPanel";
            this.Text = "SelectImgPanel";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_SourcePath;
        private System.Windows.Forms.Button btn_TargetPath;
        private System.Windows.Forms.TextBox txt_SourcePath;
        private System.Windows.Forms.TextBox txt_TargetPath;
        private System.Windows.Forms.TextBox txt_MiniWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_ImgType;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_ExcludeFloder;
        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dt_Start;
        private System.Windows.Forms.CheckBox check_Time;
    }
}