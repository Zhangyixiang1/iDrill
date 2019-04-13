using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class WITSForm : RadForm
    {
        private SerialPort comm = new SerialPort();
        private StringBuilder builder = new StringBuilder();//避免在事件处理方法中反复的创建，定义到外面。
        private long received_count = 0;//接收计数
        private long send_count = 0;//发送计数
        private string Tags;//需要发送的数据
        private List<string> list = new List<string>();
        private DrillOSEntities db;
        private List<DrillTag> listTag=new List<DrillTag>();
        private string url;
        public WITSForm()
        {
            InitializeComponent();
        }

        private void WITSForm_Load(object sender, EventArgs e)
        {
            url = System.Configuration.ConfigurationManager.AppSettings["WITSData"].ToString();//初始化系统语言
            setControlLanguage();
            //初始化下拉串口名称列表框
            string[] ports = SerialPort.GetPortNames();
            Array.Sort(ports);
            rdp_port.Items.AddRange(ports);
            rdp_port.SelectedIndex = rdp_port.Items.Count > 0 ? 0 : -1;
            string[] baudrate={"2400","4800","9600","19200","38400","43000","56000","57600","115200"};//设置波特率
            rdp_baudrate.Items.AddRange(baudrate);
            rdp_baudrate.SelectedIndex = rdp_baudrate.Items.IndexOf("9600");
            //初始化SerialPort对象
            comm.NewLine = "\r\n";
            comm.RtsEnable = true;//根据实际情况吧。
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();//开始后台执行数据
        }
        #region 异步处理
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                listTag = db.DrillTag.ToList();//获取测点信息
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
        #endregion
        public string WID;
        public string SKNO = "-9999.0000";
        public string RID = "-9999.0000";
        public string SQID = "-9999.0000";
        public string ACTC = "-9999.0000";
        public string DATE = DateTime.Now.ToString("yyMMdd");
        public string TIME = DateTime.Now.ToString("HHmmss");


        //以下实现发送广播消息
        public void SendBroadMsg(List<string> datas)
        {
            string DrillID=AppDrill.DrillID.ToString() ;
            WID = DrillID ;
            //定义第一个表数据
            string DBTM = string.Format("{0:N4}", Comm.UnitConversion(listTag,list[0],DrillID,double.Parse(datas[0])));

            string DBTV = "-9999.0000";
            string DMEA = string.Format("{0:N4}", Comm.UnitConversion(listTag, list[1], DrillID, double.Parse(datas[1])));
            string DVER = "-9999.0000";
            string BPOS = string.Format("{0:N4}", Comm.UnitConversion(listTag, list[2], DrillID, double.Parse(datas[2])));
            string ROPA = string.Format("{0:N4}", "-9999");
            string HKLA = string.Format("{0:N4}", "-9999");
            string HKLX = "-9999.0000";
            string WOBA = string.Format("{0:N4}", "-9999");
            string WOBX = "-9999.0000";
            string TQA = string.Format("{0:N4}", "-9999");
            string TQX = "-9999.0000";
            string RPMA = string.Format("{0:N4}", "-9999");
            string SPPA = string.Format("{0:N4}", "-9999");
            string CHKP = string.Format("{0:N4}", "-9999");
            string SPM1 = string.Format("{0:N4}", "-9999");
            string SPM2 = string.Format("{0:N4}", "-9999");
            string SPM3 = string.Format("{0:N4}", "-9999");
            string TVA = string.Format("{0:N4}", "-9999");
            string TVCA = "-9999.0000";
            string MFOP = "-9999.0000";
            string MFOA = string.Format("{0:N4}", "-9999");
            string MFIA = string.Format("{0:N4}", "-9999");
            string MDOA = "-9999.0000";
            string MDIA = "-9999.0000";
            string MTOA = "-9999.0000";
            string MTIA = "-9999.0000";
            string MCOA = "-9999.0000";
            string MCIA = "-9999.0000";
            string STKC = string.Format("{0:N4}", "-9999");
            //string LSTK = "-9999.0000";
            string DRTM = "-9999.0000";
            string GASA = "-9999.0000";

            //定义第二个表数据
            //    string BRVC = "-9999.0000";
            //string SCDT = "-9999.0000";
            //    string ECDT = "-9999.0000";
            //    string TVC = string.Format("{0:N4}", OPC.DB59DBD[272]);
            //string CPDI = "-9999.0000";

            //定义第11个表的数据
            string TVT = "-9999.0000";
            string TVCT = "-9999.0000";
            string TVRT = "-9999.0000";
            string TV01 = string.Format("{0:N4}", "-9999");
            string TV02 = string.Format("{0:N4}", "-9999");
            string TV03 = string.Format("{0:N4}", "-9999");
            string TV04 = string.Format("{0:N4}", "-9999");
            string TV05 = string.Format("{0:N4}", "-9999");
            string TV06 = string.Format("{0:N4}", "-9999");
            string TV07 = string.Format("{0:N4}", "-9999");
            string TV08 = string.Format("{0:N4}", "-9999");
            string TV09 = string.Format("{0:N4}", "-9999");
            string TV10 = string.Format("{0:N4}", "-9999");
            string TV11 = string.Format("{0:N4}", "-9999");
            string TV12 = string.Format("{0:N4}", "-9999");
            string TV13 = string.Format("{0:N4}", "-9999");
            string TV14 = string.Format("{0:N4}", "-9999");
            string TTV1 = "-9999.0000";
            string TTV2 = "-9999.0000";




            string strDataLineR1 = "&&\r\n" + "0101" + WID + "\r\n0102" + SKNO + "\r\n0103" + RID + "\r\n0104" + SQID + "\r\n0105" + DATE + "\r\n0106" + TIME + "\r\n0107" + ACTC + "\r\n0108" + DBTM + "\r\n0109" + DBTV + "\r\n0110" + DMEA + "\r\n0111" + DVER + "\r\n0112" + BPOS + "\r\n0113" + ROPA + "\r\n0114" + HKLA + "\r\n0115" + HKLX + "\r\n0116" + WOBA + "\r\n0117" + WOBX + "\r\n0118" + TQA + "\r\n0119" + TQX + "\r\n0120" + RPMA + "\r\n0121" + SPPA + "\r\n0122" + CHKP + "\r\n0123" + SPM1 + "\r\n0124" + SPM2 + "\r\n0125" + SPM3 + "\r\n0126" + TVA + "\r\n0127" + TVCA + "\r\n0128" + MFOP + "\r\n0129" + MFOA + "\r\n0130" + MFIA + "\r\n0131" + MDOA + "\r\n0132" + MDIA + "\r\n0133" + MTOA + "\r\n0134" + MTIA + "\r\n0135" + MCOA + "\r\n0136" + MCIA + "\r\n0137" + STKC + "\r\n0139" + DRTM + "\r\n0140" + GASA + "\r\n!!\r\n";

            //  string strDataLineR2 = "&&\r\n" + "0201" + WID + "\r\n0202" + SKNO + "\r\n0203" + RID + "\r\n0204" + SQID + "\r\n0205" + DATE + "\r\n0206" + TIME + "\r\n0207" + ACTC + "\r\n0208" + DMEA + "\r\n0209" + DVER + "\r\n0210" + ROPA + "\r\n0211" + WOBA + "\r\n0212" + HKLA + "\r\n0213" + SPPA + "\r\n0214" + TQA + "\r\n0215" + RPMA + "\r\n0217" + MDIA + "\r\n0218" + ECDT
            //    + "\r\n0219" + MFIA + "\r\n0220" + MFOA + "\r\n0221" + MFOP + "\r\n0222" + TVC + "\r\n!!\r\n";

            string strDataLineR3 = "&&\r\n" + "1101" + WID + "\r\n1102" + SKNO + "\r\n1103" + RID + "\r\n1104" + SQID + "\r\n1105" + DATE + "\r\n1106" + TIME + "\r\n1107" + ACTC + "\r\n1108" + DMEA + "\r\n1109" + DVER + "\r\n1110" + TVT + "\r\n1111" + TVA + "\r\n1113" + TVCA + "\r\n1115" + TV01 + "\r\n1116" + TV02 + "\r\n1117" + TV03 + "\r\n1118" + TV04
                + "\r\n1119" + TV05 + "\r\n1120" + TV06 + "\r\n1121" + TV07 + "\r\n1122" + TV08 + "\r\n1123" + TV09 + "\r\n1124" + TV10 + "\r\n1125" + TV11 + "\r\n1126" + TV12 + "\r\n1127" + TV13 + "\r\n1128" + TV14 + "\r\n1129" + TTV1 + "\r\n1130" + TTV2 + "\r\n!!";

            string Senddata = strDataLineR1 + strDataLineR3;

            /*IDictionaryEnumerator myEnumerator = _sessionTable.GetEnumerator();
            while (myEnumerator.MoveNext())
            {
                EndPoint tempend = (EndPoint)_sessionTable.Values;
                Client.SendTo(sendData, tempend);
            }
            */
            try
            {
                comm.WriteLine(Senddata);
            }
            catch 
            {
                lbl_status.ForeColor = Color.White;
                rbtn_send.Enabled = true;
            }
        }
        //定时器事件
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }

        private void rbtn_send_Click(object sender, EventArgs e)
        {
            rbtn_send.Enabled = false;
            lbl_status.ForeColor = Color.Lime;
            if (comm.IsOpen)
            {
                comm.Close();
            }
            else
            {
                comm.PortName = rdp_port.Text;
                comm.BaudRate = int.Parse(rdp_baudrate.Text);
                try
                {
                    comm.Open();
                }
                catch (Exception ex)
                {
                    //捕获到异常信息，创建一个新的comm对象，之前的不能用了。
                    comm = new SerialPort();
                    //现实异常信息给客户。
                    MessageBox.Show(ex.Message);
                }
            }
            timer1.Enabled = true;
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
                            if (xe.GetAttribute("key") == "WITSData")
                            {
                                XmlNodeList xn_list2 = xe.ChildNodes;
                                int flg = 0;
                                foreach (XmlNode node2 in xn_list2)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    if (flg == 0)
                                    {
                                        Tags += xe2.GetAttribute("value");
                                        flg = 1;
                                    }
                                    else
                                    {
                                        Tags +=","+xe2.GetAttribute("value");
                                    }
                                    list.Add(xe2.GetAttribute("key"));
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
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";

                //json数据赋值
                WITSData data = new WITSData();
                data.DrillId = AppDrill.DrillID;
                data.Tags = Tags;
                try
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(data);

                        streamWriter.Write(json);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        string r = result.Replace("\"", "");
                        string[] s = r.Split(',');
                        List<string> r_list = new List<string>();
                        foreach (string str in s)
                        {
                            r_list.Add(str);
                        }
                        SendBroadMsg(r_list);
                    }
                }
                catch { }

            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker2.CancelAsync();
        }

        private void WITSForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (comm.IsOpen)
            {
                comm.Close();
            }
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
