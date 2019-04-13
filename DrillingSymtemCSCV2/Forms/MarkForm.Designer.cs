namespace DrillingSymtemCSCV2.Forms
{
    partial class MarkForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_OK = new Telerik.WinControls.UI.RadButton();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.cbxReal = new System.Windows.Forms.CheckBox();
            this.cbxHistory = new System.Windows.Forms.CheckBox();
            this.pl_CalendarAndTime2 = new Telerik.WinControls.UI.RadPanel();
            this.radCalendar_end = new Telerik.WinControls.UI.RadCalendar();
            this.TimePick_NO2 = new Telerik.WinControls.UI.RadButton();
            this.TimePick_OK2 = new Telerik.WinControls.UI.RadButton();
            this.radTimePicker2 = new Telerik.WinControls.UI.RadTimePicker();
            this.tbx_time = new Telerik.WinControls.UI.RadTextBox();
            this.label_d = new System.Windows.Forms.Label();
            this.label_t = new System.Windows.Forms.Label();
            this.label_q = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pl_CalendarAndTime2)).BeginInit();
            this.pl_CalendarAndTime2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar_end)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_NO2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_OK2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTimePicker2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Gray;
            this.listBox1.Font = new System.Drawing.Font("宋体", 15F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Items.AddRange(new object[] {
            "1m",
            "5m",
            "10m",
            "20m",
            "25m",
            "50m"});
            this.listBox1.Location = new System.Drawing.Point(21, 37);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(125, 244);
            this.listBox1.TabIndex = 0;
            // 
            // btn_OK
            // 
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(349, 276);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(90, 24);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "确定";
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btn_OK.GetChildAt(0))).Text = "确定";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btn_OK.GetChildAt(0))).ForeColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).BoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).LeftColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).TopColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).RightColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).BottomColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).ForeColor2 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).ForeColor3 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).ForeColor4 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_OK.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.White;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Location = new System.Drawing.Point(462, 276);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 24);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_cancel);
            ((Telerik.WinControls.UI.RadButtonElement)(this.btn_Cancel.GetChildAt(0))).Text = "取消";
            ((Telerik.WinControls.UI.RadButtonElement)(this.btn_Cancel.GetChildAt(0))).ForeColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = false;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).BoxStyle = Telerik.WinControls.BorderBoxStyle.SingleBorder;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).LeftColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).TopColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).RightColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).BottomColor = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).ForeColor2 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).ForeColor3 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).ForeColor4 = System.Drawing.Color.White;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.btn_Cancel.GetChildAt(0).GetChildAt(2))).ForeColor = System.Drawing.Color.White;
            // 
            // listBox2
            // 
            this.listBox2.BackColor = System.Drawing.Color.Gray;
            this.listBox2.Font = new System.Drawing.Font("宋体", 15F);
            this.listBox2.FormattingEnabled = true;
            this.listBox2.ItemHeight = 20;
            this.listBox2.Items.AddRange(new object[] {
            "1 Min.",
            "2 Min.",
            "3 Min.",
            "5 Min.",
            "10 Min.",
            "20 Min.",
            "30 Min.",
            "1 Hr.",
            "2 Hr.",
            "4 Hr."});
            this.listBox2.Location = new System.Drawing.Point(177, 37);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(125, 244);
            this.listBox2.TabIndex = 2;
            // 
            // cbxReal
            // 
            this.cbxReal.AutoSize = true;
            this.cbxReal.ForeColor = System.Drawing.Color.White;
            this.cbxReal.Location = new System.Drawing.Point(349, 93);
            this.cbxReal.Name = "cbxReal";
            this.cbxReal.Size = new System.Drawing.Size(50, 17);
            this.cbxReal.TabIndex = 6;
            this.cbxReal.Text = "实时";
            this.cbxReal.UseVisualStyleBackColor = true;
            this.cbxReal.CheckedChanged += new System.EventHandler(this.cbxReal_CheckedChanged);
            // 
            // cbxHistory
            // 
            this.cbxHistory.AutoSize = true;
            this.cbxHistory.ForeColor = System.Drawing.Color.White;
            this.cbxHistory.Location = new System.Drawing.Point(349, 146);
            this.cbxHistory.Name = "cbxHistory";
            this.cbxHistory.Size = new System.Drawing.Size(50, 17);
            this.cbxHistory.TabIndex = 7;
            this.cbxHistory.Text = "历史";
            this.cbxHistory.UseVisualStyleBackColor = true;
            this.cbxHistory.CheckedChanged += new System.EventHandler(this.cbxHistory_CheckedChanged);
            // 
            // pl_CalendarAndTime2
            // 
            this.pl_CalendarAndTime2.Controls.Add(this.radCalendar_end);
            this.pl_CalendarAndTime2.Controls.Add(this.TimePick_NO2);
            this.pl_CalendarAndTime2.Controls.Add(this.TimePick_OK2);
            this.pl_CalendarAndTime2.Controls.Add(this.radTimePicker2);
            this.pl_CalendarAndTime2.Location = new System.Drawing.Point(95, 0);
            this.pl_CalendarAndTime2.Name = "pl_CalendarAndTime2";
            this.pl_CalendarAndTime2.Size = new System.Drawing.Size(401, 311);
            this.pl_CalendarAndTime2.TabIndex = 52;
            this.pl_CalendarAndTime2.Visible = false;
            // 
            // radCalendar_end
            // 
            this.radCalendar_end.Culture = new System.Globalization.CultureInfo("en");
            this.radCalendar_end.Location = new System.Drawing.Point(3, 3);
            this.radCalendar_end.Name = "radCalendar_end";
            this.radCalendar_end.Size = new System.Drawing.Size(394, 264);
            this.radCalendar_end.TabIndex = 31;
            this.radCalendar_end.Text = "radCalendar1";
            this.radCalendar_end.ThemeName = "VisualStudio2012Dark";
            // 
            // TimePick_NO2
            // 
            this.TimePick_NO2.Location = new System.Drawing.Point(319, 280);
            this.TimePick_NO2.Name = "TimePick_NO2";
            this.TimePick_NO2.Size = new System.Drawing.Size(78, 27);
            this.TimePick_NO2.TabIndex = 53;
            this.TimePick_NO2.Text = "取消";
            this.TimePick_NO2.ThemeName = "VisualStudio2012Dark";
            this.TimePick_NO2.Click += new System.EventHandler(this.TimePick_NO2_Click);
            // 
            // TimePick_OK2
            // 
            this.TimePick_OK2.Location = new System.Drawing.Point(228, 280);
            this.TimePick_OK2.Name = "TimePick_OK2";
            this.TimePick_OK2.Size = new System.Drawing.Size(78, 27);
            this.TimePick_OK2.TabIndex = 53;
            this.TimePick_OK2.Text = "确定";
            this.TimePick_OK2.ThemeName = "VisualStudio2012Dark";
            this.TimePick_OK2.Click += new System.EventHandler(this.TimePick_OK2_Click);
            // 
            // radTimePicker2
            // 
            this.radTimePicker2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radTimePicker2.Location = new System.Drawing.Point(3, 271);
            this.radTimePicker2.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 0);
            this.radTimePicker2.MinValue = new System.DateTime(((long)(0)));
            this.radTimePicker2.Name = "radTimePicker2";
            this.radTimePicker2.Size = new System.Drawing.Size(221, 36);
            this.radTimePicker2.Step = 30;
            this.radTimePicker2.TabIndex = 52;
            this.radTimePicker2.TabStop = false;
            this.radTimePicker2.Text = "radTimePicker1";
            this.radTimePicker2.ThemeName = "VisualStudio2012Dark";
            this.radTimePicker2.TimeTables = Telerik.WinControls.UI.TimeTables.HoursAndMinutesInOneTable;
            this.radTimePicker2.Value = null;
            // 
            // tbx_time
            // 
            this.tbx_time.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            this.tbx_time.ForeColor = System.Drawing.Color.White;
            this.tbx_time.Location = new System.Drawing.Point(349, 171);
            this.tbx_time.Name = "tbx_time";
            this.tbx_time.ReadOnly = true;
            this.tbx_time.Size = new System.Drawing.Size(235, 28);
            this.tbx_time.TabIndex = 53;
            this.tbx_time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbx_time.ThemeName = "VisualStudio2012Dark";
            this.tbx_time.Click += new System.EventHandler(this.tbx_time_Click);
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.tbx_time.GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.YellowGreen;
            ((Telerik.WinControls.UI.RadTextBoxItem)(this.tbx_time.GetChildAt(0).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.tbx_time.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.YellowGreen;
            // 
            // label_d
            // 
            this.label_d.AutoSize = true;
            this.label_d.ForeColor = System.Drawing.Color.White;
            this.label_d.Location = new System.Drawing.Point(21, 19);
            this.label_d.Name = "label_d";
            this.label_d.Size = new System.Drawing.Size(31, 13);
            this.label_d.TabIndex = 54;
            this.label_d.Text = "井深";
            // 
            // label_t
            // 
            this.label_t.AutoSize = true;
            this.label_t.ForeColor = System.Drawing.Color.White;
            this.label_t.Location = new System.Drawing.Point(177, 19);
            this.label_t.Name = "label_t";
            this.label_t.Size = new System.Drawing.Size(31, 13);
            this.label_t.TabIndex = 55;
            this.label_t.Text = "时间";
            // 
            // label_q
            // 
            this.label_q.AutoSize = true;
            this.label_q.ForeColor = System.Drawing.Color.White;
            this.label_q.Location = new System.Drawing.Point(524, 153);
            this.label_q.Name = "label_q";
            this.label_q.Size = new System.Drawing.Size(55, 13);
            this.label_q.TabIndex = 56;
            this.label_q.Text = "查询时间";
            this.label_q.Visible = false;
            // 
            // MarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.ClientSize = new System.Drawing.Size(597, 317);
            this.Controls.Add(this.label_q);
            this.Controls.Add(this.label_t);
            this.Controls.Add(this.label_d);
            this.Controls.Add(this.tbx_time);
            this.Controls.Add(this.pl_CalendarAndTime2);
            this.Controls.Add(this.cbxHistory);
            this.Controls.Add(this.cbxReal);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.listBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MarkForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置图表";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.MarkForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btn_OK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pl_CalendarAndTime2)).EndInit();
            this.pl_CalendarAndTime2.ResumeLayout(false);
            this.pl_CalendarAndTime2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCalendar_end)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_NO2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePick_OK2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTimePicker2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbx_time)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private Telerik.WinControls.UI.RadButton btn_OK;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.CheckBox cbxReal;
        private System.Windows.Forms.CheckBox cbxHistory;
        private Telerik.WinControls.UI.RadPanel pl_CalendarAndTime2;
        private Telerik.WinControls.UI.RadCalendar radCalendar_end;
        private Telerik.WinControls.UI.RadButton TimePick_NO2;
        private Telerik.WinControls.UI.RadButton TimePick_OK2;
        private Telerik.WinControls.UI.RadTimePicker radTimePicker2;
        private Telerik.WinControls.UI.RadTextBox tbx_time;
        private System.Windows.Forms.Label label_d;
        private System.Windows.Forms.Label label_t;
        private System.Windows.Forms.Label label_q;
    }
}