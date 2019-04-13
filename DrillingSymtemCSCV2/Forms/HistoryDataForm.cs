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
        private string removeText;//用于移除测点之后测点翻译
        private Drill Drill = new Drill();
        private DrillOSEntities db = new DrillOSEntities();
        private List<string> lbl_message = new List<string>();
        private getHistoryData dataList;
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        private List<Button> m_listBtn = new List<Button>();
        private IWorkbook workbook;
        private delegate void SetStyle();


        private int m_idepthVasibleSetp = 0;
        private int m_iWellID = -1;
        private int m_iRowIndex = 0;
        private Thread thGetdata = null;
        private bool m_bIsExport = false;
        private bool m_bIsNeedEnd = false;
        private string m_strText = string.Empty;
        private const int  halfHour= 1800;
        private ExportTime m_exportForm = null;
        private string m_strStartTime = string.Empty;
        private string m_strEndTime = string.Empty;
        private long m_lStartTime = 0L;
        private long m_lEndTime = 0L;
        private int m_iTimeIndex = 3;
        private bool m_bIsReal = true;
        private bool m_bIsDownClick = true;
        private delegate void UpdateChart();
        private List<TagValue> m_listTagValues;

        public HistoryDataForm()
        {
            InitializeComponent();
        }

        public void setStartEnd(bool bStartEnd)
        {
            m_bIsNeedEnd = bStartEnd;
        }

        public void setStartEndTime(long lStartTime, long lEndTime)
        {
            QueryStartTime = lStartTime;
            QueryEndTime = lEndTime;
        }

        private void showLable(bool bIsNeedHide)
        {
            label_end.Visible = bIsNeedHide;
            label3.Visible = bIsNeedHide;
            EndTime.Visible = bIsNeedHide;
        }

        private void addBtns()
        {
            m_listBtn.Add(SelectTag1);
            m_listBtn.Add(SelectTag2);
            m_listBtn.Add(SelectTag3);
            m_listBtn.Add(SelectTag4);
            m_listBtn.Add(SelectTag5);
            m_listBtn.Add(SelectTag6);
            m_listBtn.Add(SelectTag7);
            m_listBtn.Add(SelectTag8);
            m_listBtn.Add(SelectTag9);
            m_listBtn.Add(SelectTag10);
            m_listBtn.Add(SelectTag11);
            m_listBtn.Add(SelectTag12);
            m_listBtn.Add(SelectTag13);
            m_listBtn.Add(SelectTag14);
            m_listBtn.Add(SelectTag15);
            m_listBtn.Add(SelectTag16);
        }

        private void InitSelectTags()
        {
            for (int i = 0; i < m_listBtn.Count; ++i)
            {
                m_listBtn[i].Tag = "";
                m_listBtn[i].FlatAppearance.BorderColor = Color.FromArgb(153, 153, 153);
            }
        }

        private void InitLables()
        {
            radLabel1.Text = string.Empty;
            radLabel2.Text = string.Empty;
            radLabel3.Text = string.Empty;
            radLabel4.Text = string.Empty;
            radLabel5.Text = string.Empty;
            radLabel6.Text = string.Empty;
            radLabel7.Text = string.Empty;
            radLabel8.Text = string.Empty;
            radLabel9.Text = string.Empty;
            radLabel10.Text = string.Empty;
            radLabel11.Text = string.Empty;
            radLabel12.Text = string.Empty;
            radLabel13.Text = string.Empty;
            radLabel14.Text = string.Empty;
            radLabel15.Text = string.Empty;
            radLabel16.Text = string.Empty;
            radLabel17.Text = string.Empty;
            radLabel18.Text = string.Empty;
            radLabel19.Text = string.Empty;
            radLabel20.Text = string.Empty;
            radLabel21.Text = string.Empty;
            radLabel22.Text = string.Empty;
            radLabel23.Text = string.Empty;
            radLabel24.Text = string.Empty;
            radLabel25.Text = string.Empty;
            radLabel26.Text = string.Empty;
            radLabel27.Text = string.Empty;
            radLabel28.Text = string.Empty;
            radLabel29.Text = string.Empty;
            radLabel30.Text = string.Empty;
            radLabel31.Text = string.Empty;
            radLabel32.Text = string.Empty;

        }

        public void clearHistoryCharts()
        {
            historyDataShow1.clearLines();
            historyDataShow2.clearLines();
            historyDataShow3.clearLines();
            historyDataShow4.clearLines();
            historyDataDepthShow1.clearLines();
        }

        private void HistoryDataForm_Load(object sender, EventArgs e)
        {
           
            try
            {
                PostUrl = System.Configuration.ConfigurationManager.AppSettings["QueryHistoryData"].ToString();
            }
            catch 
            { 
            }

            SelectedTag = new List<HistoryDataModel>();
            historyDataDepthShow1.form = this;

            #region 设置默认时间
            this.radCalendar_start.SelectedDate = DateTime.Now;
            //this.radCalendar_end.SelectedDate = DateTime.Now;
            this.radCalendar_start.FocusedDate = DateTime.Now;
            //this.radCalendar_end.FocusedDate = DateTime.Now;
            this.radTimePicker1.Value = Convert.ToDateTime(this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            //this.radTimePicker2.Value = Convert.ToDateTime(this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
            this.EndTime.Text = string.Empty;
            this.StartTime.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            getTime();
            StartTime.ForeColor = Color.YellowGreen;
            EndTime.ForeColor = Color.YellowGreen;

            #endregion
            addBtns();
            InitSelectTags();
            InitLables();
            showLable(false);
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
            catch 
            { 
            }
        }

        public void setChartTimeStep()
        {
            int iTimeStep = historyDataDepthShow1.getTimeStep();

            if (historyDataShow1 != null)
            {
                historyDataShow1.setTimeStep(iTimeStep);
            }

            if (historyDataShow2 != null)
            {
                historyDataShow2.setTimeStep(iTimeStep);
            }

            if (historyDataShow3 != null)
            {
                historyDataShow3.setTimeStep(iTimeStep);
            }

            if (historyDataShow4 != null)
            {
                historyDataShow4.setTimeStep(iTimeStep);
            }
        }

        private void setPic(bool bVisible)
        {
            historyDataShow1.pic_load.Visible = bVisible;
            historyDataShow2.pic_load.Visible = bVisible;
            historyDataShow3.pic_load.Visible = bVisible;
            historyDataShow4.pic_load.Visible = bVisible;
        }

        private void clear()
        {
            historyDataShow1.clearLines();
            historyDataShow2.clearLines();
            historyDataShow3.clearLines();
            historyDataShow4.clearLines();
            historyDataDepthShow1.clearLines();
        }

        private void clearHistoryLine(HistoryDataModel model)
        {
            if (model.index >= 1 && model.index <= 4)
            {
                historyDataShow1.clearLine(getIndex(model.index) - 1);
            }
            else if (model.index > 4 && model.index <= 8)
            {
                historyDataShow2.clearLine(getIndex(model.index) - 1);
            }
            else if (model.index > 8 && model.index <= 12)
            {
                historyDataShow3.clearLine(getIndex(model.index) - 1);
            }
            else
            {
                historyDataShow4.clearLine(getIndex(model.index) - 1);
            }
        }

        private void updateListLine(HistoryDataModel model)
        {
            if (null == dataList)
            {
                return;
            }

            var item = dataList.datas.Where(o => o.Tag == model.Tag).FirstOrDefault();

            if (item != null)
            {
                if (model.index >= 1 && model.index <= 4)
                {
                    historyDataShow1.setFrom2To(model.from, model.to);
                    historyDataShow1.setCures(getIndex(model.index), model.from, model.to);
                    historyDataShow1.clearLine(getIndex(model.index) - 1);
                    historyDataShow1.upChart(item.Datas, getIndex(model.index), item.Tag);
                }
                else if (model.index > 4 && model.index <= 8)
                {
                    historyDataShow2.setFrom2To(model.from, model.to);
                    historyDataShow2.setCures(getIndex(model.index), model.from, model.to);
                    historyDataShow2.clearLine(getIndex(model.index) - 1);
                    historyDataShow2.upChart(item.Datas, getIndex(model.index), item.Tag);
                }
                else if (model.index > 8 && model.index <= 12)
                {
                    historyDataShow3.setFrom2To(model.from, model.to);
                    historyDataShow3.setCures(getIndex(model.index), model.from, model.to);
                    historyDataShow3.clearLine(getIndex(model.index) - 1);
                    historyDataShow3.upChart(item.Datas, getIndex(model.index), item.Tag);
                }
                else
                {
                    historyDataShow4.setFrom2To(model.from, model.to);
                    historyDataShow4.setCures(getIndex(model.index), model.from, model.to);
                    historyDataShow4.clearLine(getIndex(model.index) - 1);
                    historyDataShow4.upChart(item.Datas, getIndex(model.index), item.Tag);
                }
            }
        }

        private void setCaptals(string strCaptal, string strFrom, string strTo, int iIndex)
        {
            if (iIndex >= 1 && iIndex <= 4)
            {
                historyDataShow1.setCaptals(strCaptal, strFrom, strTo, getIndex(iIndex));
            }
            else if (iIndex > 4 && iIndex <= 8)
            {
                historyDataShow2.setCaptals(strCaptal, strFrom, strTo, getIndex(iIndex));
            }
            else if (iIndex > 8 && iIndex <= 12)
            {
                historyDataShow3.setCaptals(strCaptal, strFrom, strTo, getIndex(iIndex));
            }
            else
            {
                historyDataShow4.setCaptals(strCaptal, strFrom, strTo, getIndex(iIndex));
            }
        }

        private void setDefaultTags()
        {
            List<DrillTag> TagList = null;
            TagList = db.DrillTag.Where(o => o.DrillId == 1).ToList();
            if (null == TagList)
            {
                return;
            }

            SelectedTag.Clear();
            var tag = TagList.Where(o => o.Tag == "var4").FirstOrDefault();
            selectTag(tag, SelectTag1, radLabel1, radLabel2, 1);

            tag = TagList.Where(o => o.Tag == "var5").FirstOrDefault();
            selectTag(tag, SelectTag5, radLabel3, radLabel6, 2);

            tag = TagList.Where(o => o.Tag == "var7").FirstOrDefault();
            selectTag(tag, SelectTag6, radLabel4, radLabel7, 3);

            tag = TagList.Where(o => o.Tag == "var20").FirstOrDefault();
            selectTag(tag, SelectTag8, radLabel24, radLabel9, 5);

            tag = TagList.Where(o => o.Tag == "var6").FirstOrDefault();
            selectTag(tag, SelectTag2, radLabel23, radLabel10, 6);

            tag = TagList.Where(o => o.Tag == "var12").FirstOrDefault();
            selectTag(tag, SelectTag10, radLabel21, radLabel12, 8);

            tag = TagList.Where(o => o.Tag == "var19").FirstOrDefault();
            selectTag(tag, SelectTag9, radLabel22, radLabel11, 7);

            tag = TagList.Where(o => o.Tag == "var21").FirstOrDefault();
            selectTag(tag, SelectTag11, radLabel28, radLabel13, 9);

            tag = TagList.Where(o => o.Tag == "var8").FirstOrDefault();
            selectTag(tag, SelectTag12, radLabel27, radLabel14, 10);

            tag = TagList.Where(o => o.Tag == "var13").FirstOrDefault();
            selectTag(tag, SelectTag13, radLabel25, radLabel16, 12);

            tag = TagList.Where(o => o.Tag == "var15").FirstOrDefault();
            selectTag(tag, SelectTag14, radLabel32, radLabel17, 13);

            tag = TagList.Where(o => o.Tag == "var16").FirstOrDefault();
            selectTag(tag, SelectTag15, radLabel31, radLabel18, 14);

            tag = TagList.Where(o => o.Tag == "var2").FirstOrDefault();
            selectTag(tag, SelectTag16, radLabel30, radLabel19, 15);

            tag = TagList.Where(o => o.Tag == "var24").FirstOrDefault();
            selectTag(tag, SelectTag7, radLabel5, radLabel8, 4);
        }

        private void selectTag(DrillTag drillTag, Button selectBtn, Telerik.WinControls.UI.RadLabel rlblLeft, Telerik.WinControls.UI.RadLabel rlblRight, int iIndex)
        {
            if (null == drillTag)
            {
                return;
            }

            string strCaptial = Transformation(drillTag.Tag);
            rlblLeft.Text = drillTag.DefaultFrom.ToString();
            rlblRight.Text = drillTag.DefaultTo.ToString();
            selectBtn.Text = strCaptial + "(" + drillTag.Unit + ")";
            selectBtn.Tag = drillTag.Tag;
            setCaptals(strCaptial, rlblLeft.Text, rlblRight.Text, iIndex);
            HistoryDataModel hdm = new HistoryDataModel();
            if (null != hdm)
            {
                hdm.index = iIndex;
                hdm.Tag = drillTag.Tag;
                hdm.from = double.Parse(rlblLeft.Text);
                hdm.to = double.Parse(rlblRight.Text);
                SelectedTag.Add(hdm);
            }
        }

        private void selectTag(Button selectBtn, Telerik.WinControls.UI.RadLabel rlblLeft, Telerik.WinControls.UI.RadLabel rlblRight, int iIndex)
        {
           SelectTagForm form = new SelectTagForm();
            form.ThisTag = selectBtn.Tag == null ? "" : selectBtn.Tag.ToString();
            form.ShowDialog();

            if (form.RemoveFlag)
            {
                selectBtn.Text = removeText;
                selectBtn.Tag = null;

                var model = SelectedTag.Where(o => o.index == iIndex).FirstOrDefault();

                if (model != null)
                {
                    clearHistoryLine(model);
                    SelectedTag.Remove(model);
                }

                rlblLeft.Text = "";
                rlblRight.Text = "";
            }
            else
            {
                if (!string.IsNullOrEmpty(form.Tags))
                {
                    //设置曲线图量程
                    historyDataShow1.SetRange(Convert.ToDouble(form.From), Convert.ToDouble(form.To));
                    rlblLeft.Text = form.From.ToString("#0");
                    rlblRight.Text = form.To.ToString("#0");
                    selectBtn.Text = form.Captial + "(" + form.Unit + ")";
                    selectBtn.Tag = form.Tags;

                    setCaptals(form.Captial, rlblLeft.Text, rlblRight.Text, iIndex);
                    var model = SelectedTag.Where(o => o.index == iIndex).FirstOrDefault();

                    if (model == null)
                    {
                        HistoryDataModel hdm = new HistoryDataModel();
                        hdm.index = iIndex;
                        hdm.Tag = form.Tags;
                        hdm.from = double.Parse(rlblLeft.Text);
                        hdm.to = double.Parse(rlblRight.Text);
                        SelectedTag.Add(hdm);
                        updateListLine(hdm);
                    }
                    else
                    {
                        model.Tag = form.Tags;
                        model.from = double.Parse(rlblLeft.Text);
                        model.to = double.Parse(rlblRight.Text);
                        updateListLine(model);
                    }
                }
            }           
        }

        #region 测点选择按钮
        private void SelectTag1_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag1, radLabel1, radLabel2, 1);
        }

        private void SelectTag2_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag2, radLabel23, radLabel10, 6);
        }

        private void SelectTag3_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag3, radLabel26, radLabel15, 11);
        }

        private void SelectTag4_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag4, radLabel29, radLabel20, 16);
        }
        #endregion

        private bool SelectedTagMessage()
        {
            if (SelectedTag.Count <= 0)
            {
                MessageBox.Show(lbl_message[2]);
                return false;
            }

            return true;
        }

        public bool queryInit(bool bIsGetTime = true)
        {
            if (SelectedTag.Count <= 0)
            {
                MessageBox.Show(lbl_message[2]);
                return false;
            }

            if (-1 == m_iWellID)
            {
                MessageBox.Show(lbl_message[3]);
                return false;
            }

            if (bIsGetTime)
            {
                getTime();
                if (QueryStartTime == QueryEndTime)
                {
                    MessageBox.Show(lbl_message[0]);
                    return false;
                }

                //查询时间不能超过3天
                if ((m_lEndTime - m_lStartTime) > 259200)
                {
                    MessageBox.Show(lbl_message[1]);
                    return false;
                }
            }
            else
            {
                if (m_lEndTime == m_lStartTime)
                {
                    MessageBox.Show(lbl_message[0]);
                    return false;
                }

                //查询时间不能超过3天
                if ((m_lEndTime - m_lStartTime) > 259200)
                {
                    MessageBox.Show(lbl_message[1]);
                    return false;
                }
            }

            return true;
        }

        private void getTime()
        {
            this.pl_CalendarAndTime.Visible = false;
            this.pl_CalendarAndTime2.Visible = false;
            long startTime = ConvertDateTimeInt(DateTime.Parse(this.StartTime.Text));
            m_strStartTime = this.StartTime.Text;
            historyDataDepthShow1.setTimeStep(m_iTimeIndex, false);
            int iTimeStep = historyDataDepthShow1.getTimeStep();
            QueryStartTime = startTime - iTimeStep * 2;
            QueryEndTime = startTime;
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            DateTime dt = Comm.ConvertIntDateTime((double)QueryEndTime * 1000);
            setTimeText(dt, ref m_strEndTime);
        }

        private void setChart()
        {
            long count = QueryEndTime - QueryStartTime;

            historyDataShow1.SetChart(QueryStartTime, count);
            historyDataShow2.SetChart(QueryStartTime, count);
            historyDataShow3.SetChart(QueryStartTime, count);
            historyDataShow4.SetChart(QueryStartTime, count);
            //初始化井深
            historyDataDepthShow1.setDepth(QueryStartTime, (int)count);
        }

        private void queryHistory()
        {
            try
            {
                QueryHistory model = new QueryHistory();

                if (m_bIsExport)
                {
                    model.startTime = m_lStartTime;
                    model.endTime = m_lEndTime;
                }      
                else
                {
                    model.startTime = QueryStartTime;
                    model.endTime = QueryEndTime;
                }

                model.DrillId = m_iWellID;
                model.DepthTag = depth;
                model.isHistoryData = m_bIsExport;

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

                    if (!m_bIsExport)
                    {
                        clearHistoryCharts();
                    }
                }
            }
            catch
            {
            }
        }

        private void updateChart()
        {
            if (null == dataList)
            {
                return;
            }

            int iIndex = 0;
            foreach (var item in dataList.datas)
            {
                var IndexModel = SelectedTag[iIndex];
                ++iIndex;

                if (depth == item.Tag)
                {
                    if (item.Datas.Count > 0)
                    {
                        m_listTagValues = item.Datas;
                    }
                    else
                    {
                        if (m_listTagValues.Count > 0)
                        {
                            item.Datas = m_listTagValues;
                        }
                    }
                }

                if (IndexModel != null)
                {
                    if (IndexModel.index >= 1 && IndexModel.index <= 4)
                    {
                        historyDataShow1.setFrom2To(IndexModel.from, IndexModel.to);
                        historyDataShow1.setCures(getIndex(IndexModel.index), IndexModel.from, IndexModel.to);
                        historyDataShow1.upChart(item.Datas, getIndex(IndexModel.index), item.Tag);
                    }
                    else if (IndexModel.index > 4 && IndexModel.index <= 8)
                    {
                        historyDataShow2.setFrom2To(IndexModel.from, IndexModel.to);
                        historyDataShow2.setCures(getIndex(IndexModel.index), IndexModel.from, IndexModel.to);
                        historyDataShow2.upChart(item.Datas, getIndex(IndexModel.index), item.Tag);
                    }
                    else if (IndexModel.index > 8 && IndexModel.index <= 12)
                    {
                        historyDataShow3.setFrom2To(IndexModel.from, IndexModel.to);
                        historyDataShow3.setCures(getIndex(IndexModel.index), IndexModel.from, IndexModel.to);
                        historyDataShow3.upChart(item.Datas, getIndex(IndexModel.index), item.Tag);
                    }
                    else
                    {
                        historyDataShow4.setFrom2To(IndexModel.from, IndexModel.to);
                        historyDataShow4.setCures(getIndex(IndexModel.index), IndexModel.from, IndexModel.to);
                        historyDataShow4.upChart(item.Datas, getIndex(IndexModel.index), item.Tag);
                    }
                }
            }
        }

        #region 查询
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void Query_Click(object sender, EventArgs e)
        {
            try
            {
                bool bIsSuccess = queryInit();
                if (!bIsSuccess)
                {
                    return;
                }

                if (thGetdata != null)
                {
                    return;
                }

                setPic(true);
                thGetdata = new Thread(Query_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }
        private void QueryHis()
        {
            try
            {
                if (thGetdata != null)
                {
                    return;
                }

                setPic(true);
                thGetdata = new Thread(Query_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Query_process()
        {
            queryHistory();
            Query();
        }

        private void Query()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(Query);
                Invoke(uc);
            }
            else
            {
                setChart();
                updateChart();
                setPic(false);
            }


            thGetdata.Abort();
            thGetdata = null;
        }
        #endregion

        private int getIndex(int iIndex)
        {
            int iIndexValue = iIndex % 4;

            if (0 == iIndexValue)
            {
                iIndexValue = 4;
            }

            return iIndexValue;
        }

        private void showMemos()
        {
            historyDataDepthShow1.upDepth(Depthdatas);
            historyDataDepthShow1.vasibleMemo();
        }

        #region 数据操作按钮
        private void up_btn_Click(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Up_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Up_process()
        {
            historyDataDepthShow1.UpClick();
            historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            queryHistory();
            setChartTimeStep();
            showMemos();
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
                down_btn.Enabled = true;
                radButton8.Enabled = true;
                historyDataShow1.UpClick();
                historyDataShow2.UpClick();
                historyDataShow3.UpClick();
                historyDataShow4.UpClick();
                updateChart();
                setPic(false);
            }

            thGetdata.Abort();
            thGetdata = null;
        }

        private void up_btn_Click2(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Up_process2);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Up_process2()
        {
            historyDataDepthShow1.UpClick2();
            historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            queryHistory();
            setChartTimeStep();
            showMemos();
            UpClick();
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
                down_btn.Enabled = true;
                radButton8.Enabled = true;
                historyDataShow1.UpClick();
                historyDataShow2.UpClick();
                historyDataShow3.UpClick();
                historyDataShow4.UpClick();
                updateChart();
                setPic(false);
            }

            thGetdata.Abort();
            thGetdata = null;
        }

        /// <summary>
        /// 下翻页

        private void down_btn_Click(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Down_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Down_process()
        {
            m_bIsDownClick = historyDataDepthShow1.IsAllowedDownClick(false);
            if (m_bIsDownClick)
            {
                historyDataDepthShow1.DownClick();
                historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
                m_lStartTime = QueryStartTime;
                m_lEndTime = QueryEndTime;
                queryHistory();
                setChartTimeStep();
                showMemos();
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
                    setPic(false);
                    down_btn.Enabled = false;
                    thGetdata.Abort();
                    thGetdata = null;
                    return;
                }

                historyDataShow1.DownClick();
                historyDataShow2.DownClick();
                historyDataShow3.DownClick();
                historyDataShow4.DownClick();
                updateChart();
                setPic(false);
            }

            thGetdata.Abort();
            thGetdata = null;
        }

        private void down_btn_Click2(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Down_process2);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Down_process2()
        {
            m_bIsDownClick = historyDataDepthShow1.IsAllowedDownClick(true);
            if (m_bIsDownClick)
            {
                historyDataDepthShow1.DownClick2();
                historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
                m_lStartTime = QueryStartTime;
                m_lEndTime = QueryEndTime;
                queryHistory();
                setChartTimeStep();
                showMemos();
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
                    setPic(false);
                    radButton8.Enabled = false;
                    thGetdata.Abort();
                    thGetdata = null;
                    return;
                }

                historyDataShow1.DownClick();
                historyDataShow2.DownClick();
                historyDataShow3.DownClick();
                historyDataShow4.DownClick();
                updateChart();
                setPic(false);
            }

            thGetdata.Abort();
            thGetdata = null;
        }


        /// <summary>
        /// 放大

        private void setButtonEnable()
        {
            if (historyDataDepthShow1.isEnlarge)
            {
                enlarge_btn.Enabled = false;
            }
            else
            {
                enlarge_btn.Enabled = true;
            }

            if (historyDataDepthShow1.isNarrow)
            {
                narrow_btn.Enabled = false;
            }
            else
            {
                narrow_btn.Enabled = true;
            }
        }

        private void enlarge_btn_Click(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Enlarge_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Enlarge_process()
        {
            historyDataDepthShow1.Enlarge();
            historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            queryHistory();
            setChartTimeStep();
            showMemos();
            EnlargeClick();
        }

        private void EnlargeClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(EnlargeClick);
                Invoke(uc);
            }
            else
            {
                historyDataShow1.Enlarge();
                historyDataShow2.Enlarge();
                historyDataShow3.Enlarge();
                historyDataShow4.Enlarge();
                updateChart();
                setPic(false);
                setButtonEnable();
            }

            thGetdata.Abort();
            thGetdata = null;
        }

        /// <summary>
        /// 缩小

        private void narrow_btn_Click(object sender, EventArgs e)
        {
            try
            {
                bool ret = SelectedTagMessage();
                if (!ret)
                {
                    return;
                }

                setPic(true);
                if (thGetdata != null)
                {
                    return;
                }

                thGetdata = new Thread(Narrow_process);
                thGetdata.IsBackground = true;
                thGetdata.Start();
            }
            catch
            {
            }
        }

        private void Narrow_process()
        {
            historyDataDepthShow1.Narrow();
            historyDataDepthShow1.getMaxMin(ref QueryEndTime, ref QueryStartTime);
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            queryHistory();
            setChartTimeStep();
            showMemos();
            NarrowClick();
        }

        private void NarrowClick()
        {
            if (this.InvokeRequired)
            {
                UpdateChart uc = new UpdateChart(NarrowClick);
                Invoke(uc);
            }
            else
            {
                historyDataShow1.Narrow();
                historyDataShow2.Narrow();
                historyDataShow3.Narrow();
                historyDataShow4.Narrow();
                updateChart();
                setPic(false);
                setButtonEnable();
            }

            thGetdata.Abort();
            thGetdata = null;
        }

        /// <summary>
        /// 保存截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getPointList(HistoryDataPrtsc hisPrtsc)
        {
            hisPrtsc.iList1 = historyDataShow1.GetPointList(0);
            hisPrtsc.iList2 = historyDataShow1.GetPointList(1);
            hisPrtsc.iList3 = historyDataShow1.GetPointList(2);
            hisPrtsc.iList4 = historyDataShow1.GetPointList(3);

            hisPrtsc.iList5 = historyDataShow2.GetPointList(0);
            hisPrtsc.iList6 = historyDataShow2.GetPointList(1);
            hisPrtsc.iList7 = historyDataShow2.GetPointList(2);
            hisPrtsc.iList8 = historyDataShow2.GetPointList(3);

            hisPrtsc.iList9 = historyDataShow3.GetPointList(0);
            hisPrtsc.iList10 = historyDataShow3.GetPointList(1);
            hisPrtsc.iList11 = historyDataShow3.GetPointList(2);
            hisPrtsc.iList12 = historyDataShow3.GetPointList(3);

            hisPrtsc.iList13 = historyDataShow4.GetPointList(0);
            hisPrtsc.iList14 = historyDataShow4.GetPointList(1);
            hisPrtsc.iList15 = historyDataShow4.GetPointList(2);
            hisPrtsc.iList16 = historyDataShow4.GetPointList(3);
        }

        private void setText(HistoryDataPrtsc hisPrtsc)
        {
            if (null != hisPrtsc)
            {
                hisPrtsc.setText(SelectTag1.Text, radLabel1.Text, radLabel2.Text, 1);
                hisPrtsc.setText(SelectTag5.Text, radLabel3.Text, radLabel6.Text, 2);
                hisPrtsc.setText(SelectTag6.Text, radLabel4.Text, radLabel7.Text, 3);
                hisPrtsc.setText(SelectTag7.Text, radLabel5.Text, radLabel8.Text, 4);

                hisPrtsc.setText(SelectTag8.Text, radLabel24.Text, radLabel9.Text, 5);
                hisPrtsc.setText(SelectTag2.Text, radLabel23.Text, radLabel10.Text, 6);
                hisPrtsc.setText(SelectTag9.Text, radLabel22.Text, radLabel11.Text, 7);
                hisPrtsc.setText(SelectTag10.Text, radLabel21.Text, radLabel12.Text, 8);

                hisPrtsc.setText(SelectTag11.Text, radLabel28.Text, radLabel13.Text, 9);
                hisPrtsc.setText(SelectTag12.Text, radLabel27.Text, radLabel14.Text, 10);
                hisPrtsc.setText(SelectTag3.Text, radLabel26.Text, radLabel15.Text, 11);
                hisPrtsc.setText(SelectTag13.Text, radLabel25.Text, radLabel16.Text, 12);

                hisPrtsc.setText(SelectTag14.Text, radLabel32.Text, radLabel17.Text, 13);
                hisPrtsc.setText(SelectTag15.Text, radLabel31.Text, radLabel18.Text, 14);
                hisPrtsc.setText(SelectTag16.Text, radLabel30.Text, radLabel19.Text, 15);
                hisPrtsc.setText(SelectTag4.Text, radLabel29.Text, radLabel20.Text, 16);

            }
        }

        private void pickture_btn_Click(object sender, EventArgs e)
        {
            try
            {
                HistoryDataPrtsc h = new HistoryDataPrtsc();
                //h.drillNo = rtxt_drillNo.Text;
                //h.lease = rtxt_lease.Text;
                //h.company = rtxt_company.Text;
                //h.country = rtxt_country.Text;
                //h.contractor = rtxt_contractor.Text;
                //h.dateSpud = rtxt_ds.Text;
                //h.toolPusher = rtxt_tp.Text;
                //h.daterelease = rtxt_dr.Text;
                //h.companyman = rtxt_cm.Text;
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

                getPointList(h);
                setText(h);
                
                //h.iList1 = historyDataShow1.GetPointList(0);
                //h.iList2 = historyDataShow2.GetPointList(1);
                //h.iList3 = historyDataShow3.GetPointList(2);
                //h.iList4 = historyDataShow4.GetPointList(3);
                h.ShowDialog();
                if (h.clickOK)
                {
                    //路径选择
                    SaveFileDialog path = new SaveFileDialog();
                    path.FileName = "HistoryData_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    path.Filter = "(*.png)|*.png|(*.jpg)|*.jpg";
                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        h.bit.Save(path.FileName);
                        MessageBox.Show(lbl_message[4]);
                    }
                }
            }
            catch { }
        }
        #endregion

        public void showStartCalendar()
        {
            if (this.pl_CalendarAndTime2.Visible)
            {
                this.pl_CalendarAndTime2.Visible = false;
            }

            if (this.pl_CalendarAndTime.Visible)
            {
                this.pl_CalendarAndTime.Visible = false;
            }
            else
            {
                this.pl_CalendarAndTime.Visible = true;
            }
        }

        public void showEndCalendar()
        {
            if (this.pl_CalendarAndTime.Visible)
            {
                this.pl_CalendarAndTime.Visible = false;
            }

            if (this.pl_CalendarAndTime2.Visible)
            {
                this.pl_CalendarAndTime2.Visible = false;
            }
            else
            {
                this.pl_CalendarAndTime2.Visible = true;
            }
        }
        public void showEnd(bool bEnd = true)
        {
            this.pl_CalendarAndTime2.Visible = bEnd;
        }

        #region 时间选择框事件
        private void StartTime_Click(object sender, EventArgs e)
        {
            showStartCalendar();
        }

        private void EndTime_Click(object sender, EventArgs e)
        {
            showEndCalendar();
        }

        private void TimePick_OK_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_start.SelectedDate.Year != 1900)
            {
                this.StartTime.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            }

            pl_CalendarAndTime.Visible = false;
        }

        private void TimePick_No_Click(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
        }

        private void TimePick_OK2_Click(object sender, EventArgs e)
        {
            if (this.radCalendar_end.SelectedDate.Year != 1900)
            {
                this.EndTime.Text = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");

                if (m_bIsNeedEnd)
                {
                    if (null != m_exportForm)
                    {
                        m_exportForm.setStartEndTime(EndTime.Text, m_bIsNeedEnd);
                        m_lStartTime = ConvertDateTimeInt(DateTime.Parse(EndTime.Text));
                        m_strStartTime = EndTime.Text;
                    }
                }
                else
                {
                    if (null != m_exportForm)
                    {
                        m_exportForm.setStartEndTime(EndTime.Text, m_bIsNeedEnd);
                        m_lEndTime = ConvertDateTimeInt(DateTime.Parse(EndTime.Text));
                        m_strEndTime = EndTime.Text;
                    }
                }
            }

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
                                    if (c.Name == "rad_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu" || c.Name == "pl_CalendarAndTime" || c.Name == "pl_CalendarAndTime2")
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
            backgroundWorker1.CancelAsync();
        }

        private void showText(bool bShow)
        {
            if (bShow)
            {
                m_strText = getResetLableText();
                setResetLableText("正在导出数据，请稍等。。。");
                setResetLable(bShow);
            }
            else
            {
                setResetLableText(m_strText);
                setResetLable(bShow);
            }
        }

        private void createExportForm()
        {
            m_exportForm = new ExportTime();
            if (null == m_exportForm)
            {
                return;
            }

            m_exportForm.setHisForm(this);
            DateTime dt = Comm.ConvertIntDateTime((double)QueryStartTime * 1000);
            setTimeText(dt, ref m_strStartTime, false);
            m_exportForm.setStartEndTime(m_strStartTime, true);
            dt = Comm.ConvertIntDateTime((double)QueryEndTime * 1000);
            setTimeText(dt, ref m_strEndTime, false);
            m_exportForm.setStartEndTime(m_strEndTime, false);
            m_exportForm.Show();
            m_exportForm.setFormLaction();
            Point pt = m_exportForm.getLocation();
            pl_CalendarAndTime2.Location = pt;
        }

        public void exportData()
        {
            rbtn_export.Enabled = false;
            m_bIsExport = true;
            showText(true);
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void rbtn_export_Click(object sender, EventArgs e)
        {
            createExportForm();
        }

        private void setXlsData()
        {
            if (SelectedTag.Count == 0 || dataList.datas.Count == 0)
            {
                return;//测点没选中或者无数据直接return
            }

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

        private void writeXlsFile()
        {
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
                    {
                        workbook.Write(file);
                        MessageBox.Show(lbl_message[5]);
                    }
                    file.Close();
                }
            }
            catch 
            { 
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            queryHistory();
            setXlsData();
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_export.Enabled = true;
            showText(false);
            writeXlsFile();
            m_bIsExport = false;
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
            //lbl_well.BackColor = Color.White;
            //lbl_well.ForeColor = Color.Black;
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
            {
                bit.Save(path.FileName);
            }

            backgroundWorker3.CancelAsync();
        }

        private void SelectTag5_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag5, radLabel3, radLabel6, 2);
        }

        private void SelectTag6_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag6, radLabel4, radLabel7, 3);
        }

        private void SelectTag7_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag7, radLabel5, radLabel8, 4);
        }

        private void SelectTag8_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag8, radLabel24, radLabel9, 5);
        }

        private void SelectTag9_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag9, radLabel22, radLabel11, 7);
        }

        private void SelectTag10_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag10, radLabel21, radLabel12, 8);
        }

        private void SelectTag11_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag11, radLabel28, radLabel13, 9);
        }

        private void SelectTag12_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag12, radLabel27, radLabel14, 10);
        }

        private void SelectTag13_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag13, radLabel25, radLabel16, 12);
        }

        private void SelectTag14_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag14, radLabel32, radLabel17, 13);
        }

        private void SelectTag15_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag15, radLabel31, radLabel18, 14);
        }

        private void SelectTag16_Click(object sender, EventArgs e)
        {
            selectTag(SelectTag16, radLabel30, radLabel19, 15);
        }

        private void setUp(object sender, EventArgs e)
        {
            m_idepthVasibleSetp = historyDataDepthShow1.getDepthStep();

            MarkForm frm = new MarkForm(m_idepthVasibleSetp, m_iTimeIndex, m_bIsReal, QueryEndTime);

            if (null == frm)
            {
                return;
            }

            frm.setHisForm(true);
            frm.ShowDialog();
            if (frm.getCancle() || !SelectedTagMessage() || !frm.getBtnClick())
            {
                return;
            }

            m_idepthVasibleSetp = frm.getStep();
            m_iTimeIndex = frm.getTimeIndex();
            setBtnTF();
            m_bIsReal = frm.getReal();
            QueryEndTime = frm.getTime();

            DateTime dt = Comm.ConvertIntDateTime((double)QueryEndTime * 1000);
            string strStart = string.Empty;
            setTimeText(dt, ref strStart, true);

            historyDataDepthShow1.setTimeIndex(m_iTimeIndex);
            historyDataDepthShow1.setTimeStep(m_iTimeIndex, false);
            int iTimeStep = historyDataDepthShow1.getTimeStep();
            QueryStartTime = QueryEndTime - iTimeStep * 2;
            m_lStartTime = QueryStartTime;
            m_lEndTime = QueryEndTime;
            QueryHis();

            historyDataDepthShow1.setDepthStep(m_idepthVasibleSetp);
            historyDataDepthShow1.vasibleMemo();
        }

        public void setHistoryValue()
        {
            rlb_wellno.Text = "...";
            rlb_lease.Text = "...";
            rlb_rigno.Text = "...";
            rlb_contractor.Text = "...";
            rlb_oprator.Text = "...";
            rlb_datespud.Text = "...";
            rlb_daterelease.Text = "...";
        }

        private void setHistoryValue(int iDrillNo)
        {
            try
            {
                var drill = db.Drill.Where(O => O.ID == iDrillNo).FirstOrDefault();

                if (drill != null)
                {
                    rlb_wellno.Text = drill.DrillNo;
                    rlb_lease.Text = drill.Lease;
                    rlb_rigno.Text = drill.RigNo;
                    rlb_contractor.Text = drill.Contractor;
                    rlb_oprator.Text = drill.Operator;
                    rlb_datespud.Text = drill.DateSpud;
                    rlb_daterelease.Text = drill.DateRelease;
                }
            }
            catch
            {
            }
        }

        private void radBtn_hisWell_Click(object sender, EventArgs e)
        {
            HisWellList frm = new HisWellList(m_iRowIndex);
            frm.Location = new Point(0, 200);
            frm.ShowDialog();

            int iCurrentWellNo = frm.getDrillNO();
            m_iRowIndex = frm.getRowIndex();

            if (0 != iCurrentWellNo && m_iWellID != iCurrentWellNo)
            {
                InitSelectTags();
                InitLables();
                setControlLanguage();

                m_idepthVasibleSetp = 1;

                if (null != SelectedTag)
                {
                    SelectedTag.Clear();
                }

                if (null != dataList)
                {
                    if (null != dataList.datas)
                    {
                        dataList.datas.Clear();
                    }

                    if (null != dataList.Depthdatas)
                    {
                        dataList.Depthdatas.Clear();
                    }
                }

                clearHistoryCharts();
                m_iWellID = iCurrentWellNo;
                setHistoryValue(iCurrentWellNo);
            }
        }

        public void selectHisDrill(int iDrillID)
        {
            int iCurrentWellNo = iDrillID;
            if (m_iWellID != iCurrentWellNo)
            {
                InitSelectTags();
                InitLables();
                setControlLanguage();

                m_idepthVasibleSetp = 1;

                if (null != SelectedTag)
                {
                    SelectedTag.Clear();
                }

                if (null != dataList)
                {
                    if (null != dataList.datas)
                    {
                        dataList.datas.Clear();
                    }

                    if (null != dataList.Depthdatas)
                    {
                        dataList.Depthdatas.Clear();
                    }
                }

                clearHistoryCharts();
                m_iWellID = iCurrentWellNo;
                setHistoryValue(iCurrentWellNo);
            }
        }

        private void radBtnDefault_Click(object sender, EventArgs e)
        {
            InitSelectTags();
            InitLables();
            setControlLanguage();
            setDefaultTags();
        }

        private void setTimeText(DateTime dt, ref string strText, bool bIsStart = false)
        {
            if (bIsStart)
            {
                this.radCalendar_start.SelectedDate = dt;
                this.radCalendar_start.FocusedDate = dt;
                this.radTimePicker1.Value = Convert.ToDateTime(this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                this.StartTime.Text = this.radCalendar_start.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker1.Value)).ToString("HH:mm:ss");
            }
            else
            {
                this.radCalendar_end.SelectedDate = dt;
                this.radCalendar_end.FocusedDate = dt;
                this.radTimePicker2.Value = Convert.ToDateTime(this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd HH:mm:ss"));
                strText = this.radCalendar_end.SelectedDate.ToString("yyyy-MM-dd ") + ((DateTime)(this.radTimePicker2.Value)).ToString("HH:mm:ss");
            }
        }

        private void setBtnTF()
        {
            if (1 == m_iTimeIndex)
            {
                enlarge_btn.Enabled = false;
            }
            else if (10 == m_iTimeIndex)
            {
                narrow_btn.Enabled = false;
            }
            else
            {
                enlarge_btn.Enabled = true;
                narrow_btn.Enabled = true;
            }
        }
    }
}
