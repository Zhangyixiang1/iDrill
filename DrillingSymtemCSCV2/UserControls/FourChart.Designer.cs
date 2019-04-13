namespace DrillingSymtemCSCV2.UserControls
{
    partial class FourChart
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
            this.components = new System.ComponentModel.Container();
            this.zed1 = new ZedGraph.ZedGraphControl();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.dataShow4 = new DrillingSymtemCSCV2.UserControls.DataShow();
            this.dataShow3 = new DrillingSymtemCSCV2.UserControls.DataShow();
            this.dataShow2 = new DrillingSymtemCSCV2.UserControls.DataShow();
            this.dataShow1 = new DrillingSymtemCSCV2.UserControls.DataShow();
            this.pic_load = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic_load)).BeginInit();
            this.SuspendLayout();
            // 
            // zed1
            // 
            this.zed1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zed1.AutoSize = true;
            this.zed1.IsShowContextMenu = false;
            this.zed1.Location = new System.Drawing.Point(0, 0);
            this.zed1.Name = "zed1";
            this.zed1.PanButtons = System.Windows.Forms.MouseButtons.None;
            this.zed1.PanButtons2 = System.Windows.Forms.MouseButtons.None;
            this.zed1.PanModifierKeys = System.Windows.Forms.Keys.None;
            this.zed1.ScrollGrace = 0D;
            this.zed1.ScrollMaxX = 0D;
            this.zed1.ScrollMaxY = 0D;
            this.zed1.ScrollMaxY2 = 0D;
            this.zed1.ScrollMinX = 0D;
            this.zed1.ScrollMinY = 0D;
            this.zed1.ScrollMinY2 = 0D;
            this.zed1.Size = new System.Drawing.Size(394, 680);
            this.zed1.TabIndex = 31;
            this.zed1.ZoomButtons = System.Windows.Forms.MouseButtons.XButton1;
            this.zed1.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.zed1_ZoomEvent);
            this.zed1.MouseEnter += new System.EventHandler(this.zed1_MouseEnter);
            this.zed1.MouseLeave += new System.EventHandler(this.zed1_MouseLeave);
            this.zed1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zed1_MouseMove);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 383);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(397, 1);
            this.label1.TabIndex = 37;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 5000;
            this.toolTip1.OwnerDraw = true;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            this.toolTip1.Draw += new System.Windows.Forms.DrawToolTipEventHandler(this.toolTip1_Draw);
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(50, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1, 680);
            this.label2.TabIndex = 38;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // dataShow4
            // 
            this.dataShow4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow4.AutoSize = true;
            this.dataShow4.BackColor = System.Drawing.Color.Black;
            this.dataShow4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow4.DSCaptal = null;
            this.dataShow4.DSHValue = null;
            this.dataShow4.DSLValue = null;
            this.dataShow4.DSRangeFrom = null;
            this.dataShow4.DSRangeTo = null;
            this.dataShow4.DSunit = null;
            this.dataShow4.DSvalue = null;
            this.dataShow4.DSValueColor = System.Drawing.Color.Empty;
            this.dataShow4.ForeColor = System.Drawing.Color.White;
            this.dataShow4.Location = new System.Drawing.Point(-1, 829);
            this.dataShow4.Name = "dataShow4";
            this.dataShow4.Size = new System.Drawing.Size(398, 61);
            this.dataShow4.TabIndex = 35;
            // 
            // dataShow3
            // 
            this.dataShow3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow3.AutoSize = true;
            this.dataShow3.BackColor = System.Drawing.Color.Black;
            this.dataShow3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow3.DSCaptal = null;
            this.dataShow3.DSHValue = null;
            this.dataShow3.DSLValue = null;
            this.dataShow3.DSRangeFrom = null;
            this.dataShow3.DSRangeTo = null;
            this.dataShow3.DSunit = null;
            this.dataShow3.DSvalue = null;
            this.dataShow3.DSValueColor = System.Drawing.Color.Empty;
            this.dataShow3.ForeColor = System.Drawing.Color.White;
            this.dataShow3.Location = new System.Drawing.Point(-1, 781);
            this.dataShow3.Name = "dataShow3";
            this.dataShow3.Size = new System.Drawing.Size(398, 61);
            this.dataShow3.TabIndex = 35;
            // 
            // dataShow2
            // 
            this.dataShow2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow2.AutoSize = true;
            this.dataShow2.BackColor = System.Drawing.Color.Black;
            this.dataShow2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow2.DSCaptal = null;
            this.dataShow2.DSHValue = null;
            this.dataShow2.DSLValue = null;
            this.dataShow2.DSRangeFrom = null;
            this.dataShow2.DSRangeTo = null;
            this.dataShow2.DSunit = null;
            this.dataShow2.DSvalue = null;
            this.dataShow2.DSValueColor = System.Drawing.Color.Empty;
            this.dataShow2.ForeColor = System.Drawing.Color.White;
            this.dataShow2.Location = new System.Drawing.Point(-1, 734);
            this.dataShow2.Name = "dataShow2";
            this.dataShow2.Size = new System.Drawing.Size(398, 61);
            this.dataShow2.TabIndex = 35;
            // 
            // dataShow1
            // 
            this.dataShow1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow1.AutoSize = true;
            this.dataShow1.BackColor = System.Drawing.Color.Black;
            this.dataShow1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow1.DSCaptal = null;
            this.dataShow1.DSHValue = null;
            this.dataShow1.DSLValue = null;
            this.dataShow1.DSRangeFrom = null;
            this.dataShow1.DSRangeTo = null;
            this.dataShow1.DSunit = null;
            this.dataShow1.DSvalue = null;
            this.dataShow1.DSValueColor = System.Drawing.Color.Empty;
            this.dataShow1.ForeColor = System.Drawing.Color.White;
            this.dataShow1.Location = new System.Drawing.Point(-1, 686);
            this.dataShow1.Name = "dataShow1";
            this.dataShow1.Size = new System.Drawing.Size(398, 61);
            this.dataShow1.TabIndex = 35;

            // 
            // pic_load
            // 
            this.pic_load.Image = global::DrillingSymtemCSCV2.Properties.Resources.prosessbar5;
            this.pic_load.Location = new System.Drawing.Point(176, 202);
            this.pic_load.Name = "pic_load";
            this.pic_load.Size = new System.Drawing.Size(23, 24);
            this.pic_load.TabIndex = 36;
            this.pic_load.TabStop = false;
            this.pic_load.Visible = false;

            // 
            // FourChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pic_load);
            this.Controls.Add(this.dataShow4);
            this.Controls.Add(this.dataShow3);
            this.Controls.Add(this.dataShow2);
            this.Controls.Add(this.dataShow1);
            this.Controls.Add(this.zed1);
            this.Name = "FourChart";
            this.Size = new System.Drawing.Size(400, 890);
            this.Load += new System.EventHandler(this.FourChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_load)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl zed1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        public DataShow dataShow4;
        public DataShow dataShow3;
        public DataShow dataShow2;
        public DataShow dataShow1;
        public System.Windows.Forms.PictureBox pic_load;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
