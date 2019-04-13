namespace DrillingSymtemCSCV2.Forms
{
    partial class SelectTagForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTagForm));
            this.btn_OK = new Telerik.WinControls.UI.RadButton();
            this.tab_tag = new System.Windows.Forms.TabControl();
            this.DrillPage = new System.Windows.Forms.TabPage();
            this.CirculatePage = new System.Windows.Forms.TabPage();
            this.DirectionalPage = new System.Windows.Forms.TabPage();
            this.tab_gia = new System.Windows.Forms.TabPage();
            this.tab_gfm = new System.Windows.Forms.TabPage();
            this.txt_From = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_Range = new Telerik.WinControls.UI.RadLabel();
            this.lbl_to = new Telerik.WinControls.UI.RadLabel();
            this.txt_TO = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_error = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.Remove = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            this.tab_tag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_From)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Range)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_to)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_error)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Location = new System.Drawing.Point(296, 549);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(108, 33);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "确定";
            this.btn_OK.ThemeName = "VisualStudio2012Dark";
            this.btn_OK.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // tab_tag
            // 
            this.tab_tag.Controls.Add(this.DrillPage);
            this.tab_tag.Controls.Add(this.CirculatePage);
            this.tab_tag.Controls.Add(this.DirectionalPage);
            this.tab_tag.Controls.Add(this.tab_gia);
            this.tab_tag.Controls.Add(this.tab_gfm);
            this.tab_tag.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_tag.Location = new System.Drawing.Point(51, 22);
            this.tab_tag.Name = "tab_tag";
            this.tab_tag.SelectedIndex = 0;
            this.tab_tag.Size = new System.Drawing.Size(813, 456);
            this.tab_tag.TabIndex = 3;
            // 
            // DrillPage
            // 
            this.DrillPage.AutoScroll = true;
            this.DrillPage.BackColor = System.Drawing.Color.Black;
            this.DrillPage.Location = new System.Drawing.Point(4, 26);
            this.DrillPage.Name = "DrillPage";
            this.DrillPage.Padding = new System.Windows.Forms.Padding(3);
            this.DrillPage.Size = new System.Drawing.Size(805, 426);
            this.DrillPage.TabIndex = 1;
            this.DrillPage.Text = "Drill";
            // 
            // CirculatePage
            // 
            this.CirculatePage.AutoScroll = true;
            this.CirculatePage.BackColor = System.Drawing.Color.Black;
            this.CirculatePage.Location = new System.Drawing.Point(4, 26);
            this.CirculatePage.Name = "CirculatePage";
            this.CirculatePage.Padding = new System.Windows.Forms.Padding(3);
            this.CirculatePage.Size = new System.Drawing.Size(805, 426);
            this.CirculatePage.TabIndex = 2;
            this.CirculatePage.Text = "Circulate";
            // 
            // DirectionalPage
            // 
            this.DirectionalPage.AutoScroll = true;
            this.DirectionalPage.BackColor = System.Drawing.Color.Black;
            this.DirectionalPage.Location = new System.Drawing.Point(4, 26);
            this.DirectionalPage.Name = "DirectionalPage";
            this.DirectionalPage.Size = new System.Drawing.Size(805, 426);
            this.DirectionalPage.TabIndex = 4;
            this.DirectionalPage.Text = "Directional";
            // 
            // tab_gia
            // 
            this.tab_gia.AutoScroll = true;
            this.tab_gia.BackColor = System.Drawing.Color.Black;
            this.tab_gia.Location = new System.Drawing.Point(4, 26);
            this.tab_gia.Name = "tab_gia";
            this.tab_gia.Size = new System.Drawing.Size(805, 426);
            this.tab_gia.TabIndex = 3;
            this.tab_gia.Text = "Gas in air";
            // 
            // tab_gfm
            // 
            this.tab_gfm.BackColor = System.Drawing.Color.Black;
            this.tab_gfm.Location = new System.Drawing.Point(4, 26);
            this.tab_gfm.Name = "tab_gfm";
            this.tab_gfm.Padding = new System.Windows.Forms.Padding(3);
            this.tab_gfm.Size = new System.Drawing.Size(805, 426);
            this.tab_gfm.TabIndex = 5;
            this.tab_gfm.Text = "Gas from mud";
            // 
            // txt_From
            // 
            this.txt_From.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_From.Location = new System.Drawing.Point(325, 486);
            this.txt_From.MaxLength = 10;
            this.txt_From.Name = "txt_From";
            this.txt_From.Size = new System.Drawing.Size(100, 24);
            this.txt_From.TabIndex = 4;
            this.txt_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_From.ThemeName = "VisualStudio2012Dark";
            // 
            // lbl_Range
            // 
            this.lbl_Range.AutoSize = false;
            this.lbl_Range.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Range.ForeColor = System.Drawing.Color.Red;
            this.lbl_Range.Location = new System.Drawing.Point(253, 484);
            this.lbl_Range.Name = "lbl_Range";
            this.lbl_Range.Size = new System.Drawing.Size(61, 25);
            this.lbl_Range.TabIndex = 5;
            this.lbl_Range.Text = "Range:";
            this.lbl_Range.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_to
            // 
            this.lbl_to.ForeColor = System.Drawing.Color.Red;
            this.lbl_to.Location = new System.Drawing.Point(442, 486);
            this.lbl_to.Name = "lbl_to";
            this.lbl_to.Size = new System.Drawing.Size(29, 18);
            this.lbl_to.TabIndex = 6;
            this.lbl_to.Text = "——";
            // 
            // txt_TO
            // 
            this.txt_TO.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_TO.Location = new System.Drawing.Point(486, 486);
            this.txt_TO.MaxLength = 10;
            this.txt_TO.Name = "txt_TO";
            this.txt_TO.Size = new System.Drawing.Size(100, 24);
            this.txt_TO.TabIndex = 4;
            this.txt_TO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_TO.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_error
            // 
            this.rlbl_error.AutoSize = false;
            this.rlbl_error.ForeColor = System.Drawing.Color.Red;
            this.rlbl_error.Location = new System.Drawing.Point(364, 520);
            this.rlbl_error.Name = "rlbl_error";
            this.rlbl_error.Size = new System.Drawing.Size(193, 18);
            this.rlbl_error.TabIndex = 7;
            this.rlbl_error.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Remove
            // 
            this.Remove.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Remove.Location = new System.Drawing.Point(467, 549);
            this.Remove.Name = "Remove";
            this.Remove.Size = new System.Drawing.Size(108, 33);
            this.Remove.TabIndex = 2;
            this.Remove.Text = "删除";
            this.Remove.ThemeName = "VisualStudio2012Dark";
            this.Remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // SelectTagForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(909, 595);
            this.Controls.Add(this.rlbl_error);
            this.Controls.Add(this.lbl_to);
            this.Controls.Add(this.lbl_Range);
            this.Controls.Add(this.txt_TO);
            this.Controls.Add(this.txt_From);
            this.Controls.Add(this.tab_tag);
            this.Controls.Add(this.Remove);
            this.Controls.Add(this.btn_OK);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTagForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SelectTagForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            this.tab_tag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txt_From)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Range)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_to)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_TO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_error)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Remove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btn_OK;
        private System.Windows.Forms.TabControl tab_tag;
        private System.Windows.Forms.TabPage DrillPage;
        private System.Windows.Forms.TabPage CirculatePage;
        private System.Windows.Forms.TabPage tab_gia;
        private System.Windows.Forms.TabPage DirectionalPage;
        private Telerik.WinControls.UI.RadTextBox txt_From;
        private Telerik.WinControls.UI.RadLabel lbl_Range;
        private Telerik.WinControls.UI.RadLabel lbl_to;
        private Telerik.WinControls.UI.RadTextBox txt_TO;
        private Telerik.WinControls.UI.RadLabel rlbl_error;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton Remove;
        private System.Windows.Forms.TabPage tab_gfm;
    }
}