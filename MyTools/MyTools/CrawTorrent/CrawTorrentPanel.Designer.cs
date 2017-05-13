namespace MyTools.CrawTorrent
{
    partial class CrawTorrentPanel
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
            this.btn_downFile = new System.Windows.Forms.Button();
            this.btn_SaveFolder = new System.Windows.Forms.Button();
            this.btn_getInfo = new System.Windows.Forms.Button();
            this.dgv_main = new System.Windows.Forms.DataGridView();
            this.txt_threshold = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bar_down = new System.Windows.Forms.ProgressBar();
            this.btn_OpenFloder = new System.Windows.Forms.Button();
            this.IsDown = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Url = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PublishTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).BeginInit();
            this.SuspendLayout();
            // 
            // label_name
            // 
            this.label_name.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label_name.AutoSize = true;
            this.label_name.Location = new System.Drawing.Point(224, 35);
            this.label_name.Name = "label_name";
            this.label_name.Size = new System.Drawing.Size(53, 12);
            this.label_name.TabIndex = 0;
            this.label_name.Text = "动漫名：";
            // 
            // txt_name
            // 
            this.txt_name.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_name.Location = new System.Drawing.Point(283, 32);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(171, 21);
            this.txt_name.TabIndex = 1;
            this.txt_name.Text = "我的青春恋爱物语果然有问题";
            // 
            // label_path
            // 
            this.label_path.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label_path.AutoSize = true;
            this.label_path.Location = new System.Drawing.Point(237, 583);
            this.label_path.Name = "label_path";
            this.label_path.Size = new System.Drawing.Size(65, 12);
            this.label_path.TabIndex = 2;
            this.label_path.Text = "保存路径：";
            // 
            // txt_path
            // 
            this.txt_path.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txt_path.Location = new System.Drawing.Point(308, 580);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(274, 21);
            this.txt_path.TabIndex = 3;
            this.txt_path.Text = "F:\\普罗米修斯\\2013\\10月";
            // 
            // btn_downFile
            // 
            this.btn_downFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_downFile.Location = new System.Drawing.Point(340, 618);
            this.btn_downFile.Name = "btn_downFile";
            this.btn_downFile.Size = new System.Drawing.Size(75, 23);
            this.btn_downFile.TabIndex = 4;
            this.btn_downFile.Text = "下载";
            this.btn_downFile.UseVisualStyleBackColor = true;
            this.btn_downFile.Click += new System.EventHandler(this.btn_downFile_Click);
            // 
            // btn_SaveFolder
            // 
            this.btn_SaveFolder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_SaveFolder.Location = new System.Drawing.Point(588, 580);
            this.btn_SaveFolder.Name = "btn_SaveFolder";
            this.btn_SaveFolder.Size = new System.Drawing.Size(75, 23);
            this.btn_SaveFolder.TabIndex = 7;
            this.btn_SaveFolder.Text = "选择目录";
            this.btn_SaveFolder.UseVisualStyleBackColor = true;
            this.btn_SaveFolder.Click += new System.EventHandler(this.btn_SaveFolder_Click);
            // 
            // btn_getInfo
            // 
            this.btn_getInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_getInfo.Location = new System.Drawing.Point(586, 32);
            this.btn_getInfo.Name = "btn_getInfo";
            this.btn_getInfo.Size = new System.Drawing.Size(75, 23);
            this.btn_getInfo.TabIndex = 8;
            this.btn_getInfo.Text = "获取信息";
            this.btn_getInfo.UseVisualStyleBackColor = true;
            this.btn_getInfo.Click += new System.EventHandler(this.btn_getInfo_Click);
            // 
            // dgv_main
            // 
            this.dgv_main.AllowUserToAddRows = false;
            this.dgv_main.AllowUserToDeleteRows = false;
            this.dgv_main.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_main.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv_main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsDown,
            this.Url,
            this.PublishTime,
            this.Type,
            this.Title,
            this.Size});
            this.dgv_main.Location = new System.Drawing.Point(35, 74);
            this.dgv_main.Name = "dgv_main";
            this.dgv_main.RowTemplate.Height = 23;
            this.dgv_main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_main.Size = new System.Drawing.Size(813, 451);
            this.dgv_main.TabIndex = 9;
            this.dgv_main.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_main_CellMouseClick);
            // 
            // txt_threshold
            // 
            this.txt_threshold.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txt_threshold.Location = new System.Drawing.Point(531, 32);
            this.txt_threshold.Name = "txt_threshold";
            this.txt_threshold.Size = new System.Drawing.Size(49, 21);
            this.txt_threshold.TabIndex = 11;
            this.txt_threshold.Text = "500";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(460, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "筛选阈值：";
            // 
            // bar_down
            // 
            this.bar_down.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bar_down.Location = new System.Drawing.Point(35, 539);
            this.bar_down.Name = "bar_down";
            this.bar_down.Size = new System.Drawing.Size(813, 23);
            this.bar_down.TabIndex = 12;
            // 
            // btn_OpenFloder
            // 
            this.btn_OpenFloder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_OpenFloder.Enabled = false;
            this.btn_OpenFloder.Location = new System.Drawing.Point(437, 618);
            this.btn_OpenFloder.Name = "btn_OpenFloder";
            this.btn_OpenFloder.Size = new System.Drawing.Size(75, 23);
            this.btn_OpenFloder.TabIndex = 16;
            this.btn_OpenFloder.Text = "打开文件夹";
            this.btn_OpenFloder.UseVisualStyleBackColor = true;
            this.btn_OpenFloder.Click += new System.EventHandler(this.btn_OpenFloder_Click);
            // 
            // IsDown
            // 
            this.IsDown.DataPropertyName = "IsDown";
            this.IsDown.FalseValue = "false";
            this.IsDown.FillWeight = 10F;
            this.IsDown.HeaderText = "下载";
            this.IsDown.Name = "IsDown";
            this.IsDown.TrueValue = "true";
            // 
            // Url
            // 
            this.Url.DataPropertyName = "Url";
            this.Url.HeaderText = "链接";
            this.Url.Name = "Url";
            this.Url.ReadOnly = true;
            this.Url.Visible = false;
            // 
            // PublishTime
            // 
            this.PublishTime.DataPropertyName = "PublishTime";
            this.PublishTime.FillWeight = 15F;
            this.PublishTime.HeaderText = "发布时间";
            this.PublishTime.Name = "PublishTime";
            this.PublishTime.ReadOnly = true;
            this.PublishTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.FillWeight = 15F;
            this.Type.HeaderText = "分类";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            this.Title.FillWeight = 50F;
            this.Title.HeaderText = "标题";
            this.Title.Name = "Title";
            this.Title.ReadOnly = true;
            // 
            // Size
            // 
            this.Size.DataPropertyName = "Size";
            this.Size.FillWeight = 15F;
            this.Size.HeaderText = "大小（MB）";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            // 
            // CrawTorrentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.btn_OpenFloder);
            this.Controls.Add(this.bar_down);
            this.Controls.Add(this.txt_threshold);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_main);
            this.Controls.Add(this.btn_getInfo);
            this.Controls.Add(this.btn_SaveFolder);
            this.Controls.Add(this.btn_downFile);
            this.Controls.Add(this.txt_path);
            this.Controls.Add(this.label_path);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.label_name);
            this.Name = "CrawTorrentPanel";
            this.Text = "CrawTorrent";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_name;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label_path;
        private System.Windows.Forms.TextBox txt_path;
        private System.Windows.Forms.Button btn_downFile;
        private System.Windows.Forms.Button btn_SaveFolder;
        private System.Windows.Forms.Button btn_getInfo;
        private System.Windows.Forms.DataGridView dgv_main;
        private System.Windows.Forms.TextBox txt_threshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar bar_down;
        private System.Windows.Forms.Button btn_OpenFloder;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn Url;
        private System.Windows.Forms.DataGridViewTextBoxColumn PublishTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
    }
}