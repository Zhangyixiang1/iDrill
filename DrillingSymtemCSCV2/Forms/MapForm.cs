using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using DrillingSymtemCSCV2.UserControls;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class MapForm : Telerik.WinControls.UI.RadForm
    {
        private DrillOSEntities _db;
        List<Drill> drillinfo;
        public MapForm()
        {
            InitializeComponent();
            _db = new DrillOSEntities();
            drillinfo = new List<Drill>();
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            drillinfo = _db.Drill.ToList();
            foreach (Drill item in drillinfo)
            {
                Point pt = unittrans(item.location);


                mark pic = new mark();
                //  pic.BackgroundImage = Resources.标记;
                //   pic.BackgroundImageLayout = ImageLayout.Stretch;

                pic.Location = pt;
                pic.label1.MouseEnter += showmsg;
                pic.label1.MouseLeave += hidemsg;
                pic.label1.Click += changeselect;
                pic.label1.Tag = item;
                pic.label1.Text = item.ID.ToString();
                this.Controls.Add(pic);
                listBox1.Items.Add(item.ID + "." + item.DrillNo + "  "+item.Lease+","+item.Country+"  " + item.Contractor);
            }

        }
        private void showmsg(object sender, EventArgs e)
        {

            this.toolTip1.ToolTipTitle = "井队信息";
            this.toolTip1.IsBalloon = false;
            this.toolTip1.UseFading = true;
            Label pic = sender as Label;
            Drill item = pic.Tag as Drill;
            this.toolTip1.Show(item.DrillNo + "\n" + item.description, pic);

        }
        private void hidemsg(object sender, EventArgs e)
        {
            Label pic = sender as Label;
            this.toolTip1.Hide(pic);
        }
        private void changeselect(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            Label pic = sender as Label;
            Drill item = pic.Tag as Drill;
            int index = Convert.ToInt16(item.ID);
            listBox1.SelectedIndex = index - 1;
        }
        private Point unittrans(string str)
        {
            string[] jingweidu = str.Split(',');
            string jingdu = jingweidu[1];
            string[] data = jingdu.Substring(1).Split('-');
            double x = Convert.ToDouble(data[0]) + Convert.ToDouble(data[1]) / 60;
            string weidu = jingweidu[0];
            string[] data1 = weidu.Substring(1).Split('-');
            double y = Convert.ToDouble(data1[0]) + Convert.ToDouble(data1[1]) / 60;

            if (weidu.Contains("N"))
            {
                y = 800 - y * 800 / 90;
            }
            else { y = 800 + y * 800 / 90; }

            if (jingdu.Contains("E"))
            {
                x = 855 + x * 855 / 180;
            }
            else { x = 855 - x * 855 / 180; }
            return new Point((int)x, (int)y);
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择一个数据源！");
                return;
            }
            AppDrill.DrillID = listBox1.SelectedIndex + 1;
            foreach (Form frm in Application.OpenForms)
            {
                if (Convert.ToInt16(frm.Tag) == AppDrill.DrillID)
                {
                    frm.BringToFront();
                    return;
                }
            }

            DrillForm drill = new DrillForm();
            drill.Size = new System.Drawing.Size(1920, 1080);
            drill.Location = new Point(0, 0);
            drill.Tag = AppDrill.DrillID;
            this.Hide();
            drill.Show();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
    }
}
