using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class AddWorkerForm : RadForm
    {
        public DrillOSEntities db;
        public int editType { get; set; }   //0新增，1编辑
        public Worker worker = new Worker();
        public List<WorkType> list_type = new List<WorkType>();//Rota传过来的值
        public bool isEdit = false;     //判断用户是否进行操作
        private List<string> message_list = new List<string>();
        public AddWorkerForm()
        {
            InitializeComponent();
        }

        private void AddWorkerForm_Load(object sender, EventArgs e)
        {
            try
            {
                db = new DrillOSEntities();
                list_type = db.WorkType.ToList();//重新取值
                var rld_list = new List<RadListDataItem>();
                if (editType == 1)
                {
                    rtxt_name.Text = worker.Name;
                    rtxt_type.Text = list_type.Where(o => o.ID == worker.TypeWork).FirstOrDefault().Type;
                    rtxt_EmpNo.Text = worker.EmpNO;
                }
            }
            catch { }
        }

        private void rbtn_ok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(rtxt_name.Text))
            {
                MessageBox.Show(message_list[0]);
                return;
            }
            worker.Name = rtxt_name.Text;
            worker.TypeWork = list_type.Where(o => o.Type == rtxt_type.Text).FirstOrDefault().ID;
            worker.EmpNO = rtxt_EmpNo.Text;
            if (editType == 0)
            {
                worker.dataMakePGM = "AddWorkForm";
                worker.dataMakeTime = DateTime.Now;
                worker.dataMakeUser = AppDrill.username;
            }
            worker.dataUpdPGM = "AddWorkerFomr";
            worker.dataUpdTime = DateTime.Now;
            worker.dataUpdUser = AppDrill.username;
            isEdit = true;
            this.Close();
        }

        private void rbtn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            if (xe.GetAttribute("key") == "lbl_message")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                foreach (XmlNode node3 in xn_list2)
                                {
                                    XmlElement xe3 = (XmlElement)node3;
                                    message_list.Add(xe3.GetAttribute("value"));
                                }
                                continue;
                            }
                            //循环每个控件，设置当前语言应设置的值
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
            catch { }
        }
        #endregion

        private void rtxt_type_Click(object sender, EventArgs e)
        {
            SelectList sl = new SelectList();
            sl.isWhat = 5;
            sl.ShowDialog();
            try
            {
                list_type = db.WorkType.ToList();
                if (sl.selectText != null)
                {
                    WorkType wt = list_type.Where(o => o.Type == sl.selectText).FirstOrDefault();
                    if (wt == null)
                    {
                        wt = new WorkType();
                        wt.Type = sl.selectText;
                        wt.dataMakePGM = "AddWorker";
                        wt.dataMakeTime = DateTime.Now;
                        wt.dataMakeUser = AppDrill.username;
                        wt.dataUpdPGM = "AddWorker";
                        wt.dataUpdTime = DateTime.Now;
                        wt.dataUpdUser = AppDrill.username;
                        db.WorkType.Add(wt);
                        list_type.Add(wt);
                        db.SaveChanges();
                    }
                    this.rtxt_type.Text = sl.selectText;
                }
            }
            catch { }
        }

    }
}
