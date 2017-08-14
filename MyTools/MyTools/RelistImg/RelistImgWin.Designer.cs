namespace MyTools.RelistImg
{
    partial class RelistImgWin
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.checkDeep = new System.Windows.Forms.CheckBox();
            this.btnGetSourcePath = new System.Windows.Forms.Button();
            this.btnGetTargetPath = new System.Windows.Forms.Button();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLimitNum = new System.Windows.Forms.TextBox();
            this.txtExcludeFloder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.rtxtImgList = new System.Windows.Forms.RichTextBox();
            this.picRunning = new System.Windows.Forms.PictureBox();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txtSortReg = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkTime = new System.Windows.Forms.CheckBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbImgType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMinImgWidth = new System.Windows.Forms.TextBox();
            this.checkCopy = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picRunning)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "原图路径：";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(90, 12);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(218, 21);
            this.txtSourcePath.TabIndex = 1;
            this.txtSourcePath.Text = "L:\\yande2";
            // 
            // checkDeep
            // 
            this.checkDeep.AutoSize = true;
            this.checkDeep.Checked = true;
            this.checkDeep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDeep.Location = new System.Drawing.Point(419, 19);
            this.checkDeep.Name = "checkDeep";
            this.checkDeep.Size = new System.Drawing.Size(108, 16);
            this.checkDeep.TabIndex = 2;
            this.checkDeep.Text = "是否搜索子目录";
            this.checkDeep.UseVisualStyleBackColor = true;
            // 
            // btnGetSourcePath
            // 
            this.btnGetSourcePath.Location = new System.Drawing.Point(314, 12);
            this.btnGetSourcePath.Name = "btnGetSourcePath";
            this.btnGetSourcePath.Size = new System.Drawing.Size(94, 23);
            this.btnGetSourcePath.TabIndex = 4;
            this.btnGetSourcePath.Text = "选择原图目录";
            this.btnGetSourcePath.UseVisualStyleBackColor = true;
            this.btnGetSourcePath.Click += new System.EventHandler(this.btn_GetSourcePath_Click);
            // 
            // btnGetTargetPath
            // 
            this.btnGetTargetPath.Location = new System.Drawing.Point(314, 48);
            this.btnGetTargetPath.Name = "btnGetTargetPath";
            this.btnGetTargetPath.Size = new System.Drawing.Size(94, 23);
            this.btnGetTargetPath.TabIndex = 8;
            this.btnGetTargetPath.Text = "选择目标目录";
            this.btnGetTargetPath.UseVisualStyleBackColor = true;
            this.btnGetTargetPath.Click += new System.EventHandler(this.btn_GetTargetPath_Click);
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.Location = new System.Drawing.Point(90, 48);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(218, 21);
            this.txtTargetPath.TabIndex = 6;
            this.txtTargetPath.Text = "L:\\yande";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "目标路径：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "单图数量：";
            // 
            // txtLimitNum
            // 
            this.txtLimitNum.Location = new System.Drawing.Point(498, 49);
            this.txtLimitNum.Name = "txtLimitNum";
            this.txtLimitNum.Size = new System.Drawing.Size(55, 21);
            this.txtLimitNum.TabIndex = 10;
            this.txtLimitNum.Text = "500";
            // 
            // txtExcludeFloder
            // 
            this.txtExcludeFloder.Location = new System.Drawing.Point(665, 16);
            this.txtExcludeFloder.Name = "txtExcludeFloder";
            this.txtExcludeFloder.Size = new System.Drawing.Size(129, 21);
            this.txtExcludeFloder.TabIndex = 12;
            this.txtExcludeFloder.Text = "抱枕";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(570, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "排除文件夹名：";
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(345, 135);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(103, 37);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "开始重排序";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btn_Relist_Click);
            // 
            // rtxtImgList
            // 
            this.rtxtImgList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtImgList.Location = new System.Drawing.Point(335, 220);
            this.rtxtImgList.Name = "rtxtImgList";
            this.rtxtImgList.Size = new System.Drawing.Size(467, 368);
            this.rtxtImgList.TabIndex = 15;
            this.rtxtImgList.Text = "";
            this.rtxtImgList.WordWrap = false;
            // 
            // picRunning
            // 
            this.picRunning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picRunning.Image = global::MyTools.Properties.Resources.jiance;
            this.picRunning.Location = new System.Drawing.Point(25, 263);
            this.picRunning.Name = "picRunning";
            this.picRunning.Size = new System.Drawing.Size(291, 267);
            this.picRunning.TabIndex = 14;
            this.picRunning.TabStop = false;
            this.picRunning.Visible = false;
            // 
            // proBar
            // 
            this.proBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.proBar.Location = new System.Drawing.Point(12, 178);
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(790, 30);
            this.proBar.TabIndex = 16;
            // 
            // txtSortReg
            // 
            this.txtSortReg.Location = new System.Drawing.Point(643, 50);
            this.txtSortReg.Name = "txtSortReg";
            this.txtSortReg.Size = new System.Drawing.Size(151, 21);
            this.txtSortReg.TabIndex = 18;
            this.txtSortReg.Text = ".{5,10} (?<num>\\d+?) {1}.*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(572, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "序号正则：";
            // 
            // checkTime
            // 
            this.checkTime.AutoSize = true;
            this.checkTime.Location = new System.Drawing.Point(565, 93);
            this.checkTime.Name = "checkTime";
            this.checkTime.Size = new System.Drawing.Size(96, 16);
            this.checkTime.TabIndex = 27;
            this.checkTime.Text = "是否筛选时间";
            this.checkTime.UseVisualStyleBackColor = true;
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(424, 90);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(127, 21);
            this.dtpStart.TabIndex = 26;
            this.dtpStart.Value = new System.DateTime(2016, 11, 12, 0, 0, 0, 0);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(352, 94);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 25;
            this.label6.Text = "开始日期：";
            // 
            // cmbImgType
            // 
            this.cmbImgType.FormattingEnabled = true;
            this.cmbImgType.Items.AddRange(new object[] {
            "横图",
            "竖图"});
            this.cmbImgType.Location = new System.Drawing.Point(90, 90);
            this.cmbImgType.Name = "cmbImgType";
            this.cmbImgType.Size = new System.Drawing.Size(75, 20);
            this.cmbImgType.TabIndex = 24;
            this.cmbImgType.Text = "横图";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(182, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "最小宽度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 93);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 22;
            this.label8.Text = "图片类型：";
            // 
            // txtMinImgWidth
            // 
            this.txtMinImgWidth.Location = new System.Drawing.Point(253, 90);
            this.txtMinImgWidth.Name = "txtMinImgWidth";
            this.txtMinImgWidth.Size = new System.Drawing.Size(75, 21);
            this.txtMinImgWidth.TabIndex = 21;
            this.txtMinImgWidth.Text = "600";
            // 
            // checkCopy
            // 
            this.checkCopy.AutoSize = true;
            this.checkCopy.Location = new System.Drawing.Point(672, 94);
            this.checkCopy.Name = "checkCopy";
            this.checkCopy.Size = new System.Drawing.Size(108, 16);
            this.checkCopy.TabIndex = 28;
            this.checkCopy.Text = "是否仅复制图片";
            this.checkCopy.UseVisualStyleBackColor = true;
            // 
            // RelistImgWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 600);
            this.Controls.Add(this.checkCopy);
            this.Controls.Add(this.checkTime);
            this.Controls.Add(this.dtpStart);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbImgType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtMinImgWidth);
            this.Controls.Add(this.txtSortReg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.proBar);
            this.Controls.Add(this.rtxtImgList);
            this.Controls.Add(this.picRunning);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtExcludeFloder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLimitNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnGetTargetPath);
            this.Controls.Add(this.txtTargetPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGetSourcePath);
            this.Controls.Add(this.checkDeep);
            this.Controls.Add(this.txtSourcePath);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "RelistImgWin";
            this.Text = "重排列图片";
            ((System.ComponentModel.ISupportInitialize)(this.picRunning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.CheckBox checkDeep;
        private System.Windows.Forms.Button btnGetSourcePath;
        private System.Windows.Forms.Button btnGetTargetPath;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLimitNum;
        private System.Windows.Forms.TextBox txtExcludeFloder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox rtxtImgList;
        private System.Windows.Forms.PictureBox picRunning;
        private System.Windows.Forms.ProgressBar proBar;
        private System.Windows.Forms.FolderBrowserDialog dialog;
        private System.Windows.Forms.TextBox txtSortReg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkTime;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbImgType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtMinImgWidth;
        private System.Windows.Forms.CheckBox checkCopy;
    }
}