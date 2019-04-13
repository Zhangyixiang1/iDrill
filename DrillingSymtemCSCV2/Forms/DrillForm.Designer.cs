namespace DrillingSymtemCSCV2.Forms
{
    partial class DrillForm
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
            Telerik.WinControls.ThemeSource themeSource1 = new Telerik.WinControls.ThemeSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrillForm));
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.mediaShape1 = new Telerik.WinControls.Tests.MediaShape();
            this.donutShape1 = new Telerik.WinControls.Tests.DonutShape();
            this.officeShape1 = new Telerik.WinControls.UI.OfficeShape();
            this.qaShape1 = new Telerik.WinControls.Tests.QAShape();
            this.tabIEShape1 = new Telerik.WinControls.UI.TabIEShape();
            this.tabOffice12Shape1 = new Telerik.WinControls.UI.TabOffice12Shape();
            this.tabVsShape1 = new Telerik.WinControls.UI.TabVsShape();
            this.trackBarDThumbShape1 = new Telerik.WinControls.UI.TrackBarDThumbShape();
            this.heartShape1 = new Telerik.WinControls.UI.HeartShape();
            this.diamondShape1 = new Telerik.WinControls.UI.DiamondShape();
            this.ellipseShape1 = new Telerik.WinControls.EllipseShape();
            this.fourChart1 = new DrillingSymtemCSCV2.UserControls.FourChart();
            this.fourChart3 = new DrillingSymtemCSCV2.UserControls.FourChart();
            this.btn_Real = new Telerik.WinControls.UI.RadButton();
            this.btn_Down = new Telerik.WinControls.UI.RadButton();
            this.btn_Up = new Telerik.WinControls.UI.RadButton();
            this.fourChart2 = new DrillingSymtemCSCV2.UserControls.FourChart();
            this.dataShowControl1 = new DrillingSymtemCSCV2.UserControls.DataShowControl();
            this.btn_Enlarge = new Telerik.WinControls.UI.RadButton();
            this.btn_Narrow = new Telerik.WinControls.UI.RadButton();
            this.radButton6 = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.depthTimeChart = new DrillingSymtemCSCV2.UserControls.DepthTimeChart();
            this.btn_PageUp = new Telerik.WinControls.UI.RadButton();
            this.btn_PageDown = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Real)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Up)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Enlarge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Narrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_PageUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_PageDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radThemeManager1
            // 
            themeSource1.StorageType = Telerik.WinControls.ThemeStorageType.Resource;
            themeSource1.ThemeLocation = "visualStudio2012DarkTheme1";
            this.radThemeManager1.LoadedThemes.AddRange(new Telerik.WinControls.ThemeSource[] {
            themeSource1});
            // 
            // fourChart1
            // 
            this.fourChart1.BackColor = System.Drawing.Color.Transparent;
            this.fourChart1.fname = null;
            this.fourChart1.group = 0;
            this.fourChart1.Location = new System.Drawing.Point(9, 119);
            this.fourChart1.Name = "fourChart1";
            this.fourChart1.Size = new System.Drawing.Size(400, 890);
            this.fourChart1.TabIndex = 29;
            // 
            // fourChart3
            // 
            this.fourChart3.AutoSize = true;
            this.fourChart3.BackColor = System.Drawing.Color.Transparent;
            this.fourChart3.fname = null;
            this.fourChart3.group = 0;
            this.fourChart3.Location = new System.Drawing.Point(1147, 119);
            this.fourChart3.Name = "fourChart3";
            this.fourChart3.Size = new System.Drawing.Size(400, 895);
            this.fourChart3.TabIndex = 31;
            // 
            // btn_Real
            // 
            this.btn_Real.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btn_Real.Location = new System.Drawing.Point(1575, 420);
            this.btn_Real.Name = "btn_Real";
            this.btn_Real.Size = new System.Drawing.Size(73, 54);
            this.btn_Real.TabIndex = 34;
            this.btn_Real.Text = "R/T";
            this.btn_Real.ThemeName = "VisualStudio2012Dark";
            this.btn_Real.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_Down.Location = new System.Drawing.Point(1575, 268);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Size = new System.Drawing.Size(73, 54);
            this.btn_Down.TabIndex = 35;
            this.btn_Down.Text = "▼";
            this.btn_Down.ThemeName = "VisualStudio2012Dark";
            this.btn_Down.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // btn_Up
            // 
            this.btn_Up.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_Up.Location = new System.Drawing.Point(1576, 194);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Size = new System.Drawing.Size(73, 54);
            this.btn_Up.TabIndex = 36;
            this.btn_Up.Text = "▲";
            this.btn_Up.ThemeName = "VisualStudio2012Dark";
            this.btn_Up.Click += new System.EventHandler(this.radButtonUp_Click);
            // 
            // fourChart2
            // 
            this.fourChart2.AutoSize = true;
            this.fourChart2.BackColor = System.Drawing.Color.Transparent;
            this.fourChart2.fname = null;
            this.fourChart2.group = 0;
            this.fourChart2.Location = new System.Drawing.Point(715, 119);
            this.fourChart2.Name = "fourChart2";
            this.fourChart2.Size = new System.Drawing.Size(400, 895);
            this.fourChart2.TabIndex = 32;
            // 
            // dataShowControl1
            // 
            this.dataShowControl1.drillID = 0;
            this.dataShowControl1.fname = null;
            this.dataShowControl1.group = 0;
            this.dataShowControl1.Location = new System.Drawing.Point(1554, 500);
            this.dataShowControl1.Name = "dataShowControl1";
            this.dataShowControl1.Size = new System.Drawing.Size(210, 413);
            this.dataShowControl1.TabIndex = 42;
            // 
            // btn_Enlarge
            // 
            this.btn_Enlarge.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.btn_Enlarge.Image = global::DrillingSymtemCSCV2.Properties.Resources.enlarge;
            this.btn_Enlarge.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Enlarge.Location = new System.Drawing.Point(1675, 168);
            this.btn_Enlarge.Name = "btn_Enlarge";
            this.btn_Enlarge.Size = new System.Drawing.Size(73, 52);
            this.btn_Enlarge.TabIndex = 36;
            this.btn_Enlarge.ThemeName = "VisualStudio2012Dark";
            this.btn_Enlarge.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // btn_Narrow
            // 
            this.btn_Narrow.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.btn_Narrow.Image = global::DrillingSymtemCSCV2.Properties.Resources.sorrow;
            this.btn_Narrow.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btn_Narrow.Location = new System.Drawing.Point(1675, 299);
            this.btn_Narrow.Name = "btn_Narrow";
            this.btn_Narrow.Size = new System.Drawing.Size(73, 54);
            this.btn_Narrow.TabIndex = 35;
            this.btn_Narrow.ThemeName = "VisualStudio2012Dark";
            this.btn_Narrow.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // radButton6
            // 
            this.radButton6.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.radButton6.Location = new System.Drawing.Point(1675, 420);
            this.radButton6.Name = "radButton6";
            this.radButton6.Size = new System.Drawing.Size(73, 54);
            this.radButton6.TabIndex = 34;
            this.radButton6.Text = "Print";
            this.radButton6.ThemeName = "VisualStudio2012Dark";
            this.radButton6.Click += new System.EventHandler(this.radButton6_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 5000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Enabled = true;
            this.timer3.Interval = 10000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // depthTimeChart
            // 
            this.depthTimeChart.d_now = 0D;
            this.depthTimeChart.Location = new System.Drawing.Point(411, 119);
            this.depthTimeChart.Name = "depthTimeChart";
            this.depthTimeChart.Size = new System.Drawing.Size(300, 842);
            this.depthTimeChart.TabIndex = 33;
            // 
            // btn_PageUp
            // 
            this.btn_PageUp.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn_PageUp.Location = new System.Drawing.Point(1576, 134);
            this.btn_PageUp.Name = "btn_PageUp";
            this.btn_PageUp.Size = new System.Drawing.Size(73, 54);
            this.btn_PageUp.TabIndex = 37;
            this.btn_PageUp.Text = "<html><p>▲</p><p>▲</p></html>";
            this.btn_PageUp.ThemeName = "VisualStudio2012Dark";
            this.btn_PageUp.Click += new System.EventHandler(this.radButton7_Click);
            // 
            // btn_PageDown
            // 
            this.btn_PageDown.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.btn_PageDown.Location = new System.Drawing.Point(1576, 327);
            this.btn_PageDown.Name = "btn_PageDown";
            this.btn_PageDown.Size = new System.Drawing.Size(73, 54);
            this.btn_PageDown.TabIndex = 36;
            this.btn_PageDown.Text = "<html><p>▼</p><p>▼</p></html>";
            this.btn_PageDown.ThemeName = "VisualStudio2012Dark";
            this.btn_PageDown.Click += new System.EventHandler(this.radButton8_Click);
            // 
            // DrillForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1912, 1047);
            this.Controls.Add(this.btn_PageDown);
            this.Controls.Add(this.btn_PageUp);
            this.Controls.Add(this.radButton6);
            this.Controls.Add(this.dataShowControl1);
            this.Controls.Add(this.btn_Real);
            this.Controls.Add(this.btn_Narrow);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Enlarge);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.depthTimeChart);
            this.Controls.Add(this.fourChart3);
            this.Controls.Add(this.fourChart1);
            this.Controls.Add(this.fourChart2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DrillForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1920, 1080);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "iDrill 2.0";
            this.Activated += new System.EventHandler(this.DrillForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Controls.SetChildIndex(this.dataShow21, 0);
            this.Controls.SetChildIndex(this.dataShow22, 0);
            this.Controls.SetChildIndex(this.dataShow23, 0);
            this.Controls.SetChildIndex(this.dataShow24, 0);
            this.Controls.SetChildIndex(this.dataShow25, 0);
            this.Controls.SetChildIndex(this.fourChart2, 0);
            this.Controls.SetChildIndex(this.fourChart1, 0);
            this.Controls.SetChildIndex(this.fourChart3, 0);
            this.Controls.SetChildIndex(this.depthTimeChart, 0);
            this.Controls.SetChildIndex(this.btn_Up, 0);
            this.Controls.SetChildIndex(this.btn_Enlarge, 0);
            this.Controls.SetChildIndex(this.btn_Down, 0);
            this.Controls.SetChildIndex(this.btn_Narrow, 0);
            this.Controls.SetChildIndex(this.btn_Real, 0);
            this.Controls.SetChildIndex(this.dataShowControl1, 0);
            this.Controls.SetChildIndex(this.radButton6, 0);
            this.Controls.SetChildIndex(this.btn_PageUp, 0);
            this.Controls.SetChildIndex(this.btn_PageDown, 0);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Real)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Up)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Enlarge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Narrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_PageUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_PageDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.RadThemeManager radThemeManager1;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.Tests.MediaShape mediaShape1;
        private Telerik.WinControls.Tests.DonutShape donutShape1;
        private Telerik.WinControls.UI.OfficeShape officeShape1;
        private Telerik.WinControls.Tests.QAShape qaShape1;
        private Telerik.WinControls.UI.TabIEShape tabIEShape1;
        private Telerik.WinControls.UI.TabOffice12Shape tabOffice12Shape1;
        private Telerik.WinControls.UI.TabVsShape tabVsShape1;
        private Telerik.WinControls.UI.TrackBarDThumbShape trackBarDThumbShape1;
        private Telerik.WinControls.UI.HeartShape heartShape1;
        private Telerik.WinControls.UI.DiamondShape diamondShape1;
        private Telerik.WinControls.EllipseShape ellipseShape1;
        private UserControls.FourChart fourChart3;
        private Telerik.WinControls.UI.RadButton btn_Real;
        private Telerik.WinControls.UI.RadButton btn_Down;
        private Telerik.WinControls.UI.RadButton btn_Up;
        private UserControls.FourChart fourChart2;
        private UserControls.DataShowControl dataShowControl1;
        public UserControls.FourChart fourChart1;
        private Telerik.WinControls.UI.RadButton btn_Enlarge;
        private Telerik.WinControls.UI.RadButton btn_Narrow;
        private Telerik.WinControls.UI.RadButton radButton6;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private UserControls.DepthTimeChart depthTimeChart;
        private Telerik.WinControls.UI.RadButton btn_PageUp;
        private Telerik.WinControls.UI.RadButton btn_PageDown;
    }
}