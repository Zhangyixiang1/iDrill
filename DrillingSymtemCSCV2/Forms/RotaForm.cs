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
using Telerik.Pivot.Core;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class RotaForm : RadForm
    {
        private DrillOSEntities db;
        private List<Rota> list_rota;
        private List<WorkType> list_type=new List<WorkType>();//用于存取工种信息(通过XML来获取)
        private List<string> rota_str=new List<string>();//用于存取需要对语言对应的值班表
        private List<string> message = new List<string>();//装翻译用的弹出框等消息
        private bool isEdit = false;//判断用户是否进行编辑操作
        private const int SHIFT=2;//定义班次
        private string DayShift;
        public RotaForm()
        {
            InitializeComponent();
        }

        private void RotaForm_Load(object sender, EventArgs e)
        {
            try
            {
                DayShift = System.Configuration.ConfigurationManager.AppSettings["DayShift"].ToString();
                setControlLanguage();
                InitFormatColumns();
                if (AppDrill.permissionId != 1){
                    rbtn_workerManagement.Enabled = false;
                    btn_save.Enabled = false;
                }
                    
                if (AppDrill.language == "CN")
                {
                    this.StartTime.Culture = new System.Globalization.CultureInfo("zh-CN");
                    this.EndTime.Culture = new System.Globalization.CultureInfo("zh-CN");
                }
                db = new DrillOSEntities();
                #region 异步加载数据
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
                backgroundWorker1.RunWorkerAsync(); //开始
                #endregion
            }
            catch { }
        }
        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_rota = db.Rota.ToList();
                list_type = db.WorkType.ToList();
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartTime.Value = DateTime.Now.AddDays(-15);
            EndTime.Value = DateTime.Now.AddDays(15);
            try
            {
                InitGrid();//初始化表格
            }
            catch { }
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        public class listdata
        {
            public string Date { get; set; }
            public string Shift { get; set; }
            public string Type { get; set; }
            public string Name { get; set; }
            public string Content { get; set; }
        }

        //初始化值班表,从今天开始，显示7天的值班情况
        private void InitGrid(string start=null,string end=null)
        {   
            List<listdata> source = new List<listdata>();
            listdata model = new listdata();
            int days = 7;//默认定义循环的天数
            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                days = ((TimeSpan)(Convert.ToDateTime(end) - Convert.ToDateTime(start))).Days+1;
            }
            //7天
            for (int i = 0; i < days; i++)
            {
                //早晚两班
                for (int j = 1; j <= SHIFT; j++)
                {
                    //6工种
                    foreach (WorkType w in list_type)
                    {
                        model = new listdata();
                        if (string.IsNullOrEmpty(start)||string.IsNullOrEmpty(end))
                        {
                            model.Date = DateTime.Now.AddDays(i).ToString("yyy-MM-dd");
                        }
                        else
                        {
                            model.Date = Convert.ToDateTime(start).AddDays(i).ToString("yyyy-MM-dd");
                        }
                        model.Shift = j == 1 ? rota_str[5] + "(" + DayShift + ":00-" + (int.Parse(DayShift) + 12) + ":00)" : j == 2 ? rota_str[6] + "(" + (int.Parse(DayShift) + 12) + ":00-" + DayShift + ":00)" : "";//对应数据库中的早=1晚=2
                        model.Type = w.Type;
                        Rota r = list_rota.Where(o => Convert.ToDateTime(o.WorkTime).ToString("yyyy-MM-dd") == model.Date&&o.TypeWork==w.ID&&o.Shift==j).FirstOrDefault();
                        if (r != null)
                        {
                            model.Name = r.Name;
                            model.Content = r.Content;
                        }
                        else
                        {
                            model.Name = "";
                            model.Content = "";
                        }
                        source.Add(model);
                    }
                }
            }
            dgv_rota.DataSource=source;
        }

        #region "合并单元格(多行多列)"

        List<String> colsHeaderText_H = new List<String>();
 
        private void InitFormatColumns()
        {
            colsHeaderText_H.Add(rota_str[0]);
            colsHeaderText_H.Add(rota_str[1]);
            colsHeaderText_H.Add(rota_str[2]);
        }

        //绘制单元格
        private void dataGridView1_CellPainting(object sender, System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            this.dgv_rota.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            this.dgv_rota.Columns[1].Width = 300;//设置列宽度
            this.dgv_rota.Columns[2].Width = 300;
            this.dgv_rota.Columns[3].Width = 400;
            this.dgv_rota.Columns[4].Width = 650;
            //标题栏对语言对应
            this.dgv_rota.Columns[0].HeaderText = rota_str[0];
            this.dgv_rota.Columns[1].HeaderText = rota_str[1];
            this.dgv_rota.Columns[2].HeaderText = rota_str[2];
            this.dgv_rota.Columns[3].HeaderText = rota_str[3];
            this.dgv_rota.Columns[4].HeaderText = rota_str[4];

            this.dgv_rota.ColumnHeadersHeight = 30;//设置标题栏高度
            this.dgv_rota.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;//设置标题栏高度可调节
            e.CellStyle.Font = new System.Drawing.Font("Segoe UI", 12F);
            e.CellStyle.BackColor = Color.FromArgb(45,45,48);
            e.CellStyle.ForeColor = Color.White;
            this.dgv_rota.EnableHeadersVisualStyles = false;
            this.dgv_rota.RowHeadersVisible = false;
            foreach (string fieldHeaderText in colsHeaderText_H)
            {
                //纵向合并
                if (e.ColumnIndex >= 0 && this.dgv_rota.Columns[e.ColumnIndex].HeaderText == fieldHeaderText && e.RowIndex >= 0)
                {
                    using (
                        Brush gridBrush = new SolidBrush(this.dgv_rota.GridColor),
                        backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                    {
                        using (Pen gridLinePen = new Pen(gridBrush))
                        {
                            // 擦除原单元格背景
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
 
                            /****** 绘制单元格相互间隔的区分线条，datagridview自己会处理左侧和上边缘的线条，因此只需绘制下边框和和右边框
                             DataGridView控件绘制单元格时，不绘制左边框和上边框，共用左单元格的右边框，上一单元格的下边框*****/
 
                            //不是最后一行且单元格的值不为null
                            if (e.RowIndex < this.dgv_rota.RowCount - 1 && this.dgv_rota.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value != null)
                            {
                                //手动设置第一列的值显示在第N行
                                if (e.ColumnIndex == 0)
                                {
                                    int position = SHIFT * list_type.Count % 2 == 0 ? SHIFT * list_type.Count / 2 - 1 : SHIFT * list_type.Count / 2;//用于定位日期显示位置
                                    if (e.RowIndex % (SHIFT * list_type.Count) == position)
                                    {
                                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Convert.ToDateTime(e.Value.ToString())<=DateTime.Now?Brushes.Gray:Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                }
                                //设置第二列
                                if (e.ColumnIndex == 1)
                                {
                                    int position = list_type.Count % 2 == 0 ? list_type.Count / 2 - 1 : list_type.Count / 2;//定位三班位置
                                    if (e.RowIndex % list_type.Count == position)
                                    {
                                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Convert.ToDateTime(this.dgv_rota.Rows[e.RowIndex].Cells[0].Value.ToString()) <= DateTime.Now ? Brushes.Gray : Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                }
                                //若与下一单元格值不同
                                if (e.Value.ToString() != this.dgv_rota.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Value.ToString())
                                {
                                    //下边缘的线
                                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                                    //绘制值
                                    if (e.Value != null)
                                    {
                                        if (e.ColumnIndex == 0||e.ColumnIndex==1)
                                        {
                                            e.Graphics.DrawString("", e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                        }
                                        else
                                        {
                                            e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                        }
                                    }
                                }
                                //若与下一单元格值相同 
                                else
                                {
                                    //背景颜色
                                    //e.CellStyle.BackColor = Color.LightPink;   //仅在CellFormatting方法中可用
                                    this.dgv_rota.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.PowderBlue;
                                    this.dgv_rota.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].Style.BackColor = Color.PowderBlue;
                                    //只读（以免双击单元格时显示值）
                                    this.dgv_rota.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly = true;
                                    this.dgv_rota.Rows[e.RowIndex + 1].Cells[e.ColumnIndex].ReadOnly = true;
                                    this.dgv_rota.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                                    this.dgv_rota.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                                }
                            }
                            //最后一行或单元格的值为null
                            else
                            {
                                //下边缘的线
                                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1,
                                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
 
                                //绘制值
                                if (e.Value != null)
                                {
                                    if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                                    {
                                        e.Graphics.DrawString("", e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                    else
                                    {
                                        e.Graphics.DrawString(e.Value.ToString(), e.CellStyle.Font,
                                            Brushes.Crimson, e.CellBounds.X + 2,
                                            e.CellBounds.Y + 2, StringFormat.GenericDefault);
                                    }
                                }
                            }
                            ////左侧的线（）
                            //e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            //    e.CellBounds.Top, e.CellBounds.Left,
                            //    e.CellBounds.Bottom - 1);
 
                            //右侧的线
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                                e.CellBounds.Top, e.CellBounds.Right - 1,
                                e.CellBounds.Bottom - 1);
 
                            //设置处理事件完成（关键点），只有设置为ture,才能显示出想要的结果。
                            e.Handled = true;
                        }
                    }
                }
            }
 
        }
        #endregion

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 3 && e.RowIndex >= 0 && AppDrill.permissionId==1)
                {
                    if (e.ColumnIndex == 3)
                    {
                        DateTime today = DateTime.Now;//将今天的时间转换一下
                        DateTime date = Convert.ToDateTime(this.dgv_rota.Rows[e.RowIndex].Cells[0].Value.ToString());
                        //当日期
                        if (date > today)
                        {
                            SelectNameForm s = new SelectNameForm();
                            string typeName=this.dgv_rota.Rows[e.RowIndex].Cells[2].Value.ToString();
                            s.workType = list_type.Find(o => o.Type == typeName).ID;
                            s.list_type = list_type;
                            s.ShowDialog();
                            if (s.RotaName != null)
                            {
                                this.dgv_rota.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = s.RotaName;
                                isEdit = true;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            catch { }
            
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                TimeSpan ts = Convert.ToDateTime(EndTime.Text) - Convert.ToDateTime(StartTime.Text);
                double s = ts.TotalDays;
                if (ts.TotalDays >= 365)
                {
                    MessageBox.Show(message[0], message[1]);
                    StartTime.Value = DateTime.Now.AddDays(-15);
                    EndTime.Value = DateTime.Now.AddDays(15);
                    return;
                }
                string start = Convert.ToDateTime(StartTime.Text).ToString("yyyy-MM-dd");//获取日期控件的值
                string end = Convert.ToDateTime(EndTime.Text).ToString("yyyy-MM-dd");//获取日期控件的值
                InitGrid(start, end);//重绘界面
            }
            catch { }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch 
            {
                MessageBox.Show(AppDrill.message[5]);//请勿重复提交
            }
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
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    switch (c.Name)
                                    {
                                        case "dgv_rota":
                                            XmlNodeList rota = node.ChildNodes;//到达节点Form-xxxForm-Control-Control
                                            foreach (XmlNode node2 in rota)
                                            {
                                                XmlElement xe2 = (XmlElement)node2;
                                                rota_str.Add(xe2.GetAttribute("value"));
                                            } 
                                            break;
                                        
                                        default:
                                             c.Text = xe.GetAttribute("value");//设置控件的Text
                                            break;
                                    }  
                                }
                            }
                            if (xe.GetAttribute("key") == "Message")
                            {
                                XmlNodeList m=node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    message.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                        }
                    }
                }
            }
            catch { }
        }
        #endregion       

        private void RotaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppDrill.permissionId != 1)
                this.Dispose();
            try
            {
                if (isEdit)
                {
                    //单元格内容改变进行的操作
                    DialogResult dr = MessageBox.Show(AppDrill.message[2], AppDrill.message[1], MessageBoxButtons.OKCancel);
                    if (dr == DialogResult.OK)
                    {
                        backgroundWorker2.WorkerSupportsCancellation = true;
                        backgroundWorker2.RunWorkerAsync();
                        this.Close();
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                     
                }
                else
                {
                    //无改变直接退出
                    this.Dispose();
                }
            }
            catch 
            {
                e.Cancel = true;
                MessageBox.Show(message[3]);//保存失败
            }
            
        }

        //保存数据操作
        private void SaveData()
        {
            int dataCount = this.dgv_rota.Rows.Count;//获取总行数
            for (int i = 0; i < dataCount; i++)
            {
                string Name = this.dgv_rota.Rows[i].Cells[3].Value == null ? "" : this.dgv_rota.Rows[i].Cells[3].Value.ToString();
                string Content = this.dgv_rota.Rows[i].Cells[4].Value == null ? "" : this.dgv_rota.Rows[i].Cells[4].Value.ToString();
                int Shift = ((i / list_type.Count) + 1) % SHIFT == 0 ? 2 : ((i / list_type.Count) + 1) % SHIFT;//判断早1晚2
                int TypeWork = list_type.Where(o => o.Type == this.dgv_rota.Rows[i].Cells[2].Value.ToString()).Select(o => o.ID).FirstOrDefault();
                DateTime date = Convert.ToDateTime(this.dgv_rota.Rows[i].Cells[0].Value);
                Rota old = list_rota.Where(o => o.Shift == Shift && o.TypeWork == TypeWork && Convert.ToDateTime(o.WorkTime).ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd")).FirstOrDefault();
                //判断当前任务是否安排人员
                if (old != null)
                {
                    old.Name = Name;
                    old.Content = Content;
                    old.Shift = Shift;
                    old.TypeWork = TypeWork;
                    old.WorkTime = date;
                    old.dataUpdPGM = "Rota";
                    old.dataUpdTime = DateTime.Now;
                    old.dataUpdUser = AppDrill.username;
                }
                else if (!string.IsNullOrEmpty(Name))
                {
                    Rota newRota = new Rota();
                    newRota.Name = Name;
                    newRota.Content = Content;
                    newRota.Shift = Shift;
                    newRota.TypeWork = TypeWork;
                    newRota.WorkTime = date;
                    newRota.dataMakePGM = "Rota";
                    newRota.dataMakeTime = DateTime.Now;
                    newRota.dataMakeUser = AppDrill.username;
                    newRota.dataUpdPGM = "Rota";
                    newRota.dataUpdTime = DateTime.Now;
                    newRota.dataUpdUser = AppDrill.username;
                    db.Rota.Add(newRota);
                }
                else
                {
                    continue;//结束当前循环
                }
            }
            db.SaveChanges();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SaveData();
            }
            catch
            {
                MessageBox.Show(message[3]);//保存失败
            }
            
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                isEdit = false;
                MessageBox.Show(message[2]);//保存成功
                backgroundWorker2.CancelAsync();
            }
            catch { }
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rbtn_workerManagement_Click(object sender, EventArgs e)
        {
            WorkerManagement wm = new WorkerManagement();
            wm.Show();//打开工人管理界面
        }
    }
}
