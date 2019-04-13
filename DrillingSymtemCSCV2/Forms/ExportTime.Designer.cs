namespace DrillingSymtemCSCV2.Forms
{
    partial class ExportTime 
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
            this.btn_OK = new System.Windows.Forms.Button();
            this.radTextStart = new Telerik.WinControls.UI.RadTextBox();
            this.label_start = new System.Windows.Forms.Label();
            this.radTextEnd = new Telerik.WinControls.UI.RadTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.radTextStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(105, 158);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(100, 30);
            this.btn_OK.TabIndex = 18;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // radTextStart
            // 
            this.radTextStart.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.radTextStart.ForeColor = System.Drawing.Color.White;
            this.radTextStart.Location = new System.Drawing.Point(135, 35);
            this.radTextStart.Name = "radTextStart";
            this.radTextStart.ReadOnly = true;
            this.radTextStart.Size = new System.Drawing.Size(235, 28);
            this.radTextStart.TabIndex = 32;
            this.radTextStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.radTextStart.ThemeName = "VisualStudio2012Dark";
            this.radTextStart.Click += new System.EventHandler(this.radTextStart_Click);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextStart.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.YellowGreen;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextStart.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextStart.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.YellowGreen;
            // 
            // label_start
            // 
            this.label_start.AutoSize = true;
            this.label_start.BackColor = System.Drawing.Color.Transparent;
            this.label_start.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label_start.ForeColor = System.Drawing.Color.White;
            this.label_start.Location = new System.Drawing.Point(43, 38);
            this.label_start.Name = "label_start";
            this.label_start.Size = new System.Drawing.Size(82, 21);
            this.label_start.TabIndex = 31;
            this.label_start.Text = "开始时间:";
            // 
            // radTextEnd
            // 
            this.radTextEnd.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.radTextEnd.ForeColor = System.Drawing.Color.White;
            this.radTextEnd.Location = new System.Drawing.Point(135, 100);
            this.radTextEnd.Name = "radTextEnd";
            this.radTextEnd.ReadOnly = true;
            this.radTextEnd.Size = new System.Drawing.Size(235, 28);
            this.radTextEnd.TabIndex = 34;
            this.radTextEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.radTextEnd.ThemeName = "VisualStudio2012Dark";
            this.radTextEnd.Click += new System.EventHandler(this.radTextEnd_Click);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextEnd.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.YellowGreen;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.radTextEnd.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radTextEnd.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.YellowGreen;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(43, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 21);
            this.label1.TabIndex = 33;
            this.label1.Text = "结束时间:";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Location = new System.Drawing.Point(290, 158);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(100, 30);
            this.btn_Cancel.TabIndex = 35;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // ExportTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(462, 197);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.radTextEnd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radTextStart);
            this.Controls.Add(this.label_start);
            this.Controls.Add(this.btn_OK);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportTime";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "时间选择";
            this.ThemeName = "VisualStudio2012Dark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.HisWellList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radTextStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private Telerik.WinControls.UI.RadTextBox radTextStart;
        private System.Windows.Forms.Label label_start;
        private Telerik.WinControls.UI.RadTextBox radTextEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Cancel;
    }
}