using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.UserControls;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using System.Threading;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class HistoryDataForm : BaseForm
    {
        public static List<HistoryDataModel> SelectedTag;
        private string depth = "var2";  //井深
        private List<HistoryDepthData> Depthdatas;
        private string PostUrl = null;
        private string PostData = null;
        private long QueryStartTime = 0;
        private long QueryEndTime = 0;
        private bool PostError;
        private string removeText;//用于移除测点之后测点翻译
        private Drill Drill = new Drill();
        private DrillOSEntities db = new DrillOSEntities();
        private List<string> lbl_message = new List<string>();
        private getHistoryData dataList;
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        private IWorkbook workbook;
        private delegate void SetStyle();
        public HistoryDataForm()
        {
            InitializeComponent();
        }

        private void HistoryDataForm_Load(object sender, EventArgs e)
        {
            try
            {
                PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();
            }
            catch { }
            SelectedTag = new List<HistoryDataModel>();
            backgroundWorker_historydata.WorkerSupportsCancellation = true;
            historyDataDepthShow1.form = this;

            #region 设置默认时间
            this.radCalendar_start.SelectedDate = DateTime.Now.AddDays(-3);
            this.radCalendar_end.SelectedDate = DateTime.Now;
            this.radCalendar_start.FocusedDate = DateTime.Now.AddDays(-3);
            this.radCalendar_end.FocusedDate = DateTime.Now;
            this.radTimePicker1.Value = Convert.ToDateTime(this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            this.radTimePicker2.Value = Convert.ToDateTime(this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            this.EndTime.Text = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");
            this.StartTime.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            #endregion

            SelectTag1.Tag = "";
            SelectTag2.Tag = "";
            SelectTag3.Tag = "";
            SelectTag4.Tag = "";
            //多语言对应
            setControlLanguage();

            if (AppDrill.language == "CN")
            {
                this.radCalendar_start.Culture = new System.Globalization.CultureInfo("zh-CN");
                this.radCalendar_end.Culture = new System.Globalization.CultureInfo("zh-CN");
            }
            try
            {
                backgroundWorker1.WorkerSupportsCancellation = true;
                backgroundWorker1.RunWorkerAsync();
            }
            catch { }
        }

        #region 测点选择按钮
        private void SelectTag1_Click(object sender, EventArgs e)
        {
            SelectTagForm form = new SelectTagForm();
            form.ThisTag = this.SelectTag1.Tag == null ? "" : this.SelectTag1.Tag.ToString();
            form.ShowDialog();
            if (form.RemoveFlag)
            {
                this.SelectTag1.Text = removeText;
                this.SelectTag1.Tag = null;
                var model = SelectedTag.Where(o => o.index == 1).FirstOrDefault();
                if (model != null)
                    SelectedTag.Remove(model);
                rlbl_left_tag1.Text = "";
                rlbl_right_tag1.Text = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(form.Tags))
                {
                    //设置曲线图量程
                    historyDataShow1.SetRange(Convert.ToDouble(form.From), Convert.ToDouble(form.To));
                    rlbl_left_tag1.Text = form.From.ToString("#0");
                    rlbl_right_tag1.Text = form.To.ToString("#0");
                    this.SelectTag1.Text = form.Captial;
                    this.SelectTag1.Tag = form.Tags;
                    var model = SelectedTag.Where(o => o.index == 1).FirstOrDefault();
                    if (model == null)
                    {
                        HistoryDataModel hdm = new HistoryDataModel();
                        hdm.index = 1;
                        hdm.Tag = form.Tags;
                        //hdm.from = form.From;
                        //hdm.to = form.To;
                        SelectedTag.Add(hdm);
                    }
                    else
                    {
                        model.Tag = form.Tags;
                        //model.from = form.From;
                        //model.to = form.To;
                    }
                }
            }           
        }

        private void SelectTag2_Click(object sender, EventArgs e)
        {
            SelectTagForm form = new SelectTagForm();
            form.ThisTag = this.SelectTag2.Tag == null ? "" : this.SelectTag2.Tag.ToString();
            form.ShowDialog();
            if (form.RemoveFlag)
            {
                this.SelectTag2.Text = removeText;
                this.SelectTag2.Tag = null;
                var model = SelectedTag.Where(o => o.index == 2).FirstOrDefault();
                if (model != null)
                    SelectedTag.Remove(model);
                rlbl_left_tag2.Text = "";
                rlbl_right_tag2.Text = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(form.Tags))
                {
                    //设置曲线图量程
                    historyDataShow2.SetRange(Convert.ToDouble(form.From), Convert.ToDouble(form.To));
                    rlbl_left_tag2.Text = form.From.ToString("#0");
                    rlbl_right_tag2.Text = form.To.ToString("#0");
                    this.SelectTag2.Text = form.Captial;
                    this.SelectTag2.Tag = form.Tags;
                    var model = SelectedTag.Where(o => o.index == 2).FirstOrDefault();
                    if (model == null)
                    {
                        HistoryDataModel hdm = new HistoryDataModel();
                        hdm.index = 2;
                        hdm.Tag = form.Tags;
                        //hdm.from = form.From;
                        //hdm.to = form.To;
                        SelectedTag.Add(hdm);
                    }
                    else
                    {
                        model.Tag = form.Tags;
                        //model.from = form.From;
                        //model.to = form.To;
                    }
                }
            }
        }

        private void SelectTag3_Click(object sender, EventArgs e)
        {
            SelectTagForm form = new SelectTagForm();
            form.ThisTag = this.SelectTag3.Tag == null ? "" : this.SelectTag3.Tag.ToString();
            form.ShowDialog();
            if (form.RemoveFlag)
            {
                this.SelectTag3.Text = removeText;
                this.SelectTag3.Tag = null;
                var model = SelectedTag.Where(o => o.index == 3).FirstOrDefault();
                if (model != null)
                    SelectedTag.Remove(model);
                rlbl_left_tag3.Text = "";
                rlbl_right_tag3.Text = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(form.Tags))
                {
                    //设置曲线图量程
                    historyDataShow3.SetRange(Convert.ToDouble(form.From), Convert.ToDouble(form.To));
                    rlbl_left_tag3.Text = form.From.ToString("#0");
                    rlbl_right_tag3.Text = form.To.ToString("#0");
                    this.SelectTag3.Text = form.Captial;
                    this.SelectTag3.Tag = form.Tags;
                    var model = SelectedTag.Where(o => o.index == 3).FirstOrDefault();
                    if (model == null)
                    {
                        HistoryDataModel hdm = new HistoryDataModel();
                        hdm.index = 3;
                        hdm.Tag = form.Tags;
                        //hdm.from = form.From;
                        //hdm.to = form.To;
                        SelectedTag.Add(hdm);
                    }
                    else
                    {
                        model.Tag = form.Tags;
                        //model.from = form.From;
                        //model.to = form.To;
                    }
                }
            }
        }

        private void SelectTag4_Click(object sender, EventArgs e)
        {
            SelectTagForm form = new SelectTagForm();
            form.ThisTag = this.SelectTag4.Tag == null ? "" : this.SelectTag4.Tag.ToString();
            form.ShowDialog();
            if (form.RemoveFlag)
            {
                this.SelectTag4.Text = removeText;
                this.SelectTag4.Tag = null;
                var model = SelectedTag.Where(o => o.index == 4).FirstOrDefault();
                if (model != null)
                    SelectedTag.Remove(model);
                rlbl_left_tag4.Text = "";
                rlbl_right_tag4.Text = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(form.Tags))
                {
                    //设置曲线图量程
                    historyDataShow4.SetRange(Convert.ToDouble(form.From), Convert.ToDouble(form.To));
                    rlbl_left_tag4.Text = form.From.ToString("#0");
                    rlbl_right_tag4.Text = form.To.ToString("#0");
                    this.SelectTag4.Text = form.Captial;
                    this.SelectTag4.Tag = form.Tags;
                    var model = SelectedTag.Where(o => o.index == 4).FirstOrDefault();
                    if (model == null)
                    {
                        HistoryDataModel hdm = new HistoryDataModel();
                        hdm.index = 4;
                        hdm.Tag = form.Tags;
                        //hdm.from = form.From;
                        //hdm.to = form.To;
                        SelectedTag.Add(hdm);
                    }
                    else
                    {
                        model.Tag = form.Tags;
                        //model.from = form.From;
                        //model.to = form.To;
                    }
                }
            }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Query_Click(object sender, EventArgs e)
        {
            if (SelectedTag.Count <= 0)
            {
                MessageBox.Show(lbl_message[2]);
                return;
            }            
            this.pl_CalendarAndTime.Visible = false;
            this.pl_CalendarAndTime2.Visible = false;
            long startTime = ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
            long endTime = ConvertDateTimeInt(DateTime.Parse(this.EndTime.Text));
            long longStartTime = Comm.ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
            var count = ConvertDateTimeInt(DateTime.Parse(this.EndTime.Text)) - ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
            if (startTime > endTime)
            {
                //开始和结束时间位置互换
                string sTime = this.StartTime.Text;
                string eTime = this.EndTime.Text;
                this.StartTime.Text = DateTime.Parse(eTime).ToString("yyyy-MM-dd HH:mm:ss");
                this.EndTime.Text = DateTime.Parse(sTime).ToString("yyyy-MM-dd HH:mm:ss");
                this.radCalendar_start.SelectedDate = DateTime.Parse(eTime);
                this.radCalendar_end.SelectedDate = DateTime.Parse(sTime);
                this.radCalendar_start.FocusedDate = DateTime.Parse(eTime);
                this.radCalendar_end.FocusedDate = DateTime.Parse(sTime);
                this.radTimePicker1.Value = Convert.ToDateTime(eTime);
                this.radTimePicker2.Value = Convert.ToDateTime(sTime);
                //时间获取并转换
                startTime = ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
                endTime = ConvertDateTimeInt(DateTime.Parse(this.EndTime.Text));
                longStartTime = Comm.ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
                count = ConvertDateTimeInt(DateTime.Parse(this.EndTime.Text)) - ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
            }
            QueryStartTime = startTime;
            QueryEndTime = endTime;
            //查询时间不能相同
            if (QueryStartTime == QueryEndTime)
            {
                MessageBox.Show(lbl_message[0]);
                return;
            }
            //查询时间不能超过3天
            if ((QueryEndTime - QueryStartTime) > 259200)
            {
                MessageBox.Show(lbl_message[1]);
                return;
            }
            rpnl_load.Visible = true;
            if (!backgroundWorker_historydata.IsBusy)
            {
                this.Cursor = Cursors.WaitCursor;//等待
                //初始化曲线图
                historyDataShow1.SetChart(longStartTime, count);
                historyDataShow2.SetChart(longStartTime, count);
                historyDataShow3.SetChart(longStartTime, count);
                historyDataShow4.SetChart(longStartTime, count);
                //初始化井深
                historyDataDepthShow1.setDepth(longStartTime, (int)count);
                backgroundWorker_historydata.RunWorkerAsync();
            }
        }
        #endregion

        #region 异步获取数据
        private void backgroundWorker_historydata_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                PostError = false;
                if (SelectedTag.Count == 0)
                {
                    CursorsDefault();
                    return;
                }
                QueryHistory model = new QueryHistory();
                model.startTime = QueryStartTime;
                model.endTime = QueryEndTime;
                model.DrillId = AppDrill.DrillID;
                model.DepthTag = depth;
                List<string> tag = new List<string>();
                foreach (var item in HistoryDataForm.SelectedTag)
                {
                    tag.Add(item.Tag);
                }
                model.Tag = tag;
                PostData = null;
                PostData = new JavaScriptSerializer().Serialize(model);

                var QueryData = Comm.HttpPost(PostUrl, PostData);
                if (!string.IsNullOrEmpty(QueryData))
                {
                    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                    jsSerializer.MaxJsonLength = Int32.MaxValue;
                    dataList = jsSerializer.Deserialize<getHistoryData>(QueryData); //反序列化
                    Depthdatas = new List<HistoryDepthData>();
                    Depthdatas = dataList.Depthdatas;
                    foreach (var item in dataList.datas)
                    {
                        var IndexModel = SelectedTag.Where(o => o.Tag == item.Tag).FirstOrDefault();
                        DateTime d4 = DateTime.Now;
                        if (IndexModel != null)
                        {
                            switch (IndexModel.index)
                            {
                                case 1:
                                    historyDataShow1.upChart(item.Datas, 1, item.Tag);
                                    break;
                                case 2:
                                    historyDataShow2.upChart(item.Datas, 2, item.Tag);
                                    break;
                                case 3:
                                    historyDataShow3.upChart(item.Datas, 3, item.Tag);
                                    break;
                                case 4:
                                    historyDataShow4.upChart(item.Datas, 4, item.Tag);
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                PostError = true;
            }
        }

        private void backgroundWorker_historydata_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!PostError)
            {
                historyDataDepthShow1.upDepth(Depthdatas); //绘制井深
                backgroundWorker_historydata.CancelAsync();
            }
            else
                CursorsDefault();
            rpnl_load.Visible = false;
        }
        #endregion


        #region 数据操作按钮
        /// <summary>
        /// 上翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void up_btn_Click(object sender, EventArgs e)
        {
            historyDataShow1.UpClick();
            historyDataShow2.UpClick();
            historyDataShow3.UpClick();
            historyDataShow4.UpClick();
            historyDataDepthShow1.UpClick();
        }

        /// <summary>
        /// 下翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void down_btn_Click(object sender, EventArgs e)
        {
            historyDataShow1.DownClick();
            historyDataShow2.DownClick();
            historyDataShow3.DownClick();
            historyDataShow4.DownClick();
            historyDataDepthShow1.DownClick();
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enlarge_btn_Click(object sender, EventArgs e)
        {
            historyDataShow1.Enlarge();
            historyDataShow2.Enlarge();
            historyDataShow3.Enlarge();
            historyDataShow4.Enlarge();
            historyDataDepthShow1.Enlarge();
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void narrow_btn_Click(object sender, EventArgs e)
        {
            historyDataShow1.Narrow();
            historyDataShow2.Narrow();
            historyDataShow3.Narrow();
            historyDataShow4.Narrow();
            historyDataDepthShow1.Narrow();
        }

        /// <summary>
        /// 保存截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pickture_btn_Click(object sender, EventArgs e)
        {
            try
            {
                HistoryDataPrtsc h = new HistoryDataPrtsc();
                h.drillNo = rtxt_drillNo.Text;
                h.lease = rtxt_lease.Text;
                h.company = rtxt_company.Text;
                h.country = rtxt_country.Text;
                h.contractor = rtxt_contractor.Text;
                h.dateSpud = rtxt_ds.Text;
                h.toolPusher = rtxt_tp.Text;
                h.daterelease = rtxt_dr.Text;
                h.companyman = rtxt_cm.Text;
                h.tag1 = SelectedTag.Count >= 1 ? SelectTag1.Text : "";
                h.tag2 = SelectedTag.Count >= 2 ? SelectTag2.Text : "";
                h.tag3 = SelectedTag.Count >= 3 ? SelectTag3.Text : "";
                h.tag4 = SelectedTag.Count >= 5 ? SelectTag4.Text : "";
                h.time_from = historyDataDepthShow1.GetRange()[0];
                h.time_to = historyDataDepthShow1.GetRange()[1];
                h.from1 = double.Parse(rlbl_left_tag1.Text);
                h.to1 = double.Parse(rlbl_right_tag1.Text);
                h.from2 = double.Parse(rlbl_left_tag2.Text);
                h.to2 = double.Parse(rlbl_right_tag2.Text);
                h.from3 = double.Parse(rlbl_left_tag3.Text);
                h.to3 = double.Parse(rlbl_right_tag3.Text);
                h.from4 = double.Parse(rlbl_left_tag4.Text);
                h.to4 = double.Parse(rlbl_right_tag4.Text);
                h.listText = historyDataDepthShow1.GetTextObj();
                if (SelectedTag.Count <= 0)
                {
                    MessageBox.Show(lbl_message[2]);
                    return;
                }
                h.iList1 = historyDataShow1.GetPointList(0);
                h.iList2 = historyDataShow2.GetPointList(1);
                h.iList3 = historyDataShow3.GetPointList(2);
                h.iList4 = historyDataShow4.GetPointList(3);
                h.ShowDialog();
                if (h.clickOK)
                {
                    //路径选择
                    SaveFileDialog path = new SaveFileDialog();
                    path.FileName = "HistoryData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                    if (path.ShowDialog() == DialogResult.OK)
                        h.bit.Save(path.FileName);
                }
            }
            catch { }
        }
        #endregion

        #region 时间选择框事件
        private void StartTime_Click(object sender, EventArgs e)
        {
            if (this.pl_CalendarAndTime2.Visible)
                this.pl_CalendarAndTime2.Visible = false;
            if (this.pl_CalendarAndTime.Visible)
                this.pl_CalendarAndTime.Visible = false;
            else
                this.pl_CalendarAndTime.Visible = true;
        }

        private void EndTime_Click(object sender, EventArgs e)
        {
            if (this.pl_CalendarAndTime.Visible)
                this.pl_CalendarAndTime.Visible = false;
            if (this.pl_CalendarAndTime2.Visible)
                this.pl_CalendarAndTime2.Visible = false;
            else
                this.pl_CalendarAndTime2.Visible = true;
        }

        private void TimePick_OK_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_start.SelectedDate.Year != 1900)
                this.StartTime.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            pl_CalendarAndTime.Visible = false;
        }

        private void TimePick_No_Click(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
        }

        private void TimePick_OK2_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_end.SelectedDate.Year != 1900)
                this.EndTime.Text = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");
            pl_CalendarAndTime2.Visible = false;
        }

        private void TimePick_NO2_Click(object sender, EventArgs e)
        {
            pl_CalendarAndTime2.Visible = false;
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
                            string sss = xe.GetAttribute("key");
                            if (this.Name == xe.GetAttribute("key"))
                            {
                                this.Text = xe.GetAttribute("value");
                            }
                            if (xe.GetAttribute("key") == "lbl_message")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                foreach (XmlNode node3 in xn_list2)
                                {
                                    XmlElement xe3 = (XmlElement)node3;
                                    lbl_message.Add(xe3.GetAttribute("value"));
                                }
                                continue;
                            }
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "rpnl_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu" || c.Name == "pl_CalendarAndTime" || c.Name == "pl_CalendarAndTime2")
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
                                    if (c.Name == "SelectTag1")
                                        removeText = xe.GetAttribute("value");
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
        /// 恢复鼠标状态
        /// </summary>
        public void CursorsDefault()
        {
            this.Cursor = Cursors.Default;//正常状态
        }

        private void HistoryDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SelectedTag = new List<HistoryDataModel>();
            this.Cursor = Cursors.Default;//正常状态
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        private long ConvertDateTimeInt(System.DateTime time)
        {
            long intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt64((time - startTime).TotalSeconds);
            return intResult;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Drill = db.Drill.Where(O => O.ID == AppDrill.DrillID).FirstOrDefault();
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (Drill != null)
            {
                rtxt_cm.Text = Drill.CompanyMan;
                rtxt_company.Text = Drill.Company;
                rtxt_contractor.Text = Drill.Contractor;
                rtxt_country.Text = Drill.Country;
                rtxt_dr.Text = Drill.DateRelease;
                rtxt_drillNo.Text = Drill.DrillNo;
                rtxt_ds.Text = Drill.DateSpud;
                rtxt_lease.Text = Drill.Lease;
                rtxt_tp.Text = Drill.ToolPusher;
            }
            backgroundWorker1.CancelAsync();
        }

        private void rbtn_export_Click(object sender, EventArgs e)
        {
            rbtn_export.Enabled = false;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (SelectedTag.Count == 0 || dataList.datas.Count == 0)
                return;//测点没选中或者无数据直接return
            workbook = new XSSFWorkbook();//创建Workbook对象 XSSFWorkbook 生成格式为.xlsx，HSSFWorkbook生成格式.xls
            ISheet sheet = workbook.CreateSheet("HistoryData");//创建工作表
            ICell cell;
            sheet.AutoSizeColumn(7, false);
            //处理hole表
            IRow row = sheet.CreateRow(0);//在工作表中添加一行
            cell = row.CreateCell(0);
            cell.SetCellValue("DateTime");
            for (int i = 0; i < SelectedTag.Count; i++)
            {
                cell = row.CreateCell(i + 1);//从i+1列开始
                cell.SetCellValue(Transformation(SelectedTag[i].Tag));//设置当前测点的名称
            }
            //循环将数据写入excel
            for (int i = 0; i < dataList.datas[0].Datas.Count; i++)
            {
                //注意顺序检查
                row = sheet.CreateRow(i + 1);//创建一行
                for (int j = 0; j < dataList.datas.Count; j++)
                {
                    //创建时间列
                    if (j == 0)
                    {
                        cell = row.CreateCell(j);
                        cell.SetCellValue(Comm.ConvertIntDateTime(dataList.datas[j].Datas[i].Timestamp * 1000).ToString());
                    }
                    cell = row.CreateCell(j + 1);//从第二列开始
                    cell.SetCellValue(dataList.datas[j].Datas[i].Value);
                }
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_export.Enabled = true;
            try
            {
                if (workbook != null)
                {
                    //路径选择
                    SaveFileDialog path = new SaveFileDialog();
                    path.FileName = "HistoryDataForm_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    path.Filter = "(*.xlsx)|*.xlsx";
                    path.ShowDialog();
                    //生成文件流
                    FileStream file = new FileStream(path.FileName, FileMode.Create);
                    if (file != null)
                        workbook.Write(file);
                    file.Close();
                }
            }
            catch { }
            backgroundWorker2.CancelAsync();    //取消挂起的后台操作。
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

        #region
        /// <summary>
        /// 设置需要截图的风格
        /// </summary>
        private void SetStyleBW()
        {
            this.BackColor = Color.White;
            lbl_well.BackColor = Color.White;
            lbl_well.ForeColor = Color.Black;
        }
        #endregion

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.InvokeRequired)
            {
                SetStyle st = new SetStyle(SetStyleBW);
                Invoke(st);
            }
            else
            {
                SetStyleBW();
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Bitmap bit = new Bitmap(1670, 995);//实例化一个和窗体一样大的bitmap
            Graphics g = Graphics.FromImage(bit);
            g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
            g.CopyFromScreen(this.Left, this.Top, 0, -108, new Size(1670, 995));//截取需要的数据
            //g.CopyFromScreen(panel.PointToScreen(Point.Empty), Point.Empty, panel.Size);//只保存某个控件（这里是panel）
            //路径选择
            SaveFileDialog path = new SaveFileDialog();
            path.FileName = "HistoryData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
            if (path.ShowDialog() == DialogResult.OK)
                bit.Save(path.FileName);
            backgroundWorker3.CancelAsync();
        }
    }
}
