using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;
using ZedGraph;
using System.Web.Script.Serialization;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class DrillFormSec : BaseFormSec
    {
        private AlarmHistory AlarmHistory = null;
        private DrillOSEntities _db;
        private bool isRemind=false;//用于判断是否提示过与服务器断开连接
        delegate void UpdateChart();//更新曲线委托
        private Thread t_up, t_down, t_enla, t_narr;
        private List<Memo> Memo = new List<Memo>();
        private bool m_bisDepthTime = true;
        private double m_dLastTime = 0;

        private long m_lMax = 0;
        private long m_lMin = 0;

        public DrillFormSec()
        {
            InitializeComponent();
            initControl();
        }

        public void PlotByTime()
        {
            setDepthTime(true);
            depthChart.changeDepthtoTime();
            depthChart.InitViewHistory();
            InitBtn();
        }

        public void PlotByDepath()
        {
            setDepthTime(false);
            depthChart.changeTimetoDepth();
            depthChart.InitViewHistory();
            InitBtn();
        }


        private void setDepthTime(bool bDepthTime)
        {
            fourChart1.setDepthTime(bDepthTime);
            fourChart2.setDepthTime(bDepthTime);
            fourChart3.setDepthTime(bDepthTime);
            depthChart.setDepthTime(bDepthTime);
        }

        private void setDepthChart()
        {
            depthChart.setDepthChart(fourChart1, 1);
            depthChart.setDepthChart(fourChart2, 2);
            depthChart.setDepthChart(fourChart3, 3);
        }

        private void InitBtn()
        {
            radButton1.Enabled = true;
            radButton2.Enabled = true;
            radButton3.Text = "Pause";
            radButton4.Enabled = true;
            radButton5.Enabled = true;
        }


        /// <summary>
        /// 主界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            _db = new DrillOSEntities();

            #region 异步请求报警数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            
            //设置语言
            setControlLanguage();
            //状态判断
            if (!depthChart.ViewHistory)
            {
                radButton3.Text = "Pause";
            }
            else
            {
                radButton3.Text = "R/T";
            }
            //第一次初始化
            fourChart1.isActive = true;
            fourChart2.isActive = true;
            fourChart3.isActive = true;

            setDepthChart();
            setDepthTime(m_bisDepthTime);
            setBaseDepthTime(m_bisDepthTime);
            setFormFourChart(fourChart1, 1);
            setFormFourChart(fourChart2, 2);
            setFormFourChart(fourChart3, 3);
            setFormDepthTimeChart(depthChart);
            getData();
        }

        #region 异步请求报警数据
        /// <summary>
        /// 十秒请求报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            //TimeSpan t = DateTime.Now - depthChart.GetTime();
            double dTime = 0;
            if (0 != m_dLastTime)
            {
                dTime = depthChart.d_now - m_dLastTime;
            }

            m_dLastTime = depthChart.d_now;
            //modify #ll# 20186121641 修改服务器断开时间为5分钟
            if (dTime > 300 && !isRemind)
            {
                isRemind = true;
                MessageBox.Show(AppDrill.message[7]);
            }
            else if (dTime <= 30)
            {
                isRemind = false;
            }
            try
            {
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync(); //开始
                }
            }
            catch { }
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
       {
            try
            {
                AlarmHistory = _db.AlarmHistory.Where(o => o.RecoveryTime == null && o.DrillId == AppDrill.DrillID).FirstOrDefault();
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //设置报警状态
            if (AlarmHistory != null)
                Alarms.isAlarm = true;
            else
                Alarms.isAlarm = false;
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
            
        }
        #endregion

        #region 功能按钮

        //向上翻页查看历史
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                t_up = new Thread(UpClick);
                t_up.IsBackground = true;
                t_up.Start();
            }
            catch { }
        }
        private void UpClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(UpClick);
                Invoke(uc);
            }
            else
            {
                //depthChart.clearChart();
                //depthChart.UpClick();
                //depthChart.getMaxMin(ref m_lMax, ref m_lMin);
                //getData(m_lMin, m_lMax);

                fourChart1.UpClick();
                fourChart2.UpClick();
                fourChart3.UpClick();
                depthChart.UpClick();
                if (depthChart.isUp)
                {
                    radButton1.Enabled = false;
                }
                else
                {
                    radButton1.Enabled = true;
                }
                if (depthChart.isDown)
                {
                    radButton2.Enabled = false;
                }
                else
                {
                    radButton2.Enabled = true;
                }
                if (!depthChart.ViewHistory)
                {
                    radButton3.Text = "Pause";
                }
                else
                {
                    radButton3.Text = "R/T";
                }

            }
            t_up.Abort();
            t_up = null;
        }
        //向下翻页查看历史
        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                t_down = new Thread(DownClick);
                t_down.IsBackground = true;
                t_down.Start();
            }
            catch { }
        }
        private void DownClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(DownClick);
                Invoke(uc);
            }
            else
            {
                //depthChart.clearChart();
                //depthChart.DownClick();
                //depthChart.getMaxMin(ref m_lMax, ref m_lMin);
                //getData(m_lMin, m_lMax);

                fourChart1.DownClick();
                fourChart2.DownClick();
                fourChart3.DownClick();
                depthChart.DownClick();
                if (depthChart.isUp)
                {
                    radButton1.Enabled = false;
                }
                else
                {
                    radButton1.Enabled = true;
                }
                if (depthChart.isDown)
                {
                    radButton2.Enabled = false;
                }
                else
                {
                    radButton2.Enabled = true;
                }
                if (!depthChart.ViewHistory)
                {
                    radButton3.Text = "Pause";
                }
                else
                {
                    radButton3.Text = "R/T";
                }
                t_down.Abort();
                t_down = null;

                
            }
        }
        //查看实时
        private void radButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //depthChart.clearChart();
                //depthChart.RealTimeClick();
                //depthChart.getMaxMin(ref m_lMax, ref m_lMin);
                //long lLastTime = (long)depthChart.d_now;
                //m_lMin = lLastTime + m_lMin - m_lMax;
                //getData(m_lMin, lLastTime);

                //depthChart.setLastTime();

                fourChart1.RealTimeClick();
                fourChart2.RealTimeClick();
                fourChart3.RealTimeClick();
                depthChart.RealTimeClick();
                radButton1.Enabled = true;

                radButton2.Enabled = true;
                radButton4.Enabled = true;
                radButton5.Enabled = true;
                if (!depthChart.ViewHistory)
                {
                    radButton3.Text = "Pause";
                }
                else
                {
                    radButton3.Text = "R/T";
                }
            }
            catch { }
        }
        //放大
        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                t_enla = new Thread(EnlaClick);
                t_enla.IsBackground = true;
                t_enla.Start();
            }
            catch { }
        }
        private void EnlaClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(EnlaClick);
                Invoke(uc);
            }
            else
            {
                //depthChart.Enlarge();
                //depthChart.getMaxMin(ref m_lMax, ref m_lMin);
                //getData(m_lMin, m_lMax);

                fourChart1.timeNow = depthChart.d_now;
                fourChart2.timeNow = depthChart.d_now;
                fourChart3.timeNow = depthChart.d_now;
                fourChart1.Enlarge();
                fourChart2.Enlarge();
                fourChart3.Enlarge();
                depthChart.Enlarge();
                if (depthChart.isEnlarge)
                {
                    radButton4.Enabled = false;
                }
                else
                {
                    radButton4.Enabled = true;
                }
                if (depthChart.isNarrow)
                {
                    radButton5.Enabled = false;
                }
                else
                {
                    radButton5.Enabled = true;
                }
                if (!depthChart.ViewHistory)
                {
                    radButton3.Text = "Pause";
                }
                else
                {
                    radButton3.Text = "R/T";
                }
                t_enla.Abort();
                t_enla = null;
            }
        }
        //缩小
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                t_narr = new Thread(NarrClick);
                t_narr.IsBackground = true;
                t_narr.Start();
            }
            catch { }
        }
        private void NarrClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(NarrClick);
                Invoke(uc);
            }
            else
            {
                //depthChart.Narrow();
                //depthChart.getMaxMin(ref m_lMax, ref m_lMin);
                //getData(m_lMin, m_lMax);

                fourChart1.timeNow = depthChart.d_now;
                fourChart2.timeNow = depthChart.d_now;
                fourChart3.timeNow = depthChart.d_now;
                fourChart1.Narrow();
                fourChart2.Narrow();
                fourChart3.Narrow();
                depthChart.Narrow();
                if (depthChart.isEnlarge)
                {
                    radButton4.Enabled = false;
                }
                else
                {
                    radButton4.Enabled = true;
                }
                if (depthChart.isNarrow)
                {
                    radButton5.Enabled = false;
                }
                else
                {
                    radButton5.Enabled = true;
                }
                if (!depthChart.ViewHistory)
                {
                    radButton3.Text = "Pause";
                }
                else
                {
                    radButton3.Text = "R/T";
                }
                t_narr.Abort();
                t_narr = null;
            }
        }
        //打印截图
        private void radButton6_Click(object sender, EventArgs e)
        {
             try
            {
                Bitmap bit = new Bitmap(this.Width, this.Height);//实例化一个和窗体一样大的bitmap
                Graphics g = Graphics.FromImage(bit);
                g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                g.CopyFromScreen(this.Left, this.Top, 0, 0, new Size(this.Width, this.Height));//保存整个窗体为图片
                //g.CopyFromScreen(panel.PointToScreen(Point.Empty), Point.Empty, panel.Size);//只保存某个控件（这里是panel）
                //路径选择
                SaveFileDialog path = new SaveFileDialog();
                path.FileName = "Drilling_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                if (path.ShowDialog() == DialogResult.OK)
                    bit.Save(path.FileName);
            }
            catch { }
        }

        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
         //   System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        #region 用户控件参数定义
        private void initControl()
        {
            fourChart1.group = 1;
            fourChart1.fname = "Drilling";
            fourChart2.group = 2;
            fourChart2.fname = "Drilling";
            fourChart3.group = 3;
            fourChart3.fname = "Drilling";
            dataShowControl1.group = 4;
            dataShowControl1.fname = "Drilling";
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
                                continue;
                            }
                            if (xe.GetAttribute("key") == "depthTime")
                            {
                                depthChart.SetLabel(xe.GetAttribute("value"));
                                continue;
                            }
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    AppDrill.message.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            //取得各个命令
                            if (xe.GetAttribute("key") == "command")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    Command c = new Command();
                                    c.Group = int.Parse(xe2.GetAttribute("group"));
                                    c.Tag= xe2.GetAttribute("tag");
                                    c.TagName= xe2.GetAttribute("value");
                                    c.hand= int.Parse(xe2.GetAttribute("hand"));
                                    c.Unit = xe2.GetAttribute("unit");
                                    AppDrill.Command.Add(c);
                                }
                                continue;
                            }
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "rpnl_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu")
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
        //设置Memo
        public void setMemo(TextObj memo)
        {
            depthChart.myPane.GraphObjList.Add(memo);
        }
        //移除Memo
        public void RemoveMemo(double unix)
        {
            var t = depthChart.myPane.GraphObjList.Where(o =>o.Tag != null).ToList();
            var memo = depthChart.myPane.GraphObjList.Where(o => o.Location.Y == unix && o.Tag != null).FirstOrDefault();
            depthChart.myPane.GraphObjList.Remove(memo);
        }
        //重新设置Memo
        private void setTextMemo()
        {
            foreach (TextMemo tm in AppDrill.MessageList)
            {
                TextObj memo = new TextObj(tm.Text, 2, tm.unix); // 相差毫秒数);   //曲线内容与出现位置
                memo.FontSpec.Border.Color = Color.Red;            //Memo边框颜色
                memo.FontSpec.FontColor = Color.White;                     //Memo文本颜色
                memo.FontSpec.Size = 24f;                                  //Memo文本大小
                // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
                memo.Location.AlignH = AlignH.Left;
                memo.Location.AlignV = AlignV.Top;
                memo.FontSpec.Fill = new Fill(Color.Red, Color.Red, 35);
                memo.FontSpec.StringAlignment = StringAlignment.Near;
                setMemo(memo);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            ClearMemory();
        }
        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, 1000);
            }
        }
        #endregion
        /// <summary>
        /// 设置当前窗体是否激活曲线刷新
        /// </summary>
        /// <param name="isActivite"></param>
        public void setChartActivite(bool isActive)
        {
            fourChart1.isActive = isActive;
            fourChart2.isActive = isActive;
            fourChart3.isActive = isActive;
            depthChart.isActive = isActive;
        }
        /// <summary>
        /// 激活当前窗体曲线开始刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrillForm_Activated(object sender, EventArgs e)
        {
            setAllFormActivite();
            this.setChartActivite(true);
        }
        #region 日志函数
        /// <summary>
        /// 写日志
        /// </summary>
        /// <returns></returns>
        static private int WriteLog(string msg)
        {
            string path = Environment.CurrentDirectory;
            FileStream fs = new FileStream(path + "\\查询时间检测.txt", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write("=========================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "：" + msg + "=========================" + "\r\n");
            sw.Close();
            fs.Close();
            return 0;
        }
        #endregion

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (backgroundWorker2.IsBusy)
                {
                    return;
                }
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            //DateTime yd = DateTime.Now.AddDays(-1);
            //Memo = _db.Memo.Where(o => o.CreateTime >= yd).ToList();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           double d=Comm.ConvertDateTimeInt(DateTime.Now.AddDays(-1))/1000;
           foreach (var g in depthChart.myPane.GraphObjList.Where(o => o.Tag != null && o.Location.Y < d))
           {
               try
               {
                   depthChart.myPane.GraphObjList.Remove(g);//移除超过一天的
                   break;
               }
               catch { }
           }
            foreach (var m in Memo)
            {
                GraphObj t = depthChart.myPane.GraphObjList.Where(o => o.Tag != null && o.Location.Y == m.UnixTime).FirstOrDefault();
                if (t == null)
                {
                    TextObj memo = new TextObj(m.Text, 2, (double)m.UnixTime); // 相差毫秒数);   //曲线内容与出现位置
                    memo.Tag = 1;//用于鉴别是否是手动加入的memo
                    memo.FontSpec.Border.Color = Color.Red;            //Memo边框颜色
                    memo.FontSpec.FontColor = Color.White;                     //Memo文本颜色
                    memo.FontSpec.Size = 24f;                                  //Memo文本大小
                    // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
                    memo.Location.AlignH = AlignH.Left;
                    memo.Location.AlignV = AlignV.Top;
                    memo.FontSpec.Fill = new Fill(Color.Red, Color.Red, 35);
                    memo.FontSpec.StringAlignment = StringAlignment.Near;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm is DrillFormSec)
                        {
                            ((DrillFormSec)frm).setMemo(memo);
                            continue;
                        }
                        if (frm is CirculateForm)
                        {
                            ((CirculateForm)frm).setMemo(memo);
                            continue;
                        }
                        if (frm is DirectionalForm)
                        {
                            ((DirectionalForm)frm).setMemo(memo);
                            continue;
                        }
                        if (frm is DrillingGasForm)
                        {
                            ((DrillingGasForm)frm).setMemo(memo);
                            continue;
                        }
                        if (frm is DrillPVTForm)
                        {
                            ((DrillPVTForm)frm).setMemo(memo);
                            continue;
                        }
                    }
                }
            }
            backgroundWorker2.CancelAsync();
        }
    }
}
