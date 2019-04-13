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
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class SetCommand : RadForm
    {
        private List<Button> btnList = new List<Button>();
        private int readHand = 0;//读kepware的测点Hand
        private List<string> pageList = new List<string>();
        private List<string> message = new List<string>();//设置成功和设置失败2种情况
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;
        public SetCommand()
        {
            InitializeComponent();
        }

        private void SetCommand_Load(object sender, EventArgs e)
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
                setControlLanguage();
                setButtons();
                initPageName();
            }
            catch { }
        }

        #region 设置按钮相关属性
        private void setButtons()
        {
            List<Command> command1 = AppDrill.Command.Where(o => o.Group == 1).ToList();
            List<Command> command2 = AppDrill.Command.Where(o => o.Group == 2).ToList();
            List<Command> command3 = AppDrill.Command.Where(o => o.Group == 3).ToList();
            List<Command> command4 = AppDrill.Command.Where(o => o.Group == 4).ToList();
            //开始设置Button
            // 设置command1相关测点
            if (command1.Count > 0)
            {
                Button[] btns = new Button[command1.Count];  //声明对象
                for (int i = 0; i < command1.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 60 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 50);
                    btns[i].Text = command1[i].TagName;
                    btns[i].Tag = command1[i].hand;
                    if (command1[i].hand == 54)
                    {
                        btns[i].BackColor = Color.Gray;
                    }
                    else
                    {
                        btns[i].BackColor = Color.Black;
                    }
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    this.tb_command1.Controls.Add(btns[i]);
                    btnList.Add(btns[i]);
                }
            }
            // 设置command2相关测点
            if (command2.Count > 0)
            {
                Button[] btns = new Button[command2.Count];  //声明对象
                for (int i = 0; i < command2.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 60 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 50);
                    btns[i].Text = command2[i].TagName;
                    btns[i].Tag = command2[i].hand;
                    if (command2[i].hand == 37)
                    {
                        btns[i].BackColor = Color.Gray;
                    }
                    else
                    {
                        btns[i].BackColor = Color.Black;
                    }
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    this.tb_command2.Controls.Add(btns[i]);
                    btnList.Add(btns[i]);
                }
            }
            // 设置command3相关测点
            if (command3.Count > 0)
            {
                Button[] btns = new Button[command3.Count];  //声明对象
                for (int i = 0; i < command3.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 60 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 50);
                    btns[i].Text = command3[i].TagName;
                    btns[i].Tag = command3[i].hand;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    this.tb_command3.Controls.Add(btns[i]);
                    btnList.Add(btns[i]);
                }
            }
        }
        #endregion

        //按钮被点击
        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                Color c = b.BackColor;//保存点击按钮的颜色

                //取消所有的按钮选中状态
                for (int i = 0; i < btnList.Count; i++)
                {
                    if (btnList[i].BackColor == Color.Red)
                    {
                        if ((int)btnList[i].Tag == 37 || (int)btnList[i].Tag == 54)
                        {
                            btnList[i].BackColor = Color.Gray;
                        }
                        else
                        {
                            btnList[i].BackColor = Color.Black;
                        }
                        btnList[i].ForeColor = Color.White;
                    }
                }
                readHand = (int)b.Tag;
                b.BackColor = Color.Red;
                rlbl_message.Text = b.Text+":";
                //if (AppDrill.UnitFormat == "yz")
                //{
                //    rlbl_unit.Text = AppData.UnitTransfer.Find(o => o.UnitFrom == AppDrill.Command.Where(t => t.hand == (int)b.Tag).Select(m => m.Unit).FirstOrDefault()).UnitTo;
                //}
                //else
                //{
                //    rlbl_unit.Text = AppDrill.Command.Where(t => t.hand == (int)b.Tag).Select(m => m.Unit).FirstOrDefault();
                //}
                //目前以公制为准
                rlbl_unit.Text = AppDrill.Command.Where(t => t.hand == (int)b.Tag).Select(m => m.Unit).FirstOrDefault();
                lbl_message.Text = "";
                rtxt_value.Text = "";//读取kepware之前先清空值
                switch (readHand)
                {
                    case 37:
                        rlbl_message.Visible = false;
                        rtxt_value.Visible = false;
                        rlbl_unit.Visible = false;
                        btn_Set.Visible = false;
                        backgroundWorker3.WorkerSupportsCancellation = true;
                        backgroundWorker3.RunWorkerAsync();
                        break;
                    case 54:
                        rlbl_message.Visible = false;
                        rtxt_value.Visible = false;
                        rlbl_unit.Visible = false;
                        btn_Set.Visible = false;
                        backgroundWorker3.WorkerSupportsCancellation = true;
                        backgroundWorker3.RunWorkerAsync();
                        break;
                    default:
                        rlbl_message.Visible = true;
                        rtxt_value.Visible = true;
                        rlbl_unit.Visible = true;
                        btn_Set.Visible = true;
                        try
                        {
                            sendMsg(readHand.ToString());
                            try
                            {
                                string result = ReciveMsg();
                                rtxt_value.Text = result;//将读取的值设置到文本框中
                            }
                            catch {
                                lbl_message.Text = "TimeOut";
                                lbl_message.ForeColor = Color.Red;
                            }
                        }
                        catch {
                            lbl_message.Text = "TimeOut";
                            lbl_message.ForeColor = Color.Red;
                        }
                        break;
                }
            }
            catch { }
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            lbl_message.Text = "";
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {           
            try
            {
                int hand = 0;
                foreach (Button b in btnList)
                {
                    if (b.BackColor == Color.Red)
                    {
                        hand = (int)b.Tag;
                        break;
                    }
                }
                string value = rtxt_value.Text;
                sendMsg(hand + "," + value);
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (ReciveMsg() != "error")
                {
                    lbl_message.ForeColor = Color.Lime;
                    lbl_message.Text = message[0];
                }
                else
                {
                    lbl_message.ForeColor = Color.Red;
                    lbl_message.Text = message[1];
                }
                backgroundWorker1.CancelAsync();
            }
            catch
            {
                lbl_message.ForeColor = Color.Red;
                lbl_message.Text = message[3];
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
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == this.Name)//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //取得各个命令
                            if (xe.GetAttribute("key") == "pageList")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    pageList.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    message.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
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
        /// 设置Page的Text属性
        /// </summary>
        private void initPageName()
        {
            this.tb_command1.Text = pageList[0];
            this.tb_command2.Text = pageList[1];
            this.tb_command3.Text = pageList[2];
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int hand = 0;
                foreach (Button b in btnList)
                {
                    if (b.BackColor == Color.Red)
                    {
                        hand = (int)b.Tag;
                        break;
                    }
                }
                string value = rtxt_value.Text;
                sendMsg(hand + "," + 1);
                Thread.Sleep(100);
                sendMsg(hand + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() != "error")
                {
                    lbl_message.ForeColor = Color.Lime;
                    lbl_message.Text = message[0];
                }
                else
                {
                    lbl_message.ForeColor = Color.Red;
                    lbl_message.Text = message[1];
                }
                backgroundWorker3.CancelAsync();
            }
            catch 
            {
                lbl_message.ForeColor = Color.Red;
                lbl_message.Text = message[3];
            }
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
