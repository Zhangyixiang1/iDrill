namespace DrillingSymtemCSCV2.UserControls
{
    partial class DataShow4
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
            this.Value = new Telerik.WinControls.UI.RadLabel();
            this.Cube = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cube)).BeginInit();
            this.SuspendLayout();
            // 
            // Value
            // 
            this.Value.BackColor = System.Drawing.SystemColors.WindowText;
            this.Value.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Value.ForeColor = System.Drawing.Color.Lime;
            this.Value.Location = new System.Drawing.Point(21, 9);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(86, 44);
            this.Value.TabIndex = 1;
            this.Value.Text = "Value";
            // 
            // Cube
            // 
            this.Cube.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cube.ForeColor = System.Drawing.Color.Yellow;
            this.Cube.Location = new System.Drawing.Point(101, 28);
            this.Cube.Name = "Cube";
            this.Cube.Size = new System.Drawing.Size(33, 30);
            this.Cube.TabIndex = 2;
            this.Cube.Text = "m³";
            // 
            // DataShow4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.Cube);
            this.Controls.Add(this.Value);
            this.Name = "DataShow4";
            this.Size = new System.Drawing.Size(132, 56);
            this.Load += new System.EventHandler(this.AdjustmentSize);
            this.Resize += new System.EventHandler(this.DataShow4_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cube)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel Value;
        private Telerik.WinControls.UI.RadLabel Cube;

    }
}
