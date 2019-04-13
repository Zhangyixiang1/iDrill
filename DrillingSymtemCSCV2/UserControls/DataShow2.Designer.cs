namespace DrillingSymtemCSCV2.UserControls
{
    partial class DataShow2
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
            this.Captial = new Telerik.WinControls.UI.RadLabel();
            this.Unit = new Telerik.WinControls.UI.RadLabel();
            this.object_725b4edc_25f0_4d98_a86a_b42938007ada = new Telerik.WinControls.UI.RadLabelRootElement();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Value = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).BeginInit();
            this.SuspendLayout();
            // 
            // Captial
            // 
            this.Captial.BackColor = System.Drawing.Color.Transparent;
            this.Captial.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Captial.ForeColor = System.Drawing.Color.White;
            this.Captial.Location = new System.Drawing.Point(3, -1);
            this.Captial.Name = "Captial";
            this.Captial.Size = new System.Drawing.Size(49, 21);
            this.Captial.TabIndex = 0;
            this.Captial.Text = "Captial";
            // 
            // Unit
            // 
            this.Unit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Unit.AutoSize = false;
            this.Unit.BackColor = System.Drawing.Color.Transparent;
            this.Unit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Unit.ForeColor = System.Drawing.Color.Orange;
            this.Unit.Location = new System.Drawing.Point(134, 54);
            this.Unit.Name = "Unit";
            this.Unit.Size = new System.Drawing.Size(64, 18);
            this.Unit.TabIndex = 0;
            this.Unit.Text = "Unit";
            this.Unit.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // object_725b4edc_25f0_4d98_a86a_b42938007ada
            // 
            this.object_725b4edc_25f0_4d98_a86a_b42938007ada.Name = "object_725b4edc_25f0_4d98_a86a_b42938007ada";
            this.object_725b4edc_25f0_4d98_a86a_b42938007ada.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // Value
            // 
            this.Value.BackColor = System.Drawing.Color.Transparent;
            this.Value.Font = new System.Drawing.Font("Segoe UI Semibold", 28F, System.Drawing.FontStyle.Bold);
            this.Value.ForeColor = System.Drawing.Color.DodgerBlue;
            this.Value.Location = new System.Drawing.Point(-8, 12);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(181, 50);
            this.Value.TabIndex = 1;
            this.Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DataShow2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.Captial);
            this.Controls.Add(this.Value);
            this.Controls.Add(this.Unit);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "DataShow2";
            this.Size = new System.Drawing.Size(200, 76);
            this.Load += new System.EventHandler(this.DataShow2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Captial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Unit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabelRootElement object_725b4edc_25f0_4d98_a86a_b42938007ada;
        public Telerik.WinControls.UI.RadLabel Captial;
        public Telerik.WinControls.UI.RadLabel Unit;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.Label Value;
    }
}