namespace MyTools.TextEdit
{
    partial class TextEditPanel
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
            this.btn_merge = new System.Windows.Forms.Button();
            this.txt_main = new System.Windows.Forms.TextBox();
            this.bar = new System.Windows.Forms.ProgressBar();
            this.btn_replace = new System.Windows.Forms.Button();
            this.txt_Search = new System.Windows.Forms.TextBox();
            this.btn_regex = new System.Windows.Forms.Button();
            this.txt_replace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_regChapter = new System.Windows.Forms.TextBox();
            this.btn_addSpaceLine = new System.Windows.Forms.Button();
            this.checkLine = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_merge
            // 
            this.btn_merge.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_merge.Location = new System.Drawing.Point(749, 12);
            this.btn_merge.Name = "btn_merge";
            this.btn_merge.Size = new System.Drawing.Size(75, 23);
            this.btn_merge.TabIndex = 0;
            this.btn_merge.Text = "合并";
            this.btn_merge.UseVisualStyleBackColor = true;
            this.btn_merge.Click += new System.EventHandler(this.btn_merge_Click);
            // 
            // txt_main
            // 
            this.txt_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_main.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_main.Location = new System.Drawing.Point(12, 105);
            this.txt_main.MaxLength = 0;
            this.txt_main.Multiline = true;
            this.txt_main.Name = "txt_main";
            this.txt_main.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_main.Size = new System.Drawing.Size(1240, 564);
            this.txt_main.TabIndex = 1;
            // 
            // bar
            // 
            this.bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bar.Location = new System.Drawing.Point(12, 76);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(1240, 23);
            this.bar.TabIndex = 2;
            // 
            // btn_replace
            // 
            this.btn_replace.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_replace.Location = new System.Drawing.Point(794, 44);
            this.btn_replace.Name = "btn_replace";
            this.btn_replace.Size = new System.Drawing.Size(75, 23);
            this.btn_replace.TabIndex = 3;
            this.btn_replace.Text = "替换";
            this.btn_replace.UseVisualStyleBackColor = true;
            this.btn_replace.Click += new System.EventHandler(this.btn_replace_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_Search.Location = new System.Drawing.Point(415, 44);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.Size = new System.Drawing.Size(175, 21);
            this.txt_Search.TabIndex = 4;
            // 
            // btn_regex
            // 
            this.btn_regex.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_regex.Location = new System.Drawing.Point(875, 44);
            this.btn_regex.Name = "btn_regex";
            this.btn_regex.Size = new System.Drawing.Size(75, 23);
            this.btn_regex.TabIndex = 5;
            this.btn_regex.Text = "正则替换";
            this.btn_regex.UseVisualStyleBackColor = true;
            this.btn_regex.Click += new System.EventHandler(this.btn_regex_Click);
            // 
            // txt_replace
            // 
            this.txt_replace.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_replace.Location = new System.Drawing.Point(655, 44);
            this.txt_replace.Name = "txt_replace";
            this.txt_replace.Size = new System.Drawing.Size(133, 21);
            this.txt_replace.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(470, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "章节名正则匹配表达式：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(368, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "查找：";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(596, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "替换为：";
            // 
            // txt_regChapter
            // 
            this.txt_regChapter.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_regChapter.Location = new System.Drawing.Point(613, 14);
            this.txt_regChapter.Name = "txt_regChapter";
            this.txt_regChapter.Size = new System.Drawing.Size(130, 21);
            this.txt_regChapter.TabIndex = 10;
            this.txt_regChapter.Text = "第.章.{0,50}";
            // 
            // btn_addSpaceLine
            // 
            this.btn_addSpaceLine.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_addSpaceLine.Location = new System.Drawing.Point(830, 12);
            this.btn_addSpaceLine.Name = "btn_addSpaceLine";
            this.btn_addSpaceLine.Size = new System.Drawing.Size(75, 23);
            this.btn_addSpaceLine.TabIndex = 11;
            this.btn_addSpaceLine.Text = "添加空行";
            this.btn_addSpaceLine.UseVisualStyleBackColor = true;
            this.btn_addSpaceLine.Click += new System.EventHandler(this.btn_addSpaceLine_Click);
            // 
            // checkLine
            // 
            this.checkLine.AutoSize = true;
            this.checkLine.Checked = true;
            this.checkLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLine.Location = new System.Drawing.Point(912, 17);
            this.checkLine.Name = "checkLine";
            this.checkLine.Size = new System.Drawing.Size(48, 16);
            this.checkLine.TabIndex = 12;
            this.checkLine.Text = "分行";
            this.checkLine.UseVisualStyleBackColor = true;
            // 
            // TextEditPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.checkLine);
            this.Controls.Add(this.btn_addSpaceLine);
            this.Controls.Add(this.txt_regChapter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_replace);
            this.Controls.Add(this.btn_regex);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.btn_replace);
            this.Controls.Add(this.bar);
            this.Controls.Add(this.txt_main);
            this.Controls.Add(this.btn_merge);
            this.Name = "TextEditPanel";
            this.Text = "txtEdit";
            this.Load += new System.EventHandler(this.txtEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_merge;
        private System.Windows.Forms.TextBox txt_main;
        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Button btn_replace;
        private System.Windows.Forms.TextBox txt_Search;
        private System.Windows.Forms.Button btn_regex;
        private System.Windows.Forms.TextBox txt_replace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_regChapter;
        private System.Windows.Forms.Button btn_addSpaceLine;
        private System.Windows.Forms.CheckBox checkLine;
    }
}