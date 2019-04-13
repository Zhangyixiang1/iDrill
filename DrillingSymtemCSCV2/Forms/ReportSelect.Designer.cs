namespace DrillingSymtemCSCV2.Forms
{
    partial class ReportSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportSelect));
            this.rdp_report = new Telerik.WinControls.UI.RadDropDownList();
            this.rlbl_date = new Telerik.WinControls.UI.RadLabel();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            this.rbtn_Cancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rdp_report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_date)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rdp_report
            // 
            this.rdp_report.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.rdp_report.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rdp_report.ForeColor = System.Drawing.Color.Lime;
            this.rdp_report.Location = new System.Drawing.Point(22, 42);
            this.rdp_report.Name = "rdp_report";
            this.rdp_report.Size = new System.Drawing.Size(194, 28);
            this.rdp_report.TabIndex = 46;
            this.rdp_report.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_date
            // 
            this.rlbl_date.AutoSize = false;
            this.rlbl_date.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rlbl_date.ForeColor = System.Drawing.Color.White;
            this.rlbl_date.Location = new System.Drawing.Point(22, 12);
            this.rlbl_date.Name = "rlbl_date";
            this.rlbl_date.Size = new System.Drawing.Size(188, 24);
            this.rlbl_date.TabIndex = 54;
            this.rlbl_date.Text = "Please select report:";
            this.rlbl_date.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtn_OK.Location = new System.Drawing.Point(40, 89);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(71, 31);
            this.rbtn_OK.TabIndex = 55;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.rbtn_OK_Click);
            // 
            // rbtn_Cancel
            // 
            this.rbtn_Cancel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtn_Cancel.Location = new System.Drawing.Point(130, 89);
            this.rbtn_Cancel.Name = "rbtn_Cancel";
            this.rbtn_Cancel.Size = new System.Drawing.Size(71, 31);
            this.rbtn_Cancel.TabIndex = 55;
            this.rbtn_Cancel.Text = "Cancel";
            this.rbtn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Cancel.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // ReportSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(233, 133);
            this.Controls.Add(this.rbtn_Cancel);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.rlbl_date);
            this.Controls.Add(this.rdp_report);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ReportSelect";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.ReportSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rdp_report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_date)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadDropDownList rdp_report;
        private Telerik.WinControls.UI.RadLabel rlbl_date;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
        private Telerik.WinControls.UI.RadButton rbtn_Cancel;
    }
}