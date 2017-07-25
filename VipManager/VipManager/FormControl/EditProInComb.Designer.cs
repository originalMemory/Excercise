namespace VipManager.FormControl
{
    partial class EditProInComb
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
            this.lbEditPro = new System.Windows.Forms.ListBox();
            this.skinLabel10 = new CCWin.SkinControl.SkinLabel();
            this.cbPro = new CCWin.SkinControl.SkinComboBox();
            this.btnDelProInComb = new CCWin.SkinControl.SkinButton();
            this.btnAddProInComb = new CCWin.SkinControl.SkinButton();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.btnSave = new CCWin.SkinControl.SkinButton();
            this.btnLeave = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // lbEditPro
            // 
            this.lbEditPro.FormattingEnabled = true;
            this.lbEditPro.ItemHeight = 15;
            this.lbEditPro.Location = new System.Drawing.Point(228, 105);
            this.lbEditPro.Name = "lbEditPro";
            this.lbEditPro.Size = new System.Drawing.Size(207, 169);
            this.lbEditPro.TabIndex = 45;
            // 
            // skinLabel10
            // 
            this.skinLabel10.AutoSize = true;
            this.skinLabel10.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel10.BorderColor = System.Drawing.Color.White;
            this.skinLabel10.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel10.Location = new System.Drawing.Point(105, 46);
            this.skinLabel10.Name = "skinLabel10";
            this.skinLabel10.Size = new System.Drawing.Size(112, 27);
            this.skinLabel10.TabIndex = 44;
            this.skinLabel10.Text = "产品列表：";
            // 
            // cbPro
            // 
            this.cbPro.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbPro.Font = new System.Drawing.Font("宋体", 10F);
            this.cbPro.FormattingEnabled = true;
            this.cbPro.Location = new System.Drawing.Point(228, 46);
            this.cbPro.Margin = new System.Windows.Forms.Padding(4);
            this.cbPro.Name = "cbPro";
            this.cbPro.Size = new System.Drawing.Size(207, 28);
            this.cbPro.TabIndex = 43;
            this.cbPro.WaterText = "";
            // 
            // btnDelProInComb
            // 
            this.btnDelProInComb.BackColor = System.Drawing.Color.Transparent;
            this.btnDelProInComb.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnDelProInComb.DownBack = null;
            this.btnDelProInComb.Font = new System.Drawing.Font("宋体", 11F);
            this.btnDelProInComb.Location = new System.Drawing.Point(468, 105);
            this.btnDelProInComb.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelProInComb.MouseBack = null;
            this.btnDelProInComb.Name = "btnDelProInComb";
            this.btnDelProInComb.NormlBack = null;
            this.btnDelProInComb.Size = new System.Drawing.Size(113, 35);
            this.btnDelProInComb.TabIndex = 42;
            this.btnDelProInComb.Text = "删除产品";
            this.btnDelProInComb.UseVisualStyleBackColor = false;
            this.btnDelProInComb.Click += new System.EventHandler(this.btnDelProInComb_Click);
            // 
            // btnAddProInComb
            // 
            this.btnAddProInComb.BackColor = System.Drawing.Color.Transparent;
            this.btnAddProInComb.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnAddProInComb.DownBack = null;
            this.btnAddProInComb.Font = new System.Drawing.Font("宋体", 11F);
            this.btnAddProInComb.Location = new System.Drawing.Point(468, 46);
            this.btnAddProInComb.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddProInComb.MouseBack = null;
            this.btnAddProInComb.Name = "btnAddProInComb";
            this.btnAddProInComb.NormlBack = null;
            this.btnAddProInComb.Size = new System.Drawing.Size(113, 35);
            this.btnAddProInComb.TabIndex = 41;
            this.btnAddProInComb.Text = "添加产品";
            this.btnAddProInComb.UseVisualStyleBackColor = false;
            this.btnAddProInComb.Click += new System.EventHandler(this.btnAddProInComb_Click);
            // 
            // skinLabel2
            // 
            this.skinLabel2.AutoSize = true;
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.Color.White;
            this.skinLabel2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinLabel2.Location = new System.Drawing.Point(85, 97);
            this.skinLabel2.Name = "skinLabel2";
            this.skinLabel2.Size = new System.Drawing.Size(132, 27);
            this.skinLabel2.TabIndex = 40;
            this.skinLabel2.Text = "套餐内产品：";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSave.DownBack = null;
            this.btnSave.Font = new System.Drawing.Font("宋体", 11F);
            this.btnSave.Location = new System.Drawing.Point(178, 331);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.MouseBack = null;
            this.btnSave.Name = "btnSave";
            this.btnSave.NormlBack = null;
            this.btnSave.Size = new System.Drawing.Size(113, 35);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLeave
            // 
            this.btnLeave.BackColor = System.Drawing.Color.Transparent;
            this.btnLeave.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLeave.DownBack = null;
            this.btnLeave.Font = new System.Drawing.Font("宋体", 11F);
            this.btnLeave.Location = new System.Drawing.Point(351, 331);
            this.btnLeave.Margin = new System.Windows.Forms.Padding(4);
            this.btnLeave.MouseBack = null;
            this.btnLeave.Name = "btnLeave";
            this.btnLeave.NormlBack = null;
            this.btnLeave.Size = new System.Drawing.Size(113, 35);
            this.btnLeave.TabIndex = 47;
            this.btnLeave.Text = "取消";
            this.btnLeave.UseVisualStyleBackColor = false;
            this.btnLeave.Click += new System.EventHandler(this.btnLeave_Click);
            // 
            // EditProInComb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(657, 451);
            this.Controls.Add(this.btnLeave);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lbEditPro);
            this.Controls.Add(this.skinLabel10);
            this.Controls.Add(this.cbPro);
            this.Controls.Add(this.btnDelProInComb);
            this.Controls.Add(this.btnAddProInComb);
            this.Controls.Add(this.skinLabel2);
            this.Name = "EditProInComb";
            this.Text = "EditProInComb";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbEditPro;
        private CCWin.SkinControl.SkinLabel skinLabel10;
        private CCWin.SkinControl.SkinComboBox cbPro;
        private CCWin.SkinControl.SkinButton btnDelProInComb;
        private CCWin.SkinControl.SkinButton btnAddProInComb;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinButton btnSave;
        private CCWin.SkinControl.SkinButton btnLeave;
    }
}