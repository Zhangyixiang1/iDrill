using Apache.NMS;
using Apache.NMS.ActiveMQ;
using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;
using ZedGraph;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class DirectionalForm : BaseForm
    {
        private GraphicsState transState;//用于保存画板的初始状态
        private DrillOSEntities db;//数据库读取对象
        private IMessageConsumer consumer1;
        private IMessageConsumer consumer2;
        private IMessageConsumer consumer3;
        private IMessageConsumer consumer4;
        private IMessageConsumer consumer5;
        private IMessageConsumer consumer6;
        private IMessageConsumer consumer7;
        //用于装取表格中的数据
        private List<double> list_dep = new List<double>();
        private List<double> list_inc = new List<double>();
        private List<double> list_azm = new List<double>();
        private List<double> list_mtf = new List<double>();
        private List<double> list_gtf = new List<double>();
        private List<double> list_dls = new List<double>();
        private List<double> list_toolface = new List<double>();//绑定工具面
        private List<TagDictionary> list_tagdir=new List<TagDictionary>();//字典表数据
        //private List<DrillTag> listTag = new List<DrillTag>();//测点数据
        private const int GridCount = 100;//定义行数
        // 读写kepware等参数IP和端口
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;
        private bool m_bIsDownClick = true;

        private long m_lMax = 0;
        private long m_lMin = 0;

        private Thread m_tGetdata = null;//声明4个功能按钮线程
        delegate void UpdateChart();//更新曲线委托
        private Dictionary<string, List<double>> m_dicToolfaceTag = new Dictionary<string, List<double>>();
        public DirectionalForm()
        {
            InitializeComponent();
            initControl();
        }

        private void setViewHistory(bool bIsHistory)
        {
            depthTimeChart.ViewHistory = bIsHistory;
            fourChart1.setViewHistory(bIsHistory);
        }

        private void UpdateFourChart()
        {
            if (null != fourChart1)
            {
                fourChart1.UpdateFourChart();
            }
        }

        public void resetTags()
        {
            setViewHistory(true);
            resetDataShow();

            if (null != dataShowControl1)
            {
                dataShowControl1.resetDataShow();
            }

            if (null != fourChart1)
            {
                fourChart1.resetDataShow();
            }


            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            UpdateFourChart();
            setViewHistory(false);
        }

        public void setTagValues(string strValue)
        {
            if (null != dataShowControl1)
            {
                dataShowControl1.setTagValue(strValue);
            }

            if (null != fourChart1)
            {
                fourChart1.setTagValue(strValue);
            }

            setTagValue(strValue);
        }

        public void setDrillID(int iDrillID)
        {
            fourChart1.setDrillID(iDrillID);
            depthTimeChart.setDrillID(iDrillID);
            dataShowControl1.drillID = iDrillID;
        }

        private void setDepthChart()
        {
            depthTimeChart.setDepthChart(fourChart1, 1);
        }

        private void InitBtn()
        {
            btn_Up.Enabled = true;
            btn_Down.Enabled = true;
            btn_Real.Text = "Pause";
            btn_Enlarge.Enabled = true;
            btn_Narrow.Enabled = true;
        }

        private void CtrlDrillForm_Load(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    ClientIP = System.Configuration.ConfigurationManager.AppSettings["ClientIP"].ToString();//获取本机IP地址
                    ServerIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"].ToString();//获取服务器地址
                    ServerPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServerPort"].ToString());//服务器端口
                    ClientPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClientPort"].ToString());//客户端端口
                    if (AppDrill.client == null)
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        client.Bind(new IPEndPoint(IPAddress.Parse(ClientIP), ClientPort));
                        client.ReceiveTimeout = 2000;
                        client.SendTimeout = 2000;
                        AppDrill.client = client;
                    }
                    else
                    {
                        client = AppDrill.client;
                    }
                }
                catch { }
                if (AppDrill.permissionId != 1&&AppDrill.permissionId!=2)
                {
                 //   rbtn_set.Enabled = false;
                }
                db = new DrillOSEntities();
                //设置语言
                setControlLanguage();
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.RunWorkerAsync();
                setBtnRP();
                setDepthChart();
                setFormFourChart(fourChart1, 1);
                setFormDepthTimeChart(depthTimeChart);
                getData();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                addOldLable(m_lMin, m_lMax);
                m_delegateDataShow = showDataMessage;
                createConsumer();
            }
            catch 
            { 
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
                listTag = db.DrillTag.Where(o => o.DrillId == 1).ToList();//取出所有测点数据
                if (AppData.factory == null)
                    AppData.factory = new ConnectionFactory(System.Configuration.ConfigurationManager.AppSettings["DataSourceAddress"].ToString());
                //开始连接
                if (AppData.connection == null)
                    AppData.connection = AppData.factory.CreateConnection(System.Configuration.ConfigurationManager.AppSettings["DataSourceID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DataSourcePassWord"].ToString());
                AppData.connection.Start();
                if (AppData.session == null)
                    AppData.session = AppData.connection.CreateSession();
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                initGridView();
            }
            catch { }
            backgroundWorker1.CancelAsync();
        }
        #region 功能按钮
        private void InitPicLoad(bool bIsVisible)
        {
            fourChart1.pic_load.Visible = bIsVisible;
        }

        public void setBtnRP()
        {
            if (!depthTimeChart.ViewHistory)
            {
                btn_Real.Text = "Pause";
            }
            else
            {
                btn_Real.Text = "R/T";
            }
        }

        public void queryHisData()
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(query_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            }
        }

        private void setBtnTF()
        {
            int iIndex = depthTimeChart.getTimeIndex();
            if (1 == iIndex)
            {
                btn_Enlarge.Enabled = false;
                btn_Narrow.Enabled = true;
            }
            else if (10 == iIndex)
            {
                btn_Narrow.Enabled = false;
                btn_Enlarge.Enabled = true;
            }
            else
            {
                btn_Enlarge.Enabled = true;
                btn_Narrow.Enabled = true;
            }
        }

        private void query_process()
        {
            depthTimeChart.queryHis();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.setFourTimeStep();
            query();
        }

        private void query()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(query);
                Invoke(uc);
            }
            else
            {
                setBtnTF();
                fourChart1.setTimeMaxmin(m_lMin, m_lMax);
                fourChart1.queryHis();
                InitPicLoad(false);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }

            m_tGetdata.Abort();
            m_tGetdata = null;
        }
        //向上翻页查看历史
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Up_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Up_process()
        {
            depthTimeChart.UpClick();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            UpClick();
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
                fourChart1.UpClick();
                InitPicLoad(false);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Up_process2);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            }
        }

        private void Up_process2()
        {
            depthTimeChart.UpClick2();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            UpClick2();
        }

        private void UpClick2()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(UpClick2);
                Invoke(uc);
            }
            else
            {
                fourChart1.UpClick();
                InitPicLoad(false);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        public void realTime()
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                if (depthTimeChart.ViewHistory)
                {
                    InitPicLoad(true);
                    setBtnTF();
                }

                m_tGetdata = new Thread(Real_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            }
        }

        private void Real_process()
        {
            if (depthTimeChart.ViewHistory)
            {
                fourChart1.timeNow = depthTimeChart.d_now;
                depthTimeChart.RealTimeClick();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            RealClick();
        }

        private void RealClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(RealClick);
                Invoke(uc);
            }
            else
            {
                if (depthTimeChart.ViewHistory)
                {
                    InitPicLoad(false);
                    fourChart1.RealTimeClick();

                    btn_Up.Enabled = true;
                    btn_Down.Enabled = true;
                    btn_PageUp.Enabled = true;
                    btn_PageDown.Enabled = true;
                    depthTimeChart.ViewHistory = false;
                    showLable(m_lMin, m_lMax);
                }
                else
                {
                    depthTimeChart.ViewHistory = true;
                    fourChart1.setVeiwHistroy(true);
                }

                if (!depthTimeChart.ViewHistory)
                {
                    depthTimeChart.setReal(true);
                    btn_Real.Text = "Pause";
                }
                else
                {
                    depthTimeChart.setReal(false);
                    btn_Real.Text = "R/T";
                }

                m_tGetdata.Abort();
                m_tGetdata = null;
            }
        }
  
        //查看实时
        private void radButton3_Click(object sender, EventArgs e)
        {
            realTime();
        }

        //向下翻页查看历史
        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Down_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Down_process()
        {
            m_bIsDownClick = depthTimeChart.IsAllowedDownClick();

            if (m_bIsDownClick)
            {
                depthTimeChart.DownClick();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            DownClick();
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
                if (!m_bIsDownClick)
                {
                    InitPicLoad(false);
                    btn_Down.Enabled = false;
                    m_tGetdata.Abort();
                    m_tGetdata = null;
                    return;
                }

                fourChart1.DownClick();
                InitPicLoad(false);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Down_process2);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch { }
        }
        private void Down_process2()
        {
            m_bIsDownClick = depthTimeChart.IsAllowedDownClick();

            if (m_bIsDownClick)
            {
                depthTimeChart.DownClick2();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            DownClick2();
        }

        private void DownClick2()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(DownClick2);
                Invoke(uc);
            }
            else
            {
                if (!m_bIsDownClick)
                {
                    InitPicLoad(false);
                    btn_PageDown.Enabled = false;
                    m_tGetdata.Abort();
                    m_tGetdata = null;
                    return;
                }

                fourChart1.DownClick();
                InitPicLoad(false);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }

            m_tGetdata.Abort();
            m_tGetdata = null;
        }


        //放大
        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                if (depthTimeChart.getReal())
                {
                    depthTimeChart.setVeiwHistroy(true);
                    depthTimeChart.Enlarge();
                    setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                    setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                    m_tGetdata = new Thread(Real_process);
                }
                else
                {
                    m_tGetdata = new Thread(Enla_process);
                }
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Enla_process()
        {
            fourChart1.timeNow = depthTimeChart.d_now;
            depthTimeChart.Enlarge();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            EnlaClick();
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
                fourChart1.Enlarge();
                InitPicLoad(false);
                setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }
        //缩小
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                if (depthTimeChart.getReal())
                {
                    depthTimeChart.setVeiwHistroy(true);
                    depthTimeChart.Narrow();
                    setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                    setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                    m_tGetdata = new Thread(Real_process);
                }
                else
                {
                    m_tGetdata = new Thread(Narr_process);
                }
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Narr_process()
        {
            fourChart1.timeNow = depthTimeChart.d_now;

            depthTimeChart.Narrow();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            NarrClick();
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
                fourChart1.Narrow();
                InitPicLoad(false);
                setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }

            m_tGetdata.Abort();
            m_tGetdata = null;
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
                path.FileName = "Directional_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                if (path.ShowDialog() == DialogResult.OK)
                    bit.Save(path.FileName);
            }
            catch { }
            
        }

        #endregion

        #region 用户控件参数定义

        private void initControl()
        {
            fourChart1.group = 1;
            fourChart1.fname = "Directional";

            dataShowControl1.group = 4;
            dataShowControl1.fname = "Directional";
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
                            if (xe.GetAttribute("key") == "depthTime")
                            {
                                depthTimeChart.SetLabel(xe.GetAttribute("value"), 1);
                                depthTimeChart.SetLabel(xe.GetAttribute("chart"), 2);
                                depthTimeChart.SetLabel(xe.GetAttribute("message"), 3); ;
                                continue;
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "rpnl_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu"||c.Name=="rcv_toolface")
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

        private void radChartView1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                #region 基础绘制
                int x = 45;
                Graphics g = e.Graphics;//创建画板,这里的画板是由Form提供的.            
                Pen p_green = new Pen(Color.FromArgb(0, 255, 0), 30);//定义了一个绿色,宽度为15的画笔
                g.DrawEllipse(p_green, (int)(x * 1.5), (int)(x * 1.5), rcv_toolface.Width - 3 * x, rcv_toolface.Width - 3 * x);//在画板上画圆,起始坐标为(x*1.5,x*1.5),外接矩形的宽,高为 rcv_toolface.Width - 3 * x
                Pen p_blue = new Pen(Color.Blue, 30);//定义了一个蓝色,宽度为15的画笔
                g.DrawEllipse(p_blue, x * 2, x * 2, rcv_toolface.Width - 4 * x, rcv_toolface.Width - 4 * x);//在画板上画圆,起始坐标为(x*2,x*2),外接矩形的宽,高为 rcv_toolface.Width - 4 * x
                Pen p_red = new Pen(Color.Red, 30);//定义了一个红色,宽度为15的画笔
                g.DrawEllipse(p_red, (int)(x * 2.5), (int)(x * 2.5), rcv_toolface.Width - 5 * x, rcv_toolface.Width - 5 * x);//在画板上画圆,起始坐标为(x*2.5,x*2.5),外接矩形的宽,高为 rcv_toolface.Width - 5 * x
                Pen p_pb = new Pen(Color.FromArgb(67, 255, 255), 30);//定义了一个天空蓝色,宽度为的画笔
                g.DrawEllipse(p_pb, x * 3, x * 3, rcv_toolface.Width - 6 * x, rcv_toolface.Width - 6 * x);//在画板上画圆,起始坐标为(x*3,x*3),外接矩形的宽,高为 rcv_toolface.Width - 6 * x
                Pen p_orange = new Pen(Color.FromArgb(205, 133, 0), 30);//定义了一个棕色,宽度为的画笔
                g.DrawEllipse(p_orange, (int)(x * 3.5), (int)(x * 3.5), rcv_toolface.Width - 7 * x, rcv_toolface.Width - 7 * x);//在画板上画圆,起始坐标为(x*3.5,x*3.5),外接矩形的宽,高为 rcv_toolface.Width - 7 * x
                Pen p_orange2 = new Pen(Color.Orange, 30);//定义了一个橙色,宽度为的画笔
                g.DrawEllipse(p_orange2, x * 4, x * 4, rcv_toolface.Width - 8 * x, rcv_toolface.Width - 8 * x);//在画板上画圆,起始坐标为(x*4,x*4),外接矩形的宽,高为 rcv_toolface.Width - 8 * x
                Pen p_violet = new Pen(Color.FromArgb(153, 50, 204), 30);//定义了一个粉色,宽度为的画笔
                g.DrawEllipse(p_violet, (int)(x * 4.5), (int)(x * 4.5), rcv_toolface.Width - 9 * x, rcv_toolface.Width - 9 * x);//在画板上画圆,起始坐标为(x*4.5,x*4.5),外接矩形的宽,高为 rcv_toolface.Width - 9 * x
                Graphics g_partline = e.Graphics;//创建画板，专门用于画标线
                g_partline.TranslateTransform(270, 270);//将当前位置设置为坐标原点           
                for (int i = 0; i < 360 / 15; i++)
                {
                    g_partline.RotateTransform(15);//每隔15度画一条线
                    g_partline.DrawLine(new Pen(Color.FromArgb(45, 45, 48), 2), 0, 0, 0, -270);
                }
                transState = e.Graphics.Save();//保存当前的状态，以便画箭头时候可以保存当前状态
                g_partline.ResetTransform();//重置当前位置
                #endregion

                Graphics g_line = e.Graphics;//创建画板，专门用于画线
                g_line.Restore(transState);
                Pen p_green_line = new Pen(Color.Yellow, 14);//定义了一个蓝色,宽度为的画笔
                p_green_line.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;//恢复实线
                p_green_line.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;//定义线尾的样式为箭头
                int j = 0;
                for (int i = list_toolface.Count - 1; i >= (list_toolface.Count > 7 ? list_toolface.Count - 7 : 0); i--)
                {
                    g_line.RotateTransform((float)list_toolface[i]);
                    g_line.DrawLine(p_green_line, 0, -200 + j * 23, 0, -216 + 22 * j);
                    g_line.RotateTransform(-(float)list_toolface[i]);
                    j++;
                }
            }
            catch { }
        }
       
        #region 初始化GridView
        private void initGridView()
        {
            //基本属性
            this.rgv_toolface.BackColor = Color.FromArgb(45, 45, 48);//设置背景颜色
            this.rgv_toolface.ShowGroupPanel = false;//不显示GroupPanel
            this.rgv_toolface.MasterTemplate.ShowRowHeaderColumn = false;//不显示左侧第一列
            this.rgv_toolface.MasterTemplate.AllowAddNewRow = false;//禁止添加新行
            this.rgv_toolface.ReadOnly = true;//只读
            //列设定初期化
            this.rgv_toolface.DataSource = null;
            this.rgv_toolface.TableElement.BeginUpdate();
            this.rgv_toolface.MasterTemplate.Columns.Clear();
            //列设定
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var2"), "rgv_svydepth"));
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var104"), "rgv_inc"));
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var103"), "rgv_azm"));
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var105"), "rgv_mag"));
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var108"), "rgv_gra"));
            this.rgv_toolface.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(Transformation("var102"), "rgv_ga"));
            //列宽度设定
            
            this.rgv_toolface.MasterTemplate.Columns[0].Width = 85;//设置列宽度
            this.rgv_toolface.MasterTemplate.Columns[0].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[0].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[1].Width = 65;
            this.rgv_toolface.MasterTemplate.Columns[1].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[1].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[2].Width = 65;
            this.rgv_toolface.MasterTemplate.Columns[2].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[2].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[3].Width = 65;
            this.rgv_toolface.MasterTemplate.Columns[3].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[3].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[4].Width = 65;
            this.rgv_toolface.MasterTemplate.Columns[4].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[4].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[5].Width = 65;
            this.rgv_toolface.MasterTemplate.Columns[5].WrapText = true;
            this.rgv_toolface.MasterTemplate.Columns[5].AllowSort = false;
            this.rgv_toolface.MasterTemplate.Columns[0].TextAlignment = ContentAlignment.MiddleCenter;//设置列内容居中
            this.rgv_toolface.MasterTemplate.Columns[1].TextAlignment = ContentAlignment.MiddleCenter;
            this.rgv_toolface.MasterTemplate.Columns[2].TextAlignment = ContentAlignment.MiddleCenter;
            this.rgv_toolface.MasterTemplate.Columns[3].TextAlignment = ContentAlignment.MiddleCenter;
            this.rgv_toolface.MasterTemplate.Columns[4].TextAlignment = ContentAlignment.MiddleCenter;
            this.rgv_toolface.MasterTemplate.Columns[5].TextAlignment = ContentAlignment.MiddleCenter;
            for (int i = 0; i < GridCount; i++)
            {
                rgv_toolface.Rows.AddNew();//循环添加N条新行
            }

            try
            {
                //if (AppData.session != null)
                //{
                //    consumer1 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var2" + "_" + AppDrill.DrillID));
                //    consumer2 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var104" + "_" + AppDrill.DrillID));
                //    consumer3 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var103" + "_" + AppDrill.DrillID));
                //    consumer4 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var105" + "_" + AppDrill.DrillID));
                //    consumer5 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var108" + "_" + AppDrill.DrillID));
                //    consumer6 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var102" + "_" + AppDrill.DrillID));
                //    consumer7 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("var109" + "_" + AppDrill.DrillID));
                //}
                //consumer1.Listener += new MessageListener(consumer1_Listener);
                //consumer2.Listener += new MessageListener(consumer2_Listener);
                //consumer3.Listener += new MessageListener(consumer3_Listener);
                //consumer4.Listener += new MessageListener(consumer4_Listener);
                //consumer5.Listener += new MessageListener(consumer5_Listener);
                //consumer6.Listener += new MessageListener(consumer6_Listener);
                //consumer7.Listener += new MessageListener(consumer7_Listener); 
            }
            catch 
            { 
            }
            
        }
        #endregion

        #region 消息中间件 ADD by ZAY in 2017.5.24

        private delegate void ShowNoteMsgDelegate_New(Dictionary<string, string> map);

        private void consumer1_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage1), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void consumer2_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage2), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void consumer3_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage3), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void consumer4_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage4), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void consumer5_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage5), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void consumer6_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage6), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }
        private void consumer7_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage7), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }

        private void ShowMessage1(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_dep.Add(value);
            int j = 0;
            for (int i = list_dep.Count - 1; i >= (list_dep.Count > GridCount ? list_dep.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[0].Value = list_dep[i];
                j++;
            }
        }

        private void ShowMessage2(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_inc.Add(value);
            int j = 0;
            for (int i = list_inc.Count - 1; i >= (list_inc.Count > GridCount ? list_inc.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[1].Value = list_inc[i];
                j++;
            }
        }

        private void ShowMessage3(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_azm.Add(value);
            int j = 0;
            for (int i = list_azm.Count - 1; i >= (list_azm.Count > GridCount ? list_azm.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[2].Value = list_azm[i];
                j++;
            }
        }

        private void ShowMessage4(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_mtf.Add(value);
            rcv_toolface.Refresh();//刷新
            int j = 0;
            for (int i = list_mtf.Count - 1; i >= (list_mtf.Count > GridCount ? list_mtf.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[3].Value = list_mtf[i];
                j++;
            }
        }

        private void ShowMessage5(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_gtf.Add(value);
            int j = 0;
            for (int i = list_gtf.Count - 1; i >= (list_gtf.Count > GridCount ? list_gtf.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[4].Value = list_gtf[i];
                j++;
            }
        }

        private void ShowMessage6(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            list_dls.Add(value);
            int j = 0;
            for (int i = list_dls.Count - 1; i >= (list_dls.Count > GridCount ? list_dls.Count - GridCount : 0); i--)
            {
                this.rgv_toolface.Rows[j].Cells[5].Value = list_dls[i];
                j++;
            }
        }
        private void ShowMessage7(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = UnitConversion(map["Tag"], map["DrillId"], value);
            }
            if (list_toolface.Count==0||value != list_toolface[list_toolface.Count - 1])
            {
                list_toolface.Add(value);
            }
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
            catch 
            { 
                return str; 
            }
            
        }
        #endregion

        #region 公英制换算 ADD by ZAY in 2017.7.10
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tag">测点</param>
        /// <param name="DrillId">井 ID</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private double UnitConversion(string Tag, string DrillId, double value)
        {
            try
            {
                if (AppDrill.UnitFormat == "yz")
                {
                    var Dirrl = 0;
                    if (!string.IsNullOrEmpty(DrillId))
                    {
                        Dirrl = int.Parse(DrillId);
                    }

                    var TagModel = listTag.Find(o => o.DrillId == Dirrl && o.Tag == Tag);
                    if (TagModel != null)
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == TagModel.Unit);
                        if (UnitModel != null)
                        {
                            value = value * (double)UnitModel.Coefficient;
                            return value;
                        }
                    }
                }
                else if (AppDrill.UnitFormat == "gz")
                {
                    return value;
                }
            }
            catch
            {
                return value;
            }
            return value;
        }
        #endregion
        //设置Memo
        public void setMemo(TextObj memo)
        {
            depthTimeChart.myPane.GraphObjList.Add(memo);
        }
        //移除Memo
        public void RemoveMemo(double unix)
        {
            var t = depthTimeChart.myPane.GraphObjList.Where(o => o.Tag != null).ToList();
            var memo = depthTimeChart.myPane.GraphObjList.Where(o => o.Location.Y == unix && o.Tag != null).FirstOrDefault();
            depthTimeChart.myPane.GraphObjList.Remove(memo);
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
             //   sendMsg(14 + "," + txt_tlf.Text);
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //rbtn_set.Enabled = true;
            try
            {
                if (ReciveMsg() == "error")
                {
                    MessageBox.Show("Set Failed !");
                }

                backgroundWorker2.CancelAsync();
            }
            catch { }
        }

        private void rbtn_set_Click(object sender, EventArgs e)
        {
            try
            {
                //rbtn_set.Enabled = false;
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }
        /// <summary>
        /// 向特定ip的主机的端口发送数据报
        /// </summary>
        static void sendMsg(string msg)
        {
            EndPoint point = new IPEndPoint(IPAddress.Parse(ServerIP), ServerPort);
            client.SendTo(Encoding.UTF8.GetBytes(msg), point);
        }
        /// <summary>
        /// 接收发送给本机ip对应端口号的数据报
        /// </summary>
        private string ReciveMsg()
        {
            try
            {
                EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                byte[] buffer = new byte[1024];
                int length = client.ReceiveFrom(buffer, ref point);//接收数据报
                string message = Encoding.UTF8.GetString(buffer, 0, length);//数据处理结果0或1
                return message;
            }
            catch 
            { 
                return "error"; 
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
            {
                return;
            }

            base.WndProc(ref m);
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
        /// <summary>
        /// 设置当前窗体是否激活曲线刷新
        /// </summary>
        /// <param name="isActivite"></param>
        public void setChartActivite(bool isActive)
        {
            fourChart1.isActive = isActive;
            depthTimeChart.isActive = isActive;
        }
        /// <summary>
        /// 激活当前窗体曲线开始刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectionalForm_Activated(object sender, EventArgs e)
        {
            setAllFormActivite();
            this.setChartActivite(true);
        }

        private void showDataMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            if (null != dataShowControl1)
            {
                dataShowControl1.ShowMessage(realTimeData);
            }

            ShowToolFaceMessage(realTimeData);
        }

        //显示工具面列表测点的值
        private void ShowToolFaceMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            foreach (var item in m_dicToolfaceTag)
            {

                string strDrillId = realTimeData.BoxId.ToString();
                string strTag = item.Key;
                List<double> tagList = item.Value;

                var data = realTimeData.realTags.Where(o => o.Tag == strTag).FirstOrDefault();
                if (null != data && null != tagList)
                {
                    double newValue = UnitConversion(strTag, strDrillId, data.Value);
                    tagList.Add(newValue);
                    int iCount = tagList.Count;
                    int iRowIndex = 0;

                    for (int i = iCount - 1; i >= (iCount > GridCount ? iCount - GridCount : 0); --i)
                    {
                        this.rgv_toolface.Rows[iRowIndex].Cells[0].Value = tagList[i];
                        ++iRowIndex;
                    }

                    if ("var109" != strTag)
                    {
                        continue;
                    }

                    if (0 == iCount || newValue != tagList[iCount - 1])
                    {
                        tagList.Add(newValue);
                    }
                }
            }
        }

        //初始化工具面要接收的测点
        private void InitToolfaceTag()
        {
            string strTag = System.Configuration.ConfigurationManager.AppSettings["ToolFaceTag"].ToString();
            string[] strAryTag = strTag.Split(',');
            if (null != strAryTag)
            {
                for (int i = 0; i < strAryTag.Length; ++i)
                {
                    addToolFaceTag(strAryTag[i], i);
                }
            }
        }

        //添加工具面测点
        private void addToolFaceTag(string strTag, int iIndex)
        {
            switch (iIndex)
            {
                case 0:
                    m_dicToolfaceTag.Add(strTag, list_dep);
                    break;
                case 1:
                    m_dicToolfaceTag.Add(strTag, list_inc);
                    break;
                case 2:
                    m_dicToolfaceTag.Add(strTag, list_azm);
                    break;
                case 3:
                    m_dicToolfaceTag.Add(strTag, list_mtf);
                    break;
                case 4:
                    m_dicToolfaceTag.Add(strTag, list_gtf);
                    break;
                case 5:
                    m_dicToolfaceTag.Add(strTag, list_dls);
                    break;
                case 6:
                    m_dicToolfaceTag.Add(strTag, list_toolface);
                    break;
                default:
                    break;
            }
        }
    }
}
