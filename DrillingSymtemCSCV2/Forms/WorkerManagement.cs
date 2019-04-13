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
    public partial class WorkerManagement : RadForm
    {
        private DrillOSEntities db;//数据库连接对象
        public List<WorkType> list_type = new List<WorkType>();//Rota传过来的值
        private List<Worker> worker_list = new List<Worker>();//获取工人列表
        private List<string> message = new List<string>();
        public bool isSelect = false;
        public Worker worker;
        public WorkerManagement()
        {
            InitializeComponent();
        }
        private void WorkerManagement_Load(object sender, EventArgs e)
        {
            setControlLanguage();
            db = new DrillOSEntities();//初始化
            if (isSelect)
            {
                rbtn_Select.Visible = true;
                rbtn_Cancel.Visible = true;
            }
            try
            {
                backgroundWorker1.WorkerSupportsCancellation = true;//设置当前异步操作可取消
                backgroundWorker1.RunWorkerAsync();//开始执行异步
            }
            catch { }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                worker_list = db.Worker.ToList();//取出所有的用户信息，一般使用的人数很少
                list_type = db.WorkType.ToList();
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.rgv_workers.Rows.Clear();
            //设置RadGridView信息及赋值
            for (int i = 0; i < worker_list.Count; i++)
            {
                this.rgv_workers.Rows.AddNew();
                this.rgv_workers.Rows[i].Cells[0].Value = worker_list[i].Name;//姓名
                this.rgv_workers.Rows[i].Cells[1].Value = list_type.Where(o=>o.ID==worker_list[i].TypeWork).Select(o=>o.Type).FirstOrDefault();//工种
                this.rgv_workers.Rows[i].Cells[2].Value = worker_list[i].EmpNO;//姓名
            }
            //表格头设置
            try
            {
                this.rgv_workers.Columns[0].HeaderText = message[2];
                this.rgv_workers.Columns[1].HeaderText = message[3];
                this.rgv_workers.Columns[2].HeaderText = message[4];
            }
            catch { }
            backgroundWorker1.CancelAsync();//执行完成
        }
        private void rbtn_addWorker_Click(object sender, EventArgs e)
        {
            try
            {
                AddWorkerForm add = new AddWorkerForm();
                add.list_type = list_type;//传递工种类型
                add.db = db;
                add.editType = 0;//新增
                add.ShowDialog();
                if (add.isEdit)
                {
                    worker_list.Add(add.worker);
                    db.Worker.Add(add.worker);
                    db.SaveChanges();
                    //刷新表
                    this.rgv_workers.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < worker_list.Count; i++)
                    {
                        this.rgv_workers.Rows.AddNew();
                        this.rgv_workers.Rows[i].Cells[0].Value = worker_list[i].Name;//姓名
                        this.rgv_workers.Rows[i].Cells[1].Value = add.list_type.Where(o => o.ID == worker_list[i].TypeWork).Select(o => o.Type).FirstOrDefault();//工种名
                        this.rgv_workers.Rows[i].Cells[2].Value = worker_list[i].EmpNO;//姓名
                    }
                }
            }
            catch { }
        }

        private void rbtn_editWorker_Click(object sender, EventArgs e)
        {
            try
            {
                AddWorkerForm add = new AddWorkerForm();
                add.list_type = list_type;//传递工种类型
                add.editType = 1;//修改
                //获取选中行工人传入编辑界面
                int rowIndex = rgv_workers.SelectedRows[0].Index;
                add.worker = worker_list[rowIndex];
                add.ShowDialog();
                if (add.isEdit)
                {
                    worker_list[rowIndex] = add.worker;
                    db.SaveChanges();
                    //刷新表
                    this.rgv_workers.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < worker_list.Count; i++)
                    {
                        this.rgv_workers.Rows.AddNew();
                        this.rgv_workers.Rows[i].Cells[0].Value = worker_list[i].Name;//姓名
                        this.rgv_workers.Rows[i].Cells[1].Value = add.list_type.Where(o => o.ID == worker_list[i].TypeWork).Select(o => o.Type).FirstOrDefault();//工种名
                        this.rgv_workers.Rows[i].Cells[2].Value = worker_list[i].EmpNO;//姓名
                    }
                }
            }
            catch { }
        }

        private void rbtn_deleteWorker_Click(object sender, EventArgs e)
        {
            try
            {
                int p = rgv_workers.SelectedRows[0].Index;
                if (p < 0)
                    return;
                DialogResult dr = MessageBox.Show(message[1], message[0], MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    db.Worker.Remove(worker_list[p]);//移除当前元素
                    db.SaveChanges();
                    worker_list.RemoveAt(p);
                    this.rgv_workers.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < worker_list.Count; i++)
                    {
                        this.rgv_workers.Rows.AddNew();
                        this.rgv_workers.Rows[i].Cells[0].Value = worker_list[i].Name;//姓名
                        this.rgv_workers.Rows[i].Cells[1].Value = list_type.Where(o => o.ID == worker_list[i].TypeWork).Select(o => o.Type).FirstOrDefault();//工种名
                        this.rgv_workers.Rows[i].Cells[2].Value = worker_list[i].EmpNO;//姓名
                    }
                }
            }
            catch { }
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
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList xn_m = xe.ChildNodes;
                                foreach (XmlNode node_m in xn_m)
                                {
                                    XmlElement mess = (XmlElement)node_m;
                                    message.Add(mess.GetAttribute("value"));
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

        private void rbtn_Select_Click(object sender, EventArgs e)
        {
            try
            {
                int rowIndex = rgv_workers.SelectedRows[0].Index;
                worker = worker_list[rowIndex];
            }
            catch
            {
                worker = null;
            }
            this.Close();
        }

        private void rbtn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
