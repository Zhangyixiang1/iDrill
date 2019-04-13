namespace DrillingSymtemCSCV2.Forms
{
    partial class EditUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUserForm));
            this.gbx_permission = new System.Windows.Forms.GroupBox();
            this.rbtn_p2 = new System.Windows.Forms.RadioButton();
            this.rbtn_p1 = new System.Windows.Forms.RadioButton();
            this.rlbl_permission = new Telerik.WinControls.UI.RadLabel();
            this.lbl_PassWord = new Telerik.WinControls.UI.RadLabel();
            this.txt_RealName = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_RealName = new Telerik.WinControls.UI.RadLabel();
            this.lbl_LoginName = new Telerik.WinControls.UI.RadLabel();
            this.txt_pwd = new Telerik.WinControls.UI.RadTextBox();
            this.txt_userName = new Telerik.WinControls.UI.RadLabel();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.btn_ok = new Telerik.WinControls.UI.RadButton();
            this.gbx_permission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_permission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RealName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_RealName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_LoginName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_userName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gbx_permission
            // 
            this.gbx_permission.Controls.Add(this.rbtn_p2);
            this.gbx_permission.Controls.Add(this.rbtn_p1);
            this.gbx_permission.Location = new System.Drawing.Point(236, 190);
            this.gbx_permission.Name = "gbx_permission";
            this.gbx_permission.Size = new System.Drawing.Size(226, 50);
            this.gbx_permission.TabIndex = 36;
            this.gbx_permission.TabStop = false;
            // 
            // rbtn_p2
            // 
            this.rbtn_p2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtn_p2.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_p2.ForeColor = System.Drawing.Color.White;
            this.rbtn_p2.Location = new System.Drawing.Point(126, 11);
            this.rbtn_p2.Name = "rbtn_p2";
            this.rbtn_p2.Size = new System.Drawing.Size(94, 34);
            this.rbtn_p2.TabIndex = 25;
            this.rbtn_p2.Text = "Driller";
            this.rbtn_p2.UseVisualStyleBackColor = true;
            // 
            // rbtn_p1
            // 
            this.rbtn_p1.Checked = true;
            this.rbtn_p1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_p1.ForeColor = System.Drawing.Color.White;
            this.rbtn_p1.Location = new System.Drawing.Point(6, 11);
            this.rbtn_p1.Name = "rbtn_p1";
            this.rbtn_p1.Size = new System.Drawing.Size(114, 34);
            this.rbtn_p1.TabIndex = 25;
            this.rbtn_p1.TabStop = true;
            this.rbtn_p1.Text = "Tourist";
            this.rbtn_p1.UseVisualStyleBackColor = true;
            // 
            // rlbl_permission
            // 
            this.rlbl_permission.AutoSize = false;
            this.rlbl_permission.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_permission.ForeColor = System.Drawing.Color.White;
            this.rlbl_permission.Location = new System.Drawing.Point(71, 200);
            this.rlbl_permission.Name = "rlbl_permission";
            this.rlbl_permission.Size = new System.Drawing.Size(159, 30);
            this.rlbl_permission.TabIndex = 32;
            this.rlbl_permission.Text = "Permission:";
            this.rlbl_permission.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_PassWord
            // 
            this.lbl_PassWord.AutoSize = false;
            this.lbl_PassWord.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PassWord.ForeColor = System.Drawing.Color.White;
            this.lbl_PassWord.Location = new System.Drawing.Point(125, 147);
            this.lbl_PassWord.Name = "lbl_PassWord";
            this.lbl_PassWord.Size = new System.Drawing.Size(105, 30);
            this.lbl_PassWord.TabIndex = 31;
            this.lbl_PassWord.Text = "PassWord:";
            this.lbl_PassWord.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_RealName
            // 
            this.txt_RealName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_RealName.ForeColor = System.Drawing.Color.Chartreuse;
            this.txt_RealName.Location = new System.Drawing.Point(236, 91);
            this.txt_RealName.MaxLength = 15;
            this.txt_RealName.Name = "txt_RealName";
            this.txt_RealName.Size = new System.Drawing.Size(178, 32);
            this.txt_RealName.TabIndex = 30;
            this.txt_RealName.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_RealName.GetChildAt(0))).ForeColor = System.Drawing.Color.Chartreuse;
            // 
            // lbl_RealName
            // 
            this.lbl_RealName.AutoSize = false;
            this.lbl_RealName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RealName.ForeColor = System.Drawing.Color.White;
            this.lbl_RealName.Location = new System.Drawing.Point(122, 90);
            this.lbl_RealName.Name = "lbl_RealName";
            this.lbl_RealName.Size = new System.Drawing.Size(108, 30);
            this.lbl_RealName.TabIndex = 29;
            this.lbl_RealName.Text = "RealName:";
            this.lbl_RealName.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_LoginName
            // 
            this.lbl_LoginName.AutoSize = false;
            this.lbl_LoginName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoginName.ForeColor = System.Drawing.Color.White;
            this.lbl_LoginName.Location = new System.Drawing.Point(110, 40);
            this.lbl_LoginName.Name = "lbl_LoginName";
            this.lbl_LoginName.Size = new System.Drawing.Size(120, 30);
            this.lbl_LoginName.TabIndex = 27;
            this.lbl_LoginName.Text = "LoginName:";
            this.lbl_LoginName.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_pwd.ForeColor = System.Drawing.Color.Chartreuse;
            this.txt_pwd.Location = new System.Drawing.Point(236, 147);
            this.txt_pwd.MaxLength = 15;
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(178, 32);
            this.txt_pwd.TabIndex = 30;
            this.txt_pwd.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_pwd.GetChildAt(0))).ForeColor = System.Drawing.Color.Chartreuse;
            // 
            // txt_userName
            // 
            this.txt_userName.AutoSize = false;
            this.txt_userName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_userName.ForeColor = System.Drawing.Color.White;
            this.txt_userName.Location = new System.Drawing.Point(262, 40);
            this.txt_userName.Name = "txt_userName";
            this.txt_userName.Size = new System.Drawing.Size(120, 30);
            this.txt_userName.TabIndex = 27;
            this.txt_userName.Text = "...";
            this.txt_userName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(292, 282);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(108, 33);
            this.btn_Cancel.TabIndex = 38;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_ok
            // 
            this.btn_ok.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_ok.Location = new System.Drawing.Point(143, 282);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(108, 33);
            this.btn_ok.TabIndex = 39;
            this.btn_ok.Text = "OK";
            this.btn_ok.ThemeName = "VisualStudio2012Dark";
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click_1);
            // 
            // EditUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 337);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.gbx_permission);
            this.Controls.Add(this.rlbl_permission);
            this.Controls.Add(this.lbl_PassWord);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.txt_RealName);
            this.Controls.Add(this.lbl_RealName);
            this.Controls.Add(this.txt_userName);
            this.Controls.Add(this.lbl_LoginName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditUserForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.EditUserForm_Load);
            this.gbx_permission.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_permission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RealName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_RealName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_LoginName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_pwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_userName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbx_permission;
        private System.Windows.Forms.RadioButton rbtn_p2;
        private System.Windows.Forms.RadioButton rbtn_p1;
        private Telerik.WinControls.UI.RadLabel rlbl_permission;
        private Telerik.WinControls.UI.RadLabel lbl_PassWord;
        private Telerik.WinControls.UI.RadTextBox txt_RealName;
        private Telerik.WinControls.UI.RadLabel lbl_RealName;
        private Telerik.WinControls.UI.RadLabel lbl_LoginName;
        private Telerik.WinControls.UI.RadTextBox txt_pwd;
        private Telerik.WinControls.UI.RadLabel txt_userName;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private Telerik.WinControls.UI.RadButton btn_ok;
    }
}