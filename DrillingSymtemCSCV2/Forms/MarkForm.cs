using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Xml;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class MarkForm : RadForm
    {
        private int m_iStep = 0;
        private int m_iTimeIndex = 1;
        private bool m_bIsReal = true;
        private bool m_bIsCancle = false;
        private bool m_bIsBtnClick = false;
        private bool m_bIsHisForm = false;
        private long m_lStartTime = 0L;
        private Dictionary<string, int> m_dicTime = new Dictionary<string, int>();

        public MarkForm(int iStep, int iIndex = 1, bool bIsReal = true, long lStartTime = 0L)
        {
            m_iStep = iStep;
            m_iTimeIndex = iIndex;
            m_bIsReal = bIsReal;
            m_lStartTime = lStartTime;
            InitializeComponent();
        }

        private void setTimeIndex(int iIndex)
        {
            m_iTimeIndex = iIndex;
        }

        public int getTimeIndex()
        {
            return m_iTimeIndex;
        }

        public bool getReal()
        {
            return m_bIsReal;
        }

        public long getTime()
        {
            return m_lStartTime;
        }

        public bool getCancle()
        {
            return m_bIsCancle;
        }

        private long ConvertDateTimeInt(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt64((time - startTime).TotalSeconds);
            return intResult;
        }

        private void InitDictionary()
        {
            m_dicTime.Add("1 Min.", 1);
            m_dicTime.Add("2 Min.", 2);
            m_dicTime.Add("3 Min.", 3);
            m_dicTime.Add("5 Min.", 4);
            m_dicTime.Add("10 Min.", 5);
            m_dicTime.Add("20 Min.", 6);
            m_dicTime.Add("30 Min.", 7);
            m_dicTime.Add("1 Hr.", 8);
            m_dicTime.Add("2 Hr.", 9);
            m_dicTime.Add("4 Hr.", 10);
        }

        private void setTimeText(DateTime dt)
        {
            this.radCalendar_end.SelectedDate = dt;
            this.radCalendar_end.FocusedDate = dt;
            this.radTimePicker2.Value = Convert.ToDateTime(this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            this.tbx_time.Text = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");
        }

        private void InitStartTime()
        {
            try
            {
                if (0 == m_lStartTime)
                {
                    setTimeText(DateTime.Now);
                    m_lStartTime = ConvertDateTimeInt(DateTime.Parse(tbx_time.Text));
                }
                else
                {
                    DateTime dt = Comm.ConvertIntDateTime((double)m_lStartTime * 1000);
                    setTimeText(dt);
                }
            }
            catch
            {
            }
        }

        private void setListBox1Index()
        {
            switch (m_iStep)
            {
                case 1:
                    listBox1.SelectedIndex = 0;
                    break;
                case 5:
                    listBox1.SelectedIndex = 1;
                    break;
                case 10:
                    listBox1.SelectedIndex = 2;
                    break;
                case 20:
                    listBox1.SelectedIndex = 3;
                    break;
                case 25:
                    listBox1.SelectedIndex = 4;
                    break;
                case 50:
                    listBox1.SelectedIndex = 5;
                    break;
                default:
                    listBox1.SelectedIndex = 0;
                    break;

            }
        }

        private void setListBox2Index()
        {
            switch (m_iTimeIndex)
            {
                case 1:
                    listBox2.SelectedIndex = 0;
                    break;
                case 2:
                    listBox2.SelectedIndex = 1;
                    break;
                case 3:
                    listBox2.SelectedIndex = 2;
                    break;
                case 4:
                    listBox2.SelectedIndex = 3;
                    break;
                case 5:
                    listBox2.SelectedIndex = 4;
                    break;
                case 6:
                    listBox2.SelectedIndex = 5;
                    break;
                case 7:
                    listBox2.SelectedIndex = 6;
                    break;
                case 8:
                    listBox2.SelectedIndex = 7;
                    break;
                case 9:
                    listBox2.SelectedIndex = 8;
                    break;
                case 10:
                    listBox2.SelectedIndex = 9;
                    break;
                default:
                    listBox2.SelectedIndex = 0;
                    break;

            }
        }

        private void MarkForm_Load(object sender, EventArgs e)
        {
            setControlLanguage();
            InitDictionary();
            setListBox1Index();
            setListBox2Index();
            InitStartTime();

            if (m_bIsReal)
            {
                cbxReal.Checked = true;
                tbx_time.Enabled = false;
            }
            else
            {
                cbxHistory.Checked = true;
                tbx_time.Enabled = true;
            }
            if (m_bIsHisForm)
            {
                InitHisForm();
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            string str = listBox1.SelectedItem.ToString();
            string strTime = listBox2.SelectedItem.ToString();
            m_iStep = Convert.ToInt16(str.Substring(0, str.Length - 1));
            m_iTimeIndex = m_dicTime[strTime];
            m_bIsBtnClick = true;
            this.Close();
        }

        public int getStep()
        {
            return m_iStep;
        }

        private void btn_cancel(object sender, EventArgs e)
        {
            m_bIsCancle = true;
            m_bIsBtnClick = true;
            this.Close();
        }

        private void tbx_time_Click(object sender, EventArgs e)
        {
            if (this.pl_CalendarAndTime2.Visible)
            {
                this.pl_CalendarAndTime2.Visible = false;
            }
            else
            {
                this.pl_CalendarAndTime2.Visible = true;
                pl_CalendarAndTime2.BringToFront();
            }
        }

        private void TimePick_OK2_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_end.SelectedDate.Year != 1900)
            {
                long lNow = 0L;
                long lStart = 0L;
                lNow = (long)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
                string strTime = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");
                lStart = ConvertDateTimeInt(DateTime.Parse(strTime));
                if (lStart > lNow)
                {
                    DateTime dt = Comm.ConvertIntDateTime((double)lNow * 1000);
                    m_lStartTime = lNow;
                    setTimeText(dt);
                }
                else
                {
                    this.tbx_time.Text = strTime;
                    m_lStartTime = lStart;
                }
            }

            pl_CalendarAndTime2.Visible = false;
        }

        private void TimePick_NO2_Click(object sender, EventArgs e)
        {
            pl_CalendarAndTime2.Visible = false;
        }

        private void cbxReal_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxReal.Checked)
            {
                m_bIsReal = true;
                tbx_time.Enabled = false;
                cbxHistory.Checked = false;
            }
        }

        private void cbxHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxHistory.Checked)
            {
                m_bIsReal = false;
                tbx_time.Enabled = true;
                cbxReal.Checked = false;
            }
        }

        public bool getBtnClick()
        {
            return m_bIsBtnClick;
        }

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
                            }

                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    c.Text = xe.GetAttribute("value");//设置控件的Text
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void InitHisForm()
        {
            label_q.Visible = true;
            label_q.Location = new Point(tbx_time.Location.X, tbx_time.Location.Y - label_q.Height - 5);
            tbx_time.Enabled = true;
            cbxHistory.Hide();
            cbxReal.Hide();
        }

        public void setHisForm(bool bHisForm)
        {
            m_bIsHisForm = bHisForm;
        }
    }
}
