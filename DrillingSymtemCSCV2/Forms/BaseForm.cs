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
using CopyDataStruct;
namespace DrillingSymtemCSCV2.Forms
{
    public partial class BaseForm : RadForm
    {
        public DrillOSEntities _db;
        private IMessageConsumer consumer;
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
        private static string strWellNo = string.Empty;
        public int m_iDrillID = 0;
        private UserTagRef m_userTagRef = null;
        private static System.Windows.Forms.Timer timer = null;
        private static DrillOSEntities m_db = null;
        private static User m_user = null;
        private bool m_bIsShowCheckTag = false;

        private Drill drill;
        private Dictionary<string, string> LanuangeDictionary = new Dictionary<string, string>();
        //IP端口设置相关属性
        static Socket client;
        static string ServerIP;
        static string ClientIP;
        static int ServerPort;
        static int ClientPort;

        private delegate void ShowNoteMsgDelegate_New(RealTimeData realTimeData);
        public delegate void DataShow(RealTimeData realTimeData);
        private List<System.Windows.Forms.Label> m_labList = new List<System.Windows.Forms.Label>();
        private Dictionary<DATA_SHOW, UserControls.DataShow2> m_dicTags = new Dictionary<DATA_SHOW, UserControls.DataShow2>();
        private Dictionary<string, Button> m_dicCheckButton = new Dictionary<string, Button>();

        public DataShow m_delegateDataShow = null;

        public BaseForm()
        {
            _db = new DrillOSEntities();
            m_db = new DrillOSEntities();

            if (listTag == null)
            {
                listTag = new List<DrillTag>();
            }

            UserTag = new List<UserTag>();
            InitializeComponent();
            drill = new Drill();

            if (null == timer)
            {
                timer = new System.Windows.Forms.Timer(this.components);
                if (null != timer)
                {
                    timer.Enabled = true;
                    timer.Interval = 60 * 1000;
                    timer.Tick += new System.EventHandler(timer_Tick);
                }
            }
        }

        public static void modifyUserInfo(bool bActive)
        {
            try
            {
                if (null == m_db)
                {
                    return;
                }

                var userList = m_db.User.ToList();
                m_user = userList.Where(u => u.username.ToUpper() == AppDrill.username.ToUpper()).FirstOrDefault();

                int itimeStamp = (int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
                if (bActive)
                {
                    m_user.timeStamp = itimeStamp;
                    m_db.SaveChanges();
                }
                else
                {
                    m_user.timeStamp = itimeStamp;
                    m_user.isActive = bActive;
                    m_db.SaveChanges();
                }
            }
            catch
            {
            }
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (null == m_db)
                {
                    m_db = new DrillOSEntities();
                }
                else
                {
                    modifyUserInfo(true);
                }
            }
            catch
            {
            }
        }

        public int getY(long lTime)
        {
            long lMax = 0L;
            long lMin = 0L;
            int iHeight = 0;
            int iResult = 0;
            double dCurrent = 0;
            double dTotal = 1;
            double dValue = 0;

            m_depthChart.getMaxMin(ref lMax, ref lMin);
            iHeight = m_depthChart.getHeight();

            if (lTime - lMin > 0)
            {
                dCurrent = lTime - lMin;
                dTotal = lMax - lMin;

                dValue = dCurrent / dTotal;

                if (dTotal > 0 && dValue < 1.0)
                {
                    iResult = (int)(dValue * iHeight + m_depthChart.Location.Y);
                }
            }

            return iResult;
        }

        private void changeRows(System.Windows.Forms.Label lab, string strValue)
        {
            int iLblNum = strValue.Length;   //Label内容长度
            int iRowNum = 30;           //每行显示的字数
            int iRowHeight = lab.Height;           //每行的高度
            int iColNum = (iLblNum - (iLblNum / iRowNum) * iRowNum) == 0 ? (iLblNum / iRowNum) : (iLblNum / iRowNum) + 1;   //列数
            lab.AutoSize = false;    //设置AutoSize
            lab.Width = 160;         //设置显示宽度
            lab.Height = iRowHeight * iColNum;           //设置显示高度
        }

        public void showLable(long lMin, long lMax)
        {
            try
            {
                setLableLocatoin();
                addOldLable(lMin, lMax);
            }
            catch
            {
            }
        }

        public void setLableLocatoin()
        {
            foreach (var lab in m_labList)
            {
                if (null == lab)
                {
                    return;
                }

                long lTime = 0L;
                int iX = lab.Location.X;
                lTime = (long)lab.Tag;

                int iY = getY(lTime);

                if (0 >= iY)
                {
                    lab.Visible = false;
                    continue;
                }
                else
                {
                    lab.Visible = true;
                    lab.BringToFront();
                }

                lab.Location = new System.Drawing.Point(iX, iY - lab.Size.Height / 2);
            }
        }

        public void createLable(string strValue, long lTime, string strName = "")
        {
            int iX = m_depthChart.Location.X + 130;
            int iY = getY(lTime);

            System.Windows.Forms.Label lab = new System.Windows.Forms.Label();

            if (null == lab)
            {
                return;
            }

            lab.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lab.ForeColor = System.Drawing.Color.Yellow;
            lab.Text = strValue;
            lab.Name = strName;
            lab.Tag = lTime;
            changeRows(lab, strValue);
            lab.Location = new System.Drawing.Point(iX, iY - lab.Size.Height / 2);

            lab.BackColor = Color.Transparent;
            //lab.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lab.Parent = this;

            if (iY <= 0)
            {
                lab.Visible = false;
            }
            else
            {
                lab.Visible = true;
                lab.BringToFront();
            }

            m_labList.Add(lab);
        }

        public void removeLable()
        {
            int iCount = m_labList.Count;
            for (int i = 0; i < iCount; ++i)
            {
                var lab = m_labList[i];
                if (lab.Name == "old")
                {
                    lab.Visible = false;
                    m_labList.Remove(lab);
                }
            }
        }

        public void removeLable(long lTime)
        {
            int iCount = m_labList.Count;
            for (int i = 0; i < iCount; ++i)
            {
                var lab = m_labList[i];
                long lLabTime = (long)lab.Tag;

                if (lLabTime == lTime)
                {
                    lab.Visible = false;
                    m_labList.Remove(lab);
                }
            }
        }

        private bool IsNeedAddLable(long lTime)
        {
            foreach (var lab in m_labList)
            {
                if (null != lab)
                {
                    long lLabTime = (long)lab.Tag;

                    if (lTime == lLabTime)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void addOldLable(long lMin, long lMax)
        {
            var memoList = _db.Memo.Where(o => o.DrillID == m_iDrillID && o.dataMakeUser == AppDrill.username && o.UnixTime >= lMin && o.UnixTime <= lMax).ToList();

            if (null == memoList || memoList.Count <= 0)
            {
                return;
            }

            foreach (var memo in memoList)
            {
                if (null != memo)
                {
                    long lTime = (long)memo.UnixTime;

                    if (IsNeedAddLable(lTime))
                    {
                        createLable(memo.Text, lTime);
                    }
                }
            }
        }

        public void setTagValue(string strValue)
        {
            dataShow21.setTagValue(strValue);
            dataShow22.setTagValue(strValue);
            dataShow23.setTagValue(strValue);
            dataShow24.setTagValue(strValue);
            dataShow25.setTagValue(strValue);
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

            model.startTime = m_lTimestap - 3000;
            model.endTime = m_lTimestap;
            model.DrillId = AppDrill.DrillID;
            model.DepthTag = m_depthChart.getDepthTag();
            model.isHistoryData = false;

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
            }
        }

        private void InitTags()
        {
            if (null != m_tags)
            {
                m_tags.Clear();

                m_strTag1 = string.Empty;
                m_strTag2 = string.Empty;
                m_strTag3 = string.Empty;
            }
        }

        public void getData()
        {
            try
            {
                Thread.Sleep(500);

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == AppDrill.DrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                InitTags();
                getTags(UserTag, m_fourChart1, ref m_strTag1);
                getTags(UserTag, m_fourChart2, ref m_strTag2);
                getTags(UserTag, m_fourChart3, ref m_strTag3);

                getHistoryData();
                m_depthChart.setReceive(true);
            }
            catch
            {
            }
        }

        public void getData(long lTimestapBegin, long lTimestapEnd)
        {
            try
            {
                _db = new DrillOSEntities();

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == this.m_iDrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                InitTags();

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
            model.DrillId = this.m_iDrillID;
            model.DepthTag = m_depthChart.getDepthTag();

            model.isHistoryData = false;

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

                if (dataList.datas.Count <= 0 || dataList.Depthdatas.Count <= 0)
                {
                    return;
                }

                m_depthChart.clearAll();

                m_depthChart.addHistoryData(Depthdatas);
                setFourChart(dataList.datas);
            }
        }

        public void setBtnEnable(HistoryDataForm hisForm, bool bIsEnable)
        {
            hisForm.rbtn_Drill.Visible = bIsEnable;
            hisForm.rbtn_Circulate.Visible = bIsEnable;
            hisForm.rbtn_Directional.Visible = bIsEnable;
            hisForm.rbtn_Alarm.Visible = bIsEnable;
            hisForm.rbtn_HistoryData.Visible = bIsEnable;
            hisForm.rbtn_exit.Visible = true;
        }

        public void setBtnLocation(HistoryDataForm hisForm)
        {
            int X = 13;
            int Y = 15;
            int iHight = 60;
            hisForm.rbtn_return.Location = new Point(X, Y);
            hisForm.rbtn_exit.Location = new Point(X, Y + iHight);
            hisForm.rbtn_Setting.Location = new Point(X, Y + iHight * 2);
            hisForm.pnl_Menu.Size = new System.Drawing.Size(145, 1050);
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
                {
                    AppData.factory = new ConnectionFactory(System.Configuration.ConfigurationManager.AppSettings["DataSourceAddress"].ToString());
                }
                //开始
                //消息中间件
                if (AppData.connection == null)
                {
                    AppData.connection = AppData.factory.CreateConnection(System.Configuration.ConfigurationManager.AppSettings["DataSourceID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DataSourcePassWord"].ToString());
                }

                AppData.connection.Start();
                if (AppData.session == null)
                {
                    AppData.session = AppData.connection.CreateSession();
                }
                //报警数据以用户来读取数据
                m_userTagRef = _db.UserTagRef.Where(o => o.Username == AppDrill.username).FirstOrDefault();
                InitDictionaryTags();
            }
            catch
            {
            }

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
                catch
                {
                }

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
                //lbl_OperatorValue.Text = AppDrill.username;//设置用户名
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

        public void saveUserTagRef(bool bIsSaveAll, bool bIsFirst)
        {
            try
            {
                DrillOSEntities db = new DrillOSEntities();
                if (null == db)
                {
                    return;
                }

                if (bIsSaveAll && bIsFirst)
                {
                    var UserTagRef = db.UserTagRef.Where(o => o.Username == AppDrill.username).ToList();
                    if (null == UserTagRef)
                    {
                        return;
                    }

                    foreach (var user in UserTagRef)
                    {
                        if (null != user)
                        {
                            user.JsonTag = new JavaScriptSerializer().Serialize(AppDrill.UserTag);
                            user.dataUpdPGM = "reset";
                            user.dataUpdTime = DateTime.Now;
                            user.dataUpdUser = "reset";
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    var UserTagRef = db.UserTagRef.Where(o => o.DrillId == m_iDrillID && o.Username == AppDrill.username).FirstOrDefault();
                    if (null != UserTagRef)
                    {
                        UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(AppDrill.UserTag);
                        UserTagRef.dataUpdPGM = "reset";
                        UserTagRef.dataUpdTime = DateTime.Now;
                        UserTagRef.dataUpdUser = "reset";
                        db.SaveChanges();
                    }
                    else
                    {
                        UserTagRef = new UserTagRef();
                        if (null != UserTagRef)
                        {
                            UserTagRef.DrillId = m_iDrillID;
                            UserTagRef.Username = AppDrill.username;
                            UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(AppDrill.UserTag);
                            UserTagRef.dataUpdPGM = "reset";
                            UserTagRef.dataUpdTime = DateTime.Now;
                            UserTagRef.dataUpdUser = "reset";
                            db.UserTagRef.Add(UserTagRef);
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AppDrill.DrillID < 0)
                {
                    AppDrill.DrillID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DrillID"].ToString());
                }

                if (AppDrill.UnitFormat == "-1")
                {
                    AppDrill.UnitFormat = System.Configuration.ConfigurationManager.AppSettings["System"].ToString();
                }

                if (AppData.UnitTransfer == null)
                {
                    AppData.UnitTransfer = new List<UnitTransfer>();
                    AppData.UnitTransfer = _db.UnitTransfer.ToList();
                }

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == AppDrill.DrillID && o.Username == AppDrill.username).FirstOrDefault();
                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                if (AppDrill.bUserTagFirst)
                {
                    var defaultUserTagRef = _db.UserTagRef.Where(o => o.DrillId == 1 && o.Username == "default").FirstOrDefault();
                    if (defaultUserTagRef != null)
                    {
                        if (defaultUserTagRef.JsonTag != null)
                        {
                            AppDrill.UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(defaultUserTagRef.JsonTag); //反序列化
                            AppDrill.bUserTagFirst = false;
                        }
                    }
                }


                //取出tag数据
                if (listTag.Count == 0)
                {
                    listTag = _db.DrillTag.Where(o => o.DrillId == 1).ToList();
                }

                listDrill = _db.Drill.ToList();
                list_tagdir = _db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据

                using (var dbContext = new DrillOSEntities())
                {
                    ActivityQuery = dbContext.ActivityStatus.Where(o => o.IsSelect == true && o.DrillID == AppDrill.DrillID).FirstOrDefault();
                }

            }
            catch
            {
            }
        }

        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                InitDataShow();
                if (m_bIsShowCheckTag)
                {
                    pnl_CheckTag.Visible = true;
                    InitCheckTags();
                }
                else
                {
                    pnl_CheckTag.Visible = false;
                }

                if (this.Name == "HistoryDataForm")
                {
                    return;
                }


                var DrillModel = listDrill.Where(o => o.ID == m_iDrillID).FirstOrDefault();
                drill = _db.Drill.Where(O => O.ID == m_iDrillID).FirstOrDefault();
                if (DrillModel != null)
                {
                    lbl_WellNoValue.Text = DrillModel.DrillNo;
                    AppDrill.DrillNo = DrillModel.DrillNo;
                }

                //0611修改，更改抬头
                if (drill != null)
                {
                    lbl_WellNoValue.Text = drill.DrillNo;
                    strWellNo = drill.DrillNo;
                    lbl_leasevalue.Text = drill.Lease;
                    lbl_rignumvalue.Text = drill.RigNo;
                    lbl_contractorvalue.Text = drill.Contractor;
                    lbl_OperatorValue.Text = drill.Operator;
                }


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
                {
                    this.btn_alarms.BackColor = Color.Red;
                }
                else if (this.btn_alarms.BackColor.Equals(Color.Red))
                {
                    this.btn_alarms.BackColor = Color.FromArgb(62, 62, 66);
                }
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

        public void resetDataShow()
        {
            InitDataShow(true);
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
                form.DrillId = this.m_iDrillID.ToString();
                form.ShowDialog();

                if (form.remove) //在界面上删除该点
                {
                    setDataShowEmpty(dataShow21);
                }
                else
                {
                    setDataShow(dataShow21, form);
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
                form.DrillId = this.m_iDrillID.ToString();
                form.ShowDialog();

                if (form.remove) //在界面上删除该点
                {
                    setDataShowEmpty(dataShow22);
                }
                else
                {
                    setDataShow(dataShow22, form);
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
                form.DrillId = this.m_iDrillID.ToString();
                form.ShowDialog();

                if (form.remove) //在界面上删除该点
                {
                    setDataShowEmpty(dataShow23);
                }
                else
                {
                    setDataShow(dataShow23, form);
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
                form.DrillId = this.m_iDrillID.ToString();
                form.ShowDialog();

                if (form.remove) //在界面上删除该点
                {
                    setDataShowEmpty(dataShow24);
                }
                else
                {
                    setDataShow(dataShow24, form);
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
                form.DrillId = this.m_iDrillID.ToString();
                form.ShowDialog();

                if (form.remove) //在界面上删除该点
                {
                    setDataShowEmpty(dataShow25);
                }
                else
                {
                    setDataShow(dataShow25, form);
                }
            }
        }

        #endregion

        #region 报警闪烁  ADD by ZAY in 2017.5.26

        /// <summary>
        /// 判断是否报警
        /// </summary>

        private void setDataShowBackColor(int iIndex, int iSetColor)
        {
            switch (iIndex)
            {
                case 0:
                    dataShow21.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 1:
                    dataShow22.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 2:
                    dataShow23.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 3:
                    dataShow24.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 4:
                    dataShow25.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                default:
                    break;
            }
        }

        private void getJsonAlarm(ref JsonAlarm jsonAlarm, string strTag)
        {
            if (null != AppDrill.JsonAlarmList)
            {
                jsonAlarm = AppDrill.JsonAlarmList.Where(o => o.Tag == strTag).FirstOrDefault();
                return;
            }

            List<JsonAlarm> JsonAlarmList = null;

            if (m_userTagRef != null)
            {
                if (m_userTagRef.JsonAlarm != null)
                {
                    JsonAlarmList = new JavaScriptSerializer().Deserialize<List<JsonAlarm>>(m_userTagRef.JsonAlarm);

                    if (null != JsonAlarmList)
                    {
                        jsonAlarm = JsonAlarmList.Where(o => o.Tag == strTag).FirstOrDefault();
                    }
                }
            }
        }

        private bool isNeedAlarm(DrillTag drillTag, JsonAlarm jsonAlarm, string strValue, ref string alarmcode)
        {
            if (null != jsonAlarm)
            {
                if ((jsonAlarm.HIsActive && jsonAlarm.H < double.Parse(strValue)))
                {
                    alarmcode = drillTag.Tag + "_1";
                    return true;
                }
                else if ((jsonAlarm.LIsActive && jsonAlarm.L > double.Parse(strValue)))
                {
                    alarmcode = drillTag.Tag + "_0";
                    return true;
                }

                else
                {
                    return false;
                }
            }
            else
            {
                if (drillTag.HValue < Decimal.Parse(strValue))
                {
                    alarmcode = drillTag.Tag + "1";
                    return true;
                }
                else if (drillTag.LValue > Decimal.Parse(strValue))
                {
                    alarmcode = drillTag.Tag + "0";
                    return true;
                }

                else
                {
                    return false;
                }
            }
        }

        private void isAlarm(string Tag, string DrillId, string Value, int Index)
        {
            try
            {
                var Dirrl = 0;
                if (!string.IsNullOrEmpty(DrillId))
                {
                    Dirrl = int.Parse(DrillId);
                }

                var model = listTag.Find(o => o.Tag == Tag && o.DrillId == 1 && o.AlarmFlag == 1);

                JsonAlarm JsonAlarm = null;
                getJsonAlarm(ref JsonAlarm, Tag);

                //取出默认测点报警值或者用户设定的报警值
                if (model != null || null != JsonAlarm)
                {
                    if (!string.IsNullOrEmpty(Value))
                    {
                        AlarmModel old_am = Alarms.list_Alarms.Find(o => o.Tag == Tag);
                        string alarmcode = "";
                        bool isAlarm = isNeedAlarm(model, JsonAlarm, Value, ref alarmcode);

                        if (isAlarm)
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
                                //190410修改，触发新的报警后存到AlarmHis表
                                using (m_db = new Model.DrillOSEntities())
                                {
                                    AlarmHis item = new AlarmHis();
                                    item.DrillID = Dirrl;
                                    item.UserName = AppDrill.username;
                                    item.Code = alarmcode;
                                    item.Time = DateTime.Now;
                                    m_db.AlarmHis.Add(item);
                                    m_db.SaveChanges();
                                }
                            }

                            setDataShowBackColor(Index, 0);
                        }
                        else
                        {
                            if (old_am != null)
                            {
                                //    if (old_am.status != 1)
                                //    {
                                //        Alarms.list_Alarms.Remove(old_am);//已经不报警了,移除
                                //        //状态不等于1的操作数据库
                                //        var alarm = _db.AlarmHistory.Where(o => o.Tag == old_am.Tag && o.RecoveryTime == null);
                                //        if (alarm != null)
                                //        {
                                //            foreach (var a in alarm)
                                //            {
                                //                a.RecoveryTime = DateTime.Now;
                                //            }

                                //            backgroundWorker7.WorkerSupportsCancellation = true;
                                //            backgroundWorker7.RunWorkerAsync();//开启异步保存
                                //        }
                                //    }

                                //190410修改，网络版没报警直接在list中移除
                                Alarms.list_Alarms.Remove(old_am);//已经不报警了,移除
                            }




                            setDataShowBackColor(Index, 1);
                        }
                    }
                }
            }
            catch
            {
            }
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
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;

                    if (drillForm.lbl_WellNoValue.Text == strWellNo)
                    {
                        drillForm.setChartActivite(true);
                        frm.Activate();
                        frm.WindowState = FormWindowState.Normal;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 泥浆循环 朱安洋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void createCirculateForm()
        {
            CirculateForm form = new CirculateForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.m_iDrillID = this.m_iDrillID;
            form.setDrillID(this.m_iDrillID);
            form.Location = new Point(0, 0);
            form.Show();
        }

        private void radBtnCirculate_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is CirculateForm)
                {
                    CirculateForm circulateFrm = (CirculateForm)frm;

                    if (circulateFrm.m_iDrillID == this.m_iDrillID)
                    {
                        circulateFrm.setChartActivite(true);
                        frm.Activate();
                        frm.WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        removeConsumer();
                        circulateFrm.Close();
                        createCirculateForm();
                    }

                    return;
                }
            }

            createCirculateForm();
        }

        /// <summary>
        /// 定向钻井 祝鹏辉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void createDirectionalForm()
        {
            DirectionalForm form = new DirectionalForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Location = new Point(0, 0);
            form.m_iDrillID = this.m_iDrillID;
            form.setDrillID(this.m_iDrillID);
            form.Show();
        }

        private void radBtnCtrlDrill_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DirectionalForm)
                {
                    DirectionalForm dirctionalFrm = (DirectionalForm)frm;

                    if (dirctionalFrm.m_iDrillID == this.m_iDrillID)
                    {
                        dirctionalFrm.setChartActivite(true);
                        frm.Activate();
                        frm.WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        removeConsumer();
                        dirctionalFrm.Close();
                        createDirectionalForm();
                    }
                    return;
                }
            }

            createDirectionalForm();
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

                    AlarmForm alarmFrm = (AlarmForm)frm;

                    if (alarmFrm.m_iDrillID == this.m_iDrillID)
                    {
                        alarmFrm.BringToFront();
                        frm.WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        alarmFrm.Close();
                        AlarmForm frmnew = new AlarmForm();
                        frmnew.Size = new System.Drawing.Size(1920, 1080);
                        frmnew.m_iDrillID = this.m_iDrillID;
                        frmnew.Show();
                    }

                    return;

                }
            }

            AlarmForm form = new AlarmForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Location = new Point(0, 0);
            form.m_iDrillID = this.m_iDrillID;
            form.Show();

        }

        public string getResetLableText()
        {
            return radLab_reset.Text;
        }

        public void setResetLableText(string strText)
        {
            radLab_reset.Text = strText;
        }

        public void setResetLable(bool bIsShow)
        {
            if (bIsShow)
            {
                radLab_reset.Visible = true;
                int iX = (this.Width - radLab_reset.Width) / 2;
                int iY = (this.Height - radLab_reset.Height) / 2;

                radLab_reset.Location = new Point(iX, iY);
                radLab_reset.BringToFront();
            }
            else
            {
                radLab_reset.Hide();
            }
        }

        private void formResetTags(bool bResetAll, int iDrillID)
        {
            bool bIsFirst = true;

            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;

                    if (!bResetAll && drillForm.m_iDrillID != iDrillID)
                    {
                        continue;
                    }


                    if (null != drillForm)
                    {
                        setResetLable(true);
                        drillForm.saveUserTagRef(bResetAll, bIsFirst);
                        drillForm.resetTags();
                        setResetLable(false);
                    }

                    bIsFirst = false;
                }
                else if (frm is CirculateForm)
                {
                    CirculateForm circulateForm = (CirculateForm)frm;

                    if (!bResetAll && circulateForm.m_iDrillID != iDrillID)
                    {
                        continue;
                    }

                    if (null != circulateForm)
                    {
                        setResetLable(true);
                        circulateForm.resetTags();
                        setResetLable(false);
                    }
                }
                else if (frm is DirectionalForm)
                {
                    DirectionalForm directionalForm = (DirectionalForm)frm;

                    if (!bResetAll && directionalForm.m_iDrillID != iDrillID)
                    {
                        continue;
                    }


                    if (null != directionalForm)
                    {
                        setResetLable(true);
                        directionalForm.resetTags();
                        setResetLable(false);
                    }
                }
            }
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
                if (frm is SettingForm2)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    return;
                }
            }

            SettingForm2 form = new SettingForm2();
            form.setDrillID(m_iDrillID);
            form.ShowDialog();

            if (form.getResetMark())
            {
                formResetTags(form.getResetAll(), form.getDrillID());
            }
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

            DrillingGasForm form = new DrillingGasForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();

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

            DrillPVTForm form = new DrillPVTForm();
            form.Size = new System.Drawing.Size(1920, 1080);
            form.Show();
        }

        public void setHistoryValue(HistoryDataForm hisForm)
        {
            hisForm.rpnl_info.Visible = false;
            hisForm.dataShow21.Visible = false;
            hisForm.dataShow22.Visible = false;
            hisForm.dataShow23.Visible = false;
            hisForm.dataShow24.Visible = false;
            hisForm.dataShow25.Visible = false;
        }

        private void rbtn_HistoryData_Click(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is HistoryDataForm)
                {
                    frm.Activate();
                    frm.WindowState = FormWindowState.Normal;
                    frm.Location = new Point(0, 0);

                    HistoryDataForm history = (HistoryDataForm)frm;
                    history.setHistoryValue();
                    history.clearHistoryCharts();
                    return;
                }
            }

            HistoryDataForm form = new HistoryDataForm();
            setHistoryValue(form);
            form.setBtnEnable(form, false);
            //form.setBtnLocation(form);

            form.Size = new System.Drawing.Size(1920, 1080);
            form.btn_pltDepath.Visible = false;
            form.btn_pltTime.Visible = false;
            form.Show();
        }

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


        //固定窗口
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
        //    {
        //        return;
        //    }

        //    base.WndProc(ref m);
        //}

        private void rbtn_exit_Click(object sender, EventArgs e)
        {
            modifyUserInfo(false);
            Process.GetCurrentProcess().Kill();
          
        }

        private void Activity_Click(object sender, EventArgs e)
        {
            //权限控制
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
            {
                return;
            }

            //弹窗
            ActivityStatusForm d = new ActivityStatusForm();
            d.defaultString = GetDefaultText();
            d.bsform = this;
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
            catch
            {
            }

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
            catch
            {
            }

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
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.RunWorkerAsync();
        }
        private void btn_zeroGL_Click(object sender, EventArgs e)
        {
            backgroundWorker4.WorkerSupportsCancellation = true;
            backgroundWorker4.RunWorkerAsync();
        }

        private void btn_hornIO_Click(object sender, EventArgs e)
        {
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
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                {
                    MessageBox.Show("Set Failed !");
                }
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
            catch
            {
                return "error";
            }
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
            try
            {
                ReciveMsg();//要返回2次，所以先执行一次再执行一次
                if (ReciveMsg() == "error")
                {
                    MessageBox.Show("Set Failed !");
                }
            }
            catch
            {
            }

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

        private void rbtn_return_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Map_Baidu"] != null)
            {
                Application.OpenForms["Map_Baidu"].Show();
                Application.OpenForms["Map_Baidu"].BringToFront();
            }
            else
            {
                Map_Baidu frm = new Map_Baidu();
                frm.Show();
            }
        }

        public string getContractor()
        {
            string strContractor = string.Empty;
            try
            {
                drill = _db.Drill.Where(O => O.ID == m_iDrillID).FirstOrDefault();
                if (null != drill)
                {
                    strContractor = drill.Contractor;
                }
            }
            catch
            {
            }

            return strContractor;
        }

        private void writeVideoConf(string strValue)
        {
            Configure conf = new Configure();
            conf.setFileName(AppDrill.videoConf);
            conf.WriteIniData("VideoCur", "wellName", ref strValue);
        }

        private void readVideoConf(ref string strValue)
        {
            Configure conf = new Configure();
            conf.setFileName(AppDrill.videoConf);
            conf.ReadIniData("CloseMark", "close", "0", ref strValue);
            bool bIsClose = conf.FindWind("VideoForm");
        }

        private void setVideoText()
        {
            Configure conf = new Configure();
            conf.setFileName(AppDrill.videoConf);
            string strValue = string.Empty;
            while (true)
            {
                Thread.Sleep(2000);

                conf.ReadIniData("CloseMark", "close", "0", ref strValue);
                if ("1" == strValue)
                {
                    if (rbtn_Video.InvokeRequired)
                    {
                        Action actionDelegate = () =>
                        {
                            if ("EN" == AppDrill.language)
                            {
                                this.rbtn_Video.Text = "Video";
                            }
                            else
                            {
                                this.rbtn_Video.Text = "视频";
                            }

                            this.rbtn_Video.BackColor = this.rbtn_Video.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
                        };

                        rbtn_Video.Invoke(actionDelegate);
                    }

                    break;
                }
            }
        }

        private void rbtn_vedio_Click(object sender, EventArgs e)
        {
            writeVideoConf(lbl_contractorvalue.Text);
            string strClose = string.Empty;
            readVideoConf(ref strClose);
            if (!string.IsNullOrEmpty(strClose) && "1" == strClose)
            {
                createVideoProcess();
                if ("EN" == AppDrill.language)
                {
                    this.rbtn_Video.Text = "Playing...";
                }
                else
                {
                    this.rbtn_Video.Text = "播放中...";
                }
                this.rbtn_Video.BackColor = Color.Green;
                Thread thread = new Thread(setVideoText);
                thread.IsBackground = true;
                thread.Start();
            }
            else
            {
                Map_Baidu.sendMessgeToVideoProcess(lbl_contractorvalue.Text, 0, 1);
            }
        }

        private void createVideoProcess()
        {
            try
            {
                Map_Baidu.startInfo.FileName = AppDrill.videoFileName;
                Map_Baidu.pro.StartInfo = Map_Baidu.startInfo;
                Map_Baidu.pro.Start();
            }
            catch
            {
            }
        }

        //以井队号向消息中间件订阅消息，接收实时数据
        public void createConsumer()
        {
            try
            {
                if (AppData.session != null)
                {
                    consumer = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(m_iDrillID.ToString()));
                }

                consumer.Listener += new MessageListener(consumer_Listener);
            }
            catch
            {
            }
        }

        //移除对消息中间的订阅
        public void removeConsumer()
        {
            try
            {
                consumer.Listener -= new MessageListener(consumer_Listener);
            }
            catch
            {
            }
        }

        private void consumer_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                {
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    RealTimeData realData = jsSerializer.Deserialize<RealTimeData>(msg.Body.ToString()); //反序列化
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage), realData);
                }
            }
            catch
            {
            }
        }

        public void ShowMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            try
            {
                dataShow(realTimeData);
                showFourChart(realTimeData);
                m_delegateDataShow(realTimeData);
                checkButtonShow(realTimeData);
            }
            catch
            {
            }
        }

        //根据数据库的值来初始化lable的文本值
        private void InitCheckTags()
        {
            InitCheckTagLable(label1, label1.Tag.ToString(), button1);
            InitCheckTagLable(label2, label2.Tag.ToString(), button2);
            InitCheckTagLable(label3, label3.Tag.ToString(), button3);
            InitCheckTagLable(label4, label4.Tag.ToString(), button4);
        }

        private void InitCheckTagLable(System.Windows.Forms.Label label, string strTag, Button btn)
        {
            m_dicCheckButton.Add(strTag, btn);
            var tag = listTag.Where(o => o.Tag == strTag).FirstOrDefault();
            if (tag != null)
            {
                label.Text = Transformation(strTag);
            }
        }

        public void setCheckTagShow(bool bShow)
        {
            m_bIsShowCheckTag = bShow;
        }

        //初始化字典
        private void InitDictionaryTags()
        {
            m_dicTags.Add(DATA_SHOW.tagOne, dataShow21);
            m_dicTags.Add(DATA_SHOW.tagTwo, dataShow22);
            m_dicTags.Add(DATA_SHOW.tagThree, dataShow23);
            m_dicTags.Add(DATA_SHOW.tagFour, dataShow24);
            m_dicTags.Add(DATA_SHOW.tagFive, dataShow25);
        }

        //显示是否启动自动送转
        private void checkButtonShow(RealTimeData realTimeData)
        {
            foreach (var term in m_dicCheckButton)
            {
                string strTag = term.Key;
                var button = term.Value;
                var tag = realTimeData.realTags.Where(o => o.Tag == strTag).FirstOrDefault();
                if (null != button && null != tag)
                {
                    if (1 == (int)tag.Value)
                    {
                        button.BackColor = Color.Lime;
                    }
                    else
                    {
                        button.BackColor = Color.Gray;
                    }
                }
            }
        }

        //显示BaseForm测点的实时数据
        private void dataShow(RealTimeData realTimeData)
        {
            foreach (var data in m_dicTags)
            {
                var dataShow = data.Value;
                if (null != dataShow)
                {
                    int iIndex = (int)data.Key;
                    var tag = realTimeData.realTags.Where(o => o.Tag == dataShow.DTag).FirstOrDefault();
                    if (null != tag)
                    {
                        double value = tag.Value;
                        string strBoxID = realTimeData.BoxId.ToString();
                        var newValue = Comm.UnitConversion(listTag, dataShow.DTag, strBoxID, value);
                        if ((bool)listTag.Where(o => o.Tag == dataShow.DTag).FirstOrDefault().IsBool)
                        {
                            dataShow.Value.Text = newValue > 0 ? "Yes" : "No";
                        }
                        else
                        {
                            dataShow.Value.Text = newValue.ToString("#0.00");
                        }

                        isAlarm(dataShow.DTag, strBoxID, newValue.ToString(), iIndex);
                    }
                }
            }
        }

        //显示时间轴和曲线
        private void showFourChart(RealTimeData realTimeData)
        {
            if (null != m_depthChart)
            {
                m_depthChart.ShowMessage(realTimeData);
            }

            if (null != m_fourChart1)
            {
                m_fourChart1.ShowMessage(realTimeData);
            }

            if (null != m_fourChart2)
            {
                m_fourChart2.ShowMessage(realTimeData);
            }

            if (null != m_fourChart3)
            {
                m_fourChart3.ShowMessage(realTimeData);
            }
        }

        //根据数据库保存datashow的数据，设置控件DataShow2的值
        private void setDataShow(UserControls.DataShow2 dataShow, int iIndex)
        {
            var userTag = UserTag.Find(o => o.Form == "BaseForm" && o.Group == 5 && o.Order == iIndex);
            if (null != userTag)
            {
                var tag = listTag.Where(o => o.Tag == userTag.Tag).FirstOrDefault();
                if (null != tag)
                {
                    dataShow.DTag = userTag.Tag;
                    dataShow.DSCaptial = Transformation(userTag.Tag);
                    setDataShowUnit(dataShow, tag.Unit);
                    dataShow.Value.Text = "###";
                    dataShow.SetTag();
                }
                else
                {
                    dataShow.DTag = "";
                    dataShow.Captial.Text = "";
                    dataShow.Unit.Text = "";
                    dataShow.Value.Text = "";
                }
            }
            else
            {
                dataShow.DTag = "";
                dataShow.Captial.Text = "";
                dataShow.Unit.Text = "";
                dataShow.Value.Text = "";
            }
        }

        //根据选择的测点值来更新要显示的DataShow2对象值
        private void setDataShow(UserControls.DataShow2 dataShow, EditTagForm form)
        {
            if (null != form && null != dataShow)
            {
                if (!string.IsNullOrEmpty(form.Captial))
                {
                    dataShow.DTag = form.Tags;
                    dataShow.DSCaptial = form.Captial;
                    dataShow.DSLValue = "L:" + form.LValue;
                    dataShow.DSHValue = "H:" + form.HValue;
                    setDataShowUnit(dataShow, form.Unit);
                    dataShow.SetTag();
                }
            }
        }

        //将DataShow2对象的值设置为空
        private void setDataShowEmpty(UserControls.DataShow2 dataShow)
        {
            dataShow.DSCaptial = "";
            dataShow.DSValue = "";
            dataShow.DSLValue = "";
            dataShow.DSHValue = "";
            dataShow.DSUnit = "";
            dataShow.DTag = "";
            dataShow.SetTag();
            dataShow.BackColor = Color.Black;
        }

        //根据数据库保存的默认值，设置控件DataShow2的值
        private void resetTag(UserControls.DataShow2 dataShow, int iIndex)
        {
            try
            {
                if (null == AppDrill.UserTag || AppDrill.UserTag.Count <= 0)
                {
                    return;
                }

                UserTag data = null;
                data = AppDrill.UserTag.Find(o => o.Form == "BaseForm" && o.Group == 5 && o.Order == iIndex);

                if (data != null)
                {
                    var taglist = listTag.Where(o => o.Tag == data.Tag).FirstOrDefault();
                    if (taglist != null)
                    {
                        dataShow.DTag = data.Tag;
                        dataShow.DSCaptial = Transformation(data.Tag);
                        setDataShowUnit(dataShow, taglist.Unit);
                        dataShow.Value.Text = "###";
                        dataShow.SetTag();
                    }
                }
            }
            catch
            {
            }
        }

        //初始化控件DataShow2的值
        private void InitDataShow(bool bIsReset = false)
        {
            foreach (var data in m_dicTags)
            {
                var dataShow = data.Value;
                int iIndex = (int)data.Key + 1;
                if (null != dataShow)
                {
                    if (bIsReset)
                    {
                        resetTag(dataShow, iIndex);
                    }
                    else
                    {
                        setDataShow(dataShow, iIndex);
                    }
                }
            }
        }

        //设置DataShow的单位
        private void setDataShowUnit(UserControls.DataShow2 dataShow, string strUnit)
        {
            if (null == dataShow)
            {
                return;
            }

            if (AppDrill.UnitFormat == "yz")
            {
                var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == strUnit);
                if (null != UnitModel)
                {
                    dataShow.DSUnit = UnitModel.UnitTo;
                }
                else
                {
                    dataShow.DSUnit = strUnit;
                }
            }
            else if (AppDrill.UnitFormat == "gz")
            {
                dataShow.DSUnit = strUnit;
            }
        }

        //设置按钮是否可以点击
        public void setBtnEnabled(RadButton btn, bool bIsEnable)
        {
            if (bIsEnable)
            {
                btn.Enabled = false;
            }
            else
            {
                btn.Enabled = true;
            }
        }
    }
}
