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
    public partial class ShowAlarm : RadForm
    {
        private DrillOSEntities db;
        private List<AlarmData> data;
        private List<TagDictionary> list_tagdir;
        public bool alerm = true;
        public int DrillId = 0;
        private List<string> list_columns = new List<string>();
        public ShowAlarm()
        {
            InitializeComponent();
            db = new DrillOSEntities();
            data = new List<AlarmData>();
            list_tagdir = new List<TagDictionary>();    //定义测点字典表
        }

        private void ShowAlarm_Load(object sender, EventArgs e)
        {
            try
            {
                setControlLanguage();
                //页面加载，设置报警数据信息
                //setView();
                #region 异步加载数据
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
                backgroundWorker1.RunWorkerAsync(); //开始
                #endregion

                #region 异步提交数据
                backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
                backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker2_RunWorkerCompleted);
                backgroundWorker2.WorkerSupportsCancellation = true;    //声明是否支持取消线程
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
                //按时间降序获取前50条未处置报警的信息
                var listAlarm = (from a in db.AlarmHistory.Where(o => o.RecoveryTime == null && o.DrillId == AppDrill.DrillID).ToList()
                                join b in Alarms.list_Alarms
                                on a.Tag equals b.Tag
                                select new
                                {
                                    a.Tag,
                                    a.Timestamp,
                                    a.Message
                                });
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();    //取出Tag字典表中的数据
                foreach (var alarm in listAlarm.OrderByDescending(o => o.Timestamp).Take(50))
                {
                    data.Add(new AlarmData(Transformation(alarm.Tag), alarm.Message, Convert.ToDateTime(alarm.Timestamp).ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.radGridView1.DataSource = data;
            //this.radGridView1.MasterTemplate.Columns[0].Width = 179;
            
            this.radGridView1.EnableHeadersVisualStyles = false;
            this.radGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            this.radGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12);
            this.radGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < data.Count; i++)
            {
                this.radGridView1.Rows.Add(1);//一次添加一行
                this.radGridView1.Rows[i].Height = 30;//设置行高
                this.radGridView1.Rows[i].Cells[0].Value = data[i].Channel;
                this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.White;
                this.radGridView1.Rows[i].Cells[1].Value = data[i].Message;
                this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.White;
                this.radGridView1.Rows[i].Cells[2].Value = data[i].Date;
                this.radGridView1.Rows[i].Cells[2].Style.ForeColor = Color.White;
            }
            this.radGridView1.ClearSelection();
                rbtn_alarmConfirm.Enabled = true;
            rbtn_Cancel.Enabled = true;
            timer1.Enabled = true;
            timer2.Enabled = true;
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
            {
                rbtn_alarmConfirm.Enabled = false;
            }
            setColor();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 异步提交数据
        //添加DoWork事件请求数据
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            var AlarmHistory = db.AlarmHistory.Where(o => o.RecoveryTime == null && o.DrillId == AppDrill.DrillID).ToList();
            //修改报警状态
            foreach (AlarmHistory alarm in AlarmHistory)//listAlarm
            {
                alarm.Dispose = 1;
                alarm.DisposeMan = AppDrill.realName;
                alarm.DisposeTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                alarm.RecoveryTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            db.SaveChanges();
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_alarmConfirm.Enabled = true;
            rbtn_Cancel.Enabled = true;
            alerm = false;//设置报警状态取消
            backgroundWorker2.CancelAsync();    //取消挂起的后台操作。
            this.Close();
        }
        #endregion

        /// <summary>
        /// 取得报警数据
        /// </summary>
        public void setView()
        {
            this.radGridView1.BackColor = Color.FromArgb(45, 45, 48);//设置背景颜色
            this.radGridView1.DataSource = null;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            foreach (var am in Alarms.list_Alarms)
            {
                am.status = 2;
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

        #region 多语言转换
        /// <summary>
        /// 转换Tag
        /// </summary>
        /// <param name="str">需要转换的Tag</param>
        /// <returns>转换后的结果</returns>
        private string Transformation(string str)
        {
            try
            {
                TagDictionary t = list_tagdir.Where(o => o.Basic == str).FirstOrDefault();
                //判断当前测点在字典表里面是否可以查询到
                if (t != null)
                {
                    return t.TagShowName;//查询到返回转换结果
                }
                else
                {
                    return str;//查询不到直接返回
                }
            }
            catch { return str; }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            setColor();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                
                backgroundWorker3.WorkerSupportsCancellation = true;
                backgroundWorker3.RunWorkerAsync();
            }
            catch { }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //按时间降序获取前50条未处置报警的信息
                var listAlarm = from a in db.AlarmHistory.Where(o => o.RecoveryTime == null && o.DrillId == AppDrill.DrillID).ToList()
                                join b in Alarms.list_Alarms
                                on a.Tag equals b.Tag
                                select new
                                {
                                    a.Tag,
                                    a.Timestamp,
                                    a.Message
                                };
                data.Clear();
                foreach (var alarm in listAlarm.OrderByDescending(o => o.Timestamp).Take(50))
                {
                    data.Add(new AlarmData(Transformation(alarm.Tag), alarm.Message, Convert.ToDateTime(alarm.Timestamp).ToString("yyyy-MM-dd HH:mm:ss")));
                }
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //setView();
            //this.radGridView1.DataSource = data;
            //setColor();
            this.radGridView1.Rows.Clear();//清楚所有行
            this.radGridView1.EnableHeadersVisualStyles = false;
            this.radGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
            this.radGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12);
            this.radGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < data.Count; i++)
            {
                this.radGridView1.Rows.Add(1);
                this.radGridView1.Rows[i].Height = 30;
                this.radGridView1.Rows[i].Cells[0].Value = data[i].Channel;
                this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.White;
                this.radGridView1.Rows[i].Cells[1].Value = data[i].Message;
                this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.White;
                this.radGridView1.Rows[i].Cells[2].Value = data[i].Date;
                this.radGridView1.Rows[i].Cells[2].Style.ForeColor = Color.White;
            }
            this.radGridView1.ClearSelection();//清除选中状态
            setColor();
            backgroundWorker3.CancelAsync();
        }
        //设置颜色
        private void setColor()
        {
            for (int i = 0; i < radGridView1.RowCount; i++)
            {
                var tag = list_tagdir.Find(o => o.TagShowName == this.radGridView1.Rows[i].Cells[0].Value.ToString());
                if (tag == null)
                    continue;
                AlarmModel am = Alarms.list_Alarms.Find(o => o.Tag == tag.Basic);
                if (am != null)
                {
                    this.radGridView1.ClearSelection();//清除选中行的状态
                    if (am.status == 1)
                    {
                        if (this.radGridView1.Rows[i].Cells[0].Style.ForeColor == Color.Red)
                        {
                            this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.White;
                            this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.White;
                            this.radGridView1.Rows[i].Cells[2].Style.ForeColor = Color.White;
                        }
                        else
                        {
                            this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                            this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            this.radGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                        }
                    }
                    else if (am.status == 3)
                    {
                        if (this.radGridView1.Rows[i].Cells[0].Style.ForeColor != Color.Red)
                        {
                            this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                            this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            this.radGridView1.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                        }
                    }
                }
            }
        }
    }
}