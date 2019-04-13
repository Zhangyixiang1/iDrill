namespace DrillingSymtemCSCV2.UserControls
{
    partial class DepthTimeChart
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
            this.rbtn_message = new Telerik.WinControls.UI.RadButton();
            this.lbl_time = new System.Windows.Forms.Label();
            this.lbl_unit = new System.Windows.Forms.Label();
            this.setCharts = new Telerik.WinControls.UI.RadButton();
            this.rbtn_meglist = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_message)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCharts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_meglist)).BeginInit();
            this.SuspendLayout();
            // 
            // zed1
            // 
            this.zed1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.zed1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.zed1.Location = new System.Drawing.Point(-5, 3);
            this.zed1.Margin = new System.Windows.Forms.Padding(4);
            this.zed1.Name = "zed1";
            this.zed1.ScrollGrace = 0D;
            this.zed1.ScrollMaxX = 0D;
            this.zed1.ScrollMaxY = 0D;
            this.zed1.ScrollMaxY2 = 0D;
            this.zed1.ScrollMinX = 0D;
            this.zed1.ScrollMinY = 0D;
            this.zed1.ScrollMinY2 = 0D;
            this.zed1.Size = new System.Drawing.Size(300, 670);
            this.zed1.TabIndex = 32;
            // 
            // rbtn_message
            // 
            this.rbtn_message.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_message.ForeColor = System.Drawing.Color.White;
            this.rbtn_message.Location = new System.Drawing.Point(165, 743);
            this.rbtn_message.Name = "rbtn_message";
            this.rbtn_message.Size = new System.Drawing.Size(97, 40);
            this.rbtn_message.TabIndex = 34;
            this.rbtn_message.Text = "Add Message";
            this.rbtn_message.ThemeName = "VisualStudio2012Dark";
            this.rbtn_message.Click += new System.EventHandler(this.rbtn_alarmConfirm_Click);
            // 
            // lbl_time
            // 
            this.lbl_time.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lbl_time.ForeColor = System.Drawing.Color.White;
            this.lbl_time.Location = new System.Drawing.Point(5, 685);
            this.lbl_time.Name = "lbl_time";
            this.lbl_time.Size = new System.Drawing.Size(143, 21);
            this.lbl_time.TabIndex = 35;
            this.lbl_time.Text = "Recently Alarms";
            this.lbl_time.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbl_unit
            // 
            this.lbl_unit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_unit.ForeColor = System.Drawing.Color.White;
            this.lbl_unit.Location = new System.Drawing.Point(157, 703);
            this.lbl_unit.Name = "lbl_unit";
            this.lbl_unit.Size = new System.Drawing.Size(59, 21);
            this.lbl_unit.TabIndex = 36;
            this.lbl_unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbl_unit.Visible = false;
            // 
            // setCharts
            // 
            this.setCharts.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.setCharts.ForeColor = System.Drawing.Color.White;
            this.setCharts.Location = new System.Drawing.Point(28, 743);
            this.setCharts.Name = "setCharts";
            this.setCharts.Size = new System.Drawing.Size(97, 40);
            this.setCharts.TabIndex = 35;
            this.setCharts.Text = "设置图表";
            this.setCharts.ThemeName = "VisualStudio2012Dark";
            this.setCharts.Click += new System.EventHandler(this.setUp);
            // 
            // rbtn_meglist
            // 
            this.rbtn_meglist.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_meglist.ForeColor = System.Drawing.Color.White;
            this.rbtn_meglist.Location = new System.Drawing.Point(165, 798);
            this.rbtn_meglist.Name = "rbtn_meglist";
            this.rbtn_meglist.Size = new System.Drawing.Size(97, 40);
            this.rbtn_meglist.TabIndex = 35;
            this.rbtn_meglist.Text = "消息列表";
            this.rbtn_meglist.ThemeName = "VisualStudio2012Dark";
            this.rbtn_meglist.Click += new System.EventHandler(this.rbtn_showMessage_Click);
            // 
            // DepthTimeChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbtn_meglist);
            this.Controls.Add(this.setCharts);
            this.Controls.Add(this.lbl_unit);
            this.Controls.Add(this.lbl_time);
            this.Controls.Add(this.rbtn_message);
            this.Controls.Add(this.zed1);
            this.Name = "DepthTimeChart";
            this.Size = new System.Drawing.Size(300, 842);
            this.Load += new System.EventHandler(this.DepthTimeChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_message)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.setCharts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_meglist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zed1;
        private Telerik.WinControls.UI.RadButton rbtn_message;
        private System.Windows.Forms.Label lbl_time;
        private System.Windows.Forms.Label lbl_unit;
        private Telerik.WinControls.UI.RadButton setCharts;
        private Telerik.WinControls.UI.RadButton rbtn_meglist;
    }
}
