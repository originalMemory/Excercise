namespace VipManager.FormControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabVip = new System.Windows.Forms.TabPage();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.dtpRegAt = new System.Windows.Forms.DateTimePicker();
            this.labEnd = new CCWin.SkinControl.SkinLabel();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.dgvVip = new CCWin.SkinControl.SkinDataGridView();
            this.btnAddVip = new CCWin.SkinControl.SkinButton();
            this.tabPro = new System.Windows.Forms.TabPage();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RemainNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DelAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsDel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastPayAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PayNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labStart = new CCWin.SkinControl.SkinLabel();
            this.skinTextBox1 = new CCWin.SkinControl.SkinTextBox();
            this.txtBalance = new CCWin.SkinControl.SkinTextBox();
            this.txtDetail = new CCWin.SkinControl.SkinTextBox();
            this.cbType = new CCWin.SkinControl.SkinComboBox();
            this.cbGender = new CCWin.SkinControl.SkinComboBox();
            this.txtPhone = new CCWin.SkinControl.SkinTextBox();
            this.txtName = new CCWin.SkinControl.SkinTextBox();
            this.txtNo = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.labYuan = new CCWin.SkinControl.SkinLabel();
            this.labBalance = new CCWin.SkinControl.SkinLabel();
            this.labDiscount = new CCWin.SkinControl.SkinLabel();
            this.labDetail = new CCWin.SkinControl.SkinLabel();
            this.skinLabel12 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel13 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel14 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel15 = new CCWin.SkinControl.SkinLabel();
            this.tabMain.SuspendLayout();
            this.tabVip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVip)).BeginInit();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabVip);
            this.tabMain.Controls.Add(this.tabPro);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Font = new System.Drawing.Font("宋体", 13F);
            this.tabMain.Location = new System.Drawing.Point(8, 39);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1135, 692);
            this.tabMain.TabIndex = 0;
            // 
            // tabVip
            // 
            this.tabVip.Controls.Add(this.txtBalance);
            this.tabVip.Controls.Add(this.txtDetail);
            this.tabVip.Controls.Add(this.cbType);
            this.tabVip.Controls.Add(this.cbGender);
            this.tabVip.Controls.Add(this.txtPhone);
            this.tabVip.Controls.Add(this.txtName);
            this.tabVip.Controls.Add(this.txtNo);
            this.tabVip.Controls.Add(this.skinLabel5);
            this.tabVip.Controls.Add(this.labYuan);
            this.tabVip.Controls.Add(this.labBalance);
            this.tabVip.Controls.Add(this.labDiscount);
            this.tabVip.Controls.Add(this.labDetail);
            this.tabVip.Controls.Add(this.skinLabel12);
            this.tabVip.Controls.Add(this.skinLabel13);
            this.tabVip.Controls.Add(this.skinLabel14);
            this.tabVip.Controls.Add(this.skinLabel15);
            this.tabVip.Controls.Add(this.skinTextBox1);
            this.tabVip.Controls.Add(this.skinLabel10);
            this.tabVip.Controls.Add(this.dtpRegAt);
            this.tabVip.Controls.Add(this.labEnd);
            this.tabVip.Controls.Add(this.labStart);
            this.tabVip.Controls.Add(this.dtpEnd);
            this.tabVip.Controls.Add(this.dtpStart);
            this.tabVip.Controls.Add(this.dgvVip);
            this.tabVip.Controls.Add(this.btnAddVip);
            this.tabVip.Location = new System.Drawing.Point(4, 27);
            this.tabVip.Name = "tabVip";
            this.tabVip.Padding = new System.Windows.Forms.Padding(3);
            this.tabVip.Size = new System.Drawing.Size(1127, 661);
            this.tabVip.TabIndex = 0;
            this.tabVip.Text = "会员管理";
            this.tabVip.UseVisualStyleBackColor = true;
            // 
            // skinLabel10
            // 
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel10.Location = new System.Drawing.Point(276, 114);
            this.skinLabel10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(90, 21);
            this.skinLabel10.TabIndex = 47;
            this.skinLabel10.Text = "注册时间：";
            // 
            // dtpRegAt
            // 
            this.dtpRegAt.CalendarFont = new System.Drawing.Font("宋体", 11F);
            this.dtpRegAt.Enabled = false;
            this.dtpRegAt.Font = new System.Drawing.Font("宋体", 11F);
            this.dtpRegAt.Location = new System.Drawing.Point(369, 113);
            this.dtpRegAt.Margin = new System.Windows.Forms.Padding(2);
            this.dtpRegAt.Name = "dtpRegAt";
            this.dtpRegAt.Size = new System.Drawing.Size(135, 24);
            this.dtpRegAt.TabIndex = 46;
            // 
            // labEnd
            // 
            this.labEnd.AutoSize = true;
            this.labEnd.BackColor = System.Drawing.Color.Transparent;
            this.labEnd.BorderColor = System.Drawing.Color.White;
            this.labEnd.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labEnd.Location = new System.Drawing.Point(559, 110);
            this.labEnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labEnd.Name = "labEnd";
            this.labEnd.Size = new System.Drawing.Size(90, 21);
            this.labEnd.TabIndex = 39;
            this.labEnd.Text = "结束时间：";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Font = new System.Drawing.Font("宋体", 11F);
            this.dtpEnd.Location = new System.Drawing.Point(651, 110);
            this.dtpEnd.Margin = new System.Windows.Forms.Padding(2);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(135, 24);
            this.dtpEnd.TabIndex = 37;
            this.dtpEnd.Visible = false;
            // 
            // dtpStart
            // 
            this.dtpStart.CalendarFont = new System.Drawing.Font("宋体", 11F);
            this.dtpStart.Font = new System.Drawing.Font("宋体", 11F);
            this.dtpStart.Location = new System.Drawing.Point(650, 75);
            this.dtpStart.Margin = new System.Windows.Forms.Padding(2);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(135, 24);
            this.dtpStart.TabIndex = 36;
            this.dtpStart.Visible = false;
            // 
            // dgvVip
            // 
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(246)))), ((int)(((byte)(253)))));
            this.dgvVip.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvVip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVip.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvVip.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvVip.ColumnFont = null;
            this.dgvVip.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(246)))), ((int)(((byte)(239)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvVip.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvVip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVip.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.No,
            this.VipName,
            this.Phone,
            this.Type,
            this.Balance,
            this.RemainNum,
            this.Discount,
            this.CreateAt,
            this.DelAt,
            this.IsDel,
            this.Note,
            this.LastPayAt,
            this.Gender,
            this.StartAt,
            this.EndAt,
            this.PayNum,
            this.UserId});
            this.dgvVip.ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(188)))), ((int)(((byte)(240)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvVip.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvVip.EnableHeadersVisualStyles = false;
            this.dgvVip.Font = new System.Drawing.Font("宋体", 11F);
            this.dgvVip.GridColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.dgvVip.HeadFont = new System.Drawing.Font("宋体", 11F);
            this.dgvVip.HeadSelectForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvVip.Location = new System.Drawing.Point(6, 229);
            this.dgvVip.Name = "dgvVip";
            this.dgvVip.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvVip.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvVip.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvVip.RowTemplate.Height = 23;
            this.dgvVip.Size = new System.Drawing.Size(1118, 353);
            this.dgvVip.TabIndex = 1;
            this.dgvVip.TitleBack = null;
            this.dgvVip.TitleBackColorBegin = System.Drawing.Color.White;
            this.dgvVip.TitleBackColorEnd = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(196)))), ((int)(((byte)(242)))));
            // 
            // btnAddVip
            // 
            this.btnAddVip.BackColor = System.Drawing.Color.Transparent;
            this.btnAddVip.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnAddVip.DownBack = null;
            this.btnAddVip.Font = new System.Drawing.Font("宋体", 11F);
            this.btnAddVip.Location = new System.Drawing.Point(40, 17);
            this.btnAddVip.MouseBack = null;
            this.btnAddVip.Name = "btnAddVip";
            this.btnAddVip.NormlBack = null;
            this.btnAddVip.Size = new System.Drawing.Size(96, 28);
            this.btnAddVip.TabIndex = 0;
            this.btnAddVip.Text = "添加会员";
            this.btnAddVip.UseVisualStyleBackColor = false;
            this.btnAddVip.Click += new System.EventHandler(this.btnAddVip_Click);
            // 
            // tabPro
            // 
            this.tabPro.Location = new System.Drawing.Point(4, 27);
            this.tabPro.Name = "tabPro";
            this.tabPro.Padding = new System.Windows.Forms.Padding(3);
            this.tabPro.Size = new System.Drawing.Size(1127, 661);
            this.tabPro.TabIndex = 1;
            this.tabPro.Text = "产品管理";
            this.tabPro.UseVisualStyleBackColor = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // No
            // 
            this.No.DataPropertyName = "No";
            this.No.HeaderText = "编号";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            // 
            // VipName
            // 
            this.VipName.DataPropertyName = "VipName";
            this.VipName.HeaderText = "姓名";
            this.VipName.Name = "VipName";
            this.VipName.ReadOnly = true;
            // 
            // Phone
            // 
            this.Phone.DataPropertyName = "Phone";
            this.Phone.HeaderText = "联系方式";
            this.Phone.Name = "Phone";
            this.Phone.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "会员类型";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Balance
            // 
            this.Balance.DataPropertyName = "Balance";
            this.Balance.HeaderText = "余额";
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            // 
            // RemainNum
            // 
            this.RemainNum.DataPropertyName = "RemainNum";
            this.RemainNum.HeaderText = "剩余次数";
            this.RemainNum.Name = "RemainNum";
            this.RemainNum.ReadOnly = true;
            // 
            // Discount
            // 
            this.Discount.DataPropertyName = "Discount";
            this.Discount.HeaderText = "折扣";
            this.Discount.Name = "Discount";
            this.Discount.ReadOnly = true;
            // 
            // CreateAt
            // 
            this.CreateAt.DataPropertyName = "CreateAt";
            this.CreateAt.HeaderText = "加入时间";
            this.CreateAt.Name = "CreateAt";
            this.CreateAt.ReadOnly = true;
            // 
            // DelAt
            // 
            this.DelAt.DataPropertyName = "DelAt";
            this.DelAt.HeaderText = "删除时间";
            this.DelAt.Name = "DelAt";
            this.DelAt.ReadOnly = true;
            this.DelAt.Visible = false;
            // 
            // IsDel
            // 
            this.IsDel.DataPropertyName = "IsDel";
            this.IsDel.HeaderText = "删除";
            this.IsDel.Name = "IsDel";
            this.IsDel.ReadOnly = true;
            this.IsDel.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.IsDel.Visible = false;
            // 
            // Note
            // 
            this.Note.DataPropertyName = "Note";
            this.Note.HeaderText = "备注";
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            // 
            // LastPayAt
            // 
            this.LastPayAt.DataPropertyName = "LastPayAt";
            this.LastPayAt.HeaderText = "最后支付时间";
            this.LastPayAt.Name = "LastPayAt";
            this.LastPayAt.ReadOnly = true;
            // 
            // Gender
            // 
            this.Gender.DataPropertyName = "Gender";
            this.Gender.HeaderText = "性别";
            this.Gender.Name = "Gender";
            this.Gender.ReadOnly = true;
            // 
            // StartAt
            // 
            this.StartAt.DataPropertyName = "StartAt";
            this.StartAt.HeaderText = "开始时间";
            this.StartAt.Name = "StartAt";
            this.StartAt.ReadOnly = true;
            // 
            // EndAt
            // 
            this.EndAt.DataPropertyName = "EndAt";
            this.EndAt.HeaderText = "结束时间";
            this.EndAt.Name = "EndAt";
            this.EndAt.ReadOnly = true;
            // 
            // PayNum
            // 
            this.PayNum.DataPropertyName = "PayNum";
            this.PayNum.HeaderText = "消费次数";
            this.PayNum.Name = "PayNum";
            this.PayNum.ReadOnly = true;
            // 
            // UserId
            // 
            this.UserId.DataPropertyName = "UserId";
            this.UserId.HeaderText = "用户Id";
            this.UserId.Name = "UserId";
            this.UserId.ReadOnly = true;
            this.UserId.Visible = false;
            // 
            // labStart
            // 
            this.labStart.AutoSize = true;
            this.labStart.BackColor = System.Drawing.Color.Transparent;
            this.labStart.BorderColor = System.Drawing.Color.White;
            this.labStart.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labStart.Location = new System.Drawing.Point(559, 76);
            this.labStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labStart.Name = "labStart";
            this.labStart.Size = new System.Drawing.Size(90, 21);
            this.labStart.TabIndex = 38;
            this.labStart.Text = "开始时间：";
            // 
            // skinTextBox1
            // 
            this.skinTextBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinTextBox1.DownBack = null;
            this.skinTextBox1.Icon = null;
            this.skinTextBox1.IconIsButton = false;
            this.skinTextBox1.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.IsPasswordChat = '\0';
            this.skinTextBox1.IsSystemPasswordChar = false;
            this.skinTextBox1.Lines = new string[] {
        "skinTextBox1"};
            this.skinTextBox1.Location = new System.Drawing.Point(147, 17);
            this.skinTextBox1.Margin = new System.Windows.Forms.Padding(0);
            this.skinTextBox1.MaxLength = 32767;
            this.skinTextBox1.MinimumSize = new System.Drawing.Size(28, 28);
            this.skinTextBox1.MouseBack = null;
            this.skinTextBox1.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.skinTextBox1.Multiline = false;
            this.skinTextBox1.Name = "skinTextBox1";
            this.skinTextBox1.NormlBack = null;
            this.skinTextBox1.Padding = new System.Windows.Forms.Padding(5);
            this.skinTextBox1.ReadOnly = false;
            this.skinTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.skinTextBox1.Size = new System.Drawing.Size(185, 28);
            // 
            // 
            // 
            this.skinTextBox1.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinTextBox1.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skinTextBox1.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.skinTextBox1.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.skinTextBox1.SkinTxt.Name = "BaseText";
            this.skinTextBox1.SkinTxt.Size = new System.Drawing.Size(175, 18);
            this.skinTextBox1.SkinTxt.TabIndex = 0;
            this.skinTextBox1.SkinTxt.Text = "skinTextBox1";
            this.skinTextBox1.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox1.SkinTxt.WaterText = "rtfg f";
            this.skinTextBox1.TabIndex = 48;
            this.skinTextBox1.Text = "skinTextBox1";
            this.skinTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.skinTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinTextBox1.WaterText = "rtfg f";
            this.skinTextBox1.WordWrap = true;
            // 
            // txtBalance
            // 
            this.txtBalance.BackColor = System.Drawing.Color.Transparent;
            this.txtBalance.DownBack = null;
            this.txtBalance.Icon = null;
            this.txtBalance.IconIsButton = false;
            this.txtBalance.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtBalance.IsPasswordChat = '\0';
            this.txtBalance.IsSystemPasswordChar = false;
            this.txtBalance.Lines = new string[0];
            this.txtBalance.Location = new System.Drawing.Point(651, 108);
            this.txtBalance.Margin = new System.Windows.Forms.Padding(0);
            this.txtBalance.MaxLength = 32767;
            this.txtBalance.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtBalance.MouseBack = null;
            this.txtBalance.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtBalance.Multiline = false;
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.NormlBack = null;
            this.txtBalance.Padding = new System.Windows.Forms.Padding(5);
            this.txtBalance.ReadOnly = false;
            this.txtBalance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtBalance.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtBalance.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBalance.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBalance.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtBalance.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtBalance.SkinTxt.Name = "BaseText";
            this.txtBalance.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtBalance.SkinTxt.TabIndex = 0;
            this.txtBalance.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtBalance.SkinTxt.WaterText = "";
            this.txtBalance.TabIndex = 63;
            this.txtBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtBalance.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtBalance.WaterText = "";
            this.txtBalance.WordWrap = true;
            // 
            // txtDetail
            // 
            this.txtDetail.BackColor = System.Drawing.Color.Transparent;
            this.txtDetail.DownBack = null;
            this.txtDetail.Icon = null;
            this.txtDetail.IconIsButton = false;
            this.txtDetail.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtDetail.IsPasswordChat = '\0';
            this.txtDetail.IsSystemPasswordChar = false;
            this.txtDetail.Lines = new string[0];
            this.txtDetail.Location = new System.Drawing.Point(651, 73);
            this.txtDetail.Margin = new System.Windows.Forms.Padding(0);
            this.txtDetail.MaxLength = 32767;
            this.txtDetail.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtDetail.MouseBack = null;
            this.txtDetail.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtDetail.Multiline = false;
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.NormlBack = null;
            this.txtDetail.Padding = new System.Windows.Forms.Padding(5);
            this.txtDetail.ReadOnly = false;
            this.txtDetail.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtDetail.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtDetail.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDetail.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDetail.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtDetail.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtDetail.SkinTxt.Name = "BaseText";
            this.txtDetail.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtDetail.SkinTxt.TabIndex = 0;
            this.txtDetail.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtDetail.SkinTxt.WaterText = "";
            this.txtDetail.TabIndex = 61;
            this.txtDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtDetail.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtDetail.WaterText = "";
            this.txtDetail.WordWrap = true;
            // 
            // cbType
            // 
            this.cbType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbType.Font = new System.Drawing.Font("宋体", 10F);
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "次数型",
            "折扣型",
            "时间型"});
            this.cbType.Location = new System.Drawing.Point(369, 151);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(90, 24);
            this.cbType.TabIndex = 64;
            this.cbType.Text = "次数型";
            this.cbType.WaterText = "";
            // 
            // cbGender
            // 
            this.cbGender.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbGender.Font = new System.Drawing.Font("宋体", 10F);
            this.cbGender.FormattingEnabled = true;
            this.cbGender.Items.AddRange(new object[] {
            "男",
            "女"});
            this.cbGender.Location = new System.Drawing.Point(136, 150);
            this.cbGender.Name = "cbGender";
            this.cbGender.Size = new System.Drawing.Size(90, 24);
            this.cbGender.TabIndex = 62;
            this.cbGender.Text = "男";
            this.cbGender.WaterText = "";
            // 
            // txtPhone
            // 
            this.txtPhone.BackColor = System.Drawing.Color.Transparent;
            this.txtPhone.DownBack = null;
            this.txtPhone.Icon = null;
            this.txtPhone.IconIsButton = false;
            this.txtPhone.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPhone.IsPasswordChat = '\0';
            this.txtPhone.IsSystemPasswordChar = false;
            this.txtPhone.Lines = new string[0];
            this.txtPhone.Location = new System.Drawing.Point(369, 73);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(0);
            this.txtPhone.MaxLength = 32767;
            this.txtPhone.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPhone.MouseBack = null;
            this.txtPhone.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPhone.Multiline = false;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.NormlBack = null;
            this.txtPhone.Padding = new System.Windows.Forms.Padding(5);
            this.txtPhone.ReadOnly = false;
            this.txtPhone.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPhone.Size = new System.Drawing.Size(135, 28);
            // 
            // 
            // 
            this.txtPhone.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPhone.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPhone.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtPhone.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPhone.SkinTxt.Name = "BaseText";
            this.txtPhone.SkinTxt.Size = new System.Drawing.Size(125, 18);
            this.txtPhone.SkinTxt.TabIndex = 0;
            this.txtPhone.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPhone.SkinTxt.WaterText = "";
            this.txtPhone.TabIndex = 60;
            this.txtPhone.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPhone.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPhone.WaterText = "";
            this.txtPhone.WordWrap = true;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.Transparent;
            this.txtName.DownBack = null;
            this.txtName.Icon = null;
            this.txtName.IconIsButton = false;
            this.txtName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtName.IsPasswordChat = '\0';
            this.txtName.IsSystemPasswordChar = false;
            this.txtName.Lines = new string[0];
            this.txtName.Location = new System.Drawing.Point(136, 112);
            this.txtName.Margin = new System.Windows.Forms.Padding(0);
            this.txtName.MaxLength = 32767;
            this.txtName.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtName.MouseBack = null;
            this.txtName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            this.txtName.NormlBack = null;
            this.txtName.Padding = new System.Windows.Forms.Padding(5);
            this.txtName.ReadOnly = false;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtName.SkinTxt.Name = "BaseText";
            this.txtName.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtName.SkinTxt.TabIndex = 0;
            this.txtName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtName.SkinTxt.WaterText = "";
            this.txtName.TabIndex = 59;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtName.WaterText = "";
            this.txtName.WordWrap = true;
            // 
            // txtNo
            // 
            this.txtNo.BackColor = System.Drawing.Color.Transparent;
            this.txtNo.DownBack = null;
            this.txtNo.Icon = null;
            this.txtNo.IconIsButton = false;
            this.txtNo.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtNo.IsPasswordChat = '\0';
            this.txtNo.IsSystemPasswordChar = false;
            this.txtNo.Lines = new string[0];
            this.txtNo.Location = new System.Drawing.Point(136, 72);
            this.txtNo.Margin = new System.Windows.Forms.Padding(0);
            this.txtNo.MaxLength = 32767;
            this.txtNo.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtNo.MouseBack = null;
            this.txtNo.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtNo.Multiline = false;
            this.txtNo.Name = "txtNo";
            this.txtNo.NormlBack = null;
            this.txtNo.Padding = new System.Windows.Forms.Padding(5);
            this.txtNo.ReadOnly = true;
            this.txtNo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNo.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtNo.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNo.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNo.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtNo.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtNo.SkinTxt.Name = "BaseText";
            this.txtNo.SkinTxt.ReadOnly = true;
            this.txtNo.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtNo.SkinTxt.TabIndex = 0;
            this.txtNo.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtNo.SkinTxt.WaterText = "";
            this.txtNo.TabIndex = 58;
            this.txtNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNo.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtNo.WaterText = "";
            this.txtNo.WordWrap = true;
            // 
            // skinLabel5
            // 
            this.skinLabel5.AutoSize = true;
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.Color.White;
            this.skinLabel5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel5.Location = new System.Drawing.Point(78, 72);
            this.skinLabel5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel5.Name = "skinLabel5";
            this.skinLabel5.Size = new System.Drawing.Size(58, 21);
            this.skinLabel5.TabIndex = 57;
            this.skinLabel5.Text = "卡号：";
            // 
            // labYuan
            // 
            this.labYuan.AutoSize = true;
            this.labYuan.BackColor = System.Drawing.Color.Transparent;
            this.labYuan.BorderColor = System.Drawing.Color.White;
            this.labYuan.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labYuan.Location = new System.Drawing.Point(745, 111);
            this.labYuan.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labYuan.Name = "labYuan";
            this.labYuan.Size = new System.Drawing.Size(26, 21);
            this.labYuan.TabIndex = 56;
            this.labYuan.Text = "元";
            // 
            // labBalance
            // 
            this.labBalance.AutoSize = true;
            this.labBalance.BackColor = System.Drawing.Color.Transparent;
            this.labBalance.BorderColor = System.Drawing.Color.White;
            this.labBalance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labBalance.Location = new System.Drawing.Point(593, 109);
            this.labBalance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labBalance.Name = "labBalance";
            this.labBalance.Size = new System.Drawing.Size(58, 21);
            this.labBalance.TabIndex = 55;
            this.labBalance.Text = "充值：";
            // 
            // labDiscount
            // 
            this.labDiscount.AutoSize = true;
            this.labDiscount.BackColor = System.Drawing.Color.Transparent;
            this.labDiscount.BorderColor = System.Drawing.Color.White;
            this.labDiscount.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDiscount.Location = new System.Drawing.Point(744, 76);
            this.labDiscount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labDiscount.Name = "labDiscount";
            this.labDiscount.Size = new System.Drawing.Size(24, 21);
            this.labDiscount.TabIndex = 54;
            this.labDiscount.Text = "%";
            this.labDiscount.Visible = false;
            // 
            // labDetail
            // 
            this.labDetail.AutoSize = true;
            this.labDetail.BackColor = System.Drawing.Color.Transparent;
            this.labDetail.BorderColor = System.Drawing.Color.White;
            this.labDetail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDetail.Location = new System.Drawing.Point(593, 75);
            this.labDetail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labDetail.Name = "labDetail";
            this.labDetail.Size = new System.Drawing.Size(58, 21);
            this.labDetail.TabIndex = 53;
            this.labDetail.Text = "次数：";
            // 
            // skinLabel12
            // 
            this.skinLabel12.AutoSize = true;
            this.skinLabel12.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel12.BorderColor = System.Drawing.Color.White;
            this.skinLabel12.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel12.Location = new System.Drawing.Point(309, 151);
            this.skinLabel12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel12.Name = "skinLabel12";
            this.skinLabel12.Size = new System.Drawing.Size(58, 21);
            this.skinLabel12.TabIndex = 52;
            this.skinLabel12.Text = "类型：";
            // 
            // skinLabel13
            // 
            this.skinLabel13.AutoSize = true;
            this.skinLabel13.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel13.BorderColor = System.Drawing.Color.White;
            this.skinLabel13.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel13.Location = new System.Drawing.Point(278, 76);
            this.skinLabel13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel13.Name = "skinLabel13";
            this.skinLabel13.Size = new System.Drawing.Size(90, 21);
            this.skinLabel13.TabIndex = 51;
            this.skinLabel13.Text = "联系方式：";
            // 
            // skinLabel14
            // 
            this.skinLabel14.AutoSize = true;
            this.skinLabel14.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel14.BorderColor = System.Drawing.Color.White;
            this.skinLabel14.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel14.Location = new System.Drawing.Point(78, 151);
            this.skinLabel14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel14.Name = "skinLabel14";
            this.skinLabel14.Size = new System.Drawing.Size(58, 21);
            this.skinLabel14.TabIndex = 50;
            this.skinLabel14.Text = "性别：";
            // 
            // skinLabel15
            // 
            this.skinLabel15.AutoSize = true;
            this.skinLabel15.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel15.BorderColor = System.Drawing.Color.White;
            this.skinLabel15.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel15.Location = new System.Drawing.Point(78, 114);
            this.skinLabel15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel15.Name = "skinLabel15";
            this.skinLabel15.Size = new System.Drawing.Size(58, 21);
            this.skinLabel15.TabIndex = 49;
            this.skinLabel15.Text = "姓名：";
            // 
            // Mains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 739);
            this.Controls.Add(this.tabMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Mains";
            this.Text = "Mains";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Mains_FormClosed);
            this.Load += new System.EventHandler(this.Mains_Load);
            this.tabMain.ResumeLayout(false);
            this.tabVip.ResumeLayout(false);
            this.tabVip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabVip;
        private System.Windows.Forms.TabPage tabPro;
        private CCWin.SkinControl.SkinDataGridView dgvVip;
        private CCWin.SkinControl.SkinButton btnAddVip;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private System.Windows.Forms.DateTimePicker dtpRegAt;
        private CCWin.SkinControl.SkinLabel labEnd;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn VipName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn RemainNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn DelAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsDel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastPayAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn PayNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
        private CCWin.SkinControl.SkinLabel labStart;
        private CCWin.SkinControl.SkinTextBox skinTextBox1;
        private CCWin.SkinControl.SkinTextBox txtBalance;
        private CCWin.SkinControl.SkinTextBox txtDetail;
        private CCWin.SkinControl.SkinComboBox cbType;
        private CCWin.SkinControl.SkinComboBox cbGender;
        private CCWin.SkinControl.SkinTextBox txtPhone;
        private CCWin.SkinControl.SkinTextBox txtName;
        private CCWin.SkinControl.SkinTextBox txtNo;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinLabel labYuan;
        private CCWin.SkinControl.SkinLabel labBalance;
        private CCWin.SkinControl.SkinLabel labDiscount;
        private CCWin.SkinControl.SkinLabel labDetail;
        private CCWin.SkinControl.SkinLabel skinLabel12;
        private CCWin.SkinControl.SkinLabel skinLabel13;
        private CCWin.SkinControl.SkinLabel skinLabel14;
        private CCWin.SkinControl.SkinLabel skinLabel15;



    }
}