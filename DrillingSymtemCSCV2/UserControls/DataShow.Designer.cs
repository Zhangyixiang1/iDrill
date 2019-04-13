namespace DrillingSymtemCSCV2.UserControls
{
    partial class DataShow
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
            this.From = new Telerik.WinControls.UI.RadLabel();
            this.Captial = new Telerik.WinControls.UI.RadLabel();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.To = new Telerik.WinControls.UI.RadLabel();
            this.Unit = new Telerik.WinControls.UI.RadLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Value = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.To)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // From
            // 
            this.From.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.From.ForeColor = System.Drawing.Color.DodgerBlue;
            this.From.Location = new System.Drawing.Point(10, 22);
            this.From.Name = "From";
            this.From.Size = new System.Drawing.Size(35, 19);
            this.From.TabIndex = 1;
            this.From.Text = "From";
            // 
            // Captial
            // 
            this.Captial.BackColor = System.Drawing.Color.Transparent;
            this.Captial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Captial.ForeColor = System.Drawing.Color.White;
            this.Captial.Location = new System.Drawing.Point(7, -1);
            this.Captial.Name = "Captial";
            this.Captial.Size = new System.Drawing.Size(49, 21);
            this.Captial.TabIndex = 0;
            this.Captial.Text = "Captial";
            this.toolTip1.SetToolTip(this.Captial, "Captial");
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(394, 45);
            this.shapeContainer1.TabIndex = 2;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.lineShape1.Enabled = false;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 9;
            this.lineShape1.X2 = 140;
            this.lineShape1.Y1 = 20;
            this.lineShape1.Y2 = 20;
            // 
            // To
            // 
            this.To.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.To.ForeColor = System.Drawing.Color.DodgerBlue;
            this.To.Location = new System.Drawing.Point(102, 22);
            this.To.Name = "To";
            this.To.Size = new System.Drawing.Size(20, 19);
            this.To.TabIndex = 1;
            this.To.Text = "To";
            // 
            // Unit
            // 
            this.Unit.AutoSize = false;
            this.Unit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Unit.ForeColor = System.Drawing.Color.Orange;
            this.Unit.Location = new System.Drawing.Point(329, 23);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(62, 19);
            this.Unit.TabIndex = 1;
            this.Unit.Text = "Unit";
            this.Unit.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // Value
            // 
            this.Value.AutoSize = false;
            this.Value.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.Value.ForeColor = System.Drawing.Color.White;
            this.Value.Location = new System.Drawing.Point(145, -6);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(188, 56);
            this.Value.TabIndex = 0;
            this.Value.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radLabel1.ForeColor = System.Drawing.Color.White;
            this.radLabel1.Location = new System.Drawing.Point(64, 22);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(14, 18);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "~";
            // 
            // DataShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.Unit);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.To);
            this.Controls.Add(this.Captial);
            this.Controls.Add(this.From);
            this.Controls.Add(this.shapeContainer1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "DataShow";
            this.Size = new System.Drawing.Size(394, 45);
            this.Load += new System.EventHandler(this.DataShow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.To)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private System.Windows.Forms.ToolTip toolTip1;
        public Telerik.WinControls.UI.RadLabel Value;
        public Telerik.WinControls.UI.RadLabel From;
        public Telerik.WinControls.UI.RadLabel Captial;
        public Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        public Telerik.WinControls.UI.RadLabel To;
        public Telerik.WinControls.UI.RadLabel Unit;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}