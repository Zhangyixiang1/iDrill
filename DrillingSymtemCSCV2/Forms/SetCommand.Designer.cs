namespace DrillingSymtemCSCV2.Forms
{
    partial class SetCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetCommand));
            this.tab_command = new System.Windows.Forms.TabControl();
            this.tb_command1 = new System.Windows.Forms.TabPage();
            this.tb_command2 = new System.Windows.Forms.TabPage();
            this.tb_command3 = new System.Windows.Forms.TabPage();
            this.rlbl_message = new Telerik.WinControls.UI.RadLabel();
            this.rtxt_value = new Telerik.WinControls.UI.RadTextBox();
            this.btn_Cancel = new Telerik.WinControls.UI.RadButton();
            this.btn_Set = new Telerik.WinControls.UI.RadButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.lbl_message = new System.Windows.Forms.Label();
            this.rlbl_unit = new Telerik.WinControls.UI.RadLabel();
            this.backgroundWorker3 = new System.ComponentModel.BackgroundWorker();
            this.tab_command.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_message)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_value)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Set)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_unit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tab_command
            // 
            this.tab_command.Controls.Add(this.tb_command1);
            this.tab_command.Controls.Add(this.tb_command2);
            this.tab_command.Controls.Add(this.tb_command3);
            this.tab_command.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.tab_command.Location = new System.Drawing.Point(13, 24);
            this.tab_command.Name = "tab_command";
            this.tab_command.SelectedIndex = 0;
            this.tab_command.Size = new System.Drawing.Size(813, 339);
            this.tab_command.TabIndex = 0;
            // 
            // tb_command1
            // 
            this.tb_command1.BackColor = System.Drawing.Color.Black;
            this.tb_command1.Location = new System.Drawing.Point(4, 26);
            this.tb_command1.Name = "tb_command1";
            this.tb_command1.Padding = new System.Windows.Forms.Padding(3);
            this.tb_command1.Size = new System.Drawing.Size(805, 309);
            this.tb_command1.TabIndex = 0;
            this.tb_command1.Text = "Command1";
            // 
            // tb_command2
            // 
            this.tb_command2.BackColor = System.Drawing.Color.Black;
            this.tb_command2.Location = new System.Drawing.Point(4, 26);
            this.tb_command2.Name = "tb_command2";
            this.tb_command2.Padding = new System.Windows.Forms.Padding(3);
            this.tb_command2.Size = new System.Drawing.Size(805, 309);
            this.tb_command2.TabIndex = 1;
            this.tb_command2.Text = "Command2";
            // 
            // tb_command3
            // 
            this.tb_command3.BackColor = System.Drawing.Color.Black;
            this.tb_command3.Location = new System.Drawing.Point(4, 26);
            this.tb_command3.Name = "tb_command3";
            this.tb_command3.Size = new System.Drawing.Size(805, 309);
            this.tb_command3.TabIndex = 2;
            this.tb_command3.Text = "Command3";
            // 
            // rlbl_message
            // 
            this.rlbl_message.AutoSize = false;
            this.rlbl_message.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_message.ForeColor = System.Drawing.Color.White;
            this.rlbl_message.Location = new System.Drawing.Point(45, 393);
            this.rlbl_message.Name = "rlbl_message";
            this.rlbl_message.Size = new System.Drawing.Size(260, 30);
            this.rlbl_message.TabIndex = 14;
            this.rlbl_message.Text = "Value：";
            this.rlbl_message.TextAlignment = System.Drawing.ContentAlignment.TopRight;
            // 
            // rtxt_value
            // 
            this.rtxt_value.Font = new System.Drawing.Font("Segoe UI", 18F);
            this.rtxt_value.Location = new System.Drawing.Point(310, 389);
            this.rtxt_value.MaxLength = 50;
            this.rtxt_value.Name = "rtxt_value";
            this.rtxt_value.Size = new System.Drawing.Size(216, 38);
            this.rtxt_value.TabIndex = 13;
            this.rtxt_value.ThemeName = "VisualStudio2012Dark";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Cancel.Location = new System.Drawing.Point(461, 456);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(150, 30);
            this.btn_Cancel.TabIndex = 16;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.ThemeName = "VisualStudio2012Dark";
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Set
            // 
            this.btn_Set.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btn_Set.Location = new System.Drawing.Point(214, 456);
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.Size = new System.Drawing.Size(150, 30);
            this.btn_Set.TabIndex = 15;
            this.btn_Set.Text = "Set";
            this.btn_Set.ThemeName = "VisualStudio2012Dark";
            this.btn_Set.Click += new System.EventHandler(this.btn_Select_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Transparent;
            this.lbl_message.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lbl_message.ForeColor = System.Drawing.Color.Lime;
            this.lbl_message.Location = new System.Drawing.Point(625, 389);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(174, 38);
            this.lbl_message.TabIndex = 59;
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rlbl_unit
            // 
            this.rlbl_unit.AutoSize = false;
            this.rlbl_unit.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rlbl_unit.ForeColor = System.Drawing.Color.White;
            this.rlbl_unit.Location = new System.Drawing.Point(532, 393);
            this.rlbl_unit.Name = "rlbl_unit";
            this.rlbl_unit.Size = new System.Drawing.Size(79, 30);
            this.rlbl_unit.TabIndex = 60;
            this.rlbl_unit.Text = "M";
            this.rlbl_unit.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // backgroundWorker3
            // 
            this.backgroundWorker3.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker3_DoWork);
            this.backgroundWorker3.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker3_RunWorkerCompleted);
            // 
            // SetCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 494);
            this.Controls.Add(this.rlbl_unit);
            this.Controls.Add(this.lbl_message);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Set);
            this.Controls.Add(this.rlbl_message);
            this.Controls.Add(this.rtxt_value);
            this.Controls.Add(this.tab_command);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetCommand";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rig View";
            this.ThemeName = "VisualStudio2012Dark";
            this.Load += new System.EventHandler(this.SetCommand_Load);
            this.tab_command.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_message)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxt_value)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Set)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlbl_unit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tab_command;
        private System.Windows.Forms.TabPage tb_command1;
        private System.Windows.Forms.TabPage tb_command2;
        private Telerik.WinControls.UI.RadLabel rlbl_message;
        private Telerik.WinControls.UI.RadTextBox rtxt_value;
        private Telerik.WinControls.UI.RadButton btn_Cancel;
        private Telerik.WinControls.UI.RadButton btn_Set;
        private System.Windows.Forms.TabPage tb_command3;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.Label lbl_message;
        private Telerik.WinControls.UI.RadLabel rlbl_unit;
        private System.ComponentModel.BackgroundWorker backgroundWorker3;
    }
}