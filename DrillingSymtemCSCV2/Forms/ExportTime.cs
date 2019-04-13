using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class ExportTime : RadForm
    {
        private HistoryDataForm m_historyForm = null;
        private Point m_ptEndTime;

        public ExportTime()
        {
            InitializeComponent();
        }

        public void setHisForm(HistoryDataForm historyForm)
        {
            m_historyForm = historyForm;
        }

        public void setStartEndTime(string strStartTime, bool bEnd)
        {
            if (bEnd)
            {
                radTextStart.Text = strStartTime;
            }
            else
            {
                radTextEnd.Text = strStartTime;
            }
        }


        public void setFormLaction()
        {
            if (null == m_historyForm)
            {
                return;
            }

            int iHeight = m_historyForm.Height;
            int iWidth = m_historyForm.Width;
            this.Location = new Point((iWidth - this.Width) / 2, (iHeight - this.Height) / 2);
        }

        public Point getLocation()
        {
            m_ptEndTime = new Point(this.Location.X - 3, this.Location.Y + this.Height - 30);
            return m_ptEndTime;
        }

        private void HisWellList_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(radTextStart.Text) || string.IsNullOrEmpty(radTextEnd.Text))
            {
                MessageBox.Show("请选择开始时间和结束时间");
                return;
            }

            if (null != m_historyForm)
            {
                m_historyForm.showEnd(false);
                bool bSuccess = m_historyForm.queryInit(false);
                if (bSuccess)
                {
                    m_historyForm.exportData();
                }
                else
                {
                    return;
                }
            }
            this.Close();
        }

        private void radTextStart_Click(object sender, EventArgs e)
        {
            if (null != m_historyForm)
            {
                m_historyForm.showEnd();
                m_historyForm.setStartEnd(true);
            }
        }

        private void radTextEnd_Click(object sender, EventArgs e)
        {
            if (null != m_historyForm)
            {
                m_historyForm.showEnd();
                m_historyForm.setStartEnd(false);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
