using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class Chb : UserControl
    {
        private bool _checked;
        public Chb()
        {
            InitializeComponent();
        }
        [CategoryAttribute("Checked"), DescriptionAttribute("报警是否启用")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (_checked) label1.Visible = true;
                else label1.Visible = false;

            }


        }

        private void Chb_Paint(object sender, PaintEventArgs e)
        {
            //    ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void Chb_Click(object sender, EventArgs e)
        {

            if (Checked)
            {
                this.Checked = false;
                //  label1.Visible = false;
            }
            else
            {
                this.Checked = true;
                //  label1.Visible = true;
            }
        }

        private void label1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label1.ClientRectangle, Color.White, ButtonBorderStyle.Solid);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (Checked)
            {
                this.Checked = false;

            }
            else
            {
                this.Checked = true;

            }
        }
    }
}
