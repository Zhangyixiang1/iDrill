namespace DrillingSymtemCSCV2.Forms
{
    partial class AddWorkerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddWorkerForm));
            this.rlbl_name = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_workerType = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_name = new Telerik.WinControls.UI.RadTextBox();
            this.rbtn_ok = new Telerik.WinControls.UI.RadButton();
            this.rbtn_cancel = new Telerik.WinControls.UI.RadButton();
            this.rlbl_EmpNo = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_EmpNo = new Telerik.WinControls.UI.RadTextBox();
            this.rtxt_type = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_workerType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_EmpNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_EmpNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_type)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlbl_name
            // 
            this.rlbl_name.AutoSize = false;
            this.rlbl_name.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_name.ForeColor = System.Drawing.Color.White;
            this.rlbl_name.Location = new System.Drawing.Point(71, 38);
            this.rlbl_name.Name = "rlbl_name";
            this.rlbl_name.Size = new System.Drawing.Size(100, 28);
            this.rlbl_name.TabIndex = 0;
            this.rlbl_name.Text = "Name:";
            this.rlbl_name.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rlbl_workerType
            // 
            this.rlbl_workerType.AutoSize = false;
            this.rlbl_workerType.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_workerType.ForeColor = System.Drawing.Color.White;
            this.rlbl_workerType.Location = new System.Drawing.Point(71, 79);
            this.rlbl_workerType.Name = "rlbl_workerType";
            this.rlbl_workerType.Size = new System.Drawing.Size(100, 28);
            this.rlbl_workerType.TabIndex = 0;
            this.rlbl_workerType.Text = "WorkerType:";
            this.rlbl_workerType.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtxt_name
            // 
            this.rtxt_name.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_name.Location = new System.Drawing.Point(186, 35);
            this.rtxt_name.Name = "rtxt_name";
            this.rtxt_name.Size = new System.Drawing.Size(159, 28);
            this.rtxt_name.TabIndex = 1;
            this.rtxt_name.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_ok
            // 
            this.rbtn_ok.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_ok.Location = new System.Drawing.Point(81, 166);
            this.rbtn_ok.Name = "rbtn_ok";
            this.rbtn_ok.Size = new System.Drawing.Size(110, 32);
            this.rbtn_ok.TabIndex = 3;
            this.rbtn_ok.Text = "OK";
            this.rbtn_ok.ThemeName = "VisualStudio2012Dark";
            this.rbtn_ok.Click += new System.EventHandler(this.rbtn_ok_Click);
            // 
            // rbtn_cancel
            // 
            this.rbtn_cancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_cancel.Location = new System.Drawing.Point(234, 166);
            this.rbtn_cancel.Name = "rbtn_cancel";
            this.rbtn_cancel.Size = new System.Drawing.Size(110, 32);
            this.rbtn_cancel.TabIndex = 3;
            this.rbtn_cancel.Text = "Cancel";
            this.rbtn_cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_cancel.Click += new System.EventHandler(this.rbtn_cancel_Click);
            // 
            // rlbl_EmpNo
            // 
            this.rlbl_EmpNo.AutoSize = false;
            this.rlbl_EmpNo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_EmpNo.ForeColor = System.Drawing.Color.White;
            this.rlbl_EmpNo.Location = new System.Drawing.Point(71, 126);
            this.rlbl_EmpNo.Name = "rlbl_EmpNo";
            this.rlbl_EmpNo.Size = new System.Drawing.Size(100, 28);
            this.rlbl_EmpNo.TabIndex = 0;
            this.rlbl_EmpNo.Text = "Emp NO.:";
            this.rlbl_EmpNo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rtxt_EmpNo
            // 
            this.rtxt_EmpNo.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_EmpNo.Location = new System.Drawing.Point(186, 126);
            this.rtxt_EmpNo.Name = "rtxt_EmpNo";
            this.rtxt_EmpNo.Size = new System.Drawing.Size(159, 28);
            this.rtxt_EmpNo.TabIndex = 3;
            this.rtxt_EmpNo.ThemeName = "VisualStudio2012Dark";
            // 
            // rtxt_type
            // 
            this.rtxt_type.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_type.Location = new System.Drawing.Point(186, 79);
            this.rtxt_type.Name = "rtxt_type";
            this.rtxt_type.ReadOnly = true;
            this.rtxt_type.Size = new System.Drawing.Size(159, 28);
            this.rtxt_type.TabIndex = 2;
            this.rtxt_type.ThemeName = "VisualStudio2012Dark";
            this.rtxt_type.Click += new System.EventHandler(this.rtxt_type_Click);
            // 
            // AddWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 222);
            this.Controls.Add(this.rbtn_cancel);
            this.Controls.Add(this.rbtn_ok);
            this.Controls.Add(this.rtxt_type);
            this.Controls.Add(this.rtxt_EmpNo);
            this.Controls.Add(this.rtxt_name);
            this.Controls.Add(this.rlbl_workerType);
            this.Controls.Add(this.rlbl_EmpNo);
            this.Controls.Add(this.rlbl_name);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddWorkerForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.AddWorkerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_workerType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_EmpNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_EmpNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_type)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel rlbl_name;
        private Telerik.WinControls.UI.RadLabel rlbl_workerType;
        private Telerik.WinControls.UI.RadTextBox rtxt_name;
        private Telerik.WinControls.UI.RadButton rbtn_ok;
        private Telerik.WinControls.UI.RadButton rbtn_cancel;
        private Telerik.WinControls.UI.RadLabel rlbl_EmpNo;
        private Telerik.WinControls.UI.RadTextBox rtxt_EmpNo;
        private Telerik.WinControls.UI.RadTextBox rtxt_type;
    }
}