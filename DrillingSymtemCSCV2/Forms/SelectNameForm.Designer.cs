namespace DrillingSymtemCSCV2.Forms
{
    partial class SelectNameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectNameForm));
            this.rbtn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rpal_selectRota = new Telerik.WinControls.UI.RadPanel();
            this.rlbl_selectRota = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpal_selectRota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_selectRota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rbtn_Cancel
            // 
            this.rbtn_Cancel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Cancel.Location = new System.Drawing.Point(413, 401);
            this.rbtn_Cancel.Name = "rbtn_Cancel";
            this.rbtn_Cancel.Size = new System.Drawing.Size(115, 38);
            this.rbtn_Cancel.TabIndex = 4;
            this.rbtn_Cancel.Text = "Cancel";
            this.rbtn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Cancel.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_OK.Location = new System.Drawing.Point(266, 401);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(115, 38);
            this.rbtn_OK.TabIndex = 5;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // rpal_selectRota
            // 
            this.rpal_selectRota.AutoScroll = true;
            this.rpal_selectRota.Location = new System.Drawing.Point(123, 32);
            this.rpal_selectRota.Name = "rpal_selectRota";
            this.rpal_selectRota.Size = new System.Drawing.Size(559, 336);
            this.rpal_selectRota.TabIndex = 6;
            this.rpal_selectRota.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_selectRota
            // 
            this.rlbl_selectRota.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rlbl_selectRota.ForeColor = System.Drawing.Color.White;
            this.rlbl_selectRota.Location = new System.Drawing.Point(123, 3);
            this.rlbl_selectRota.Name = "rlbl_selectRota";
            this.rlbl_selectRota.Size = new System.Drawing.Size(120, 21);
            this.rlbl_selectRota.TabIndex = 7;
            this.rlbl_selectRota.Text = "Please Select Rota:";
            // 
            // SelectNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 464);
            this.Controls.Add(this.rlbl_selectRota);
            this.Controls.Add(this.rpal_selectRota);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.rbtn_Cancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectNameForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.SelectName_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpal_selectRota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_selectRota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton rbtn_Cancel;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadPanel rpal_selectRota;
        private Telerik.WinControls.UI.RadLabel rlbl_selectRota;
    }
}