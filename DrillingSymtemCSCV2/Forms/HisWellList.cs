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
using System.Xml;

namespace DrillingSymtemCSCV2.Forms
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class HisWellList : RadForm
    {
        private DrillOSEntities _db;
        List<Drill> drillinfo;
        private int m_iDrillNo = 0;
        private int m_iRowIndex = 0;
        private List<string> m_strHeadList = new List<string>();

        public HisWellList(int iRowIndex)
        {
            InitializeComponent();
            _db = new DrillOSEntities();
            drillinfo = new List<Drill>();
            m_iRowIndex = iRowIndex;
        }

        private void HisWellList_Load(object sender, EventArgs e)
        {
            setControlLanguage();

            if (null == _db.Drill)
            {
                return;
            }

            try
            {
                drillinfo = _db.Drill.ToList();
                var drill = (from r in _db.Drill select new {   Operator = r.Operator, 
                                                                Lease = r.Lease, 
                                                                DateSpud = r.DateSpud, 
                                                                DrillNo = r.DrillNo, 
                                                                ID = r.ID 
                                                            }).ToList();

                dataGridView1.DataSource = drill;
                dataGridView1.Columns[0].Width = 350;
                dataGridView1.Columns[2].Width = 160;
                for (int i = 0; i < m_strHeadList.Count; ++i)
                {
                    dataGridView1.Columns[i].HeaderText = m_strHeadList[i];
                }

                dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                dataGridView1.DefaultCellStyle.SelectionBackColor = Color.Gray;
                dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
                dataGridView1.CurrentCell = dataGridView1.Rows[m_iRowIndex].Cells[0];
            }
            catch
            {
            }
        }

        public int getDrillNO()
        {
            return m_iDrillNo;
        }

        public int getRowIndex()
        {
            return m_iRowIndex;
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null)
            {
                MessageBox.Show("请选择一个数据源！");
                return;
            }

            m_iRowIndex = dataGridView1.CurrentCell.RowIndex;
            m_iDrillNo = (int)dataGridView1.SelectedRows[0].Cells[4].Value;

            this.Close();
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


                            if (xe.GetAttribute("key") == "Head_List")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control

                                foreach (XmlNode node3 in xn_list2)
                                {
                                    XmlElement xe3 = (XmlElement)node3;
                                    m_strHeadList.Add(xe3.GetAttribute("value"));
                                }
                                continue;
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
    }
}
