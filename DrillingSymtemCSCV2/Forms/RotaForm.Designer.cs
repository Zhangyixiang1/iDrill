namespace DrillingSymtemCSCV2.Forms
{
    partial class RotaForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RotaForm));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rlbl_onDutyInQuiry = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_startDate = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_endate = new Telerik.WinControls.UI.RadLabel();
            this.StartTime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.EndTime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.rbtn_select = new Telerik.WinControls.UI.RadButton();
            this.btn_save = new Telerik.WinControls.UI.RadButton();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.dgv_rota = new System.Windows.Forms.DataGridView();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.calendarColumn1 = new DrillingSymtemCSCV2.UserControls.CalendarColumn();
            this.calendarColumn2 = new DrillingSymtemCSCV2.UserControls.CalendarColumn();
            this.rbtn_workerManagement = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_onDutyInQuiry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_startDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_endate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_workerManagement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // rlbl_onDutyInQuiry
            // 
            this.rlbl_onDutyInQuiry.AutoSize = false;
            this.rlbl_onDutyInQuiry.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_onDutyInQuiry.ForeColor = System.Drawing.Color.White;
            this.rlbl_onDutyInQuiry.Location = new System.Drawing.Point(3, 11);
            this.rlbl_onDutyInQuiry.Name = "rlbl_onDutyInQuiry";
            this.rlbl_onDutyInQuiry.Size = new System.Drawing.Size(157, 21);
            this.rlbl_onDutyInQuiry.TabIndex = 3;
            this.rlbl_onDutyInQuiry.Text = "On duty inquiry:";
            // 
            // rlbl_startDate
            // 
            this.rlbl_startDate.AutoSize = false;
            this.rlbl_startDate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_startDate.ForeColor = System.Drawing.Color.White;
            this.rlbl_startDate.Location = new System.Drawing.Point(134, 11);
            this.rlbl_startDate.Name = "rlbl_startDate";
            this.rlbl_startDate.Size = new System.Drawing.Size(83, 21);
            this.rlbl_startDate.TabIndex = 4;
            this.rlbl_startDate.Text = "StartDate";
            // 
            // rlbl_endate
            // 
            this.rlbl_endate.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_endate.ForeColor = System.Drawing.Color.White;
            this.rlbl_endate.Location = new System.Drawing.Point(492, 11);
            this.rlbl_endate.Name = "rlbl_endate";
            this.rlbl_endate.Size = new System.Drawing.Size(69, 25);
            this.rlbl_endate.TabIndex = 4;
            this.rlbl_endate.Text = "EndDate";
            // 
            // StartTime
            // 
            this.StartTime.Culture = new System.Globalization.CultureInfo("en-US");
            this.StartTime.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.StartTime.Location = new System.Drawing.Point(224, 9);
            this.StartTime.Name = "StartTime";
            this.StartTime.Size = new System.Drawing.Size(211, 28);
            this.StartTime.TabIndex = 5;
            this.StartTime.TabStop = false;
            this.StartTime.Text = "Sunday, January 01, 2017";
            this.StartTime.ThemeName = "VisualStudio2012Dark";
            this.StartTime.Value = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            // 
            // EndTime
            // 
            this.EndTime.Culture = new System.Globalization.CultureInfo("en");
            this.EndTime.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.EndTime.Location = new System.Drawing.Point(581, 10);
            this.EndTime.Name = "EndTime";
            this.EndTime.Size = new System.Drawing.Size(211, 28);
            this.EndTime.TabIndex = 5;
            this.EndTime.TabStop = false;
            this.EndTime.Text = "Tuesday, June 06, 2017";
            this.EndTime.ThemeName = "VisualStudio2012Dark";
            this.EndTime.Value = new System.DateTime(2017, 6, 6, 16, 1, 34, 730);
            // 
            // radLabel4
            // 
            this.radLabel4.Font = new System.Drawing.Font("Segoe UI", 16F);
            this.radLabel4.ForeColor = System.Drawing.Color.White;
            this.radLabel4.Location = new System.Drawing.Point(456, 4);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(25, 33);
            this.radLabel4.TabIndex = 4;
            this.radLabel4.Text = "~";
            // 
            // rbtn_select
            // 
            this.rbtn_select.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_select.Location = new System.Drawing.Point(823, 7);
            this.rbtn_select.Name = "rbtn_select";
            this.rbtn_select.Size = new System.Drawing.Size(121, 32);
            this.rbtn_select.TabIndex = 6;
            this.rbtn_select.Text = "Select";
            this.rbtn_select.ThemeName = "VisualStudio2012Dark";
            this.rbtn_select.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // btn_save
            // 
            this.btn_save.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_save.Location = new System.Drawing.Point(967, 7);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(121, 32);
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "Save";
            this.btn_save.ThemeName = "VisualStudio2012Dark";
            this.btn_save.Click += new System.EventHandler(this.Save_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Cancel.Location = new System.Drawing.Point(1114, 7);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(121, 32);
            this.btn_Cancel.TabIndex = 6;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // dgv_rota
            // 
            this.dgv_rota.AllowUserToAddRows = false;
            this.dgv_rota.AllowUserToDeleteRows = false;
            this.dgv_rota.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.dgv_rota.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_rota.Location = new System.Drawing.Point(3, 46);
            this.dgv_rota.Name = "dgv_rota";
            this.dgv_rota.RowTemplate.Height = 23;
            this.dgv_rota.Size = new System.Drawing.Size(1786, 714);
            this.dgv_rota.TabIndex = 2;
            this.dgv_rota.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dgv_rota.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            // 
            // backgroundWorker2
            // 
            this.backgroundWorker2.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker2_DoWork);
            this.backgroundWorker2.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker2_RunWorkerCompleted);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "TypeWork";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Content";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Name";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 150;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Content";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 150;
            // 
            // calendarColumn1
            // 
            this.calendarColumn1.HeaderText = "StartTime";
            this.calendarColumn1.Name = "calendarColumn1";
            this.calendarColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.calendarColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.calendarColumn1.Width = 150;
            // 
            // calendarColumn2
            // 
            this.calendarColumn2.HeaderText = "EndTime";
            this.calendarColumn2.Name = "calendarColumn2";
            this.calendarColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.calendarColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.calendarColumn2.Width = 150;
            // 
            // rbtn_workerManagement
            // 
            this.rbtn_workerManagement.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rbtn_workerManagement.Location = new System.Drawing.Point(1556, 6);
            this.rbtn_workerManagement.Name = "rbtn_workerManagement";
            this.rbtn_workerManagement.Size = new System.Drawing.Size(177, 33);
            this.rbtn_workerManagement.TabIndex = 7;
            this.rbtn_workerManagement.Text = "WorkerManagement";
            this.rbtn_workerManagement.ThemeName = "VisualStudio2012Dark";
            this.rbtn_workerManagement.Click += new System.EventHandler(this.rbtn_workerManagement_Click);
            // 
            // RotaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1792, 867);
            this.Controls.Add(this.rbtn_workerManagement);
            this.Controls.Add(this.StartTime);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.rbtn_select);
            this.Controls.Add(this.EndTime);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.rlbl_endate);
            this.Controls.Add(this.rlbl_startDate);
            this.Controls.Add(this.rlbl_onDutyInQuiry);
            this.Controls.Add(this.dgv_rota);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RotaForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RotaForm_FormClosing);
            this.Load += new System.EventHandler(this.RotaForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_onDutyInQuiry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_startDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_endate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EndTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_rota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_workerManagement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private UserControls.CalendarColumn calendarColumn1;
        private UserControls.CalendarColumn calendarColumn2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private Telerik.WinControls.UI.RadLabel rlbl_onDutyInQuiry;
        private Telerik.WinControls.UI.RadLabel rlbl_startDate;
        private Telerik.WinControls.UI.RadLabel rlbl_endate;
        private Telerik.WinControls.UI.RadDateTimePicker StartTime;
        private Telerik.WinControls.UI.RadDateTimePicker EndTime;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadButton rbtn_select;
        private Telerik.WinControls.UI.RadButton btn_save;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.Windows.Forms.DataGridView dgv_rota;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private Telerik.WinControls.UI.RadButton rbtn_workerManagement;
    }
}