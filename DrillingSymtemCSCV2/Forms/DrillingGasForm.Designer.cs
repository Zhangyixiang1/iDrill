namespace DrillingSymtemCSCV2.Forms
{
    partial class DrillingGasForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrillingGasForm));
            this.fourChart1 = new DrillingSymtemCSCV2.UserControls.FourChart();
            this.depthTimeChart1 = new DrillingSymtemCSCV2.UserControls.DepthTimeChart();
            this.fourChart2 = new DrillingSymtemCSCV2.UserControls.FourChart();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.dataShowControl1 = new DrillingSymtemCSCV2.UserControls.DataShowControl();
            this.btn_p = new Telerik.WinControls.UI.RadButton();
            this.btn_zhong = new Telerik.WinControls.UI.RadButton();
            this.btn_jian = new Telerik.WinControls.UI.RadButton();
            this.btn_down = new Telerik.WinControls.UI.RadButton();
            this.btn_add = new Telerik.WinControls.UI.RadButton();
            this.btn_up = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_p)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_zhong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_jian)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_add)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_up)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // fourChart1
            // 
            this.fourChart1.BackColor = System.Drawing.Color.Transparent;
            this.fourChart1.fname = null;
            this.fourChart1.group = 0;
            this.fourChart1.Location = new System.Drawing.Point(318, 83);
            this.fourChart1.Name = "fourChart1";
            this.fourChart1.Size = new System.Drawing.Size(400, 883);
            this.fourChart1.TabIndex = 16;
            // 
            // depthTimeChart1
            // 
            this.depthTimeChart1.d_now = 0D;
            this.depthTimeChart1.Location = new System.Drawing.Point(834, 93);
            this.depthTimeChart1.Name = "depthTimeChart1";
            this.depthTimeChart1.Size = new System.Drawing.Size(220, 802);
            this.depthTimeChart1.TabIndex = 17;
            // 
            // fourChart2
            // 
            this.fourChart2.BackColor = System.Drawing.Color.Transparent;
            this.fourChart2.fname = null;
            this.fourChart2.group = 0;
            this.fourChart2.Location = new System.Drawing.Point(1147, 83);
            this.fourChart2.Name = "fourChart2";
            this.fourChart2.Size = new System.Drawing.Size(400, 889);
            this.fourChart2.TabIndex = 18;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // dataShowControl1
            // 
            this.dataShowControl1.fname = null;
            this.dataShowControl1.group = 0;
            this.dataShowControl1.Location = new System.Drawing.Point(56, 310);
            this.dataShowControl1.Name = "dataShowControl1";
            this.dataShowControl1.Size = new System.Drawing.Size(216, 429);
            this.dataShowControl1.TabIndex = 32;
            // 
            // btn_p
            // 
            this.btn_p.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btn_p.Location = new System.Drawing.Point(1666, 457);
            this.btn_p.Name = "btn_p";
            this.btn_p.Size = new System.Drawing.Size(73, 54);
            this.btn_p.TabIndex = 37;
            this.btn_p.Text = "P";
            this.btn_p.ThemeName = "VisualStudio2012Dark";
            this.btn_p.Click += new System.EventHandler(this.radButton6_Click_1);
            // 
            // btn_zhong
            // 
            this.btn_zhong.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.btn_zhong.Location = new System.Drawing.Point(1577, 457);
            this.btn_zhong.Name = "btn_zhong";
            this.btn_zhong.Size = new System.Drawing.Size(73, 54);
            this.btn_zhong.TabIndex = 38;
            this.btn_zhong.Text = "R/T";
            this.btn_zhong.ThemeName = "VisualStudio2012Dark";
            this.btn_zhong.Click += new System.EventHandler(this.radButton3_Click_1);
            // 
            // btn_jian
            // 
            this.btn_jian.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.btn_jian.Location = new System.Drawing.Point(1666, 383);
            this.btn_jian.Name = "btn_jian";
            this.btn_jian.Size = new System.Drawing.Size(73, 54);
            this.btn_jian.TabIndex = 39;
            this.btn_jian.Text = "-";
            this.btn_jian.ThemeName = "VisualStudio2012Dark";
            this.btn_jian.Click += new System.EventHandler(this.radButton5_Click_1);
            // 
            // btn_down
            // 
            this.btn_down.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_down.Location = new System.Drawing.Point(1577, 383);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(73, 54);
            this.btn_down.TabIndex = 40;
            this.btn_down.Text = "▼";
            this.btn_down.ThemeName = "VisualStudio2012Dark";
            this.btn_down.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // btn_add
            // 
            this.btn_add.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.btn_add.Location = new System.Drawing.Point(1666, 310);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(73, 54);
            this.btn_add.TabIndex = 41;
            this.btn_add.Text = "+";
            this.btn_add.ThemeName = "VisualStudio2012Dark";
            this.btn_add.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // btn_up
            // 
            this.btn_up.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.btn_up.Location = new System.Drawing.Point(1577, 310);
            this.btn_up.Name = "btn_up";
            this.btn_up.Size = new System.Drawing.Size(73, 54);
            this.btn_up.TabIndex = 42;
            this.btn_up.Text = "▲";
            this.btn_up.ThemeName = "VisualStudio2012Dark";
            this.btn_up.Click += new System.EventHandler(this.radButton6_Click);
            // 
            // DrillingGasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btn_p);
            this.Controls.Add(this.btn_zhong);
            this.Controls.Add(this.btn_jian);
            this.Controls.Add(this.btn_down);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_up);
            this.Controls.Add(this.dataShowControl1);
            this.Controls.Add(this.fourChart2);
            this.Controls.Add(this.depthTimeChart1);
            this.Controls.Add(this.fourChart1);
            this.Name = "DrillingGasForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1920, 1080);
            this.Text = "GasForm";
            this.Activated += new System.EventHandler(this.DrillingGasForm_Activated);
            this.Load += new System.EventHandler(this.GasForm_Load);
            this.Controls.SetChildIndex(this.fourChart1, 0);
            this.Controls.SetChildIndex(this.depthTimeChart1, 0);
            this.Controls.SetChildIndex(this.fourChart2, 0);
            this.Controls.SetChildIndex(this.dataShowControl1, 0);
            this.Controls.SetChildIndex(this.btn_up, 0);
            this.Controls.SetChildIndex(this.btn_add, 0);
            this.Controls.SetChildIndex(this.btn_down, 0);
            this.Controls.SetChildIndex(this.btn_jian, 0);
            this.Controls.SetChildIndex(this.btn_zhong, 0);
            this.Controls.SetChildIndex(this.btn_p, 0);
            ((System.ComponentModel.ISupportInitialize)(this.btn_p)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_zhong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_jian)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_add)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_up)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserControls.FourChart fourChart1;
        private UserControls.DepthTimeChart depthTimeChart1;
        private UserControls.FourChart fourChart2;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private UserControls.DataShowControl dataShowControl1;
        private Telerik.WinControls.UI.RadButton btn_p;
        private Telerik.WinControls.UI.RadButton btn_zhong;
        private Telerik.WinControls.UI.RadButton btn_jian;
        private Telerik.WinControls.UI.RadButton btn_down;
        private Telerik.WinControls.UI.RadButton btn_add;
        private Telerik.WinControls.UI.RadButton btn_up;
    }
}