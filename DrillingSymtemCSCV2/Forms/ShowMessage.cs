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
    public partial class ShowMessage : RadForm
    {
        private DrillOSEntities db;
        private List<Memo> Memo;//装数据
        private List<string> list_columns = new List<string>();
        private List<string> message = new List<string>();
        private long unix;
        private int m_iDrillId = 0;

        public void setDrillId(int iDrillId)
        {
            m_iDrillId = iDrillId;
        }

        public ShowMessage()
        {
            InitializeComponent();
        }

        private void ShowMessage_Load(object sender, EventArgs e)
        {
            try
            {
                setControlLanguage();//多语言对应
                db = new DrillOSEntities();//数据初始化
                setView();//设置表格属性
                backgroundWorker1.WorkerSupportsCancellation = true;//设置线程可取消
                backgroundWorker1.RunWorkerAsync();//开始执行线程
            }
            catch { }
        }

        /// <summary>
        /// 取得Memo数据
        /// </summary>
        public void setView()
        {
            this.radGridView1.BackColor = Color.FromArgb(45, 45, 48);
            this.radGridView1.ShowGroupPanel = false;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.ReadOnly = true;
            //列设定初期化
            this.radGridView1.DataSource = null;
            this.radGridView1.TableElement.BeginUpdate();
            this.radGridView1.MasterTemplate.Columns.Clear();
            //列设定
            this.radGridView1.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_columns[0], "Message"));
            this.radGridView1.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_columns[1], "User"));
            this.radGridView1.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_columns[2], "Date"));
            this.radGridView1.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_columns[3], "Depth"));
            this.radGridView1.MasterTemplate.Columns[0].Width = 250;
            this.radGridView1.MasterTemplate.Columns[1].Width = 140;
            this.radGridView1.MasterTemplate.Columns[2].Width = 170;
            this.radGridView1.MasterTemplate.Columns[3].Width = 80;
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
                                    #region 单独针对pageview
                                    //分别对每个gridview进行操作
                                    if (c.Name == "radGridView1")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//到达节点Form-xxxForm-Control-Control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            list_columns.Add(xe2.GetAttribute("value"));
                                        }
                                    }
                                    else
                                    {
                                        c.Text = xe.GetAttribute("value");//设置控件的Text
                                        break;
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Memo = db.Memo.Where(o => o.DrillID == AppDrill.DrillID && o.dataMakeUser == AppDrill.username).OrderByDescending(o => o.UnixTime).Take(200).ToList();//取前200条数据
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SetData();
                backgroundWorker1.CancelAsync();
            }
            catch { }
        }
        //设置数据
        private void SetData()
        {
            for (int i = 0; i < Memo.Count; i++)
            {
                this.radGridView1.Rows.AddNew();
                this.radGridView1.Rows[i].Height = 30;
                this.radGridView1.Rows[i].Cells[0].Value = Memo[i].Text;
                this.radGridView1.Rows[i].Cells[1].Value = Memo[i].dataMakeUser;
                double dValue = (double)Memo[i].UnixTime;
                DateTime dt =  Comm.ConvertIntDateTime(dValue * 1000);
                this.radGridView1.Rows[i].Cells[2].Value = dt;
                this.radGridView1.Rows[i].Cells[3].Value = Memo[i].Value;
            }
            //this.radGridView1.MasterTemplate.Columns[0].Width = 299;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rbtn_delete_Click(object sender, EventArgs e)
        {
            //DateTime dt = Convert.ToDateTime(radGridView1.SelectedRows[0].Cells[2].Value);
            //if ((DateTime.Now - dt).TotalSeconds > 120)
            //{
            //    MessageBox.Show(message[3]);
            //    return;//如果时间大于2分钟，不能进行删除操作
            //}
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
            {
                MessageBox.Show(message[2]);
                return;//如果时间大于2分钟，不能进行删除操作
            }
            //单元格内容改变进行的操作
            DialogResult dr = MessageBox.Show(message[1], AppDrill.message[0], MessageBoxButtons.OKCancel);
            if (dr == DialogResult.OK)
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            else
            {
                return;
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = radGridView1.SelectedRows[0].Index;//找到当前选中行的索引
            unix = (long)Memo[i].UnixTime;
            db.Memo.Remove(Memo[i]);//移除当前选中行
            db.SaveChanges();
            Memo = db.Memo.Where(o => o.DrillID == AppDrill.DrillID && o.dataMakeUser == AppDrill.username).OrderByDescending(o => o.UnixTime).Take(200).ToList();//取前200条数据
        }

        private void deleteFormMessage()
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillform = (DrillForm)frm;

                    if (m_iDrillId == drillform.m_iDrillID)
                    {
                        ((DrillForm)frm).removeLable(unix);
                    }
                }
                else if (frm is CirculateForm)
                {
                    ((CirculateForm)frm).removeLable(unix);
                }
                else if (frm is DirectionalForm)
                {
                    ((DirectionalForm)frm).removeLable(unix); 
                }
                else if (frm is DrillingGasForm)
                {
                    ((DrillingGasForm)frm).removeLable(unix);
                }
                else if (frm is DrillPVTForm)
                {
                    ((DrillPVTForm)frm).removeLable(unix);
                }
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //foreach (Form frm in Application.OpenForms)
                //{
                //    if (frm is DrillForm)
                //    {
                //        ((DrillForm)frm).RemoveMemo(unix);
                //        continue;
                //    }
                //    if (frm is CirculateForm)
                //    {
                //        ((CirculateForm)frm).RemoveMemo(unix);
                //        continue;
                //    }
                //    if (frm is DirectionalForm)
                //    {
                //        ((DirectionalForm)frm).RemoveMemo(unix); ;
                //        continue;
                //    }
                //    if (frm is DrillingGasForm)
                //    {
                //        ((DrillingGasForm)frm).RemoveMemo(unix);
                //        continue;
                //    }
                //    if (frm is DrillPVTForm)
                //    {
                //        ((DrillPVTForm)frm).RemoveMemo(unix);
                //        continue;
                //    }
                //}
                deleteFormMessage();
                setView();
                SetData();
                backgroundWorker2.CancelAsync();
            }
            catch { }
        }
    }
}
