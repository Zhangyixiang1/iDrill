namespace DrillingSymtemCSCV2.UserControls
{
    partial class chart
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_chart = new System.Windows.Forms.Label();
            this.pnl_Memotext = new Telerik.WinControls.UI.RadPanel();
            this.lbl_status = new System.Windows.Forms.Label();
            this.Value = new Telerik.WinControls.UI.RadLabel();
            this.Unit = new Telerik.WinControls.UI.RadLabel();
            this.Captial = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.rlbl_percent = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Memotext)).BeginInit();
            this.pnl_Memotext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_percent)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_chart
            // 
            this.lbl_chart.BackColor = System.Drawing.Color.Red;
            this.lbl_chart.Location = new System.Drawing.Point(46, 7);
            this.lbl_chart.Name = "lbl_chart";
            this.lbl_chart.Size = new System.Drawing.Size(40, 100);
            this.lbl_chart.TabIndex = 0;
            this.lbl_chart.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // pnl_Memotext
            // 
            this.pnl_Memotext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnl_Memotext.Controls.Add(this.lbl_status);
            this.pnl_Memotext.Controls.Add(this.Value);
            this.pnl_Memotext.Controls.Add(this.Unit);
            this.pnl_Memotext.Controls.Add(this.Captial);
            this.pnl_Memotext.Location = new System.Drawing.Point(0, 110);
            this.pnl_Memotext.Name = "pnl_Memotext";
            this.pnl_Memotext.Size = new System.Drawing.Size(128, 53);
            this.pnl_Memotext.TabIndex = 22;
            this.pnl_Memotext.ThemeName = "VisualStudio2012Dark";
            this.pnl_Memotext.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // lbl_status
            // 
            this.lbl_status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_status.BackColor = System.Drawing.Color.Transparent;
            this.lbl_status.Font = new System.Drawing.Font("Segoe UI", 25F);
            this.lbl_status.ForeColor = System.Drawing.Color.White;
            this.lbl_status.Location = new System.Drawing.Point(110, -16);
            this.lbl_status.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(21, 39);
            this.lbl_status.TabIndex = 3;
            this.lbl_status.Text = "●";
            this.lbl_status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Value
            // 
            this.Value.BackColor = System.Drawing.Color.Transparent;
            this.Value.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.Value.ForeColor = System.Drawing.Color.White;
            this.Value.Location = new System.Drawing.Point(6, 20);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(20, 31);
            this.Value.TabIndex = 2;
            this.Value.Text = "0";
            // 
            // Unit
            // 
            this.Unit.BackColor = System.Drawing.Color.Transparent;
            this.Unit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Unit.ForeColor = System.Drawing.Color.Orange;
            this.Unit.Location = new System.Drawing.Point(93, 30);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(30, 21);
            this.Unit.TabIndex = 1;
            this.Unit.Text = "unit";
            // 
            // Captial
            // 
            this.Captial.BackColor = System.Drawing.Color.Transparent;
            this.Captial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Captial.ForeColor = System.Drawing.Color.White;
            this.Captial.Location = new System.Drawing.Point(3, 3);
            this.Captial.Name = "Captial";
            this.Captial.Size = new System.Drawing.Size(25, 21);
            this.Captial.TabIndex = 1;
            this.Captial.Text = "var";
            // 
            // radLabel2
            // 
            this.radLabel2.BackColor = System.Drawing.Color.Transparent;
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.radLabel2.ForeColor = System.Drawing.Color.White;
            this.radLabel2.Location = new System.Drawing.Point(19, 97);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(25, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "0%-";
            this.radLabel2.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // radLabel3
            // 
            this.radLabel3.BackColor = System.Drawing.Color.Transparent;
            this.radLabel3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.radLabel3.ForeColor = System.Drawing.Color.White;
            this.radLabel3.Location = new System.Drawing.Point(13, 47);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(31, 18);
            this.radLabel3.TabIndex = 1;
            this.radLabel3.Text = "50%-";
            this.radLabel3.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // radLabel4
            // 
            this.radLabel4.BackColor = System.Drawing.Color.Transparent;
            this.radLabel4.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.radLabel4.ForeColor = System.Drawing.Color.White;
            this.radLabel4.Location = new System.Drawing.Point(6, -2);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(37, 18);
            this.radLabel4.TabIndex = 1;
            this.radLabel4.Text = "100%-";
            this.radLabel4.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.White;
            this.radPanel1.Location = new System.Drawing.Point(46, 6);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(40, 100);
            this.radPanel1.TabIndex = 22;
            this.radPanel1.ThemeName = "VisualStudio2012Dark";
            this.radPanel1.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // rlbl_percent
            // 
            this.rlbl_percent.BackColor = System.Drawing.Color.Transparent;
            this.rlbl_percent.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.rlbl_percent.ForeColor = System.Drawing.Color.White;
            this.rlbl_percent.Location = new System.Drawing.Point(87, -2);
            this.rlbl_percent.Name = "rlbl_percent";
            this.rlbl_percent.Size = new System.Drawing.Size(33, 18);
            this.rlbl_percent.TabIndex = 1;
            this.rlbl_percent.Text = "100%";
            this.rlbl_percent.Click += new System.EventHandler(this.pnl_Memotext_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.Controls.Add(this.pnl_Memotext);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.rlbl_percent);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.lbl_chart);
            this.Controls.Add(this.radPanel1);
            this.Name = "chart";
            this.Size = new System.Drawing.Size(128, 167);
            this.Load += new System.EventHandler(this.chart_Load);
            this.Click += new System.EventHandler(this.pnl_Memotext_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Memotext)).EndInit();
            this.pnl_Memotext.ResumeLayout(false);
            this.pnl_Memotext.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_percent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_chart;
        private Telerik.WinControls.UI.RadPanel pnl_Memotext;
        public Telerik.WinControls.UI.RadLabel Captial;
        public Telerik.WinControls.UI.RadLabel Value;
        public Telerik.WinControls.UI.RadLabel Unit;
        public Telerik.WinControls.UI.RadLabel radLabel2;
        public Telerik.WinControls.UI.RadLabel radLabel3;
        public Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        public Telerik.WinControls.UI.RadLabel rlbl_percent;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label lbl_status;
    }
}
