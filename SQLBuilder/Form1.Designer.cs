namespace SQLBuilder
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
            this.connectBtn = new System.Windows.Forms.Button();
            this.str = new System.Windows.Forms.Label();
            this.SQLStrTB = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.TBList = new System.Windows.Forms.Panel();
            this.SQLTB = new System.Windows.Forms.TextBox();
            this.AppBtn = new System.Windows.Forms.Button();
            this.SAllBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.sfDialog = new System.Windows.Forms.SaveFileDialog();
            this.ReplaceCB = new System.Windows.Forms.CheckBox();
            this.InsertUpdateCB = new System.Windows.Forms.CheckBox();
            this.SelectRangeByWhereCB = new System.Windows.Forms.CheckBox();
            this.SelectByWhereCB = new System.Windows.Forms.CheckBox();
            this.DeleteByWhereCB = new System.Windows.Forms.CheckBox();
            this.UpdateCB = new System.Windows.Forms.CheckBox();
            this.SelectRowCB = new System.Windows.Forms.CheckBox();
            this.SelectAllCB = new System.Windows.Forms.CheckBox();
            this.InsertCB = new System.Windows.Forms.CheckBox();
            this.DeleteRowCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(782, 4);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(75, 23);
            this.connectBtn.TabIndex = 0;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // str
            // 
            this.str.AutoSize = true;
            this.str.Location = new System.Drawing.Point(12, 9);
            this.str.Name = "str";
            this.str.Size = new System.Drawing.Size(113, 12);
            this.str.TabIndex = 1;
            this.str.Text = "Connection String:";
            // 
            // SQLStrTB
            // 
            this.SQLStrTB.Location = new System.Drawing.Point(131, 6);
            this.SQLStrTB.Name = "SQLStrTB";
            this.SQLStrTB.Size = new System.Drawing.Size(645, 21);
            this.SQLStrTB.TabIndex = 2;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(363, 432);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // TBList
            // 
            this.TBList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TBList.Location = new System.Drawing.Point(12, 52);
            this.TBList.Name = "TBList";
            this.TBList.Size = new System.Drawing.Size(426, 371);
            this.TBList.TabIndex = 5;
            // 
            // SQLTB
            // 
            this.SQLTB.Location = new System.Drawing.Point(444, 52);
            this.SQLTB.Multiline = true;
            this.SQLTB.Name = "SQLTB";
            this.SQLTB.Size = new System.Drawing.Size(413, 371);
            this.SQLTB.TabIndex = 6;
            // 
            // AppBtn
            // 
            this.AppBtn.Location = new System.Drawing.Point(525, 432);
            this.AppBtn.Name = "AppBtn";
            this.AppBtn.Size = new System.Drawing.Size(75, 23);
            this.AppBtn.TabIndex = 7;
            this.AppBtn.Text = "Applicate";
            this.AppBtn.UseVisualStyleBackColor = true;
            this.AppBtn.Click += new System.EventHandler(this.AppBtn_Click);
            // 
            // SAllBtn
            // 
            this.SAllBtn.Location = new System.Drawing.Point(444, 432);
            this.SAllBtn.Name = "SAllBtn";
            this.SAllBtn.Size = new System.Drawing.Size(75, 23);
            this.SAllBtn.TabIndex = 8;
            this.SAllBtn.Text = "Select All";
            this.SAllBtn.UseVisualStyleBackColor = true;
            this.SAllBtn.Click += new System.EventHandler(this.SAllBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(782, 431);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 23);
            this.SaveBtn.TabIndex = 9;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // sfDialog
            // 
            this.sfDialog.DefaultExt = "sql";
            this.sfDialog.FileName = "StoreBuilder";
            // 
            // ReplaceCB
            // 
            this.ReplaceCB.AutoSize = true;
            this.ReplaceCB.Checked = true;
            this.ReplaceCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ReplaceCB.Location = new System.Drawing.Point(12, 436);
            this.ReplaceCB.Name = "ReplaceCB";
            this.ReplaceCB.Size = new System.Drawing.Size(102, 16);
            this.ReplaceCB.TabIndex = 10;
            this.ReplaceCB.Text = "Replace Store";
            this.ReplaceCB.UseVisualStyleBackColor = true;
            // 
            // InsertUpdateCB
            // 
            this.InsertUpdateCB.AutoSize = true;
            this.InsertUpdateCB.Checked = true;
            this.InsertUpdateCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.InsertUpdateCB.Location = new System.Drawing.Point(728, 32);
            this.InsertUpdateCB.Name = "InsertUpdateCB";
            this.InsertUpdateCB.Size = new System.Drawing.Size(96, 16);
            this.InsertUpdateCB.TabIndex = 28;
            this.InsertUpdateCB.Text = "InsertUpdate";
            this.InsertUpdateCB.UseVisualStyleBackColor = true;
            // 
            // SelectRangeByWhereCB
            // 
            this.SelectRangeByWhereCB.AutoSize = true;
            this.SelectRangeByWhereCB.Checked = true;
            this.SelectRangeByWhereCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectRangeByWhereCB.Location = new System.Drawing.Point(593, 32);
            this.SelectRangeByWhereCB.Name = "SelectRangeByWhereCB";
            this.SelectRangeByWhereCB.Size = new System.Drawing.Size(132, 16);
            this.SelectRangeByWhereCB.TabIndex = 27;
            this.SelectRangeByWhereCB.Text = "SelectRangeByWhere";
            this.SelectRangeByWhereCB.UseVisualStyleBackColor = true;
            // 
            // SelectByWhereCB
            // 
            this.SelectByWhereCB.AutoSize = true;
            this.SelectByWhereCB.Checked = true;
            this.SelectByWhereCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectByWhereCB.Location = new System.Drawing.Point(488, 32);
            this.SelectByWhereCB.Name = "SelectByWhereCB";
            this.SelectByWhereCB.Size = new System.Drawing.Size(102, 16);
            this.SelectByWhereCB.TabIndex = 26;
            this.SelectByWhereCB.Text = "SelectByWhere";
            this.SelectByWhereCB.UseVisualStyleBackColor = true;
            // 
            // DeleteByWhereCB
            // 
            this.DeleteByWhereCB.AutoSize = true;
            this.DeleteByWhereCB.Checked = true;
            this.DeleteByWhereCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DeleteByWhereCB.Location = new System.Drawing.Point(383, 32);
            this.DeleteByWhereCB.Name = "DeleteByWhereCB";
            this.DeleteByWhereCB.Size = new System.Drawing.Size(102, 16);
            this.DeleteByWhereCB.TabIndex = 25;
            this.DeleteByWhereCB.Text = "DeleteByWhere";
            this.DeleteByWhereCB.UseVisualStyleBackColor = true;
            // 
            // UpdateCB
            // 
            this.UpdateCB.AutoSize = true;
            this.UpdateCB.Checked = true;
            this.UpdateCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UpdateCB.Location = new System.Drawing.Point(320, 32);
            this.UpdateCB.Name = "UpdateCB";
            this.UpdateCB.Size = new System.Drawing.Size(60, 16);
            this.UpdateCB.TabIndex = 24;
            this.UpdateCB.Text = "Update";
            this.UpdateCB.UseVisualStyleBackColor = true;
            // 
            // SelectRowCB
            // 
            this.SelectRowCB.AutoSize = true;
            this.SelectRowCB.Checked = true;
            this.SelectRowCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectRowCB.Location = new System.Drawing.Point(239, 32);
            this.SelectRowCB.Name = "SelectRowCB";
            this.SelectRowCB.Size = new System.Drawing.Size(78, 16);
            this.SelectRowCB.TabIndex = 23;
            this.SelectRowCB.Text = "SelectRow";
            this.SelectRowCB.UseVisualStyleBackColor = true;
            // 
            // SelectAllCB
            // 
            this.SelectAllCB.AutoSize = true;
            this.SelectAllCB.Checked = true;
            this.SelectAllCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SelectAllCB.Location = new System.Drawing.Point(158, 32);
            this.SelectAllCB.Name = "SelectAllCB";
            this.SelectAllCB.Size = new System.Drawing.Size(78, 16);
            this.SelectAllCB.TabIndex = 22;
            this.SelectAllCB.Text = "SelectAll";
            this.SelectAllCB.UseVisualStyleBackColor = true;
            // 
            // InsertCB
            // 
            this.InsertCB.AutoSize = true;
            this.InsertCB.Checked = true;
            this.InsertCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.InsertCB.Location = new System.Drawing.Point(95, 32);
            this.InsertCB.Name = "InsertCB";
            this.InsertCB.Size = new System.Drawing.Size(60, 16);
            this.InsertCB.TabIndex = 21;
            this.InsertCB.Text = "Insert";
            this.InsertCB.UseVisualStyleBackColor = true;
            // 
            // DeleteRowCB
            // 
            this.DeleteRowCB.AutoSize = true;
            this.DeleteRowCB.Checked = true;
            this.DeleteRowCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DeleteRowCB.Location = new System.Drawing.Point(14, 32);
            this.DeleteRowCB.Name = "DeleteRowCB";
            this.DeleteRowCB.Size = new System.Drawing.Size(78, 16);
            this.DeleteRowCB.TabIndex = 20;
            this.DeleteRowCB.Text = "DeleteRow";
            this.DeleteRowCB.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 463);
            this.Controls.Add(this.InsertUpdateCB);
            this.Controls.Add(this.SelectRangeByWhereCB);
            this.Controls.Add(this.SelectByWhereCB);
            this.Controls.Add(this.DeleteByWhereCB);
            this.Controls.Add(this.UpdateCB);
            this.Controls.Add(this.SelectRowCB);
            this.Controls.Add(this.SelectAllCB);
            this.Controls.Add(this.InsertCB);
            this.Controls.Add(this.DeleteRowCB);
            this.Controls.Add(this.ReplaceCB);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.SAllBtn);
            this.Controls.Add(this.AppBtn);
            this.Controls.Add(this.SQLTB);
            this.Controls.Add(this.TBList);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SQLStrTB);
            this.Controls.Add(this.str);
            this.Controls.Add(this.connectBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.Label str;
        private System.Windows.Forms.TextBox SQLStrTB;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Panel TBList;
        private System.Windows.Forms.TextBox SQLTB;
        private System.Windows.Forms.Button AppBtn;
        private System.Windows.Forms.Button SAllBtn;
        private System.Windows.Forms.Button SaveBtn;
        private System.Windows.Forms.SaveFileDialog sfDialog;
        private System.Windows.Forms.CheckBox ReplaceCB;
        private System.Windows.Forms.CheckBox InsertUpdateCB;
        private System.Windows.Forms.CheckBox SelectRangeByWhereCB;
        private System.Windows.Forms.CheckBox SelectByWhereCB;
        private System.Windows.Forms.CheckBox DeleteByWhereCB;
        private System.Windows.Forms.CheckBox UpdateCB;
        private System.Windows.Forms.CheckBox SelectRowCB;
        private System.Windows.Forms.CheckBox SelectAllCB;
        private System.Windows.Forms.CheckBox InsertCB;
        private System.Windows.Forms.CheckBox DeleteRowCB;
    }
}

