namespace DrillingSymtemCSCV2.UserControls
{
    partial class HistoryDataShow
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
            this.pic_load = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pic_load)).BeginInit();
            this.SuspendLayout();
            // 
            // zed1
            // 
            this.zed1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.zed1.BackColor = System.Drawing.SystemColors.Control;
            this.zed1.ForeColor = System.Drawing.Color.White;
            this.zed1.IsShowContextMenu = false;
            this.zed1.Location = new System.Drawing.Point(12, 3);
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
            this.zed1.Size = new System.Drawing.Size(330, 617);
            this.zed1.TabIndex = 32;
            this.zed1.ZoomButtons = System.Windows.Forms.MouseButtons.XButton1;
            //this.zed1.ZoomEvent += new ZedGraph.ZedGraphControl.ZoomEventHandler(this.zed1_ZoomEvent);
            this.zed1.MouseEnter += new System.EventHandler(this.zed1_MouseEnter);
            this.zed1.MouseLeave += new System.EventHandler(this.zed1_MouseLeave);
            this.zed1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.zed1_MouseMove);
            // 
            // pic_load
            // 
            this.pic_load.Image = global::DrillingSymtemCSCV2.Properties.Resources.prosessbar5;
            this.pic_load.Location = new System.Drawing.Point(177, 221);
            this.pic_load.Name = "pic_load";
            this.pic_load.Size = new System.Drawing.Size(23, 24);
            this.pic_load.TabIndex = 37;
            this.pic_load.TabStop = false;
            this.pic_load.Visible = false;
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
            // HistoryDataShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pic_load);
            this.Controls.Add(this.zed1);
            this.Name = "HistoryDataShow";
            this.Size = new System.Drawing.Size(345, 620);
            this.Load += new System.EventHandler(this.HistoryDataShow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_load)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zed1;
        public System.Windows.Forms.PictureBox pic_load;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
