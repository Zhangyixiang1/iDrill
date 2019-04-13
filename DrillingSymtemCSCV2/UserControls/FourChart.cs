using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using DrillingSymtemCSCV2.Forms;
using DrillingSymtemCSCV2.Model;
using Microsoft.VisualBasic.PowerPacks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class FourChart : UserControl
    {
        //用户当前是否正在查看历史数据
        private bool ViewHistory = false;
        public int group { get; set; }
        public string fname { get; set; }
        private Color _BACK_COLOR = Color.Black; 
        private double _YCount = 1800;
        //页面缓存点个数,程序默认一天，后从appconfig读取配置
        private int DataShowCnt = 86400;
        //当前可见点个数
        private int VisiableCnt = 0;
        Point m_ptLocation;
        string m_strTxt = "";
        Point m_ptTemp;

        private GraphPane myPane;
        // 起始时间以毫秒为单位
        //int tickStart = 0;
        //测点数据
        public static List<DrillTag> listTag;
        private List<UserTag> UserTag;
        DrillOSEntities _db;
        private List<JsonAlarm> m_jsonAlarmList = null;
        
        
        //是否显示Y轴
        public bool IsShowY = false;
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表

        //定义4个List用于存放chart数据
        private List<ChartPoint> chart1 = new List<ChartPoint>(43200);
        private List<ChartPoint> chart2 = new List<ChartPoint>(43200);
        private List<ChartPoint> chart3 = new List<ChartPoint>(43200);
        private List<ChartPoint> chart4 = new List<ChartPoint>(43200);
        
        private  bool m_bisReceive = false;
        private double m_dDepthMax = 0.0;
        private double m_dDepthMin = 0.0;
        private double m_dTimeMax = 0.0;
        private double m_dTimeMin = 0.0;
        private int m_iTimeStep = 0;
        private double m_dBaseTime = 0.0;
        private int m_iDrillID = 0;
        private bool m_bShowTip = false;
        public bool isActive = false;//判断当前窗体是否激活
        delegate void updateChart();//使用委托更新曲线

        //定义全局变量，存放chart点数据
        private LineItem curve1, curve2, curve3, curve4;
        public double timeNow = 0;
        Dictionary<DATA_SHOW, DataShow> m_dicDataShow = new Dictionary<DATA_SHOW, DataShow>();
        public FourChart()
        {
            DateTime dt = DateTime.Now;


            if (listTag == null)
            {
                listTag = new List<DrillTag>();
            }

            UserTag = new List<UserTag>();

            VisiableCnt = (int)_YCount;
            InitializeComponent();
            zed1.PointValueEvent += new ZedGraphControl.PointValueHandler(Point_SetFormatEvent);
            SetDataShowClick();
        }
        private void clearChart(int iIndex)
        {
            switch (iIndex)
            {
                case 1:
                    chart1.Clear();
                    break;
                case 2:
                    chart2.Clear();
                    break;
                case 3:
                    chart3.Clear();
                    break;
                case 4:
                    chart4.Clear();
                    break;
                default:
                    break;
            }
        }
	
        public void resetDataShow()
        {
            InitDataShow(true);
        }

        public void setTagValue(string strValue)
        {
            dataShow1.setTagValue(strValue);
            dataShow2.setTagValue(strValue);
            dataShow3.setTagValue(strValue);
            dataShow4.setTagValue(strValue);
        }

        public void setDrillID(int iDrillID)
        {
            m_iDrillID = iDrillID;
        }

        public void setVeiwHistroy(bool bVeiwHistroy)
        {
            ViewHistory = bVeiwHistroy;
        }

        public void setTimeStep(int iTimeStep)
        {
            m_iTimeStep = iTimeStep;
        }

        public void setTimeMaxMin(double dTimeMin, double dTimeMax)
        {
            m_dTimeMin = dTimeMin;
            m_dTimeMax = dTimeMax;
        }

        private void setChartPoint(List<TagValue> tags, List<ChartPoint> cpList)
        {
            foreach (var item in tags)
            {
                ChartPoint cp = new ChartPoint();
                cp.dateTime = item.Timestamp;
                cp.value = item.Value;
                cpList.Add(cp);
            }
        }

        public void setFourChart(List<TagValue> tags, int iIndex)
        {
            switch(iIndex)
            {
                case 1:
                    setChartPoint(tags, chart1);
                    break;
                case 2:
                    setChartPoint(tags, chart2);
                    break;
                case 3:
                    setChartPoint(tags, chart3);
                    break;
                case 4:
                    setChartPoint(tags, chart4);
                    break;
                default:
                    break;
            }
        }

        private void ClearList(LineItem curve)
        {
            if (curve != null)
            {
                IPointListEdit list = curve.Points as IPointListEdit;

                if (list != null)
                {
                    list.Clear();
                }
            }

            zed1.AxisChange();
            zed1.Invalidate();
           
        }

        public void ClearPointList()
        {
            ClearList(curve1);
            ClearList(curve2);
            ClearList(curve3);
            ClearList(curve4);
        }

        public void ClearChart()
        {
            if (null != chart1)
            {
                chart1.Clear();
            }

            if (null != chart2)
            {
                chart2.Clear();
            }

            if (null != chart3)
            {
                chart3.Clear();
            }

            if (null != chart4)
            {
                chart4.Clear();
            }
        }

        public void setReceive(bool bReceive)
        {
            m_bisReceive = bReceive;
        }

        public void setDepthMaxmin(bool bisSet)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            if (bisSet)
            {
                yScale.Max = m_dDepthMax;
                yScale.Min = m_dDepthMin;
            }
            else
            {
                 m_dDepthMax = yScale.Max;
                 m_dDepthMin = yScale.Min;
            }

        }

        public void setTimeMaxmin(double dlastDepath, int iNum)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            yScale.Max = dlastDepath;
            yScale.Min = dlastDepath - iNum;
        }

        public void setTimeMaxmin(long lMin, long lMax)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            yScale.Max = lMax;
            yScale.Min = lMin;
            myPane.YAxis.Scale.MajorStep = m_iTimeStep;
            myPane.XAxis.Scale.Min = -0.5;
            myPane.XAxis.Scale.Max = 101;                               
            myPane.XAxis.Scale.MajorStep = 10;
            zed1.AxisChange();
        }

        public void setViewHistory(bool bviewHsty)
        {
            ViewHistory = bviewHsty;
        }

        public void setMajorStep(double bmajorStep)
        {
            myPane.YAxis.Scale.MajorStep = bmajorStep;
        }


        /// <summary>
        /// 初期加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FourChart_Load(object sender, EventArgs e)
        {
            
            _db = new DrillOSEntities();
            //CreateListener();

            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion

            try
            {
                //获取用户配置页面缓存点个数
                DataShowCnt = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DataShowCnt"].ToString());
            }
            catch { }

            // *** BEGIN 控件整体设置 ***
            zed1.IsEnableZoom = false;
            zed1.IsEnableVZoom = false;
            zed1.IsEnableHZoom = false;
            zed1.IsShowContextMenu = false; //禁用右键弹出菜单

            // zed1.IsShowPointValues = true;    //显示网格上对应值
            //zed1.IsZoomOnMouseCenter = true;  //加上这个鼠标中间可以缩放，但是有问题
            zed1.BackColor = _BACK_COLOR;
            zed1.BorderStyle = BorderStyle.None;
            // *** END 控件整体值显示 ***

            // *** BEGIN myPane设置 ***
            myPane = this.zed1.GraphPane;
            myPane.Fill = new Fill(_BACK_COLOR);                         //chart外框背景颜色
            myPane.Border.IsVisible = false;                             //chart图边框不显示
            myPane.Title.IsVisible = false;                              //chart图抬头不显示
            myPane.Chart.Fill = new Fill(_BACK_COLOR);                   //chart内容绘制区域颜色
            myPane.Chart.Border.IsVisible = false;                       //chart曲线边框不显示
            // *** END myPane设置 ***

            // *** BEGIN X轴设置 ***
            myPane.XAxis.Cross = 0.0d;                                    //X轴交叉刻度
            myPane.XAxis.CrossAuto = true;
            myPane.XAxis.Title.IsVisible = false;                         //X轴不显示抬头
            myPane.XAxis.Color = Color.DarkGray;                          //X轴颜色
            myPane.XAxis.Scale.IsVisible = true;                          //X轴显示
            myPane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;    //X轴字体颜色
            myPane.XAxis.MajorGrid.IsVisible = true;                      //X轴显示网格
            myPane.XAxis.MajorGrid.Color = Color.Gray;                    //X轴网格颜色                  
            //X轴最小值0
            myPane.XAxis.Scale.IsVisible = false;                         //设置X轴数值不可见
            myPane.XAxis.Scale.Min = -0.5;
            myPane.XAxis.Scale.Max = 101;                                  //X轴最大60
            //myPane.XAxis.Scale.MajorStepAuto = true;                      //X轴显示步长
            myPane.XAxis.Scale.FontSpec.IsUnderline = false;
            myPane.XAxis.Scale.MajorStep = 10;                             //X轴小步长1,也就是小间隔
            // *** END X轴设置 ***

            // *** BEGIN Y轴设置 ***
            myPane.YAxis.Title.IsVisible = false;                         //Y轴不显示抬头
            myPane.YAxis.Color = Color.Transparent;                       //Y轴颜色
            myPane.YAxis.Scale.IsVisible = IsShowY;                  //Y轴不显示
         //   myPane.YAxis.Scale.IsVisible = true;
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.AlignH = AlignH.Right;
            myPane.YAxis.Scale.FontSpec.Size = 24;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;    //Y轴字体颜色
            myPane.YAxis.MajorGrid.IsVisible = true;                      //Y轴显示网格
            myPane.YAxis.MajorGrid.Color = Color.Gray;                    //Y轴网格颜色

            myPane.YAxis.Scale.Max = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;              //Y轴从0开始,这个地方要影响X轴的显示
            myPane.YAxis.Scale.Min = myPane.YAxis.Scale.Max - _YCount;
            myPane.YAxis.Scale.MajorStep = (myPane.YAxis.Scale.Max - myPane.YAxis.Scale.Min) / 10;                         //X轴大步长，也就是显示文字的大间隔
            m_dBaseTime = myPane.YAxis.Scale.Max;//(myPane.YAxis.Scale.Max + myPane.YAxis.Scale.Min) / 2;

  
            myPane.YAxis.Scale.IsReverse = true;                          //从上到下画线
            // *** BEGIN Y轴设置 ***

            // *** BEGIN 四条曲线设置 ***
            //设置86400个点,假设每1秒更新一次,刚好检测1天,一旦构造后将不能更改这个值
            RollingPointPairList list1 = new RollingPointPairList(DataShowCnt);
            RollingPointPairList list2 = new RollingPointPairList(DataShowCnt);
            RollingPointPairList list3 = new RollingPointPairList(DataShowCnt);
            RollingPointPairList list4 = new RollingPointPairList(DataShowCnt);

            //开始，增加的线是没有数据点的(也就是list为空)  
            //★★★增加一条名称:Voltage，颜色Color.Bule，无符号，无数据的空线条
            curve1 = myPane.AddCurve("", list1, Color.FromArgb(0, 255, 0), SymbolType.None);

            //curve.Line.IsSmooth = true;    //平滑曲线
            curve1.Line.SmoothTension = 0.6F;
            curve1.Line.Width = 2;

            //★★★第二条曲线
            curve2 = myPane.AddCurve("", list2, Color.FromArgb(0, 255, 255), SymbolType.None);
            //curve2.Line.IsSmooth = true;    //平滑曲线
            curve2.Line.SmoothTension = 0.6F;
            curve2.Line.Width = 2;

            //★★★第三条曲线
            curve3 = myPane.AddCurve("", list3, Color.FromArgb(255, 165, 0), SymbolType.None);
            //curve3.Line.IsSmooth = true;    //平滑曲线
            curve3.Line.SmoothTension = 0.6F;
            curve3.Line.Width = 2;

            //★★★第四条曲线
            curve4 = myPane.AddCurve("", list4, Color.FromArgb(255, 255, 0), SymbolType.None);
            //curve4.Line.IsSmooth = true;    //平滑曲线
            curve4.Line.SmoothTension = 0.6F;
            curve4.Line.Width = 2;
            // *** END 四条曲线设置 ***

            //Y轴显示的文本格式化
            if (IsShowY)
            {
                zed1.GraphPane.YAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(YAxis_ScaleFormatEvent);
            }

            //改变轴的刻度
            zed1.AxisChange();
            //保存开始时间
            //tickStart = Environment.TickCount;
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                InitDictionary();

                if (AppDrill.UnitFormat == "-1")
                {
                    AppDrill.UnitFormat = System.Configuration.ConfigurationManager.AppSettings["System"].ToString();
                }

                if (AppData.UnitTransfer == null)
                {
                    AppData.UnitTransfer = new List<UnitTransfer>();
                    AppData.UnitTransfer = _db.UnitTransfer.ToList();
                }

                list_tagdir = _db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据

                if (listTag.Count == 0)
                //0703修改，drilltag用同一个
                {
                    listTag = _db.DrillTag.Where(o => o.DrillId == 1).ToList();
                }

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == m_iDrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                var userTagRef = _db.UserTagRef.Where(o => o.DrillId == 1 && o.Username == AppDrill.username).FirstOrDefault();
                {
                    if (userTagRef.JsonAlarm != null)
                    {
                        m_jsonAlarmList = new JavaScriptSerializer().Deserialize<List<JsonAlarm>>(userTagRef.JsonAlarm);
                    }
                }
            }
            catch 
            { 
            }
        }

        public void InvalidateChart()
        {
            UpdateFourChart();
            zed1.AxisChange();
            zed1.Invalidate();
        }

        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InitDataShow();
            InvalidateChart();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 曲线图鼠标事件

        /// <summary>
        /// 鼠标移动到点上时要表示的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="pane"></param>
        /// <param name="curve"></param>
        /// <param name="iPt"></param>
        /// <returns></returns>
        private string Point_SetFormatEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
         {
            string colorName = curve.Color.Name;
            //通过颜色判断哪条chart
            double range = 1;
            double from = 0;
            switch (colorName)
            {
                case "Lime":
                    range = getTagRange(1);//第一条
                    from = double.Parse(dataShow1.From.Text);
                    break;
                case "LightSkyBlue":
                    range = getTagRange(2);//第二条
                    from = double.Parse(dataShow2.From.Text);
                    break;
                case "Pink":
                    range = getTagRange(3);//第三条
                    from = double.Parse(dataShow3.From.Text);
                    break;
                case "Red":
                    range = getTagRange(4);//第四条
                    from = double.Parse(dataShow4.From.Text);
                    break;
            }

            double xVlaue = curve.Points[iPt].X;
            double yVlaue = curve.Points[iPt].Y;
            string strValue = "";

            yVlaue *= 1000;
            strValue = Comm.ConvertIntDateTime(yVlaue).ToString() + "，" + (xVlaue * range / 100 + from).ToString();
   
            return strValue;
        }

        /// <summary>
        /// 放大缩小事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        private void zed1_ZoomEvent(ZedGraphControl sender, ZoomState oldState, ZoomState newState)
        {
            var start = sender.GraphPane.YAxis.Scale.Min;
            var end = sender.GraphPane.YAxis.Scale.Max;
            _YCount = end - start;
            //sender.GraphPane.YAxis.Scale.Min = 0;
            //zed1.Invalidate();
        }

        #endregion

        #region 历史数据查看  Add by ZAY in 2017.5.4

        /// <summary>
        /// 查看历史向上翻页（在使用该控件(FourChart)的主页面中，Name.UpClick(Num) 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        private void UpClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            ViewHistory = true;
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;
            m_dBaseTime = yScale.Max; //(yScale.Max + yScale.Min) / 2;
            UpdateFourChart();//重新获取点的个数

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void queryHis()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            ViewHistory = true;
            m_dBaseTime = yScale.Max; //(yScale.Max + yScale.Min) / 2;
            UpdateFourChart();//重新获取点的个数
            myPane.YAxis.Scale.MajorStep = m_iTimeStep;
            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void UpClick()
        {
            UpClickByTime();
        }

        /// <summary>
        /// 查看历史向下翻页（在使用该控件(FourChart)的主页面中，Name.DownClick(Num) 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        private void DownClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            ViewHistory = true;
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;
            m_dBaseTime = yScale.Max;
            UpdateFourChart();//重新获取点的个数

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void DownClick()
        {
            DownClickByTime();
        }

        /// <summary>
        /// 当用户点击上下翻页查看数据时，曲线图会停止刷新，此事件用于重新开启实时刷新（在使用该控件(FourChart)的主页面中，Name.RealTimeClick() 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        private void RealTimeClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            if (ViewHistory)
            {
                if (timeNow <= 0)
                {
                    double dnow = (double)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
                    yScale.Max = dnow;
                    yScale.Min = dnow - (m_iTimeStep * 2);
                    yScale.MajorStep = (m_iTimeStep * 2) / 10;
                }
                else
                {
                    yScale.Max = timeNow;
                    yScale.Min = timeNow - (m_iTimeStep * 2);
                    myPane.YAxis.Scale.MajorStep = m_iTimeStep * 2 / 10;
                }

                m_dBaseTime = yScale.Max;

                UpdateFourChart();//重新获取点的个数
                  
                zed1.AxisChange();
                zed1.Invalidate();
                ViewHistory = false;
            }
            else
            {
                ViewHistory = true;
            }
        }

        public void RealTimeClick()
        {
             RealTimeClickByTime();
        }

        /// <summary>
        /// 放大
        /// </summary>
        private void EnlargeByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;
            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
            ViewHistory = true;
            UpdateFourChart();//重新获取点的个数

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void Enlarge()
        {
            EnlargeByTime();
        }
        /// <summary>
        /// 缩小
        /// </summary>
        private void NarrowByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;

            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
            ViewHistory = true;
            UpdateFourChart();//重新获取点的个数

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void Narrow()
        {
            NarrowByTime();
        }
        #endregion

        #region DataShow点击事件  Add by ZAY in 2017.5.5
        //订阅点击事件
        public void SetDataShowClick()
        {
            dataShow1.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.Captial.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.lineShape1.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.From.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.Unit.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.To.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow1.Value.MouseClick += new MouseEventHandler(dataShow1_MouseClick);

            dataShow2.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.Captial.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.lineShape1.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.From.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.Unit.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.To.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow2.Value.MouseClick += new MouseEventHandler(dataShow2_MouseClick);

            dataShow3.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.Captial.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.lineShape1.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.From.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.Unit.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.To.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow3.Value.MouseClick += new MouseEventHandler(dataShow3_MouseClick);

            dataShow4.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.Captial.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.lineShape1.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.From.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.Unit.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.To.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow4.Value.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
        }

        private void updateUserTag()
        {
            try
            {
                //更新UserTag
                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == m_iDrillID && o.Username == AppDrill.username).FirstOrDefault();

                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }
            }
            catch 
            { 
            }
        }

        private void setDataShowValue(DataShow dataShow, EditTagForm form, List<ChartPoint> chart)
        {
            string oldTag = dataShow.DSCaptal;//保存之前的测点
            string oldFrom = dataShow.DSRangeFrom;//保存之前的测点量程
            string oldTo = dataShow.DSRangeTo;

            if (!string.IsNullOrEmpty(form.Captial))
            {
                if (!string.IsNullOrEmpty(form.From))
                {
                    var from = Comm.UnitConversion(listTag, form.Captial, m_iDrillID.ToString(), Convert.ToDouble(form.From));
                    dataShow.DSRangeFrom = from.ToString();
                }

                if (!string.IsNullOrEmpty(form.To))
                {
                    var to = Comm.UnitConversion(listTag, form.Captial, m_iDrillID.ToString(), Convert.ToDouble(form.To));
                    dataShow.DSRangeTo = to.ToString();
                }

               
                if (AppDrill.UnitFormat == "yz")
                {
                    var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == form.Unit);
                    if (UnitModel != null)
                        dataShow.DSunit = UnitModel.UnitTo;
                    else
                        dataShow.DSunit = form.Unit;
                }
                else if (AppDrill.UnitFormat == "gz")
                {
                    dataShow.DSunit = form.Unit;
                }

                dataShow.DSCaptal = form.Captial;
                dataShow.DSLValue = form.LValue;
                dataShow.DSHValue = form.HValue;
                dataShow.SetTag();

                if (!string.IsNullOrEmpty(oldFrom) && !string.IsNullOrEmpty(oldTo))
                {
                    Clear(form.order, double.Parse(oldFrom), double.Parse(oldTo), double.Parse(dataShow.DSRangeFrom), double.Parse(dataShow.DSRangeTo), oldTag != dataShow.DSCaptal ? true : false);
                }

                if (oldTag != dataShow.DSCaptal)
                {
                    chart.Clear();
                }
            }
        }

        private void getTagData(string strTag, int iIndex)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            string PostData = null;
            string PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();

            QueryHistory model = new QueryHistory();
            model.startTime = (long)yScale.Min;
            model.endTime = (long)yScale.Max;
            model.DrillId = m_iDrillID;
            model.DepthTag = "var2";
            model.isHistoryData = false;

            List<string> tag = new List<string>();
            tag.Add(strTag);
            model.Tag = tag;

            PostData = new JavaScriptSerializer().Serialize(model);

            var QueryData = Comm.HttpPost(PostUrl, PostData);

            if (!string.IsNullOrEmpty(QueryData))
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                jsSerializer.MaxJsonLength = Int32.MaxValue;
                getHistoryData dataList = jsSerializer.Deserialize<getHistoryData>(QueryData); //反序列化

                if (dataList.datas.Count <= 0)
                {
                    return;
                }

                setFourChart(dataList.datas[0].Datas, iIndex);
                UpdateFourChart(iIndex);
            }
        }

        private void dataShow1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                EditTagForm form = new EditTagForm();
                if (e.Button.ToString() != "Right")
                {
                    form.group = group;
                    form.order = 1;
                    form.formName = fname;
                    form.DrillId = m_iDrillID.ToString();
                    form.ShowDialog();

                    //在界面上删除该点
                    if (form.remove) 
                    {
                        setEmptyDataShow(dataShow1);
                        dataShow1.SetTag();
                        //删除的时候需要将报警状态初始化
                        dataShow1.BackColor = Color.Black;
                        ClearLine(1);
                        chart1.Clear();
                    }
                    else
                    {
                        setDataShow(dataShow1, form);
                    }
                }
            }
            catch 
            { 
            }
        }

        private void dataShow2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                EditTagForm form = new EditTagForm();
                if (e.Button.ToString() != "Right")
                {
                    form.group = group;
                    form.order = 2;
                    form.formName = fname;
                    form.DrillId = m_iDrillID.ToString();
                    form.ShowDialog();

                    //在界面上删除该点
                    if (form.remove) 
                    {
                        setEmptyDataShow(dataShow2);
                        dataShow2.SetTag();
                        //删除的时候需要将报警状态初始化
                        dataShow2.BackColor = Color.Black;
                        ClearLine(2);
                        chart2.Clear();
                    }
                    else
                    {
                        setDataShow(dataShow2, form);
                    }
                }
            }
            catch 
            { 
            }
        }

        private void dataShow3_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                EditTagForm form = new EditTagForm();
                if (e.Button.ToString() != "Right")
                {
                    form.group = group;
                    form.order = 3;
                    form.formName = fname;
                    form.DrillId = m_iDrillID.ToString();
                    form.ShowDialog();

                    //在界面上删除该点
                    if (form.remove)
                    {
                        setEmptyDataShow(dataShow3);
                        dataShow3.SetTag();
                        //删除的时候需要将报警状态初始化
                        dataShow3.BackColor = Color.Black;
                        ClearLine(3);
                        chart3.Clear();
                    }
                    else
                    {
                        setDataShow(dataShow3, form);
                    }
                }
            }
            catch 
            { 
            }
        }

        private void dataShow4_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                EditTagForm form = new EditTagForm();
                if (e.Button.ToString() != "Right")
                {
                    form.group = group;
                    form.order = 4;
                    form.formName = fname;
                    form.DrillId = m_iDrillID.ToString();
                    form.ShowDialog();

                    //在界面上删除该点
                    if (form.remove)
                    {
                        setEmptyDataShow(dataShow4);
                        dataShow4.SetTag();
                        //删除的时候需要将报警状态初始化
                        dataShow4.BackColor = Color.Black;
                        ClearLine(4);
                        chart4.Clear();
                    }
                    else
                    {
                        setDataShow(dataShow4, form);
                    }                  
                }
            }
            catch 
            { 
            }
        }
        #endregion

        private void setDataShowDS(DataShow dataShow, string Value)
        {
            if (Value == "Yes" || Value == "No")
            {
                dataShow.DSvalue = Value;
            }
            else
            {
                dataShow.DSvalue = double.Parse(Value).ToString("#0.00");
            }

            dataShow.SetTag();
        }

        #region DataShow设值  Add by ZAY in 2017.5.5
        private void csDataShow(string Value, int Index)
        {
            switch(Index)
            {
                case 1:
                    setDataShowDS(dataShow1, Value);
                    break;
                case 2:
                    setDataShowDS(dataShow2, Value);
                    break;
                case 3:
                    setDataShowDS(dataShow3, Value);
                    break;
                case 4:
                    setDataShowDS(dataShow4, Value);
                    break;
                default:
                    break;
            }
        }
        #endregion

        /// <summary>
        /// Y轴显示格式
        /// </summary>
        /// <returns></returns>
        private string YAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            //根据 val值 返回你需要的 string  
            //return Comm.ConvertIntDateTime(val).ToString();
            val *= 1000;
            return Comm.ConvertIntDateTime(val).ToString();
        }

        //刷新chart曲线
        public void zedInvalidate(long lNewValue)
        {
            if (isActive)
            {
                Scale yScale = zed1.GraphPane.YAxis.Scale;

                if (!ViewHistory)
                {
                    VisiableCnt = (int)(yScale.Max - yScale.Min);
                    double distance = lNewValue - yScale.Max;

                    yScale.Min = yScale.Max - VisiableCnt + distance;
                    yScale.Max = lNewValue;

                    if (distance >= 0)
                    {
                        double Num = yScale.Max - yScale.Min;
                        yScale.MajorStep = Num / 10;
                    }


                    zed1.AxisChange();
                    zed1.Invalidate();
                }
            }
        }

        #region 报警闪烁  ADD by ZAY in 2017.5.25

        /// <summary>
        /// 判断是否报警
        /// </summary>

        private void setDataShowBackColor(int iIndex, int iSetColor)
        {
            switch (iIndex)
            {
                case 1:
                    dataShow1.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 2:
                    dataShow2.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 3:
                    dataShow3.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 4:
                    dataShow4.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    //dataShow4.Value.ForeColor = 0 == iSetColor ? Color.White : Color.Red;
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

            if (null != m_jsonAlarmList)
            {
                jsonAlarm = m_jsonAlarmList.Where(o => o.Tag == strTag).FirstOrDefault();
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

                if (model != null || null != JsonAlarm)
                {
                    if (!string.IsNullOrEmpty(Value))
                    {
                        AlarmModel old_am = Alarms.list_Alarms.Find(o => o.Tag == Tag);
                        string alarmcode = "";
                        bool isAlarm = isNeedAlarm(model, JsonAlarm, Value,ref alarmcode);
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
                                using (_db = new Model.DrillOSEntities())
                                {
                                    AlarmHis item = new AlarmHis();
                                    item.DrillID = Dirrl;
                                    item.UserName = AppDrill.username;
                                    item.Code = alarmcode;
                                    item.Time = DateTime.Now;
                                    _db.AlarmHis.Add(item);
                                    _db.SaveChanges();
                                }
                            }
                            
                            setDataShowBackColor(Index, 0);
                        }
                        else
                        {
                            if (old_am != null)
                            {
                                //if (old_am.status != 1)
                                //{
                                //    Alarms.list_Alarms.Remove(old_am);//已经不报警了,移除
                                //    //状态不等于1的操作数据库
                                //    var alarm = _db.AlarmHistory.Where(o => o.Tag == old_am.Tag && o.RecoveryTime == null);
                                //    if (alarm != null)
                                //    {
                                //        foreach (var a in alarm)
                                //        {
                                //            a.RecoveryTime = DateTime.Now;
                                //        }

                                //        backgroundWorker2.WorkerSupportsCancellation = true;
                                //        backgroundWorker2.RunWorkerAsync();//开启异步保存
                                //    }
                                //}

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

        #region 获取每一个fourchart上的测点范围
        private void getRange(DataShow dataShow, ref double range)
        {
            if (!string.IsNullOrEmpty(dataShow.From.Text) && !string.IsNullOrEmpty(dataShow.To.Text))
            {
                range = ((double.Parse(dataShow.To.Text) - double.Parse(dataShow.From.Text))) == 0 ? 1 : (double.Parse(dataShow.To.Text) - double.Parse(dataShow.From.Text));//如果范围为0的话，强制设置为1
            }
            else
            {
                range = 1;
            }
        }

        private double getTagRange(int Tag)
        {
            double range = 0;

            switch (Tag)
            {
                case 1:
                    getRange(dataShow1, ref range);
                    break;
                case 2:
                    getRange(dataShow2, ref range);
                    break;
                case 3:
                    getRange(dataShow3, ref range);
                    break;
                case 4:
                    getRange(dataShow4, ref range);
                    break;
            }

            return range;
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

        public void showToolTip()
        {
            if (!m_bShowTip)
            {
                return;
            }

            double dTimestamp;//时间戳
            double dUseless;//占位
            GraphPane pane = zed1.GraphPane;
            pane.ReverseTransform(m_ptLocation, out dUseless, out dTimestamp);
            m_strTxt = GetPointValue(dTimestamp);
            toolTip1.SetToolTip(zed1, m_strTxt);
        }

        private void zed1_MouseMove(object sender, MouseEventArgs e)
        {
            GraphPane pane = zed1.GraphPane;
            if (m_ptTemp.Y != e.Y)
            {
                zed1.Invalidate();
                pane.Chart.pt.X = e.X;
                pane.Chart.pt.Y = e.Y;
                m_ptLocation = new Point(e.X, e.Y);
                double timestamp;//时间戳
                double useless;//占位
                pane.ReverseTransform(m_ptLocation, out useless, out timestamp);
                m_strTxt = GetPointValue(timestamp);
                toolTip1.SetToolTip(zed1, m_strTxt);
                m_bShowTip = true;
            }

            m_ptTemp.X = e.X;
            m_ptTemp.Y = e.Y;
        }
        #endregion


        private double UnitConversion(string Tag, string DrillId, double value)
        {
            try
            {
                if (AppDrill.UnitFormat == "yz")
                {
                    var Dirrl = 0;
                    if (!string.IsNullOrEmpty(DrillId))
                        Dirrl = int.Parse(DrillId);
                    var TagModel = listTag.Find(o => o.DrillId == Dirrl && o.Tag == Tag);
                    if (TagModel != null)
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == TagModel.Unit);
                        if (UnitModel != null)
                        {
                            value = value * (double)UnitModel.Coefficient;
                            return Math.Round(value, 2);
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

        public void Clear(int lineNo, double oldFrom, double oldTo, double newFrom, double newTo, bool isNewTag)
        {
            //取Graph第一个曲线，也就是第一步:在GraphPane.CurveList集合中查找CurveItem
            LineItem curve = zed1.GraphPane.CurveList[lineNo - 1] as LineItem;
            double from = double.Parse(dataShow1.From.Text);
            if (curve == null)
            {
                return;
            }
            IPointListEdit list = curve.Points as IPointListEdit;
            if (list == null)
            {
                return;
            }
            if (isNewTag)
            {
                list.Clear();
            }
            else
            {
                for (int i = 0; i < curve.Points.Count; i++)
                {
                    curve.Points[i].X = ((curve.Points[i].X * (oldTo - oldFrom) / 100 + oldFrom) - newFrom) / getTagRange(lineNo) * 100;
                }
            }
            zed1.AxisChange();
            zed1.Invalidate();
        }

        private void zed1_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Active = false;
            zed1.GraphPane.Chart.isShowCursorLine = false;
            zed1.Invalidate();
            m_bShowTip = false;
        }

        private void zed1_MouseEnter(object sender, EventArgs e)
        {
            zed1.GraphPane.Chart.isShowCursorLine = true;
            toolTip1.Active = true;
        }

        public void ClearLine(int lineNo)
        {
            LineItem curve = zed1.GraphPane.CurveList[lineNo - 1] as LineItem;
            IPointListEdit list = curve.Points as IPointListEdit;
            list.Clear();
            //第三步:调用ZedGraphControl.AxisChange()方法更新X和Y轴的范围
            zed1.AxisChange();
            //第四步:调用Form.Invalidate()方法更新图表
            zed1.Invalidate();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _db.SaveChanges();
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker2.CancelAsync();
        }

        private void addListPoint(LineItem curve, List<ChartPoint> chart, DataShow dataShow, int iIndex)
        {
            try
            {
                if (curve != null)
                {
                    var pList = curve.Points as IPointListEdit;
                    if (pList != null)
                    {
                        pList.Clear();
                    }

                    for (int i = 0; i < chart.Count; i++)
                    {
                        pList.Add((chart[i].value - double.Parse(dataShow.From.Text)) / getTagRange(iIndex) * 100, chart[i].dateTime);
                    }
                }
            }
            catch
            {
            }
        }

        public void UpdateFourChart()
        {
            addListPoint(curve1, chart1, dataShow1, 1);
            addListPoint(curve2, chart2, dataShow2, 2);
            addListPoint(curve3, chart3, dataShow3, 3);
            addListPoint(curve4, chart4, dataShow4, 4);
        }

        public void UpdateFourChart(int iIndex)
        {
            switch(iIndex)
            {
                case 1:
                    addListPoint(curve1, chart1, dataShow1, 1);
                break;

                case 2:
                    addListPoint(curve2, chart2, dataShow2, 2);
                break;

                case 3:
                    addListPoint(curve3, chart3, dataShow3, 3);
                break;

                case 4:
                    addListPoint(curve4, chart4, dataShow4, 4);
                break;
                default:
                break;
            }
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


        private double getX(double dLineX, string strFrom, int iIndex)
        {
            double dX = 0;
            try
            {
                dX = dLineX / 100 * getTagRange(iIndex) + Convert.ToInt16(strFrom);
            }
            catch
            {
                return dX;
            }

            return dX;
        }

        private string getXValue(IPointListEdit line, double dTimestap, string strText, int iIndex)
        {
            bool bIsFind = false;
            string strXValue = string.Empty;
            for (int i = 0; i < line.Count; ++i)
            {
                double dY = line[i].Y;
                if (dY >= dTimestap)
                {
                    double dX = getX(line[i].X, strText, iIndex);
                    strXValue = dX.ToString();
                    bIsFind = true;
                    break;
                }
            }

            if (!bIsFind)
            {
                strXValue = "NA";
            }

            return strXValue;
        }

        private string GetPointValue(double timestamp)
        {
            string strValue = " ";
            try
            {
                string[] ParaDisplay = new string[4];
                IPointListEdit line1 = zed1.GraphPane.CurveList[0].Points as IPointListEdit;
                IPointListEdit line2 = zed1.GraphPane.CurveList[1].Points as IPointListEdit;
                IPointListEdit line3 = zed1.GraphPane.CurveList[2].Points as IPointListEdit;
                IPointListEdit line4 = zed1.GraphPane.CurveList[3].Points as IPointListEdit;

                //取得当前时间点对应的参数值
                try
                {
                    ParaDisplay[0] = getXValue(line1, timestamp, dataShow1.From.Text, 1);
                }
                catch 
                { 
                    ParaDisplay[0] = "NA"; 
                }

                try
                {
                    ParaDisplay[1] = getXValue(line2, timestamp, dataShow2.From.Text, 2);
                }
                catch 
                { 
                    ParaDisplay[1] = "NA"; 
                }

                try
                {
                    ParaDisplay[2] = getXValue(line3, timestamp, dataShow3.From.Text, 3);
                }
                catch 
                { 
                    ParaDisplay[2] = "NA"; 
                }

                try
                {
                    ParaDisplay[3] = getXValue(line4, timestamp, dataShow4.From.Text, 4);
                }
                catch 
                { 
                    ParaDisplay[3] = "NA"; 
                }

                timestamp *= 1000;
                strValue = Comm.ConvertIntDateTime(timestamp).ToString() + "\n" + ParaDisplay[0] + "\n" + ParaDisplay[1] + "\n" + ParaDisplay[2] + "\n" + ParaDisplay[3];
            }

            catch
            {
            }
            return strValue;
        }

        private string getString(string strValue)
        {
            string strRet = string.Empty;
            if (!string.IsNullOrEmpty(strValue) && strValue != "NA")
            {
                strRet = Convert.ToDouble(strValue).ToString("0.00");
            }
            else
            {
                strRet = strValue;
            }

            return strRet;
        }

        //鼠标移动画线
        private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            toolTip1.BackColor = Color.Black;
            e.DrawBorder();
            SolidBrush br0 = new SolidBrush(Color.White);
            SolidBrush br1 = new SolidBrush(Color.FromArgb(0, 255, 0));
            SolidBrush br2 = new SolidBrush(Color.FromArgb(0, 255, 255));
            SolidBrush br3 = new SolidBrush(Color.FromArgb(255, 165, 0));
            SolidBrush br4 = new SolidBrush(Color.FromArgb(255, 255, 0));
            string strTxt = e.ToolTipText;
            Font fCaptal = new Font("Segoe UI", 11);
            Font fTitle = new Font("Segoe UI", 9);
            string[] strAry = strTxt.Split('\n');
            if (strAry.Length < 5)
            {
                return;
            }
            e.Graphics.DrawString(strAry[0], fTitle, br0, 5, 0);
            e.Graphics.DrawString(dataShow1.DSCaptal + " " + getString(strAry[1]), fCaptal, br1, 5, 25);
            e.Graphics.DrawString(dataShow2.DSCaptal + " " + getString(strAry[2]), fCaptal, br2, 5, 45);
            e.Graphics.DrawString(dataShow3.DSCaptal + " " + getString(strAry[3]), fCaptal, br3, 5, 65);
            e.Graphics.DrawString(dataShow4.DSCaptal + " " + getString(strAry[4]), fCaptal, br4, 5, 85);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            var temp = TextRenderer.MeasureText(m_strTxt, new Font("Segoe UI", 13));
            e.ToolTipSize = new Size(temp.Width + 15, temp.Height);
        }

        public void setBaseTime(double bTime)
        {
            m_dBaseTime = bTime;
        }

        //初始化DataShow字典
        private void InitDictionary()
        {
            m_dicDataShow.Add(DATA_SHOW.tagOne, dataShow1);
            m_dicDataShow.Add(DATA_SHOW.tagTwo, dataShow2);
            m_dicDataShow.Add(DATA_SHOW.tagThree, dataShow3);
            m_dicDataShow.Add(DATA_SHOW.tagFour, dataShow4);
        }

        //初始化DataShow
        private void InitDataShow(bool bIsReset = false)
        {
            foreach (var data in m_dicDataShow)
            {
                int iIndex = (int)data.Key + 1;
                var dataShow = data.Value;
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

            setDataShowFontColor();
        }

        //根据数据库保存DataShow的值，设置DataShow的内容
        private void setDataShow(DataShow dataShow, int iIndex)
        {
            if (null == UserTag)
            {
                return;
            }

            var userTag = UserTag.Find(o => o.Form == fname && o.Group == group && o.Order == iIndex);
            if (null != userTag)
            {
                var tagList = listTag.Where(o => o.Tag == userTag.Tag).FirstOrDefault();
                if (null != tagList)
                {
                    var from = Comm.UnitConversion(listTag, tagList.Tag, tagList.DrillId.ToString(), Convert.ToDouble(userTag.VFrom));
                    var to = Comm.UnitConversion(listTag, tagList.Tag, tagList.DrillId.ToString(), Convert.ToDouble(userTag.VTo));
                    dataShow.DTag = userTag.Tag;
                    dataShow.DSCaptal = Transformation(userTag.Tag);
                    dataShow.DSRangeFrom = from.ToString();
                    dataShow.DSRangeTo = to.ToString();

                    setDataShowUnit(dataShow, tagList.Unit);
                }
                else
                {
                    setEmptyDataShow(dataShow);
                }
            }
            else
            {
                setEmptyDataShow(dataShow);
            }
        }

        //根据在EditTagForm选择的测点来设置DataShow控制的值
        private void setDataShow(DataShow dataShow, EditTagForm form)
        {
            if (null == dataShow || null == form)
            {
                return;
            }

            int iIndex = form.order;
            string oldTag = dataShow.DSCaptal;//保存之前的测点
            string oldFrom = dataShow.DSRangeFrom;//保存之前的测点量程
            string oldTo = dataShow.DSRangeTo;

            if (!string.IsNullOrEmpty(form.Captial))
            {
                if (!string.IsNullOrEmpty(form.From))
                {
                    var from = Comm.UnitConversion(listTag, form.Captial, m_iDrillID.ToString(), Convert.ToDouble(form.From));
                    dataShow.DSRangeFrom = from.ToString();
                }

                if (!string.IsNullOrEmpty(form.To))
                {
                    var to = Comm.UnitConversion(listTag, form.Captial, m_iDrillID.ToString(), Convert.ToDouble(form.To));
                    dataShow.DSRangeTo = to.ToString();
                }

                dataShow.DTag = form.Tags;
                dataShow.DSCaptal = form.Captial;
                dataShow.DSLValue = form.LValue;
                dataShow.DSHValue = form.HValue;
                setDataShowUnit(dataShow, form.Unit);
                dataShow.SetTag();


                if (!string.IsNullOrEmpty(oldFrom) && !string.IsNullOrEmpty(oldTo))
                {
                    Clear(iIndex, double.Parse(oldFrom), double.Parse(oldTo), double.Parse(dataShow.DSRangeFrom), double.Parse(dataShow.DSRangeTo), oldTag != dataShow.DSCaptal ? true : false);
                }

                if (oldTag != dataShow.DSCaptal)
                {
                    clearChart(iIndex);
                    getTagData(form.Tags, iIndex);
                }
            }      
        }

        //将DataShow的值设置为空
        private void setEmptyDataShow(DataShow dataShow)
        {
            dataShow.DSCaptal = "";
            dataShow.DSunit = "";
            dataShow.DSRangeFrom = "";
            dataShow.DSRangeTo = "";
            dataShow.DSLValue = "";
            dataShow.DSHValue = "";
            dataShow.DTag = "";
        }

        //根据数据库保存的默认值，重置DataShow控件的值
        private void resetTag(DataShow dataShow, int iIndex)
        {
            try
            {
                if (null == AppDrill.UserTag || AppDrill.UserTag.Count <= 0)
                {
                    return;
                }

                UserTag data = null;
                data = AppDrill.UserTag.Find(o => o.Form == fname && o.Group == group && o.Order == iIndex);

                if (data != null)
                {
                    var taglist = listTag.Where(o => o.Tag == data.Tag).FirstOrDefault();
                    if (taglist != null)
                    {
                        var from = Comm.UnitConversion(listTag, taglist.Tag, taglist.DrillId.ToString(), Convert.ToDouble(data.VFrom));
                        var to = Comm.UnitConversion(listTag, taglist.Tag, taglist.DrillId.ToString(), Convert.ToDouble(data.VTo));
                        dataShow.DTag = data.Tag;
                        dataShow.DSCaptal = Transformation(data.Tag);
                        dataShow.DSRangeFrom = from.ToString();
                        dataShow.DSRangeTo = to.ToString();
                        setDataShowUnit(dataShow, taglist.Unit);
                        clearChart(iIndex);
                    }
                }
            }
            catch
            {
            }
        }

        //设置DataShow字体颜色
        private void setDataShowFontColor()
        {
            try
            {
                dataShow1.DSValueColor = Color.FromArgb(0, 255, 0);
                dataShow2.DSValueColor = Color.FromArgb(0, 255, 255);
                dataShow3.DSValueColor = Color.FromArgb(255, 165, 0);
                dataShow4.DSValueColor = Color.FromArgb(255, 255, 0);
                dataShow1.SetTag();
                dataShow2.SetTag();
                dataShow3.SetTag();
                dataShow4.SetTag();
            }
            catch
            {
            }
        }

        //设置DataShow的单位
        private void setDataShowUnit(DataShow dataShow, string strUnit)
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
                    dataShow.DSunit = UnitModel.UnitTo;
                }
                else
                {
                    dataShow.DSunit = strUnit;
                }
            }
            else if (AppDrill.UnitFormat == "gz")
            {
                dataShow.DSunit = strUnit;
            }
        }

        //显示DataShow接收的实时数据
        public void ShowMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            foreach (var data in m_dicDataShow)
            {
                int iIndex = (int)data.Key;
                var dataShow = data.Value;

                if (null == dataShow)
                {
                    continue;
                }

                IPointListEdit list = getPointList(iIndex);
                if (list == null)
                {
                    return;
                }

                double value = 0;
                long lTimestamp = realTimeData.Timestamp;
                string strBoxID = realTimeData.BoxId.ToString();
                var tag = realTimeData.realTags.Where(o => o.Tag == dataShow.DTag).FirstOrDefault();

                if (null != tag)
                {
                    value = tag.Value;
                    var newValue = Comm.UnitConversion(listTag, dataShow.DTag, strBoxID, value);

                    if (m_bisReceive && !ViewHistory)
                    {
                        try
                        {
                            list.Add((newValue - double.Parse(dataShow.From.Text)) / getTagRange(iIndex + 1) * 100, lTimestamp);
                        }
                        catch
                        {
                        }
                    }

                    if ((bool)listTag.Where(o => o.Tag == dataShow.DTag).FirstOrDefault().IsBool)
                    {
                        csDataShow(newValue > 0 ? "Yes" : "No", iIndex + 1);
                    }
                    else
                    {
                        csDataShow(newValue.ToString(), iIndex + 1);
                    }

                    isAlarm(dataShow.DTag, strBoxID, value.ToString(), iIndex + 1);
                }
            }
        }

        //获取要接收测点值的列表
        private IPointListEdit getPointList(int iIndex)
        {
            if (zed1.GraphPane.CurveList.Count <= 0)
            {
                return null;
            }

            LineItem curve = zed1.GraphPane.CurveList[iIndex] as LineItem;
            if (null == curve)
            {
                return null;
            }

            IPointListEdit list = curve.Points as IPointListEdit;
            if (null == list)
            {
                return null;
            }

            return list;
        }
    }
}
