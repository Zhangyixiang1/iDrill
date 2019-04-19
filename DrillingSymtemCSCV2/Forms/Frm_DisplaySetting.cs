using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Configuration;
using System.Collections;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class Frm_DisplaySetting : Telerik.WinControls.UI.RadForm
    {
        DrillOSEntities db;
        Configuration config;
        public Frm_DisplaySetting()
        {
            InitializeComponent();
            db = new DrillOSEntities();
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void Frm_DisplaySetting_Load(object sender, EventArgs e)
        {
            //绑定输入源

            var welllist = db.Drill.Where(o => o.isActive == true).ToList();
            comboBox2.Items.Add("循环");
            comboBox3.Items.Add("循环");
            comboBox4.Items.Add("循环");
            foreach (var item in welllist)
            {
                comboBox2.Items.Add(item.Contractor);
                comboBox3.Items.Add(item.Contractor);
                comboBox4.Items.Add(item.Contractor);
                radListView1.Items.Add(item.Contractor);
            }
            textBox1.Text = config.AppSettings.Settings["cycletime"].Value;


            //读取配置文件
            string dis2 = config.AppSettings.Settings["display2"].Value;
            if (!string.IsNullOrEmpty(dis2)) comboBox2.Text = dis2;

            string dis3 = config.AppSettings.Settings["display3"].Value;
            if (!string.IsNullOrEmpty(dis3)) comboBox3.Text = dis3;

            string dis4 = config.AppSettings.Settings["display4"].Value;
            if (!string.IsNullOrEmpty(dis4)) comboBox4.Text = dis4;

            //循环列表变色
            foreach (var item in radListView1.Items)
            {
                item.ForeColor = Color.Lime;
                if (item.Text == dis2 || item.Text == dis3 || item.Text == dis4)
                    item.ForeColor = Color.White;

            }
        }



        private void btn_confirm1_Click(object sender, EventArgs e)
        {
            //判断，输入源不能相同
            Hashtable temp = new Hashtable();
            try
            {
                if (comboBox2.Text != "循环") temp.Add(comboBox2.Text, 2);
                if (comboBox3.Text != "循环") temp.Add(comboBox3.Text, 2);
                if (comboBox4.Text != "循环") temp.Add(comboBox4.Text, 3);
            }
            catch
            {
                MessageBox.Show("输入源不能相同！");
                return;
            }


            Comm.SaveConfig(comboBox2.Items[comboBox2.SelectedIndex].ToString(), "display2");
            Comm.SaveConfig(comboBox3.Items[comboBox3.SelectedIndex].ToString(), "display3");
            Comm.SaveConfig(comboBox4.Items[comboBox4.SelectedIndex].ToString(), "display4");
            //循环列表变色
            foreach (var item in radListView1.Items)
            {
                if (item.Text == comboBox2.Text || item.Text == comboBox3.Text || item.Text == comboBox4.Text)
                    item.ForeColor = Color.White;
                else item.ForeColor = Color.Lime;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("警告！点击确定后程序将程序已生效设置！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                Application.Restart();
            }
        }

        private void btn_confirm2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
                Comm.SaveConfig(textBox1.Text, "cycletime");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbl_dis2.Text = "无显示"; lbl_dis3.Text = "无显示"; lbl_dis4.Text = "无显示";
            //测试用，显示displaylist窗体的位置
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    if (frm.Location == new Point(1920 * 2, 0)) lbl_dis2.Text = frm.Tag.ToString();
                    else if (frm.Location == new Point(1920 , 1080)) lbl_dis3.Text = frm.Tag.ToString();
                    else if (frm.Location == new Point(1920 * 2, 1080)) lbl_dis4.Text = frm.Tag.ToString();
                }
            }
        }
    }
}
