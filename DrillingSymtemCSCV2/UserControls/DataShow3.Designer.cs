namespace DrillingSymtemCSCV2.UserControls
{
    partial class DataShow3
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
            this.Title = new Telerik.WinControls.UI.RadLabel();
            this.Value = new Telerik.WinControls.UI.RadLabel();
            this.Cube = new Telerik.WinControls.UI.RadLabel();
            this.Describe = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Title)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Describe)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.Yellow;
            this.Title.Location = new System.Drawing.Point(75, -5);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(42, 25);
            this.Title.TabIndex = 0;
            this.Title.Text = "Title";
            // 
            // Value
            // 
            this.Value.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Value.ForeColor = System.Drawing.Color.Lime;
            this.Value.Location = new System.Drawing.Point(70, 12);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(60, 37);
            this.Value.TabIndex = 1;
            this.Value.Text = "0.00";
            this.Value.Click += new System.EventHandler(this.Value_Click);
            // 
            // Cube
            // 
            this.Cube.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cube.ForeColor = System.Drawing.Color.Yellow;
            this.Cube.Location = new System.Drawing.Point(170, 10);
            this.Cube.Name = "Cube";
            this.Cube.Size = new System.Drawing.Size(33, 30);
            this.Cube.TabIndex = 2;
            this.Cube.Text = "m³";
            // 
            // Describe
            // 
            this.Describe.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Describe.ForeColor = System.Drawing.Color.Yellow;
            this.Describe.Location = new System.Drawing.Point(52, 45);
            this.Describe.Name = "Describe";
            this.Describe.Size = new System.Drawing.Size(74, 25);
            this.Describe.TabIndex = 3;
            this.Describe.Text = "Describe";
            // 
            // DataShow3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.Describe);
            this.Controls.Add(this.Cube);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.Value);
            this.Name = "DataShow3";
            this.Size = new System.Drawing.Size(201, 63);
            this.Load += new System.EventHandler(this.DataShow3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Title)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Describe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel Title;
        private Telerik.WinControls.UI.RadLabel Value;
        private Telerik.WinControls.UI.RadLabel Cube;
        private Telerik.WinControls.UI.RadLabel Describe;

    }
}
