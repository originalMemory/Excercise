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
            this.txt_SourcePath = new System.Windows.Forms.TextBox();
            this.cb_IsDeep = new System.Windows.Forms.CheckBox();
            this.btn_GetSourcePath = new System.Windows.Forms.Button();
            this.btn_GetTargetPath = new System.Windows.Forms.Button();
            this.txt_TargetPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_LimitNum = new System.Windows.Forms.TextBox();
            this.txt_ExcludeFloder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_Relist = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pic_Working = new System.Windows.Forms.PictureBox();
            this.proBar = new System.Windows.Forms.ProgressBar();
            this.dialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txt_reg = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Working)).BeginInit();
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
            // txt_SourcePath
            // 
            this.txt_SourcePath.Location = new System.Drawing.Point(90, 12);
            this.txt_SourcePath.Name = "txt_SourcePath";
            this.txt_SourcePath.Size = new System.Drawing.Size(218, 21);
            this.txt_SourcePath.TabIndex = 1;
            this.txt_SourcePath.Text = "L:\\yande2";
            // 
            // cb_IsDeep
            // 
            this.cb_IsDeep.AutoSize = true;
            this.cb_IsDeep.Checked = true;
            this.cb_IsDeep.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_IsDeep.Location = new System.Drawing.Point(419, 19);
            this.cb_IsDeep.Name = "cb_IsDeep";
            this.cb_IsDeep.Size = new System.Drawing.Size(108, 16);
            this.cb_IsDeep.TabIndex = 2;
            this.cb_IsDeep.Text = "是否搜索子目录";
            this.cb_IsDeep.UseVisualStyleBackColor = true;
            // 
            // btn_GetSourcePath
            // 
            this.btn_GetSourcePath.Location = new System.Drawing.Point(314, 12);
            this.btn_GetSourcePath.Name = "btn_GetSourcePath";
            this.btn_GetSourcePath.Size = new System.Drawing.Size(94, 23);
            this.btn_GetSourcePath.TabIndex = 4;
            this.btn_GetSourcePath.Text = "选择原图目录";
            this.btn_GetSourcePath.UseVisualStyleBackColor = true;
            this.btn_GetSourcePath.Click += new System.EventHandler(this.btn_GetSourcePath_Click);
            // 
            // btn_GetTargetPath
            // 
            this.btn_GetTargetPath.Location = new System.Drawing.Point(314, 48);
            this.btn_GetTargetPath.Name = "btn_GetTargetPath";
            this.btn_GetTargetPath.Size = new System.Drawing.Size(94, 23);
            this.btn_GetTargetPath.TabIndex = 8;
            this.btn_GetTargetPath.Text = "选择目标目录";
            this.btn_GetTargetPath.UseVisualStyleBackColor = true;
            this.btn_GetTargetPath.Click += new System.EventHandler(this.btn_GetTargetPath_Click);
            // 
            // txt_TargetPath
            // 
            this.txt_TargetPath.Location = new System.Drawing.Point(90, 48);
            this.txt_TargetPath.Name = "txt_TargetPath";
            this.txt_TargetPath.Size = new System.Drawing.Size(218, 21);
            this.txt_TargetPath.TabIndex = 6;
            this.txt_TargetPath.Text = "L:\\yande";
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
            // txt_LimitNum
            // 
            this.txt_LimitNum.Location = new System.Drawing.Point(498, 49);
            this.txt_LimitNum.Name = "txt_LimitNum";
            this.txt_LimitNum.Size = new System.Drawing.Size(55, 21);
            this.txt_LimitNum.TabIndex = 10;
            this.txt_LimitNum.Text = "500";
            // 
            // txt_ExcludeFloder
            // 
            this.txt_ExcludeFloder.Location = new System.Drawing.Point(665, 16);
            this.txt_ExcludeFloder.Name = "txt_ExcludeFloder";
            this.txt_ExcludeFloder.Size = new System.Drawing.Size(129, 21);
            this.txt_ExcludeFloder.TabIndex = 12;
            this.txt_ExcludeFloder.Text = "抱枕";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(534, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "只复制的文件夹名称：";
            // 
            // btn_Relist
            // 
            this.btn_Relist.Location = new System.Drawing.Point(345, 81);
            this.btn_Relist.Name = "btn_Relist";
            this.btn_Relist.Size = new System.Drawing.Size(103, 37);
            this.btn_Relist.TabIndex = 13;
            this.btn_Relist.Text = "开始重排序";
            this.btn_Relist.UseVisualStyleBackColor = true;
            this.btn_Relist.Click += new System.EventHandler(this.btn_Relist_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(335, 166);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(467, 368);
            this.richTextBox1.TabIndex = 15;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // pic_Working
            // 
            this.pic_Working.Image = global::MyTools.Properties.Resources.jiance;
            this.pic_Working.Location = new System.Drawing.Point(25, 209);
            this.pic_Working.Name = "pic_Working";
            this.pic_Working.Size = new System.Drawing.Size(291, 267);
            this.pic_Working.TabIndex = 14;
            this.pic_Working.TabStop = false;
            this.pic_Working.Visible = false;
            // 
            // proBar
            // 
            this.proBar.Location = new System.Drawing.Point(12, 124);
            this.proBar.Name = "proBar";
            this.proBar.Size = new System.Drawing.Size(790, 30);
            this.proBar.TabIndex = 16;
            // 
            // txt_reg
            // 
            this.txt_reg.Location = new System.Drawing.Point(643, 50);
            this.txt_reg.Name = "txt_reg";
            this.txt_reg.Size = new System.Drawing.Size(151, 21);
            this.txt_reg.TabIndex = 18;
            this.txt_reg.Text = ".{5,10} (?<num>\\d+?) {1}.*";
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
            // RelistImgWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 546);
            this.Controls.Add(this.txt_reg);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.proBar);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.pic_Working);
            this.Controls.Add(this.btn_Relist);
            this.Controls.Add(this.txt_ExcludeFloder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_LimitNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_GetTargetPath);
            this.Controls.Add(this.txt_TargetPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_GetSourcePath);
            this.Controls.Add(this.cb_IsDeep);
            this.Controls.Add(this.txt_SourcePath);
            this.Controls.Add(this.label1);
            this.Name = "RelistImgWin";
            this.Text = "重排列图片";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RelistImgWin_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Working)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_SourcePath;
        private System.Windows.Forms.CheckBox cb_IsDeep;
        private System.Windows.Forms.Button btn_GetSourcePath;
        private System.Windows.Forms.Button btn_GetTargetPath;
        private System.Windows.Forms.TextBox txt_TargetPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_LimitNum;
        private System.Windows.Forms.TextBox txt_ExcludeFloder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Relist;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox pic_Working;
        private System.Windows.Forms.ProgressBar proBar;
        private System.Windows.Forms.FolderBrowserDialog dialog;
        private System.Windows.Forms.TextBox txt_reg;
        private System.Windows.Forms.Label label5;
    }
}