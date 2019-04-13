namespace DrillingSymtemCSCV2
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.txtUsername = new Telerik.WinControls.UI.RadTextBox();
            this.txtPassword = new Telerik.WinControls.UI.RadTextBox();
            this.rlbl_username = new Telerik.WinControls.UI.RadLabel();
            this.rlbl_password = new Telerik.WinControls.UI.RadLabel();
            this.btn_login = new Telerik.WinControls.UI.RadButton();
            this.btn_close = new Telerik.WinControls.UI.RadButton();
            this.panelLogin = new Telerik.WinControls.UI.RadPanel();
            this.rbtn_keyborad = new Telerik.WinControls.UI.RadButton();
            this.txt_loginfailed = new Telerik.WinControls.UI.RadLabel();
            this.labSystemName = new System.Windows.Forms.Label();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.rlbl_name = new Telerik.WinControls.UI.RadLabel();
            this.picCoverImg = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.picHHLogo = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.visualStudio2012DarkTheme1 = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_password)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogin)).BeginInit();
            this.panelLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_keyborad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_loginfailed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCoverImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHHLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.AutoSize = false;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(762, 296);
            this.txtUsername.Multiline = true;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(162, 30);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.ThemeName = "VisualStudio2012Dark";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.AutoSize = false;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(762, 335);
            this.txtPassword.Multiline = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(162, 30);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.ThemeName = "VisualStudio2012Dark";
            // 
            // rlbl_username
            // 
            this.rlbl_username.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rlbl_username.AutoSize = false;
            this.rlbl_username.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_username.ForeColor = System.Drawing.Color.White;
            this.rlbl_username.Location = new System.Drawing.Point(592, 296);
            this.rlbl_username.Name = "rlbl_username";
            this.rlbl_username.Size = new System.Drawing.Size(164, 30);
            this.rlbl_username.TabIndex = 2;
            this.rlbl_username.Text = "UserName:";
            this.rlbl_username.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rlbl_password
            // 
            this.rlbl_password.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rlbl_password.AutoSize = false;
            this.rlbl_password.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.rlbl_password.ForeColor = System.Drawing.Color.White;
            this.rlbl_password.Location = new System.Drawing.Point(592, 335);
            this.rlbl_password.Name = "rlbl_password";
            this.rlbl_password.Size = new System.Drawing.Size(164, 30);
            this.rlbl_password.TabIndex = 3;
            this.rlbl_password.Text = "Password:";
            this.rlbl_password.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // btn_login
            // 
            this.btn_login.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_login.Location = new System.Drawing.Point(666, 408);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(110, 30);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = "Login";
            this.btn_login.ThemeName = "VisualStudio2012Dark";
            this.btn_login.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.Location = new System.Drawing.Point(797, 408);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(110, 30);
            this.btn_close.TabIndex = 5;
            this.btn_close.Text = "Close";
            this.btn_close.ThemeName = "VisualStudio2012Dark";
            this.btn_close.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelLogin
            // 
            this.panelLogin.Controls.Add(this.rbtn_keyborad);
            this.panelLogin.Location = new System.Drawing.Point(589, 257);
            this.panelLogin.Name = "panelLogin";
            this.panelLogin.Size = new System.Drawing.Size(379, 198);
            this.panelLogin.TabIndex = 6;
            this.panelLogin.Paint += new System.Windows.Forms.PaintEventHandler(this.panelLogin_Paint);
            // 
            // rbtn_keyborad
            // 
            this.rbtn_keyborad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rbtn_keyborad.BackColor = System.Drawing.Color.Transparent;
            this.rbtn_keyborad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.rbtn_keyborad.ForeColor = System.Drawing.Color.Transparent;
            this.rbtn_keyborad.Image = ((System.Drawing.Image)(resources.GetObject("rbtn_keyborad.Image")));
            this.rbtn_keyborad.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtn_keyborad.ImageScalingSize = new System.Drawing.Size(5, 5);
            this.rbtn_keyborad.Location = new System.Drawing.Point(341, 41);
            this.rbtn_keyborad.Name = "rbtn_keyborad";
            this.rbtn_keyborad.Size = new System.Drawing.Size(32, 21);
            this.rbtn_keyborad.TabIndex = 5;
            this.rbtn_keyborad.ThemeName = "VisualStudio2012Dark";
            this.rbtn_keyborad.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // txt_loginfailed
            // 
            this.txt_loginfailed.AutoSize = false;
            this.txt_loginfailed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txt_loginfailed.ForeColor = System.Drawing.Color.Red;
            this.txt_loginfailed.Location = new System.Drawing.Point(629, 371);
            this.txt_loginfailed.Name = "txt_loginfailed";
            this.txt_loginfailed.Size = new System.Drawing.Size(333, 25);
            this.txt_loginfailed.TabIndex = 0;
            this.txt_loginfailed.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labSystemName
            // 
            this.labSystemName.Font = new System.Drawing.Font("Microsoft YaHei", 18F);
            this.labSystemName.ForeColor = System.Drawing.Color.White;
            this.labSystemName.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.labSystemName.Location = new System.Drawing.Point(62, 43);
            this.labSystemName.Name = "labSystemName";
            this.labSystemName.Size = new System.Drawing.Size(397, 44);
            this.labSystemName.TabIndex = 9;
            this.labSystemName.Text = "iDrill 2.0";
            this.labSystemName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radLabel4
            // 
            this.radLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radLabel4.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radLabel4.ForeColor = System.Drawing.Color.DarkOrange;
            this.radLabel4.Location = new System.Drawing.Point(722, 159);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(148, 25);
            this.radLabel4.TabIndex = 3;
            this.radLabel4.Text = "HONGHUA GROUP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Comic Sans MS", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(137, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(226, 19);
            this.label7.TabIndex = 46;
            this.label7.Text = "PoweredBy HongHua Co.Ltd. 2019";
            // 
            // rlbl_name
            // 
            this.rlbl_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rlbl_name.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_name.ForeColor = System.Drawing.Color.DarkOrange;
            this.rlbl_name.Location = new System.Drawing.Point(629, 226);
            this.rlbl_name.Name = "rlbl_name";
            this.rlbl_name.Size = new System.Drawing.Size(48, 25);
            this.rlbl_name.TabIndex = 4;
            this.rlbl_name.Text = "Login";
            // 
            // picCoverImg
            // 
            this.picCoverImg.Image = ((System.Drawing.Image)(resources.GetObject("picCoverImg.Image")));
            this.picCoverImg.Location = new System.Drawing.Point(12, 142);
            this.picCoverImg.Name = "picCoverImg";
            this.picCoverImg.Size = new System.Drawing.Size(528, 428);
            this.picCoverImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCoverImg.TabIndex = 48;
            this.picCoverImg.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::DrillingSymtemCSCV2.Properties.Resources.user_48;
            this.pictureBox3.Location = new System.Drawing.Point(589, 221);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(33, 33);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 47;
            this.pictureBox3.TabStop = false;
            // 
            // picHHLogo
            // 
            this.picHHLogo.Image = global::DrillingSymtemCSCV2.Properties.Resources.hhlogo;
            this.picHHLogo.Location = new System.Drawing.Point(705, 60);
            this.picHHLogo.Name = "picHHLogo";
            this.picHHLogo.Size = new System.Drawing.Size(178, 91);
            this.picHHLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHHLogo.TabIndex = 43;
            this.picHHLogo.TabStop = false;
            // 
            // label3
            // 
            this.label3.Image = global::DrillingSymtemCSCV2.Properties.Resources.line2;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.label3.Location = new System.Drawing.Point(59, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(401, 18);
            this.label3.TabIndex = 10;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork_1);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 569);
            this.Controls.Add(this.txt_loginfailed);
            this.Controls.Add(this.rlbl_password);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.picCoverImg);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.rlbl_name);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.picHHLogo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labSystemName);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.rlbl_username);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.panelLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iDrill 2.0";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_password)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_close)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogin)).EndInit();
            this.panelLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rbtn_keyborad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_loginfailed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCoverImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHHLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox txtUsername;
        private Telerik.WinControls.UI.RadTextBox txtPassword;
        private Telerik.WinControls.UI.RadLabel rlbl_username;
        private Telerik.WinControls.UI.RadLabel rlbl_password;
        private Telerik.WinControls.UI.RadButton btn_login;
        private Telerik.WinControls.UI.RadButton btn_close;
        private Telerik.WinControls.UI.RadPanel panelLogin;
        private System.Windows.Forms.Label labSystemName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picHHLogo;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private System.Windows.Forms.Label label7;
        private Telerik.WinControls.UI.RadLabel rlbl_name;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox picCoverImg;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme1;
        private Telerik.WinControls.UI.RadLabel txt_loginfailed;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Telerik.WinControls.UI.RadButton rbtn_keyborad;
    }
}

