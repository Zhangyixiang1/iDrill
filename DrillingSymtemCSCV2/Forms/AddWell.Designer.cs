namespace DrillingSymtemCSCV2.Forms
{
    partial class AddWellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddWellForm));
            this.rlbl_drillNo = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_drillNo = new Telerik.WinControls.UI.RadTextBox();
            this.rbtn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.rlbl_error = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.rlbl_lease = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_lease = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_country = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_country = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_contractor = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_contractor = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_ds = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_ds = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_tp = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_tp = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_dr = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_dr = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_cm = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_cm = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_company = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_company = new Telerik.WinControls.UI.RadTextBox();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_drillNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_drillNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_error)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_lease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_lease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_country)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_country)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_contractor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_contractor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_ds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_tp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_tp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_dr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_dr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_cm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_cm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_company)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_company)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlbl_drillNo
            // 
            this.rlbl_drillNo.AutoSize = false;
            this.rlbl_drillNo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.rlbl_drillNo.ForeColor = System.Drawing.Color.White;
            this.rlbl_drillNo.Location = new System.Drawing.Point(3, 15);
            this.rlbl_drillNo.Name = "rlbl_drillNo";
            this.rlbl_drillNo.Size = new System.Drawing.Size(180, 35);
            this.rlbl_drillNo.TabIndex = 0;
            this.rlbl_drillNo.Text = "DrillNo：";
            this.rlbl_drillNo.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_drillNo
            // 
            this.rtxt_drillNo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_drillNo.Location = new System.Drawing.Point(189, 15);
            this.rtxt_drillNo.MaxLength = 8;
            this.rtxt_drillNo.Name = "rtxt_drillNo";
            this.rtxt_drillNo.Size = new System.Drawing.Size(147, 28);
            this.rtxt_drillNo.TabIndex = 2;
            this.rtxt_drillNo.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_Cancel
            // 
            this.rbtn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.rbtn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Cancel.Location = new System.Drawing.Point(199, 428);
            this.rbtn_Cancel.Name = "rbtn_Cancel";
            this.rbtn_Cancel.Size = new System.Drawing.Size(83, 30);
            this.rbtn_Cancel.TabIndex = 32;
            this.rbtn_Cancel.Text = "Cancel";
            this.rbtn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Cancel.Click += new System.EventHandler(this.rbtn_Cancel_Click);
            // 
            // rlbl_error
            // 
            this.rlbl_error.AutoSize = false;
            this.rlbl_error.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_error.ForeColor = System.Drawing.Color.Red;
            this.rlbl_error.Location = new System.Drawing.Point(69, 388);
            this.rlbl_error.Name = "rlbl_error";
            this.rlbl_error.Size = new System.Drawing.Size(229, 27);
            this.rlbl_error.TabIndex = 33;
            this.rlbl_error.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // rlbl_lease
            // 
            this.rlbl_lease.AutoSize = false;
            this.rlbl_lease.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_lease.ForeColor = System.Drawing.Color.White;
            this.rlbl_lease.Location = new System.Drawing.Point(3, 56);
            this.rlbl_lease.Name = "rlbl_lease";
            this.rlbl_lease.Size = new System.Drawing.Size(180, 35);
            this.rlbl_lease.TabIndex = 0;
            this.rlbl_lease.Text = "Lease：";
            this.rlbl_lease.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_lease
            // 
            this.rtxt_lease.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_lease.Location = new System.Drawing.Point(189, 56);
            this.rtxt_lease.MaxLength = 8;
            this.rtxt_lease.Name = "rtxt_lease";
            this.rtxt_lease.Size = new System.Drawing.Size(147, 28);
            this.rtxt_lease.TabIndex = 2;
            this.rtxt_lease.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_country
            // 
            this.rlbl_country.AutoSize = false;
            this.rlbl_country.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_country.ForeColor = System.Drawing.Color.White;
            this.rlbl_country.Location = new System.Drawing.Point(3, 138);
            this.rlbl_country.Name = "rlbl_country";
            this.rlbl_country.Size = new System.Drawing.Size(180, 35);
            this.rlbl_country.TabIndex = 0;
            this.rlbl_country.Text = "Country：";
            this.rlbl_country.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_country
            // 
            this.rtxt_country.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_country.Location = new System.Drawing.Point(189, 138);
            this.rtxt_country.MaxLength = 8;
            this.rtxt_country.Name = "rtxt_country";
            this.rtxt_country.Size = new System.Drawing.Size(147, 28);
            this.rtxt_country.TabIndex = 2;
            this.rtxt_country.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_contractor
            // 
            this.rlbl_contractor.AutoSize = false;
            this.rlbl_contractor.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_contractor.ForeColor = System.Drawing.Color.White;
            this.rlbl_contractor.Location = new System.Drawing.Point(3, 179);
            this.rlbl_contractor.Name = "rlbl_contractor";
            this.rlbl_contractor.Size = new System.Drawing.Size(180, 35);
            this.rlbl_contractor.TabIndex = 0;
            this.rlbl_contractor.Text = "Contractor：";
            this.rlbl_contractor.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_contractor
            // 
            this.rtxt_contractor.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_contractor.Location = new System.Drawing.Point(189, 179);
            this.rtxt_contractor.MaxLength = 8;
            this.rtxt_contractor.Name = "rtxt_contractor";
            this.rtxt_contractor.Size = new System.Drawing.Size(147, 28);
            this.rtxt_contractor.TabIndex = 2;
            this.rtxt_contractor.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_ds
            // 
            this.rlbl_ds.AutoSize = false;
            this.rlbl_ds.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_ds.ForeColor = System.Drawing.Color.White;
            this.rlbl_ds.Location = new System.Drawing.Point(3, 220);
            this.rlbl_ds.Name = "rlbl_ds";
            this.rlbl_ds.Size = new System.Drawing.Size(180, 35);
            this.rlbl_ds.TabIndex = 0;
            this.rlbl_ds.Text = "Date Spud：";
            this.rlbl_ds.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_ds
            // 
            this.rtxt_ds.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_ds.Location = new System.Drawing.Point(189, 220);
            this.rtxt_ds.MaxLength = 8;
            this.rtxt_ds.Name = "rtxt_ds";
            this.rtxt_ds.Size = new System.Drawing.Size(147, 28);
            this.rtxt_ds.TabIndex = 2;
            this.rtxt_ds.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_tp
            // 
            this.rlbl_tp.AutoSize = false;
            this.rlbl_tp.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_tp.ForeColor = System.Drawing.Color.White;
            this.rlbl_tp.Location = new System.Drawing.Point(3, 261);
            this.rlbl_tp.Name = "rlbl_tp";
            this.rlbl_tp.Size = new System.Drawing.Size(180, 35);
            this.rlbl_tp.TabIndex = 0;
            this.rlbl_tp.Text = "Tool Pusher：";
            this.rlbl_tp.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_tp
            // 
            this.rtxt_tp.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_tp.Location = new System.Drawing.Point(189, 261);
            this.rtxt_tp.MaxLength = 8;
            this.rtxt_tp.Name = "rtxt_tp";
            this.rtxt_tp.Size = new System.Drawing.Size(147, 28);
            this.rtxt_tp.TabIndex = 2;
            this.rtxt_tp.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_dr
            // 
            this.rlbl_dr.AutoSize = false;
            this.rlbl_dr.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_dr.ForeColor = System.Drawing.Color.White;
            this.rlbl_dr.Location = new System.Drawing.Point(3, 302);
            this.rlbl_dr.Name = "rlbl_dr";
            this.rlbl_dr.Size = new System.Drawing.Size(180, 35);
            this.rlbl_dr.TabIndex = 0;
            this.rlbl_dr.Text = "Date Release：";
            this.rlbl_dr.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_dr
            // 
            this.rtxt_dr.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_dr.Location = new System.Drawing.Point(189, 302);
            this.rtxt_dr.MaxLength = 8;
            this.rtxt_dr.Name = "rtxt_dr";
            this.rtxt_dr.Size = new System.Drawing.Size(147, 28);
            this.rtxt_dr.TabIndex = 2;
            this.rtxt_dr.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_cm
            // 
            this.rlbl_cm.AutoSize = false;
            this.rlbl_cm.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_cm.ForeColor = System.Drawing.Color.White;
            this.rlbl_cm.Location = new System.Drawing.Point(3, 343);
            this.rlbl_cm.Name = "rlbl_cm";
            this.rlbl_cm.Size = new System.Drawing.Size(180, 35);
            this.rlbl_cm.TabIndex = 0;
            this.rlbl_cm.Text = "Company Man：";
            this.rlbl_cm.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_cm
            // 
            this.rtxt_cm.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_cm.Location = new System.Drawing.Point(189, 343);
            this.rtxt_cm.MaxLength = 8;
            this.rtxt_cm.Name = "rtxt_cm";
            this.rtxt_cm.Size = new System.Drawing.Size(147, 28);
            this.rtxt_cm.TabIndex = 2;
            this.rtxt_cm.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_company
            // 
            this.rlbl_company.AutoSize = false;
            this.rlbl_company.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_company.ForeColor = System.Drawing.Color.White;
            this.rlbl_company.Location = new System.Drawing.Point(3, 97);
            this.rlbl_company.Name = "rlbl_company";
            this.rlbl_company.Size = new System.Drawing.Size(180, 35);
            this.rlbl_company.TabIndex = 0;
            this.rlbl_company.Text = "Company：";
            this.rlbl_company.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_company
            // 
            this.rtxt_company.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_company.Location = new System.Drawing.Point(189, 97);
            this.rtxt_company.MaxLength = 8;
            this.rtxt_company.Name = "rtxt_company";
            this.rtxt_company.Size = new System.Drawing.Size(147, 28);
            this.rtxt_company.TabIndex = 2;
            this.rtxt_company.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_OK.Location = new System.Drawing.Point(83, 428);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(93, 30);
            this.rbtn_OK.TabIndex = 34;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.rbtn_OK_Click);
            // 
            // AddWellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 465);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.rlbl_error);
            this.Controls.Add(this.rbtn_Cancel);
            this.Controls.Add(this.rtxt_cm);
            this.Controls.Add(this.rtxt_dr);
            this.Controls.Add(this.rtxt_tp);
            this.Controls.Add(this.rtxt_ds);
            this.Controls.Add(this.rtxt_contractor);
            this.Controls.Add(this.rtxt_country);
            this.Controls.Add(this.rtxt_company);
            this.Controls.Add(this.rtxt_lease);
            this.Controls.Add(this.rtxt_drillNo);
            this.Controls.Add(this.rlbl_cm);
            this.Controls.Add(this.rlbl_dr);
            this.Controls.Add(this.rlbl_tp);
            this.Controls.Add(this.rlbl_ds);
            this.Controls.Add(this.rlbl_contractor);
            this.Controls.Add(this.rlbl_country);
            this.Controls.Add(this.rlbl_company);
            this.Controls.Add(this.rlbl_lease);
            this.Controls.Add(this.rlbl_drillNo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddWellForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.AddWellForm_Load);
            this.Click += new System.EventHandler(this.rbtn_OK_Click);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_drillNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_drillNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_error)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_lease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_lease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_country)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_country)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_contractor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_contractor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_ds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_tp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_tp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_dr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_dr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_cm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_cm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_company)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_company)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel rlbl_drillNo;
        private Telerik.WinControls.UI.RadTextBox rtxt_drillNo;
        private Telerik.WinControls.UI.RadButton rbtn_Cancel;
        private Telerik.WinControls.UI.RadLabel rlbl_error;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Telerik.WinControls.UI.RadLabel rlbl_lease;
        private Telerik.WinControls.UI.RadLabel rlbl_contractor;
        private Telerik.WinControls.UI.RadTextBox rtxt_lease;
        private Telerik.WinControls.UI.RadTextBox rtxt_contractor;
        private Telerik.WinControls.UI.RadLabel rlbl_country;
        private Telerik.WinControls.UI.RadTextBox rtxt_country;
        private Telerik.WinControls.UI.RadTextBox rtxt_ds;
        private Telerik.WinControls.UI.RadLabel rlbl_ds;
        private Telerik.WinControls.UI.RadLabel rlbl_cm;
        private Telerik.WinControls.UI.RadLabel rlbl_tp;
        private Telerik.WinControls.UI.RadTextBox rtxt_tp;
        private Telerik.WinControls.UI.RadTextBox rtxt_cm;
        private Telerik.WinControls.UI.RadLabel rlbl_dr;
        private Telerik.WinControls.UI.RadTextBox rtxt_dr;
        private Telerik.WinControls.UI.RadLabel rlbl_company;
        private Telerik.WinControls.UI.RadTextBox rtxt_company;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
    }
}