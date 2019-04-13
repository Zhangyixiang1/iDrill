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
    public partial class DrillingGasForm : BaseForm
    {
        private Thread t_up, t_down, t_enla, t_narr;//声明4个功能按钮线程
        delegate void UpdateChart();//更新曲线委托
        public DrillingGasForm()
        {
            InitializeComponent();
            initControl();
        }

        private void setDepthChart()
        {
            depthTimeChart1.setDepthChart(fourChart1, 1);
            depthTimeChart1.setDepthChart(fourChart2, 2);
        }

        private void InitBtn()
        {
            btn_up.Enabled = true;
            btn_down.Enabled = true;
            btn_zhong.Text = "Pause";
            btn_add.Enabled = true;
            btn_jian.Enabled = true;
        }

        private void GasForm_Load(object sender, EventArgs e)
        {
            //设置语言
            setControlLanguage();            
            if (!depthTimeChart1.ViewHistory)
            {
                btn_zhong.Text = "Pause";
            }
            else
            {
                btn_zhong.Text = "R/T";
            }

            setDepthChart();
            setFormFourChart(fourChart1, 1);
            setFormFourChart(fourChart2, 2);
            setFormDepthTimeChart(depthTimeChart1);
            //getData();
        }

        #region 功能按钮

        //向上翻页查看历史
        private void radButton6_Click(object sender, EventArgs e)
        {
            try
            {
                t_up = new Thread(UpClick);
                t_up.IsBackground = true;
                t_up.Start();
            }
            catch { }
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
                depthTimeChart1.UpClick();
                if (depthTimeChart1.isUp)
                {
                    btn_up.Enabled = false;
                }
                else
                {
                    btn_up.Enabled = true;
                }
                if (depthTimeChart1.isDown)
                {
                    btn_down.Enabled = false;
                }
                else
                {
                    btn_down.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_up.Abort();
            t_up = null;
        }
        //向下翻页查看历史
        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                t_down = new Thread(DownClick);
                t_down.IsBackground = true;
                t_down.Start();
            }
            catch { }
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
                fourChart1.DownClick();
                fourChart2.DownClick();
                depthTimeChart1.DownClick();
                if (depthTimeChart1.isUp)
                {
                    btn_up.Enabled = false;
                }
                else
                {
                    btn_up.Enabled = true;
                }
                if (depthTimeChart1.isDown)
                {
                    btn_down.Enabled = false;
                }
                else
                {
                    btn_down.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_down.Abort();
            t_down = null;
        }
        //查看实时
        private void radButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                fourChart1.RealTimeClick();
                fourChart2.RealTimeClick();
                depthTimeChart1.RealTimeClick();
                btn_up.Enabled = true;
                btn_down.Enabled = true;
                btn_jian.Enabled = true;
                btn_add.Enabled = true;
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            catch { }
        }

        //放大
        private void radButton4_Click(object sender, EventArgs e)
        {
            try
            {
                t_enla = new Thread(EnlaClick);
                t_enla.IsBackground = true;
                t_enla.Start();
            }
            catch { }       
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
                fourChart1.timeNow = depthTimeChart1.d_now;
                fourChart2.timeNow = depthTimeChart1.d_now;
                fourChart1.Enlarge();
                fourChart2.Enlarge();
                depthTimeChart1.Enlarge();
                if (depthTimeChart1.isEnlarge)
                {
                    btn_add.Enabled = false;
                }
                else
                {
                    btn_add.Enabled = true;
                }
                if (depthTimeChart1.isNarrow)
                {
                    btn_jian.Enabled = false;
                }
                else
                {
                    btn_jian.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_enla.Abort();
            t_enla = null;
        }
        //缩小
        private void radButton5_Click_1(object sender, EventArgs e)
        {
            try
            {
                t_narr = new Thread(NarrClick);
                t_narr.IsBackground = true;
                t_narr.Start();
            }
            catch { }
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
                fourChart1.timeNow = depthTimeChart1.d_now;
                fourChart2.timeNow = depthTimeChart1.d_now;
                fourChart1.Narrow();
                fourChart2.Narrow();
                depthTimeChart1.Narrow();
                if (depthTimeChart1.isEnlarge)
                {
                    btn_add.Enabled = false;
                }
                else
                {
                    btn_add.Enabled = true;
                }
                if (depthTimeChart1.isNarrow)
                {
                    btn_jian.Enabled = false;
                }
                else
                {
                    btn_jian.Enabled = true;
                }
                if (!depthTimeChart1.ViewHistory)
                {
                    btn_zhong.Text = "Pause";
                }
                else
                {
                    btn_zhong.Text = "R/T";
                }
            }
            t_narr.Abort();
            t_narr = null;
        }
        //保存截图
        private void radButton6_Click_1(object sender, EventArgs e)
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
                path.FileName = "DrillingGas_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                if (path.ShowDialog() == DialogResult.OK)
                    bit.Save(path.FileName);
            }
            catch { }
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.ShowDialog();
        }

        #region 用户控件参数定义

        private void initControl()
        {
            //设置fourChart的相关属性并启动
            fourChart1.group = 1;
            fourChart1.fname = "Gas";
            fourChart2.group = 2;
            fourChart2.fname = "Gas";
            dataShowControl1.group = 4;
            dataShowControl1.fname = "Gas";
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
                                depthTimeChart1.SetLabel(xe.GetAttribute("value"), 1);
                                depthTimeChart1.SetLabel(xe.GetAttribute("chart"), 2);
                                depthTimeChart1.SetLabel(xe.GetAttribute("message"), 3);
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

        //设置Memo
        public void setMemo(TextObj memo)
        {
            depthTimeChart1.myPane.GraphObjList.Add(memo);
        }
        //移除Memo
        public void RemoveMemo(double unix)
        {
            var t = depthTimeChart1.myPane.GraphObjList.Where(o => o.Tag != null).ToList();
            var memo = depthTimeChart1.myPane.GraphObjList.Where(o => o.Location.Y == unix && o.Tag != null).FirstOrDefault();
            depthTimeChart1.myPane.GraphObjList.Remove(memo);
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
            depthTimeChart1.isActive = isActive;
        }
        /// <summary>
        /// 激活当前窗体曲线开始刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrillingGasForm_Activated(object sender, EventArgs e)
        {
            setAllFormActivite();
            this.setChartActivite(true);
        }
    }
}