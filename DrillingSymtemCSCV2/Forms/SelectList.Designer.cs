namespace DrillingSymtemCSCV2.Forms
{
    partial class SelectList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectList));
            this.rlv_selectItem = new Telerik.WinControls.UI.RadListView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            this.lbl_new = new System.Windows.Forms.Label();
            this.txt_box1 = new Telerik.WinControls.UI.RadTextBox();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_delete = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rlv_selectItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_box1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlv_selectItem
            // 
            this.rlv_selectItem.AutoScroll = true;
            this.rlv_selectItem.ForeColor = System.Drawing.Color.White;
            this.rlv_selectItem.Location = new System.Drawing.Point(-1, 27);
            this.rlv_selectItem.Name = "rlv_selectItem";
            this.rlv_selectItem.Size = new System.Drawing.Size(196, 289);
            this.rlv_selectItem.TabIndex = 0;
            this.rlv_selectItem.Text = "radListview1";
            this.rlv_selectItem.ThemeName = "VisualStudio2012Dark";
            this.rlv_selectItem.ItemMouseDoubleClick += new Telerik.WinControls.UI.ListViewItemEventHandler(this.radListView1_ItemMouseDoubleClick);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.ForeColor = System.Drawing.Color.White;
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(2, 2);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Please Select";
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_OK.Location = new System.Drawing.Point(9, 380);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(82, 30);
            this.rbtn_OK.TabIndex = 2;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // lbl_new
            // 
            this.lbl_new.AutoSize = true;
            this.lbl_new.BackColor = System.Drawing.Color.Transparent;
            this.lbl_new.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_new.ForeColor = System.Drawing.Color.White;
            this.lbl_new.Location = new System.Drawing.Point(11, 330);
            this.lbl_new.Name = "lbl_new";
            this.lbl_new.Size = new System.Drawing.Size(45, 21);
            this.lbl_new.TabIndex = 3;
            this.lbl_new.Text = "New:";
            this.lbl_new.Visible = false;
            // 
            // txt_box1
            // 
            this.txt_box1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_box1.ForeColor = System.Drawing.Color.Chartreuse;
            this.txt_box1.Location = new System.Drawing.Point(62, 327);
            this.txt_box1.MaxLength = 10;
            this.txt_box1.Name = "txt_box1";
            this.txt_box1.Size = new System.Drawing.Size(119, 28);
            this.txt_box1.TabIndex = 15;
            this.txt_box1.ThemeName = "VisualStudio2012Dark";
            this.txt_box1.Visible = false;
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_box1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadTextBoxElement)(this.txt_box1.GetChildAt(0))).ForeColor = System.Drawing.Color.Chartreuse;
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // rbtn_delete
            // 
            this.rbtn_delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_delete.Location = new System.Drawing.Point(108, 380);
            this.rbtn_delete.Name = "rbtn_delete";
            this.rbtn_delete.Size = new System.Drawing.Size(82, 30);
            this.rbtn_delete.TabIndex = 2;
            this.rbtn_delete.Text = "Delete";
            this.rbtn_delete.ThemeName = "VisualStudio2012Dark";
            this.rbtn_delete.Click += new System.EventHandler(this.rbtn_delete_Click);
            // 
            // SelectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 427);
            this.Controls.Add(this.txt_box1);
            this.Controls.Add(this.lbl_new);
            this.Controls.Add(this.rbtn_delete);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.rlv_selectItem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "VisualStudio2012Dark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SelectList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rlv_selectItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_box1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadListView rlv_selectItem;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
        private System.Windows.Forms.Label lbl_new;
        private Telerik.WinControls.UI.RadTextBox txt_box1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Telerik.WinControls.UI.RadButton rbtn_delete;
    }
}