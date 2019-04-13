using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class PitStatus : RadForm
    {
        public int ClientHandle { get; set; }//写入kepware的位置
        public string pitName { get; set; }//当前罐名称
        public int pitStatus { get; set; }//当前罐状态值
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;
        public PitStatus()
        {
            InitializeComponent();
        }

        private void PitStatus_Load(object sender, EventArgs e)
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
                setControlLanguage();
                rbtn_enable.Enabled = pitStatus >= 1 ? false : true;//当前状态是启用则不可点击启用按钮
                rbtn_disable.Enabled = pitStatus >= 1 ? true : false;//与上相反
                rlbl_pitName.Text = pitName;
                rlbl_pitValue.Text = pitStatus >= 1 ? "Activated" : "Deactivated";
            }
            catch { }
        }
        private void rbtn_enable_Click(object sender, EventArgs e)
        {
            try
            {
                rbtn_enable.Enabled = false;
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.RunWorkerAsync();
            }
            catch { }
        }

        private void rbtn_disable_Click(object sender, EventArgs e)
        {
            try
            {
                rbtn_disable.Enabled = false;
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(ClientHandle+","+ 1);//当前状态是禁用，发1启用
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_enable.Enabled = true;
            this.Close();
            try
            {
                ReciveMsg();
            }
            catch { }
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(ClientHandle+","+ 0);//当前状态启用，发0禁用
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_disable.Enabled = true;
            this.Close();
            try
            {
                ReciveMsg();
            }
            catch { }
            backgroundWorker2.CancelAsync();
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
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
            byte[] buffer = new byte[1024];
            int length = client.ReceiveFrom(buffer, ref point);//接收数据报
            string message = Encoding.UTF8.GetString(buffer, 0, length);//数据处理结果0或1
            return message;
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
