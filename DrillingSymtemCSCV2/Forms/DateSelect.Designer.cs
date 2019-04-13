namespace DrillingSymtemCSCV2.Forms
{
    partial class DateSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateSelect));
            this.rc_date = new Telerik.WinControls.UI.RadCalendar();
            this.rlbl_date = new Telerik.WinControls.UI.RadLabel();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rc_date)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_date)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rc_date
            // 
            this.rc_date.Culture = new System.Globalization.CultureInfo("en");
            this.rc_date.FocusedDate = new System.DateTime(2017, 7, 19, 0, 0, 0, 0);
            this.rc_date.Location = new System.Drawing.Point(3, 31);
            this.rc_date.Name = "rc_date";
            this.rc_date.RangeMinDate = new System.DateTime(((long)(0)));
            this.rc_date.Size = new System.Drawing.Size(461, 260);
            this.rc_date.TabIndex = 52;
            this.rc_date.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_date
            // 
            this.rlbl_date.AutoSize = false;
            this.rlbl_date.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rlbl_date.ForeColor = System.Drawing.Color.White;
            this.rlbl_date.Location = new System.Drawing.Point(3, -2);
            this.rlbl_date.Name = "rlbl_date";
            this.rlbl_date.Size = new System.Drawing.Size(188, 24);
            this.rlbl_date.TabIndex = 53;
            this.rlbl_date.Text = "Please select date:";
            this.rlbl_date.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(255, 306);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(108, 33);
            this.btn_Cancel.TabIndex = 54;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_OK.Location = new System.Drawing.Point(109, 306);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(108, 33);
            this.rbtn_OK.TabIndex = 54;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.rbtn_OK_Click);
            // 
            // DateSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 350);
            this.Controls.Add(this.rc_date);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.rlbl_date);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DateSelect";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.DateSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rc_date)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_date)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadCalendar rc_date;
        private Telerik.WinControls.UI.RadLabel rlbl_date;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
    }
}