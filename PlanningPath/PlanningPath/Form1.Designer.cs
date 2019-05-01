namespace PlanningPath
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axMapMain = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.数据查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.空间查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清除查询结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClearMap = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbPointD = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbPointC = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPointB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbPointA = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbTurnArea3 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbTurnArea2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbTurnArea1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDrawLine = new System.Windows.Forms.Button();
            this.btnComputeWidth = new System.Windows.Forms.Button();
            this.tbOutLength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbWorkWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbMinTurnRadius = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbMachineType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectBlock = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axMapMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // axMapMain
            // 
            this.axMapMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.axMapMain.Location = new System.Drawing.Point(424, 61);
            this.axMapMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axMapMain.Name = "axMapMain";
            this.axMapMain.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapMain.OcxState")));
            this.axMapMain.Size = new System.Drawing.Size(746, 712);
            this.axMapMain.TabIndex = 0;
            this.axMapMain.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapMain_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(475, 34);
            this.axLicenseControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.数据查询ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1377, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(51, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(114, 26);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // 数据查询ToolStripMenuItem
            // 
            this.数据查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.空间查询ToolStripMenuItem,
            this.清除查询结果ToolStripMenuItem});
            this.数据查询ToolStripMenuItem.Name = "数据查询ToolStripMenuItem";
            this.数据查询ToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.数据查询ToolStripMenuItem.Text = "数据查询";
            // 
            // 空间查询ToolStripMenuItem
            // 
            this.空间查询ToolStripMenuItem.Name = "空间查询ToolStripMenuItem";
            this.空间查询ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.空间查询ToolStripMenuItem.Text = "属性查询";
            this.空间查询ToolStripMenuItem.Click += new System.EventHandler(this.属性查询ToolStripMenuItem_Click);
            // 
            // 清除查询结果ToolStripMenuItem
            // 
            this.清除查询结果ToolStripMenuItem.Name = "清除查询结果ToolStripMenuItem";
            this.清除查询结果ToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.清除查询结果ToolStripMenuItem.Text = "清除查询结果";
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(11, 28);
            this.axToolbarControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1263, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnClearMap);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnSelectBlock);
            this.groupBox1.Location = new System.Drawing.Point(15, 76);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(544, 712);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作菜单";
            // 
            // btnClearMap
            // 
            this.btnClearMap.Location = new System.Drawing.Point(275, 52);
            this.btnClearMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearMap.Name = "btnClearMap";
            this.btnClearMap.Size = new System.Drawing.Size(112, 35);
            this.btnClearMap.TabIndex = 4;
            this.btnClearMap.Text = "清除地图";
            this.btnClearMap.UseVisualStyleBackColor = true;
            this.btnClearMap.Click += new System.EventHandler(this.btnClearMap_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbPointD);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.tbPointC);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.tbPointB);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.tbPointA);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.tbTurnArea3);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.tbTurnArea2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.tbTurnArea1);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(8, 400);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(529, 224);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "信息面板";
            // 
            // tbPointD
            // 
            this.tbPointD.Location = new System.Drawing.Point(320, 160);
            this.tbPointD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPointD.Name = "tbPointD";
            this.tbPointD.ReadOnly = true;
            this.tbPointD.Size = new System.Drawing.Size(191, 25);
            this.tbPointD.TabIndex = 50;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(284, 165);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 15);
            this.label13.TabIndex = 49;
            this.label13.Text = "D：";
            // 
            // tbPointC
            // 
            this.tbPointC.Location = new System.Drawing.Point(51, 159);
            this.tbPointC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPointC.Name = "tbPointC";
            this.tbPointC.ReadOnly = true;
            this.tbPointC.Size = new System.Drawing.Size(191, 25);
            this.tbPointC.TabIndex = 48;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 164);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 15);
            this.label14.TabIndex = 47;
            this.label14.Text = "C：";
            // 
            // tbPointB
            // 
            this.tbPointB.Location = new System.Drawing.Point(320, 129);
            this.tbPointB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPointB.Name = "tbPointB";
            this.tbPointB.ReadOnly = true;
            this.tbPointB.Size = new System.Drawing.Size(191, 25);
            this.tbPointB.TabIndex = 46;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(284, 134);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(30, 15);
            this.label12.TabIndex = 45;
            this.label12.Text = "B：";
            // 
            // tbPointA
            // 
            this.tbPointA.Location = new System.Drawing.Point(51, 128);
            this.tbPointA.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPointA.Name = "tbPointA";
            this.tbPointA.ReadOnly = true;
            this.tbPointA.Size = new System.Drawing.Size(191, 25);
            this.tbPointA.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 15);
            this.label11.TabIndex = 43;
            this.label11.Text = "A：";
            // 
            // tbTurnArea3
            // 
            this.tbTurnArea3.Location = new System.Drawing.Point(140, 82);
            this.tbTurnArea3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTurnArea3.Name = "tbTurnArea3";
            this.tbTurnArea3.ReadOnly = true;
            this.tbTurnArea3.Size = new System.Drawing.Size(101, 25);
            this.tbTurnArea3.TabIndex = 40;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 15);
            this.label9.TabIndex = 39;
            this.label9.Text = "地头转弯区3：";
            // 
            // tbTurnArea2
            // 
            this.tbTurnArea2.Location = new System.Drawing.Point(409, 46);
            this.tbTurnArea2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTurnArea2.Name = "tbTurnArea2";
            this.tbTurnArea2.ReadOnly = true;
            this.tbTurnArea2.Size = new System.Drawing.Size(101, 25);
            this.tbTurnArea2.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(284, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 15);
            this.label7.TabIndex = 37;
            this.label7.Text = "地头转弯区2：";
            // 
            // tbTurnArea1
            // 
            this.tbTurnArea1.Location = new System.Drawing.Point(140, 46);
            this.tbTurnArea1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTurnArea1.Name = "tbTurnArea1";
            this.tbTurnArea1.ReadOnly = true;
            this.tbTurnArea1.Size = new System.Drawing.Size(101, 25);
            this.tbTurnArea1.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 15);
            this.label8.TabIndex = 35;
            this.label8.Text = "地头转弯区1：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDrawLine);
            this.groupBox2.Controls.Add(this.btnComputeWidth);
            this.groupBox2.Controls.Add(this.tbOutLength);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbWorkWidth);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbMinTurnRadius);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cbMachineType);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(7, 141);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(531, 196);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "路径计算操作";
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.Location = new System.Drawing.Point(264, 140);
            this.btnDrawLine.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(112, 35);
            this.btnDrawLine.TabIndex = 10;
            this.btnDrawLine.Text = "画线";
            this.btnDrawLine.UseVisualStyleBackColor = true;
            this.btnDrawLine.Click += new System.EventHandler(this.btnDrawLine_Click);
            // 
            // btnComputeWidth
            // 
            this.btnComputeWidth.Location = new System.Drawing.Point(111, 140);
            this.btnComputeWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnComputeWidth.Name = "btnComputeWidth";
            this.btnComputeWidth.Size = new System.Drawing.Size(112, 35);
            this.btnComputeWidth.TabIndex = 9;
            this.btnComputeWidth.Text = "计算宽度";
            this.btnComputeWidth.UseVisualStyleBackColor = true;
            this.btnComputeWidth.Click += new System.EventHandler(this.btnComputeWidth_Click);
            // 
            // tbOutLength
            // 
            this.tbOutLength.Location = new System.Drawing.Point(409, 76);
            this.tbOutLength.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbOutLength.Name = "tbOutLength";
            this.tbOutLength.Size = new System.Drawing.Size(97, 25);
            this.tbOutLength.TabIndex = 8;
            this.tbOutLength.Text = "1.95";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "机组出线长度：";
            // 
            // tbWorkWidth
            // 
            this.tbWorkWidth.Location = new System.Drawing.Point(144, 76);
            this.tbWorkWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbWorkWidth.Name = "tbWorkWidth";
            this.tbWorkWidth.Size = new System.Drawing.Size(91, 25);
            this.tbWorkWidth.TabIndex = 6;
            this.tbWorkWidth.Text = "3.6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "农机作业幅宽：";
            // 
            // tbMinTurnRadius
            // 
            this.tbMinTurnRadius.Location = new System.Drawing.Point(409, 29);
            this.tbMinTurnRadius.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMinTurnRadius.Name = "tbMinTurnRadius";
            this.tbMinTurnRadius.Size = new System.Drawing.Size(97, 25);
            this.tbMinTurnRadius.TabIndex = 4;
            this.tbMinTurnRadius.Text = "4.65";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "农机最小转弯半径：";
            // 
            // cbMachineType
            // 
            this.cbMachineType.FormattingEnabled = true;
            this.cbMachineType.Items.AddRange(new object[] {
            "悬挂式",
            "牵引式"});
            this.cbMachineType.Location = new System.Drawing.Point(112, 30);
            this.cbMachineType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbMachineType.Name = "cbMachineType";
            this.cbMachineType.Size = new System.Drawing.Size(97, 23);
            this.cbMachineType.TabIndex = 2;
            this.cbMachineType.Text = "悬挂式";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "农机种类：";
            // 
            // btnSelectBlock
            // 
            this.btnSelectBlock.Location = new System.Drawing.Point(105, 52);
            this.btnSelectBlock.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelectBlock.Name = "btnSelectBlock";
            this.btnSelectBlock.Size = new System.Drawing.Size(112, 35);
            this.btnSelectBlock.TabIndex = 0;
            this.btnSelectBlock.Text = "提取地块";
            this.btnSelectBlock.UseVisualStyleBackColor = true;
            this.btnSelectBlock.Click += new System.EventHandler(this.btnSelectBlock_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1377, 804);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axMapMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapMain;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDrawLine;
        private System.Windows.Forms.Button btnComputeWidth;
        private System.Windows.Forms.TextBox tbOutLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbWorkWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbMinTurnRadius;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbMachineType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectBlock;
        private System.Windows.Forms.ToolStripMenuItem 数据查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 空间查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除查询结果ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbTurnArea3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTurnArea2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbTurnArea1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPointD;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbPointC;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbPointB;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbPointA;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnClearMap;
    }
}

