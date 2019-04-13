namespace DrillingSymtemCSCV2.Forms
{
    partial class AddUserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddUserForm));
            this.lbl_AddUser = new Telerik.WinControls.UI.RadLabel();
            this.radPanel = new Telerik.WinControls.UI.RadPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.gbx_permission = new System.Windows.Forms.GroupBox();
            this.rbtn_p2 = new System.Windows.Forms.RadioButton();
            this.rbtn_p1 = new System.Windows.Forms.RadioButton();
            this.txt_PassWordAgain = new Telerik.WinControls.UI.RadTextBox();
            this.txt_PassWord = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_permission = new Telerik.WinControls.UI.RadLabel();
            this.lbl_PassWordAgain = new Telerik.WinControls.UI.RadLabel();
            this.lbl_PassWord = new Telerik.WinControls.UI.RadLabel();
            this.txt_RealName = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_RealName = new Telerik.WinControls.UI.RadLabel();
            this.txt_LoginName = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_LoginName = new Telerik.WinControls.UI.RadLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btn_OK = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_AddUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).BeginInit();
            this.radPanel.SuspendLayout();
            this.panel.SuspendLayout();
            this.gbx_permission.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_PassWordAgain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_PassWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_permission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWordAgain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RealName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_RealName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_LoginName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_LoginName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_AddUser
            // 
            this.lbl_AddUser.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_AddUser.ForeColor = System.Drawing.Color.White;
            this.lbl_AddUser.Location = new System.Drawing.Point(46, 10);
            this.lbl_AddUser.Name = "lbl_AddUser";
            this.lbl_AddUser.Size = new System.Drawing.Size(115, 31);
            this.lbl_AddUser.TabIndex = 21;
            this.lbl_AddUser.Text = "Add User：";
            // 
            // radPanel
            // 
            this.radPanel.Controls.Add(this.panel);
            this.radPanel.Controls.Add(this.label1);
            this.radPanel.Location = new System.Drawing.Point(42, 28);
            this.radPanel.Name = "radPanel";
            this.radPanel.Size = new System.Drawing.Size(519, 398);
            this.radPanel.TabIndex = 22;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.gbx_permission);
            this.panel.Controls.Add(this.txt_PassWordAgain);
            this.panel.Controls.Add(this.txt_PassWord);
            this.panel.Controls.Add(this.rlbl_permission);
            this.panel.Controls.Add(this.lbl_PassWordAgain);
            this.panel.Controls.Add(this.lbl_PassWord);
            this.panel.Controls.Add(this.txt_RealName);
            this.panel.Controls.Add(this.lbl_RealName);
            this.panel.Controls.Add(this.txt_LoginName);
            this.panel.Controls.Add(this.lbl_LoginName);
            this.panel.Location = new System.Drawing.Point(3, 26);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(513, 296);
            this.panel.TabIndex = 19;
            // 
            // gbx_permission
            // 
            this.gbx_permission.Controls.Add(this.rbtn_p2);
            this.gbx_permission.Controls.Add(this.rbtn_p1);
            this.gbx_permission.Location = new System.Drawing.Point(215, 224);
            this.gbx_permission.Name = "gbx_permission";
            this.gbx_permission.Size = new System.Drawing.Size(226, 50);
            this.gbx_permission.TabIndex = 26;
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
            // txt_PassWordAgain
            // 
            this.txt_PassWordAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_PassWordAgain.AutoSize = false;
            this.txt_PassWordAgain.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PassWordAgain.Location = new System.Drawing.Point(215, 181);
            this.txt_PassWordAgain.MaxLength = 15;
            this.txt_PassWordAgain.Multiline = true;
            this.txt_PassWordAgain.Name = "txt_PassWordAgain";
            this.txt_PassWordAgain.PasswordChar = '*';
            this.txt_PassWordAgain.Size = new System.Drawing.Size(180, 30);
            this.txt_PassWordAgain.TabIndex = 24;
            this.txt_PassWordAgain.ThemeName = "VisualStudio2012Dark";
            // 
            // txt_PassWord
            // 
            this.txt_PassWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_PassWord.AutoSize = false;
            this.txt_PassWord.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_PassWord.Location = new System.Drawing.Point(215, 127);
            this.txt_PassWord.MaxLength = 15;
            this.txt_PassWord.Multiline = true;
            this.txt_PassWord.Name = "txt_PassWord";
            this.txt_PassWord.PasswordChar = '*';
            this.txt_PassWord.Size = new System.Drawing.Size(180, 30);
            this.txt_PassWord.TabIndex = 23;
            this.txt_PassWord.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_permission
            // 
            this.rlbl_permission.AutoSize = false;
            this.rlbl_permission.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_permission.ForeColor = System.Drawing.Color.White;
            this.rlbl_permission.Location = new System.Drawing.Point(50, 234);
            this.rlbl_permission.Name = "rlbl_permission";
            this.rlbl_permission.Size = new System.Drawing.Size(159, 30);
            this.rlbl_permission.TabIndex = 21;
            this.rlbl_permission.Text = "Permission:";
            this.rlbl_permission.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_PassWordAgain
            // 
            this.lbl_PassWordAgain.AutoSize = false;
            this.lbl_PassWordAgain.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PassWordAgain.ForeColor = System.Drawing.Color.White;
            this.lbl_PassWordAgain.Location = new System.Drawing.Point(50, 180);
            this.lbl_PassWordAgain.Name = "lbl_PassWordAgain";
            this.lbl_PassWordAgain.Size = new System.Drawing.Size(159, 30);
            this.lbl_PassWordAgain.TabIndex = 21;
            this.lbl_PassWordAgain.Text = "PassWordAgain:";
            this.lbl_PassWordAgain.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_PassWord
            // 
            this.lbl_PassWord.AutoSize = false;
            this.lbl_PassWord.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_PassWord.ForeColor = System.Drawing.Color.White;
            this.lbl_PassWord.Location = new System.Drawing.Point(104, 129);
            this.lbl_PassWord.Name = "lbl_PassWord";
            this.lbl_PassWord.Size = new System.Drawing.Size(105, 30);
            this.lbl_PassWord.TabIndex = 19;
            this.lbl_PassWord.Text = "PassWord:";
            this.lbl_PassWord.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_RealName
            // 
            this.txt_RealName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_RealName.ForeColor = System.Drawing.Color.Chartreuse;
            this.txt_RealName.Location = new System.Drawing.Point(215, 73);
            this.txt_RealName.MaxLength = 15;
            this.txt_RealName.Name = "txt_RealName";
            this.txt_RealName.Size = new System.Drawing.Size(178, 32);
            this.txt_RealName.TabIndex = 18;
            this.txt_RealName.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_RealName.GetChildAt(0))).ForeColor = System.Drawing.Color.Chartreuse;
            // 
            // lbl_RealName
            // 
            this.lbl_RealName.AutoSize = false;
            this.lbl_RealName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RealName.ForeColor = System.Drawing.Color.White;
            this.lbl_RealName.Location = new System.Drawing.Point(101, 72);
            this.lbl_RealName.Name = "lbl_RealName";
            this.lbl_RealName.Size = new System.Drawing.Size(108, 30);
            this.lbl_RealName.TabIndex = 17;
            this.lbl_RealName.Text = "RealName:";
            this.lbl_RealName.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // txt_LoginName
            // 
            this.txt_LoginName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_LoginName.ForeColor = System.Drawing.Color.Chartreuse;
            this.txt_LoginName.Location = new System.Drawing.Point(215, 22);
            this.txt_LoginName.MaxLength = 15;
            this.txt_LoginName.Name = "txt_LoginName";
            this.txt_LoginName.Size = new System.Drawing.Size(178, 32);
            this.txt_LoginName.TabIndex = 16;
            this.txt_LoginName.ThemeName = "VisualStudio2012Dark";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_LoginName.GetChildAt(0))).ForeColor = System.Drawing.Color.Chartreuse;
            // 
            // lbl_LoginName
            // 
            this.lbl_LoginName.AutoSize = false;
            this.lbl_LoginName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_LoginName.ForeColor = System.Drawing.Color.White;
            this.lbl_LoginName.Location = new System.Drawing.Point(89, 22);
            this.lbl_LoginName.Name = "lbl_LoginName";
            this.lbl_LoginName.Size = new System.Drawing.Size(120, 30);
            this.lbl_LoginName.TabIndex = 15;
            this.lbl_LoginName.Text = "UserName:";
            this.lbl_LoginName.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(175, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 18;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(331, 437);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(108, 33);
            this.btn_Cancel.TabIndex = 31;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.Remove_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_OK.Location = new System.Drawing.Point(179, 437);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(108, 33);
            this.btn_OK.TabIndex = 32;
            this.btn_OK.Text = "OK";
            this.btn_OK.ThemeName = "VisualStudio2012Dark";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // AddUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 489);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lbl_AddUser);
            this.Controls.Add(this.radPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUserForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddUserForm";
            this.ThemeName = "VisualStudio2012Dark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddUserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbl_AddUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel)).EndInit();
            this.radPanel.ResumeLayout(false);
            this.radPanel.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.gbx_permission.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_PassWordAgain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_PassWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_permission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWordAgain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_PassWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_RealName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_RealName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_LoginName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_LoginName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel lbl_AddUser;
        private Telerik.WinControls.UI.RadPanel radPanel;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel;
        private Telerik.WinControls.UI.RadLabel lbl_PassWordAgain;
        private Telerik.WinControls.UI.RadLabel lbl_PassWord;
        private Telerik.WinControls.UI.RadTextBox txt_RealName;
        private Telerik.WinControls.UI.RadLabel lbl_RealName;
        private Telerik.WinControls.UI.RadTextBox txt_LoginName;
        private Telerik.WinControls.UI.RadLabel lbl_LoginName;
        private Telerik.WinControls.UI.RadTextBox txt_PassWord;
        private Telerik.WinControls.UI.RadTextBox txt_PassWordAgain;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox gbx_permission;
        private System.Windows.Forms.RadioButton rbtn_p2;
        private System.Windows.Forms.RadioButton rbtn_p1;
        private Telerik.WinControls.UI.RadLabel rlbl_permission;
        private Telerik.WinControls.UI.RadButton btn_OK;
    }
}