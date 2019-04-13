using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class SelectMessage : RadForm
    {
        public string SendText;
        public long m_lTime = 0L;
        public List<string> MemoList = new List<string>();
        private List<Button> btnList = new List<Button>();
        public SelectMessage()
        {
            InitializeComponent();
        }

        private void SelectMessage_Load(object sender, EventArgs e)
        {
            try
            {
                setControlLanguage();
                setButtons();
                InitTime();
            }
            catch 
            { 
            }
        }

        private void InitTime()
        {
            this.radCalendar_start.SelectedDate = DateTime.Now;
            this.radTimePicker1.Value = Convert.ToDateTime(this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            rtxt_time.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
        }

        #region 读取xml文件设置语言
        private void setControlLanguage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == this.Name)//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            if (this.Name == xe.GetAttribute("key"))
                            {
                                this.Text = xe.GetAttribute("value");
                                continue;
                            }
                            if (xe.GetAttribute("key") == "AcitvityStatusPage")
                            {
                                this.AcitvityStatusPage.Text = xe.GetAttribute("value");
                                continue;
                            }
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList xn_m = xe.ChildNodes;
                                foreach (XmlNode node_m in xn_m)
                                {
                                    XmlElement mess=(XmlElement)node_m;
                                    MemoList.Add(mess.GetAttribute("value"));
                                }
                                continue;
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "pnl_Memotext")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            foreach (Control ctl in c.Controls)
                                            {
                                                if (ctl.Name == xe2.GetAttribute("key"))
                                                {
                                                    ctl.Text = xe2.GetAttribute("value");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    c.Text = xe.GetAttribute("value");//设置控件的Text
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        private void setButtons()
        {
            try
            {
                Button[] btns = new Button[MemoList.Count];  //声明对象
                for (int i = 0; i < MemoList.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 164 * (i % 5), 6 + 56 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(158, 50);
                    btns[i].Text = MemoList[i];
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    this.AcitvityStatusPage.Controls.Add(btns[i]);
                    btnList.Add(btns[i]);
                }
            }
            catch 
            { 
            }
        }

        //按钮被点击
        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                Color c = b.BackColor;//保存点击按钮的颜色

                //取消所有的按钮选中状态
                for (int i = 0; i < btnList.Count; i++)
                {
                    if (btnList[i].BackColor == Color.Red)
                    {
                        btnList[i].BackColor = Color.Black;
                        btnList[i].ForeColor = Color.White;
                    }
                }
                if (c == Color.Black)
                {
                    b.BackColor = Color.Red;
                    rtxt_message.Text = b.Text;
                }
                else
                {
                    b.BackColor = Color.Black;//如果重复点击了该按钮，则取消点击事件
                    rtxt_message.Text = "";
                }
            }
            catch 
            { 
            }
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            this.SendText = rtxt_message.Text;
            m_lTime = ConvertDateTimeInt(DateTime.Parse(this.rtxt_time.Text));
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtn_keyboard_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe");
                rtxt_message.Focus();
            }
            catch 
            { 
            }
        }

        private void StartTime_Click(object sender, EventArgs e)
        {
            if (this.pl_CalendarAndTime.Visible)
            {
                this.pl_CalendarAndTime.Visible = false;
            }
            else
            {
                this.pl_CalendarAndTime.Visible = true;
            }
        }

        private void TimePick_OK_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_start.SelectedDate.Year != 1900)
            {
                rtxt_time.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            }

            pl_CalendarAndTime.Visible = false;
        }

        private void TimePick_No_Click(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
        }

        private long ConvertDateTimeInt(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt64((time - startTime).TotalSeconds);
            return intResult;
        }

        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
            {
                return;
            }

            base.WndProc(ref m);
        }
    }
}
