namespace DrillingSymtemCSCV2.Forms
{
    partial class Map_Baidu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Map_Baidu));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_OK = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.his_btn = new System.Windows.Forms.Button();
            this.real_btn = new System.Windows.Forms.Button();
            this.cacel_btn = new System.Windows.Forms.Button();
            this.btn_Wellset = new System.Windows.Forms.Button();
            this.btn_displayset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(416, 12);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(1475, 1016);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // listBox1
            // 
            this.listBox1.BackColor = System.Drawing.Color.Black;
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox1.Font = new System.Drawing.Font("SimSun", 12F);
            this.listBox1.ForeColor = System.Drawing.Color.White;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(12, 44);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(400, 978);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // btn_OK
            // 
            this.btn_OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_OK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btn_OK.ForeColor = System.Drawing.Color.White;
            this.btn_OK.Location = new System.Drawing.Point(53, 964);
            this.btn_OK.Margin = new System.Windows.Forms.Padding(0);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(120, 40);
            this.btn_OK.TabIndex = 18;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = false;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DrillingSymtemCSCV2.Properties.Resources.load;
            this.pictureBox1.Location = new System.Drawing.Point(774, 269);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(185, 158);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // his_btn
            // 
            this.his_btn.BackColor = System.Drawing.Color.Gray;
            this.his_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.his_btn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.his_btn.ForeColor = System.Drawing.Color.White;
            this.his_btn.Location = new System.Drawing.Point(212, 11);
            this.his_btn.Margin = new System.Windows.Forms.Padding(0);
            this.his_btn.Name = "his_btn";
            this.his_btn.Size = new System.Drawing.Size(200, 30);
            this.his_btn.TabIndex = 20;
            this.his_btn.Text = "历史钻井";
            this.his_btn.UseVisualStyleBackColor = false;
            this.his_btn.Click += new System.EventHandler(this.his_btn_Click);
            // 
            // real_btn
            // 
            this.real_btn.BackColor = System.Drawing.Color.Gray;
            this.real_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.real_btn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.real_btn.ForeColor = System.Drawing.Color.White;
            this.real_btn.Location = new System.Drawing.Point(12, 11);
            this.real_btn.Margin = new System.Windows.Forms.Padding(0);
            this.real_btn.Name = "real_btn";
            this.real_btn.Size = new System.Drawing.Size(200, 30);
            this.real_btn.TabIndex = 21;
            this.real_btn.Text = "实时钻井";
            this.real_btn.UseVisualStyleBackColor = false;
            this.real_btn.Click += new System.EventHandler(this.real_btn_Click);
            // 
            // cacel_btn
            // 
            this.cacel_btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.cacel_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cacel_btn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.cacel_btn.ForeColor = System.Drawing.Color.White;
            this.cacel_btn.Location = new System.Drawing.Point(249, 964);
            this.cacel_btn.Margin = new System.Windows.Forms.Padding(0);
            this.cacel_btn.Name = "cacel_btn";
            this.cacel_btn.Size = new System.Drawing.Size(120, 40);
            this.cacel_btn.TabIndex = 22;
            this.cacel_btn.TabStop = false;
            this.cacel_btn.Text = "退出";
            this.cacel_btn.UseVisualStyleBackColor = false;
            this.cacel_btn.Click += new System.EventHandler(this.cacel_btn_Click);
            // 
            // btn_Wellset
            // 
            this.btn_Wellset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_Wellset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Wellset.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btn_Wellset.ForeColor = System.Drawing.Color.White;
            this.btn_Wellset.Location = new System.Drawing.Point(53, 901);
            this.btn_Wellset.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Wellset.Name = "btn_Wellset";
            this.btn_Wellset.Size = new System.Drawing.Size(120, 40);
            this.btn_Wellset.TabIndex = 18;
            this.btn_Wellset.Text = "井队设置";
            this.btn_Wellset.UseVisualStyleBackColor = false;
            this.btn_Wellset.Click += new System.EventHandler(this.btn_Wellset_Click);
            // 
            // btn_displayset
            // 
            this.btn_displayset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btn_displayset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_displayset.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btn_displayset.ForeColor = System.Drawing.Color.White;
            this.btn_displayset.Location = new System.Drawing.Point(249, 901);
            this.btn_displayset.Margin = new System.Windows.Forms.Padding(0);
            this.btn_displayset.Name = "btn_displayset";
            this.btn_displayset.Size = new System.Drawing.Size(120, 40);
            this.btn_displayset.TabIndex = 22;
            this.btn_displayset.TabStop = false;
            this.btn_displayset.Text = "大屏设置";
            this.btn_displayset.UseVisualStyleBackColor = false;
            this.btn_displayset.Click += new System.EventHandler(this.btn_displayset_Click);
            // 
            // Map_Baidu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(1904, 1042);
            this.Controls.Add(this.btn_displayset);
            this.Controls.Add(this.cacel_btn);
            this.Controls.Add(this.real_btn);
            this.Controls.Add(this.his_btn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_Wellset);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.webBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Map_Baidu";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "钻井列表";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Map_Baidu_FormClosed);
            this.Load += new System.EventHandler(this.Map_Baidu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button his_btn;
        private System.Windows.Forms.Button real_btn;
        private System.Windows.Forms.Button cacel_btn;
        private System.Windows.Forms.Button btn_Wellset;
        private System.Windows.Forms.Button btn_displayset;
    }
}