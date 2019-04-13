namespace DrillingSymtemCSCV2.Forms
{
    partial class ResetForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetForm));
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rbtn_Type2 = new System.Windows.Forms.RadioButton();
            this.rbtn_Type1 = new System.Windows.Forms.RadioButton();
            this.lbl_geoinfo = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_geoinfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.rbtn_OK.Location = new System.Drawing.Point(134, 114);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(70, 25);
            this.rbtn_OK.TabIndex = 32;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.rbtn_OK_Click);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.rbtn_Type2);
            this.radPanel1.Controls.Add(this.rbtn_Type1);
            this.radPanel1.Location = new System.Drawing.Point(12, 15);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(318, 90);
            this.radPanel1.TabIndex = 41;
            // 
            // rbtn_Type2
            // 
            this.rbtn_Type2.AutoSize = true;
            this.rbtn_Type2.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtn_Type2.ForeColor = System.Drawing.Color.White;
            this.rbtn_Type2.Location = new System.Drawing.Point(6, 59);
            this.rbtn_Type2.Name = "rbtn_Type2";
            this.rbtn_Type2.Size = new System.Drawing.Size(42, 23);
            this.rbtn_Type2.TabIndex = 0;
            this.rbtn_Type2.TabStop = true;
            this.rbtn_Type2.Text = "All";
            this.rbtn_Type2.UseVisualStyleBackColor = true;
            // 
            // rbtn_Type1
            // 
            this.rbtn_Type1.AutoSize = true;
            this.rbtn_Type1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rbtn_Type1.ForeColor = System.Drawing.Color.White;
            this.rbtn_Type1.Location = new System.Drawing.Point(6, 16);
            this.rbtn_Type1.Name = "rbtn_Type1";
            this.rbtn_Type1.Size = new System.Drawing.Size(74, 23);
            this.rbtn_Type1.TabIndex = 0;
            this.rbtn_Type1.TabStop = true;
            this.rbtn_Type1.Text = "Current";
            this.rbtn_Type1.UseVisualStyleBackColor = true;
            // 
            // lbl_geoinfo
            // 
            this.lbl_geoinfo.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_geoinfo.ForeColor = System.Drawing.Color.White;
            this.lbl_geoinfo.Location = new System.Drawing.Point(17, 3);
            this.lbl_geoinfo.Name = "lbl_geoinfo";
            this.lbl_geoinfo.Size = new System.Drawing.Size(127, 22);
            this.lbl_geoinfo.TabIndex = 37;
            this.lbl_geoinfo.Text = "选择测点重置类型:";
            // 
            // ResetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(345, 140);
            this.Controls.Add(this.lbl_geoinfo);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.radPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "iDrill2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ResetForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingForm2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_geoinfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.RadioButton rbtn_Type2;
        private System.Windows.Forms.RadioButton rbtn_Type1;
        private Telerik.WinControls.UI.RadLabel lbl_geoinfo;
    }
}
