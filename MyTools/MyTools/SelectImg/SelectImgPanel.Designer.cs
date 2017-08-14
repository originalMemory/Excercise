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
            this.btnSourcePath = new System.Windows.Forms.Button();
            this.btnTargetPath = new System.Windows.Forms.Button();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.txtMinImgWidth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbImgType = new System.Windows.Forms.ComboBox();
            this.pbCopy = new System.Windows.Forms.ProgressBar();
            this.picRunning = new System.Windows.Forms.PictureBox();
            this.rtxtPicList = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExcludeFloder = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.checkTime = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picRunning)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSourcePath
            // 
            this.btnSourcePath.Location = new System.Drawing.Point(306, 21);
            this.btnSourcePath.Name = "btnSourcePath";
            this.btnSourcePath.Size = new System.Drawing.Size(75, 23);
            this.btnSourcePath.TabIndex = 0;
            this.btnSourcePath.Text = "选择源目录";
            this.btnSourcePath.UseVisualStyleBackColor = true;
            this.btnSourcePath.Click += new System.EventHandler(this.btn_SourcePath_Click);
            // 
            // btnTargetPath
            // 
            this.btnTargetPath.Location = new System.Drawing.Point(306, 52);
            this.btnTargetPath.Name = "btnTargetPath";
            this.btnTargetPath.Size = new System.Drawing.Size(90, 23);
            this.btnTargetPath.TabIndex = 1;
            this.btnTargetPath.Text = "选择目标目录";
            this.btnTargetPath.UseVisualStyleBackColor = true;
            this.btnTargetPath.Click += new System.EventHandler(this.btn_TargetPath_Click);
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(94, 22);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(200, 21);
            this.txtSourcePath.TabIndex = 2;
            this.txtSourcePath.Text = "M:\\H\\图片\\Cosplay\\Flameworks";
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.Location = new System.Drawing.Point(94, 52);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(200, 21);
            this.txtTargetPath.TabIndex = 3;
            this.txtTargetPath.Text = "E:\\图片\\Cosplay\\壁纸";
            // 
            // txtMinImgWidth
            // 
            this.txtMinImgWidth.Location = new System.Drawing.Point(257, 92);
            this.txtMinImgWidth.Name = "txtMinImgWidth";
            this.txtMinImgWidth.Size = new System.Drawing.Size(75, 21);
            this.txtMinImgWidth.TabIndex = 4;
            this.txtMinImgWidth.Text = "600";
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
            // cmbImgType
            // 
            this.cmbImgType.FormattingEnabled = true;
            this.cmbImgType.Items.AddRange(new object[] {
            "横图",
            "竖图"});
            this.cmbImgType.Location = new System.Drawing.Point(94, 92);
            this.cmbImgType.Name = "cmbImgType";
            this.cmbImgType.Size = new System.Drawing.Size(75, 20);
            this.cmbImgType.TabIndex = 10;
            this.cmbImgType.Text = "横图";
            // 
            // pbCopy
            // 
            this.pbCopy.Location = new System.Drawing.Point(12, 174);
            this.pbCopy.Name = "pbCopy";
            this.pbCopy.Size = new System.Drawing.Size(713, 23);
            this.pbCopy.TabIndex = 11;
            // 
            // picRunning
            // 
            this.picRunning.Image = global::MyTools.Properties.Resources.jiance;
            this.picRunning.Location = new System.Drawing.Point(41, 220);
            this.picRunning.Name = "picRunning";
            this.picRunning.Size = new System.Drawing.Size(278, 259);
            this.picRunning.TabIndex = 12;
            this.picRunning.TabStop = false;
            this.picRunning.Visible = false;
            // 
            // rtxtPicList
            // 
            this.rtxtPicList.Location = new System.Drawing.Point(362, 220);
            this.rtxtPicList.Name = "rtxtPicList";
            this.rtxtPicList.Size = new System.Drawing.Size(346, 259);
            this.rtxtPicList.TabIndex = 13;
            this.rtxtPicList.Text = "";
            this.rtxtPicList.WordWrap = false;
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
            // txtExcludeFloder
            // 
            this.txtExcludeFloder.Location = new System.Drawing.Point(511, 22);
            this.txtExcludeFloder.Name = "txtExcludeFloder";
            this.txtExcludeFloder.Size = new System.Drawing.Size(200, 21);
            this.txtExcludeFloder.TabIndex = 14;
            this.txtExcludeFloder.Text = "新建文件夹";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(306, 131);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 30);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "开始复制";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btn_Start_Click);
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
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(428, 92);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(127, 21);
            this.dtpStart.TabIndex = 19;
            this.dtpStart.Value = new System.DateTime(2016, 11, 12, 0, 0, 0, 0);
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.Location = new System.Drawing.Point(569, 95);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(96, 16);
            this.checkTime.TabIndex = 20;
            this.checkTime.Text = "是否筛选时间";
            this.checkTime.UseVisualStyleBackColor = true;
            // 
            // SelectImgPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 515);
            this.Controls.Add(this.checkTime);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtExcludeFloder);
            this.Controls.Add(this.rtxtPicList);
            this.Controls.Add(this.picRunning);
            this.Controls.Add(this.pbCopy);
            this.Controls.Add(this.cmbImgType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMinImgWidth);
            this.Controls.Add(this.txtTargetPath);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.btnTargetPath);
            this.Controls.Add(this.btnSourcePath);
            this.Name = "SelectImgPanel";
            this.Text = "SelectImgPanel";
            ((System.ComponentModel.ISupportInitialize)(this.picRunning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSourcePath;
        private System.Windows.Forms.Button btnTargetPath;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.TextBox txtMinImgWidth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbImgType;
        private System.Windows.Forms.ProgressBar pbCopy;
        private System.Windows.Forms.PictureBox picRunning;
        private System.Windows.Forms.RichTextBox rtxtPicList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExcludeFloder;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.CheckBox checkTime;
    }
}