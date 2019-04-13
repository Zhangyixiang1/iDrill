namespace DrillingSymtemCSCV2.Forms
{
    partial class ActivityStatusForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActivityStatusForm));
            this.tab_tag = new System.Windows.Forms.TabControl();
            this.AcitvityStatusPage = new System.Windows.Forms.TabPage();
            this.btn_Select = new Telerik.WinControls.UI.RadButton();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.bkg_LoadData = new System.ComponentModel.BackgroundWorker();
            this.tab_tag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tab_tag
            // 
            this.tab_tag.Controls.Add(this.AcitvityStatusPage);
            this.tab_tag.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_tag.Location = new System.Drawing.Point(15, 4);
            this.tab_tag.Name = "tab_tag";
            this.tab_tag.SelectedIndex = 0;
            this.tab_tag.Size = new System.Drawing.Size(832, 595);
            this.tab_tag.TabIndex = 4;
            // 
            // AcitvityStatusPage
            // 
            this.AcitvityStatusPage.AutoScroll = true;
            this.AcitvityStatusPage.BackColor = System.Drawing.Color.Black;
            this.AcitvityStatusPage.Location = new System.Drawing.Point(4, 26);
            this.AcitvityStatusPage.Name = "AcitvityStatusPage";
            this.AcitvityStatusPage.Size = new System.Drawing.Size(824, 565);
            this.AcitvityStatusPage.TabIndex = 4;
            this.AcitvityStatusPage.Text = "Activity";
            // 
            // btn_Select
            // 
            this.btn_Select.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_Select.Location = new System.Drawing.Point(195, 639);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(181, 56);
            this.btn_Select.TabIndex = 5;
            this.btn_Select.Text = "Select";
            this.btn_Select.ThemeName = "VisualStudio2012Dark";
            this.btn_Select.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_Cancel.Location = new System.Drawing.Point(485, 639);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(181, 56);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // bkg_LoadData
            // 
            this.bkg_LoadData.WorkerSupportsCancellation = true;
            this.bkg_LoadData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bkg_LoadData_DoWork);
            this.bkg_LoadData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bkg_LoadData_RunWorkerCompleted);
            // 
            // ActivityStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 712);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Select);
            this.Controls.Add(this.tab_tag);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ActivityStatusForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "ActivityStatus";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.ActivityStatusForm_Load);
            this.tab_tag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab_tag;
        private System.Windows.Forms.TabPage AcitvityStatusPage;
        private Telerik.WinControls.UI.RadButton btn_Select;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.ComponentModel.BackgroundWorker bkg_LoadData;
    }
}