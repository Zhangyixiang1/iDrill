using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class AddWellForm : RadForm
    {
        private List<string> list_error = new List<string>();//定义错误提示消息
        private DrillOSEntities db;//数据库访问对象
        private List<Drill> list_drill;//用于缓存Drill中的数据
        private bool isCancle = false;//判断是否是取消
        public AddWellForm()
        {
            InitializeComponent();
        }

        private void AddWellForm_Load(object sender, EventArgs e)
        {
            //设置语言
            setControlLanguage();
            db = new DrillOSEntities();
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
        }
        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_drill = db.Drill.ToList();
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

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
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    if (c.Name == "rlbl_error")
                                    {
                                        XmlNodeList xn_list2=xe.ChildNodes;
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            list_error.Add(xe2.GetAttribute("value"));
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        c.Text = xe.GetAttribute("value");//设置控件的Text
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        private void rbtn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtn_OK_Click(object sender, EventArgs e)
        {
            isCancle = false;
            rbtn_OK.Enabled = false;
            try
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch
            {
                MessageBox.Show(AppDrill.message[5]);//请勿重复点击
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string drillNo;
            if (string.IsNullOrEmpty(rtxt_drillNo.Text.Trim()))
            {
                isCancle = true;
                MessageBox.Show(list_error[0]);
                return;
            }
            drillNo = rtxt_drillNo.Text.Trim();
            try
            {
                Drill drill = list_drill.Where(o => o.DrillNo == drillNo).FirstOrDefault();
                if (drill == null)
                {
                    DialogResult dr= MessageBox.Show(list_error[1],"", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.Cancel)
                    {
                        isCancle = true;
                        return;//如果取消，直接return
                    }
                    Drill d = new Drill();
                    d.DrillNo = rtxt_drillNo.Text;
                    d.Lease = rtxt_lease.Text;
                    d.Company = rtxt_company.Text;
                    d.CompanyMan = rtxt_cm.Text;
                    d.Contractor = rtxt_contractor.Text;
                    d.DateRelease = rtxt_dr.Text;
                    d.DateSpud = rtxt_ds.Text;
                    d.ToolPusher = rtxt_tp.Text;
                    d.Country = rtxt_country.Text;
                    d.dataMakePGM = "AddWell";
                    d.dataMakeTime = DateTime.Now;
                    d.dataMakeUser = AppDrill.username;
                    d.dataUpdPGM = "AddWell";
                    d.dataUpdTime = DateTime.Now;
                    d.dataUpdUser = AppDrill.username;
                    db.Drill.Add(d);
                    db.SaveChanges();
                }
                else
                {
                    DialogResult dr = MessageBox.Show(list_error[2], "", MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.Cancel)
                    {
                        isCancle = true;
                        return;//如果取消，直接return
                    }
                    drill.Lease = rtxt_lease.Text;
                    drill.Company = rtxt_company.Text;
                    drill.CompanyMan = rtxt_cm.Text;
                    drill.Contractor = rtxt_contractor.Text;
                    drill.DateRelease = rtxt_dr.Text;
                    drill.DateSpud = rtxt_ds.Text;
                    drill.ToolPusher = rtxt_tp.Text;
                    drill.Country = rtxt_country.Text;
                    drill.dataUpdPGM = "AddWell";
                    drill.dataUpdTime = DateTime.Now;
                    drill.dataUpdUser = AppDrill.username;
                    db.SaveChanges();
                }
                MessageBox.Show(AppDrill.message[3]);//保存成功
            }
            catch 
            {
                MessageBox.Show(AppDrill.message[4]);//保存失败
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_OK.Enabled = true;
            if (!isCancle)
                this.Close();//如果点击的非取消
            backgroundWorker2.CancelAsync();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
