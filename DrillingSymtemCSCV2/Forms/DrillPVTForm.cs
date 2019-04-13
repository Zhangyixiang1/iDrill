using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using DrillingSymtemCSCV2.UserControls;
using System.Xml;
using System.Drawing.Drawing2D;
using DrillingSymtemCSCV2.Model;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using ZedGraph;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class DrillPVTForm : BaseForm
    {
        private DrillOSEntities db;//数据库读取对象
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//字典表数据
        //读写kepwareIP和端口设置
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;
        private bool m_bisDepthTime = true;

        private Thread t_up, t_down, t_enla, t_narr;//声明4个功能按钮线程
        delegate void UpdateChart();//更新曲线委托
        public DrillPVTForm()
        {
            InitializeComponent();
        }

        private void setDepthChart()
        {
            depthTimeChart1.setDepthChart(fourChart2, 2);
        }

        private void InitBtn()
        {
            btn_up.Enabled = true;
            btn_down.Enabled = true;
            btn_zhong.Text = "Pause";
            btn_add.Enabled = true;
            btn_jian.Enabled = true;
        }

        private void DrillPVTForm_Load(object sender, EventArgs e)
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
                if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
                {
                    rbtn_zts.Enabled = false;
                    rbtn_ztt.Enabled = false;
                }
                db = new DrillOSEntities();
                //读取xml文件设置语言
                setControlLanguage();
                //用户控件参数定义
                initControl();
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.RunWorkerAsync();               
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }

                setDepthChart();
                setFormFourChart(fourChart2, 2);
                setFormDepthTimeChart(depthTimeChart1);
                //getData();

                rbtn_zts.Text = AppDrill.Command.Where(o => o.hand == 15).Select(o => o.TagName).FirstOrDefault();
                rbtn_ztt.Text = AppDrill.Command.Where(o => o.hand == 16).Select(o => o.TagName).FirstOrDefault();
            }
            catch { }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
                listTag = db.DrillTag.Where(o => o.DrillId == AppDrill.DrillID).ToList();//取出所有测点数据
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
                //初始化20个chart
                initChart();
                backgroundWorker1.CancelAsync();
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
                                depthTimeChart1.SetLabel(xe.GetAttribute("value"), 1);
                                depthTimeChart1.SetLabel(xe.GetAttribute("chart"), 2);
                                depthTimeChart1.SetLabel(xe.GetAttribute("message"), 3);
                                continue;
                            }
                            //string s = xe.GetAttribute("key");
                            //string s2 = xe.GetAttribute("value");
                            //循环每个控件，设置当前语言应设置的值
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

        #region 用户控件参数定义
        private void initControl()
        {
            //fourChart1.group = 1;
            //fourChart1.fname = "DrillPVT";
            //fourChart2.group = 2;
            //fourChart1.fname = "DrillPVT";
            dataShowControl1.fname = "DrillPVT";
            dataShowControl1.group = 3;
            dataShowControl2.fname = "DrillPVT";
            dataShowControl2.group = 4;
        }
        #endregion

        #region 按钮事件

        //向上翻页查看历史
        private void btn_up_Click(object sender, EventArgs e)
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
                fourChart2.UpClick();
                depthTimeChart1.UpClick();
                if (depthTimeChart1.isUp)
                {
                    btn_up.Enabled = false;
                }
                else
                {
                    btn_up.Enabled = true;
                }
                if (depthTimeChart1.isDown)
                {
                    btn_down.Enabled = false;
                }
                else
                {
                    btn_down.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_up.Abort();
            t_up = null;
        }

        //向下翻页查看历史
        private void btn_down_Click(object sender, EventArgs e)
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
                fourChart2.DownClick();
                depthTimeChart1.DownClick();
                if (depthTimeChart1.isUp)
                {
                    btn_up.Enabled = false;
                }
                else
                {
                    btn_up.Enabled = true;
                }
                if (depthTimeChart1.isDown)
                {
                    btn_down.Enabled = false;
                }
                else
                {
                    btn_down.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }

            }
            t_down.Abort();
            t_down = null;
        }
        //向上翻页查看历史
        private void btn_zhong_Click(object sender, EventArgs e)
        {
            try
            {
                fourChart2.RealTimeClick();
                depthTimeChart1.RealTimeClick();
                btn_up.Enabled = true;
                btn_down.Enabled = true;
                btn_jian.Enabled = true;
                btn_add.Enabled = true;
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            catch { }
        }

        //放大
        private void btn_add_Click(object sender, EventArgs e)
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
                fourChart2.timeNow = depthTimeChart1.d_now;
                fourChart2.Enlarge();
                depthTimeChart1.Enlarge();
                if (depthTimeChart1.isEnlarge)
                {
                    btn_add.Enabled = false;
                }
                else
                {
                    btn_add.Enabled = true;
                }
                if (depthTimeChart1.isNarrow)
                {
                    btn_jian.Enabled = false;
                }
                else
                {
                    btn_jian.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_enla.Abort();
            t_enla = null;
        }
        //缩小
        private void btn_jian_Click(object sender, EventArgs e)
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
                fourChart2.timeNow = depthTimeChart1.d_now;
                fourChart2.Narrow();
                depthTimeChart1.Narrow();
                if (depthTimeChart1.isEnlarge)
                {
                    btn_add.Enabled = false;
                }
                else
                {
                    btn_add.Enabled = true;
                }
                if (depthTimeChart1.isNarrow)
                {
                    btn_jian.Enabled = false;
                }
                else
                {
                    btn_jian.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_narr.Abort();
            t_narr = null;
        }
        //
        private void btn_p_Click(object sender, EventArgs e)
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
                path.FileName = "DrillPVT_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                if (path.ShowDialog() == DialogResult.OK)
                    bit.Save(path.FileName);
            }
            catch { }
        }
        #endregion

        private void DrillPVTForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        
        //配置20个固定测点
        private void initChart()
        {
            chart1.setParameter("var32", "var76", listTag, 17, Transformation("var32"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart2.setParameter("var33", "var77", listTag, 18, Transformation("var33"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart3.setParameter("var34", "var78", listTag, 19, Transformation("var34"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart4.setParameter("var35", "var79", listTag, 20, Transformation("var35"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart5.setParameter("var36", "var80", listTag, 21, Transformation("var36"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart6.setParameter("var37", "var81", listTag, 22, Transformation("var37"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart7.setParameter("var38", "var82", listTag, 23, Transformation("var38"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart8.setParameter("var39", "var83", listTag, 24, Transformation("var39"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart9.setParameter("var40", "var84", listTag, 25, Transformation("var40"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart10.setParameter("var41", "var85", listTag, 26, Transformation("var41"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart11.setParameter("var42", "var86", listTag, 27, Transformation("var45"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart12.setParameter("var43", "var87", listTag, 28, Transformation("var43"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart13.setParameter("var44", "var88", listTag, 29, Transformation("var44"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart14.setParameter("var45", "var89", listTag, 30, Transformation("var45"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart15.setParameter("var46", "var90", listTag, 31, Transformation("var46"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart16.setParameter("var47", "var91", listTag, 32, Transformation("var47"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart17.setParameter("var48", "var92", listTag, 33, Transformation("var48"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart18.setParameter("var49", "var93", listTag, 34, Transformation("var49"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart19.setParameter("var50", "var94", listTag, 35, Transformation("var50"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
            chart20.setParameter("var51", "var95", listTag, 36, Transformation("var51"), 1000, 90, 80, 20, 10, Color.Blue, Color.Red, Color.Orange);
        }

        #region 多语言转换
        /// <summary>
        /// 转换Tag
        /// </summary>
        /// <param name="str">需要转换的Tag</param>
        /// <returns>转换后的结果</returns>
        private string Transformation(string str)
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
        #endregion

        //设置Memo
        public void setMemo(TextObj memo)
        {
            depthTimeChart1.myPane.GraphObjList.Add(memo);
        }
        //移除Memo
        public void RemoveMemo(double unix)
        {
            var t = depthTimeChart1.myPane.GraphObjList.Where(o => o.Tag != null).ToList();
            var memo = depthTimeChart1.myPane.GraphObjList.Where(o => o.Location.Y == unix && o.Tag != null).FirstOrDefault();
            depthTimeChart1.myPane.GraphObjList.Remove(memo);
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(15 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(15 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_zts.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                    MessageBox.Show("Set Failed !");
                backgroundWorker2.CancelAsync();
            }
            catch { }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(16 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(16 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_ztt.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                    MessageBox.Show("Set Failed !");
                backgroundWorker3.CancelAsync();
            }
            catch
            {

            }
        }

        private void rbtn_zts_Click(object sender, EventArgs e)
        {
            rbtn_zts.Enabled = false;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void rbtn_ztt_Click(object sender, EventArgs e)
        {
            rbtn_ztt.Enabled = false ;
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.RunWorkerAsync();
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
            catch { return "error"; }
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
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
            fourChart2.isActive = isActive;
            depthTimeChart1.isActive = isActive;
        }
        /// <summary>
        /// 激活当前窗体曲线开始刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrillPVTForm_Activated(object sender, EventArgs e)
        {
            setAllFormActivite();
            this.setChartActivite(true);
        }
    }
}
