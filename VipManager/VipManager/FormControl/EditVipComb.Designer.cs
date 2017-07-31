namespace VipManager.FormControl
{
    partial class EditVipComb
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
            this.lbPro = new System.Windows.Forms.ListBox();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.cbComb = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.btnSaveVipComb = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // lbPro
            // 
            this.lbPro.FormattingEnabled = true;
            this.lbPro.ItemHeight = 15;
            this.lbPro.Location = new System.Drawing.Point(229, 123);
            this.lbPro.Name = "lbPro";
            this.lbPro.Size = new System.Drawing.Size(207, 169);
            this.lbPro.TabIndex = 45;
            // 
            // skinLabel10
            // 
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel10.Location = new System.Drawing.Point(106, 64);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(112, 27);
            this.skinLabel10.TabIndex = 44;
            this.skinLabel10.Text = "套餐列表：";
            // 
            // cbComb
            // 
            this.cbComb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbComb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbComb.Font = new System.Drawing.Font("宋体", 10F);
            this.cbComb.FormattingEnabled = true;
            this.cbComb.Location = new System.Drawing.Point(229, 64);
            this.cbComb.Margin = new System.Windows.Forms.Padding(4);
            this.cbComb.Name = "cbComb";
            this.cbComb.Size = new System.Drawing.Size(207, 28);
            this.cbComb.TabIndex = 43;
            this.cbComb.WaterText = "";
            this.cbComb.TextChanged += new System.EventHandler(this.cbComb_TextChanged);
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(86, 126);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(132, 27);
            this.skinLabel2.TabIndex = 40;
            this.skinLabel2.Text = "套餐内产品：";
            // 
            // btnSaveVipComb
            // 
            this.btnSaveVipComb.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveVipComb.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSaveVipComb.DownBack = null;
            this.btnSaveVipComb.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSaveVipComb.Location = new System.Drawing.Point(199, 351);
            this.btnSaveVipComb.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveVipComb.MouseBack = null;
            this.btnSaveVipComb.Name = "btnSaveVipComb";
            this.btnSaveVipComb.NormlBack = null;
            this.btnSaveVipComb.Size = new System.Drawing.Size(128, 35);
            this.btnSaveVipComb.TabIndex = 158;
            this.btnSaveVipComb.Text = "保存";
            this.btnSaveVipComb.UseVisualStyleBackColor = false;
            this.btnSaveVipComb.Click += new System.EventHandler(this.btnSaveVipComb_Click);
            // 
            // EditVipComb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 453);
            this.Controls.Add(this.btnSaveVipComb);
            this.Controls.Add(this.lbPro);
            this.Controls.Add(this.skinLabel10);
            this.Controls.Add(this.cbComb);
            this.Controls.Add(this.skinLabel2);
            this.Name = "EditVipComb";
            this.Text = "修改会员套餐";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbPro;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private CCWin.SkinControl.SkinComboBox cbComb;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinButton btnSaveVipComb;
    }
}