namespace DrillingSymtemCSCV2.Forms
{
    partial class PitStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PitStatus));
            this.rlbl_pit = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_pitstatus = new Telerik.WinControls.UI.RadLabel();
            this.rbtn_enable = new Telerik.WinControls.UI.RadButton();
            this.rbtn_disable = new Telerik.WinControls.UI.RadButton();
            this.rlbl_pitName = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_pitValue = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitstatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_disable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlbl_pit
            // 
            this.rlbl_pit.AutoSize = false;
            this.rlbl_pit.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_pit.ForeColor = System.Drawing.Color.White;
            this.rlbl_pit.Location = new System.Drawing.Point(25, 25);
            this.rlbl_pit.Name = "rlbl_pit";
            this.rlbl_pit.Size = new System.Drawing.Size(120, 25);
            this.rlbl_pit.TabIndex = 2;
            this.rlbl_pit.Text = "Pit：";
            this.rlbl_pit.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rlbl_pitstatus
            // 
            this.rlbl_pitstatus.AutoSize = false;
            this.rlbl_pitstatus.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_pitstatus.ForeColor = System.Drawing.Color.White;
            this.rlbl_pitstatus.Location = new System.Drawing.Point(25, 56);
            this.rlbl_pitstatus.Name = "rlbl_pitstatus";
            this.rlbl_pitstatus.Size = new System.Drawing.Size(120, 25);
            this.rlbl_pitstatus.TabIndex = 2;
            this.rlbl_pitstatus.Text = "Pit Status：";
            this.rlbl_pitstatus.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rbtn_enable
            // 
            this.rbtn_enable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_enable.Location = new System.Drawing.Point(64, 112);
            this.rbtn_enable.Name = "rbtn_enable";
            this.rbtn_enable.Size = new System.Drawing.Size(108, 33);
            this.rbtn_enable.TabIndex = 32;
            this.rbtn_enable.Text = "Enable";
            this.rbtn_enable.ThemeName = "VisualStudio2012Dark";
            this.rbtn_enable.Click += new System.EventHandler(this.rbtn_enable_Click);
            // 
            // rbtn_disable
            // 
            this.rbtn_disable.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_disable.Location = new System.Drawing.Point(214, 112);
            this.rbtn_disable.Name = "rbtn_disable";
            this.rbtn_disable.Size = new System.Drawing.Size(108, 33);
            this.rbtn_disable.TabIndex = 33;
            this.rbtn_disable.Text = "Disable";
            this.rbtn_disable.ThemeName = "VisualStudio2012Dark";
            this.rbtn_disable.Click += new System.EventHandler(this.rbtn_disable_Click);
            // 
            // rlbl_pitName
            // 
            this.rlbl_pitName.AutoSize = false;
            this.rlbl_pitName.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.rlbl_pitName.ForeColor = System.Drawing.Color.White;
            this.rlbl_pitName.Location = new System.Drawing.Point(147, 25);
            this.rlbl_pitName.Name = "rlbl_pitName";
            this.rlbl_pitName.Size = new System.Drawing.Size(216, 25);
            this.rlbl_pitName.TabIndex = 34;
            this.rlbl_pitName.Text = "Pit  Volume 1";
            this.rlbl_pitName.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rlbl_pitValue
            // 
            this.rlbl_pitValue.AutoSize = false;
            this.rlbl_pitValue.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.rlbl_pitValue.ForeColor = System.Drawing.Color.White;
            this.rlbl_pitValue.Location = new System.Drawing.Point(147, 56);
            this.rlbl_pitValue.Name = "rlbl_pitValue";
            this.rlbl_pitValue.Size = new System.Drawing.Size(216, 25);
            this.rlbl_pitValue.TabIndex = 35;
            this.rlbl_pitValue.Text = "Enable";
            this.rlbl_pitValue.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // PitStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 158);
            this.Controls.Add(this.rlbl_pitValue);
            this.Controls.Add(this.rlbl_pitName);
            this.Controls.Add(this.rbtn_disable);
            this.Controls.Add(this.rbtn_enable);
            this.Controls.Add(this.rlbl_pitstatus);
            this.Controls.Add(this.rlbl_pit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PitStatus";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.PitStatus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitstatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_disable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_pitValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel rlbl_pit;
        private Telerik.WinControls.UI.RadLabel rlbl_pitstatus;
        private Telerik.WinControls.UI.RadButton rbtn_enable;
        private Telerik.WinControls.UI.RadButton rbtn_disable;
        private Telerik.WinControls.UI.RadLabel rlbl_pitName;
        private Telerik.WinControls.UI.RadLabel rlbl_pitValue;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
    }
}