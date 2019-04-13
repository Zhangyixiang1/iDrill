namespace DrillingSymtemCSCV2.Forms
{
    partial class SelectTable
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTable));
            this.gvw_selectTable = new Telerik.WinControls.UI.RadGridView();
            this.rbtn_OK = new Telerik.WinControls.UI.RadButton();
            this.rbtn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.lbl_Select = new Telerik.WinControls.UI.RadLabel();
            this.lbl_NO = new Telerik.WinControls.UI.RadLabel();
            this.txt_NO = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_item = new Telerik.WinControls.UI.RadLabel();
            this.txt_item = new Telerik.WinControls.UI.RadTextBox();
            this.lbl_length = new Telerik.WinControls.UI.RadLabel();
            this.txt_length = new Telerik.WinControls.UI.RadTextBox();
            this.rbtn_delete = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_selectTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_selectTable.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_item)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_item)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_length)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_delete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // gvw_selectTable
            // 
            this.gvw_selectTable.AutoScroll = true;
            this.gvw_selectTable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.gvw_selectTable.Location = new System.Drawing.Point(3, 9);
            // 
            // gvw_selectTable
            // 
            this.gvw_selectTable.MasterTemplate.AllowAddNewRow = false;
            this.gvw_selectTable.MasterTemplate.AllowDeleteRow = false;
            this.gvw_selectTable.MasterTemplate.AllowDragToGroup = false;
            this.gvw_selectTable.MasterTemplate.AllowEditRow = false;
            this.gvw_selectTable.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.gvw_selectTable.MasterTemplate.EnableSorting = false;
            this.gvw_selectTable.MasterTemplate.ShowFilteringRow = false;
            this.gvw_selectTable.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvw_selectTable.Name = "gvw_selectTable";
            this.gvw_selectTable.ReadOnly = true;
            this.gvw_selectTable.ShowGroupPanel = false;
            this.gvw_selectTable.Size = new System.Drawing.Size(586, 268);
            this.gvw_selectTable.TabIndex = 7;
            this.gvw_selectTable.Text = "radGridView2";
            this.gvw_selectTable.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_OK
            // 
            this.rbtn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_OK.Location = new System.Drawing.Point(160, 357);
            this.rbtn_OK.Name = "rbtn_OK";
            this.rbtn_OK.Size = new System.Drawing.Size(108, 33);
            this.rbtn_OK.TabIndex = 30;
            this.rbtn_OK.Text = "OK";
            this.rbtn_OK.ThemeName = "VisualStudio2012Dark";
            this.rbtn_OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // rbtn_Cancel
            // 
            this.rbtn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_Cancel.Location = new System.Drawing.Point(320, 357);
            this.rbtn_Cancel.Name = "rbtn_Cancel";
            this.rbtn_Cancel.Size = new System.Drawing.Size(108, 33);
            this.rbtn_Cancel.TabIndex = 31;
            this.rbtn_Cancel.Text = "Cancel";
            this.rbtn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.rbtn_Cancel.Click += new System.EventHandler(this.Remove_Click);
            // 
            // lbl_Select
            // 
            this.lbl_Select.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Select.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Select.ForeColor = System.Drawing.Color.White;
            this.lbl_Select.Location = new System.Drawing.Point(4, 283);
            this.lbl_Select.Name = "lbl_Select";
            this.lbl_Select.Size = new System.Drawing.Size(52, 30);
            this.lbl_Select.TabIndex = 32;
            this.lbl_Select.Text = "New:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Select.GetChildAt(0))).Text = "New:";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_Select.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // lbl_NO
            // 
            this.lbl_NO.AutoSize = false;
            this.lbl_NO.BackColor = System.Drawing.Color.Transparent;
            this.lbl_NO.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NO.ForeColor = System.Drawing.Color.White;
            this.lbl_NO.Location = new System.Drawing.Point(-1, 314);
            this.lbl_NO.Name = "lbl_NO";
            this.lbl_NO.Size = new System.Drawing.Size(54, 30);
            this.lbl_NO.TabIndex = 32;
            this.lbl_NO.Text = "NO.";
            this.lbl_NO.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_NO.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_NO.GetChildAt(0))).Text = "NO.";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_NO.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // txt_NO
            // 
            this.txt_NO.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_NO.Location = new System.Drawing.Point(59, 318);
            this.txt_NO.MaxLength = 10;
            this.txt_NO.Name = "txt_NO";
            this.txt_NO.Size = new System.Drawing.Size(100, 24);
            this.txt_NO.TabIndex = 33;
            this.txt_NO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_NO.ThemeName = "VisualStudio2012Dark";
            // 
            // lbl_item
            // 
            this.lbl_item.AutoSize = false;
            this.lbl_item.BackColor = System.Drawing.Color.Transparent;
            this.lbl_item.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_item.ForeColor = System.Drawing.Color.White;
            this.lbl_item.Location = new System.Drawing.Point(165, 314);
            this.lbl_item.Name = "lbl_item";
            this.lbl_item.Size = new System.Drawing.Size(68, 30);
            this.lbl_item.TabIndex = 32;
            this.lbl_item.Text = "ITEM";
            this.lbl_item.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_item.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_item.GetChildAt(0))).Text = "ITEM";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_item.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // txt_item
            // 
            this.txt_item.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_item.Location = new System.Drawing.Point(239, 318);
            this.txt_item.MaxLength = 10;
            this.txt_item.Name = "txt_item";
            this.txt_item.Size = new System.Drawing.Size(156, 24);
            this.txt_item.TabIndex = 33;
            this.txt_item.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_item.ThemeName = "VisualStudio2012Dark";
            // 
            // lbl_length
            // 
            this.lbl_length.AutoSize = false;
            this.lbl_length.BackColor = System.Drawing.Color.Transparent;
            this.lbl_length.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_length.ForeColor = System.Drawing.Color.White;
            this.lbl_length.Location = new System.Drawing.Point(401, 314);
            this.lbl_length.Name = "lbl_length";
            this.lbl_length.Size = new System.Drawing.Size(81, 30);
            this.lbl_length.TabIndex = 32;
            this.lbl_length.Text = "LENGTH";
            this.lbl_length.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_length.GetChildAt(0))).TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_length.GetChildAt(0))).Text = "LENGTH";
            ((Telerik.WinControls.UI.RadLabelElement)(this.lbl_length.GetChildAt(0))).BackColor = System.Drawing.Color.Transparent;
            // 
            // txt_length
            // 
            this.txt_length.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_length.Location = new System.Drawing.Point(486, 318);
            this.txt_length.MaxLength = 10;
            this.txt_length.Name = "txt_length";
            this.txt_length.Size = new System.Drawing.Size(103, 24);
            this.txt_length.TabIndex = 33;
            this.txt_length.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txt_length.ThemeName = "VisualStudio2012Dark";
            // 
            // rbtn_delete
            // 
            this.rbtn_delete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtn_delete.Location = new System.Drawing.Point(507, 358);
            this.rbtn_delete.Name = "rbtn_delete";
            this.rbtn_delete.Size = new System.Drawing.Size(82, 28);
            this.rbtn_delete.TabIndex = 31;
            this.rbtn_delete.Text = "Delete";
            this.rbtn_delete.ThemeName = "VisualStudio2012Dark";
            this.rbtn_delete.Click += new System.EventHandler(this.rbtn_delete_Click);
            // 
            // SelectTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 398);
            this.Controls.Add(this.txt_length);
            this.Controls.Add(this.txt_item);
            this.Controls.Add(this.txt_NO);
            this.Controls.Add(this.lbl_length);
            this.Controls.Add(this.lbl_item);
            this.Controls.Add(this.lbl_NO);
            this.Controls.Add(this.lbl_Select);
            this.Controls.Add(this.rbtn_OK);
            this.Controls.Add(this.rbtn_delete);
            this.Controls.Add(this.rbtn_Cancel);
            this.Controls.Add(this.gvw_selectTable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectTable";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SelectTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvw_selectTable.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_selectTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_NO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_item)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_item)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_length)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_delete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView gvw_selectTable;
        private Telerik.WinControls.UI.RadButton rbtn_OK;
        private Telerik.WinControls.UI.RadButton rbtn_Cancel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadLabel lbl_Select;
        private Telerik.WinControls.UI.RadLabel lbl_NO;
        private Telerik.WinControls.UI.RadTextBox txt_NO;
        private Telerik.WinControls.UI.RadLabel lbl_item;
        private Telerik.WinControls.UI.RadTextBox txt_item;
        private Telerik.WinControls.UI.RadLabel lbl_length;
        private Telerik.WinControls.UI.RadTextBox txt_length;
        private Telerik.WinControls.UI.RadButton rbtn_delete;
    }
}