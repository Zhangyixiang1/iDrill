using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class DisplayTv : UserControl
    {
        public DisplayTv()
        {
            InitializeComponent();
        }

        private void DisplayTv_Load(object sender, EventArgs e)
        {
           
        }

        private void DisplayTv_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.White, 2);
            Point p_zero = new Point(0, 0);
            Size size = this.Size;

            g.DrawRectangle(pen, new Rectangle(p_zero, size));

            Point p1 = new Point(p_zero.X + size.Width / 2, p_zero.Y);
            Point p2 = new Point(p1.X, p1.Y + size.Height);
            Point p3 = new Point(p_zero.X, p_zero.Y + size.Height / 2);
            Point p4 = new Point(p3.X + size.Width, p3.Y);
            g.DrawLine(pen, p1, p2); g.DrawLine(pen, p3, p4);
        }
    }
}
