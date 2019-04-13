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
using Apache.NMS;
using DrillingSymtemCSCV2.Model;
using Apache.NMS.ActiveMQ;
using DrillingSymtemCSCV2.Forms;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class DepthTimeChart : UserControl
    {
        private string DrillTag = "var2";
        private string Unit = "m";
        private DrillOSEntities _db;
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        public bool ViewHistory = false;

        private Color _BACK_COLOR = System.Drawing.Color.FromArgb(255, 45, 45, 48);
        private double _YCount = 1800;
        public GraphPane myPane;

        //当前可见点个数
        int VisiableCnt = 0;
        //页面缓存点个数,程序默认一天，后从appconfig读取配置
        private int DataShowCnt = 43200;

        // 起始时间以毫秒为单位
        int tickStart = 0;

        private int m_idepthSetp = 1;
        public int m_idepthVasibleSetp = 1;
        public int m_irepeateValue = -1;
        private int m_istepIndex = 0;
        private int m_iTimeStep = 60;
        private int m_iTimeIndex = 3;
        private double m_dBaseTime = 0.0;
        private bool m_bisReceive = false;
        private bool m_bIsReal = true;
        private bool m_bIsBase = true;
        private int m_iDrillID = 0;
        private long m_lStartTime = 0L;
        private UserControls.FourChart m_fourChart1;
        private UserControls.FourChart m_fourChart2;
        private UserControls.FourChart m_fourChart3;

        private DateTime time_now = DateTime.Now;
        private int m_idataShowCount = 1;
        //功能按钮是否禁用
        public bool isEnlarge = false;//是否放到最大
        public bool isNarrow = false;//是否缩小到最小
        public bool isUp = false;//是否到达最上方
        public bool isDown = false;//是否到达最下方
        public bool isPageUp= false;//是否到达最上方
        public bool isPageDown = false;//是否到达最下方
        public bool isActive = false;//是否激活刷新
        LineItem curve;
        public double d_now { get; set; }    //服务器时间戳

        public DepthTimeChart()
        {
            InitializeComponent();
            VisiableCnt = (int)_YCount;
            //zed1.PointValueEvent += new ZedGraphControl.PointValueHandler(Point_SetFormatEvent);

        }

        public void InitPicLoad(bool bIsVisible)
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.pic_load.Visible = bIsVisible;
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.pic_load.Visible = bIsVisible;
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.pic_load.Visible = bIsVisible;
            }
        }

        public void setDrillID(int iDrillID)
        {
            m_iDrillID = iDrillID;
        }

        enum TimeStep
        {
            oneMinute = 1,
            twoMinutes,
            threeMinutes,
            fiveMinutes,
            tenMinutes,
            twentyMinutes,
            thirtyMinutes,
            oneHour,
            twoHours,
            fourHours
        }

        private void getTimeStep(int iIndex, bool bUpDown = true)
        {
            switch (iIndex)
            {
                case (int)TimeStep.oneMinute:
                    m_iTimeStep = bUpDown ? 60 : 5 * 60;
                    break;
                case (int)TimeStep.twoMinutes:
                    m_iTimeStep = bUpDown ? 2 * 60 : 10 * 60;
                    break;
                case (int)TimeStep.threeMinutes:
                    m_iTimeStep = bUpDown ? 3 * 60 : 15 * 60;
                    break;
                case (int)TimeStep.fiveMinutes:
                    m_iTimeStep = bUpDown ? 5 * 60 : 25 * 60;
                    break;
                case (int)TimeStep.tenMinutes:
                    m_iTimeStep = bUpDown ? 10 * 60 : 50 * 60;
                    break;
                case (int)TimeStep.twentyMinutes:
                    m_iTimeStep = bUpDown ? 20 * 60 : 100 * 60;
                    break;
                case (int)TimeStep.thirtyMinutes:
                    m_iTimeStep = bUpDown ? 30 * 60 : 150 * 60;
                    break;
                case (int)TimeStep.oneHour:
                    m_iTimeStep = bUpDown ? 60 * 60 : 5 * 60 * 60;
                    break;
                case (int)TimeStep.twoHours:
                    m_iTimeStep = bUpDown ? 2 * 60 * 60 : 10 * 60 * 60;
                    break;
                case (int)TimeStep.fourHours:
                    m_iTimeStep = bUpDown ? 4 * 60 * 60 : 20 * 60 * 60;
                    break;
                default:
                    m_iTimeStep = 5 * 60;
                    break;
            }
        }   

        public void setFourTimeStep()
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.setTimeStep(m_iTimeStep);
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.setTimeStep(m_iTimeStep);
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.setTimeStep(m_iTimeStep);
            }
        }

        private void setLableDate(double dTime)
        {
            DateTime date = Comm.ConvertIntDateTime(dTime * 1000);
            if (lbl_time.InvokeRequired)
            {
                lbl_time.Invoke(new Action(() => { lbl_time.Text = date.ToString("yyyy/MM/dd"); }));
            }
            else
            {
                lbl_time.Text = date.ToString("yyyy/MM/dd");
            }
        }


        public string getDepthTag()
        {
            return DrillTag;
        }

        public void addHistoryData(List<HistoryDepthData> Depthdatas)
        {
            if (null == Depthdatas)
            {
                return;
            }

            foreach (var item in Depthdatas)
            {
                if (item.Value <= 0)
                {
                    continue;
                }

                int iValue = (int)UnitConversion(item.Value);
                if (!IsRepeateData(iValue))
                {
                    AddMemo(iValue.ToString("#0"), item.Timestamp, true);
                }
            }
        }

        public void setDepthChart(UserControls.FourChart fourChart, int iIndex)
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
            }
        }

        private void clearFourchart()
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.ClearPointList();
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.ClearPointList();
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.ClearPointList();
            }
        }

        public void clearChart()
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.ClearChart();
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.ClearChart();
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.ClearChart();
            }
        }

        public void clearAll()
        {
            if (null != myPane.GraphObjList)
            {
                myPane.GraphObjList.Clear();
            }

            clearFourchart();
            clearChart();
        }

        public void setMajorStep()
        {
            myPane.YAxis.Scale.MajorStep = m_idepthSetp;

            if (m_fourChart1 != null)
            {
                m_fourChart1.setMajorStep(m_idepthSetp);
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.setMajorStep(m_idepthSetp);
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.setMajorStep(m_idepthSetp);
            }
        }

        public void setReceive(bool bReceive)
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.setReceive(bReceive);
            }


            if (m_fourChart2 != null)
            {
                m_fourChart2.setReceive(bReceive);
            }


            if (m_fourChart3 != null)
            {
                m_fourChart3.setReceive(bReceive);
            }

            m_bisReceive = bReceive;
        }

        public void setTimeMaxmin(long lMax)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            getTimeStep(m_iTimeIndex);
            long lMin = 0L;
            lMin = lMax - m_iTimeStep * 10;
            yScale.Max = lMax;
            yScale.Min = lMin;
            myPane.YAxis.Scale.MajorStep = m_iTimeStep;
        }

        private int getStep()
        {
            int istep = 0;
            switch (m_istepIndex)
            {
                case 0:
                    istep = 1;
                    break;

                case 1:
                    istep = 5;
                    break;

                case 2:
                    istep = 10;
                    break;

                case 3:
                    istep = 20;
                    break;

                case 4:
                    istep = 25;
                    break;

                case 5:
                    istep = 50;
                    break;

                default:
                    break;
            }

            return istep;
        }

        private void DepthTimeChart_Load(object sender, EventArgs e)
        {
            _db = new DrillOSEntities();
            //初次加载，读取当前井深
           // m_irepeateValue = (int)double.Parse(_db.Drill.Where(p => p.ID == AppDrill.DrillID).FirstOrDefault().HoleDepth);

            lbl_time.Text = DateTime.Now.ToString("yyyy/MM/dd");
            try
            {
                if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
                {
                    rbtn_message.Enabled = false;
                }
                //获取用户配置页面缓存点个数
                DataShowCnt = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["DataShowCnt"].ToString());
            }
            catch
            {
            }

            // *** BEGIN 控件整体设置 ***
           // zed1.IsShowPointValues = true;    //显示网格上对应值
            zed1.IsEnableZoom = false;
            zed1.IsEnableVZoom = false;
            zed1.IsEnableHZoom = false;
            zed1.IsShowContextMenu = false; //禁用右键弹出菜单
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
            myPane.XAxis.IsVisible = false;                               //X轴直接不显示
            myPane.XAxis.Color = Color.Transparent;                       //X轴颜色
            myPane.XAxis.Scale.IsVisible = true;                          //X轴显示
            myPane.XAxis.Scale.FontSpec.FontColor = Color.GreenYellow;    //X轴字体颜色
            myPane.XAxis.MajorGrid.IsVisible = false;                     //X轴显示网格
            myPane.XAxis.MajorGrid.Color = Color.Gray;                    //X轴网格颜色
            myPane.XAxis.Scale.Min = 1;                                   //X轴最小值0
            myPane.XAxis.Scale.Max = 10;                                  //X轴最大60
            //myPane.XAxis.Scale.MajorStepAuto = true;                      //X轴显示步长
            myPane.XAxis.Scale.FontSpec.IsUnderline = false;
            //myPane.XAxis.Scale.MajorStep = 1;                             //X轴小步长1,也就是小间隔
            // *** END X轴设置 ***

            // *** BEGIN Y轴设置 ***
            myPane.YAxis.Title.IsVisible = false;
            myPane.YAxis.MinorTic.IsOpposite = false;                //Y轴1显示抬头
            myPane.YAxis.MajorTic.IsOpposite = false;
            //myPane.YAxis.Title.Text = "Timing";                           //Y轴1时间类型
            //myPane.YAxis.Title.FontSpec.FontColor = Color.YellowGreen;    //Y轴字体颜色
            //myPane.YAxis.Title.FontSpec.Size = 22;
            myPane.YAxis.Color = Color.White;                       //Y轴颜色
            myPane.YAxis.Scale.IsVisible = true;                          //Y轴显示
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.AlignH = AlignH.Center;
            myPane.YAxis.Scale.FontSpec.Size = 20;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.White;    //Y轴字体颜色
            myPane.YAxis.MajorGrid.IsVisible = true;                     //Y轴显示网格
            myPane.YAxis.MajorGrid.Color = Color.WhiteSmoke;                    //Y轴网格颜色
            myPane.YAxis.Scale.Max = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;            //Y轴从0开始,这个地方要影响X轴的显示
            myPane.YAxis.Scale.Min = myPane.YAxis.Scale.Max - _YCount;
            myPane.YAxis.Scale.MajorStep = (myPane.YAxis.Scale.Max - myPane.YAxis.Scale.Min) / 10;
            m_dBaseTime = myPane.YAxis.Scale.Max;


            myPane.YAxis.Scale.IsReverse = true;                          //从上到下画线

            // *** ★★★★★ 第二轴设置 ★★★★★ ***
            //myPane.Y2Axis.IsVisible = true;
            //myPane.Y2Axis.Title.IsVisible = true;                         //Y轴2显示抬头
            //myPane.Y2Axis.Title.Text = "Depth";                           //Y轴2时间类型
            //myPane.Y2Axis.Title.FontSpec.Size = 22;
            //myPane.Y2Axis.Color = Color.YellowGreen;
            //myPane.Y2Axis.Scale.FontSpec.Size = 22;
            //myPane.Y2Axis.Scale.FontSpec.FontColor = Color.YellowGreen;
            //myPane.Y2Axis.Title.FontSpec.FontColor = Color.YellowGreen;
            //myPane.Y2Axis.MajorTic.IsOpposite = false;
            //myPane.Y2Axis.MinorTic.IsOpposite = false;
            //myPane.Y2Axis.MajorGrid.IsVisible = true;
            //// Align the Y2 axis labels so they are flush to the axis
            //myPane.Y2Axis.Scale.Align = AlignP.Inside;
            //myPane.Y2Axis.Scale.Min = 0;     // ★★★★★ Y2的最小值可以设置为当前井深
            //myPane.Y2Axis.Scale.Max = myPane.Y2Axis.Scale.Min + _DepthCount;      //可以为一定步长
            //myPane.Y2Axis.Scale.IsReverse = true;
            // *** BEGIN Y轴设置 ***

            // *** BEGIN chart线段设置 ***
            //设置1200个点,假设每50毫秒更新一次,刚好检测1分钟,一旦构造后将不能更改这个值
            RollingPointPairList list1 = new RollingPointPairList(DataShowCnt);
            //RollingPointPairList list2 = new RollingPointPairList(259200);

            // *** ★★★★★ Begin Y2的最小值可以设置为当前井深 ★★★★★
            //list2.Add(6, myPane.Y2Axis.Scale.Min);   //设置当前井深 由于要画线，所以得两个点
            //list2.Add(6, 0);                    //设置当前井深 由于要画线，所以得两个点
            // *** ★★★★★ End Y2的最小值可以设置为当前井深 ★★★★★

            //开始，增加的线是没有数据点的(也就是list为空)
            //★★★增加一条名称:Voltage，颜色Color.Bule，无符号，无数据的空线条
            curve = myPane.AddCurve("", list1, Color.Green, SymbolType.None);
            //curve.YAxisIndex = 0;
            curve.Line.IsSmooth = true;    //平滑曲线
            curve.Line.SmoothTension = 0.6F;

            //curve = myPane.AddCurve("", list2, Color.Red, SymbolType.None);
            curve.IsY2Axis = true;
            //curve.YAxisIndex = 1;                     //★★★ 通过词句，该队列与第二个坐标轴关联
            curve.Line.IsSmooth = true;    //平滑曲线
            curve.Line.Width = 3;
            curve.Line.SmoothTension = 0.6F;
            // *** END chart线段设置 ***

            zed1.GraphPane.YAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(YAxis_ScaleFormatEvent);

            //改变轴的刻度
            zed1.AxisChange();
            //保存开始时间
            tickStart = Environment.TickCount;
            //SetDepth(Comm.ConvertDateTimeInt(DateTime.Now) / 1000, 1800);
        }

        private bool IsValidate(int value)
        {
            if (!IsRepeateData(value) && (0 == value % m_idepthVasibleSetp))
            {
                return true;
            }

            return false;
        }

        private bool IsRepeateData(int value)
        {
            if (value == m_irepeateValue)
            {
                return true;
            }
            else
            {
                m_irepeateValue = value;
                return false;
            }
        }

        private void ResetGraphObj(GraphObj obj)
        {
            TextObj memo = (TextObj)obj;
            int value = (int)double.Parse(memo.Text);
            memo.IsVisible = IsValidate(value);
        }

        private void AddMemo(string strValue, double dValue, bool bVisible)
        {
            TextObj memo = new TextObj("" + strValue + "", 1.25, dValue);   //曲线内容与出现位置
            memo.FontSpec.Border.Color = Color.Transparent;            //Memo边框颜色
            memo.FontSpec.FontColor = Color.White;                     //Memo文本颜色
            memo.FontSpec.Size = 20f;                                  //Memo文本大小
            // Align the text such that the Bottom-Center is at (175, 80) in user scale coordinates
            memo.Location.AlignH = AlignH.Left;
            memo.Location.AlignV = AlignV.Bottom;
            memo.FontSpec.Fill = new Fill(Color.FromArgb(45, 45, 48), Color.FromArgb(45, 45, 48), 100);
            memo.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(memo);

            myPane.GraphObjList[myPane.GraphObjList.Count - 1].IsVisible = bVisible;
        }

        private void zedInvalidateByTime(long lNewValue)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            if (isActive)
            {
                //时间走动
                if (!ViewHistory)
                {
                    double distance = lNewValue - yScale.Max;
                    VisiableCnt = (int)(yScale.Max - yScale.Min);
                    yScale.Min = yScale.Max - VisiableCnt + distance;
                    yScale.Max = lNewValue;

                    if (distance != 1)
                    {
                        //更新步长
                        double Num = yScale.Max - yScale.Min;
                        yScale.MajorStep = Num / 10;
                    }

                    zed1.AxisChange();
                    zed1.Invalidate();
                }
            }
        }

        private void showFourchartToolTip()
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.showToolTip();
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.showToolTip();
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.showToolTip();
            }
        }

        private void zedFourChartInvalidate(long lNewValue)
        {
            if (m_fourChart1 != null)
            {
                m_fourChart1.zedInvalidate(lNewValue);
            }

            if (m_fourChart2 != null)
            {
                m_fourChart2.zedInvalidate(lNewValue);
            }

            if (m_fourChart3 != null)
            {
                m_fourChart3.zedInvalidate(lNewValue);
            }
        }

        //显示数据
        public void ShowMessage(RealTimeData realTimeData)
        {
            double newValue = 0.0;
            int iValue  = 0;
            long lTimestamp = 0L;
            time_now = DateTime.Now;
            if (null == realTimeData)
            {
                return;
            }

            lTimestamp = realTimeData.Timestamp;
            d_now = lTimestamp;
            if (ViewHistory || !m_bisReceive)
            {
                return;
            }

            if (null != realTimeData.realTags)
            {
                var tag = realTimeData.realTags.Where(o => o.Tag == DrillTag).FirstOrDefault();
                if (null != tag)
                {
                    newValue = UnitConversion(tag.Value);
                    iValue = (int)newValue;
                    AddMemo(iValue.ToString("#0"), lTimestamp, IsValidate(iValue));
                    zedInvalidateByTime(lTimestamp);
                    zedFourChartInvalidate(lTimestamp);
                    showFormMessage();
                    showFourchartToolTip();
                    removeMemo();
                    ++m_idataShowCount;
                }
            }
        }

        private void removeMemo()
        {
            if (myPane.GraphObjList.Count >= DataShowCnt)
            {
                if (m_idataShowCount > DataShowCnt)
                {
                    m_idataShowCount = 1;
                }

                myPane.GraphObjList.Remove(myPane.GraphObjList[0]);
            }
        }

        #region 历史数据查看  Add by ZAY in 2017.5.4

        /// <summary>
        /// 查看历史向上翻页（在使用该控件(FourChart)的主页面中，Name.UpClick(Num) 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        /// <param name="Num">增量，即翻页时Y轴位移量，例如：5 = 5条数据 </param>

        public void getMaxMin(ref long lMax, ref long lMin)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            lMax = (long)yScale.Max;
            lMin = (long)yScale.Min;
        }

        public int getHeight()
        {
            return zed1.Height;
        }

        public void ShowMemo()
        {
            try
            {
                foreach (var item in myPane.GraphObjList.Where(o => o.Tag == null))
                {
                    ResetGraphObj(item);
                }
            }
            catch
            {
            }

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void queryHis()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            ViewHistory = true;
            m_dBaseTime = yScale.Max;
            myPane.YAxis.Scale.MajorStep = m_iTimeStep;
            setLableDate(yScale.Max);
        }

        private void UpClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            isDown = false;
            isPageDown = false;
            ViewHistory = true;
            m_bIsBase = false;
			m_bIsReal = false;
            getTimeStep(m_iTimeIndex);
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;

            m_dBaseTime = yScale.Max;
            setLableDate(yScale.Max);
        }

        private void UpClick2ByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            isDown = false;
            isPageDown = false;
            ViewHistory = true;
            m_bIsBase = false;
			m_bIsReal = false;
            getTimeStep(m_iTimeIndex, false);
            m_iTimeStep = m_iTimeStep * 2;
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;

            m_dBaseTime = yScale.Max;
            setLableDate(yScale.Max);
        }

        public void UpClick()
        {
              UpClickByTime();
        }

        public void UpClick2()
        {
            UpClick2ByTime();
        }

        public bool IsAllowedDownClick()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            double dMax = yScale.Max + m_iTimeStep;
            double dnow = 0.0;

            if (d_now <= 0)
            {
                dnow = (double)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            }
            else
            {
                dnow = d_now;
            }

            if (dMax > dnow)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DownClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            isUp = false;
            isPageUp = false;
            ViewHistory = true;
            m_bIsBase = false;
            getTimeStep(m_iTimeIndex);
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;

            m_dBaseTime = yScale.Max;

            setLableDate(yScale.Max);
        }

        private void DownClick2ByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            isPageUp = false;
            isUp = false;
            ViewHistory = true;
            m_bIsReal = false;
            getTimeStep(m_iTimeIndex, false);
            m_iTimeStep = m_iTimeStep * 2;
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;
            m_bIsBase = false;
            m_dBaseTime = yScale.Max;

            setLableDate(yScale.Max);
        }

        public void DownClick()
        {
            DownClickByTime();
        }

        public void DownClick2()
        {
            DownClick2ByTime();
        }

        private void RealTimeClickByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            getTimeStep(m_iTimeIndex, false);
            m_bIsBase = true;
            if (d_now <= 0)
            {
                double dnow = (double)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
                yScale.Max = dnow;
                yScale.Min = dnow - (m_iTimeStep * 2);
                yScale.MajorStep = (m_iTimeStep * 2) / 10;
            }
            else
            {
                yScale.Max = d_now;
                yScale.Min = d_now - (m_iTimeStep * 2);
                yScale.MajorStep = (m_iTimeStep * 2) / 10;
            }

            m_dBaseTime = yScale.Max;

            if (d_now > 0)
            {
                setLableDate(d_now);
            }
            else
            {
                lbl_time.Text = DateTime.Now.ToString("yyyy/MM/dd");
            }

            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void RealTimeClick()
        {
            RealTimeClickByTime();
        }

        private void EnlargeByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            --m_iTimeIndex;
            //放大到最大不再进行放大

            if ((int)TimeStep.oneMinute == m_iTimeIndex)
            {
                isEnlarge = true;
            }
            else
            {
                isEnlarge = false;
            }


            isNarrow = false;
            ViewHistory = true;

            if (m_bIsBase)
            {
                setBaseTime();
            }

            if (m_bIsReal)
            {
                getTimeStep(m_iTimeIndex);
            }
            else
            {
                getTimeStep(m_iTimeIndex, false);
                yScale.Max = m_dBaseTime;
                yScale.Min = m_dBaseTime - m_iTimeStep * 2;
                yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
			}

            setLableDate(yScale.Max);
        }

        public void Enlarge()
        {
            EnlargeByTime();
        }

        //缩小

        private void NarrowByTime()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            ++m_iTimeIndex;
            //缩小到最小不再进行缩小
            if ((int)TimeStep.fourHours == m_iTimeIndex)
            {
                isNarrow = true;
            }
            else
            {
                isNarrow = false;
            }

            isEnlarge = false;
            ViewHistory = true;

            if (m_bIsReal)
            {
                setBaseTime();
            }
            if (m_bIsReal)
            {
                getTimeStep(m_iTimeIndex);
            }
            else
            {
                getTimeStep(m_iTimeIndex, false);
                yScale.Max = m_dBaseTime;
                yScale.Min = m_dBaseTime - m_iTimeStep * 2;
                yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
			}

            setLableDate(yScale.Max);
        }

        public void Narrow()
        {
            NarrowByTime();
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
        #endregion

        #region 公英制换算 ADD by ZAY in 2017.7.21
        private double UnitConversion(double value)
        {
            try
            {
                if (AppDrill.UnitFormat == "yz")
                {
                    if (AppData.UnitTransfer != null)
                    {
                        var UnitModel = AppData.UnitTransfer.Find(o => o.UnitFrom == Unit);
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
        #endregion

        /// <summary>
        /// Y轴显示格式
        /// </summary>
        /// <param name="pane"></param>
        /// <param name="axis"></param>
        /// <param name="val"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string YAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            return Comm.ConvertIntDateTime(val * 1000).ToString("HH:mm:ss");//yyyy/MM/dd 
        }

        private void saveMemo(string strValue, long lTime)
        {
            Memo memoModel = new Memo();

            if (null == memoModel)
            {
                return;
            }

            memoModel.Text = strValue;
            memoModel.Tag = "var2";
            memoModel.Value = getDepthValue(lTime);
            memoModel.UnixTime = Convert.ToInt32(lTime);
            memoModel.CreateTime = DateTime.Now;
            memoModel.DrillID = m_iDrillID;
            memoModel.dataMakePGM = "Hand";
            memoModel.dataMakeTime = DateTime.Now;
            memoModel.dataMakeUser = AppDrill.username;
            memoModel.dataUpdPGM = "Hand";
            memoModel.dataUpdTime = DateTime.Now;
            memoModel.dataUpdUser = AppDrill.username;

            _db.Memo.Add(memoModel);
            _db.SaveChanges();
        }

        private void addMemo(string strValue, long lTime)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;

                    if (m_iDrillID == drillForm.m_iDrillID)
                    {
                        drillForm.createLable(strValue, lTime);
                    }
                }
                else if (frm is CirculateForm)
                {
                    CirculateForm circulateForm = (CirculateForm)frm;
                    circulateForm.createLable(strValue, lTime);
                }
                else if (frm is DirectionalForm)
                {
                    DirectionalForm directionalForm = (DirectionalForm)frm;
                    directionalForm.createLable(strValue, lTime);
                }
                else if (frm is DrillingGasForm)
                {
                    DrillingGasForm drillgasForm = (DrillingGasForm)frm;
                    drillgasForm.createLable(strValue, lTime);

                }
                else if (frm is DrillPVTForm)
                {
                    DrillPVTForm drillpvtForm = (DrillPVTForm)frm;
                    drillpvtForm.createLable(strValue, lTime);
                }
            }
        }

        private void showFormMessage()
        {
            var frm = this.Parent;

            if (null != frm)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;
                    drillForm.setLableLocatoin();
                }
                else if (frm is CirculateForm)
                {
                    CirculateForm circulateForm = (CirculateForm)frm;
                    circulateForm.setLableLocatoin();
                }
                else if (frm is DirectionalForm)
                {
                    DirectionalForm directionalForm = (DirectionalForm)frm;
                    directionalForm.setLableLocatoin();
                }
                else if (frm is DrillingGasForm)
                {
                    DrillingGasForm drillgasForm = (DrillingGasForm)frm;
                    drillgasForm.setLableLocatoin();

                }
                else if (frm is DrillPVTForm)
                {
                    DrillPVTForm drillpvtForm = (DrillPVTForm)frm;
                    drillpvtForm.setLableLocatoin();
                }
            }
        }

        private void rbtn_alarmConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                //跳转编辑窗口
                SelectMessage form = new SelectMessage();
                form.ShowDialog();

                if (!string.IsNullOrEmpty(form.SendText) && form.m_lTime > 0)
                {
                    addMemo(form.SendText, form.m_lTime);
                    saveMemo(form.SendText, form.m_lTime);
                }
            }
            catch 
            { 
            }
        }

        private void rbtn_alarmConfirm_Click1(object sender, EventArgs e)
        {

        }
        public void SetLabel(string s, int iIndex)
        {
            switch(iIndex)
            {
                case 1 :
                this.rbtn_message.Text = s;
                break;
                case 2:
                this.setCharts.Text = s;
                break;
                case 3:
                this.rbtn_meglist.Text = s;
                break;
                default:
                break;
            }
        }
        public DateTime GetTime()
        {
            return time_now;
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

        private void queryHisData()
        {
            var frm = this.Parent;

            if (null != frm)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;
                    drillForm.queryHisData();
                }
                else if (frm is CirculateForm)
                {
                    CirculateForm circulateForm = (CirculateForm)frm;
                    circulateForm.queryHisData();
                }
                else if (frm is DirectionalForm)
                {
                    DirectionalForm directionalForm = (DirectionalForm)frm;
                    directionalForm.queryHisData();
                }
                else if (frm is DrillingGasForm)
                {
                    DrillingGasForm drillgasForm = (DrillingGasForm)frm;
                    drillgasForm.setLableLocatoin();

                }
                else if (frm is DrillPVTForm)
                {
                    DrillPVTForm drillpvtForm = (DrillPVTForm)frm;
                    drillpvtForm.setLableLocatoin();
                }
            }
        }

        private void queryReal()
        {
            var frm = this.Parent;

            if (null != frm)
            {
                if (frm is DrillForm)
                {
                    DrillForm drillForm = (DrillForm)frm;
                    drillForm.setBtnRP();
                    drillForm.realTime();
                }
                else if (frm is CirculateForm)
                {
                    CirculateForm circulateForm = (CirculateForm)frm;
                    circulateForm.setBtnRP();
                    circulateForm.realTime();
                }
                else if (frm is DirectionalForm)
                {
                    DirectionalForm directionalForm = (DirectionalForm)frm;
                    directionalForm.setBtnRP();
                    directionalForm.realTime();
                }
                else if (frm is DrillingGasForm)
                {
                    DrillingGasForm drillgasForm = (DrillingGasForm)frm;
                    drillgasForm.setLableLocatoin();

                }
                else if (frm is DrillPVTForm)
                {
                    DrillPVTForm drillpvtForm = (DrillPVTForm)frm;
                    drillpvtForm.setLableLocatoin();
                }
            }
        }

        public void setVeiwHistroy(bool bTrue)
        {
            ViewHistory = bTrue;
            if (null != m_fourChart1)
            {
                m_fourChart1.setVeiwHistroy(bTrue);
            }

            if (null != m_fourChart2)
            {
                m_fourChart2.setVeiwHistroy(bTrue);
            }

            if (null != m_fourChart3)
            {
                m_fourChart3.setVeiwHistroy(bTrue);
            }
        }

        private void setUp(object sender, EventArgs e)
        {
            MarkForm frm = new MarkForm(m_idepthVasibleSetp, m_iTimeIndex, m_bIsReal, m_lStartTime);

            if (null == frm)
            {
                return;
            }

            frm.ShowDialog();

            if (frm.getCancle() || !frm.getBtnClick())
            {
                return;
            }

            setVeiwHistroy(true);
            m_idepthVasibleSetp = frm.getStep();
            m_iTimeIndex = frm.getTimeIndex();
            m_bIsReal = frm.getReal();
            m_lStartTime = frm.getTime();


            if (!m_bIsReal)
            {
                m_bIsBase = false;
                setTimeMaxmin(m_lStartTime);
                queryHisData();
            }
            else
            {
                if (ViewHistory)
                {
                    getTimeStep(m_iTimeIndex);
                    queryReal();
                }
            }

            ShowMemo();
        }

        private void rbtn_showMessage_Click(object sender, EventArgs e)
        {
            ShowMessage message = new ShowMessage();
            message.setDrillId(m_iDrillID);
            message.ShowDialog();
        }

        private double getDepthValue(long lTime)
        {
            double iDepth = 0;
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            string PostData = null;
            string PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();

            QueryHistory model = new QueryHistory();
            model.startTime = lTime - 20;
            model.endTime = lTime;
            model.DrillId = m_iDrillID;
            model.DepthTag = "var2";
            model.isHistoryData = true;

            List<string> tag = new List<string>();
            tag.Add("var2");
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
                    return iDepth;
                }
                else
                {
                    if (dataList.Depthdatas.Count <= 0)
                    {
                        return iDepth;
                    }

                    int iIndex = dataList.Depthdatas.Count - 1;
                    iDepth = dataList.Depthdatas[iIndex].Value;

                    return Math.Round(iDepth, 2);
                }
            }

            return iDepth;
        }

        public int getTimeIndex()
        {
            return m_iTimeIndex;
        }

        private void setBaseTime()
        {
            m_dBaseTime = d_now;
            if (null != m_fourChart1)
            {
                m_fourChart1.setBaseTime(d_now);
            }
            if (null != m_fourChart2)
            {
                m_fourChart1.setBaseTime(d_now);
            }
            if (null != m_fourChart3)
            {
                m_fourChart1.setBaseTime(d_now);
            }
        }
		
		public bool getReal()
        {
            return m_bIsReal;
        }

        public void setReal(bool bReal)
        {
            m_bIsReal = bReal;
        }
    }
}
