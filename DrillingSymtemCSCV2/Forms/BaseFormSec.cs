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
using Telerik.WinControls.UI;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Collections;
using System.Web.Script.Serialization;
using System.Xml;
using ZedGraph;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
namespace DrillingSymtemCSCV2.Forms
{
    public partial class BaseFormSec : RadForm
    {
        //private AlarmHistory AlarmHistory = null;
        private DrillOSEntities _db;
        //private string ActivityTag = "var17";
        //private IMessageConsumer consumer0;
        private IMessageConsumer consumer1;
        private IMessageConsumer consumer2;
        private IMessageConsumer consumer3;
        private IMessageConsumer consumer4;
        private IMessageConsumer consumer5;
        public static List<DrillTag> listTag;
        private List<UserTag> UserTag;
        private List<Drill> listDrill = new List<Drill>();
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        private ActivityStatus ActivityQuery = new ActivityStatus();
        List<String> TransList = new List<string>();

        private UserControls.FourChart m_fourChart1 = null;
        private UserControls.FourChart m_fourChart2 = null;
        private UserControls.FourChart m_fourChart3 = null;
        private UserControls.DepthTimeChart m_depthChart = null;

        private List<string> m_tags = new List<string>();
        private string m_strTag1 = null;
        private string m_strTag2 = null;
        private string m_strTag3 = null;
        private long m_lTimestap = 0;
        public static Dictionary<String, int> m_wellNoMap = new Dictionary<String, int>();

        private Drill drill;
        //自己语言的字典
        //M1_翻译
        //M1，翻译
        private Dictionary<string, string> LanuangeDictionary = new Dictionary<string, string>();
        //IP端口设置相关属性
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;
        public BaseFormSec()
        {
            _db = new DrillOSEntities();
            if (listTag == null)
                listTag = new List<DrillTag>();
            UserTag = new List<UserTag>();
            InitializeComponent();
            drill = new Drill();
        }

        private void addWellNoMap(string strKey, int iValue)
        {
            if (null != m_wellNoMap)
            {
                m_wellNoMap[strKey] = iValue;
            }
        }

        public void setBaseFormSize(int iHight, int iWidth)
        {
            this.Size = new System.Drawing.Size(iWidth, iHight);
        }

        public void setFormDepthTimeChart(UserControls.DepthTimeChart depthChart)
        {
            m_depthChart = depthChart;
        }

        public void setFormFourChart(UserControls.FourChart fourChart, int iIndex)
        {
            switch (iIndex)
            {
                case 1:
                    m_fourChart1 = fourChart;
                    break;
                case 2:
                    m_fourChart2 = fourChart;
                    break;
                case 3:
                    m_fourChart3 = fourChart;
                    break;
                default:
                    break;
            }
        }

        private void getTags(List<UserTag> UserTag, UserControls.FourChart fourChart, ref string strTag)
        {
            if (null == fourChart)
            {
                return;
            }

            for (int iIndex = 1; iIndex <= 4; iIndex++)
            {
                var TagModel = UserTag.Where(o => o.Form == fourChart.fname && o.Group == fourChart.group && o.Order == iIndex).FirstOrDefault();
                if (TagModel != null)
                {
                    m_tags.Add(TagModel.Tag);
                    strTag += TagModel.Tag + ",";
                }
                else
                {
                    strTag += "0,";
                }
            }
        }

        private void setFourChart(UserControls.FourChart fourChart, List<HistoryData> historyData, ref string strTags)
        {
            if (null == strTags || "" == strTags)
            {
                return;
            }

            string strTag = strTags.Substring(0, strTags.Length - 1);

            if (null == strTag || "" == strTag)
            {
                return;
            }

            string[] str = strTag.Split(',');

            if (null != str)
            {
                for (int iIndex = 0; iIndex < str.Length; iIndex++)
                {
                    var item = historyData.Where(o => o.Tag == str[iIndex]).FirstOrDefault();

                    if (null != fourChart && null != item)
                    {
                        fourChart.setFourChart(item.Datas, iIndex + 1);
                    }
                }
            }
        }

        private void setFourChart(List<HistoryData> historyData)
        {
            setFourChart(m_fourChart1, historyData, ref m_strTag1);
            setFourChart(m_fourChart2, historyData, ref m_strTag2);
            setFourChart(m_fourChart3, historyData, ref m_strTag3);
        }

        private void setTimeMaxMin(double dMin, double dMax)
        {
            if (null != m_fourChart1)
            {
                m_fourChart1.setTimeMaxMin(dMin, dMax);
            }

            if (null != m_fourChart2)
            {
                m_fourChart2.setTimeMaxMin(dMin, dMax);
            }

            if (null != m_fourChart3)
            {
                m_fourChart3.setTimeMaxMin(dMin, dMax);
            }
        }

        private void getHistoryData()
        {
            m_lTimestap = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;
            string PostData = null;
            string PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();
            QueryHistory model = new QueryHistory();

            model.startTime = m_lTimestap - 1800;
            model.endTime = m_lTimestap;
            model.DrillId = AppDrill.DrillID;
            model.DepthTag = m_depthChart.getDepthTag();

            List<string> tag = new List<string>();

            foreach (var item in m_tags)
            {
                tag.Add(item);
            }

            model.Tag = tag;

            PostData = new JavaScriptSerializer().Serialize(model);

            var QueryData = Comm.HttpPost(PostUrl, PostData);

            if (!string.IsNullOrEmpty(QueryData))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                jsSerializer.MaxJsonLength = Int32.MaxValue;
                getHistoryData dataList = jsSerializer.Deserialize<getHistoryData>(QueryData); //反序列化
                List<HistoryDepthData> Depthdatas = dataList.Depthdatas;

                m_depthChart.addHistoryData(Depthdatas);
                setFourChart(dataList.datas);
                setTimeMaxMin(m_lTimestap - 1800, m_lTimestap);
                //loadFourChartData();
            }
        }

        private void loadFourChartData()
        {
            if (null != m_fourChart1)
            {
                m_fourChart1.InvalidateChart();
            }

            if (null != m_fourChart2)
            {
                m_fourChart2.InvalidateChart();
            }

            if (null != m_fourChart3)
            {
                m_fourChart3.InvalidateChart();
            }
        }

        public void getData()
        {
            try
            {
                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == AppDrill.DrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                getTags(UserTag, m_fourChart1, ref m_strTag1);
                getTags(UserTag, m_fourChart2, ref m_strTag2);
                getTags(UserTag, m_fourChart3, ref m_strTag3);

                getHistoryData();
            }
            catch
            {
            }
        }

        public void getData(long lTimestapBegin, long lTimestapEnd)
        {
            try
            {
                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == AppDrill.DrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                getTags(UserTag, m_fourChart1, ref m_strTag1);
                getTags(UserTag, m_fourChart2, ref m_strTag2);
                getTags(UserTag, m_fourChart3, ref m_strTag3);

                getHistoryData(lTimestapBegin, lTimestapEnd);
            }
            catch
            {
            }
        }

        private void getHistoryData(long lTimestapBegin, long lTimestapEnd)
        {
            string PostData = null;
            string PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();
            QueryHistory model = new QueryHistory();

            model.startTime = lTimestapBegin;
            model.endTime = lTimestapEnd;
            model.DrillId = AppDrill.DrillID;
            model.DepthTag = m_depthChart.getDepthTag();

            List<string> tag = new List<string>();

            foreach (var item in m_tags)
            {
                tag.Add(item);
            }

            model.Tag = tag;

            PostData = new JavaScriptSerializer().Serialize(model);

            var QueryData = Comm.HttpPost(PostUrl, PostData);

            if (!string.IsNullOrEmpty(QueryData))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                jsSerializer.MaxJsonLength = Int32.MaxValue;
                getHistoryData dataList = jsSerializer.Deserialize<getHistoryData>(QueryData); //反序列化
                List<HistoryDepthData> Depthdatas = dataList.Depthdatas;
                m_depthChart.clearAll();
                m_depthChart.addHistoryData(Depthdatas);
                setFourChart(dataList.datas);
            }
        }



        public void setBaseDepthTime(bool bDepthTime)
        {
            if (!bDepthTime)
            {
                btn_pltTime.Enabled = true;
                btn_pltDepath.Enabled = false;
            }
            else
            {
                btn_pltTime.Enabled = false;
                btn_pltDepath.Enabled = true;
            }
        }

        /// <summary>
        /// 主界面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (AppData.factory == null)
                    AppData.factory = new ConnectionFactory(System.Configuration.ConfigurationManager.AppSettings["DataSourceAddress"].ToString());
                //开始
                //消息中间件
                if (AppData.connection == null)
                    AppData.connection = AppData.factory.CreateConnection(System.Configuration.ConfigurationManager.AppSettings["DataSourceID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DataSourcePassWord"].ToString());
                AppData.connection.Start();
                if (AppData.session == null)
                    AppData.session = AppData.connection.CreateSession();
            }
            catch { }
            try
            {
                ClientIP = System.Configuration.ConfigurationManager.AppSettings["ClientIP"].ToString();//获取本机IP地址
                ServerIP = System.Configuration.ConfigurationManager.AppSettings["ServerIP"].ToString();//获取服务器地址
                ServerPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ServerPort"].ToString());//服务器端口
                ClientPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ClientPort"].ToString());//客户端端口
                try
                {
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
                //获取字典翻译
                this.TransList = getTranslateList();

                List<String> TranslateList = new List<string>();
                switch (this.Name)
                {
                    case "DrillForm":
                        rbtn_Drill.BackColor = Color.SkyBlue;
                        rbtn_return.Visible = true;
                        rbtn_exit.Visible = true;
                        break;
                    case "CirculateForm":
                        rbtn_Circulate.BackColor = Color.SkyBlue;
                        break;
                    case "DrillingGasForm":
                        rbtn_Gas.BackColor = Color.SkyBlue;
                        break;
                    case "DirectionalForm":
                        rbtn_Directional.BackColor = Color.SkyBlue;
                        break;
                    case "DrillPVTForm":
                        rbtn_DrillPVT.BackColor = Color.SkyBlue;
                        break;
                    case "ToolManageForm":
                        rbtn_Tool.BackColor = Color.SkyBlue;
                        break;
                    case "AlarmForm":
                        rbtn_Alarm.BackColor = Color.SkyBlue;
                        break;
                    case "HistoryDataForm":
                        rbtn_HistoryData.BackColor = Color.SkyBlue;
                        break;
                }
                #region 设置界面是否启用
                try
                {
                    if (!AppDrill.FormSet.Contains("DrillingGasForm"))
                    {
                        rbtn_Gas.Enabled = false;//按钮禁用
                        rbtn_Gas.BackColor = Color.Gray;
                    }

                    if (!AppDrill.FormSet.Contains("DirectionalForm"))
                    {
                        rbtn_Directional.Enabled = false;//按钮禁用
                        rbtn_Directional.BackColor = Color.Gray;
                    }

                    if (!AppDrill.FormSet.Contains("ToolManageForm"))
                    {
                        rbtn_Tool.Enabled = false;//按钮禁用
                        rbtn_Tool.BackColor = Color.Gray;
                    }

                    if (!AppDrill.FormSet.Contains("RotaForm"))
                    {
                        rbtn_Rota.Enabled = false;//按钮禁用
                        rbtn_Rota.BackColor = Color.Gray;
                    }

                    //add by #ll# 20185211104
                    if (!AppDrill.FormSet.Contains("CirculateForm"))
                    {
                        rbtn_Circulate.Enabled = false;//按钮禁用
                        rbtn_Circulate.BackColor = Color.Gray;
                    }
                }
                catch { }
                #endregion
                this.radLabeltime.Text = DateTime.Now.ToString();
                lbl_OperatorValue.Text = AppDrill.username;//设置用户名
                radLabelElement1.Text = "Hello " + AppDrill.username;
                lbl_Auto.Text = AppDrill.slip_list[0];
                lbl_Out.Text = AppDrill.slip_list[2];
                lbl_SlipStatus.Text = AppDrill.slip_list[4];
                #region 查询报警
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
                backgroundWorker1.RunWorkerAsync(); //开始
                #endregion
                SetDataShowClick();
                InitPermission();
            }
            catch { }
        }

        /// <summary>
        /// Menu Pannel绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panlMenu_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                                this.pnl_Menu.ClientRectangle,
                                Color.SkyBlue,//7f9db9
                                1,
                                ButtonBorderStyle.Solid,
                                Color.SkyBlue,
                                1,
                                ButtonBorderStyle.Solid,
                                Color.SkyBlue,
                                1,
                                ButtonBorderStyle.Solid,
                                Color.SkyBlue,
                                1,
                                ButtonBorderStyle.Solid);
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AppDrill.DrillID < 0)
                    AppDrill.DrillID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DrillID"].ToString());

                if (AppDrill.UnitFormat == "-1")
                    AppDrill.UnitFormat = System.Configuration.ConfigurationManager.AppSettings["System"].ToString();

                if (AppData.UnitTransfer == null)
                {
                    AppData.UnitTransfer = new List<UnitTransfer>();
                    AppData.UnitTransfer = _db.UnitTransfer.ToList();
                }

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == AppDrill.DrillID && o.Username == AppDrill.username).FirstOrDefault();
                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                }

                //取出tag数据
                if (listTag.Count == 0)
                    listTag = _db.DrillTag.Where(o => o.DrillId == AppDrill.DrillID).ToList();

                listDrill = _db.Drill.ToList();
                list_tagdir = _db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据

                using (var dbContext = new DrillOSEntities())
                {
                    ActivityQuery = dbContext.ActivityStatus.Where(o => o.IsSelect == true && o.DrillID == AppDrill.DrillID).FirstOrDefault();
                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void setWellNoValue()
        {
            var DrillModel = listDrill.Where(o => o.ID == AppDrill.DrillID).FirstOrDefault();

            if (DrillModel != null)
            {
                lbl_WellNoValue.Text = DrillModel.DrillNo;
                AppDrill.DrillNo = DrillModel.DrillNo;
            }
        }

        private void setWellNoValue(string strWellNo)
        {
            lbl_WellNoValue.Text = strWellNo;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //设置报警状态
                //if (AlarmHistory != null)
                //    AlarmStatus.isAlarm = true;
                setDataShow2();

                //setWellNoValue();

                //var DrillModel = listDrill.Where(o => o.ID == AppDrill.DrillID).FirstOrDefault();
                //if (DrillModel != null)
                //{
                //    lbl_WellNoValue.Text = DrillModel.DrillNo;
                //    AppDrill.DrillNo = DrillModel.DrillNo;
                //}

                var DrillModel = listDrill.Where(o => o.ID == AppDrill.DrillID).FirstOrDefault();
                drill = _db.Drill.Where(O => O.ID == AppDrill.DrillID).FirstOrDefault();
                if (DrillModel != null)
                {
                    lbl_WellNoValue.Text = DrillModel.DrillNo;
                    AppDrill.DrillNo = DrillModel.DrillNo;
                }

                //0611修改，更改抬头
                if (drill != null) {
                    lbl_WellNoValue.Text = drill.DrillNo;
                    lbl_leasevalue.Text = drill.Lease;
                    lbl_rignumvalue.Text = drill.RigNo;
                    lbl_contractorvalue.Text = drill.Contractor;
                    lbl_OperatorValue.Text = drill.Operator;
                }

                //#region 注册"Activity"实时数据监听
                //try
                //{
                //    if (AppData.session != null)
                //        consumer0 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(ActivityTag + "_" + AppDrill.DrillID));
                //    consumer0.Listener += new MessageListener(consumer0_Listener);
                //}
                //catch { }
                //#endregion
                if (ActivityQuery == null)
                {
                    //lb_Activity.Text = GetDefaultText();
                }
                else
                {
                    AppDrill.ActivityName = LanuangeDictionary[ActivityQuery.ActivityName.Substring(0, ActivityQuery.ActivityName.IndexOf("_"))];
                    lb_Activity.Text = AppDrill.ActivityName;
                }
            }
            catch { }
        }
        #endregion

        #region 设置dataShow2控件的值
        private void setDataShow2()
        {
            //分别取出最后一组控件的数据
            var dataShow1 = UserTag.Find(o => o.Group == 5 && o.Order == 1);
            if (dataShow1 != null)
            {
                var taglist1 = listTag.Where(o => o.Tag == dataShow1.Tag).FirstOrDefault();
                if (taglist1 != null)
                {
                    dataShow21.Captial.Text = Transformation(dataShow1.Tag);
                    if (AppDrill.UnitFormat == "yz")
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == taglist1.Unit);
                        if (UnitModel != null)
                            dataShow21.Unit.Text = UnitModel.UnitTo;
                        else
                            dataShow21.Unit.Text = taglist1.Unit;
                    }
                    else if (AppDrill.UnitFormat == "gz")
                    {
                        dataShow21.Unit.Text = taglist1.Unit;
                    }
                    //创建Consumer
                    try
                    {
                        if (AppData.session != null)
                            consumer1 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(taglist1.Tag + "_" + AppDrill.DrillID.ToString()));
                        consumer1.Listener += new MessageListener(consumer1_Listener);
                    }
                    catch { }
                }
                else
                {
                    dataShow21.Captial.Text = "";
                    dataShow21.Unit.Text = "";
                    dataShow21.Value.Text = "";
                }
            }
            else
            {
                dataShow21.Captial.Text = "";
                dataShow21.Unit.Text = "";
                dataShow21.Value.Text = "";
            }
            var dataShow2 = UserTag.Find(o => o.Group == 5 && o.Order == 2);
            if (dataShow2 != null)
            {
                var taglist2 = listTag.Where(o => o.Tag == dataShow2.Tag).FirstOrDefault();
                if (taglist2 != null)
                {
                    dataShow22.Captial.Text = Transformation(dataShow2.Tag);
                    if (AppDrill.UnitFormat == "yz")
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == taglist2.Unit);
                        if (UnitModel != null)
                            dataShow22.Unit.Text = UnitModel.UnitTo;
                        else
                            dataShow22.Unit.Text = taglist2.Unit;
                    }
                    else if (AppDrill.UnitFormat == "gz")
                    {
                        dataShow22.Unit.Text = taglist2.Unit;
                    }
                    //创建Consumer
                    try
                    {
                        if (AppData.session != null)
                            consumer2 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(taglist2.Tag + "_" + AppDrill.DrillID.ToString()));
                        consumer2.Listener += new MessageListener(consumer2_Listener);
                    }
                    catch { }
                }
                else
                {
                    dataShow22.Captial.Text = "";
                    dataShow22.Unit.Text = "";
                    dataShow22.Value.Text = "";
                }
            }
            else
            {
                dataShow22.Captial.Text = "";
                dataShow22.Unit.Text = "";
                dataShow22.Value.Text = "";
            }
            var dataShow3 = UserTag.Find(o => o.Group == 5 && o.Order == 3);
            if (dataShow3 != null)
            {
                var taglist3 = listTag.Where(o => o.Tag == dataShow3.Tag).FirstOrDefault();
                if (taglist3 != null)
                {
                    dataShow23.Captial.Text = Transformation(dataShow3.Tag);
                    if (AppDrill.UnitFormat == "yz")
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == taglist3.Unit);
                        if (UnitModel != null)
                            dataShow23.Unit.Text = UnitModel.UnitTo;
                        else
                            dataShow23.Unit.Text = taglist3.Unit;
                    }
                    else if (AppDrill.UnitFormat == "gz")
                    {
                        dataShow23.Unit.Text = taglist3.Unit;
                    }
                    //创建Consumer
                    try
                    {
                        if (AppData.session != null)
                            consumer3 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(taglist3.Tag + "_" + AppDrill.DrillID.ToString()));
                        consumer3.Listener += new MessageListener(consumer3_Listener);
                    }
                    catch { }
                }
                else
                {
                    dataShow23.Captial.Text = "";
                    dataShow23.Unit.Text = "";
                    dataShow23.Value.Text = "";
                }
            }
            else
            {
                dataShow23.Captial.Text = "";
                dataShow23.Unit.Text = "";
                dataShow23.Value.Text = "";
            }
            var dataShow4 = UserTag.Find(o => o.Group == 5 && o.Order == 4);
            if (dataShow4 != null)
            {
                var taglist4 = listTag.Where(o => o.Tag == dataShow4.Tag).FirstOrDefault();
                if (taglist4 != null)
                {
                    dataShow24.Captial.Text = Transformation(dataShow4.Tag);
                    if (AppDrill.UnitFormat == "yz")
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == taglist4.Unit);
                        if (UnitModel != null)
                            dataShow24.Unit.Text = UnitModel.UnitTo;
                        else
                            dataShow24.Unit.Text = taglist4.Unit;
                    }
                    else if (AppDrill.UnitFormat == "gz")
                    {
                        dataShow24.Unit.Text = taglist4.Unit;
                    }
                    //创建Consumer
                    try
                    {
                        if (AppData.session != null)
                            consumer4 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(taglist4.Tag + "_" + AppDrill.DrillID.ToString()));
                        consumer4.Listener += new MessageListener(consumer4_Listener);
                    }
                    catch { }
                }
                else
                {
                    dataShow24.Captial.Text = "";
                    dataShow24.Unit.Text = "";
                    dataShow24.Value.Text = "";
                }
            }
            else
            {
                dataShow24.Captial.Text = "";
                dataShow24.Unit.Text = "";
                dataShow24.Value.Text = "";
            }

            var dataShow5 = UserTag.Find(o => o.Group == 5 && o.Order == 5);
            if (dataShow5 != null)
            {
                var taglist5 = listTag.Where(o => o.Tag == dataShow5.Tag).FirstOrDefault();
                if (taglist5 != null)
                {
                    dataShow25.Captial.Text = Transformation(dataShow5.Tag);
                    if (AppDrill.UnitFormat == "yz")
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == taglist5.Unit);
                        if (UnitModel != null)
                            dataShow25.Unit.Text = UnitModel.UnitTo;
                        else
                            dataShow25.Unit.Text = taglist5.Unit;
                    }
                    else if (AppDrill.UnitFormat == "gz")
                    {
                        dataShow25.Unit.Text = taglist5.Unit;
                    }
                    //创建Consumer
                    try
                    {
                        if (AppData.session != null)
                            consumer5 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(taglist5.Tag + "_" + AppDrill.DrillID.ToString()));
                        consumer5.Listener += new MessageListener(consumer5_Listener);
                    }
                    catch { }
                }
                else
                {
                    dataShow25.Captial.Text = "";
                    dataShow25.Unit.Text = "";
                    dataShow25.Value.Text = "";
                }
            }
            else
            {
                dataShow25.Captial.Text = "";
                dataShow25.Unit.Text = "";
                dataShow25.Value.Text = "";
            }
        }

        #endregion

        /// <summary>
        /// 1秒定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now;
            this.radLabeltime.Text = DateTime.Now.ToString();
            this.lb_Activity.Text = AppDrill.ActivityName;
            int isAlarm = Alarms.list_Alarms.Where(o => o.status == 1).Count();
            if (isAlarm > 0)
            {
                if (this.btn_alarms.BackColor.Equals(Color.FromArgb(62, 62, 66)))
                    this.btn_alarms.BackColor = Color.Red;
                else if (this.btn_alarms.BackColor.Equals(Color.Red))
                    this.btn_alarms.BackColor = Color.FromArgb(62, 62, 66);
            }
            else
            {
                if (Alarms.list_Alarms.Where(o => o.status == 3).Count() > 0)
                {
                    this.btn_alarms.BackColor = Color.Red;
                }
                else
                {
                    this.btn_alarms.BackColor = Color.FromArgb(62, 62, 66);
                }
            }
        }

        #region 顶部datashow双击事件

        public void SetDataShowClick()
        {
            dataShow21.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Captial.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Value.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Unit.MouseClick += new MouseEventHandler(dataShow1_MouseClick);

            dataShow22.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Captial.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Value.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Unit.MouseClick += new MouseEventHandler(dataShow2_MouseClick);

            dataShow23.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Captial.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Value.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Unit.MouseClick += new MouseEventHandler(dataShow3_MouseClick);

            dataShow24.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Captial.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Value.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Unit.MouseClick += new MouseEventHandler(dataShow4_MouseClick);

            dataShow25.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Captial.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Value.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Unit.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
        }

        private void dataShow1_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = 5;
                form.order = 1;
                form.formName = "BaseForm";
                form.ShowDialog();
                if (form.remove) //在界面上删除该点
                {
                    //反注册事件
                    try
                    {
                        consumer1.Listener -= new MessageListener(consumer1_Listener);
                    }
                    catch { }
                    dataShow21.DSCaptial = "";
                    dataShow21.DSValue = "";
                    dataShow21.DSLValue = "";
                    dataShow21.DSHValue = "";
                    dataShow21.DSUnit = "";
                    dataShow21.SetTag();
                    //删除的时候需要将报警状态初始化
                    dataShow21.BackColor = Color.Black;
                }
                else
                {
                    if (!string.IsNullOrEmpty(form.Captial))
                    {
                        dataShow21.DSCaptial = form.Captial;
                        dataShow21.DSLValue = "L:" + form.LValue;
                        dataShow21.DSHValue = "H:" + form.HValue;
                        if (AppDrill.UnitFormat == "yz")
                        {
                            var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                            if (UnitModel != null)
                                dataShow21.DSUnit = UnitModel.UnitTo;
                            else
                                dataShow21.DSUnit = form.Unit;
                        }
                        else if (AppDrill.UnitFormat == "gz")
                        {
                            dataShow21.DSUnit = form.Unit;
                        }
                        dataShow21.SetTag();
                    }
                    try
                    {
                        //反注册事件，以取消曲线更新
                        if (!string.IsNullOrEmpty(dataShow21.Captial.Text) && string.IsNullOrEmpty(form.Tags))
                        {

                        }
                        else
                        {
                            try
                            {
                                consumer1.Listener -= new MessageListener(consumer1_Listener);

                            }
                            catch { }

                            if (AppData.session != null)
                                consumer1 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(form.Tags + "_" + form.DrillId));
                            consumer1.Listener += new MessageListener(consumer1_Listener);
                        }
                    }
                    catch { }
                }
            }
        }

        private void dataShow2_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = 5;
                form.order = 2;
                form.formName = "BaseForm";
                form.ShowDialog();
                if (form.remove) //在界面上删除该点
                {
                    //反注册事件
                    try
                    {
                        consumer2.Listener -= new MessageListener(consumer2_Listener);
                    }
                    catch { }
                    dataShow22.DSCaptial = "";
                    dataShow22.DSValue = "";
                    dataShow22.DSLValue = "";
                    dataShow22.DSHValue = "";
                    dataShow22.DSUnit = "";
                    dataShow22.SetTag();
                    //删除的时候需要将报警状态初始化
                    dataShow22.BackColor = Color.Black;
                }
                else
                {
                    if (!string.IsNullOrEmpty(form.Captial))
                    {
                        dataShow22.DSCaptial = form.Captial;
                        dataShow22.DSLValue = "L:" + form.LValue;
                        dataShow22.DSHValue = "H:" + form.HValue;
                        if (AppDrill.UnitFormat == "yz")
                        {
                            var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                            if (UnitModel != null)
                                dataShow22.DSUnit = UnitModel.UnitTo;
                            else
                                dataShow22.DSUnit = form.Unit;
                        }
                        else if (AppDrill.UnitFormat == "gz")
                        {
                            dataShow22.DSUnit = form.Unit;
                        }

                        dataShow22.SetTag();
                    }
                    try
                    {
                        //反注册事件，以取消曲线更新
                        if (!string.IsNullOrEmpty(dataShow22.Captial.Text) && string.IsNullOrEmpty(form.Tags))
                        {

                        }
                        else
                        {
                            try
                            {
                                consumer2.Listener -= new MessageListener(consumer2_Listener);
                            }
                            catch { }
                            if (AppData.session != null)
                                consumer2 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(form.Tags + "_" + form.DrillId));
                            consumer2.Listener += new MessageListener(consumer2_Listener);
                        }
                    }
                    catch { }
                }
            }
        }
        private void dataShow3_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = 5;
                form.order = 3;
                form.formName = "BaseForm";
                form.ShowDialog();
                if (form.remove) //在界面上删除该点
                {
                    //反注册事件
                    try
                    {
                        consumer2.Listener -= new MessageListener(consumer2_Listener);
                    }
                    catch { }
                    dataShow23.DSCaptial = "";
                    dataShow23.DSValue = "";
                    dataShow23.DSLValue = "";
                    dataShow23.DSHValue = "";
                    dataShow23.DSUnit = "";
                    dataShow23.SetTag();
                    //删除的时候需要将报警状态初始化
                    dataShow23.BackColor = Color.Black;
                }
                else
                {
                    if (!string.IsNullOrEmpty(form.Captial))
                    {
                        dataShow23.DSCaptial = form.Captial;
                        dataShow23.DSLValue = "L:" + form.LValue;
                        dataShow23.DSHValue = "H:" + form.HValue;
                        if (AppDrill.UnitFormat == "yz")
                        {
                            var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                            if (UnitModel != null)
                                dataShow23.DSUnit = UnitModel.UnitTo;
                            else
                                dataShow23.DSUnit = form.Unit;
                        }
                        else if (AppDrill.UnitFormat == "gz")
                        {
                            dataShow23.DSUnit = form.Unit;
                        }

                        dataShow23.SetTag();
                    }
                    try
                    {
                        //反注册事件，以取消曲线更新
                        if (!string.IsNullOrEmpty(dataShow23.Captial.Text) && string.IsNullOrEmpty(form.Tags))
                        {

                        }
                        else
                        {
                            try
                            {
                                consumer3.Listener -= new MessageListener(consumer3_Listener);

                            }
                            catch { }

                            if (AppData.session != null)
                                consumer3 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(form.Tags + "_" + form.DrillId));
                            consumer3.Listener += new MessageListener(consumer3_Listener);
                        }
                    }
                    catch { }
                }
            }
        }
        private void dataShow4_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = 5;
                form.order = 4;
                form.formName = "BaseForm";
                form.ShowDialog();
                if (form.remove) //在界面上删除该点
                {
                    //反注册事件
                    try
                    {
                        consumer4.Listener -= new MessageListener(consumer4_Listener);
                    }
                    catch { }
                    dataShow24.DSCaptial = "";
                    dataShow24.DSValue = "";
                    dataShow24.DSLValue = "";
                    dataShow24.DSHValue = "";
                    dataShow24.DSUnit = "";
                    dataShow24.SetTag();
                    //删除的时候需要将报警状态初始化
                    dataShow24.BackColor = Color.Black;
                }
                else
                {
                    if (!string.IsNullOrEmpty(form.Captial))
                    {
                        dataShow24.DSCaptial = form.Captial;
                        dataShow24.DSLValue = "L:" + form.LValue;
                        dataShow24.DSHValue = "H:" + form.HValue;
                        if (AppDrill.UnitFormat == "yz")
                        {
                            var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                            if (UnitModel != null)
                                dataShow24.DSUnit = UnitModel.UnitTo;
                            else
                                dataShow24.DSUnit = form.Unit;
                        }
                        else if (AppDrill.UnitFormat == "gz")
                        {
                            dataShow24.DSUnit = form.Unit;
                        }

                        dataShow24.SetTag();
                    }
                    try
                    {
                        //反注册事件，以取消曲线更新
                        if (!string.IsNullOrEmpty(dataShow24.Captial.Text) && string.IsNullOrEmpty(form.Tags))
                        {

                        }
                        else
                        {
                            try
                            {
                                consumer4.Listener -= new MessageListener(consumer4_Listener);

                            }
                            catch { }

                            if (AppData.session != null)
                                consumer4 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(form.Tags + "_" + form.DrillId));
                            consumer4.Listener += new MessageListener(consumer4_Listener);
                        }
                    }
                    catch { }
                }
            }
        }

        private void dataShow5_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = 5;
                form.order = 5;
                form.formName = "BaseForm";
                form.ShowDialog();
                if (form.remove) //在界面上删除该点
                {
                    //反注册事件
                    try
                    {
                        consumer5.Listener -= new MessageListener(consumer5_Listener);
                    }
                    catch { }
                    dataShow25.DSCaptial = "";
                    dataShow25.DSValue = "";
                    dataShow25.DSLValue = "";
                    dataShow25.DSHValue = "";
                    dataShow25.DSUnit = "";
                    dataShow25.SetTag();
                    //删除的时候需要将报警状态初始化
                    dataShow25.BackColor = Color.Black;
                }
                else
                {
                    if (!string.IsNullOrEmpty(form.Captial))
                    {
                        dataShow25.DSCaptial = form.Captial;
                        dataShow25.DSLValue = "L:" + form.LValue;
                        dataShow25.DSHValue = "H:" + form.HValue;
                        if (AppDrill.UnitFormat == "yz")
                        {
                            var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                            if (UnitModel != null)
                                dataShow25.DSUnit = UnitModel.UnitTo;
                            else
                                dataShow25.DSUnit = form.Unit;
                        }
                        else if (AppDrill.UnitFormat == "gz")
                        {
                            dataShow25.DSUnit = form.Unit;
                        }

                        dataShow25.SetTag();
                    }
                    try
                    {
                        //反注册事件，以取消曲线更新
                        if (!string.IsNullOrEmpty(dataShow25.Captial.Text) && string.IsNullOrEmpty(form.Tags))
                        {

                        }
                        else
                        {
                            try
                            {
                                consumer5.Listener -= new MessageListener(consumer5_Listener);

                            }
                            catch { }

                            if (AppData.session != null)
                                consumer5 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(form.Tags + "_" + form.DrillId));
                            consumer5.Listener += new MessageListener(consumer5_Listener);
                        }
                    }
                    catch { }
                }
            }
        }

        #endregion

        #region 消息中间件 ADD by ZAY in 2017.5.26

        //private delegate void ShowNoteMsgDelegate(string Tag, string DrillId, string Value);
        private delegate void ShowNoteMsgDelegate_New(Dictionary<string, string> map);

        //private void consumer0_Listener(IMessage message)
        //{
        //    try
        //    {
        //        IObjectMessage msg = (IObjectMessage)message;
        //        if (this.InvokeRequired)
        //            this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage0), (Dictionary<string, string>)msg.Body);
        //    }
        //    catch { }
        //}

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

        //private void ShowMessage0(Dictionary<string, string> map)
        //{
        //    Activity.Text = map["Value"];
        //}

        private void ShowMessage1(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, map["Tag"], map["DrillId"], value);
                if ((bool)listTag.Where(o => o.Tag == map["Tag"]).FirstOrDefault().IsBool)
                {
                    dataShow21.Value.Text = newValue > 0 ? "Yes" : "No";
                }
                else
                {
                    dataShow21.Value.Text = newValue.ToString("#0.00");
                }
                isAlarm(map["Tag"], map["DrillId"], map["Value"], 1);
            }
        }

        private void ShowMessage2(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, map["Tag"], map["DrillId"], value);
                if ((bool)listTag.Where(o => o.Tag == map["Tag"]).FirstOrDefault().IsBool)
                {
                    dataShow22.Value.Text = newValue > 0 ? "Yes" : "No";
                }
                else
                {
                    dataShow22.Value.Text = newValue.ToString("#0.00");
                }
                isAlarm(map["Tag"], map["DrillId"], map["Value"], 2);
            }
        }
        private void ShowMessage3(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, map["Tag"], map["DrillId"], value);
                if ((bool)listTag.Where(o => o.Tag == map["Tag"]).FirstOrDefault().IsBool)
                {
                    dataShow23.Value.Text = newValue > 0 ? "Yes" : "No";
                }
                else
                {
                    dataShow23.Value.Text = newValue.ToString("#0.00");
                }
                isAlarm(map["Tag"], map["DrillId"], map["Value"], 3);
            }
        }
        private void ShowMessage4(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, map["Tag"], map["DrillId"], value);
                if ((bool)listTag.Where(o => o.Tag == map["Tag"]).FirstOrDefault().IsBool)
                {
                    dataShow24.Value.Text = newValue > 0 ? "Yes" : "No";
                }
                else
                {
                    dataShow24.Value.Text = newValue.ToString("#0.00");
                }
                isAlarm(map["Tag"], map["DrillId"], map["Value"], 4);
            }
        }

        private void ShowMessage5(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, map["Tag"], map["DrillId"], value);
                if ((bool)listTag.Where(o => o.Tag == map["Tag"]).FirstOrDefault().IsBool)
                {
                    dataShow25.Value.Text = newValue > 0 ? "Yes" : "No";
                }
                else
                {
                    dataShow25.Value.Text = newValue.ToString("#0.00");
                }
                isAlarm(map["Tag"], map["DrillId"], map["Value"], 5);
            }
        }

        #endregion

        #region 报警闪烁  ADD by ZAY in 2017.5.26

        /// <summary>
        /// 判断是否报警
        /// </summary>
        private void isAlarm(string Tag, string DrillId, string Value, int Index)
        {
            try
            {
                var Dirrl = 0;
                if (!string.IsNullOrEmpty(DrillId))
                    Dirrl = int.Parse(DrillId);
                var model = listTag.Find(o => o.Tag == Tag && o.DrillId == Dirrl && o.AlarmFlag == 1);
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(Value))
                    {
                        AlarmModel old_am = Alarms.list_Alarms.Find(o => o.Tag == Tag);
                        if (model.HValue <= Decimal.Parse(Value) || model.LValue >= Decimal.Parse(Value))
                        {
                            //报警
                            if (old_am != null)
                            {
                                if (old_am.status == 2)
                                {
                                    old_am.status = 3;//已确认继续报警
                                }
                            }
                            else
                            {
                                AlarmModel new_am = new AlarmModel();
                                new_am.DrillId = Dirrl;
                                new_am.Tag = Tag;
                                new_am.Value = decimal.Parse(Value);
                                new_am.status = 1;
                                Alarms.list_Alarms.Add(new_am);
                            }
                            if (Index == 1)
                                dataShow21.BackColor = Color.Red;
                            else if (Index == 2)
                                dataShow22.BackColor = Color.Red;
                            else if (Index == 3)
                                dataShow23.BackColor = Color.Red;
                            else if (Index == 4)
                                dataShow24.BackColor = Color.Red;
                        }
                        else
                        {
                            if (old_am != null)
                            {
                                if (old_am.status != 1)
                                {
                                    Alarms.list_Alarms.Remove(old_am);//已经不报警了,移除
                                    //状态不等于1的操作数据库
                                    var alarm = _db.AlarmHistory.Where(o => o.Tag == old_am.Tag && o.RecoveryTime == null);
                                    if (alarm != null)
                                    {
                                        foreach (var a in alarm)
                                        {
                                            a.RecoveryTime = DateTime.Now;
                                        }
                                        backgroundWorker7.WorkerSupportsCancellation = true;
                                        backgroundWorker7.RunWorkerAsync();//开启异步保存
                                    }
                                }
                            }
                            if (Index == 1)
                                dataShow21.BackColor = Color.Black;
                            else if (Index == 2)
                                dataShow22.BackColor = Color.Black;
                            else if (Index == 3)
                                dataShow23.BackColor = Color.Black;
                            else if (Index == 4)
                                dataShow24.BackColor = Color.Black;
                        }
                    }
                }
            }
            catch { }
        }

        #endregion

        /// <summary>
        /// 主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnMain_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillFormSec)
                {
                    DrillFormSec drillForm = (DrillFormSec)frm;

                    if (drillForm.lbl_WellNoValue.Text == lbl_WellNoValue.Text)
                    {
                        drillForm.setChartActivite(true);
                        frm.Activate();
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
            }

            DrillForm form = new DrillForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();
            //this.Dispose();
        }

        /// <summary>
        /// 泥浆循环 朱安洋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnCirculate_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is CirculateForm)
                {
                    ((CirculateForm)frm).setChartActivite(true);
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "CirculateForm")
            //    return;
            CirculateForm form = new CirculateForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Location = new Point(0, 0);
            form.Show();
            //this.Dispose();
        }

        /// <summary>
        /// 定向钻井 祝鹏辉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnCtrlDrill_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DirectionalForm)
                {
                    ((DirectionalForm)frm).setChartActivite(true);
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "CtrlDrillForm")
            //    return;
            DirectionalForm form = new DirectionalForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Location = new Point(0, 0);
            form.Show();
            //this.Dispose();
        }

        /// <summary>
        /// 钻具管理 祝鹏辉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnTool_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is ToolManageForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "ToolManageForm")
            //    return;
            ToolManageForm form = new ToolManageForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();
            //this.Dispose();
        }

        /// <summary>
        /// 报表 邵哥
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnReport_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is ReportForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    frm.TopMost = true;
                    frm.TopMost = false;
                    return;
                }
            }
            ReportForm form = new ReportForm();

            form.Show();
        }

        /// <summary>
        /// 报警 杨帆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnAlarm_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is AlarmForm)
                {
                    frm.Close();
                    AlarmForm frmnew = new AlarmForm();
                    frmnew.Show();
                    return;
                }
            }
            //if (this.Name == "AlarmForm")
            //    return;
            AlarmForm form = new AlarmForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Location = new Point(0, 0);
            form.Show();
            //this.Dispose();
        }

        /// <summary>
        /// 设置 杨帆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnSetting_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is SettingForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            SettingForm form = new SettingForm();
            form.ShowDialog();
        }

        /// <summary>
        /// 钻井气体 李镰明
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnGas_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillingGasForm)
                {
                    ((DrillingGasForm)frm).setChartActivite(true);
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "GasForm")
            //    return;
            DrillingGasForm form = new DrillingGasForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();
            //this.Dispose();
        }

        private void radBtnRota_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is RotaForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "RotaForm")
            //    return;
            RotaForm form = new RotaForm();
            form.ShowDialog();
        }

        private void DrillPVT_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillPVTForm)
                {
                    ((DrillPVTForm)frm).setChartActivite(true);
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }
            //if (this.Name == "DrillPVTForm")
            //    return;
            DrillPVTForm form = new DrillPVTForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();
            //this.Dispose();
        }

        private void setHistoryValue(HistoryDataForm hisForm)
        {
            //hisForm.lbl_WellNoValue.Text = lbl_WellNoValue.Text;
            //hisForm.lbl_leasevalue.Text = lbl_leasevalue.Text;
            //hisForm.lbl_OperatorValue.Text = lbl_OperatorValue.Text;
            //hisForm.lbl_rignumvalue.Text = lbl_rignumvalue.Text;
            //hisForm.lbl_contractorvalue.Text = lbl_contractorvalue.Text;
        }

        private void rbtn_HistoryData_Click(object sender, EventArgs e)
        {
            Drill wellNo = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is HistoryDataForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    frm.Location = new Point(0, 0);
                    HistoryDataForm history = (HistoryDataForm)frm;

                    //if (history.lbl_WellNoValue.Text != lbl_WellNoValue.Text)
                    //{
                    //    history.clearHistoryCharts();
                    //}

                    setHistoryValue(history);
                    wellNo = _db.Drill.Where(O => O.DrillNo == lbl_WellNoValue.Text).FirstOrDefault();
                    AppDrill.DrillID = wellNo.ID;
                    return;
                }
            }
           
            wellNo = _db.Drill.Where(O => O.DrillNo == lbl_WellNoValue.Text).FirstOrDefault();
            AppDrill.DrillID = wellNo.ID;

            HistoryDataForm form = new HistoryDataForm();
            setHistoryValue(form);
            form.Size = new System.Drawing.Size(1920, 1080);
            //form.btn_pltDepath.Visible = false;
            //form.btn_pltTime.Visible = false;
            form.Show();
        }

        //private void radMenuItem1_Click(object sender, EventArgs e)
        //{
        //    var Obj = (Telerik.WinControls.UI.RadMenuItem)sender;
        //    MessageBox.Show(Obj.AccessibleName);
        //}

        private void button6_Click(object sender, EventArgs e)
        {
            ShowAlarm form = new ShowAlarm();
            form.ShowDialog();
            //如果设置的的是取消报警
            if (form.alerm == false)
            {
                Alarms.CancelAlarm();//取消所有报警
            }
            this.btn_alarms.BackColor = Color.FromArgb(62, 62, 66);
        }

        private void BaseForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            //当在主界面点击关闭时，才结束整个程序    
            //switch (this.Name)
            //{
            //    case "DrillForm":
            //        DialogResult dr = MessageBox.Show(AppDrill.message[0], AppDrill.message[1], MessageBoxButtons.OKCancel);
            //        if (dr == DialogResult.OK)
            //        {
            //            System.Diagnostics.Process.GetCurrentProcess().Kill();
            //        }
            //        else
            //        {
            //            e.Cancel = true;
            //        }
            //        break;
            //    case "CirculateForm":
            //        e.Cancel = true;
            //        break;
            //    //case "DrillingGasForm":
            //    //    e.Cancel = true;
            //    //    break;
            //    //case "DirectionalForm":
            //    //    e.Cancel = true;
            //    //    break;
            //    case "DrillPVTForm":
            //        e.Cancel = true;
            //        break;
            //    case "HistoryDataForm":
            //        e.Cancel = true;
            //        break;
            //    default:
            //        this.Dispose();
            //        break;
            //}
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

        //#region 公英制换算 ADD by ZAY in 2017.7.10
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="Tag">测点</param>
        ///// <param name="DrillId">井 ID</param>
        ///// <param name="value">值</param>
        ///// <returns></returns>
        //private double UnitConversion(string Tag, string DrillId, double value)
        //{
        //    try
        //    {
        //        if (AppDrill.UnitFormat == "yz")
        //        {
        //            var Dirrl = 0;
        //            if (!string.IsNullOrEmpty(DrillId))
        //                Dirrl = int.Parse(DrillId);
        //            var TagModel = listTag.Find(o => o.DrillId == Dirrl && o.Tag == Tag);
        //            if (TagModel != null)
        //            {
        //                var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == TagModel.Unit);
        //                if (UnitModel != null)
        //                {
        //                    value = value * (double)UnitModel.Coefficient;
        //                    return Math.Round(value, 2);
        //                }
        //            }
        //        }
        //        else if (AppDrill.UnitFormat == "gz")
        //        {
        //            return value;
        //        }
        //    }
        //    catch
        //    {
        //        return value;
        //    }
        //    return value;
        //}
        //#endregion
        //固定窗口
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rbtn_exit_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void Activity_Click(object sender, EventArgs e)
        {
            //权限控制
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
                return;
            //弹窗
            ActivityStatusForm d = new ActivityStatusForm();
            d.defaultString = GetDefaultText();
            //d.bsform = this;
            d.Show();
        }
        public void setActivityTitle(string text)
        {
            AppDrill.ActivityName = text;
            lb_Activity.Text = AppDrill.ActivityName;
        }

        //获取按钮List文本
        private List<String> getTranslateList()
        {
            List<String> TranslateList = new List<string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == "ReportForm")//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "Activity")
                            {
                                XmlNodeList m = node.ChildNodes;
                                int LanuangeCnt = 1;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    TranslateList.Add(xe2.GetAttribute("value"));
                                    LanuangeDictionary.Add("M" + LanuangeCnt, xe2.GetAttribute("value"));
                                    ++LanuangeCnt;
                                }
                                continue;
                            }
                        }
                    }
                }

            }
            catch { }
            return TranslateList;
        }

        //获取Default
        private string GetDefaultText()
        {
            string defaultText = "";
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == "MainForm")//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "lb_Activity")
                            {
                                return xe.GetAttribute("value");
                            }
                        }
                    }
                }

            }
            catch { }
            return defaultText;
        }

        //用于实时更新
        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();//开始
            }
            catch { }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                ActivityQuery = _db.ActivityStatus.Where(o => o.IsSelect == true && o.DrillID == AppDrill.DrillID).FirstOrDefault();
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                AppDrill.ActivityName = LanuangeDictionary[ActivityQuery.ActivityName.Substring(0, ActivityQuery.ActivityName.IndexOf("_"))];
                lb_Activity.Text = AppDrill.ActivityName;
                backgroundWorker2.CancelAsync();
            }
            catch { }
        }

        private void btn_pipeT_Click(object sender, EventArgs e)
        {
            ShowMessage message = new ShowMessage();
            message.ShowDialog();
        }

        public void insertMemo(string s)
        {
            try
            {
                //var X = list1[index].X;
                //var Y = list1[index].Y;
                //var range = 0;
                //var xValue = X * range / 100;
                //var yValue = Comm.ConvertIntDateTime(Y);

                //跳转编辑窗口
                string MemoText = s;
                long unix;
                if (!string.IsNullOrEmpty(MemoText))
                {
                    // 给曲线添加一个Memo
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                    unix = (long)(DateTime.Now - startTime).TotalMilliseconds / 1000 - 10;
                    string showMemo = MemoText;
                    byte[] showMemo_b = System.Text.Encoding.Default.GetBytes(showMemo);
                    string result = "";
                    //每16个字节换一行
                    for (int j = 0; j <= showMemo_b.Length / 16; j++)
                    {
                        showMemo = showMemo.Insert(j * 16, "\n");
                        result += System.Text.Encoding.Default.GetString(showMemo_b, j * 16, (j + 1) * 16 > showMemo_b.Length ? showMemo_b.Length - j * 16 : 16) + "\n";
                    }
                    TextObj memo = new TextObj(showMemo, 2, unix); // 相差毫秒数);   //曲线内容与出现位置
                    memo.Tag = 1;//用于鉴别是否是手动加入的memo
                    memo.FontSpec.Border.Color = Color.Red;            //Memo边框颜色
                    memo.FontSpec.FontColor = Color.White;                     //Memo文本颜色
                    memo.FontSpec.Size = 24f;                                  //Memo文本大小
                    // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
                    memo.Location.AlignH = AlignH.Left;
                    memo.Location.AlignV = AlignV.Top;
                    memo.FontSpec.Fill = new Fill(Color.Red, Color.Red, 35);
                    memo.FontSpec.StringAlignment = StringAlignment.Near;
                    AppDrill.MessageList.Add(new TextMemo { Text = showMemo, unix = unix });
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm is DrillForm)
                        {
                            ((DrillForm)frm).setMemo(memo);
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

                    //myPane.GraphObjList.Add(memo);

                    #region 将Memo插入数据库

                    Memo memoModel = new Memo();
                    memoModel.Text = MemoText;
                    //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    //DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
                    //TimeSpan toNow = dtNow.Subtract(dtStart);
                    //string timeStamp = toNow.Ticks.ToString();
                    //timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);//将UNIX时间戳转换成系统时间
                    memoModel.UnixTime = Convert.ToInt32(unix);
                    memoModel.CreateTime = DateTime.Now;
                    memoModel.DrillID = AppDrill.DrillID;
                    memoModel.dataMakePGM = "Hand";
                    memoModel.dataMakeTime = DateTime.Now;
                    memoModel.dataMakeUser = AppDrill.username;
                    memoModel.dataUpdPGM = "Hand";
                    memoModel.dataUpdTime = DateTime.Now;
                    memoModel.dataUpdUser = AppDrill.username;
                    _db.Memo.Add(memoModel);
                    _db.SaveChanges();
                }
                    #endregion
            }
            catch { }
        }

        private void lbl_Auto_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_Auto.Text == AppDrill.slip_list[0])
                {
                    lbl_Auto.Text = AppDrill.slip_list[1];//自动
                    lbl_Out.Enabled = false;
                }
                else
                {
                    lbl_Auto.Text = AppDrill.slip_list[0];
                    lbl_Out.Enabled = true;
                }
            }
            catch { }
        }

        private void lbl_Out_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbl_Out.Text == AppDrill.slip_list[2])
                {
                    lbl_Out.Text = AppDrill.slip_list[3];
                }
                else
                {
                    lbl_Out.Text = AppDrill.slip_list[2];
                }
            }
            catch { }
        }
        private void btn_zeroBW_Click(object sender, EventArgs e)
        {
            //btn_zeroBW.Enabled = false;
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.RunWorkerAsync();
        }
        private void btn_zeroGL_Click(object sender, EventArgs e)
        {
            //btn_zeroGL.Enabled = false;
            backgroundWorker4.WorkerSupportsCancellation = true;
            backgroundWorker4.RunWorkerAsync();
        }

        private void btn_hornIO_Click(object sender, EventArgs e)
        {
            //btn_hornIO.Enabled = false;
            backgroundWorker5.WorkerSupportsCancellation = true;
            backgroundWorker5.RunWorkerAsync();
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(3 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(3 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btn_zeroBW.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                ReciveMsg();
            }
            catch { }
            backgroundWorker3.CancelAsync();
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(4 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(4 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btn_zeroGL.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                    MessageBox.Show("Set Failed !");
            }
            catch { }
            backgroundWorker4.CancelAsync();
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(5 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(5 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btn_hornIO.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                    MessageBox.Show("Set Failed !");
            }
            catch { }
            backgroundWorker5.CancelAsync();
        }

        private void btn_setDP_Click(object sender, EventArgs e)
        {
            SetCommand command = new SetCommand();
            command.Show();
        }
        /// <summary>
        /// 权限配置
        /// </summary>
        private void InitPermission()
        {
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
            {
                //btn_hornIO.Enabled = false;
                //btn_hornIO.BackColor = Color.Gray;
                //btn_setDP.Enabled = false;
                //btn_setDP.BackColor = Color.Gray;
                //btn_zeroBW.Enabled = false;
                //btn_zeroBW.BackColor = Color.Gray;
                //btn_zeroGL.Enabled = false;
                //btn_zeroGL.BackColor = Color.Gray;
                // btn_zeroDP.Enabled = false;
                //btn_zeroDP.BackColor = Color.Gray;
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

        private void btn_zeroDP_Click(object sender, EventArgs e)
        {
            //btn_zeroDP.Enabled = false;
            backgroundWorker6.WorkerSupportsCancellation = true;
            backgroundWorker6.RunWorkerAsync();
        }

        private void backgroundWorker6_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                sendMsg(55 + "," + 1);
                Thread.Sleep(2000);
                sendMsg(55 + "," + 0);
            }
            catch { }
        }

        private void backgroundWorker6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //btn_zeroDP.Enabled = true;
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                    MessageBox.Show("Set Failed !");
            }
            catch { }
            backgroundWorker6.CancelAsync();
        }

        private void backgroundWorker7_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _db.SaveChanges();
            }
            catch { }
        }

        private void backgroundWorker7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker7.CancelAsync();
        }
        /// <summary>
        /// 设置所有窗体不激活曲线刷新
        /// </summary>
        public void setAllFormActivite()
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    ((DrillForm)frm).setChartActivite(false);
                    continue;
                }
                if (frm is CirculateForm)
                {
                    ((CirculateForm)frm).setChartActivite(false);
                    continue;
                }
                if (frm is DirectionalForm)
                {
                    ((DirectionalForm)frm).setChartActivite(false);
                    continue;
                }
                if (frm is DrillingGasForm)
                {
                    ((DrillingGasForm)frm).setChartActivite(false);
                    continue;
                }
                if (frm is DrillPVTForm)
                {
                    ((DrillPVTForm)frm).setChartActivite(false);
                    continue;
                }
            }
        }

        private void btn_pltTime_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    DrillForm dill = (DrillForm)frm;

                    //if (dill.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    dill.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }

                if (frm is CirculateForm)
                {
                    CirculateForm circulate = (CirculateForm)frm;

                    //if (circulate.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    circulate.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }

                if (frm is DirectionalForm)
                {
                    DirectionalForm directional = (DirectionalForm)frm;

                    //if (directional.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    directional.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }


                if (frm is DrillingGasForm)
                {
                    DrillingGasForm gas = (DrillingGasForm)frm;

                    //if (gas.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    gas.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }

                if (frm is DrillPVTForm)
                {
                    DrillPVTForm pvt = (DrillPVTForm)frm;

                    //if (pvt.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    pvt.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }

                if (frm is HistoryDataForm)
                {
                    HistoryDataForm history = (HistoryDataForm)frm;

                    //if (history.btn_pltTime.GetHashCode() == sender.GetHashCode())
                    //{
                    //    //history.PlotByTime();
                    //    btn_pltTime.Enabled = false;
                    //    btn_pltDepath.Enabled = true;
                    //}
                }
            }
        }

        private void btn_pltDepath_Click(object sender, EventArgs e)
        {

            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    DrillForm dill = (DrillForm)frm;

                    //if (dill.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    dill.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }

              
                if (frm is CirculateForm)
                {
                    CirculateForm circulate = (CirculateForm)frm;

                    //if (circulate.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    circulate.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }

                if (frm is DirectionalForm)
                {
                    DirectionalForm directional = (DirectionalForm)frm;

                    //if (directional.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    directional.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }

                if (frm is DrillingGasForm)
                {
                    DrillingGasForm gas = (DrillingGasForm)frm;

                    //if (gas.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    gas.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }

                if (frm is DrillPVTForm)
                {
                    DrillPVTForm history = (DrillPVTForm)frm;

                    //if (history.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    history.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }

                if (frm is HistoryDataForm)
                {
                    HistoryDataForm pvt = (HistoryDataForm)frm;

                    //if (pvt.btn_pltDepath.GetHashCode() == sender.GetHashCode())
                    //{
                    //    //pvt.PlotByDepath();
                    //    btn_pltTime.Enabled = true;
                    //    btn_pltDepath.Enabled = false;
                    //}
                }
            }
        }

        private void lbl_WellNoValue_Click(object sender, EventArgs e)
        {

        }

        private void rbtn_return_Click(object sender, EventArgs e)
        {

            //for (int i = 0; i < Application.OpenForms.Count; i++)
            //{
            //    if (Application.OpenForms[i].Name != "MapForm" && Application.OpenForms[i].Name != "LoginForm" && Application.OpenForms[i].Name != "DrillForm")
            //    {
            //        Application.OpenForms[i].Close();
            //    }
            //}
            //this.Close();
            if (Application.OpenForms["MapForm"] != null)
            {
                Application.OpenForms["MapForm"].Show();
                Application.OpenForms["MapForm"].BringToFront();
            }
            else {
                MapForm frm = new MapForm();
                frm.Show();
            }
        }
    }
}
