namespace DrillingSymtemCSCV2.Forms
{
    partial class WITSForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WITSForm));
            this.rlbl_port = new Telerik.WinControls.UI.RadLabel();
            this.rdp_port = new Telerik.WinControls.UI.RadDropDownList();
            this.rlbl_baudrate = new Telerik.WinControls.UI.RadLabel();
            this.rdp_baudrate = new Telerik.WinControls.UI.RadDropDownList();
            this.rbtn_send = new Telerik.WinControls.UI.RadButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.lbl_status = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdp_port)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_baudrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdp_baudrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_send)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlbl_port
            // 
            this.rlbl_port.AutoSize = false;
            this.rlbl_port.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_port.ForeColor = System.Drawing.Color.White;
            this.rlbl_port.Location = new System.Drawing.Point(69, 13);
            this.rlbl_port.Name = "rlbl_port";
            this.rlbl_port.Size = new System.Drawing.Size(82, 25);
            this.rlbl_port.TabIndex = 2;
            this.rlbl_port.Text = "Port：";
            this.rlbl_port.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rdp_port
            // 
            this.rdp_port.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.rdp_port.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rdp_port.ForeColor = System.Drawing.Color.Lime;
            this.rdp_port.Location = new System.Drawing.Point(154, 13);
            this.rdp_port.Name = "rdp_port";
            this.rdp_port.Size = new System.Drawing.Size(145, 28);
            this.rdp_port.TabIndex = 30;
            this.rdp_port.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_baudrate
            // 
            this.rlbl_baudrate.AutoSize = false;
            this.rlbl_baudrate.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_baudrate.ForeColor = System.Drawing.Color.White;
            this.rlbl_baudrate.Location = new System.Drawing.Point(33, 65);
            this.rlbl_baudrate.Name = "rlbl_baudrate";
            this.rlbl_baudrate.Size = new System.Drawing.Size(118, 25);
            this.rlbl_baudrate.TabIndex = 2;
            this.rlbl_baudrate.Text = "Baudrate：";
            this.rlbl_baudrate.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rdp_baudrate
            // 
            this.rdp_baudrate.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.rdp_baudrate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rdp_baudrate.ForeColor = System.Drawing.Color.Lime;
            this.rdp_baudrate.Location = new System.Drawing.Point(154, 65);
            this.rdp_baudrate.Name = "rdp_baudrate";
            this.rdp_baudrate.Size = new System.Drawing.Size(147, 28);
            this.rdp_baudrate.TabIndex = 30;
            this.rdp_baudrate.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_send
            // 
            this.rbtn_send.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_send.Location = new System.Drawing.Point(84, 124);
            this.rbtn_send.Name = "rbtn_send";
            this.rbtn_send.Size = new System.Drawing.Size(108, 33);
            this.rbtn_send.TabIndex = 31;
            this.rbtn_send.Text = "Send";
            this.rbtn_send.ThemeName = "VisualStudio2012Dark";
            this.rbtn_send.Click += new System.EventHandler(this.rbtn_send_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            // lbl_status
            // 
            this.lbl_status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_status.BackColor = System.Drawing.Color.Transparent;
            this.lbl_status.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.lbl_status.ForeColor = System.Drawing.Color.White;
            this.lbl_status.Location = new System.Drawing.Point(225, 114);
            this.lbl_status.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(52, 52);
            this.lbl_status.TabIndex = 32;
            this.lbl_status.Text = "●";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // WITSForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 182);
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.rbtn_send);
            this.Controls.Add(this.rdp_baudrate);
            this.Controls.Add(this.rdp_port);
            this.Controls.Add(this.rlbl_baudrate);
            this.Controls.Add(this.rlbl_port);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WITSForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WITSForm_FormClosed);
            this.Load += new System.EventHandler(this.WITSForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdp_port)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_baudrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdp_baudrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_send)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel rlbl_port;
        private Telerik.WinControls.UI.RadDropDownList rdp_port;
        private Telerik.WinControls.UI.RadLabel rlbl_baudrate;
        private Telerik.WinControls.UI.RadDropDownList rdp_baudrate;
        private Telerik.WinControls.UI.RadButton rbtn_send;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Label lbl_status;
    }
}