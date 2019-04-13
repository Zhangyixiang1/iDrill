namespace DrillingSymtemCSCV2.Forms
{
    partial class WorkerManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkerManagement));
            this.rbtn_deleteWorker = new Telerik.WinControls.UI.RadButton();
            this.rbtn_editWorker = new Telerik.WinControls.UI.RadButton();
            this.rbtn_addWorker = new Telerik.WinControls.UI.RadButton();
            this.rgv_workers = new Telerik.WinControls.UI.RadGridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_Select = new Telerik.WinControls.UI.RadButton();
            this.rbtn_Cancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_deleteWorker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_editWorker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_addWorker)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_workers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_workers.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtn_deleteWorker
            // 
            this.rbtn_deleteWorker.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_deleteWorker.Location = new System.Drawing.Point(394, 16);
            this.rbtn_deleteWorker.Name = "rbtn_deleteWorker";
            this.rbtn_deleteWorker.Size = new System.Drawing.Size(118, 33);
            this.rbtn_deleteWorker.TabIndex = 37;
            this.rbtn_deleteWorker.Text = "Delete Worker";
            this.rbtn_deleteWorker.ThemeName = "VisualStudio2012Dark";
            this.rbtn_deleteWorker.Click += new System.EventHandler(this.rbtn_deleteWorker_Click);
            // 
            // rbtn_editWorker
            // 
            this.rbtn_editWorker.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_editWorker.Location = new System.Drawing.Point(199, 16);
            this.rbtn_editWorker.Name = "rbtn_editWorker";
            this.rbtn_editWorker.Size = new System.Drawing.Size(118, 33);
            this.rbtn_editWorker.TabIndex = 38;
            this.rbtn_editWorker.Text = "Edit Worker";
            this.rbtn_editWorker.ThemeName = "VisualStudio2012Dark";
            this.rbtn_editWorker.Click += new System.EventHandler(this.rbtn_editWorker_Click);
            // 
            // rbtn_addWorker
            // 
            this.rbtn_addWorker.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_addWorker.Location = new System.Drawing.Point(4, 16);
            this.rbtn_addWorker.Name = "rbtn_addWorker";
            this.rbtn_addWorker.Size = new System.Drawing.Size(118, 33);
            this.rbtn_addWorker.TabIndex = 40;
            this.rbtn_addWorker.Text = "Add Worker";
            this.rbtn_addWorker.ThemeName = "VisualStudio2012Dark";
            this.rbtn_addWorker.Click += new System.EventHandler(this.rbtn_addWorker_Click);
            // 
            // rgv_workers
            // 
            this.rgv_workers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rgv_workers.Location = new System.Drawing.Point(4, 67);
            this.rgv_workers.Margin = new System.Windows.Forms.Padding(2);
            // 
            // rgv_workers
            // 
            this.rgv_workers.MasterTemplate.AllowAddNewRow = false;
            this.rgv_workers.MasterTemplate.AllowColumnReorder = false;
            this.rgv_workers.MasterTemplate.AllowColumnResize = false;
            this.rgv_workers.MasterTemplate.AllowDeleteRow = false;
            this.rgv_workers.MasterTemplate.AllowEditRow = false;
            this.rgv_workers.MasterTemplate.AllowRowResize = false;
            gridViewTextBoxColumn1.HeaderText = "UserName";
            gridViewTextBoxColumn1.Name = "Name";
            gridViewTextBoxColumn1.Width = 180;
            gridViewTextBoxColumn2.HeaderText = "WorkerType";
            gridViewTextBoxColumn2.Name = "WorkerType";
            gridViewTextBoxColumn2.Width = 150;
            gridViewTextBoxColumn3.HeaderText = "EmpNO.";
            gridViewTextBoxColumn3.Name = "EmpNO";
            gridViewTextBoxColumn3.Width = 180;
            this.rgv_workers.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.rgv_workers.MasterTemplate.EnableSorting = false;
            this.rgv_workers.MasterTemplate.ShowRowHeaderColumn = false;
            this.rgv_workers.Name = "rgv_workers";
            this.rgv_workers.ShowGroupPanel = false;
            this.rgv_workers.Size = new System.Drawing.Size(518, 376);
            this.rgv_workers.TabIndex = 36;
            this.rgv_workers.Text = "rgv_showAlarm";
            this.rgv_workers.ThemeName = "VisualStudio2012Dark";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // rbtn_Select
            // 
            this.rbtn_Select.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_Select.Location = new System.Drawing.Point(125, 456);
            this.rbtn_Select.Name = "rbtn_Select";
            this.rbtn_Select.Size = new System.Drawing.Size(118, 33);
            this.rbtn_Select.TabIndex = 38;
            this.rbtn_Select.Text = "Select";
            this.rbtn_Select.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Select.Visible = false;
            this.rbtn_Select.Click += new System.EventHandler(this.rbtn_Select_Click);
            // 
            // rbtn_Cancel
            // 
            this.rbtn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_Cancel.Location = new System.Drawing.Point(270, 454);
            this.rbtn_Cancel.Name = "rbtn_Cancel";
            this.rbtn_Cancel.Size = new System.Drawing.Size(118, 33);
            this.rbtn_Cancel.TabIndex = 38;
            this.rbtn_Cancel.Text = "Cancel";
            this.rbtn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Cancel.Visible = false;
            this.rbtn_Cancel.Click += new System.EventHandler(this.rbtn_Cancel_Click);
            // 
            // WorkerManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 490);
            this.Controls.Add(this.rbtn_deleteWorker);
            this.Controls.Add(this.rbtn_Cancel);
            this.Controls.Add(this.rbtn_Select);
            this.Controls.Add(this.rbtn_editWorker);
            this.Controls.Add(this.rbtn_addWorker);
            this.Controls.Add(this.rgv_workers);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "WorkerManagement";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.WorkerManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_deleteWorker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_editWorker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_addWorker)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_workers.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rgv_workers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton rbtn_deleteWorker;
        private Telerik.WinControls.UI.RadButton rbtn_editWorker;
        private Telerik.WinControls.UI.RadButton rbtn_addWorker;
        private Telerik.WinControls.UI.RadGridView rgv_workers;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_Select;
        private Telerik.WinControls.UI.RadButton rbtn_Cancel;
    }
}