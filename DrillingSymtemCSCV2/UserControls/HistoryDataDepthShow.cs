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
using DrillingSymtemCSCV2.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class HistoryDataDepthShow : UserControl
    {
        public HistoryDataForm form { get; set; }
        private string Unit = "m";
        private Color _BACK_COLOR = System.Drawing.Color.FromArgb(255, 45, 45, 48);
        private double _YCount = 1800;
        private GraphPane myPane;
        private GraphObjList goList = new GraphObjList();

        public int m_idepthSetp = 1;
        int m_irepeateValue = -1;
        private int m_iTimeStep = 60;
        private int m_iTimeIndex = 3;
        private double m_dBaseTime = 0.0;
        public bool isNarrow = false;
        public bool isEnlarge = false;


        public HistoryDataDepthShow()
        {
            InitializeComponent();
        }

        public void setDepthStep(int iStep)
        {
            m_idepthSetp = iStep;
        }

        public int getDepthStep()
        {
            return m_idepthSetp;
        }

        public void vasibleMemo()
        {
            m_irepeateValue = -1;

            foreach (var item in myPane.GraphObjList)
            {
                ResetGraphObj(item);
            }

            zed1.Invalidate();
        }

        private bool IsValidate(int value)
        {
            if (!IsRepeateData(value) && (0 == value % m_idepthSetp))
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


        public void clearLines()
        {
            if (null != myPane)
            {
                myPane.GraphObjList.Clear();
                zed1.Invalidate();
            }
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

        public int getTimeStep()
        {
            return m_iTimeStep;
        }

        public void setTimeStep(int iIndex, bool bUpDown = true)
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

        public void getMaxMin(ref long lMax, ref long lMin)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            lMax = (long)yScale.Max;
            lMin = (long)yScale.Min;
        }

        private void DepthTimeChart_Load(object sender, System.EventArgs e)
        {
            //backgroundWorker1_depthshow.WorkerSupportsCancellation = true;    //声明是否支持取消线程

            // *** BEGIN 控件整体设置 ***
            zed1.IsShowPointValues = false;    //显示网格上对应值
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
            myPane.XAxis.Scale.Min = -0.1;                                   //X轴最小值0
            myPane.XAxis.Scale.Max = 10.1;                                  //X轴最大60
            //myPane.XAxis.Scale.MajorStepAuto = true;                      //X轴显示步长
            myPane.XAxis.Scale.FontSpec.IsUnderline = false;
            //myPane.XAxis.Scale.MajorStep = 1;                             //X轴小步长1,也就是小间隔
            // *** END X轴设置 ***

            // *** BEGIN Y轴设置 ***
            myPane.YAxis.Title.IsVisible = false;                          //Y轴1显示抬头
            //myPane.YAxis.Title.Text = "Timing";                           //Y轴1时间类型
            //myPane.YAxis.Title.FontSpec.FontColor = Color.YellowGreen;    //Y轴字体颜色
            //myPane.YAxis.Title.FontSpec.Size = 22;
            myPane.YAxis.MinorTic.IsOpposite = false;                
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.Color = Color.YellowGreen;                       //Y轴颜色
            myPane.YAxis.Scale.IsVisible = true;                          //Y轴显示
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.AlignH = AlignH.Center;
            myPane.YAxis.Scale.FontSpec.Size = 20;
            myPane.YAxis.Scale.FontSpec.FontColor = Color.GreenYellow;    //Y轴字体颜色
            myPane.YAxis.MajorGrid.IsVisible = false;                     //Y轴显示网格
            myPane.YAxis.MajorGrid.Color = Color.Gray;                    //Y轴网格颜色
            myPane.YAxis.Scale.Max = Comm.ConvertDateTimeInt(DateTime.Now) / 1000;            //Y轴从0开始,这个地方要影响X轴的显示
            myPane.YAxis.Scale.Min = myPane.YAxis.Scale.Max - _YCount;                        //Y轴上放200个点，这样看起来曲线更平滑
            //myPane.YAxis.Scale.MajorStepAuto = true;                      //Y轴显示步长
            myPane.YAxis.Scale.MajorStep = _YCount / 10;                           //X轴大步长，也就是显示文字的大间隔
            myPane.YAxis.Scale.IsReverse = true;                          //从上到下画线

            zed1.GraphPane.YAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(YAxis_ScaleFormatEvent);
            m_dBaseTime = myPane.YAxis.Scale.Max;// (myPane.YAxis.Scale.Max + myPane.YAxis.Scale.Min) / 2;

            //改变轴的刻度
            zed1.AxisChange();
        }

        /// <summary>
        /// 初始化显示
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        public void setDepth(long startTime, int count)
        {
            myPane.GraphObjList.RemoveRange(0, myPane.GraphObjList.Count);
            myPane.YAxis.Scale.Min = startTime;
            myPane.YAxis.Scale.Max = myPane.YAxis.Scale.Min + count;
            myPane.YAxis.Scale.MajorStep = count / 10;
            m_dBaseTime = (myPane.YAxis.Scale.Max + myPane.YAxis.Scale.Min) / 2;

            zed1.AxisChange();
            zed1.Invalidate();
        }

        /// <summary>
        /// 绘制井深
        /// </summary>
        /// <param name="data"></param>
        public void upDepth(List<HistoryDepthData> data)
        {
            if (data != null)
            {
                foreach (var item in data)
                {
                    int newValue = (int)UnitConversion(item.Value);
                    addMemos(newValue, item.Timestamp);
                }
            }
        }

        private void addMemos(int iValue, long lTimestamp)
        {
            TextObj memo = new TextObj("" + iValue + "", 2, lTimestamp);   //曲线内容与出现位置
            memo.FontSpec.Border.Color = Color.Transparent;                //Memo边框颜色
            memo.FontSpec.FontColor = Color.White;                         //Memo文本颜色
            memo.FontSpec.Size = 24f;                                      //Memo文本大小
            memo.Location.AlignH = AlignH.Left;
            memo.Location.AlignV = AlignV.Bottom;
            memo.FontSpec.Fill = new Fill(Color.Transparent, Color.Transparent, 100);
            memo.FontSpec.StringAlignment = StringAlignment.Far;
            myPane.GraphObjList.Add(memo);
            myPane.GraphObjList[myPane.GraphObjList.Count - 1].IsVisible = IsValidate(iValue);
        }

        #region 历史数据查看
        /// <summary>
        /// 查看历史向上翻页（在使用该控件(FourChart)的主页面中，Name.UpClick(Num) 即可调用）
        /// </summary>
        public void UpClick()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            setTimeStep(m_iTimeIndex);
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;
        }

        public void UpClick2()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            setTimeStep(m_iTimeIndex, false);
            m_iTimeStep = m_iTimeStep * 2;
            yScale.Max -= m_iTimeStep;
            yScale.Min -= m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;
        }

        /// <summary>
        /// 查看历史向下翻页（在使用该控件(FourChart)的主页面中，Name.DownClick(Num) 即可调用）
        /// </summary>
        public void DownClick()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            setTimeStep(m_iTimeIndex);
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;
        }

        public void DownClick2()
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            setTimeStep(m_iTimeIndex, false);
            m_iTimeStep = m_iTimeStep * 2;
            yScale.Max += m_iTimeStep;
            yScale.Min += m_iTimeStep;
            m_dBaseTime = yScale.Max;// (yScale.Max + yScale.Min) / 2;
        }
        /// <summary>
        /// 放大

        public void Enlarge()
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

            setTimeStep(m_iTimeIndex, false);
            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;
            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void Narrow()
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

            setTimeStep(m_iTimeIndex, false);

            yScale.Max = m_dBaseTime;// +m_iTimeStep;
            yScale.Min = m_dBaseTime - m_iTimeStep * 2;
            yScale.MajorStep = (yScale.Max - yScale.Min) / 10;
        }

        #endregion

        #region 公英制换算
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
            //根据 val值 返回你需要的 string  
            return Comm.ConvertIntDateTime(val * 1000).ToString("yyyy/MM/dd HH:mm:ss");
        }
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
        /// 获取需要传递的数据
        /// </summary>
        /// <returns></returns>
        public GraphObjList GetTextObj()
        {
            return goList;
        }
        /// <summary>
        /// 设置时间范围
        /// </summary>
        /// <param name="form"></param>
        /// <param name="to"></param>
        public void SetRange(double form, double to)
        {
            myPane.YAxis.Scale.Min = form;
            myPane.YAxis.Scale.Max = to;
            //更新步长
            double Num = myPane.YAxis.Scale.Max - myPane.YAxis.Scale.Min;
            myPane.YAxis.Scale.MajorStep = Num / 10;
            zed1.AxisChange();
            //第四步:调用Form.Invalidate()方法更新图表
            zed1.Invalidate();
        }
        /// <summary>
        /// 返回量程
        /// </summary>
        /// <returns></returns>
        public List<Double> GetRange()
        {
            List<Double> range = new List<double>();
            double from = zed1.GraphPane.YAxis.Scale.Min;
            double to = zed1.GraphPane.YAxis.Scale.Max;
            range.Add(from);
            range.Add(to);
            return range;
        }
        /// <summary>
        /// 设置memo
        /// </summary>
        /// <param name="gl"></param>
        public void SetTextObj(GraphObjList gl)
        {
            zed1.GraphPane.GraphObjList = gl;
            foreach (var a in zed1.GraphPane.GraphObjList)
            {
                ((TextObj)a).FontSpec.FontColor = Color.Black;
            }
        }

        public void InvalidateChart()
        {
            double Num = myPane.YAxis.Scale.Max - myPane.YAxis.Scale.Min;
            myPane.YAxis.Scale.MajorStep = Num / 10;
            zed1.AxisChange();
            zed1.Invalidate();
        }

        public void setTimeIndex(int iIndex)
        {
            m_iTimeIndex = iIndex;
        }

        public bool IsAllowedDownClick(bool bIsPage)
        {
            Scale yScale = zed1.GraphPane.YAxis.Scale;
            if (bIsPage)
            {
                setTimeStep(m_iTimeIndex, false);
                m_iTimeStep = m_iTimeStep * 2;
            }
            else
            {
                setTimeStep(m_iTimeIndex);
            }

            double dMax = yScale.Max + m_iTimeStep;
            double dnow = 0.0;
            dnow = (double)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            if (dMax > dnow)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
