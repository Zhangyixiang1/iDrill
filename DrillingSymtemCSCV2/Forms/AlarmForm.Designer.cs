namespace DrillingSymtemCSCV2.Forms
{
    partial class AlarmForm
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
            Telerik.WinControls.UI.CartesianArea cartesianArea1 = new Telerik.WinControls.UI.CartesianArea();
            Telerik.WinControls.UI.CartesianArea cartesianArea2 = new Telerik.WinControls.UI.CartesianArea();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.gbx_AlarmSetting = new System.Windows.Forms.GroupBox();
            this.chb_H = new DrillingSymtemCSCV2.UserControls.Chb();
            this.chb_L = new DrillingSymtemCSCV2.UserControls.Chb();
            this.lbl_DrillId = new System.Windows.Forms.Label();
            this.lbl_TagId = new System.Windows.Forms.Label();
            this.lbl_Tags = new System.Windows.Forms.Label();
            this.lbl_Status = new Telerik.WinControls.UI.RadLabel();
            this.btn_Default = new Telerik.WinControls.UI.RadButton();
            this.btn_Save = new Telerik.WinControls.UI.RadButton();
            this.txt_LowAlarm = new System.Windows.Forms.TextBox();
            this.lbl_LowAlarm = new System.Windows.Forms.Label();
            this.txt_HighAlarm = new System.Windows.Forms.TextBox();
            this.lbl_HighAlarm = new System.Windows.Forms.Label();
            this.lst_channel = new System.Windows.Forms.ListBox();
            this.lbl_ChannelList = new System.Windows.Forms.Label();
            this.lbl_RecentlyAlarms = new System.Windows.Forms.Label();
            this.rcv_percent = new Telerik.WinControls.UI.RadChartView();
            this.rcv_top = new Telerik.WinControls.UI.RadChartView();
            this.gvw_RecentlyAlarms = new Telerik.WinControls.UI.RadGridView();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.gbx_AlarmSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Status)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Default)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcv_percent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcv_top)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_RecentlyAlarms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_RecentlyAlarms.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // dataShow21
            // 
            this.dataShow21.Visible = false;
            // 
            // dataShow22
            // 
            this.dataShow22.Visible = false;
            // 
            // dataShow23
            // 
            this.dataShow23.Visible = false;
            // 
            // dataShow24
            // 
            this.dataShow24.Visible = false;
            // 
            // dataShow25
            // 
            this.dataShow25.Visible = false;
            // 
            // gbx_AlarmSetting
            // 
            this.gbx_AlarmSetting.Controls.Add(this.chb_H);
            this.gbx_AlarmSetting.Controls.Add(this.chb_L);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_DrillId);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_TagId);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_Tags);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_Status);
            this.gbx_AlarmSetting.Controls.Add(this.btn_Default);
            this.gbx_AlarmSetting.Controls.Add(this.btn_Save);
            this.gbx_AlarmSetting.Controls.Add(this.txt_LowAlarm);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_LowAlarm);
            this.gbx_AlarmSetting.Controls.Add(this.txt_HighAlarm);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_HighAlarm);
            this.gbx_AlarmSetting.Controls.Add(this.lst_channel);
            this.gbx_AlarmSetting.Controls.Add(this.lbl_ChannelList);
            this.gbx_AlarmSetting.ForeColor = System.Drawing.Color.White;
            this.gbx_AlarmSetting.Location = new System.Drawing.Point(13, 85);
            this.gbx_AlarmSetting.Name = "gbx_AlarmSetting";
            this.gbx_AlarmSetting.Size = new System.Drawing.Size(391, 880);
            this.gbx_AlarmSetting.TabIndex = 26;
            this.gbx_AlarmSetting.TabStop = false;
            this.gbx_AlarmSetting.Text = "Alarm Setting";
            // 
            // chb_H
            // 
            this.chb_H.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chb_H.Checked = true;
            this.chb_H.Location = new System.Drawing.Point(344, 206);
            this.chb_H.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.chb_H.Name = "chb_H";
            this.chb_H.Size = new System.Drawing.Size(30, 30);
            this.chb_H.TabIndex = 38;
            // 
            // chb_L
            // 
            this.chb_L.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chb_L.Checked = true;
            this.chb_L.Location = new System.Drawing.Point(344, 143);
            this.chb_L.Margin = new System.Windows.Forms.Padding(5);
            this.chb_L.Name = "chb_L";
            this.chb_L.Size = new System.Drawing.Size(30, 30);
            this.chb_L.TabIndex = 38;
            // 
            // lbl_DrillId
            // 
            this.lbl_DrillId.AutoSize = true;
            this.lbl_DrillId.BackColor = System.Drawing.Color.Transparent;
            this.lbl_DrillId.ForeColor = System.Drawing.Color.Transparent;
            this.lbl_DrillId.Location = new System.Drawing.Point(238, 256);
            this.lbl_DrillId.Name = "lbl_DrillId";
            this.lbl_DrillId.Size = new System.Drawing.Size(50, 20);
            this.lbl_DrillId.TabIndex = 36;
            this.lbl_DrillId.Text = "DrillId";
            this.lbl_DrillId.Visible = false;
            // 
            // lbl_TagId
            // 
            this.lbl_TagId.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TagId.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_TagId.ForeColor = System.Drawing.Color.Transparent;
            this.lbl_TagId.Location = new System.Drawing.Point(237, 285);
            this.lbl_TagId.Name = "lbl_TagId";
            this.lbl_TagId.Size = new System.Drawing.Size(139, 25);
            this.lbl_TagId.TabIndex = 36;
            this.lbl_TagId.Text = "Tag";
            this.lbl_TagId.Visible = false;
            // 
            // lbl_Tags
            // 
            this.lbl_Tags.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Tags.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Tags.ForeColor = System.Drawing.Color.Transparent;
            this.lbl_Tags.Location = new System.Drawing.Point(236, 52);
            this.lbl_Tags.Name = "lbl_Tags";
            this.lbl_Tags.Size = new System.Drawing.Size(140, 50);
            this.lbl_Tags.TabIndex = 36;
            this.lbl_Tags.Text = "Tag";
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = false;
            this.lbl_Status.Font = new System.Drawing.Font("Segoe UI", 14.25F);
            this.lbl_Status.ForeColor = System.Drawing.Color.White;
            this.lbl_Status.Location = new System.Drawing.Point(228, 313);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(157, 93);
            this.lbl_Status.TabIndex = 0;
            this.lbl_Status.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Default
            // 
            this.btn_Default.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Default.Location = new System.Drawing.Point(222, 835);
            this.btn_Default.Name = "btn_Default";
            this.btn_Default.Size = new System.Drawing.Size(110, 30);
            this.btn_Default.TabIndex = 1;
            this.btn_Default.Text = "Default";
            this.btn_Default.ThemeName = "VisualStudio2012Dark";
            this.btn_Default.Visible = false;
            this.btn_Default.Click += new System.EventHandler(this.Default_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Save.Location = new System.Drawing.Point(50, 835);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(110, 30);
            this.btn_Save.TabIndex = 1;
            this.btn_Save.Text = "Save";
            this.btn_Save.ThemeName = "VisualStudio2012Dark";
            this.btn_Save.Click += new System.EventHandler(this.save_Click);
            // 
            // txt_LowAlarm
            // 
            this.txt_LowAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txt_LowAlarm.ForeColor = System.Drawing.Color.White;
            this.txt_LowAlarm.Location = new System.Drawing.Point(226, 143);
            this.txt_LowAlarm.MaxLength = 10;
            this.txt_LowAlarm.Name = "txt_LowAlarm";
            this.txt_LowAlarm.Size = new System.Drawing.Size(93, 21);
            this.txt_LowAlarm.TabIndex = 33;
            // 
            // lbl_LowAlarm
            // 
            this.lbl_LowAlarm.AutoSize = true;
            this.lbl_LowAlarm.ForeColor = System.Drawing.Color.White;
            this.lbl_LowAlarm.Location = new System.Drawing.Point(227, 112);
            this.lbl_LowAlarm.Name = "lbl_LowAlarm";
            this.lbl_LowAlarm.Size = new System.Drawing.Size(132, 20);
            this.lbl_LowAlarm.TabIndex = 32;
            this.lbl_LowAlarm.Text = "Low Alarm(Metric)";
            // 
            // txt_HighAlarm
            // 
            this.txt_HighAlarm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.txt_HighAlarm.ForeColor = System.Drawing.Color.White;
            this.txt_HighAlarm.Location = new System.Drawing.Point(226, 206);
            this.txt_HighAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.txt_HighAlarm.MaxLength = 10;
            this.txt_HighAlarm.Name = "txt_HighAlarm";
            this.txt_HighAlarm.Size = new System.Drawing.Size(93, 21);
            this.txt_HighAlarm.TabIndex = 31;
            // 
            // lbl_HighAlarm
            // 
            this.lbl_HighAlarm.AutoSize = true;
            this.lbl_HighAlarm.ForeColor = System.Drawing.Color.White;
            this.lbl_HighAlarm.Location = new System.Drawing.Point(227, 181);
            this.lbl_HighAlarm.Name = "lbl_HighAlarm";
            this.lbl_HighAlarm.Size = new System.Drawing.Size(137, 20);
            this.lbl_HighAlarm.TabIndex = 30;
            this.lbl_HighAlarm.Text = "High Alarm(Metric)";
            // 
            // lst_channel
            // 
            this.lst_channel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.lst_channel.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lst_channel.ForeColor = System.Drawing.Color.White;
            this.lst_channel.FormattingEnabled = true;
            this.lst_channel.HorizontalScrollbar = true;
            this.lst_channel.ItemHeight = 25;
            this.lst_channel.Location = new System.Drawing.Point(13, 49);
            this.lst_channel.Name = "lst_channel";
            this.lst_channel.Size = new System.Drawing.Size(208, 754);
            this.lst_channel.TabIndex = 27;
            // 
            // lbl_ChannelList
            // 
            this.lbl_ChannelList.AutoSize = true;
            this.lbl_ChannelList.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_ChannelList.ForeColor = System.Drawing.Color.White;
            this.lbl_ChannelList.Location = new System.Drawing.Point(16, 17);
            this.lbl_ChannelList.Name = "lbl_ChannelList";
            this.lbl_ChannelList.Size = new System.Drawing.Size(127, 28);
            this.lbl_ChannelList.TabIndex = 26;
            this.lbl_ChannelList.Text = "Channel List";
            // 
            // lbl_RecentlyAlarms
            // 
            this.lbl_RecentlyAlarms.AutoSize = true;
            this.lbl_RecentlyAlarms.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_RecentlyAlarms.ForeColor = System.Drawing.Color.White;
            this.lbl_RecentlyAlarms.Location = new System.Drawing.Point(425, 83);
            this.lbl_RecentlyAlarms.Name = "lbl_RecentlyAlarms";
            this.lbl_RecentlyAlarms.Size = new System.Drawing.Size(166, 28);
            this.lbl_RecentlyAlarms.TabIndex = 28;
            this.lbl_RecentlyAlarms.Text = "Recently Alarms";
            // 
            // rcv_percent
            // 
            this.rcv_percent.AreaDesign = cartesianArea1;
            this.rcv_percent.BackColor = System.Drawing.Color.White;
            this.rcv_percent.ForeColor = System.Drawing.Color.White;
            this.rcv_percent.Location = new System.Drawing.Point(1122, 87);
            this.rcv_percent.Margin = new System.Windows.Forms.Padding(2);
            this.rcv_percent.Name = "rcv_percent";
            this.rcv_percent.ShowGrid = false;
            this.rcv_percent.Size = new System.Drawing.Size(626, 420);
            this.rcv_percent.TabIndex = 29;
            this.rcv_percent.Text = "radChartView1";
            ((Telerik.WinControls.UI.RadChartElement)(this.rcv_percent.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.UI.ChartTitleElement)(this.rcv_percent.GetChildAt(0).GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // rcv_top
            // 
            this.rcv_top.AreaDesign = cartesianArea2;
            this.rcv_top.BackColor = System.Drawing.Color.White;
            this.rcv_top.ForeColor = System.Drawing.Color.White;
            this.rcv_top.Location = new System.Drawing.Point(1122, 544);
            this.rcv_top.Margin = new System.Windows.Forms.Padding(2);
            this.rcv_top.Name = "rcv_top";
            this.rcv_top.ShowGrid = false;
            this.rcv_top.Size = new System.Drawing.Size(626, 420);
            this.rcv_top.TabIndex = 30;
            this.rcv_top.Text = "radChartView2";
            ((Telerik.WinControls.UI.RadChartElement)(this.rcv_top.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            ((Telerik.WinControls.UI.ChartTitleElement)(this.rcv_top.GetChildAt(0).GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // gvw_RecentlyAlarms
            // 
            this.gvw_RecentlyAlarms.Location = new System.Drawing.Point(422, 118);
            this.gvw_RecentlyAlarms.Margin = new System.Windows.Forms.Padding(2);
            // 
            // 
            // 
            this.gvw_RecentlyAlarms.MasterTemplate.AllowCellContextMenu = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowColumnReorder = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowColumnResize = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowDragToGroup = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowEditRow = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowRowResize = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "Channel";
            gridViewTextBoxColumn1.Name = "Channel";
            gridViewTextBoxColumn1.Width = 188;
            gridViewTextBoxColumn2.HeaderText = "Message";
            gridViewTextBoxColumn2.Name = "Message";
            gridViewTextBoxColumn2.Width = 281;
            gridViewTextBoxColumn3.HeaderText = "Date";
            gridViewTextBoxColumn3.Name = "Date";
            gridViewTextBoxColumn3.Width = 206;
            this.gvw_RecentlyAlarms.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3});
            this.gvw_RecentlyAlarms.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.gvw_RecentlyAlarms.Name = "gvw_RecentlyAlarms";
            this.gvw_RecentlyAlarms.Size = new System.Drawing.Size(694, 846);
            this.gvw_RecentlyAlarms.TabIndex = 31;
            this.gvw_RecentlyAlarms.Text = "radGridView1";
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1910, 1007);
            this.Controls.Add(this.gvw_RecentlyAlarms);
            this.Controls.Add(this.rcv_top);
            this.Controls.Add(this.rcv_percent);
            this.Controls.Add(this.lbl_RecentlyAlarms);
            this.Controls.Add(this.gbx_AlarmSetting);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AlarmForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1920, 1080);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "s";
            this.Load += new System.EventHandler(this.AlarmForm_Load);
            this.Controls.SetChildIndex(this.dataShow21, 0);
            this.Controls.SetChildIndex(this.dataShow22, 0);
            this.Controls.SetChildIndex(this.dataShow23, 0);
            this.Controls.SetChildIndex(this.dataShow24, 0);
            this.Controls.SetChildIndex(this.dataShow25, 0);
            this.Controls.SetChildIndex(this.gbx_AlarmSetting, 0);
            this.Controls.SetChildIndex(this.lbl_RecentlyAlarms, 0);
            this.Controls.SetChildIndex(this.rcv_percent, 0);
            this.Controls.SetChildIndex(this.rcv_top, 0);
            this.Controls.SetChildIndex(this.gvw_RecentlyAlarms, 0);
            this.gbx_AlarmSetting.ResumeLayout(false);
            this.gbx_AlarmSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_Status)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Default)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcv_percent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rcv_top)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_RecentlyAlarms.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvw_RecentlyAlarms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbx_AlarmSetting;
        private System.Windows.Forms.TextBox txt_LowAlarm;
        private System.Windows.Forms.Label lbl_LowAlarm;
        private System.Windows.Forms.TextBox txt_HighAlarm;
        private System.Windows.Forms.Label lbl_HighAlarm;
        private System.Windows.Forms.ListBox lst_channel;
        private System.Windows.Forms.Label lbl_ChannelList;
        private System.Windows.Forms.Label lbl_RecentlyAlarms;
        private Telerik.WinControls.UI.RadChartView rcv_percent;
        private Telerik.WinControls.UI.RadChartView rcv_top;
        private Telerik.WinControls.UI.RadGridView gvw_RecentlyAlarms;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadButton btn_Default;
        private Telerik.WinControls.UI.RadButton btn_Save;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadLabel lbl_Status;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Label lbl_Tags;
        private System.Windows.Forms.Label lbl_DrillId;
        private System.Windows.Forms.Label lbl_TagId;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
        private UserControls.Chb chb_L;
        private UserControls.Chb chb_H;

    }
}