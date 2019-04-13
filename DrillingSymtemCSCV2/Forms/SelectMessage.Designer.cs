namespace DrillingSymtemCSCV2.Forms
{
    partial class SelectMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectMessage));
            this.btn_Select = new Telerik.WinControls.UI.RadButton();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.tab_tag = new System.Windows.Forms.TabControl();
            this.AcitvityStatusPage = new System.Windows.Forms.TabPage();
            this.radCalendar_start = new Telerik.WinControls.UI.RadCalendar();
            this.rtxt_message = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_message = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.rbtn_keyborad = new Telerik.WinControls.UI.RadButton();
            this.rlbl_time = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_time = new Telerik.WinControls.UI.RadTextBox();
            this.pl_CalendarAndTime = new Telerik.WinControls.UI.RadPanel();
            this.TimePick_No = new Telerik.WinControls.UI.RadButton();
            this.TimePick_OK = new Telerik.WinControls.UI.RadButton();
            this.radTimePicker1 = new Telerik.WinControls.UI.RadTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Select)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            this.tab_tag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar_start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_message)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_message)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_keyborad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pl_CalendarAndTime)).BeginInit();
            this.pl_CalendarAndTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTimePicker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Select
            // 
            this.btn_Select.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Select.Location = new System.Drawing.Point(275, 658);
            this.btn_Select.Name = "btn_Select";
            this.btn_Select.Size = new System.Drawing.Size(132, 43);
            this.btn_Select.TabIndex = 8;
            this.btn_Select.Text = "Add";
            this.btn_Select.ThemeName = "VisualStudio2012Dark";
            this.btn_Select.Click += new System.EventHandler(this.btn_Select_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Cancel.Location = new System.Drawing.Point(509, 658);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(132, 43);
            this.btn_Cancel.TabIndex = 9;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // tab_tag
            // 
            this.tab_tag.Controls.Add(this.AcitvityStatusPage);
            this.tab_tag.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tab_tag.Location = new System.Drawing.Point(15, 6);
            this.tab_tag.Name = "tab_tag";
            this.tab_tag.SelectedIndex = 0;
            this.tab_tag.Size = new System.Drawing.Size(832, 513);
            this.tab_tag.TabIndex = 10;
            // 
            // AcitvityStatusPage
            // 
            this.AcitvityStatusPage.AutoScroll = true;
            this.AcitvityStatusPage.BackColor = System.Drawing.Color.Black;
            this.AcitvityStatusPage.Location = new System.Drawing.Point(4, 26);
            this.AcitvityStatusPage.Name = "AcitvityStatusPage";
            this.AcitvityStatusPage.Size = new System.Drawing.Size(824, 483);
            this.AcitvityStatusPage.TabIndex = 4;
            this.AcitvityStatusPage.Text = "Activity";
            // 
            // radCalendar_start
            // 
            this.radCalendar_start.Culture = new System.Globalization.CultureInfo("en");
            this.radCalendar_start.Location = new System.Drawing.Point(2, 3);
            this.radCalendar_start.Name = "radCalendar_start";
            this.radCalendar_start.Size = new System.Drawing.Size(470, 327);
            this.radCalendar_start.TabIndex = 31;
            this.radCalendar_start.Text = "radCalendar1";
            this.radCalendar_start.ThemeName = "VisualStudio2012Dark";
            // 
            // rtxt_message
            // 
            this.rtxt_message.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_message.Location = new System.Drawing.Point(274, 544);
            this.rtxt_message.MaxLength = 50;
            this.rtxt_message.Name = "rtxt_message";
            this.rtxt_message.Size = new System.Drawing.Size(367, 28);
            this.rtxt_message.TabIndex = 11;
            this.rtxt_message.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_message
            // 
            this.rlbl_message.AutoSize = false;
            this.rlbl_message.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_message.ForeColor = System.Drawing.Color.White;
            this.rlbl_message.Location = new System.Drawing.Point(150, 548);
            this.rlbl_message.Name = "rlbl_message";
            this.rlbl_message.Size = new System.Drawing.Size(120, 30);
            this.rlbl_message.TabIndex = 12;
            this.rlbl_message.Text = "Message：";
            this.rlbl_message.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rbtn_keyborad
            // 
            this.rbtn_keyborad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtn_keyborad.BackColor = System.Drawing.Color.Black;
            this.rbtn_keyborad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtn_keyborad.Image = ((System.Drawing.Image)(resources.GetObject("rbtn_keyborad.Image")));
            this.rbtn_keyborad.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_keyborad.ImageScalingSize = new System.Drawing.Size(5, 5);
            this.rbtn_keyborad.Location = new System.Drawing.Point(647, 552);
            this.rbtn_keyborad.Name = "rbtn_keyborad";
            this.rbtn_keyborad.Size = new System.Drawing.Size(32, 21);
            this.rbtn_keyborad.TabIndex = 13;
            this.rbtn_keyborad.ThemeName = "VisualStudio2012Dark";
            this.rbtn_keyborad.Click += new System.EventHandler(this.rbtn_keyboard_Click);
            // 
            // rlbl_time
            // 
            this.rlbl_time.AutoSize = false;
            this.rlbl_time.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_time.ForeColor = System.Drawing.Color.White;
            this.rlbl_time.Location = new System.Drawing.Point(150, 596);
            this.rlbl_time.Name = "rlbl_time";
            this.rlbl_time.Size = new System.Drawing.Size(120, 30);
            this.rlbl_time.TabIndex = 14;
            this.rlbl_time.Text = "Time：";
            this.rlbl_time.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_time
            // 
            this.rtxt_time.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rtxt_time.Location = new System.Drawing.Point(274, 593);
            this.rtxt_time.MaxLength = 50;
            this.rtxt_time.Name = "rtxt_time";
            this.rtxt_time.Size = new System.Drawing.Size(367, 28);
            this.rtxt_time.TabIndex = 13;
            this.rtxt_time.ThemeName = "VisualStudio2012Dark";
            this.rtxt_time.Click += new System.EventHandler(this.StartTime_Click);
            // 
            // pl_CalendarAndTime
            // 
            this.pl_CalendarAndTime.Controls.Add(this.TimePick_No);
            this.pl_CalendarAndTime.Controls.Add(this.TimePick_OK);
            this.pl_CalendarAndTime.Controls.Add(this.radTimePicker1);
            this.pl_CalendarAndTime.Controls.Add(this.radCalendar_start);
            this.pl_CalendarAndTime.Location = new System.Drawing.Point(153, 38);
            this.pl_CalendarAndTime.Name = "pl_CalendarAndTime";
            this.pl_CalendarAndTime.Size = new System.Drawing.Size(475, 370);
            this.pl_CalendarAndTime.TabIndex = 52;
            this.pl_CalendarAndTime.Visible = false;
            // 
            // TimePick_No
            // 
            this.TimePick_No.Location = new System.Drawing.Point(357, 336);
            this.TimePick_No.Name = "TimePick_No";
            this.TimePick_No.Size = new System.Drawing.Size(88, 27);
            this.TimePick_No.TabIndex = 53;
            this.TimePick_No.Text = "取消";
            this.TimePick_No.ThemeName = "VisualStudio2012Dark";
            this.TimePick_No.Click += new System.EventHandler(this.TimePick_No_Click);
            // 
            // TimePick_OK
            // 
            this.TimePick_OK.Location = new System.Drawing.Point(252, 336);
            this.TimePick_OK.Name = "TimePick_OK";
            this.TimePick_OK.Size = new System.Drawing.Size(88, 27);
            this.TimePick_OK.TabIndex = 54;
            this.TimePick_OK.Text = "确定";
            this.TimePick_OK.ThemeName = "VisualStudio2012Dark";
            this.TimePick_OK.Click += new System.EventHandler(this.TimePick_OK_Click);
            // 
            // radTimePicker1
            // 
            this.radTimePicker1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTimePicker1.Location = new System.Drawing.Point(3, 334);
            this.radTimePicker1.Name = "radTimePicker1";
            this.radTimePicker1.Size = new System.Drawing.Size(221, 36);
            this.radTimePicker1.Step = 30;
            this.radTimePicker1.TabIndex = 55;
            this.radTimePicker1.TabStop = false;
            this.radTimePicker1.Text = "radTimePicker1";
            this.radTimePicker1.ThemeName = "VisualStudio2012Dark";
            this.radTimePicker1.TimeTables = Telerik.WinControls.UI.TimeTables.HoursAndMinutesInOneTable;
            this.radTimePicker1.Value = null;
            // 
            // SelectMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 712);
            this.Controls.Add(this.pl_CalendarAndTime);
            this.Controls.Add(this.rlbl_time);
            this.Controls.Add(this.rtxt_time);
            this.Controls.Add(this.rbtn_keyborad);
            this.Controls.Add(this.rlbl_message);
            this.Controls.Add(this.rtxt_message);
            this.Controls.Add(this.tab_tag);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Select);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectMessage";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.SelectMessage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_Select)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            this.tab_tag.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar_start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_message)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_message)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_keyborad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pl_CalendarAndTime)).EndInit();
            this.pl_CalendarAndTime.ResumeLayout(false);
            this.pl_CalendarAndTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTimePicker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btn_Select;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.Windows.Forms.TabControl tab_tag;
        private System.Windows.Forms.TabPage AcitvityStatusPage;
        private Telerik.WinControls.UI.RadTextBox rtxt_message;
        private Telerik.WinControls.UI.RadLabel rlbl_message;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_keyborad;
        private Telerik.WinControls.UI.RadLabel rlbl_time;
        private Telerik.WinControls.UI.RadTextBox rtxt_time;
        private Telerik.WinControls.UI.RadPanel pl_CalendarAndTime;
        private Telerik.WinControls.UI.RadButton TimePick_No;
        private Telerik.WinControls.UI.RadButton TimePick_OK;
        private Telerik.WinControls.UI.RadTimePicker radTimePicker1;
        private Telerik.WinControls.UI.RadCalendar radCalendar_start;
    }
}