using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq;
using System.Collections;
namespace DrillingSymtemCSCV2.Forms
{
    public partial class ResetForm : Telerik.WinControls.UI.RadForm
    {
        private bool m_bIsResetAll = false;
        private bool m_bIsClose = false;

        public ResetForm()
        {
            InitializeComponent();
        }
        private void SettingForm2_Load(object sender, EventArgs e)
        {
            setControlLanguage();
            rbtn_Type1.Checked = true;
        }

        public bool getResetValue()
        {
            return m_bIsResetAll;
        }

        public bool getClose()
        {
            return m_bIsClose;
        }

        #region 读取xml文件设置语言
        private void setControlLanguage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == "ResetForm")//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    if (c.Name == "radPanel1")
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
                                                }
                                            }
                                        }
                                    }
      
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


        private void rbtn_OK_Click(object sender, EventArgs e)
        {
            if (rbtn_Type1.Checked)
            {
                m_bIsResetAll = false;
            }
            else if (rbtn_Type2.Checked)
            {
                m_bIsResetAll = true;
            }

            this.Close();
            m_bIsClose = false;

        }

        private void ResetForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_bIsClose = true;
        }

        protected override void WndProc(ref  Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                return;
            }

            base.WndProc(ref m);
        }

    }
}
