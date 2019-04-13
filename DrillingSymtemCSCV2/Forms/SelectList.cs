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
    public partial class SelectList : RadForm
    {
        private DrillOSEntities db;
        public int isWhat { get; set;}      //1代表BushingSize,2代表BushingType,3代表WorkCode,4代表添加剂记录
        private List<BushingSize> bsize;
        private List<BushingType> btype;
        private List<WorkCode> workcode;
        private List<Table_MudChemicalsAdded> MudChemicalsAdded;
        private List<WorkType> WT_List;
        public string selectText;//需要回填的数据
        private List<string> message_list = new List<string>();
        public List<WorkCode> d_list = new List<WorkCode>();//保存删除的WorkCode数据
        public SelectList()
        {
            InitializeComponent();
        }

        private void SelectList_Load(object sender, EventArgs e)
        {

            db = new DrillOSEntities();
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            lbl_new.Visible = true;
            txt_box1.Visible = true;
            setControlLanguage();
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                switch (isWhat)
                {
                    case 1:
                        bsize = db.BushingSize.ToList();
                        break;
                    case 2:
                        btype = db.BushingType.ToList();
                        break;
                    case 3:
                        workcode = db.WorkCode.ToList();
                        break;
                    case 4:
                        MudChemicalsAdded = db.Table_MudChemicalsAdded.ToList();
                        break;
                    case 5:
                        WT_List = db.WorkType.ToList();
                        break;
                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (isWhat)
            {
                case 1:
                    //this.radLabel1.Text = "Please Select Size";
                    if (bsize.Count > 0)
                    {
                        foreach (BushingSize size in bsize)
                        {
                            ListViewDataItem l = new ListViewDataItem();
                            l.Text = size.Size;
                            l.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l);//对ListView中添加数据
                        }
                    }
                    break;
                case 2:
                    //this.radLabel1.Text = "Please Select Type";
                    if (btype.Count > 0)
                    {
                        foreach (BushingType type in btype)
                        {
                            ListViewDataItem l = new ListViewDataItem();
                            l.Text = type.Type;
                            l.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l);//对ListView中添加数据
                        }
                    }
                    break;
                case 3:
                    //this.radLabel1.Text = "Please Select CodeNo";
                    if (workcode.Count > 0)
                    {
                        foreach (WorkCode code in workcode)
                        {
                            ListViewDataItem l = new ListViewDataItem();
                            l.Text = code.CodeNo;
                            l.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l);//对ListView中添加数据
                        }
                    }
                    break;
                case 4:
                    if (MudChemicalsAdded != null)
                    {
                        //this.radLabel1.Text = "Please Select";
                        foreach (var item in MudChemicalsAdded)
                        {
                            ListViewDataItem l = new ListViewDataItem();
                            l.Text = item.Item;
                            l.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l);//对ListView中添加数据
                        }
                    }
                    break;
                case 5:
                    if(WT_List != null)
                    {
                        //this.radLabel1.Text = "Please Select";
                        foreach (var item in WT_List)
                        {
                            ListViewDataItem l = new ListViewDataItem();
                            l.Text = item.Type;
                            l.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l);//对ListView中添加数据
                        }
                    }
                    break;
                case 6:
                    //this.radLabel1.Text = "Please Select";
                    ListViewDataItem l1 = new ListViewDataItem();
                            l1.Text = "YES";
                            l1.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l1);//对ListView中添加数据
                    ListViewDataItem l2 = new ListViewDataItem();
                            l2.Text = "NO";
                            l2.TextAlignment = ContentAlignment.MiddleCenter;
                            this.rlv_selectItem.Items.Add(l2);//对ListView中添加数据
                    break;
            }
            this.rlv_selectItem.SelectedIndex = 0;
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }

        private void radListView1_ItemMouseDoubleClick(object sender, ListViewItemEventArgs e)
        {
            this.selectText = this.rlv_selectItem.SelectedItem.Text;
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
                            if (xe.GetAttribute("key") == "Message")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode n in m)
                                {
                                    XmlElement el = (XmlElement)n;
                                    message_list.Add(el.GetAttribute("value"));
                                }
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_box1.Text))
                {
                    this.selectText = txt_box1.Text;
                    switch(isWhat){
                        case 1:
                            BushingSize bs = bsize.Where(o => o.Size == selectText).FirstOrDefault();
                            if (bs == null)
                            {
                                bs = new BushingSize();
                                bs.Size = selectText;
                                bs.dataMakePGM = "AddBushingSize";
                                bs.dataMakeTime = DateTime.Now;
                                bs.dataMakeUser = AppDrill.username;
                                bs.dataUpdPGM = "AddBushingSize";
                                bs.dataUpdTime = DateTime.Now;
                                bs.dataUpdUser = AppDrill.username;
                                db.BushingSize.Add(bs);
                                db.SaveChanges();
                            }
                            break;
                        case 4:
                            DateTime dt = DateTime.Now;
                            Table_MudChemicalsAdded model = MudChemicalsAdded.Where(o => o.Item == txt_box1.Text).FirstOrDefault();
                            if (model == null)
                            {
                                model = new Table_MudChemicalsAdded();
                                model.Item = txt_box1.Text;
                                model.dataMakeTime = dt;
                                model.dataMakeUser = AppDrill.username;
                                model.dataMakePGM = "SelectList";
                                model.dataUpdTime = dt;
                                model.dataUpdUser = AppDrill.username; ;
                                model.dataUpdPGM = "SelectList";
                                db.Table_MudChemicalsAdded.Add(model);
                                db.SaveChanges();
                            }
                            break;
                    }
                }
                else
                {
                    this.selectText = this.rlv_selectItem.SelectedItem.Text;
                }
            }
            catch
            {
                this.selectText = null;
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            backgroundWorker2.CancelAsync();
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rbtn_delete_Click(object sender, EventArgs e)
        {
            switch (isWhat)
            {
                case 1:
                    try
                    {
                        if (MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            string select = rlv_selectItem.SelectedItem.Text;
                            BushingSize bs = bsize.Where(o => o.Size == select).FirstOrDefault();
                            if (bs != null)
                            {
                                db.BushingSize.Remove(bs);
                                db.SaveChanges();
                            }
                            rlv_selectItem.Items.RemoveAt(rlv_selectItem.SelectedIndex);
                        }
                    }
                    catch { }
                    break;
                case 3:
                    try
                    {
                        if (rlv_selectItem.SelectedIndex > 20)
                        {
                            if (MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OKCancel) == DialogResult.OK)
                            {
                                string select = rlv_selectItem.SelectedItem.Text;
                                WorkCode w = workcode.Where(o => o.CodeNo == select).FirstOrDefault();
                                if (w != null)
                                {
                                    db.WorkCode.Remove(w);
                                    workcode.Remove(w);
                                    db.SaveChanges();
                                    d_list.Add(w);
                                }
                                rlv_selectItem.Items.RemoveAt(rlv_selectItem.SelectedIndex);
                            }
                        }
                    }
                    catch { }
                    break;
                case 4:
                    try
                    {
                        if (MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            string select = rlv_selectItem.SelectedItem.Text;
                            Table_MudChemicalsAdded tm = MudChemicalsAdded.Where(o => o.Item == select).FirstOrDefault();
                            if (tm != null)
                            {
                                db.Table_MudChemicalsAdded.Remove(tm);
                                db.SaveChanges();
                            }
                            rlv_selectItem.Items.RemoveAt(rlv_selectItem.SelectedIndex);
                        }
                    }
                    catch { }
                    break;
                case 5:
                    try
                    {
                        if (MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            string select = rlv_selectItem.SelectedItem.Text;
                            WorkType wt = WT_List.Where(o => o.Type == select).FirstOrDefault();
                            if (wt != null)
                            {
                                db.WorkType.Remove(wt);
                                db.SaveChanges();
                            }
                            rlv_selectItem.Items.RemoveAt(rlv_selectItem.SelectedIndex);
                        }
                    }
                    catch { }
                    break;
            }
        }
    }
}
