using DrillingSymtemCSCV2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Xml;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class EditBoxForm : RadForm
    {
        public string SendText;
        public List<MemoContent> MemoList = new List<MemoContent>();

        public EditBoxForm()
        {
            InitializeComponent();
        }

        private void EditBoxForm_Load(object sender, EventArgs e)
        {
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            cbo_Select.SelectedValueChanged += comboBox1_SelectedValueChanged;
            //设置语言
            setControlLanguage();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ListItem selectedItem = (ListItem)cbo_Select.SelectedItem;
            if (!string.IsNullOrEmpty(selectedItem.Text))
            {
                txt_Input.Text = selectedItem.Text;
            }
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var dbContext = new DrillOSEntities())
                {
                    MemoList = dbContext.MemoContent.ToList();
                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SelectList();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 钻井井号选择框

        private void SelectList()
        {
            try
            {
                var selectdata = new ArrayList();
                selectdata.Add(new ListItem("-1", ""));
                foreach (var item in MemoList)
                {
                    selectdata.Add(new ListItem(item.ID.ToString(), item.MemoText));
                }
                cbo_Select.DataSource = selectdata;
            }
            catch { }
        }

        #endregion

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.SendText = txt_Input.Text;
            this.Close();
        }

        private void radButton2_Click(object sender, EventArgs e)
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
    }
}
