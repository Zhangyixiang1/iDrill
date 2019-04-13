using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DrillingSymtemCSCV2.Model;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class ReportSelect : RadForm
    {
        public List<ReportData> ReportData;//报表界面传过来的
        public string d_select;//选中日期的报表
        public string Type;//命令:load是加载，delete是删除
        public List<string> message_list = new List<string>();//存放需要提示的基本数据
        public ReportSelect()
        {
            InitializeComponent();
        }

        private void ReportSelect_Load(object sender, EventArgs e)
        {
            SelectList();
        }
        /// <summary>
        /// 加载已经存在的报表
        /// </summary>
        private void SelectList()
        {
            try
            {
                var selectdata = new ArrayList();
                foreach (var item in ReportData.OrderByDescending(o => o.Date).Distinct())
                {
                    selectdata.Add(new RadListDataItem(item.Date, item.ID.ToString()));
                }
                rdp_report.DataSource = selectdata;//井号数据
            }
            catch { }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtn_OK_Click(object sender, EventArgs e)
        {
            d_select = this.rdp_report.Text;//当前选中的报表
            if (Type == "load")
            {
                this.Close();
            }
            else if (Type == "delete")
            {
                if (MessageBox.Show("你确定要删除当前选中日期的报表吗？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    d_select = null;
                }
            }
        }
    }
}
