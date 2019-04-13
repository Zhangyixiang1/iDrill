using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ZedGraph;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class CirculateForm : BaseForm
    {
        private Thread m_tGetdata = null;//声明4个功能按钮线程
        delegate void UpdateChart();//更新曲线委托
        private bool m_bIsDownClick = true;

        private long m_lMax = 0;
        private long m_lMin = 0;

        public CirculateForm()
        {
            InitializeComponent();
            initControl();
        }

        private void setViewHistory(bool bIsHistory)
        {
            depthTimeChart.ViewHistory = bIsHistory;
            fourChart1.setViewHistory(bIsHistory);
            fourChart2.setViewHistory(bIsHistory);
            fourChart3.setViewHistory(bIsHistory);
        }

        private void UpdateFourChart()
        {
            if (null != fourChart1)
            {
                fourChart1.UpdateFourChart();
            }

            if (null != fourChart2)
            {
                fourChart2.UpdateFourChart();
            }

            if (null != fourChart3)
            {
                fourChart3.UpdateFourChart();
            }
        }

        public void resetTags()
        {
            setViewHistory(true);
            resetDataShow();

            if (null != dataShowControl1)
            {
                dataShowControl1.resetDataShow();
            }

            if (null != fourChart1)
            {
                fourChart1.resetDataShow();
            }

            if (null != fourChart2)
            {
                fourChart2.resetDataShow();
            }

            if (null != fourChart3)
            {
                fourChart3.resetDataShow();
            }

            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            UpdateFourChart();
            setViewHistory(false);
        }

        public void setTagValues(string strValue)
        {
            if (null != dataShowControl1)
            {
                dataShowControl1.setTagValue(strValue);
            }

            if (null != fourChart1)
            {
                fourChart1.setTagValue(strValue);
            }

            if (null != fourChart2)
            {
                fourChart2.setTagValue(strValue);
            }

            if (null != fourChart3)
            {
                fourChart3.setTagValue(strValue);
            }

            setTagValue(strValue);
        }

        public void setDrillID(int iDrillID)
        {
            fourChart1.setDrillID(iDrillID);
            fourChart2.setDrillID(iDrillID);
            fourChart3.setDrillID(iDrillID);
            depthTimeChart.setDrillID(iDrillID);
            dataShowControl1.drillID = iDrillID;
        }

        private void setDepthChart()
        {
            depthTimeChart.setDepthChart(fourChart1, 1);
            depthTimeChart.setDepthChart(fourChart2, 2);
            depthTimeChart.setDepthChart(fourChart3, 3);
        }

        private void InitBtn()
        {
            btn_Up.Enabled = true;
            btn_Down.Enabled = true;
            btn_Real.Text = "Pause";
            btn_Enlarge.Enabled = true;
            btn_Narrow.Enabled = true;
        }

        private void CirculateForm_Load(object sender, EventArgs e)
        {
            //设置语言
            setControlLanguage();
            setBtnRP();
            setDepthChart();
            setFormFourChart(fourChart1, 1);
            setFormFourChart(fourChart2, 2);
            setFormFourChart(fourChart3, 3);
            setFormDepthTimeChart(depthTimeChart);
            getData();

            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            addOldLable(m_lMin, m_lMax);
            m_delegateDataShow = showDataMessage;
            createConsumer();
        }

        #region 功能按钮

        private void InitPicLoad(bool bIsVisible)
        {
            fourChart1.pic_load.Visible = bIsVisible;
            fourChart2.pic_load.Visible = bIsVisible;
            fourChart3.pic_load.Visible = bIsVisible;
        }

        public void setBtnRP()
        {
            if (!depthTimeChart.ViewHistory)
            {
                btn_Real.Text = "Pause";
            }
            else
            {
                btn_Real.Text = "R/T";
            }
        }

        public void queryHisData()
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(query_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            }
        }

        private void setBtnTF()
        {
            int iIndex = depthTimeChart.getTimeIndex();
            if (1 == iIndex)
            {
                btn_Enlarge.Enabled = false;
                btn_Narrow.Enabled = true;
            }
            else if (10 == iIndex)
            {
                btn_Narrow.Enabled = false;
                btn_Enlarge.Enabled = true;
            }
            else
            {
                btn_Enlarge.Enabled = true;
                btn_Narrow.Enabled = true;
            }
        }

        private void query_process()
        {
            depthTimeChart.queryHis();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.setFourTimeStep();
            query();
        }

        private void query()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(query);
                Invoke(uc);
            }
            else
            {
                setBtnTF();
                fourChart1.setTimeMaxmin(m_lMin, m_lMax);
                fourChart1.queryHis();
                fourChart2.setTimeMaxmin(m_lMin, m_lMax);
                fourChart2.queryHis();
                fourChart3.setTimeMaxmin(m_lMin, m_lMax);
                fourChart3.queryHis();
                InitPicLoad(false);

                if (!depthTimeChart.ViewHistory)
                {
                    btn_Real.Text = "Pause";
                }
                else
                {
                    btn_Real.Text = "R/T";
                }

                showLable(m_lMin, m_lMax);
            }

            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        //向上翻页查看历史
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Up_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Up_process()
        {
            depthTimeChart.UpClick();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            UpClick();
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
                fourChart1.UpClick();
                fourChart2.UpClick();
                fourChart3.UpClick();
                InitPicLoad(false);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Up_process2);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            }
        }

        private void Up_process2()
        {
            depthTimeChart.UpClick2();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            UpClick2();
        }

        private void UpClick2()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(UpClick2);
                Invoke(uc);
            }
            else
            {
                fourChart1.UpClick();
                fourChart2.UpClick();
                fourChart3.UpClick();
                InitPicLoad(false);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        //向下翻页查看历史
        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Down_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Down_process()
        {
            m_bIsDownClick = depthTimeChart.IsAllowedDownClick();

            if (m_bIsDownClick)
            {
                depthTimeChart.DownClick();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            DownClick();
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
                if (!m_bIsDownClick)
                {
                    InitPicLoad(false);
                    btn_Down.Enabled = false;
                    m_tGetdata.Abort();
                    m_tGetdata = null;
                    return;
                }

                fourChart1.DownClick();
                fourChart2.DownClick();
                fourChart3.DownClick();
                InitPicLoad(false);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnEnabled(btn_Down, depthTimeChart.isDown);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        private void radButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                m_tGetdata = new Thread(Down_process2);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch { }
        }
        private void Down_process2()
        {
            m_bIsDownClick = depthTimeChart.IsAllowedDownClick();

            if (m_bIsDownClick)
            {
                depthTimeChart.DownClick2();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            DownClick2();
        }

        private void DownClick2()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(DownClick2);
                Invoke(uc);
            }
            else
            {
                if (!m_bIsDownClick)
                {
                    InitPicLoad(false);
                    btn_PageDown.Enabled = false;
                    m_tGetdata.Abort();
                    m_tGetdata = null;
                    return;
                }

                fourChart1.DownClick();
                fourChart2.DownClick();
                fourChart3.DownClick();
                InitPicLoad(false);
                setBtnEnabled(btn_PageUp, depthTimeChart.isPageUp);
                setBtnEnabled(btn_PageDown, depthTimeChart.isPageDown);
                setBtnEnabled(btn_Up, depthTimeChart.isUp);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }

            m_tGetdata.Abort();
            m_tGetdata = null;
        }

        public void realTime()
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                if (depthTimeChart.ViewHistory)
                {
                    InitPicLoad(true);
                    setBtnTF();
                }

                m_tGetdata = new Thread(Real_process);
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch
            {
            } 
        }

        private void Real_process()
        {
            if (depthTimeChart.ViewHistory)
            {
                fourChart1.timeNow = depthTimeChart.d_now;
                fourChart2.timeNow = depthTimeChart.d_now;
                fourChart3.timeNow = depthTimeChart.d_now;
                depthTimeChart.RealTimeClick();
                depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
                getData(m_lMin, m_lMax);
                depthTimeChart.ShowMemo();
                depthTimeChart.setFourTimeStep();
            }

            RealClick();
        }

        private void RealClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(RealClick);
                Invoke(uc);
            }
            else
            {
                if (depthTimeChart.ViewHistory)
                {
                    InitPicLoad(false);
                    fourChart1.RealTimeClick();
                    fourChart2.RealTimeClick();
                    fourChart3.RealTimeClick();

                    btn_Up.Enabled = true;
                    btn_Down.Enabled = true;
                    btn_PageUp.Enabled = true;
                    btn_PageDown.Enabled = true;
                    depthTimeChart.ViewHistory = false;
                    showLable(m_lMin, m_lMax);
                }
                else
                {
                    depthTimeChart.ViewHistory = true;
                    fourChart1.setVeiwHistroy(true);
                    fourChart2.setVeiwHistroy(true);
                    fourChart3.setVeiwHistroy(true);
                }

                if (!depthTimeChart.ViewHistory)
                {
                    depthTimeChart.setReal(true);
                    btn_Real.Text = "Pause";
                }
                else
                {
                    depthTimeChart.setReal(false);
                    btn_Real.Text = "R/T";
                }

                m_tGetdata.Abort();
                m_tGetdata = null;
            }
        }
        //查看实时
        private void radButton3_Click(object sender, EventArgs e)
        {
            realTime();
        }

        //放大
        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                if (depthTimeChart.getReal())
                {
                    depthTimeChart.setVeiwHistroy(true);
                    depthTimeChart.Enlarge();
                    setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                    setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                    m_tGetdata = new Thread(Real_process);
                }
                else
                {
                    m_tGetdata = new Thread(Enla_process);
                }
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Enla_process()
        {
            fourChart1.timeNow = depthTimeChart.d_now;
            fourChart2.timeNow = depthTimeChart.d_now;
            fourChart3.timeNow = depthTimeChart.d_now;
            depthTimeChart.Enlarge();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            EnlaClick();
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

                fourChart1.Enlarge();
                fourChart2.Enlarge();
                fourChart3.Enlarge();
                InitPicLoad(false);
                setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }
        //缩小
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_tGetdata != null)
                {
                    return;
                }

                InitPicLoad(true);
                if (depthTimeChart.getReal())
                {
                    depthTimeChart.setVeiwHistroy(true);
                    depthTimeChart.Enlarge();
                    setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                    setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                    m_tGetdata = new Thread(Real_process);
                }
                else
                {
                    m_tGetdata = new Thread(Narr_process);
                }
                m_tGetdata.IsBackground = true;
                m_tGetdata.Start();
            }
            catch 
            { 
            }
        }

        private void Narr_process()
        {
            fourChart1.timeNow = depthTimeChart.d_now;
            fourChart2.timeNow = depthTimeChart.d_now;
            fourChart3.timeNow = depthTimeChart.d_now;

            depthTimeChart.Narrow();
            depthTimeChart.getMaxMin(ref m_lMax, ref m_lMin);
            getData(m_lMin, m_lMax);
            depthTimeChart.ShowMemo();
            depthTimeChart.setFourTimeStep();
            NarrClick();
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
                fourChart1.Narrow();
                fourChart2.Narrow();
                fourChart3.Narrow();
                InitPicLoad(false);
                setBtnEnabled(btn_Enlarge, depthTimeChart.isEnlarge);
                setBtnEnabled(btn_Narrow, depthTimeChart.isNarrow);
                setBtnRP();
                showLable(m_lMin, m_lMax);
            }
            m_tGetdata.Abort();
            m_tGetdata = null;
        }
        //打印截图
        private void radButton6_Click(object sender, EventArgs e)
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
                path.FileName = "Circulate_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                if (path.ShowDialog() == DialogResult.OK)
                {
                    bit.Save(path.FileName);
                }
            }
            catch { }
        }

        #endregion

        #region 用户控件参数定义
        private void initControl()
        {
            fourChart1.group = 1;
            fourChart1.fname = "Circulate";
            fourChart2.group = 2;
            fourChart2.fname = "Circulate";
            fourChart3.group = 3;
            fourChart3.fname = "Circulate";

            dataShowControl1.group = 4;
            dataShowControl1.fname = "Circulate";
        }
        #endregion

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
                                depthTimeChart.SetLabel(xe.GetAttribute("value"), 1);
                                depthTimeChart.SetLabel(xe.GetAttribute("chart"), 2);
                                depthTimeChart.SetLabel(xe.GetAttribute("message"), 3); ;
                                continue;
                            }
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

        private void CirculateForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        //设置Memo
        public void setMemo(TextObj memo)
        {
            depthTimeChart.myPane.GraphObjList.Add(memo);
        }
        //移除Memo
        public void RemoveMemo(double unix)
        {
            var t = depthTimeChart.myPane.GraphObjList.Where(o => o.Tag != null).ToList();
            var memo = depthTimeChart.myPane.GraphObjList.Where(o => o.Location.Y == unix && o.Tag != null).FirstOrDefault();
            depthTimeChart.myPane.GraphObjList.Remove(memo);
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
            fourChart1.isActive = isActive;
            fourChart2.isActive = isActive;
            fourChart3.isActive = isActive;
            depthTimeChart.isActive = isActive;
        }
        /// <summary>
        /// 激活当前窗体曲线开始刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CirculateForm_Activated(object sender, EventArgs e)
        {
            setAllFormActivite();
            this.setChartActivite(true);
        }

        private void showDataMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            if (null != dataShowControl1)
            {
                dataShowControl1.ShowMessage(realTimeData);
            }

        }
    }
}
