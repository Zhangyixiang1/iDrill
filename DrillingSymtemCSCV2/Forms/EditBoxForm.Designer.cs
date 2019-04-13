namespace DrillingSymtemCSCV2.Forms
{
    partial class EditBoxForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditBoxForm));
            this.btn_OK = new Telerik.WinControls.UI.RadButton();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.pnl_Memotext = new Telerik.WinControls.UI.RadPanel();
            this.txt_Input = new System.Windows.Forms.TextBox();
            this.lbl_Select = new Telerik.WinControls.UI.RadLabel();
            this.lbl_Input = new Telerik.WinControls.UI.RadLabel();
            this.cbo_Select = new System.Windows.Forms.ComboBox();
            this.lbl_Memotext = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Memotext)).BeginInit();
            this.pnl_Memotext.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Memotext)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Location = new System.Drawing.Point(124, 303);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(115, 38);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "OK";
            this.btn_OK.ThemeName = "VisualStudio2012Dark";
            this.btn_OK.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(295, 303);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(115, 38);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // pnl_Memotext
            // 
            this.pnl_Memotext.Controls.Add(this.txt_Input);
            this.pnl_Memotext.Controls.Add(this.lbl_Select);
            this.pnl_Memotext.Controls.Add(this.lbl_Input);
            this.pnl_Memotext.Controls.Add(this.cbo_Select);
            this.pnl_Memotext.Location = new System.Drawing.Point(80, 41);
            this.pnl_Memotext.Name = "pnl_Memotext";
            this.pnl_Memotext.Size = new System.Drawing.Size(390, 226);
            this.pnl_Memotext.TabIndex = 21;
            // 
            // txt_Input
            // 
            this.txt_Input.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.txt_Input.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.txt_Input.ForeColor = System.Drawing.Color.Red;
            this.txt_Input.Location = new System.Drawing.Point(131, 148);
            this.txt_Input.Name = "txt_Input";
            this.txt_Input.Size = new System.Drawing.Size(219, 33);
            this.txt_Input.TabIndex = 7;
            // 
            // lbl_Select
            // 
            this.lbl_Select.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Select.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Select.ForeColor = System.Drawing.Color.White;
            this.lbl_Select.Location = new System.Drawing.Point(61, 62);
            this.lbl_Select.Name = "lbl_Select";
            this.lbl_Select.Size = new System.Drawing.Size(64, 30);
            this.lbl_Select.TabIndex = 4;
            this.lbl_Select.Text = "Select:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Select.GetChildAt(0))).Text = "Select:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Select.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // lbl_Input
            // 
            this.lbl_Input.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Input.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Input.ForeColor = System.Drawing.Color.White;
            this.lbl_Input.Location = new System.Drawing.Point(66, 150);
            this.lbl_Input.Name = "lbl_Input";
            this.lbl_Input.Size = new System.Drawing.Size(59, 30);
            this.lbl_Input.TabIndex = 4;
            this.lbl_Input.Text = "Input:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Input.GetChildAt(0))).Text = "Input:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Input.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // cbo_Select
            // 
            this.cbo_Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbo_Select.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbo_Select.FormattingEnabled = true;
            this.cbo_Select.Location = new System.Drawing.Point(131, 62);
            this.cbo_Select.Name = "cbo_Select";
            this.cbo_Select.Size = new System.Drawing.Size(219, 33);
            this.cbo_Select.TabIndex = 6;
            // 
            // lbl_Memotext
            // 
            this.lbl_Memotext.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Memotext.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Memotext.ForeColor = System.Drawing.Color.White;
            this.lbl_Memotext.Location = new System.Drawing.Point(84, 23);
            this.lbl_Memotext.Name = "lbl_Memotext";
            this.lbl_Memotext.Size = new System.Drawing.Size(113, 31);
            this.lbl_Memotext.TabIndex = 3;
            this.lbl_Memotext.Text = "Memo text:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Memotext.GetChildAt(0))).Text = "Memo text:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Memotext.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // EditBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 364);
            this.Controls.Add(this.lbl_Memotext);
            this.Controls.Add(this.pnl_Memotext);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditBoxForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EditBoxForm";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.EditBoxForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl_Memotext)).EndInit();
            this.pnl_Memotext.ResumeLayout(false);
            this.pnl_Memotext.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Memotext)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btn_OK;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadPanel pnl_Memotext;
        private Telerik.WinControls.UI.RadLabel lbl_Select;
        private Telerik.WinControls.UI.RadLabel lbl_Input;
        private System.Windows.Forms.ComboBox cbo_Select;
        private Telerik.WinControls.UI.RadLabel lbl_Memotext;
        private System.Windows.Forms.TextBox txt_Input;
    }
}