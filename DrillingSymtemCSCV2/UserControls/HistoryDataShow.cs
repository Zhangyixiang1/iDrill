using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using DrillingSymtemCSCV2.Model;
using System.Web.Script.Serialization;
using System.Threading;
using DrillingSymtemCSCV2.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class HistoryDataShow : UserControl
    {
        private DrillOSEntities _db;
        private List<DrillTag> listTag = new List<DrillTag>();

        private Color _BACK_COLOR = Color.Black;
        private GraphPane myPane;
        private double _YCount = 1800;
        //是否显示Y轴
        public bool IsShowY = false;
        private bool m_bShowTip = false;
        Point location;
        double TIMEStamp;
        string txt = "";
        Point temp;

        private double m_dTo = 1;
        private double m_dFrom = 0;

        private Dictionary<LineItem, CureValue> m_cureMap = new Dictionary<LineItem, CureValue>();
        private int m_iTimeStep = 0;
        private double m_dBaseTime = 0.0;
        private const int DataShowCnt = 3 * 86400;
        private LineItem curve1;
        private LineItem curve2;
        private LineItem curve3;
        private LineItem curve4;

        string strCaptal1 = "NA", strFrom1 = "0", strTo1 = "0";
        string strCaptal2 = "NA", strFrom2 = "0", strTo2 = "0";
        string strCaptal3 = "NA", strFrom3 = "0", strTo3 = "0";
        string strCaptal4 = "NA", strFrom4 = "0", strTo4 = "0";



        public HistoryDataShow()
        {
            InitializeComponent();
            zed1.PointValueEvent += new ZedGraphControl.PointValueHandler(Point_SetFormatEvent);
            _db = new DrillOSEntities();
        }

        public void setCaptals(string strCaptal, string strFrom, string strTo, int iIndex)
        {
            switch (iIndex)
            {
                case 1:
                    strCaptal1 = strCaptal;
                    strFrom1 = strFrom;
                    strTo1 = strTo;
                    break;
                case 2:
                    strCaptal2 = strCaptal;
                    strFrom2 = strFrom;
                    strTo2 = strTo;
                    break;
                case 3:
                    strCaptal3 = strCaptal;
                    strFrom3 = strFrom;
                    strTo3 = strTo;
                    break;
                case 4:
                    strCaptal4 = strCaptal;
                    strFrom4 = strFrom;
                    strTo4 = strTo;
                    break;
                default:
                    break;
            }
        }

        public void setTimeStep(int iTimeStep)
        {
            m_iTimeStep = iTimeStep;
        }

        public void clearLine(int iIndex)
        {
            if (zed1.GraphPane.CurveList.Count <= 0)
            {
                return;
            }

            LineItem curve = zed1.GraphPane.CurveList[iIndex] as LineItem;
            if (curve == null)
            {
                return;
            }

            IPointListEdit list = curve.Points as IPointListEdit;
            if (list != null)
            {
                list.Clear();
            }

            zed1.Invalidate();
        }

        public void clearLines()
        {
            if (zed1.GraphPane.CurveList.Count <= 0)
            {
                return;
            }


            LineItem curve1 = zed1.GraphPane.CurveList[0] as LineItem;
            if (curve1 == null)
            {
                return;
            }

            IPointListEdit list1 = curve1.Points as IPointListEdit;
            if (list1 != null)
            {
                list1.Clear();
            }

    
            LineItem curve2 = zed1.GraphPane.CurveList[1] as LineItem;
            if (curve2 == null)
            {
                return;
            }


            IPointListEdit list2 = curve2.Points as IPointListEdit;
            if (list2 != null)
            {
                list2.Clear();
            }

            LineItem curve3 = zed1.GraphPane.CurveList[2] as LineItem;
            if (curve3 == null)
            {
                return;
            }

            IPointListEdit list3 = curve3.Points as IPointListEdit;
            if (list3 != null)
            {
                list3.Clear();
            }


            LineItem curve4 = zed1.GraphPane.CurveList[3] as LineItem;
            if (curve4 == null)
            {
                return;
            }

            IPointListEdit list4 = curve4.Points as IPointListEdit;
            if (list4 != null)
            {
                list4.Clear();
            }

            zed1.Invalidate();
        }


        public void setFrom2To(double dFrom, double dTo)
        {
            m_dTo = dTo;
            m_dFrom = dFrom;
        }

        public void setCures(int iIndex, double dFrom, double dTo)
        {
            switch (iIndex)
            {
                case 1:
                    LineItem curve1 = zed1.GraphPane.CurveList[0] as LineItem;

                    if (curve1 != null)
                    {
                        CureValue cure = new CureValue();

                        if (null != cure)
                        {
                            cure.from = dFrom;
                            cure.to = dTo;
                            m_cureMap[curve1] = cure;
                        }

                    }
                    break;

                case 2:
                    LineItem curve2 = zed1.GraphPane.CurveList[1] as LineItem;

                    if (curve2 != null)
                    {
                        CureValue cure = new CureValue();

                        if (null != cure)
                        {
                            cure.from = dFrom;
                            cure.to = dTo;
                            m_cureMap[curve2] = cure;
                        }
                    }
                    break;

                case 3:
                    LineItem curve3 = zed1.GraphPane.CurveList[2] as LineItem;

                    if (curve3 != null)
                    {
                        CureValue cure = new CureValue();

                        if (null != cure)
                        {
                            cure.from = dFrom;
                            cure.to = dTo;
                            m_cureMap[curve3] = cure;
                        }
                    }
                    break;

                case 4:
                    LineItem curve4 = zed1.GraphPane.CurveList[3] as LineItem;

                    if (curve4 != null)
                    {
                        CureValue cure = new CureValue();

                        if (null != cure)
                        {
                            cure.from = dFrom;
                            cure.to = dTo;
                            m_cureMap[curve4] = cure;
                        }
                    }
                    break;

                default:
                    break;
                      
            }
        }

        private void HistoryDataShow_Load(object sender, EventArgs e)
        {
            // *** BEGIN 控件整体设置 ***
            zed1.IsEnableZoom = false;
            zed1.IsEnableVZoom = false;
            zed1.IsEnableHZoom = false;
            zed1.IsShowContextMenu = false; //禁用右键弹出菜单

            zed1.IsShowPointValues = false;    //显示网格上对应值
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
            myPane.XAxis.Scale.Min = -1;
            myPane.XAxis.Scale.Max = 101;                                  //X轴最大60
            myPane.XAxis.Scale.IsVisible = false;                          //X轴显示
            //myPane.XAxis.Scale.MajorStepAuto = true;                      //X轴显示步长
            myPane.XAxis.Scale.FontSpec.IsUnderline = false;
            myPane.XAxis.Scale.MajorStep = 20;                             //X轴小步长1,也就是小间隔
            // *** END X轴设置 ***

            // *** BEGIN Y轴设置 ***
           
            myPane.YAxis.Title.IsVisible = false;                         //Y轴不显示抬头
            myPane.YAxis.MinorTic.IsOpposite = false;                
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.Color = Color.Transparent;                       //Y轴颜色
            myPane.YAxis.Scale.IsVisible = IsShowY;                  //Y轴不显示
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.AlignH = AlignH.Center;
            myPane.YAxis.Scale.FontSpec.Size = 6;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;    //Y轴字体颜色
            myPane.YAxis.MajorGrid.IsVisible = true;                      //Y轴显示网格
            myPane.YAxis.MajorGrid.Color = Color.Gray;                    //Y轴网格颜色
            //myPane.YAxis.CrossAuto = true;
            //myPane.YAxis.Cross = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;              //这个地方如果加上要影响Memo显示，导致Memo在放大的情况下不能展示出来
            myPane.YAxis.Scale.Max = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;              //Y轴从0开始,这个地方要影响X轴的显示
            myPane.YAxis.Scale.Min = myPane.YAxis.Scale.Max - _YCount;                        //Y轴上放200个点，这样看起来曲线更平滑
            //myPane.YAxis.Scale.MajorStepAuto = true;                      //Y轴显示步长
            myPane.YAxis.Scale.MajorStep = _YCount / 10;                            //X轴大步长为5，也就是显示文字的大间隔
            myPane.YAxis.Scale.IsReverse = true;                          //从上到下画线
            myPane.YAxis.MinorTic.IsInside = false;
            // *** BEGIN Y轴设置 ***
            addCurves();
            //Y轴显示的文本格式化
            if (IsShowY)
                zed1.GraphPane.YAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(YAxis_ScaleFormatEvent);
            m_dBaseTime = myPane.YAxis.Scale.Max;// (myPane.YAxis.Scale.Max + myPane.YAxis.Scale.Min) / 2;
            //改变轴的刻度
            zed1.AxisChange();
        }

        private void addCurves()
        {
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
        }

        /// <summary>
        /// 初始化chart
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void SetChart(long longStartTime, long count)
        {

            myPane.YAxis.Scale.Min = longStartTime;
            myPane.YAxis.Scale.Max = myPane.YAxis.Scale.Min + count;
            myPane.YAxis.Scale.MajorStep = count / 10;
            m_dBaseTime = myPane.YAxis.Scale.Max;// (myPane.YAxis.Scale.Max + myPane.YAxis.Scale.Min) / 2;
        }

        /// <summary>
        /// 设置量程
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        public void SetRange(double form, double to)
        {
            return;

            myPane.XAxis.Scale.Min = form;
            myPane.XAxis.Scale.Max = to;
            myPane.XAxis.Scale.MajorStep = (to - form) / 5;
            //myPane.XAxis.Scale.MajorStep = (to - form) / 5;
            //第三步:调用ZedGraphControl.AxisChange()方法更新X和Y轴的范围
            zed1.AxisChange();
            //第四步:调用Form.Invalidate()方法更新图表
            zed1.Invalidate();
        }

        /// <summary>
        /// 向chart填充数据
        /// </summary>
        /// <param name="dataLists"></param>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        public void upChart(List<TagValue> dataLists, int index, string tag)
        {
            foreach (var item in dataLists)
            {
                setValue(index, tag, item.Value, item.Timestamp); //绘制测点
            }

            InvalidateChart();
        }

        public void InvalidateChart()
        {
            double Num = myPane.YAxis.Scale.Max - myPane.YAxis.Scale.Min;
            myPane.YAxis.Scale.MajorStep = Num / 10;
            zed1.AxisChange();
            zed1.Invalidate();
        }


        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="index"></param>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <param name="timestamp"></param>
        
        private double getNewValue(double dValue)
        {
            double dNewValue = 1;

            try
            {
                double div = m_dTo - m_dFrom;

                if (div <= 0)
                {
                    div = 1;
                }
                dValue -= m_dFrom;

                dNewValue = (dValue / div) * 100;
            }
            catch
            {
            }

            return dNewValue;
        }

        private void setValue(int index, string tag, double value, long timestamp)
        {
            //确保CurveList不为空
            if (zed1.GraphPane.CurveList.Count <= 0)
            {
                return;
            }

            switch (index)
            {
                case 1:
                    //取Graph第一个曲线，也就是第一步:在GraphPane.CurveList集合中查找CurveItem
                    LineItem curve1 = zed1.GraphPane.CurveList[0] as LineItem;
                    if (curve1 == null)
                        return;

                    //第二步:在CurveItem中访问PointPairList(或者其它的IPointList)，根据自己的需要增加新数据或修改已存在的数据
                    IPointListEdit list1 = curve1.Points as IPointListEdit;
                    if (list1 == null)
                        return;

                    var newValue1 = Comm.UnitConversion(listTag, tag, AppDrill.DrillID.ToString(), value);
                    newValue1 = getNewValue(newValue1);
                    list1.Add(newValue1, timestamp);
                    break;
                case 2:
                    //取Graph第一个曲线，也就是第一步:在GraphPane.CurveList集合中查找CurveItem
                    LineItem curve2 = zed1.GraphPane.CurveList[1] as LineItem;
                    if (curve2 == null)
                        return;

                    //第二步:在CurveItem中访问PointPairList(或者其它的IPointList)，根据自己的需要增加新数据或修改已存在的数据
                    IPointListEdit list2 = curve2.Points as IPointListEdit;
                    if (list2 == null)
                        return;

                    var newValue2 = Comm.UnitConversion(listTag, tag, AppDrill.DrillID.ToString(), value);
                    newValue2 = getNewValue(newValue2);
                    list2.Add(newValue2, timestamp);
                    break;
                case 3:
                    //取Graph第一个曲线，也就是第一步:在GraphPane.CurveList集合中查找CurveItem
                    LineItem curve3 = zed1.GraphPane.CurveList[2] as LineItem;
                    if (curve3 == null)
                        return;

                    //第二步:在CurveItem中访问PointPairList(或者其它的IPointList)，根据自己的需要增加新数据或修改已存在的数据
                    IPointListEdit list3 = curve3.Points as IPointListEdit;
                    if (list3 == null)
                        return;

                    var newValue3 = Comm.UnitConversion(listTag, tag, AppDrill.DrillID.ToString(), value);
                    newValue3 = getNewValue(newValue3);
                    list3.Add(newValue3, timestamp);
                    break;
                case 4:
                    //取Graph第一个曲线，也就是第一步:在GraphPane.CurveList集合中查找CurveItem
                    LineItem curve4 = zed1.GraphPane.CurveList[3] as LineItem;
                    if (curve4 == null)
                        return;

                    //第二步:在CurveItem中访问PointPairList(或者其它的IPointList)，根据自己的需要增加新数据或修改已存在的数据
                    IPointListEdit list4 = curve4.Points as IPointListEdit;
                    if (list4 == null)
                        return;

                    var newValue4 = Comm.UnitConversion(listTag, tag, AppDrill.DrillID.ToString(), value);
                    newValue4 = getNewValue(newValue4);
                    list4.Add(newValue4, timestamp);
                    break;
            }
        }

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
            string strValue = string.Empty;

            if (m_cureMap.Count <= 0)
            {
                return strValue;
            }

            LineItem curveItem = (LineItem)curve;
            double range = m_cureMap[curveItem].to - m_cureMap[curveItem].from;
            double xVlaue = curve.Points[iPt].X;
            double yVlaue = curve.Points[iPt].Y;
            yVlaue *= 1000;

            strValue = Comm.ConvertIntDateTime(yVlaue).ToString() + "，" + (xVlaue * range / 100 + m_cureMap[curveItem].from).ToString();

            return strValue;
        }

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

        #region 数据查看功能

        /// <summary>
        /// 查看历史向上翻页（在使用该控件(FourChart)的主页面中，Name.UpClick(Num) 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        public void UpClick()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;

            zed1.AxisChange();
            zed1.Invalidate();
        }

        /// <summary>
        /// 查看历史向下翻页（在使用该控件(FourChart)的主页面中，Name.DownClick(Num) 即可调用）  Add by ZAY in 2017.5.4
        /// </summary>
        public void DownClick()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;

            zed1.AxisChange();
            zed1.Invalidate();
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void Enlarge()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;
            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;

            zed1.AxisChange();
            zed1.Invalidate();
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void Narrow()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;

            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;
            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;

            zed1.AxisChange();
            zed1.Invalidate();
        }
        #endregion
        /// <summary>
        /// 设置黑白风格
        /// </summary>
        public void SetStyle()
        {
            zed1.BackColor = Color.White;
            this.zed1.GraphPane.Fill.Color = Color.White;
            this.zed1.GraphPane.Chart.Fill.Color = Color.White;
            this.zed1.GraphPane.YAxis.Scale.FontSpec.FontColor = Color.Black;
            this.zed1.GraphPane.YAxis.Color = Color.Black;
        }
        /// <summary>
        /// 根据index来获取PointList
        /// </summary>
        /// <param name="index"></param>
        public IPointListEdit GetPointList(int index)
        {
            try
            {
                IPointListEdit iList = zed1.GraphPane.CurveList[index].Points as IPointListEdit;
                return iList;
            }
            catch { return null; }
        }
        /// <summary>
        /// 设置时间轴
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        public void SetTimeFT(double form, double to)
        {
            myPane.YAxis.Scale.Min = form;
            myPane.YAxis.Scale.Max = to;
            myPane.YAxis.Scale.MajorStep = (to - form) / 10;
            //第三步:调用ZedGraphControl.AxisChange()方法更新X和Y轴的范围
            zed1.AxisChange();
            //第四步:调用Form.Invalidate()方法更新图表
            zed1.Invalidate();
        }
        /// <summary>
        /// 设置需要显示的点
        /// </summary>
        /// <param name="index"></param>
        /// <param name="list"></param>
        public void SetPointList(int index,IPointListEdit list)
        {
            try
            {
                //开始，增加的线是没有数据点的(也就是list为空)
                //★★★增加一条名称:Voltage，颜色Color.Bule，无符号，无数据的空线条
                LineItem curve = null;

                switch(index)
                {
                    case 1:
                        curve = myPane.AddCurve("", list, Color.Green, SymbolType.None);
                    break;
                    case 2:
                    curve = myPane.AddCurve("", list, Color.LightSkyBlue, SymbolType.None);
                    break;
                    case 3:
                        curve = myPane.AddCurve("", list, Color.Pink, SymbolType.None);
                    break;
                    case 4:
                        curve = myPane.AddCurve("", list, Color.Red, SymbolType.None);
                    break;

                    default :
                    break;
                }

                //curve.Line.IsSmooth = true;    //平滑曲线
                curve.Line.SmoothTension = 0.6F;
                curve.Line.Width = 2;
                zed1.AxisChange();
                zed1.Invalidate();
            }
            catch 
            { 
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
            pane.ReverseTransform(location, out dUseless, out dTimestamp);
            txt = GetPointValue(dTimestamp);
            toolTip1.SetToolTip(zed1, txt);
        }

        private void zed1_MouseMove(object sender, MouseEventArgs e)
        {
            GraphPane pane = zed1.GraphPane;
            if (temp.Y != e.Y)
            {
                zed1.Invalidate();
                pane.Chart.pt.X = e.X;
                pane.Chart.pt.Y = e.Y;
                location = new Point(e.X, e.Y);
                double timestamp;//时间戳
                double useless;//占位
                pane.ReverseTransform(location, out useless, out timestamp);
                TIMEStamp = timestamp;
                txt = GetPointValue(TIMEStamp);
                toolTip1.SetToolTip(zed1, txt);
                m_bShowTip = true;
            }
            temp.X = e.X;
            temp.Y = e.Y;
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
            string txt = e.ToolTipText;

            Font f = new Font("Segoe UI", 11);
            Font f1 = new Font("Segoe UI", 9);
            string[] txt2 = txt.Split('\n');
            string para1;
            string para2;
            string para3;
            string para4;
            if (txt2[1] != "NA" && !string.IsNullOrEmpty(txt2[1])) { para1 = Convert.ToDouble(txt2[1]).ToString("0.00"); } else { para1 = txt2[1]; }
            if (txt2[2] != "NA" && !string.IsNullOrEmpty(txt2[2])) { para2 = Convert.ToDouble(txt2[2]).ToString("0.00"); } else { para2 = txt2[2]; }
            if (txt2[3] != "NA" && !string.IsNullOrEmpty(txt2[3])) { para3 = Convert.ToDouble(txt2[3]).ToString("0.00"); } else { para3 = txt2[3]; }
            if (txt2[4] != "NA" && !string.IsNullOrEmpty(txt2[4])) { para4 = Convert.ToDouble(txt2[4]).ToString("0.00"); } else { para4 = txt2[4]; }

            e.Graphics.DrawString(txt2[0], f1, br0, 5, 0);
            e.Graphics.DrawString(strCaptal1 + " " + para1, f, br1, 5, 16);
            e.Graphics.DrawString(strCaptal2 + " " + para2, f, br2, 5, 36);
            e.Graphics.DrawString(strCaptal3 + " " + para3, f, br3, 5, 56);
            e.Graphics.DrawString(strCaptal4 + " " + para4, f, br4, 5, 76);


        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
            var temp = TextRenderer.MeasureText(txt, new Font("Calibri", 13));
            e.ToolTipSize = new Size(temp.Width + 15, temp.Height);

        }

        private string GetPointValue(double timestamp)
        {
            string strValue = " ";
            try
            {

                double[] xVlaue = new double[4];
                string[] ParaDisplay = new string[4];

                List<ChartPoint> list1 = new List<ChartPoint>(); List<ChartPoint> list2 = new List<ChartPoint>();
                List<ChartPoint> list3 = new List<ChartPoint>(); List<ChartPoint> list4 = new List<ChartPoint>();
                IPointListEdit line1 = zed1.GraphPane.CurveList[0].Points as IPointListEdit;
                IPointListEdit line2 = zed1.GraphPane.CurveList[1].Points as IPointListEdit;
                IPointListEdit line3 = zed1.GraphPane.CurveList[2].Points as IPointListEdit;
                IPointListEdit line4 = zed1.GraphPane.CurveList[3].Points as IPointListEdit;

                //取得曲线上的点
                for (int i = 0; i < line1.Count; i++)
                {
                    list1.Add(new ChartPoint((long)line1[i].Y, line1[i].X / 100 * getTagRange(1) + Convert.ToInt16(strFrom1)));
                }
                for (int i = 0; i < line2.Count; i++)
                {
                    list2.Add(new ChartPoint((long)line2[i].Y, line2[i].X / 100 * getTagRange(2) + Convert.ToInt16(strFrom2)));
                }
                for (int i = 0; i < line3.Count; i++)
                {
                    list3.Add(new ChartPoint((long)line3[i].Y, line3[i].X / 100 * getTagRange(3) + Convert.ToInt16(strFrom3)));
                }
                for (int i = 0; i < line4.Count; i++)
                {
                    list4.Add(new ChartPoint((long)line4[i].Y, line4[i].X / 100 * getTagRange(4) + Convert.ToInt16(strFrom4)));
                }
                //取得当前时间点对应的参数值
                try
                {
                    if (list1[0].dateTime < timestamp)
                    {
                        xVlaue[0] = list1.Where(o => o.dateTime >= timestamp).OrderBy(p => p.dateTime).First().value; ParaDisplay[0] = xVlaue[0].ToString();
                    }

                }
                catch 
                { 
                    ParaDisplay[0] = "NA"; 
                }

                try
                {
                    if (list2[0].dateTime < timestamp)
                    {
                        xVlaue[1] = list2.Where(o => o.dateTime >= timestamp).OrderBy(p => p.dateTime).First().value; ParaDisplay[1] = xVlaue[1].ToString();
                    }

                }
                catch 
                { 
                    ParaDisplay[1] = "NA"; 
                }

                try
                {
                    if (list3[0].dateTime < timestamp)
                    {
                        xVlaue[2] = list3.Where(o => o.dateTime >= timestamp).OrderBy(p => p.dateTime).First().value; ParaDisplay[2] = xVlaue[2].ToString();
                    }

                }
                catch 
                { 
                    ParaDisplay[2] = "NA"; 
                }

                try
                {
                    if (list4[0].dateTime < timestamp)
                    {
                        xVlaue[3] = list4.Where(o => o.dateTime >= timestamp).OrderBy(p => p.dateTime).First().value; ParaDisplay[3] = xVlaue[3].ToString();
                    }

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

        private void getRange(string strFrom, string strTo, ref double range)
        {
            if (!string.IsNullOrEmpty(strFrom) && !string.IsNullOrEmpty(strTo))
            {
                range = ((double.Parse(strTo) - double.Parse(strFrom))) == 0 ? 1 : (double.Parse(strTo) - double.Parse(strFrom));//如果范围为0的话，强制设置为1
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
                    getRange(strFrom1, strTo1, ref range);
                    break;
                case 2:
                    getRange(strFrom2, strTo2, ref range);
                    break;
                case 3:
                    getRange(strFrom3, strTo3, ref range);
                    break;
                case 4:
                    getRange(strFrom4, strTo4, ref range);
                    break;
            }

            return range;
        }
    }
}
