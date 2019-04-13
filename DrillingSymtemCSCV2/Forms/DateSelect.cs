using System;
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
    public partial class DateSelect : RadForm
    {
        public List<ReportData> ReportData;//从报表界面传过来用于判断当前需要新建的报表是否已经存在
        public DateTime d_select;//创建新的报表需要返回的日期
        public List<string> message_list = new List<string>();//存放需要提示的基本数据
        public DateSelect()
        {
            InitializeComponent();
        }

        private void DateSelect_Load(object sender, EventArgs e)
        {
            //如果为中文
            if (AppDrill.language == "CN")
            {
                this.rc_date.Culture = new System.Globalization.CultureInfo("zh-CN");
            }
            this.rc_date.SelectedDate = DateTime.Now.Date;//设置默认选中今日
            this.rc_date.FocusedDate = DateTime.Now.Date;//设置默认选中今日
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void rbtn_OK_Click(object sender, EventArgs e)
        {
            d_select=this.rc_date.SelectedDate;//设置选中日期，返回
            //判断是否已经存在
            ReportData rd = ReportData.Where(o => o.Date == d_select.ToString("yyyy-MM-dd")).FirstOrDefault();
            if (rd != null)
            {
                //已经存在的情况
                MessageBox.Show("当前日期的报表已经存在，请重新选择");
                return;
            }
            else
            {
                this.Dispose();
            }
        }
    }
}
