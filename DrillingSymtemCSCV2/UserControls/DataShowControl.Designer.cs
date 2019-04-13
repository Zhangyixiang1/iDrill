namespace DrillingSymtemCSCV2.UserControls
{
    partial class DataShowControl
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
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.dataShow25 = new DrillingSymtemCSCV2.UserControls.DataShow2();
            this.dataShow24 = new DrillingSymtemCSCV2.UserControls.DataShow2();
            this.dataShow23 = new DrillingSymtemCSCV2.UserControls.DataShow2();
            this.dataShow22 = new DrillingSymtemCSCV2.UserControls.DataShow2();
            this.dataShow21 = new DrillingSymtemCSCV2.UserControls.DataShow2();
            this.SuspendLayout();
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // dataShow25
            // 
            this.dataShow25.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow25.BackColor = System.Drawing.Color.Black;
            this.dataShow25.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow25.DSCaptial = null;
            this.dataShow25.DSHValue = null;
            this.dataShow25.DSLValue = null;
            this.dataShow25.DSUnit = null;
            this.dataShow25.DSValue = null;
            this.dataShow25.ForeColor = System.Drawing.Color.White;
            this.dataShow25.Location = new System.Drawing.Point(3, 335);
            this.dataShow25.Name = "dataShow25";
            this.dataShow25.Size = new System.Drawing.Size(200, 80);
            this.dataShow25.TabIndex = 0;
            // 
            // dataShow24
            // 
            this.dataShow24.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow24.BackColor = System.Drawing.Color.Black;
            this.dataShow24.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow24.DSCaptial = null;
            this.dataShow24.DSHValue = null;
            this.dataShow24.DSLValue = null;
            this.dataShow24.DSUnit = null;
            this.dataShow24.DSValue = null;
            this.dataShow24.ForeColor = System.Drawing.Color.White;
            this.dataShow24.Location = new System.Drawing.Point(3, 252);
            this.dataShow24.Name = "dataShow24";
            this.dataShow24.Size = new System.Drawing.Size(200, 80);
            this.dataShow24.TabIndex = 0;
            // 
            // dataShow23
            // 
            this.dataShow23.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow23.BackColor = System.Drawing.Color.Black;
            this.dataShow23.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow23.DSCaptial = null;
            this.dataShow23.DSHValue = null;
            this.dataShow23.DSLValue = null;
            this.dataShow23.DSUnit = null;
            this.dataShow23.DSValue = null;
            this.dataShow23.ForeColor = System.Drawing.Color.White;
            this.dataShow23.Location = new System.Drawing.Point(3, 169);
            this.dataShow23.Name = "dataShow23";
            this.dataShow23.Size = new System.Drawing.Size(200, 80);
            this.dataShow23.TabIndex = 0;
            // 
            // dataShow22
            // 
            this.dataShow22.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow22.BackColor = System.Drawing.Color.Black;
            this.dataShow22.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow22.DSCaptial = null;
            this.dataShow22.DSHValue = null;
            this.dataShow22.DSLValue = null;
            this.dataShow22.DSUnit = null;
            this.dataShow22.DSValue = null;
            this.dataShow22.ForeColor = System.Drawing.Color.White;
            this.dataShow22.Location = new System.Drawing.Point(3, 86);
            this.dataShow22.Name = "dataShow22";
            this.dataShow22.Size = new System.Drawing.Size(200, 80);
            this.dataShow22.TabIndex = 0;
            // 
            // dataShow21
            // 
            this.dataShow21.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dataShow21.BackColor = System.Drawing.Color.Black;
            this.dataShow21.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataShow21.DSCaptial = null;
            this.dataShow21.DSHValue = null;
            this.dataShow21.DSLValue = null;
            this.dataShow21.DSUnit = null;
            this.dataShow21.DSValue = null;
            this.dataShow21.ForeColor = System.Drawing.Color.White;
            this.dataShow21.Location = new System.Drawing.Point(3, 3);
            this.dataShow21.Name = "dataShow21";
            this.dataShow21.Size = new System.Drawing.Size(200, 80);
            this.dataShow21.TabIndex = 0;
            // 
            // DataShowControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataShow25);
            this.Controls.Add(this.dataShow24);
            this.Controls.Add(this.dataShow23);
            this.Controls.Add(this.dataShow22);
            this.Controls.Add(this.dataShow21);
            this.Name = "DataShowControl";
            this.Size = new System.Drawing.Size(205, 417);
            this.Load += new System.EventHandler(this.DataShowControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public DataShow2 dataShow21;
        public DataShow2 dataShow22;
        public DataShow2 dataShow23;
        public DataShow2 dataShow24;
        public DataShow2 dataShow25;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;

    }
}
