namespace VipManager.UControl
{
    partial class UCombNum
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNum = new CCWin.SkinControl.SkinTextBox();
            this.txtPrice = new CCWin.SkinControl.SkinTextBox();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.labDetail2 = new CCWin.SkinControl.SkinLabel();
            this.labPrice = new CCWin.SkinControl.SkinLabel();
            this.SuspendLayout();
            // 
            // txtNum
            // 
            this.txtNum.BackColor = System.Drawing.Color.Transparent;
            this.txtNum.DownBack = null;
            this.txtNum.Icon = null;
            this.txtNum.IconIsButton = false;
            this.txtNum.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtNum.IsPasswordChat = '\0';
            this.txtNum.IsSystemPasswordChar = false;
            this.txtNum.Lines = new string[0];
            this.txtNum.Location = new System.Drawing.Point(68, 60);
            this.txtNum.Margin = new System.Windows.Forms.Padding(0);
            this.txtNum.MaxLength = 32767;
            this.txtNum.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtNum.MouseBack = null;
            this.txtNum.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtNum.Multiline = false;
            this.txtNum.Name = "txtNum";
            this.txtNum.NormlBack = null;
            this.txtNum.Padding = new System.Windows.Forms.Padding(5);
            this.txtNum.ReadOnly = false;
            this.txtNum.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNum.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtNum.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNum.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNum.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtNum.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtNum.SkinTxt.Name = "BaseText";
            this.txtNum.SkinTxt.ReadOnly = true;
            this.txtNum.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtNum.SkinTxt.TabIndex = 0;
            this.txtNum.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtNum.SkinTxt.WaterText = "";
            this.txtNum.TabIndex = 38;
            this.txtNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNum.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtNum.WaterText = "";
            this.txtNum.WordWrap = true;
            this.txtNum.SkinTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(VipManager.Helper.ControlEvent.NumLimit);
            // 
            // txtPrice
            // 
            this.txtPrice.BackColor = System.Drawing.Color.Transparent;
            this.txtPrice.DownBack = null;
            this.txtPrice.Icon = null;
            this.txtPrice.IconIsButton = false;
            this.txtPrice.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPrice.IsPasswordChat = '\0';
            this.txtPrice.IsSystemPasswordChar = false;
            this.txtPrice.Lines = new string[0];
            this.txtPrice.Location = new System.Drawing.Point(68, 19);
            this.txtPrice.Margin = new System.Windows.Forms.Padding(0);
            this.txtPrice.MaxLength = 32767;
            this.txtPrice.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPrice.MouseBack = null;
            this.txtPrice.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPrice.Multiline = false;
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.NormlBack = null;
            this.txtPrice.Padding = new System.Windows.Forms.Padding(5);
            this.txtPrice.ReadOnly = false;
            this.txtPrice.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPrice.Size = new System.Drawing.Size(90, 28);
            // 
            // 
            // 
            this.txtPrice.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrice.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrice.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.txtPrice.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPrice.SkinTxt.Name = "BaseText";
            this.txtPrice.SkinTxt.Size = new System.Drawing.Size(80, 18);
            this.txtPrice.SkinTxt.TabIndex = 0;
            this.txtPrice.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPrice.SkinTxt.WaterText = "";
            this.txtPrice.TabIndex = 37;
            this.txtPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPrice.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPrice.WaterText = "";
            this.txtPrice.WordWrap = true;
            this.txtPrice.SkinTxt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(VipManager.Helper.ControlEvent.DoubleLimit);
            // 
            // skinLabel7
            // 
            this.skinLabel7.AutoSize = true;
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.Color.White;
            this.skinLabel7.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel7.Location = new System.Drawing.Point(10, 63);
            this.skinLabel7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.skinLabel7.Name = "skinLabel7";
            this.skinLabel7.Size = new System.Drawing.Size(58, 21);
            this.skinLabel7.TabIndex = 35;
            this.skinLabel7.Text = "次数：";
            // 
            // labDetail2
            // 
            this.labDetail2.AutoSize = true;
            this.labDetail2.BackColor = System.Drawing.Color.Transparent;
            this.labDetail2.BorderColor = System.Drawing.Color.White;
            this.labDetail2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labDetail2.Location = new System.Drawing.Point(161, 22);
            this.labDetail2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labDetail2.Name = "labDetail2";
            this.labDetail2.Size = new System.Drawing.Size(26, 21);
            this.labDetail2.TabIndex = 34;
            this.labDetail2.Text = "元";
            this.labDetail2.Visible = false;
            // 
            // labPrice
            // 
            this.labPrice.AutoSize = true;
            this.labPrice.BackColor = System.Drawing.Color.Transparent;
            this.labPrice.BorderColor = System.Drawing.Color.White;
            this.labPrice.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labPrice.Location = new System.Drawing.Point(10, 22);
            this.labPrice.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new System.Drawing.Size(58, 21);
            this.labPrice.TabIndex = 33;
            this.labPrice.Text = "价格：";
            // 
            // UCombNum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.skinLabel7);
            this.Controls.Add(this.labDetail2);
            this.Controls.Add(this.labPrice);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UCombNum";
            this.Size = new System.Drawing.Size(200, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinTextBox txtNum;
        private CCWin.SkinControl.SkinTextBox txtPrice;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinLabel labDetail2;
        private CCWin.SkinControl.SkinLabel labPrice;
    }
}
