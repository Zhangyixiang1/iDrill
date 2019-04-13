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
    public partial class SelectTable : RadForm
    {
        public string TabelName = null;
        public List<Table_BitRecord> BitRecordList=new List<Table_BitRecord>();
        public List<Table_DrillingAssembly> DrillingAssemblyList=new List<Table_DrillingAssembly>();
        public Table_BitRecord BitRecord;
        public Table_DrillingAssembly DrillingAssembly;
        public DrillOSEntities db = new DrillOSEntities();
        public SelectTable()
        {
            InitializeComponent();
        }

        private void SelectTable_Load(object sender, EventArgs e)
        {
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            setControlLanguage();
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                switch (TabelName)
                {
                    case "BitRecord":
                        BitRecordList = db.Table_BitRecord.ToList();
                        break;
                    case "DrillAsmb":
                        DrillingAssemblyList = db.Table_DrillingAssembly.ToList();
                        break;
                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gvw_selectTable.Rows.Clear();
            GridViewRowInfo rowInfo;
            switch (TabelName)
            {
                case "BitRecord":
                    gvw_selectTable.Columns.Add("BIT NO.");
                    gvw_selectTable.Columns.Add("SIZE");
                    gvw_selectTable.Columns.Add("IADC CODE");
                    gvw_selectTable.Columns.Add("SERIAL NO.");
                    gvw_selectTable.Columns.Add("JETS");
                    gvw_selectTable.Columns.Add("TFA");
                    gvw_selectTable.Columns.Add("DEPTH OUT");
                    gvw_selectTable.Columns.Add("DEPTH IN");
                    gvw_selectTable.Columns.Add("TOTAL DRILLED");
                    gvw_selectTable.Columns.Add("TOTAL HOURS");
                    if (BitRecordList != null)
                    {
                        var i = 0;
                        foreach (var item in BitRecordList)
                        {
                            rowInfo = gvw_selectTable.Rows.AddNew();
                            rowInfo.Height = 25;
                            gvw_selectTable.Rows[i].Cells[0].Value = item.BitNo;
                            gvw_selectTable.Rows[i].Cells[1].Value = item.Size;
                            gvw_selectTable.Rows[i].Cells[2].Value = item.IADCCode;
                            gvw_selectTable.Rows[i].Cells[3].Value = item.SerialNO;
                            gvw_selectTable.Rows[i].Cells[4].Value = item.JETS;
                            gvw_selectTable.Rows[i].Cells[5].Value = item.TFA;
                            gvw_selectTable.Rows[i].Cells[6].Value = item.DepthOut;
                            gvw_selectTable.Rows[i].Cells[7].Value = item.DepthIn;
                            gvw_selectTable.Rows[i].Cells[8].Value = item.TotalDrilled;
                            gvw_selectTable.Rows[i].Cells[9].Value = item.TotalHours;
                            i++;
                        }
                    }
                    break;

                case "DrillAsmb":
                    gvw_selectTable.Columns.Add("CODE NO.");
                    gvw_selectTable.Columns.Add("ITEM");
                    var j = 0;
                    gvw_selectTable.Columns.Add("LENGTH");
                    if (DrillingAssemblyList != null)
                    {
                        foreach (var item in DrillingAssemblyList)
                        {
                            rowInfo = gvw_selectTable.Rows.AddNew();
                            rowInfo.Height = 25;
                            gvw_selectTable.Rows[j].Cells[0].Value = item.CodeNo;
                            gvw_selectTable.Rows[j].Cells[1].Value = item.Item;
                            gvw_selectTable.Rows[j].Cells[2].Value = item.Length;
                            j++;
                        }
                    }
                    break;
            }
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        private void Remove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            switch (TabelName)
            {
                case "BitRecord":
                    if (BitRecordList != null)
                    {
                        this.BitRecord = BitRecordList[gvw_selectTable.CurrentRow.Index];
                    }
                    break;
                case "DrillAsmb":
                    if (!string.IsNullOrEmpty(txt_NO.Text) && !string.IsNullOrEmpty(txt_item.Text) && !string.IsNullOrEmpty(txt_length.Text))
                    {

                        Table_DrillingAssembly td = new Table_DrillingAssembly();
                        td.CodeNo = txt_NO.Text;
                        td.Item = txt_item.Text;
                        td.Length = txt_length.Text;
                        td.dataMakePGM = "Add";
                        td.dataMakeTime = DateTime.Now;
                        td.dataMakeUser = AppDrill.username;
                        td.dataUpdPGM = "Add";
                        td.dataUpdTime = DateTime.Now;
                        td.dataUpdUser = AppDrill.username;
                        db.Table_DrillingAssembly.Add(td);
                        db.SaveChanges();
                        this.DrillingAssembly = td;

                    }
                    else if (DrillingAssemblyList != null)
                    {
                        this.DrillingAssembly = DrillingAssemblyList[gvw_selectTable.CurrentRow.Index];
                    }
                    break;
            }
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
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rbtn_delete_Click(object sender, EventArgs e)
        {
            switch (TabelName)
            {
                case "DrillAsmb":
                    try
                    {
                        if (DrillingAssemblyList != null)
                        {
                            DrillingAssembly = DrillingAssemblyList[gvw_selectTable.CurrentRow.Index];
                            if (DrillingAssembly != null)
                            {
                                if (MessageBox.Show("", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {
                                    db.Table_DrillingAssembly.Remove(DrillingAssembly);
                                    db.SaveChanges();
                                    DrillingAssemblyList.Remove(DrillingAssembly);//重新获取数据
                                    gvw_selectTable.Rows.RemoveAt(gvw_selectTable.CurrentRow.Index);//移除当前选中的行
                                }
                            }
                        }
                    }
                    catch { }
                    DrillingAssembly = null;
                    break;
            }
        }
    }
}
