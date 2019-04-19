namespace DrillingSymtemCSCV2.Forms
{
    partial class Frm_DisplaySetting
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
            this.components = new System.ComponentModel.Container();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.btn_confirm1 = new System.Windows.Forms.Button();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_confirm2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radListView1 = new Telerik.WinControls.UI.RadListView();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.lbl_dis2 = new System.Windows.Forms.Label();
            this.lbl_dis3 = new System.Windows.Forms.Label();
            this.lbl_dis4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.displayTv1 = new DrillingSymtemCSCV2.UserControls.DisplayTv();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.btn_confirm1);
            this.radGroupBox1.Controls.Add(this.comboBox4);
            this.radGroupBox1.Controls.Add(this.comboBox3);
            this.radGroupBox1.Controls.Add(this.comboBox2);
            this.radGroupBox1.Controls.Add(this.comboBox1);
            this.radGroupBox1.Controls.Add(this.label4);
            this.radGroupBox1.Controls.Add(this.label3);
            this.radGroupBox1.Controls.Add(this.label2);
            this.radGroupBox1.Controls.Add(this.label1);
            this.radGroupBox1.HeaderText = "固定输入源";
            this.radGroupBox1.Location = new System.Drawing.Point(520, 21);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(201, 275);
            this.radGroupBox1.TabIndex = 1;
            this.radGroupBox1.Text = "固定输入源";
            ((Telerik.WinControls.UI.GroupBoxHeader)(this.radGroupBox1.GetChildAt(0).GetChildAt(1))).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Standard;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radGroupBox1.GetChildAt(0).GetChildAt(1).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // btn_confirm1
            // 
            this.btn_confirm1.Location = new System.Drawing.Point(20, 238);
            this.btn_confirm1.Name = "btn_confirm1";
            this.btn_confirm1.Size = new System.Drawing.Size(75, 23);
            this.btn_confirm1.TabIndex = 2;
            this.btn_confirm1.Text = "应用";
            this.btn_confirm1.UseVisualStyleBackColor = true;
            this.btn_confirm1.Click += new System.EventHandler(this.btn_confirm1_Click);
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point(53, 181);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 20);
            this.comboBox4.TabIndex = 1;
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(53, 139);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 20);
            this.comboBox3.TabIndex = 1;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(53, 95);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "大屏地图"});
            this.comboBox1.Location = new System.Drawing.Point(53, 47);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "大屏地图";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(16, 172);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 28);
            this.label4.TabIndex = 0;
            this.label4.Text = "4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(16, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "1";
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.label5);
            this.radGroupBox2.Controls.Add(this.btn_confirm2);
            this.radGroupBox2.Controls.Add(this.textBox1);
            this.radGroupBox2.Controls.Add(this.radListView1);
            this.radGroupBox2.HeaderText = "滚动列表";
            this.radGroupBox2.Location = new System.Drawing.Point(737, 21);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(195, 275);
            this.radGroupBox2.TabIndex = 1;
            this.radGroupBox2.Text = "滚动列表";
            ((Telerik.WinControls.UI.GroupBoxHeader)(this.radGroupBox2.GetChildAt(0).GetChildAt(1))).GroupBoxStyle = Telerik.WinControls.UI.RadGroupBoxStyle.Standard;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(243)))), ((int)(((byte)(247)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radGroupBox2.GetChildAt(0).GetChildAt(1).GetChildAt(0))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "间隔";
            // 
            // btn_confirm2
            // 
            this.btn_confirm2.Location = new System.Drawing.Point(103, 239);
            this.btn_confirm2.Name = "btn_confirm2";
            this.btn_confirm2.Size = new System.Drawing.Size(75, 23);
            this.btn_confirm2.TabIndex = 2;
            this.btn_confirm2.Text = "应用";
            this.btn_confirm2.UseVisualStyleBackColor = true;
            this.btn_confirm2.Click += new System.EventHandler(this.btn_confirm2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(47, 240);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(48, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "5";
            // 
            // radListView1
            // 
            this.radListView1.Location = new System.Drawing.Point(5, 21);
            this.radListView1.Name = "radListView1";
            this.radListView1.Size = new System.Drawing.Size(185, 212);
            this.radListView1.TabIndex = 0;
            this.radListView1.Text = "radListView1";
            this.radListView1.ThemeName = "VisualStudio2012Dark";
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(737, 307);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 2;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(840, 307);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 2;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // lbl_dis2
            // 
            this.lbl_dis2.AutoSize = true;
            this.lbl_dis2.ForeColor = System.Drawing.Color.White;
            this.lbl_dis2.Location = new System.Drawing.Point(459, 30);
            this.lbl_dis2.Name = "lbl_dis2";
            this.lbl_dis2.Size = new System.Drawing.Size(38, 13);
            this.lbl_dis2.TabIndex = 3;
            this.lbl_dis2.Text = "label6";
            // 
            // lbl_dis3
            // 
            this.lbl_dis3.AutoSize = true;
            this.lbl_dis3.ForeColor = System.Drawing.Color.White;
            this.lbl_dis3.Location = new System.Drawing.Point(213, 167);
            this.lbl_dis3.Name = "lbl_dis3";
            this.lbl_dis3.Size = new System.Drawing.Size(38, 13);
            this.lbl_dis3.TabIndex = 3;
            this.lbl_dis3.Text = "label6";
            // 
            // lbl_dis4
            // 
            this.lbl_dis4.AutoSize = true;
            this.lbl_dis4.ForeColor = System.Drawing.Color.White;
            this.lbl_dis4.Location = new System.Drawing.Point(459, 167);
            this.lbl_dis4.Name = "lbl_dis4";
            this.lbl_dis4.Size = new System.Drawing.Size(38, 13);
            this.lbl_dis4.TabIndex = 3;
            this.lbl_dis4.Text = "label6";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // displayTv1
            // 
            this.displayTv1.BackColor = System.Drawing.Color.Black;
            this.displayTv1.Location = new System.Drawing.Point(25, 21);
            this.displayTv1.Name = "displayTv1";
            this.displayTv1.Size = new System.Drawing.Size(480, 275);
            this.displayTv1.TabIndex = 0;
            // 
            // Frm_DisplaySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 342);
            this.Controls.Add(this.lbl_dis3);
            this.Controls.Add(this.lbl_dis4);
            this.Controls.Add(this.lbl_dis2);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.radGroupBox2);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.displayTv1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_DisplaySetting";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.Text = "大屏设置";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.Frm_DisplaySetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radListView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private UserControls.DisplayTv displayTv1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_confirm1;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox2;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_confirm2;
        private System.Windows.Forms.TextBox textBox1;
        private Telerik.WinControls.UI.RadListView radListView1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.Label lbl_dis2;
        public System.Windows.Forms.Label lbl_dis3;
        public System.Windows.Forms.Label lbl_dis4;
    }
}
