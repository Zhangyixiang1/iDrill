namespace DrillingSymtemCSCV2.Forms
{
    partial class UserManagement
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagement));
            this.rgv_users = new Telerik.WinControls.UI.RadGridView();
            this.rbtn_addUser = new Telerik.WinControls.UI.RadButton();
            this.rbtn_editUser = new Telerik.WinControls.UI.RadButton();
            this.rbtn_deleteUser = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_refresh = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_users)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_users.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_addUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_editUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_deleteUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_refresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rgv_users
            // 
            this.rgv_users.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rgv_users.Location = new System.Drawing.Point(2, 76);
            this.rgv_users.Margin = new System.Windows.Forms.Padding(2);
            // 
            // rgv_users
            // 
            this.rgv_users.MasterTemplate.AllowAddNewRow = false;
            this.rgv_users.MasterTemplate.AllowColumnReorder = false;
            this.rgv_users.MasterTemplate.AllowColumnResize = false;
            this.rgv_users.MasterTemplate.AllowDeleteRow = false;
            this.rgv_users.MasterTemplate.AllowEditRow = false;
            this.rgv_users.MasterTemplate.AllowRowResize = false;
            gridViewTextBoxColumn1.HeaderText = "UserName";
            gridViewTextBoxColumn1.Name = "UserName";
            gridViewTextBoxColumn1.Width = 250;
            gridViewTextBoxColumn2.HeaderText = "RealName";
            gridViewTextBoxColumn2.Name = "RealName";
            gridViewTextBoxColumn2.Width = 250;
            gridViewTextBoxColumn3.HeaderText = "Permission";
            gridViewTextBoxColumn3.Name = "Permission";
            gridViewTextBoxColumn3.Width = 170;
            this.rgv_users.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.rgv_users.MasterTemplate.EnableSorting = false;
            this.rgv_users.MasterTemplate.ShowRowHeaderColumn = false;
            this.rgv_users.Name = "rgv_users";
            this.rgv_users.ShowGroupPanel = false;
            this.rgv_users.Size = new System.Drawing.Size(692, 357);
            this.rgv_users.TabIndex = 34;
            this.rgv_users.Text = "rgv_showAlarm";
            this.rgv_users.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_addUser
            // 
            this.rbtn_addUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_addUser.Location = new System.Drawing.Point(241, 15);
            this.rbtn_addUser.Name = "rbtn_addUser";
            this.rbtn_addUser.Size = new System.Drawing.Size(118, 33);
            this.rbtn_addUser.TabIndex = 35;
            this.rbtn_addUser.Text = "Add User";
            this.rbtn_addUser.ThemeName = "VisualStudio2012Dark";
            this.rbtn_addUser.Click += new System.EventHandler(this.rbtn_addUser_Click);
            // 
            // rbtn_editUser
            // 
            this.rbtn_editUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_editUser.Location = new System.Drawing.Point(400, 15);
            this.rbtn_editUser.Name = "rbtn_editUser";
            this.rbtn_editUser.Size = new System.Drawing.Size(118, 33);
            this.rbtn_editUser.TabIndex = 35;
            this.rbtn_editUser.Text = "Edit User";
            this.rbtn_editUser.ThemeName = "VisualStudio2012Dark";
            this.rbtn_editUser.Click += new System.EventHandler(this.rbtn_editUser_Click);
            // 
            // rbtn_deleteUser
            // 
            this.rbtn_deleteUser.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_deleteUser.Location = new System.Drawing.Point(569, 15);
            this.rbtn_deleteUser.Name = "rbtn_deleteUser";
            this.rbtn_deleteUser.Size = new System.Drawing.Size(118, 33);
            this.rbtn_deleteUser.TabIndex = 35;
            this.rbtn_deleteUser.Text = "Delete User";
            this.rbtn_deleteUser.ThemeName = "VisualStudio2012Dark";
            this.rbtn_deleteUser.Click += new System.EventHandler(this.rbtn_deleteUser_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // rbtn_refresh
            // 
            this.rbtn_refresh.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_refresh.Location = new System.Drawing.Point(3, 15);
            this.rbtn_refresh.Name = "rbtn_refresh";
            this.rbtn_refresh.Size = new System.Drawing.Size(118, 33);
            this.rbtn_refresh.TabIndex = 35;
            this.rbtn_refresh.Text = "Refresh";
            this.rbtn_refresh.ThemeName = "VisualStudio2012Dark";
            this.rbtn_refresh.Click += new System.EventHandler(this.rbtn_refresh_Click);
            // 
            // UserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 465);
            this.Controls.Add(this.rbtn_deleteUser);
            this.Controls.Add(this.rbtn_editUser);
            this.Controls.Add(this.rbtn_refresh);
            this.Controls.Add(this.rbtn_addUser);
            this.Controls.Add(this.rgv_users);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UserManagement";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.UserManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rgv_users.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_users)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_addUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_editUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_deleteUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_refresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView rgv_users;
        private Telerik.WinControls.UI.RadButton rbtn_addUser;
        private Telerik.WinControls.UI.RadButton rbtn_editUser;
        private Telerik.WinControls.UI.RadButton rbtn_deleteUser;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_refresh;
    }
}