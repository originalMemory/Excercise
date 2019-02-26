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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbPointD = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbPointC = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbPointB = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbPointA = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbAngleAlpha = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbTurnArea3 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbTurnArea2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbTurnArea1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbEdgeLength = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbAngle4 = new System.Windows.Forms.TextBox();
            this.labelAngle4 = new System.Windows.Forms.Label();
            this.tbAngle3 = new System.Windows.Forms.TextBox();
            this.labelAngle3 = new System.Windows.Forms.Label();
            this.tbAngle2 = new System.Windows.Forms.TextBox();
            this.labelAngle2 = new System.Windows.Forms.Label();
            this.tbAngle1 = new System.Windows.Forms.TextBox();
            this.labelAngle1 = new System.Windows.Forms.Label();
            this.tbMaxEdgeLength = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.btnSelectAttribute = new System.Windows.Forms.Button();
            this.btnSelectBlock = new System.Windows.Forms.Button();
            this.btnClearMap = new System.Windows.Forms.Button();
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
            this.axMapMain.Name = "axMapMain";
            this.axMapMain.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapMain.OcxState")));
            this.axMapMain.Size = new System.Drawing.Size(597, 570);
            this.axMapMain.TabIndex = 0;
            this.axMapMain.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapMain_OnMouseDown);
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(475, 34);
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
            this.menuStrip1.Size = new System.Drawing.Size(1033, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.退出ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            // 
            // 数据查询ToolStripMenuItem
            // 
            this.数据查询ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.空间查询ToolStripMenuItem,
            this.清除查询结果ToolStripMenuItem});
            this.数据查询ToolStripMenuItem.Name = "数据查询ToolStripMenuItem";
            this.数据查询ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.数据查询ToolStripMenuItem.Text = "数据查询";
            // 
            // 空间查询ToolStripMenuItem
            // 
            this.空间查询ToolStripMenuItem.Name = "空间查询ToolStripMenuItem";
            this.空间查询ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.空间查询ToolStripMenuItem.Text = "空间查询";
            this.空间查询ToolStripMenuItem.Click += new System.EventHandler(this.空间查询ToolStripMenuItem_Click);
            // 
            // 清除查询结果ToolStripMenuItem
            // 
            this.清除查询结果ToolStripMenuItem.Name = "清除查询结果ToolStripMenuItem";
            this.清除查询结果ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.清除查询结果ToolStripMenuItem.Text = "清除查询结果";
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Location = new System.Drawing.Point(11, 28);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1010, 28);
            this.axToolbarControl1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnClearMap);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.btnSelectAttribute);
            this.groupBox1.Controls.Add(this.btnSelectBlock);
            this.groupBox1.Location = new System.Drawing.Point(11, 61);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(408, 570);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "操作菜单";
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
            this.groupBox3.Controls.Add(this.tbAngleAlpha);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.tbTurnArea3);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.tbTurnArea2);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.tbTurnArea1);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.tbEdgeLength);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.tbAngle4);
            this.groupBox3.Controls.Add(this.labelAngle4);
            this.groupBox3.Controls.Add(this.tbAngle3);
            this.groupBox3.Controls.Add(this.labelAngle3);
            this.groupBox3.Controls.Add(this.tbAngle2);
            this.groupBox3.Controls.Add(this.labelAngle2);
            this.groupBox3.Controls.Add(this.tbAngle1);
            this.groupBox3.Controls.Add(this.labelAngle1);
            this.groupBox3.Controls.Add(this.tbMaxEdgeLength);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Location = new System.Drawing.Point(6, 275);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(397, 290);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "信息面板";
            // 
            // tbPointD
            // 
            this.tbPointD.Location = new System.Drawing.Point(242, 243);
            this.tbPointD.Margin = new System.Windows.Forms.Padding(2);
            this.tbPointD.Name = "tbPointD";
            this.tbPointD.ReadOnly = true;
            this.tbPointD.Size = new System.Drawing.Size(144, 21);
            this.tbPointD.TabIndex = 50;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(215, 247);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 12);
            this.label13.TabIndex = 49;
            this.label13.Text = "D：";
            // 
            // tbPointC
            // 
            this.tbPointC.Location = new System.Drawing.Point(40, 242);
            this.tbPointC.Margin = new System.Windows.Forms.Padding(2);
            this.tbPointC.Name = "tbPointC";
            this.tbPointC.ReadOnly = true;
            this.tbPointC.Size = new System.Drawing.Size(144, 21);
            this.tbPointC.TabIndex = 48;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(13, 246);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(23, 12);
            this.label14.TabIndex = 47;
            this.label14.Text = "C：";
            // 
            // tbPointB
            // 
            this.tbPointB.Location = new System.Drawing.Point(242, 218);
            this.tbPointB.Margin = new System.Windows.Forms.Padding(2);
            this.tbPointB.Name = "tbPointB";
            this.tbPointB.ReadOnly = true;
            this.tbPointB.Size = new System.Drawing.Size(144, 21);
            this.tbPointB.TabIndex = 46;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(215, 222);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 45;
            this.label12.Text = "B：";
            // 
            // tbPointA
            // 
            this.tbPointA.Location = new System.Drawing.Point(40, 217);
            this.tbPointA.Margin = new System.Windows.Forms.Padding(2);
            this.tbPointA.Name = "tbPointA";
            this.tbPointA.ReadOnly = true;
            this.tbPointA.Size = new System.Drawing.Size(144, 21);
            this.tbPointA.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 221);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 12);
            this.label11.TabIndex = 43;
            this.label11.Text = "A：";
            // 
            // tbAngleAlpha
            // 
            this.tbAngleAlpha.Location = new System.Drawing.Point(326, 52);
            this.tbAngleAlpha.Margin = new System.Windows.Forms.Padding(2);
            this.tbAngleAlpha.Name = "tbAngleAlpha";
            this.tbAngleAlpha.ReadOnly = true;
            this.tbAngleAlpha.Size = new System.Drawing.Size(60, 21);
            this.tbAngleAlpha.TabIndex = 42;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(215, 56);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 41;
            this.label10.Text = "alpha角：";
            // 
            // tbTurnArea3
            // 
            this.tbTurnArea3.Location = new System.Drawing.Point(107, 181);
            this.tbTurnArea3.Margin = new System.Windows.Forms.Padding(2);
            this.tbTurnArea3.Name = "tbTurnArea3";
            this.tbTurnArea3.ReadOnly = true;
            this.tbTurnArea3.Size = new System.Drawing.Size(77, 21);
            this.tbTurnArea3.TabIndex = 40;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 185);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 39;
            this.label9.Text = "地头转弯区3：";
            // 
            // tbTurnArea2
            // 
            this.tbTurnArea2.Location = new System.Drawing.Point(309, 152);
            this.tbTurnArea2.Margin = new System.Windows.Forms.Padding(2);
            this.tbTurnArea2.Name = "tbTurnArea2";
            this.tbTurnArea2.ReadOnly = true;
            this.tbTurnArea2.Size = new System.Drawing.Size(77, 21);
            this.tbTurnArea2.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(215, 156);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 37;
            this.label7.Text = "地头转弯区2：";
            // 
            // tbTurnArea1
            // 
            this.tbTurnArea1.Location = new System.Drawing.Point(107, 152);
            this.tbTurnArea1.Margin = new System.Windows.Forms.Padding(2);
            this.tbTurnArea1.Name = "tbTurnArea1";
            this.tbTurnArea1.ReadOnly = true;
            this.tbTurnArea1.Size = new System.Drawing.Size(77, 21);
            this.tbTurnArea1.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 156);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 35;
            this.label8.Text = "地头转弯区1：";
            // 
            // tbEdgeLength
            // 
            this.tbEdgeLength.Location = new System.Drawing.Point(94, 19);
            this.tbEdgeLength.Margin = new System.Windows.Forms.Padding(2);
            this.tbEdgeLength.Name = "tbEdgeLength";
            this.tbEdgeLength.ReadOnly = true;
            this.tbEdgeLength.Size = new System.Drawing.Size(292, 21);
            this.tbEdgeLength.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 23);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 33;
            this.label6.Text = "边长(逆时针)：";
            // 
            // tbAngle4
            // 
            this.tbAngle4.Location = new System.Drawing.Point(326, 115);
            this.tbAngle4.Margin = new System.Windows.Forms.Padding(2);
            this.tbAngle4.Name = "tbAngle4";
            this.tbAngle4.ReadOnly = true;
            this.tbAngle4.Size = new System.Drawing.Size(60, 21);
            this.tbAngle4.TabIndex = 32;
            // 
            // labelAngle4
            // 
            this.labelAngle4.AutoSize = true;
            this.labelAngle4.Location = new System.Drawing.Point(215, 119);
            this.labelAngle4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAngle4.Name = "labelAngle4";
            this.labelAngle4.Size = new System.Drawing.Size(53, 12);
            this.labelAngle4.TabIndex = 31;
            this.labelAngle4.Text = "角度四：";
            // 
            // tbAngle3
            // 
            this.tbAngle3.Location = new System.Drawing.Point(124, 114);
            this.tbAngle3.Margin = new System.Windows.Forms.Padding(2);
            this.tbAngle3.Name = "tbAngle3";
            this.tbAngle3.ReadOnly = true;
            this.tbAngle3.Size = new System.Drawing.Size(60, 21);
            this.tbAngle3.TabIndex = 30;
            // 
            // labelAngle3
            // 
            this.labelAngle3.AutoSize = true;
            this.labelAngle3.Location = new System.Drawing.Point(13, 118);
            this.labelAngle3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAngle3.Name = "labelAngle3";
            this.labelAngle3.Size = new System.Drawing.Size(53, 12);
            this.labelAngle3.TabIndex = 29;
            this.labelAngle3.Text = "角度三：";
            // 
            // tbAngle2
            // 
            this.tbAngle2.Location = new System.Drawing.Point(326, 84);
            this.tbAngle2.Margin = new System.Windows.Forms.Padding(2);
            this.tbAngle2.Name = "tbAngle2";
            this.tbAngle2.ReadOnly = true;
            this.tbAngle2.Size = new System.Drawing.Size(60, 21);
            this.tbAngle2.TabIndex = 28;
            // 
            // labelAngle2
            // 
            this.labelAngle2.AutoSize = true;
            this.labelAngle2.Location = new System.Drawing.Point(215, 88);
            this.labelAngle2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAngle2.Name = "labelAngle2";
            this.labelAngle2.Size = new System.Drawing.Size(53, 12);
            this.labelAngle2.TabIndex = 27;
            this.labelAngle2.Text = "角度二：";
            // 
            // tbAngle1
            // 
            this.tbAngle1.Location = new System.Drawing.Point(124, 82);
            this.tbAngle1.Margin = new System.Windows.Forms.Padding(2);
            this.tbAngle1.Name = "tbAngle1";
            this.tbAngle1.ReadOnly = true;
            this.tbAngle1.Size = new System.Drawing.Size(60, 21);
            this.tbAngle1.TabIndex = 26;
            // 
            // labelAngle1
            // 
            this.labelAngle1.AutoSize = true;
            this.labelAngle1.Location = new System.Drawing.Point(13, 86);
            this.labelAngle1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAngle1.Name = "labelAngle1";
            this.labelAngle1.Size = new System.Drawing.Size(53, 12);
            this.labelAngle1.TabIndex = 25;
            this.labelAngle1.Text = "角度一：";
            // 
            // tbMaxEdgeLength
            // 
            this.tbMaxEdgeLength.Location = new System.Drawing.Point(124, 51);
            this.tbMaxEdgeLength.Margin = new System.Windows.Forms.Padding(2);
            this.tbMaxEdgeLength.Name = "tbMaxEdgeLength";
            this.tbMaxEdgeLength.ReadOnly = true;
            this.tbMaxEdgeLength.Size = new System.Drawing.Size(60, 21);
            this.tbMaxEdgeLength.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 55);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "最长边长度：";
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
            this.groupBox2.Location = new System.Drawing.Point(5, 113);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(398, 157);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "路径计算操作";
            // 
            // btnDrawLine
            // 
            this.btnDrawLine.Location = new System.Drawing.Point(198, 112);
            this.btnDrawLine.Margin = new System.Windows.Forms.Padding(2);
            this.btnDrawLine.Name = "btnDrawLine";
            this.btnDrawLine.Size = new System.Drawing.Size(84, 28);
            this.btnDrawLine.TabIndex = 10;
            this.btnDrawLine.Text = "画线";
            this.btnDrawLine.UseVisualStyleBackColor = true;
            // 
            // btnComputeWidth
            // 
            this.btnComputeWidth.Location = new System.Drawing.Point(83, 112);
            this.btnComputeWidth.Margin = new System.Windows.Forms.Padding(2);
            this.btnComputeWidth.Name = "btnComputeWidth";
            this.btnComputeWidth.Size = new System.Drawing.Size(84, 28);
            this.btnComputeWidth.TabIndex = 9;
            this.btnComputeWidth.Text = "计算宽度";
            this.btnComputeWidth.UseVisualStyleBackColor = true;
            this.btnComputeWidth.Click += new System.EventHandler(this.btnComputeWidth_Click);
            // 
            // tbOutLength
            // 
            this.tbOutLength.Location = new System.Drawing.Point(307, 61);
            this.tbOutLength.Margin = new System.Windows.Forms.Padding(2);
            this.tbOutLength.Name = "tbOutLength";
            this.tbOutLength.Size = new System.Drawing.Size(74, 21);
            this.tbOutLength.TabIndex = 8;
            this.tbOutLength.Text = "1.95";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 64);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "机组出线长度：";
            // 
            // tbWorkWidth
            // 
            this.tbWorkWidth.Location = new System.Drawing.Point(108, 61);
            this.tbWorkWidth.Margin = new System.Windows.Forms.Padding(2);
            this.tbWorkWidth.Name = "tbWorkWidth";
            this.tbWorkWidth.Size = new System.Drawing.Size(69, 21);
            this.tbWorkWidth.TabIndex = 6;
            this.tbWorkWidth.Text = "3.6";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 64);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "农机作业幅宽：";
            // 
            // tbMinTurnRadius
            // 
            this.tbMinTurnRadius.Location = new System.Drawing.Point(307, 23);
            this.tbMinTurnRadius.Margin = new System.Windows.Forms.Padding(2);
            this.tbMinTurnRadius.Name = "tbMinTurnRadius";
            this.tbMinTurnRadius.Size = new System.Drawing.Size(74, 21);
            this.tbMinTurnRadius.TabIndex = 4;
            this.tbMinTurnRadius.Text = "4.65";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "农机最小转弯半径：";
            // 
            // cbMachineType
            // 
            this.cbMachineType.FormattingEnabled = true;
            this.cbMachineType.Items.AddRange(new object[] {
            "悬挂式",
            "牵引式"});
            this.cbMachineType.Location = new System.Drawing.Point(84, 24);
            this.cbMachineType.Margin = new System.Windows.Forms.Padding(2);
            this.cbMachineType.Name = "cbMachineType";
            this.cbMachineType.Size = new System.Drawing.Size(74, 20);
            this.cbMachineType.TabIndex = 2;
            this.cbMachineType.Text = "悬挂式";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "农机种类：";
            // 
            // btnSelectAttribute
            // 
            this.btnSelectAttribute.Location = new System.Drawing.Point(160, 42);
            this.btnSelectAttribute.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectAttribute.Name = "btnSelectAttribute";
            this.btnSelectAttribute.Size = new System.Drawing.Size(84, 28);
            this.btnSelectAttribute.TabIndex = 1;
            this.btnSelectAttribute.Text = "查询属性";
            this.btnSelectAttribute.UseVisualStyleBackColor = true;
            this.btnSelectAttribute.Click += new System.EventHandler(this.btnSelectAttribute_Click);
            // 
            // btnSelectBlock
            // 
            this.btnSelectBlock.Location = new System.Drawing.Point(35, 42);
            this.btnSelectBlock.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectBlock.Name = "btnSelectBlock";
            this.btnSelectBlock.Size = new System.Drawing.Size(84, 28);
            this.btnSelectBlock.TabIndex = 0;
            this.btnSelectBlock.Text = "提取地块";
            this.btnSelectBlock.UseVisualStyleBackColor = true;
            this.btnSelectBlock.Click += new System.EventHandler(this.btnSelectBlock_Click);
            // 
            // btnClearMap
            // 
            this.btnClearMap.Location = new System.Drawing.Point(289, 42);
            this.btnClearMap.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearMap.Name = "btnClearMap";
            this.btnClearMap.Size = new System.Drawing.Size(84, 28);
            this.btnClearMap.TabIndex = 4;
            this.btnClearMap.Text = "清除地图";
            this.btnClearMap.UseVisualStyleBackColor = true;
            this.btnClearMap.Click += new System.EventHandler(this.btnClearMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 643);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axMapMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
        private System.Windows.Forms.Button btnSelectAttribute;
        private System.Windows.Forms.Button btnSelectBlock;
        private System.Windows.Forms.ToolStripMenuItem 数据查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 空间查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清除查询结果ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbEdgeLength;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbAngle4;
        private System.Windows.Forms.Label labelAngle4;
        private System.Windows.Forms.TextBox tbAngle3;
        private System.Windows.Forms.Label labelAngle3;
        private System.Windows.Forms.TextBox tbAngle2;
        private System.Windows.Forms.Label labelAngle2;
        private System.Windows.Forms.TextBox tbAngle1;
        private System.Windows.Forms.Label labelAngle1;
        private System.Windows.Forms.TextBox tbMaxEdgeLength;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTurnArea3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTurnArea2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbTurnArea1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbAngleAlpha;
        private System.Windows.Forms.Label label10;
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

