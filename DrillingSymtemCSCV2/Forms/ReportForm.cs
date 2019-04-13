using DrillingSymtemCSCV2.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.Pivot.Core;
using Telerik.Pivot.Core.Aggregates;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Xml;
using System.Globalization;
using System.Net;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Collections;

namespace DrillingSymtemCSCV2.Forms
{
    #region 判断右键菜单点击的是哪张表
    public struct ReportTable {
        //表1
        public static int sheet1_rgda_drillAss_night = 1;
        public static int sheet1_rgbr_bitRecord = 2;
        public static int sheet1_rgv_mudchemicals_night = 3;
        public static int sheet1_rgv_mudchemicals_day = 11;
        public static int sheet1_rgda_drillAss_day = 12;
        //表2
        public static int sheet2_rmv_operation_night = 4;
        public static int sheet2_rmv_operation_day = 5;
        public static int sheet2_radGridView4 = 6;
        public static int sheet2_rgv_wireline = 7;
        public static int sheet2_rgv_laseCasing = 8;
        public static int sheet2_rgv_deviation_night = 9;
        public static int sheet2_rgv_deviation_day = 9;
        //表3
        public static int sheet3_rgv_drillCrew = 9;
        public static int sheet3_radGridView9 = 10;
    }
    #endregion

    public partial class ReportForm : RadForm
    {
        private List<Table_PumpNo> PumpNoList;
        private List<string> HeaderLanuage;
        DateTime dt = new DateTime();
        //记录表2 gradview选择Cell的row
        int mouse_SelectRow = 1;
        //记录表2 gradview选择Cell的Col
        int mouse_SelectCol = 1;
        //记录鼠标点击的是哪个表
        int mouse_SelectTable = 0;
        //Tag表 累计数量
        Dictionary<string, TagDayMiddleNightModel> TagArray = new Dictionary<string, TagDayMiddleNightModel>();
        //
        private List<decimal> TotalLength = new List<decimal>();
        private Color autocolor = Color.LightGray;//定义自动填写单元格颜色
        private List<string> bit_list = new List<string>();//钻头数据
        private List<string> mud_list = new List<string>();//泥浆数据
        private List<string> cut_list = new List<string>();//钻头磨损
        private List<string> com_list = new List<string>();//完井作业
        private List<string> dwork_list = new List<string>();
        private List<string> crew_list = new List<string>();
        private List<WorkCode> workCodeList = new List<WorkCode>();

        private List<ActivityStatus> ActivityList = new List<ActivityStatus>();

        //自己语言的翻译字典
        //M1_钻井
        //M2_井深
        //M1 指的是语言的唯一标记
        //M1后面的文字指的是Name
        private Dictionary<string, string> LanuangeDictionary = new Dictionary<string, string>();
        private List<string> message_list = new List<string>();//存放需要提示的基本数据

        private IWorkbook workbook;//分别定义三个报表
        private bool isAuto1 = false;//判断报表是否为自动导出（23:59自动导出一份）
        private bool isAutoExport = false;
        private bool isSave { get; set; }   //是否保存操作，如果是保存操作不输出excel
        private Report report = new Report();
        private Report report1 = new Report();//用于获取报表1数据
        private List<ReportData> ReportData = new List<ReportData>();
        private DrillOSEntities db;
        private DateTime d_morning, d_evening, d_morning_next;//定义早，晚，第二天早
        private int DayShift;//定义早班交接时间
        private Drill Drill = new Drill();
        private DateTime d_now;//当前选择时间
        public List<SXCode> SXCode = new List<SXCode>();
        public ReportForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 界面初期加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                db = new DrillOSEntities();
                if (AppDrill.permissionId != 1)
                {
                    radButton1.Enabled = false;
                    rbtn_save.Enabled = false;
                }
                HeaderLanuage = new List<string>();
                setControlLanguage();               
                d_now = DateTime.Now;
                rdtp_date.Value = DateTime.Now.Date;
                #region 异步加载数据
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
                backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
                backgroundWorker1.RunWorkerAsync(); //开始
                #endregion
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker3.WorkerSupportsCancellation = true;
                radCalendar1.SelectedDate = DateTime.Now;
                radCalendar1.FocusedDate = DateTime.Now;
                radCalendar2.SelectedDate = DateTime.Now;
                radCalendar2.FocusedDate = DateTime.Now;
                if (AppDrill.language == "CN")
                {
                    this.radCalendar1.Culture = new System.Globalization.CultureInfo("zh-CN");
                    this.radCalendar2.Culture = new System.Globalization.CultureInfo("zh-CN");
                    this.rdtp_date.Culture = new System.Globalization.CultureInfo("zh-CN");
                }
                DayShift = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DayShift"].ToString());
            }
            catch { }
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                workCodeList = db.WorkCode.ToList();
                Drill = db.Drill.Where(O => O.ID == AppDrill.DrillID).FirstOrDefault();
                PumpNoList = db.Table_PumpNo.ToList();
                TotalLength = (from a in db.DrillTools
                               where a.status == 2 && a.Basic != 0 && a.DrillId == AppDrill.DrillID
                               select (a.Length)).ToList();
                ReportData = db.ReportData.Where(o => o.DrillId == AppDrill.DrillID && o.UserName == AppDrill.username).ToList();
                report1 = new JavaScriptSerializer().Deserialize<Report>(ReportData.Where(o => o.Date == DateTime.Now.ToString("yyyy-MM-dd")).FirstOrDefault().JsonData);
            }
            catch { }
        }

        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //读取时效数据
                int j = 1;
                foreach (var w in workCodeList)
                {
                    SXCode.Add(new SXCode { Number = j, Text = w.CodeNo });
                    j++;
                }
                InitRadGridTime();
                InitRadGridDrillPipe();
                InitRadGridBitRecord();
                InitRadGridMudRecord();
                IntiRadGridCuttingStructure();
                InitGridCompletion();
                IntiGridDrillAssembly();
                IntiGridDayworkSummary();
                InitDrillingParameters();
                IntiGridMCAdded();
                Report_Three_one();
                SetGridViewData();
                SetBasicData();
                backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
            }
            catch { }
        }
        #endregion

        #region 表格时效
        /// <summary>
        /// 初期化时效表
        /// Update 2017.10.26
        /// </summary>
        private void InitRadGridTime()
        {            
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("OPERATION");
                dt.Columns.Add("NIGHT");
                dt.Columns.Add("DAY");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < SXCode.Count; i++)
                {
                    dr = dt.NewRow();
                    dr["OPERATION"] = (i + 1) + "." + SXCode[i].Text;
                    dr["NIGHT"] = "";
                    dr["DAY"] = "";
                    dt.Rows.Add(dr);
                }
                rgt_timeDistribution.Rows.Clear();
                GridViewRowInfo rowInfo;
                for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
                {
                    rowInfo = rgt_timeDistribution.Rows.AddNew();
                    rowInfo.Height = 25;
                    rgt_timeDistribution.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[0].Style.DrawFill = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                    rgt_timeDistribution.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
                    rgt_timeDistribution.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["OPERATION"].ToString();
                    rgt_timeDistribution.Rows[iLoop].Cells[0].ReadOnly = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[1].Style.CustomizeFill = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[1].Style.DrawFill = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[1].Style.ForeColor = Color.Black;
                    rgt_timeDistribution.Rows[iLoop].Cells[1].Style.BackColor = autocolor;
                    rgt_timeDistribution.Rows[iLoop].Cells[1].ReadOnly = true;
                    rgt_timeDistribution.Rows[iLoop].Cells[2].Value = "";
                    rgt_timeDistribution.Rows[iLoop].Cells[2].Style.CustomizeFill = true;//设置可修改单元格样式
                    rgt_timeDistribution.Rows[iLoop].Cells[2].Style.ForeColor = Color.Black;
                    rgt_timeDistribution.Rows[iLoop].Cells[2].Style.BackColor = autocolor;
                    rgt_timeDistribution.Rows[iLoop].Cells[2].ReadOnly = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// DRILL PIPE(钻杆) ,Update 2017.10.26 18:00
        /// </summary>
        private void InitRadGridDrillPipe()
        {
            rgv_drillPipe.AutoSizeRows = false;//设置高度不能自动变化
            ///添加4行新数据
            for (int i = 0; i < 4; i++)
            {
                rgv_drillPipe.Rows.AddNew();
                rgv_drillPipe.Rows[i].Height = 25;
            }
        }

        /// <summary>
        /// BIT RECORD(钻头数据）
        /// </summary>
        private void InitRadGridBitRecord()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("values");

            DataRow dr = dt.NewRow();
            foreach (string s in bit_list)
            {
                dr = dt.NewRow();
                dr["name"] = s;
                dr["values"] = "";
                dt.Rows.Add(dr);
            }

            //Night
            rgv_bitRecord_night.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_bitRecord_night.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_bitRecord_night.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_bitRecord_night.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_bitRecord_night.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_bitRecord_night.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_bitRecord_night.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
            //Day
            rgv_bitRecord_day.Rows.Clear();
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_bitRecord_day.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_bitRecord_day.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_bitRecord_day.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_bitRecord_day.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_bitRecord_day.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_bitRecord_day.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
        }

        /// <summary>
        /// 泥浆记录
        /// </summary>
        private void InitRadGridMudRecord()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("values");

            DataRow dr = dt.NewRow();
            foreach (string s in mud_list)
            {
                dr = dt.NewRow();
                dr["name"] = s;
                dr["values"] = "";
                dt.Rows.Add(dr);
            }
            //Night
            rgv_mudRecord_night.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_mudRecord_night.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_mudRecord_night.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_mudRecord_night.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_mudRecord_night.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_mudRecord_night.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_mudRecord_night.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_mudRecord_night.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
            //Day
            rgv_mudRecord_day.Rows.Clear();
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_mudRecord_day.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_mudRecord_day.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_mudRecord_day.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_mudRecord_day.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_mudRecord_day.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_mudRecord_day.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_mudRecord_day.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
        }

        /// <summary>
        /// 钻头磨损分析
        /// </summary>
        private void IntiRadGridCuttingStructure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("values");

            DataRow dr = dt.NewRow();
            foreach (string s in cut_list)
            {
                dr = dt.NewRow();
                dr["name"] = s;
                dr["values"] = "";
                dt.Rows.Add(dr);
            }
            //Night
            rgv_cuttingStruc_night.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_cuttingStruc_night.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_cuttingStruc_night.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;               
            }
            //Day            
            rgv_cuttingStruc_day.Rows.Clear();
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_cuttingStruc_day.Rows.AddNew();
                //rowInfo.Height = 30;
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["name"].ToString();
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgv_cuttingStruc_day.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
        }

        /// <summary>
        /// 完井作业
        /// </summary>
        private void InitGridCompletion()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");

            DataRow dr = dt.NewRow();
            foreach (string s in com_list)
            {
                dr = dt.NewRow();
                dr["col1"] = s;
                dr["col2"] = "";
                dt.Rows.Add(dr);
            }
            rgc_completion.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgc_completion.Rows.AddNew();
                //rowInfo.Height = 30;
                rgc_completion.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["col1"].ToString();
                rgc_completion.Rows[iLoop].Cells[0].ReadOnly = true;
                rgc_completion.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgc_completion.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgc_completion.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgc_completion.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
            rgc_completion.Rows[com_list.Count - 1].Cells[1].Style.CustomizeFill = true;
            rgc_completion.Rows[com_list.Count - 1].Cells[1].Style.BackColor = autocolor;
            rgc_completion.Rows[com_list.Count - 1].Cells[2].Style.CustomizeFill = true;
            rgc_completion.Rows[com_list.Count - 1].Cells[2].Style.BackColor = autocolor;

            rgc_completion.Rows[com_list.Count - 1].Cells[1].ReadOnly = true;
            rgc_completion.Rows[com_list.Count - 1].Cells[2].ReadOnly = true;
        }

        /// <summary>
        /// 钻具组合 rgda_drillAss
        /// </summary>
        private void IntiGridDrillAssembly()
        {
            //Night
            rgda_drillAss_night.AutoSizeRows = false;
            for (int iLoop = 0; iLoop < 9; iLoop++)
            {
                rgda_drillAss_night.Rows.AddNew();
                rgda_drillAss_night.Rows[iLoop].Height = 25;
            }
            //Day
            rgda_drillAss_day.AutoSizeRows = false;
            for (int iLoop = 0; iLoop < 9; iLoop++)
            {
                rgda_drillAss_day.Rows.AddNew();
                rgda_drillAss_day.Rows[iLoop].Height = 25;
            }
        }

        /// <summary>
        /// 添加剂记录
        /// </summary>
        private void IntiGridMCAdded()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TYPE");
            dt.Columns.Add("AMOUNT");

            DataRow dr = dt.NewRow();
            for (int i = 0; i < 10; i++)
            {
                dr = dt.NewRow();
                dr["TYPE"] = "";
                dr["AMOUNT"] = "";
                dt.Rows.Add(dr);
            }
            //Night            
            rgv_mudchemicals_night.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_mudchemicals_night.Rows.AddNew();
                rowInfo.Height = 25;
                rgv_mudchemicals_night.Rows[iLoop].Cells[0].ReadOnly = true;
            }
            //Day
            rgv_mudchemicals_day.Rows.Clear();
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_mudchemicals_day.Rows.AddNew();
                rowInfo.Height = 25;
                rgv_mudchemicals_day.Rows[iLoop].Cells[0].ReadOnly = true;
            }
        }

        /// <summary>
        /// 作业时间概要
        /// </summary>
        private void IntiGridDayworkSummary()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");

            DataRow dr = dt.NewRow();
            foreach (string s in dwork_list)
            {
                dr = dt.NewRow();
                dr["col1"] = s;
                dr["col2"] = "";
                dt.Rows.Add(dr);
            }
            rgds_daywork.Rows.Clear();
            GridViewRowInfo rowInfo;
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgds_daywork.Rows.AddNew();
                rowInfo.Height = 25;
                rgds_daywork.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["col1"].ToString();
                rgds_daywork.Rows[iLoop].Cells[0].ReadOnly = true;
                rgds_daywork.Rows[iLoop].Cells[0].Style.CustomizeFill = true;
                rgds_daywork.Rows[iLoop].Cells[0].Style.DrawFill = true;
                rgds_daywork.Rows[iLoop].Cells[0].Style.ForeColor = Color.Black;
                rgds_daywork.Rows[iLoop].Cells[0].Style.BackColor = Color.PowderBlue;
            }
            //处理自动生成的数据
            rgds_daywork.Rows[7].Cells[1].Style.CustomizeFill = true;
            rgds_daywork.Rows[7].Cells[1].Style.BackColor = autocolor;
            rgds_daywork.Rows[7].Cells[2].Style.CustomizeFill = true;
            rgds_daywork.Rows[7].Cells[2].Style.BackColor = autocolor;
            //一天总作业时间
            //rgds_daywork.Rows[7].Cells[1].Value = "0";

            rgds_daywork.Rows[7].Cells[1].ReadOnly = true;
            rgds_daywork.Rows[7].Cells[2].ReadOnly = true;
        }

        /// <summary>
        /// 报表三 WELL RIG INFORMATION 上部分
        /// </summary>
        private void Report_Three_one()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("CREW");
            dt.Columns.Add("EMPL.ID NO.");
            dt.Columns.Add("NAME");
            dt.Columns.Add("HRS");
            dt.Columns.Add("INITIAL");
            dt.Columns.Add("YES OR NO");

            DataRow dr = dt.NewRow();
            foreach (string s in crew_list)
            {
                dr = dt.NewRow();
                dr["CREW"] = s;
                dr["EMPL.ID NO."] = "";
                dr["NAME"] = "";
                dr["HRS"] = "";
                dr["INITIAL"] = "";
                dr["YES OR NO"] = "";
                dt.Rows.Add(dr);
            }
            rgv_drillCrew_night.Rows.Clear();
            GridViewRowInfo rowInfo;

            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_drillCrew_night.Rows.AddNew();
                rowInfo.Height = 25;
                rgv_drillCrew_night.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["CREW"].ToString();
                rgv_drillCrew_night.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_drillCrew_night.Rows[iLoop].Cells[1].ReadOnly = true;
                rgv_drillCrew_night.Rows[iLoop].Cells[2].ReadOnly = true;
                rgv_drillCrew_night.Rows[iLoop].Cells[5].ReadOnly = true;
            }
            rgv_drillCrew_day.Rows.Clear();
            for (int iLoop = 0; iLoop < dt.Rows.Count; iLoop++)
            {
                rowInfo = rgv_drillCrew_day.Rows.AddNew();
                rowInfo.Height = 25;
                rgv_drillCrew_day.Rows[iLoop].Cells[0].Value = dt.Rows[iLoop]["CREW"].ToString();
                rgv_drillCrew_day.Rows[iLoop].Cells[0].ReadOnly = true;
                rgv_drillCrew_day.Rows[iLoop].Cells[1].ReadOnly = true;
                rgv_drillCrew_day.Rows[iLoop].Cells[2].ReadOnly = true;
                rgv_drillCrew_day.Rows[iLoop].Cells[5].ReadOnly = true;
            }
        }
        #endregion

        #region Export
        private void radButton1_Click_1(object sender, EventArgs e)
        {

            try
            {
                isSave = false;//非保存操作
                isAuto1 = false;//非自动导出
                radButton1.Enabled = false;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }

        }
        #endregion

        #region 初始化钻井信息 By LLM
        private void InitDrillingParameters()
        {
            try
            {
                #region 合并列
                this.rmv_operation_night.AddSpanHeader(0, 2, HeaderLanuage[0]);
                this.rmv_operation_night.AddSpanHeader(8, 2, HeaderLanuage[1]);
                this.rmv_operation_night.AddSpanHeader(10, 2, HeaderLanuage[2]);
                this.rmv_operation_night.AddSpanHeader(12, 2, HeaderLanuage[3]);
                this.rmv_operation_night.AddSpanHeader(14, 2, HeaderLanuage[4]);
                this.rmv_operation_day.AddSpanHeader(0, 2, HeaderLanuage[0]);
                this.rmv_operation_day.AddSpanHeader(8, 2, HeaderLanuage[1]);
                this.rmv_operation_day.AddSpanHeader(10, 2, HeaderLanuage[2]);
                this.rmv_operation_day.AddSpanHeader(12, 2, HeaderLanuage[3]);
                this.rmv_operation_day.AddSpanHeader(14, 2, HeaderLanuage[4]);
                #endregion
                #region rmv_operation 早晚
                this.rmv_operation_night.Rows.Add(3);//添加5行空数据
                this.rmv_operation_night.EnableHeadersVisualStyles = false;
                this.rmv_operation_night.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                this.rmv_operation_night.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);

               
                this.rmv_operation_day.Rows.Add(3);//添加5行空数据
                this.rmv_operation_day.EnableHeadersVisualStyles = false;
                this.rmv_operation_day.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                this.rmv_operation_day.DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
                
                #endregion
                #region rgv_drecord 早晚
                rgv_drecord_night.AutoSizeRows = false;
                for (int i = 0; i < 2; i++)
                {
                    this.rgv_drecord_night.Rows.AddNew();
                    rgv_drecord_night.Rows[i].Height = 25;
                }
                rgv_drecord_day.AutoSizeRows = false;
                for (int i = 0; i < 2; i++)
                {
                    this.rgv_drecord_day.Rows.AddNew();
                    rgv_drecord_day.Rows[i].Height = 25;
                }
                #endregion
                #region rgv_laseCasing
                rgv_laseCasing.AutoSizeRows = false;
                //最后一层套管数据
                for (int i = 0; i < 3; i++)
                {
                    this.rgv_laseCasing.Rows.AddNew();
                    this.rgv_laseCasing.Rows[i].Cells[0].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_laseCasing.Rows[i].Height = 25;                   
                }
                #endregion
                #region rgv_deviation 早晚
                rgv_deviation_night.AutoSizeRows = false;
                for (int i = 0; i < 18; i++)
                {
                    this.rgv_deviation_night.Rows.AddNew();
                    this.rgv_deviation_night.Rows[i].Cells[0].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_night.Rows[i].Cells[1].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_night.Rows[i].Cells[2].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_night.Rows[i].Cells[3].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_night.Rows[i].Height = 25;                   
                }
                rgv_deviation_day.AutoSizeRows = false;
                for (int i = 0; i < 18; i++)
                {
                    this.rgv_deviation_day.Rows.AddNew();
                    this.rgv_deviation_day.Rows[i].Cells[0].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_day.Rows[i].Cells[1].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_day.Rows[i].Cells[2].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_day.Rows[i].Cells[3].ReadOnly = true;//此单元格数据由选择框获取
                    this.rgv_deviation_day.Rows[i].Height = 25;
                }
                #endregion

                this.rgv_laseCasing.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView6_CellClick);
            }
            catch { }
        }
        #endregion

        #region 设置最后一层套管数据记录 By LLM /zy
        private void radGridView6_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                SelectList sl = new SelectList();
                sl.isWhat = 1;
                sl.ShowDialog();
                //回填数据
                if (!string.IsNullOrEmpty(sl.selectText))
                {
                    this.rgv_laseCasing.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sl.selectText;
                }
            }
            if (e.ColumnIndex == 7 && e.RowIndex >= 0)
            {
                SelectList sl = new SelectList();
                sl.isWhat = 2;
                sl.ShowDialog();
                //回填数据
                if (!string.IsNullOrEmpty(sl.selectText))
                {
                    this.rgv_laseCasing.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sl.selectText;
                }
            }
        }
        #endregion

        #region 设置作业内容详细资料 By LLM
        private void radGridView7_CellClick(object sender, GridViewCellEventArgs e)
        {
            //弹出列表框
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                SelectList sl = new SelectList();
                sl.isWhat = 3;
                sl.ShowDialog();
                //处理删除情况
                foreach (var w in sl.d_list)
                {
                    WorkCode code = workCodeList.Where(o => o.ID == w.ID).FirstOrDefault();
                    if (code != null)
                    {
                        workCodeList.Remove(code);
                    }
                }
                //如果删除的数据条数大于0，则重新排列数据
                if (sl.d_list.Count > 0)
                {
                    for (int i = 0; i < sl.d_list.Count; i++)
                    {
                        this.rgt_timeDistribution.Rows.RemoveAt(this.rgt_timeDistribution.Rows.Count - 1);
                    }
                    SXCode.Clear();
                    int j = 1;
                    foreach (var w in workCodeList)
                    {
                        SXCode.Add(new SXCode { Number = j, Text = w.CodeNo });
                        j++;
                    }
                    //判断SXCode是否大于21条,需重新排列大于21条的数据
                    if (SXCode.Count > 21)
                    {
                        for (int i = 21; i < SXCode.Count; i++)
                        {
                            this.rgt_timeDistribution.Rows[i].Cells[0].Value = SXCode[i].Number + "." + SXCode[i].Text;
                            this.rgt_timeDistribution.Rows[i].Cells[1].Value = "";
                            this.rgt_timeDistribution.Rows[i].Cells[2].Value = "";
                        }
                    }
                }
                //回填数据
                if (!string.IsNullOrEmpty(sl.selectText))
                {
                    SXCode sx = SXCode.Where(o => o.Text == sl.selectText).FirstOrDefault();
                    if (sx != null)
                    {
                        try
                        {
                            double old_v = 0;
                            int old_codeNo = 0;
                            int CodeNo = sx.Number;
                            try
                            {
                                old_codeNo = int.Parse(this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());//保存之前的编号
                            }
                            catch { }
                            this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CodeNo;
                            this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = sl.selectText;
                            //判断之前CodeNo是否存在
                            try
                            {
                                for (int i = 0; i < this.rgv_deviation_night.Rows.Count&&old_codeNo!=0; i++)
                                {
                                    if (this.rgv_deviation_night.Rows[i].Cells[3].Value.ToString() == old_codeNo.ToString())
                                    {
                                        try
                                        {
                                            old_v += double.Parse(this.rgv_deviation_night.Rows[i].Cells[2].Value.ToString());//之前的值
                                        }
                                        catch { }
                                    }
                                }
                                this.rgt_timeDistribution.Rows[old_codeNo - 1].Cells[1].Value = old_v;
                            }
                            catch { }
                            //同步晚班时效过去
                            //先转换为double类型
                            double new_v = 0;
                            for (int i = 0; i < this.rgv_deviation_night.Rows.Count; i++)
                            {
                                if (this.rgv_deviation_night.Rows[i].Cells[3].Value.ToString() == CodeNo.ToString())
                                {
                                    try
                                    {
                                        new_v += double.Parse(this.rgv_deviation_night.Rows[i].Cells[2].Value.ToString());//之前的值
                                    }
                                    catch { }
                                }
                            }
                            this.rgt_timeDistribution.Rows[CodeNo - 1].Cells[1].Value = new_v;
                        }
                        catch { }                        
                    }
                    else if(AppDrill.permissionId==1&&workCodeList.Count<27)
                    {
                        try
                        {
                            //需要创建并保存数据库
                            WorkCode wk = new WorkCode();
                            wk.CodeNo = sl.selectText;
                            wk.dataMakePGM = "AddSX";
                            wk.dataMakeTime = DateTime.Now;
                            wk.dataMakeUser = AppDrill.username;
                            wk.dataUpdPGM = "AddSX";
                            wk.dataUpdTime = DateTime.Now;
                            wk.dataUpdUser = AppDrill.username;
                            db.WorkCode.Add(wk);
                            workCodeList.Add(wk);
                            db.SaveChanges();
                            SXCode.Add(new SXCode { Number = workCodeList.Count, Text = sl.selectText });
                            this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SXCode.Count;
                            this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = sl.selectText;
                            rgt_timeDistribution.Rows.AddNew();
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Value = SXCode.Count + "." + sl.selectText;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Value = this.rgv_deviation_night.Rows[e.RowIndex].Cells[e.ColumnIndex-1].Value;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.CustomizeFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.DrawFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.BackColor = Color.PowderBlue;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].ReadOnly = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.CustomizeFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.DrawFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.BackColor = autocolor;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].ReadOnly = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Value = "";
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.CustomizeFill = true;//设置可修改单元格样式
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.BackColor = autocolor;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].ReadOnly = true;
                        }
                        catch { }
                    }
                    else if (workCodeList.Count >= 27)
                    {
                        MessageBox.Show(message_list[14]);//新建失败，当前数据最多存在27个
                    }
                }
                SetSXTime(1);
                SetSXTime(2);
            }
            //弹出日期框
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
            {
                //记录下选择Cell的row
                mouse_SelectRow = e.RowIndex;
                //记录下选择Cell的Col
                mouse_SelectCol = e.ColumnIndex;
                //记录点击表是下表
                mouse_SelectTable = ReportTable.sheet2_rgv_deviation_night;
                //显示日期控件 让用户选择日期
                pl_CalendarAndTime.Visible = true;
            }
            else
            {
                pl_CalendarAndTime.Visible = false;
            }
            //  this.rgv_deviation.Rows[e.RowIndex].Cells[2].Value
            //  this.rgv_deviation.Rows[e.RowIndex].Cells[3].Value
            //  this.rgv_deviation.Rows[e.RowIndex].Cells[4].Value
            //queryModel.DrillId
            //if (txtWellNo.Text!="")
            //{               
            //}                      
        }
        private void rgv_deviation_day_CellClick(object sender, GridViewCellEventArgs e)
        {
            ///弹出列表框
            if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                SelectList sl = new SelectList();
                sl.isWhat = 3;
                sl.ShowDialog();
                //处理删除情况
                foreach (var w in sl.d_list)
                {
                    WorkCode code = workCodeList.Where(o => o.ID == w.ID).FirstOrDefault();
                    if (code != null)
                    {
                        workCodeList.Remove(code);
                    }
                }
                //如果删除的数据条数大于0，则重新排列数据
                if (sl.d_list.Count > 0)
                {
                    for (int i = 0; i < sl.d_list.Count; i++)
                    {
                        this.rgt_timeDistribution.Rows.RemoveAt(this.rgt_timeDistribution.Rows.Count - 1);
                    }
                    SXCode.Clear();
                    int j = 1;
                    foreach (var w in workCodeList)
                    {
                        SXCode.Add(new SXCode { Number = j, Text = w.CodeNo });
                        j++;
                    }
                    //判断SXCode是否大于21条,需重新排列大于21条的数据
                    if (SXCode.Count > 21)
                    {
                        for (int i = 21; i < SXCode.Count; i++)
                        {
                            this.rgt_timeDistribution.Rows[i].Cells[0].Value = SXCode[i].Number + "." + SXCode[i].Text;
                            this.rgt_timeDistribution.Rows[i].Cells[1].Value = "";
                            this.rgt_timeDistribution.Rows[i].Cells[2].Value = "";
                        }
                    }
                }
                //回填数据
                if (!string.IsNullOrEmpty(sl.selectText))
                {
                    SXCode sx = SXCode.Where(o => o.Text == sl.selectText).FirstOrDefault();
                    if (sx != null)
                    {
                        try
                        {
                            double old_v = 0;
                            int old_codeNo = 0; ;
                            int CodeNo = sx.Number;
                            try
                            {
                                old_codeNo = int.Parse(this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());//保存之前的编号
                            }
                            catch { }
                            this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CodeNo;
                            this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = sl.selectText;
                            //判断之前CodeNo是否存在
                            try
                            {
                                for (int i = 0; i < this.rgv_deviation_day.Rows.Count && old_codeNo != 0; i++)
                                {
                                    if (this.rgv_deviation_day.Rows[i].Cells[3].Value.ToString() == old_codeNo.ToString())
                                    {
                                        try
                                        {
                                            old_v += double.Parse(this.rgv_deviation_day.Rows[i].Cells[2].Value.ToString());//之前的值
                                        }
                                        catch { }
                                    }
                                }
                                this.rgt_timeDistribution.Rows[old_codeNo - 1].Cells[2].Value = old_v;
                            }
                            catch { }
                            //同步晚班时效过去
                            //先转换为double类型
                            double new_v = 0;
                            for (int i = 0; i < this.rgv_deviation_day.Rows.Count; i++)
                            {
                                if (this.rgv_deviation_day.Rows[i].Cells[3].Value.ToString() == CodeNo.ToString())
                                {
                                    try
                                    {
                                        new_v += double.Parse(this.rgv_deviation_day.Rows[i].Cells[2].Value.ToString());//之前的值
                                    }
                                    catch { }
                                }
                            }
                            this.rgt_timeDistribution.Rows[CodeNo - 1].Cells[2].Value = new_v;
                        }
                        catch { }
                    }
                    else if (AppDrill.permissionId == 1 && workCodeList.Count < 27)
                    {
                        try
                        {
                            //需要创建并保存数据库
                            WorkCode wk = new WorkCode();
                            wk.CodeNo = sl.selectText;
                            wk.dataMakePGM = "AddSX";
                            wk.dataMakeTime = DateTime.Now;
                            wk.dataMakeUser = AppDrill.username;
                            wk.dataUpdPGM = "AddSX";
                            wk.dataUpdTime = DateTime.Now;
                            wk.dataUpdUser = AppDrill.username;
                            db.WorkCode.Add(wk);
                            workCodeList.Add(wk);
                            db.SaveChanges();
                            SXCode.Add(new SXCode { Number = workCodeList.Count, Text = sl.selectText });
                            this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SXCode.Count;
                            this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex + 1].Value = sl.selectText;
                            rgt_timeDistribution.Rows.AddNew();
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Value = SXCode.Count + "." + sl.selectText;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Value = this.rgv_deviation_day.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.CustomizeFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.DrawFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].Style.BackColor = Color.PowderBlue;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[0].ReadOnly = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.CustomizeFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.DrawFill = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].Style.BackColor = autocolor;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[1].ReadOnly = true;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.CustomizeFill = true;//设置可修改单元格样式
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.ForeColor = Color.Black;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].Style.BackColor = autocolor;
                            rgt_timeDistribution.Rows[SXCode.Count - 1].Cells[2].ReadOnly = true;
                        }
                        catch { }
                    }
                    else if (workCodeList.Count >= 27)
                    {
                        MessageBox.Show(message_list[14]);
                    }
                }
                SetSXTime(1);
                SetSXTime(2);
            }
            //弹出日期框
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
            {
                //记录下选择Cell的row
                mouse_SelectRow = e.RowIndex;
                //记录下选择Cell的Col
                mouse_SelectCol = e.ColumnIndex;
                //记录点击表是下表
                mouse_SelectTable = ReportTable.sheet2_rgv_deviation_day;
                //显示日期控件 让用户选择日期
                pl_CalendarAndTime2.Visible = true;
            }
            else
            {
                pl_CalendarAndTime2.Visible = false;
            }
        }
        #endregion

        #region 设置右键删除添加行事件
        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Insert")
            {
                this.rmv_operation_night.Rows.Add(1);
                //选择时间的两列要设置为不可编辑状态
                this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[0].Value = "";
                this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[0].ReadOnly = true;
                this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[1].Value = "";
                this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[1].ReadOnly = true;
                for (int col = 5; col < rmv_operation_night.Columns.Count; col++)
                {
                    this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[col].Value = "";
                    this.rmv_operation_night.Rows[rmv_operation_night.Rows.Count - 1].Cells[col].ReadOnly = true;
                }

            }
            else if (e.ClickedItem.Text == "Delete")
            {
                this.rmv_operation_night.Rows.Remove(this.rmv_operation_night.SelectedRows[0]);
                pl_CalendarAndTime.Visible = false;
            }
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Insert")
            {
                this.rgv_drecord_night.Rows.AddNew();
            }
            else if (e.ClickedItem.Text == "Delete")
            {
                this.rgv_drecord_night.Rows.Remove(this.rgv_drecord_night.SelectedRows[0]);
            }
        }

        private void contextMenuStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Insert")
            {
                this.rgv_laseCasing.Rows.AddNew();
            }
            else if (e.ClickedItem.Text == "Delete")
            {
                this.rgv_laseCasing.Rows.Remove(this.rgv_laseCasing.SelectedRows[0]);
            }
        }

        private void contextMenuStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //隐藏日期选择控件
            pl_CalendarAndTime.Visible = false;
            //如果点击了清空那么改行清空
            if (e.ClickedItem.Text == "Clear Row")
            {
                if (mouse_SelectTable == ReportTable.sheet2_rmv_operation_night)
                {
                    for (int i = 0; i < rmv_operation_night.Columns.Count; i++)
                    {
                        rmv_operation_night.SelectedRows[0].Cells[i].Value = "";
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 钻头数据Cell点击事件
        /// </summary>
        private void radGridBitRecord_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex != 3 && e.RowIndex != 4)
            {
                SelectTable SelectForm = new SelectTable();
                SelectForm.TabelName = "BitRecord";
                SelectForm.ShowDialog();
                var model = SelectForm.BitRecord;
                if (model != null)
                {
                    rgv_bitRecord_night.Rows[0].Cells[e.ColumnIndex].Value = model.BitNo;
                    rgv_bitRecord_night.Rows[1].Cells[e.ColumnIndex].Value = model.Size;
                    rgv_bitRecord_night.Rows[2].Cells[e.ColumnIndex].Value = model.IADCCode;
                    rgv_bitRecord_night.Rows[5].Cells[e.ColumnIndex].Value = model.SerialNO;
                    rgv_bitRecord_night.Rows[6].Cells[e.ColumnIndex].Value = model.JETS;
                    rgv_bitRecord_night.Rows[7].Cells[e.ColumnIndex].Value = model.TFA;
                    rgv_bitRecord_night.Rows[8].Cells[e.ColumnIndex].Value = model.DepthOut;
                    rgv_bitRecord_night.Rows[9].Cells[e.ColumnIndex].Value = model.DepthIn;
                    rgv_bitRecord_night.Rows[10].Cells[e.ColumnIndex].Value = model.TotalDrilled;
                    rgv_bitRecord_night.Rows[11].Cells[e.ColumnIndex].Value = model.TotalHours;
                }
            }
        }

        /// <summary>
        /// 钻具组合Cell点击事件
        /// </summary>
        private void radGridDrillAsmb_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            SelectTable SelectForm = new SelectTable();
            SelectForm.TabelName = "DrillAsmb";
            SelectForm.ShowDialog();
            var model = SelectForm.DrillingAssembly;
            if (model != null)
            {
                rgda_drillAss_night.Rows[e.RowIndex].Cells[0].Value = model.CodeNo;
                rgda_drillAss_night.Rows[e.RowIndex].Cells[1].Value = model.Item;
                rgda_drillAss_night.Rows[e.RowIndex].Cells[2].Value = model.Length;
            }
            TotalDA_Night();
        }

        /// <summary>
        /// 添加剂记录Cell点击事件
        /// </summary>
        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 2) && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 4;
                SelectForm.ShowDialog();
                rgv_mudchemicals_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
        }

        /// <summary>
        /// 报表三上部分 CREW CEll 点击事件
        /// </summary>
        private void radGridView8_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 5;
                SelectForm.ShowDialog();
                rgv_drillCrew_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 6;
                SelectForm.ShowDialog();
                rgv_drillCrew_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
        }

        /// <summary>
        /// 报表三 下部分 CREW CELL 点击事件
        /// </summary>
        private void radGridView9_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 5;
                SelectForm.ShowDialog();
                rgv_drillCrew_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
            if (e.ColumnIndex == 5 && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 6;
                SelectForm.ShowDialog();
                rgv_drillCrew_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
        }

        #region 读取xml文件设置语言
        private void setControlLanguage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
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
                            #region
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "lbl_message")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    message_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region Activity
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "Activity")
                            {
                                XmlNodeList m = node.ChildNodes;
                                int LanuangeCnt = 1;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    LanuangeDictionary.Add("M" + LanuangeCnt, xe2.GetAttribute("value"));
                                    ++LanuangeCnt;
                                }
                                continue;
                            }
                            #endregion
                            #region Bit Record
                            if (xe.GetAttribute("key") == "BitRecord")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    bit_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region Mud Record
                            if (xe.GetAttribute("key") == "MudRecord")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    mud_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region Cutting Record
                            if (xe.GetAttribute("key") == "CutRecord")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    cut_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region Completion
                            if (xe.GetAttribute("key") == "Completion")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    com_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region DayWork
                            if (xe.GetAttribute("key") == "DayWork")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    dwork_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            #region Drill Crew
                            if (xe.GetAttribute("key") == "Crew")
                            {
                                XmlNodeList m = node.ChildNodes;
                                foreach (XmlNode node2 in m)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    crew_list.Add(xe2.GetAttribute("value"));
                                }
                                continue;
                            }
                            #endregion
                            if (xe.GetAttribute("key") == "caculate")
                            {
                                this.btn_operation_night.Text = xe.GetAttribute("value");
                                this.btn_operation_day.Text = xe.GetAttribute("value");
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对pageview
                                    if (c.Name == "radPageView1")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//到达节点Form-xxxForm-Control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;//循环当前节点下的Control
                                            foreach (Control ctl in c.Controls)
                                            {
                                                if (ctl.Name == xe2.GetAttribute("key"))
                                                {
                                                    if (ctl.Name == "rp_lease" || ctl.Name == "rp_ors" || ctl.Name == "rp_dc")
                                                    {
                                                        ((Control)ctl).Text = xe2.GetAttribute("value");
                                                        XmlNodeList xn_list3 = node2.ChildNodes;//到达节点Form-xxxForm-Control-Control
                                                        foreach (XmlNode node3 in xn_list3)
                                                        {
                                                            XmlElement xe3 = (XmlElement)node3;
                                                            #region rmv_operation 对应早晚两班
                                                            //分别对每个gridview进行操作
                                                            if (xe3.GetAttribute("key") == "rmv_operation")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    if (xe4.GetAttribute("key") == "ColumnName")
                                                                    {
                                                                        XmlNodeList xn_list5 = node4.ChildNodes;
                                                                        int i = 0;
                                                                        foreach (XmlNode node5 in xn_list5)
                                                                        {
                                                                            XmlElement xe5 = (XmlElement)node5;
                                                                            rmv_operation_night.Columns[i].HeaderText = xe5.GetAttribute("value");
                                                                            rmv_operation_day.Columns[i].HeaderText = xe5.GetAttribute("value");
                                                                            i++;
                                                                        }
                                                                    }
                                                                    else if (xe4.GetAttribute("key") == "Header")
                                                                    {
                                                                        XmlNodeList xn_list5 = node4.ChildNodes;
                                                                        int i = 0;
                                                                        foreach (XmlNode node5 in xn_list5)
                                                                        {
                                                                            XmlElement xe5 = (XmlElement)node5;
                                                                            HeaderLanuage.Add(xe5.GetAttribute("value"));
                                                                            i++;
                                                                        }
                                                                    }
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgt_timeDistribution
                                                            //分别对每个gridview进行操作
                                                            if (xe3.GetAttribute("key") == "rgt_timeDistribution")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgt_timeDistribution.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgv_drillPipe
                                                            if (xe3.GetAttribute("key") == "rgv_drillPipe")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_drillPipe.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgda_drillAss早晚
                                                            if (xe3.GetAttribute("key") == "rgda_drillAss")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgda_drillAss_night.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgda_drillAss_day.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgv_mudchemicals早晚
                                                            if (xe3.GetAttribute("key") == "rgv_mudchemicals")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_mudchemicals_night.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_mudchemicals_day.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgv_drecord早晚
                                                            if (xe3.GetAttribute("key") == "rgv_drecord")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_drecord_night.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drecord_night.Columns[i + 5].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drecord_night.Columns[i + 10].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drecord_day.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drecord_day.Columns[i + 5].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drecord_day.Columns[i + 10].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgv_laseCasing
                                                            if (xe3.GetAttribute("key") == "rgv_laseCasing")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_laseCasing.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region rgv_deviation 早晚
                                                            if (xe3.GetAttribute("key") == "rgv_deviation")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_deviation_night.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_deviation_day.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            #region
                                                            if (xe3.GetAttribute("key") == "rgv_drillCrew")
                                                            {
                                                                XmlNodeList xn_list4 = node3.ChildNodes;//到达节点Form-xxxForm-Control-Control-Control
                                                                int i = 0;
                                                                foreach (XmlNode node4 in xn_list4)
                                                                {
                                                                    XmlElement xe4 = (XmlElement)node4;
                                                                    rgv_drillCrew_night.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    rgv_drillCrew_day.Columns[i].HeaderText = xe4.GetAttribute("value");
                                                                    i++;
                                                                }
                                                                continue;
                                                            }
                                                            #endregion
                                                            foreach (Control ctr3 in ctl.Controls)
                                                            {
                                                                if (ctr3.Name == xe3.GetAttribute("key"))
                                                                {
                                                                    ctr3.Text = xe3.GetAttribute("value");
                                                                    break;
                                                                }
                                                            }
                                                        }

                                                    }
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

        #region 前两列显示日期时间控件
        private void rmv_operation_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rmv_operation_night;
        }
        #endregion

        #region 日期控件被点击事件
        private void radCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            //获取点击的时间
            dt = this.radCalendar1.FocusedDate;
            //获取当前控件的时间
            var dtm = radTimePicker1.Value;
            //只取年月日
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            //时间加上小时
            dt = dt.AddHours(dtm.Value.Hour);
            dt = dt.AddMinutes(dtm.Value.Minute);

            var startDt = new DateTime();
            var endDt = new DateTime();
            //被选择的是下面的那张表
            if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_night)
            {
                rgv_deviation_night.SelectedRows[0].Cells[mouse_SelectCol].Value = dt.ToString("yyyy-MM-dd HH:mm:00");//这个表 精确到小时就够 否则不利于后续计算
                if (!IsEmptyOrNull(rgv_deviation_night.SelectedRows[0].Cells[0].Value) && !IsEmptyOrNull(rgv_deviation_night.SelectedRows[0].Cells[1].Value))
                {
                    startDt = DateTime.Parse(rgv_deviation_night.SelectedRows[0].Cells[0].Value.ToString());
                    endDt = DateTime.Parse(rgv_deviation_night.SelectedRows[0].Cells[1].Value.ToString());
                }
            }
            //隐藏日期时间控件
            pl_CalendarAndTime.Visible = false;

            //如果用户输入时间错误 那么提示 + 清空
            if (DateTime.Compare(startDt, endDt) > 0)
            {
                MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                //被选择的表是 表2下表
                if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_night)
                {
                    rgv_deviation_night.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
                }
            }
            else
            {
                try
                {
                    TimeSpan t = Convert.ToDateTime(rgv_deviation_night.SelectedRows[0].Cells[1].Value.ToString()) - Convert.ToDateTime(rgv_deviation_night.SelectedRows[0].Cells[0].Value.ToString());
                    rgv_deviation_night.SelectedRows[0].Cells[2].Value = t.TotalHours;
                    //如果第4列不为空
                    try
                    {
                        int codeNo = int.Parse(rgv_deviation_night.SelectedRows[0].Cells[3].Value.ToString());
                        double old_v = 0;
                        try
                        {
                            for (int i = 0; i < this.rgv_deviation_night.Rows.Count && codeNo != 0; i++)
                            {
                                if (this.rgv_deviation_night.Rows[i].Cells[3].Value.ToString() == codeNo.ToString())
                                {
                                    try
                                    {
                                        old_v += double.Parse(this.rgv_deviation_night.Rows[i].Cells[2].Value.ToString());//之前的值
                                    }
                                    catch { }
                                }
                            }
                            this.rgt_timeDistribution.Rows[codeNo - 1].Cells[1].Value = old_v;
                        }
                        catch { }
                        SetSXTime(1);
                    }
                    catch { }
                }
                catch { }
            }
            
        }
        #endregion

        #region 根据字符串转换成对应的10位数字时间戳
        /// <summary>  
        /// 根据字符串转换成对应的时间戳 
        /// </summary>  
        /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>  
        /// <returns></returns>  
        private long GetTimeStamp(string NeedConvertTime)
        {
            //被转换的字符串转换成时间类型
            DateTime dt2 = DateTime.ParseExact(NeedConvertTime, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.CurrentCulture);
            //减去初始时间得到 时间间隔
            TimeSpan ts = dt2 - TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
            //返回10位的时间戳
            long ret = Convert.ToInt32(ts.TotalSeconds);
            return ret;
        }
        #endregion

        #region 判断是否为整数字符串
        /// <summary>
        /// 判断是否为整数字符串  是的话则返回true, 否则为false
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool isNumberic(string numberic)
        {
            int number = 0;
            try
            {
                number = Convert.ToInt32(numberic);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 判断是否为浮点数字符串
        /// <summary>
        /// 判断是否为浮点数字符串  是的话则返回true, 否则为false
        /// </summary>
        /// <param name="message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool isNumbericD(string numberic)
        {
            double number = 0;
            try
            {
                number = Convert.ToDouble(numberic);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 请求API获得被查询的泵冲Tag数据 用于表格自动查询
        /// <summary>
        /// 请求API获得被查询的泵冲Tag数据 用于表格自动查询
        /// </summary>
        /// <param name="QueryModel"></param>
        /// <returns></returns>
        public OutTagAS GetTagData(InputTagAS QueryModel)
        {
            OutTagAS QueryResult = new OutTagAS();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["HistorianTagAS"].ToString());
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";
            try
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = new JavaScriptSerializer().Serialize(QueryModel);

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();//全是三斜杠
                    object tags = new JavaScriptSerializer().DeserializeObject(result);//转换成object之后全是单斜杠
                    QueryResult = new JavaScriptSerializer().Deserialize<OutTagAS>(tags.ToString());
                }
            }
            catch
            {

            }

            //返回查询结果
            return QueryResult;
        }
        #endregion

        #region 日期控件的右上角'X'被点击事件 清空选中该行
        private void radButton2_Click_1(object sender, EventArgs e)
        {
            //被选择的是上面那张表
            if (mouse_SelectTable == ReportTable.sheet2_rmv_operation_night)
            {
                rmv_operation_night.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
            }

            //被选择的是下面的那张表
            if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_night)
            {
                rgv_deviation_night.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
            }
            pl_CalendarAndTime.Visible = false;
        }
        #endregion

        #region 清空表格指定行的自动查询数据
        private void clearQueryTable(int Row)
        {
            //当开始时间和结束时间有数据时 代码运行到这里 说明 用户时间选择错误，例如结束时间小于了开始时间
            if (this.rmv_operation_night.Rows[Row].Cells[0].Value != null && this.rmv_operation_night.Rows[Row].Cells[1].Value != null)
            {
                //清空选择错误的两个时间
                this.rmv_operation_night.Rows[Row].Cells[0].Value = "";
                this.rmv_operation_night.Rows[Row].Cells[1].Value = "";
            }
            //把当前鼠标选中的这一行清空
            //for (int col = 5; col < rmv_operation.Columns.Count - 1; col++)
            //{
            //    this.rmv_operation.Rows[Row].Cells[col].Value = "";
            //}
        }
        #endregion

        //自动计算合计时间
        private void Completion_CellClick(object sender, GridViewCellEventArgs e)
        {
            if(e.ColumnIndex==1){
                SetSXTime(1);
            }
            else if (e.ColumnIndex == 2)
            {
                SetSXTime(2);
            }
        }

        #region 每个表单被点击后 记录下当前鼠标点击表单
        //钻井表单被点击
        private void rgda_drillAss_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet1_rgda_drillAss_night;
        }
        //记录表单
        private void rgbr_bitRecord_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet1_rgbr_bitRecord;
        }
        private void rgv_mudchemicals_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet1_rgv_mudchemicals_night;
        }
        private void rmv_operation_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rmv_operation_night;
        }
        private void radGridView4_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_radGridView4;
        }

        private void rgv_wireline_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rgv_wireline;
        }

        private void rgv_laseCasing_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rgv_laseCasing;
        }

        private void rgv_deviation_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rgv_deviation_night;
        }

        private void rgv_drillCrew_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet3_rgv_drillCrew;
        }

        private void radGridView9_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet3_radGridView9;
        }
        #endregion

        //时间控件被选择
        private void TimePicker_Click(object sender, EventArgs e)
        {

        }

        //该对象是否为空或null
        private bool IsEmptyOrNull(object str)
        {
            if (str == null)
            {
                return true;
            }
            else if (str.ToString() == "")
            {
                return true;
            }
            return false;
        }

        #region 弹出右键 清空该表选中的该行
        private void mns_rgda_drillAss_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgda_drillAss_night.SelectedRows[0].Cells.Count; i++)
                rgda_drillAss_night.SelectedRows[0].Cells[i].Value = "";
            TotalDA_Night();
        }

        private void mns_rgbr_bitRecord_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_bitRecord_night.Rows.Count; i++)
            {
                rgv_bitRecord_night.Rows[i].Cells[1].Value = "";
            }
        }

        private void mns_rgv_mudchemicals_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_mudchemicals_night.Rows.Count; i++)
            {
                rgv_mudchemicals_night.Rows[i].Cells[0].Value = "";
                rgv_mudchemicals_night.Rows[i].Cells[1].Value = "";
            }
        }

        private void mns_radGridView4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_drecord_night.Columns.Count; i++)
            {
                rgv_drecord_night.SelectedRows[0].Cells[i].Value = "";
            }
        }



        private void mns_rgv_laseCasing_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_laseCasing.Columns.Count; i++)
            {
                rgv_laseCasing.SelectedRows[0].Cells[i].Value = "";
            }
        }

        private void mns_rgv_deviation_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int codeNo = 0;
            for (int i = 0; i < rgv_deviation_night.Columns.Count; i++)
            {
                try
                {
                    //清除此行，如果此行之前有数据则重新计算
                    if (i == 3)
                    {
                        try
                        {
                            codeNo = int.Parse(rgv_deviation_night.SelectedRows[0].Cells[i].Value.ToString());
                            double old_v = 0;
                            try
                            {
                                for (int j = 0; j < this.rgv_deviation_night.Rows.Count && codeNo != 0; j++)
                                {
                                    if (this.rgv_deviation_night.Rows[j].Cells[3].Value.ToString() == codeNo.ToString())
                                    {
                                        try
                                        {
                                            old_v += double.Parse(this.rgv_deviation_night.Rows[j].Cells[2].Value.ToString());//之前的值
                                        }
                                        catch { }
                                    }
                                }
                                this.rgt_timeDistribution.Rows[codeNo - 1].Cells[1].Value = old_v;
                            }
                            catch { }
                            SetSXTime(1);
                        }
                        catch { }
                    }
                    rgv_deviation_night.SelectedRows[0].Cells[i].Value = "";
                }
                catch { }
            }
            //清空完该行的数据后 要重新计算 表1的时效表列表
            //synSheet1();
        }

        private void mns_rgv_drillCrew_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_drillCrew_night.Columns.Count; i++)
            {
                rgv_drillCrew_night.SelectedRows[0].Cells[i].Value = "";
            }
        }

        private void mns_radGridView9_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_drillCrew_day.Columns.Count; i++)
            {
                rgv_drillCrew_day.SelectedRows[0].Cells[i].Value = "";
            }
        }
        #endregion

        //防止用户错误操作 表2上表弹出右键菜单时 要隐藏日期时间控件
        private void contextMenuStrip5_Opened(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
            pl_CalendarAndTime2.Visible = false;
        }
        //防止用户错误操作 表2下表弹出右键菜单时 要隐藏日期时间控件
        private void mns_rgv_deviation_Opened(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
            pl_CalendarAndTime2.Visible = false;
        }
        //清除掉该字符串中所有空格
        private string TrimALL(string str)
        {
            return str.Replace(" ", "");
        }

        private void ReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
             
            //admin不能关闭此界面，涉及报表自动导出
            if (AppDrill.permissionId==1)
            {
                this.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
            else
            {
                this.Dispose();
            }
        }

        //计算数据并回填表格
        private void timeDistribution_FillData()
        {
            try
            {
                //清空字典
                TagArray.Clear();

                //循环行数
                for (var row = 0; row < ActivityList.Count; row++)
                {
                    var start = ActivityList[row].SelectDateForm;//第1列 开始时间   
                    var end = ActivityList[row].SelectDateTo;//第2列 结束时间
                    //var col3 = ((col2.Value - col1.Value).TotalHours).ToString("0.00");//第3列 间隔的小时
                    var col4 = ActivityList[row].ActivityName;//第4列 TagName  

                    if (col4.ToString().Trim() == "")
                    {
                        continue;
                    }

                    DateTime d_start = DateTime.Parse(start == null ? DateTime.Now.ToString() : start.ToString());
                    DateTime d_end = DateTime.Parse(end == null ? DateTime.Now.ToString() : end.ToString());
                    if (TagArray.ContainsKey(TrimALL(col4.ToString())))
                    {
                        // 存在Tag 则更新值
                        TagDayMiddleNightModel ValueModel = TagArray[TrimALL(col4.ToString())];
                        //早班
                        //处理开始时间在早班之前，开始时间2种情况，早班之前，晚班之前
                        if (d_start < d_morning)
                        {
                            //结束时间在早班之前,晚班之后
                            if (d_end >= d_evening)
                            {
                                ValueModel.Value[1] += 12;
                            }
                            else //结束时间在晚班之前，开始时间在早班之前，时间以早班时间为节点
                            {
                                ValueModel.Value[1] += (d_end - d_morning).TotalHours;
                            }
                        }
                        else if (d_start >= d_morning && d_start < d_evening)//开始时间在早班之前,晚班之前
                        {
                            //结束时间晚班之前
                            if (d_end <= d_evening)
                            {
                                ValueModel.Value[1] += (d_end - d_start).TotalHours;
                            }
                            else//结束时间在晚班之后，以晚班为时间节点
                            {
                                ValueModel.Value[1] += (d_evening - d_start).TotalHours;
                            }
                        }
                        //晚班
                        //晚班时间情况，开始时间处于晚班之前，晚班之后，结束时间必须在晚班之后
                        if (d_end > d_evening)
                        {
                            if (d_start < d_evening)
                            {
                                ValueModel.Value[0] += (d_end - d_evening).TotalHours;
                            }
                            else
                            {
                                ValueModel.Value[0] += (d_end - d_start).TotalHours;
                            }
                        }
                        TagArray[TrimALL(col4.ToString())] = ValueModel;
                    }
                    else
                    {
                        //不存在该TagName则新建一个Tag
                        TagDayMiddleNightModel ValueModel = new TagDayMiddleNightModel();

                        ValueModel.Type.Add(0);
                        ValueModel.Type.Add(1);
                        ValueModel.Value.Add(0);
                        ValueModel.Value.Add(0);

                        //早班
                        //处理开始时间在早班之前，开始时间2种情况，早班之前，晚班之前
                        if (d_start < d_morning)
                        {
                            //结束时间在早班之前,晚班之后
                            if (d_end >= d_evening)
                            {
                                ValueModel.Value[1] += 12;
                            }
                            else //结束时间在晚班之前，开始时间在早班之前，时间以早班时间为节点
                            {
                                ValueModel.Value[1] += (d_end - d_morning).TotalHours;
                            }
                        }
                        else if (d_start >= d_morning && d_start < d_evening)//开始时间在早班之前,晚班之前
                        {
                            //结束时间晚班之前
                            if (d_end <= d_evening)
                            {
                                ValueModel.Value[1] += (d_end - d_start).TotalHours;
                            }
                            else//结束时间在晚班之后，以晚班为时间节点
                            {
                                ValueModel.Value[1] += (d_evening - d_start).TotalHours;
                            }
                        }
                        //晚班
                        //晚班时间情况，开始时间处于晚班之前，晚班之后，结束时间必须在晚班之后
                        if (d_end > d_evening)
                        {
                            if (d_start < d_evening)
                            {
                                ValueModel.Value[0] += (d_end - d_evening).TotalHours;
                            }
                            else
                            {
                                ValueModel.Value[0] += (d_end - d_start).TotalHours;
                            }
                        }
                        TagArray.Add(TrimALL(col4.ToString()), ValueModel);
                    }
                }
            }
            catch { }
        }
        #region 修改AppConfig

        private void SetValue(string AppKey, string AppValue)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", AppKey);
                xElem2.SetAttribute("value", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        #endregion

        /// <summary>
        /// 赋值操作
        /// </summary>
        private void SetBasicData()
        {
            try
            {
                if (Drill != null)
                {
                    txt_lease.Text = Drill.Lease;
                    txt_operator.Text = Drill.ToolPusher;
                    txt_contractor.Text = Drill.Contractor;
                    txt_wellNo.Text = Drill.DrillNo;//当前井号

                    txt_state.Text = Drill.Country;

                    txt_wnan.Text = Drill.DrillNo;
                    txt_company.Text = Drill.Company;
                    txt_tool.Text = Drill.ToolPusher;
                }
            }
            catch { }
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void rp_lease_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rmv_operation_day_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rmv_operation_day;
        }

        private void radCalendar2_SelectionChanged(object sender, EventArgs e)
        {
            //获取点击的时间
            dt = this.radCalendar2.FocusedDate;
            //获取当前控件的时间
            var dtm = radTimePicker2.Value;
            //只取年月日
            dt = new DateTime(dt.Year, dt.Month, dt.Day);
            //时间加上小时
            dt = dt.AddHours(dtm.Value.Hour);
            dt = dt.AddMinutes(dtm.Value.Minute);

            var startDt = new DateTime();
            var endDt = new DateTime();
            if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_day)
            {
                rgv_deviation_day.SelectedRows[0].Cells[mouse_SelectCol].Value = dt.ToString("yyyy-MM-dd HH:00:00");//这个表 精确到小时就够 否则不利于后续计算
                if (!IsEmptyOrNull(rgv_deviation_day.SelectedRows[0].Cells[0].Value) && !IsEmptyOrNull(rgv_deviation_day.SelectedRows[0].Cells[1].Value))
                {
                    startDt = DateTime.Parse(rgv_deviation_day.SelectedRows[0].Cells[0].Value.ToString());
                    endDt = DateTime.Parse(rgv_deviation_day.SelectedRows[0].Cells[1].Value.ToString());
                }
                //如果用户输入时间错误 那么提示 + 清空
                if (DateTime.Compare(startDt, endDt) > 0)
                {
                    MessageBox.Show(message_list[1], message_list[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //被选择的表是 表2下表
                    if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_day)
                    {
                        rgv_deviation_day.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
                        return;
                    }
                }
                else
                {
                    try
                    {
                        TimeSpan t = Convert.ToDateTime(rgv_deviation_day.SelectedRows[0].Cells[1].Value.ToString()) - Convert.ToDateTime(rgv_deviation_day.SelectedRows[0].Cells[0].Value.ToString());
                        rgv_deviation_day.SelectedRows[0].Cells[2].Value = t.TotalHours;
                        //如果第4列不为空
                        try
                        {
                            int codeNo = int.Parse(rgv_deviation_day.SelectedRows[0].Cells[3].Value.ToString());
                            double old_v=0;
                            try
                            {
                                for (int i = 0; i < this.rgv_deviation_day.Rows.Count && codeNo != 0; i++)
                                {
                                    if (this.rgv_deviation_day.Rows[i].Cells[3].Value.ToString() == codeNo.ToString())
                                    {
                                        try
                                        {
                                            old_v += double.Parse(this.rgv_deviation_day.Rows[i].Cells[2].Value.ToString());//之前的值
                                        }
                                        catch { }
                                    }
                                }
                                this.rgt_timeDistribution.Rows[codeNo - 1].Cells[2].Value = old_v;
                            }
                            catch { }
                        }
                        catch { }
                        SetSXTime(2);
                    }
                    catch { }
                }
            }
            //隐藏日期时间控件
            pl_CalendarAndTime2.Visible = false;

            
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            //被选择的是上面那张表
            if (mouse_SelectTable == ReportTable.sheet2_rmv_operation_day)
            {
                rmv_operation_day.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
            }

            //被选择的是下面的那张表
            if (mouse_SelectTable == ReportTable.sheet2_rgv_deviation_day)
            {
                rgv_deviation_day.SelectedRows[0].Cells[mouse_SelectCol].Value = "";
            }
            pl_CalendarAndTime2.Visible = false;
        }

        private void rmv_operation_day_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rmv_operation_day;
        }

        private void rgv_deviation_day_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet2_rgv_deviation_day;
        }

        

        private void rgv_mudchemicals_day_CellClick(object sender, GridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 2) && e.RowIndex >= 0)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 4;
                SelectForm.ShowDialog();
                rgv_mudchemicals_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
        }

        private void rgda_drillAss_day_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            SelectTable SelectForm = new SelectTable();
            SelectForm.TabelName = "DrillAsmb";
            SelectForm.ShowDialog();
            var model = SelectForm.DrillingAssembly;
            if (model != null)
            {
                rgda_drillAss_day.Rows[e.RowIndex].Cells[0].Value = model.CodeNo;
                rgda_drillAss_day.Rows[e.RowIndex].Cells[1].Value = model.Item;
                rgda_drillAss_day.Rows[e.RowIndex].Cells[2].Value = model.Length;
            }
            TotalDA_Day();
        }

        private void contextMenuStrip6_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //隐藏日期选择控件
            pl_CalendarAndTime.Visible = false;
            //如果点击了清空那么改行清空
            if (e.ClickedItem.Text == "Clear Row")
            {
                if (mouse_SelectTable == ReportTable.sheet1_rgda_drillAss_day)
                {
                    for (int i = 0; i < rgda_drillAss_day.Columns.Count; i++)
                    {
                        rgda_drillAss_day.SelectedRows[0].Cells[i].Value = "";
                    }
                    TotalDA_Day();
                }
            }
        }

        private void rgda_drillAss_day_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet1_rgda_drillAss_day;
        }

        private void rgv_mudchemicals_day_Click(object sender, EventArgs e)
        {
            mouse_SelectTable = ReportTable.sheet1_rgv_mudchemicals_day;
        }

        private void contextMenuStrip7_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            for (int i = 0; i < rgv_mudchemicals_day.Rows.Count; i++)
            {
                rgv_mudchemicals_day.Rows[i].Cells[0].Value = "";
                rgv_mudchemicals_day.Rows[i].Cells[1].Value = "";
            }
        }

        private void mns_rgv_deviation_day_Opened(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
            pl_CalendarAndTime2.Visible = false;
        }

        private void mns_rgv_deviation_day_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int codeNo = 0;
            for (int i = 0; i < rgv_deviation_day.Columns.Count; i++)
            {
                try
                {
                    //清除此行，如果此行之前有数据则重新计算
                    if (i == 3)
                    {
                        try
                        {
                            codeNo = int.Parse(rgv_deviation_day.SelectedRows[0].Cells[i].Value.ToString());
                            double old_v = 0;
                            try
                            {
                                for (int j = 0; j < this.rgv_deviation_day.Rows.Count && codeNo != 0; j++)
                                {
                                    if (this.rgv_deviation_day.Rows[j].Cells[3].Value.ToString() == codeNo.ToString())
                                    {
                                        try
                                        {
                                            old_v += double.Parse(this.rgv_deviation_day.Rows[j].Cells[2].Value.ToString());//之前的值
                                        }
                                        catch { }
                                    }
                                }
                                this.rgt_timeDistribution.Rows[codeNo - 1].Cells[2].Value = old_v;
                            }
                            catch { }
                            SetSXTime(2);
                        }
                        catch { }
                    }
                    rgv_deviation_day.SelectedRows[0].Cells[i].Value = "";
                }
                catch { }
            }
        }

        private void contextMenuStrip8_Opened(object sender, EventArgs e)
        {
            pl_CalendarAndTime.Visible = false;
            pl_CalendarAndTime2.Visible = false;
        }

        private void contextMenuStrip8_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //隐藏日期选择控件
            pl_CalendarAndTime2.Visible = false;
            //如果点击了清空那么改行清空
            if (e.ClickedItem.Text == "Clear Row")
            {
                if (mouse_SelectTable == ReportTable.sheet2_rmv_operation_day)
                {
                    for (int i = 0; i < rmv_operation_night.Columns.Count; i++)
                    {
                        rmv_operation_day.SelectedRows[0].Cells[i].Value = "";
                    }
                }
            }
        }

        private void rgv_drillCrew_night_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 5)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 6;
                SelectForm.ShowDialog();
                rgv_drillCrew_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
            if (e.ColumnIndex <= 2)
            {
                WorkerManagement wm = new WorkerManagement();
                wm.isSelect = true;
                wm.ShowDialog();
                if (wm.worker != null)
                {
                    this.rgv_drillCrew_night.Rows[e.RowIndex].Cells[0].Value = wm.list_type.Where(o => o.ID == wm.worker.TypeWork).FirstOrDefault().Type;
                    this.rgv_drillCrew_night.Rows[e.RowIndex].Cells[1].Value = wm.worker.EmpNO;
                    this.rgv_drillCrew_night.Rows[e.RowIndex].Cells[2].Value = wm.worker.Name;
                }
            }
        }

        private void rgv_drillCrew_day_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == 5)
            {
                SelectList SelectForm = new SelectList();
                SelectForm.isWhat = 6;
                SelectForm.ShowDialog();
                rgv_drillCrew_day.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = SelectForm.selectText;
            }
            if (e.ColumnIndex <= 2)
            {
                WorkerManagement wm = new WorkerManagement();
                wm.isSelect = true;
                wm.ShowDialog();
                if (wm.worker != null)
                {
                    this.rgv_drillCrew_day.Rows[e.RowIndex].Cells[0].Value = wm.list_type.Where(o => o.ID == wm.worker.TypeWork).FirstOrDefault().Type;
                    this.rgv_drillCrew_day.Rows[e.RowIndex].Cells[1].Value = wm.worker.EmpNO;
                    this.rgv_drillCrew_day.Rows[e.RowIndex].Cells[2].Value = wm.worker.Name;
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            report = new Report();
            TableDetail td = new TableDetail();
            #region 报表 RIG INFORMATION
            try
            {
                using (var fs = File.OpenRead(@"" + System.Environment.CurrentDirectory + "\\Template\\DAILY DRILLING REPORT.xls"))
                {
                    workbook = new HSSFWorkbook(fs);  // 2007版本 .xlsx（new HSSFWorkbook(); 2003版本 .xls）
                    #region 第一张报表
                    ISheet sheet = workbook.GetSheetAt(0);
                    #region 顶部基础数据
                    sheet.GetRow(1).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_lease.Text);
                    td.TableName = txt_lease.Name;
                    td.RowDatas.Add(txt_lease.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(1).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_wellNo.Text);

                    sheet.GetRow(2).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_operator.Text);
                    td = new TableDetail();
                    td.TableName = txt_operator.Name;
                    td.RowDatas.Add(txt_operator.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(1).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_apiNumber.Text);
                    td = new TableDetail();
                    td.TableName = txt_apiNumber.Name;
                    td.RowDatas.Add(txt_apiNumber.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(1).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_waterDepth.Text);
                    td = new TableDetail();
                    td.TableName = txt_waterDepth.Name;
                    td.RowDatas.Add(txt_waterDepth.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(2).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_contractor.Text);
                    td = new TableDetail();
                    td.TableName = txt_contractor.Name;
                    td.RowDatas.Add(txt_contractor.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(2).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_rigNo.Text);
                    td = new TableDetail();
                    td.TableName = txt_rigNo.Name;
                    td.RowDatas.Add(txt_rigNo.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(1).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rdtp_date.Value.ToString("yyyy-MM-dd"));

                    sheet.GetRow(3).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_soor.Text);

                    sheet.GetRow(3).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_soct.Text);

                    sheet.GetRow(31).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_drill_night.Text);

                    sheet.GetRow(55).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_drill_day.Text);

                    #endregion

                    #region TIME DISTRIBUTION(时效分析)
                    td = new TableDetail();
                    td.TableName = rgt_timeDistribution.Name;//将表的名字保存进去
                    for (int i = 0; i < rgt_timeDistribution.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 11).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgt_timeDistribution.Rows[i].Cells[0].Value == null ? "" : rgt_timeDistribution.Rows[i].Cells[0].Value.ToString());
                        sheet.GetRow(i + 11).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgt_timeDistribution.Rows[i].Cells[1].Value == null ? "" : rgt_timeDistribution.Rows[i].Cells[1].Value.ToString());
                        data += rgt_timeDistribution.Rows[i].Cells[1].Value == null ? "" : rgt_timeDistribution.Rows[i].Cells[1].Value.ToString();
                        sheet.GetRow(i + 11).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgt_timeDistribution.Rows[i].Cells[2].Value == null ? "" : rgt_timeDistribution.Rows[i].Cells[2].Value.ToString());
                        data += rgt_timeDistribution.Rows[i].Cells[2].Value == null ? "," :","+ rgt_timeDistribution.Rows[i].Cells[2].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion

                    #region DRILL PIPE(钻杆)
                    td = new TableDetail();
                    td.TableName = rgv_drillPipe.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_drillPipe.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 5).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[0].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[0].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[0].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[0].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[1].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[1].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[1].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[1].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[2].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[2].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[2].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[2].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[3].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[3].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[3].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[3].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[4].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[4].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[4].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[4].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[5].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[5].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[5].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[5].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[6].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[6].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[6].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[6].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[7].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[7].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[7].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[7].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[8].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[8].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[8].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[8].Value.ToString();//需要存入数据库的字段
                        sheet.GetRow(i + 5).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillPipe.Rows[i].Cells[9].Value == null ? "" : rgv_drillPipe.Rows[i].Cells[9].Value.ToString());
                        data += rgv_drillPipe.Rows[i].Cells[9].Value == null ? "," : "," + rgv_drillPipe.Rows[i].Cells[9].Value.ToString();//需要存入数据库的字段
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中
                    #endregion

                    #region COMPLETION(完井作业)
                    td = new TableDetail();
                    td.TableName = rgc_completion.Name;//将表的名字保存进去
                    for (int i = 0; i < rgc_completion.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 38).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgc_completion.Rows[i].Cells[0].Value == null ? "" : rgc_completion.Rows[i].Cells[0].Value.ToString());
                        sheet.GetRow(i + 38).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgc_completion.Rows[i].Cells[1].Value == null ? "" : rgc_completion.Rows[i].Cells[1].Value.ToString());
                        data+=rgc_completion.Rows[i].Cells[1].Value == null ? "" : rgc_completion.Rows[i].Cells[1].Value.ToString();
                        sheet.GetRow(i + 38).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgc_completion.Rows[i].Cells[2].Value == null ? "" : rgc_completion.Rows[i].Cells[2].Value.ToString());
                        data+=rgc_completion.Rows[i].Cells[2].Value == null ? "," : "," + rgc_completion.Rows[i].Cells[2].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中
                    #endregion

                    #region DAYWORK TIME SUMMARY(作业时间概要)
                    td = new TableDetail();
                    td.TableName = rgds_daywork.Name;//将表的名字保存进去
                    for (int i = 0; i < rgds_daywork.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 48).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgds_daywork.Rows[i].Cells[1].Value == null ? "" : rgds_daywork.Rows[i].Cells[1].Value.ToString());
                        data+=rgds_daywork.Rows[i].Cells[1].Value == null ? "" : rgds_daywork.Rows[i].Cells[1].Value.ToString();
                        sheet.GetRow(i + 48).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgds_daywork.Rows[i].Cells[2].Value == null ? "" : rgds_daywork.Rows[i].Cells[2].Value.ToString());
                        data+=rgds_daywork.Rows[i].Cells[2].Value == null ? "," :","+ rgds_daywork.Rows[i].Cells[2].Value.ToString();
                        td.RowDatas.Add(data);
                        if (i > 3 && i < 8)
                        {
                            sheet.GetRow(i + 48).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgds_daywork.Rows[i].Cells[0].Value == null ? "" : rgds_daywork.Rows[i].Cells[0].Value.ToString());
                        }
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中
                    #endregion

                    #region daywork 附属4个文本框
                    sheet.GetRow(56).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_nodfs.Text);
                    td = new TableDetail();
                    td.TableName = txt_nodfs.Name;
                    td.RowDatas.Add(txt_nodfs.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(57).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_crh.Text);
                    td = new TableDetail();
                    td.TableName = txt_crh.Name;
                    td.RowDatas.Add(txt_crh.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(58).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_dmc.Text);
                    td = new TableDetail();
                    td.TableName = txt_crh.Name;
                    td.RowDatas.Add(txt_crh.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(59).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_tmc.Text);
                    td = new TableDetail();
                    td.TableName = txt_tmc.Name;
                    td.RowDatas.Add(txt_tmc.Text);
                    report.TableDetails.Add(td);//添加一条数据
                    #endregion

                    #region DRILLING ASSEMBLY(钻具组合) Night
                    td = new TableDetail();
                    td.TableName = rgda_drillAss_night.Name;//将表的名字保存进去
                    for (int i = 0; i < rgda_drillAss_night.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 11).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_night.Rows[i].Cells[0].Value == null ? "" : rgda_drillAss_night.Rows[i].Cells[0].Value.ToString());
                        data += rgda_drillAss_night.Rows[i].Cells[0].Value == null ? "" : rgda_drillAss_night.Rows[i].Cells[0].Value.ToString();
                        sheet.GetRow(i + 11).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_night.Rows[i].Cells[1].Value == null ? "" : rgda_drillAss_night.Rows[i].Cells[1].Value.ToString());
                        data += rgda_drillAss_night.Rows[i].Cells[1].Value == null ? "," : "," + rgda_drillAss_night.Rows[i].Cells[1].Value.ToString();
                        sheet.GetRow(i + 11).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_night.Rows[i].Cells[2].Value == null ? "" : rgda_drillAss_night.Rows[i].Cells[2].Value.ToString());
                        data += rgda_drillAss_night.Rows[i].Cells[2].Value == null ? "," : "," + rgda_drillAss_night.Rows[i].Cells[2].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中

                    sheet.GetRow(20).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_stands_night.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_stands_night.Name;
                    td.RowDatas.Add(rtxt_stands_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(21).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_singles_night.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_singles_night.Name;
                    td.RowDatas.Add(rtxt_singles_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(22).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_kelly_night.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_kelly_night.Name;
                    td.RowDatas.Add(rtxt_kelly_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    td = new TableDetail();
                    td.TableName = rtxt_total_night.Name;
                    sheet.GetRow(23).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_total_night.Text);
                    td.RowDatas.Add(rtxt_total_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(25).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_wos_night.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_wos_night.Name;
                    td.RowDatas.Add(rtxt_wos_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    #endregion

                    #region DRILLING ASSEMBLY(钻具组合) Day
                    td = new TableDetail();
                    td.TableName = rgda_drillAss_day.Name;//将表的名字保存进去
                    for (int i = 0; i < rgda_drillAss_day.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 36).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_day.Rows[i].Cells[0].Value == null ? "" : rgda_drillAss_day.Rows[i].Cells[0].Value.ToString());
                        data += rgda_drillAss_day.Rows[i].Cells[0].Value == null ? "" : rgda_drillAss_day.Rows[i].Cells[0].Value.ToString();
                        sheet.GetRow(i + 36).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_day.Rows[i].Cells[1].Value == null ? "" : rgda_drillAss_day.Rows[i].Cells[1].Value.ToString());
                        data += rgda_drillAss_day.Rows[i].Cells[1].Value == null ? "," : "," + rgda_drillAss_day.Rows[i].Cells[1].Value.ToString();
                        sheet.GetRow(i + 36).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgda_drillAss_day.Rows[i].Cells[2].Value == null ? "" : rgda_drillAss_day.Rows[i].Cells[2].Value.ToString());
                        data += rgda_drillAss_day.Rows[i].Cells[2].Value == null ? "," : "," + rgda_drillAss_day.Rows[i].Cells[2].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中

                    sheet.GetRow(45).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_stands_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_stands_day.Name;
                    td.RowDatas.Add(rtxt_stands_day.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(46).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_singles_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_singles_day.Name;
                    td.RowDatas.Add(rtxt_singles_day.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(47).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_kelly_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_kelly_day.Name;
                    td.RowDatas.Add(rtxt_kelly_day.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(48).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_total_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_total_day.Name;
                    td.RowDatas.Add(rtxt_total_day.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(50).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_wos_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_wos_day.Name;
                    td.RowDatas.Add(rtxt_wos_day.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    #endregion

                    #region BIT RECORD(钻头数据) Night
                    td = new TableDetail();
                    td.TableName = rgv_bitRecord_night.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_bitRecord_night.Rows.Count; i++)
                    {
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 10).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_bitRecord_night.Rows[i].Cells[1].Value == null ? "" : rgv_bitRecord_night.Rows[i].Cells[1].Value.ToString());
                        td.RowDatas.Add(rgv_bitRecord_night.Rows[i].Cells[1].Value == null ? "" : rgv_bitRecord_night.Rows[i].Cells[1].Value.ToString());
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中
                    #endregion

                    #region BIT RECORD(钻头数据) Day
                    td = new TableDetail();
                    td.TableName = rgv_bitRecord_day.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_bitRecord_day.Rows.Count; i++)
                    {
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 35).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_bitRecord_day.Rows[i].Cells[1].Value == null ? "" : rgv_bitRecord_day.Rows[i].Cells[1].Value.ToString());
                        td.RowDatas.Add(rgv_bitRecord_day.Rows[i].Cells[1].Value == null ? "" : rgv_bitRecord_day.Rows[i].Cells[1].Value.ToString());
                    }
                    report.TableDetails.Add(td);//将整张表存入模型中
                    #endregion

                    #region CUTTING STRUCTURE(钻头磨损分析) Night
                    td = new TableDetail();
                    td.TableName = rgv_cuttingStruc_night.Name;//将表的名字保存进去
                    string bit_night = "";
                    sheet.GetRow(24).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[0].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[0].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[0].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[0].Cells[1].Value.ToString());
                    sheet.GetRow(24).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[1].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[1].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[1].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[1].Cells[1].Value.ToString());
                    sheet.GetRow(24).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[2].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[2].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[2].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[2].Cells[1].Value.ToString());
                    sheet.GetRow(24).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[3].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[3].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[3].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[3].Cells[1].Value.ToString());
                    sheet.GetRow(26).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[4].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[4].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[4].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[4].Cells[1].Value.ToString());
                    sheet.GetRow(26).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[5].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[5].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[5].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[5].Cells[1].Value.ToString());
                    sheet.GetRow(26).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[6].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[6].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[6].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[6].Cells[1].Value.ToString());
                    sheet.GetRow(26).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_night.Rows[7].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[7].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_night.Rows[7].Cells[1].Value == null ? "" : rgv_cuttingStruc_night.Rows[7].Cells[1].Value.ToString());
                    td.RowDatas.Add(bit_night);
                    report.TableDetails.Add(td);
                    #endregion

                    #region CUTTING STRUCTURE(钻头磨损分析) Day
                    td = new TableDetail();
                    td.TableName = rgv_cuttingStruc_day.Name;//将表的名字保存进去
                    sheet.GetRow(49).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[0].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[0].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[0].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[0].Cells[1].Value.ToString());
                    sheet.GetRow(49).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[1].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[1].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[1].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[1].Cells[1].Value.ToString());
                    sheet.GetRow(49).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[2].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[2].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[2].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[2].Cells[1].Value.ToString());
                    sheet.GetRow(49).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[3].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[3].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[3].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[3].Cells[1].Value.ToString());
                    sheet.GetRow(51).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[4].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[4].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[4].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[4].Cells[1].Value.ToString());
                    sheet.GetRow(51).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[5].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[5].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[5].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[5].Cells[1].Value.ToString());
                    sheet.GetRow(51).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[6].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[6].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[6].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[6].Cells[1].Value.ToString());
                    sheet.GetRow(51).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_cuttingStruc_day.Rows[7].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[7].Cells[1].Value.ToString());
                    td.RowDatas.Add(rgv_cuttingStruc_day.Rows[7].Cells[1].Value == null ? "" : rgv_cuttingStruc_day.Rows[7].Cells[1].Value.ToString());
                    report.TableDetails.Add(td);
                    #endregion

                    #region MUD RECORD(泥浆记录) Night
                    td = new TableDetail();
                    td.TableName = rgv_mudRecord_night.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_mudRecord_night.Rows.Count; i++)
                    {
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 10).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudRecord_night.Rows[i].Cells[1].Value == null ? "" : rgv_mudRecord_night.Rows[i].Cells[1].Value.ToString());
                        td.RowDatas.Add(rgv_mudRecord_night.Rows[i].Cells[1].Value == null ? "" : rgv_mudRecord_night.Rows[i].Cells[1].Value.ToString());
                    }
                    report.TableDetails.Add(td);
                    #endregion

                    #region MUD RECORD(泥浆记录) Day
                    td = new TableDetail();
                    td.TableName = rgv_mudRecord_day.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_mudRecord_day.Rows.Count; i++)
                    {
                        //读取GrideView，并写入excel
                        sheet.GetRow(i + 35).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudRecord_day.Rows[i].Cells[1].Value == null ? "" : rgv_mudRecord_day.Rows[i].Cells[1].Value.ToString());
                        td.RowDatas.Add(rgv_mudRecord_day.Rows[i].Cells[1].Value == null ? "" : rgv_mudRecord_day.Rows[i].Cells[1].Value.ToString());
                    }
                    report.TableDetails.Add(td);
                    #endregion

                    #region MUD&CHEMICALS ADDED(添加剂记录) Night
                    td = new TableDetail();
                    td.TableName = rgv_mudchemicals_night.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_mudchemicals_night.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        if (i < 5)
                        {
                            sheet.GetRow(i + 22).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_night.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_night.Rows[i].Cells[0].Value.ToString());
                            sheet.GetRow(i + 22).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_night.Rows[i].Cells[1].Value == null ? "" : rgv_mudchemicals_night.Rows[i].Cells[1].Value.ToString());
                        }
                        else
                        {
                            sheet.GetRow(i + 17).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_night.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_night.Rows[i].Cells[0].Value.ToString());
                            sheet.GetRow(i + 17).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_night.Rows[i].Cells[1].Value == null ? "" : rgv_mudchemicals_night.Rows[i].Cells[1].Value.ToString());
                        }
                        data += rgv_mudchemicals_night.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_night.Rows[i].Cells[0].Value.ToString();
                        data += rgv_mudchemicals_night.Rows[i].Cells[1].Value == null ? "," : "," + rgv_mudchemicals_night.Rows[i].Cells[1].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion

                    #region MUD&CHEMICALS ADDED(添加剂记录) Day
                    td = new TableDetail();
                    td.TableName = rgv_mudchemicals_day.Name;//将表的名字保存进去
                    for (int i = 0; i < rgv_mudchemicals_day.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        if (i < 5)
                        {
                            sheet.GetRow(i + 47).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_day.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_day.Rows[i].Cells[0].Value.ToString());
                            sheet.GetRow(i + 47).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_day.Rows[i].Cells[1].Value == null ? "" : rgv_mudchemicals_day.Rows[i].Cells[1].Value.ToString());
                        }
                        else
                        {
                            sheet.GetRow(i + 42).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_day.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_day.Rows[i].Cells[0].Value.ToString());
                            sheet.GetRow(i + 42).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_mudchemicals_day.Rows[i].Cells[1].Value == null ? "" : rgv_mudchemicals_day.Rows[i].Cells[1].Value.ToString());
                        }
                        data += rgv_mudchemicals_day.Rows[i].Cells[0].Value == null ? "" : rgv_mudchemicals_day.Rows[i].Cells[0].Value.ToString();
                        data += rgv_mudchemicals_day.Rows[i].Cells[1].Value == null ? "," : "," + rgv_mudchemicals_day.Rows[i].Cells[1].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region remark
                    sheet.GetRow(28).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_remarks_night.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_remarks_night.Name;
                    td.RowDatas.Add(rtxt_remarks_night.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet.GetRow(53).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rtxt_remarks_day.Text);
                    td = new TableDetail();
                    td.TableName = rtxt_remarks_day.Name;
                    td.RowDatas.Add(rtxt_remarks_day.Text);
                    report.TableDetails.Add(td);//添加一条数据
                    #endregion
                    #endregion
                    #region 第二张
                    var sheet2 = workbook.GetSheetAt(1);
                    #region 表的顶部信息
                    sheet2.GetRow(1).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_fod.Text);
                    td = new TableDetail();
                    td.TableName = txt_fod.Name;
                    td.RowDatas.Add(txt_fod.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(1).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_county.Text);
                    td = new TableDetail();
                    td.TableName = txt_county.Name;
                    td.RowDatas.Add(txt_county.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(1).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_state.Text);
                    td = new TableDetail();
                    td.TableName = txt_state.Name;
                    td.RowDatas.Add(txt_state.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(1).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_wlrrn.Text);
                    td = new TableDetail();
                    td.TableName = txt_wlrrn.Name;
                    td.RowDatas.Add(txt_wlrrn.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(2).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_size.Text);
                    td = new TableDetail();
                    td.TableName = txt_size.Name;
                    td.RowDatas.Add(txt_size.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(2).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_lines.Text);
                    td = new TableDetail();
                    td.TableName = txt_lines.Name;
                    td.RowDatas.Add(txt_lines.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(2).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_lslipped.Text);
                    td = new TableDetail();
                    td.TableName = txt_lslipped.Name;
                    td.RowDatas.Add(txt_lslipped.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(3).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_loo.Text);
                    td = new TableDetail();
                    td.TableName = txt_loo.Name;
                    td.RowDatas.Add(txt_loo.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(3).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_present.Text);
                    td = new TableDetail();
                    td.TableName = txt_present.Name;
                    td.RowDatas.Add(txt_present.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(4).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_wotslc.Text);
                    td = new TableDetail();
                    td.TableName = txt_wotslc.Name;
                    td.RowDatas.Add(txt_wotslc.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet2.GetRow(5).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_cwot.Text);
                    td = new TableDetail();
                    td.TableName = txt_cwot.Name;
                    td.RowDatas.Add(txt_cwot.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    #endregion

                    #region DRILLING PARAMETERS钻井信息 Night
                    td = new TableDetail();
                    td.TableName = rmv_operation_night.Name;//将表的名字保存进去
                    for (int i = 0; i < rmv_operation_night.Rows.Count; i++)
                    {
                        string data = "";
                        for (int j = 0; j < rmv_operation_night.Rows[i].Cells.Count; j++)
                        {
                            if (j != 0)
                            {
                                sheet2.GetRow(i + 9).GetCell(j+2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rmv_operation_night.Rows[i].Cells[j].Value == null ? "" : this.rmv_operation_night.Rows[i].Cells[j].Value.ToString());
                                data += this.rmv_operation_night.Rows[i].Cells[j].Value == null ? "," : "," + this.rmv_operation_night.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                sheet2.GetRow(i + 9).GetCell(j+2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rmv_operation_night.Rows[i].Cells[j].Value == null ? "" : this.rmv_operation_night.Rows[i].Cells[j].Value.ToString());
                                data += this.rmv_operation_night.Rows[i].Cells[j].Value == null ? "" : "" + this.rmv_operation_night.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region DRILLING PARAMETERS钻井信息 Day
                    td = new TableDetail();
                    td.TableName = rmv_operation_day.Name;//将表的名字保存进去
                    for (int i = 0; i < rmv_operation_day.Rows.Count; i++)
                    {
                        string data = "";
                        for (int j = 0; j < rmv_operation_day.Rows[i].Cells.Count; j++)
                        {
                            if (j != 0)
                            {
                                sheet2.GetRow(i + 39).GetCell(j+2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rmv_operation_day.Rows[i].Cells[j].Value == null ? "" : this.rmv_operation_day.Rows[i].Cells[j].Value.ToString());
                                data += this.rmv_operation_day.Rows[i].Cells[j].Value == null ? "," : "," + this.rmv_operation_day.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                sheet2.GetRow(i + 39).GetCell(j+2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rmv_operation_day.Rows[i].Cells[j].Value == null ? "" : this.rmv_operation_day.Rows[i].Cells[j].Value.ToString());
                                data += this.rmv_operation_day.Rows[i].Cells[j].Value == null ? "" : "" + this.rmv_operation_day.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region 设置测斜记录 Night
                    td = new TableDetail();
                    td.TableName = rgv_drecord_night.Name;
                    for (int i = 0; i < rgv_drecord_night.Rows.Count; i++)
                    {
                        string data = "";
                        for (int j = 0; j < rgv_drecord_night.Rows[i].Cells.Count; j++)
                        {
                            sheet2.GetRow(i + 13).GetCell(j + 4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_drecord_night.Rows[i].Cells[j].Value == null ? "" : this.rgv_drecord_night.Rows[i].Cells[j].Value.ToString());
                            if (j == 0)
                            {
                                data += this.rgv_drecord_night.Rows[i].Cells[j].Value == null ? "" : this.rgv_drecord_night.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                data += this.rgv_drecord_night.Rows[i].Cells[j].Value == null ? "," : "," + this.rgv_drecord_night.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region 设置测斜记录 Day
                    td = new TableDetail();
                    td.TableName = rgv_drecord_day.Name;
                    for (int i = 0; i < rgv_drecord_day.Rows.Count; i++)
                    {
                        string data = "";
                        for (int j = 0; j < rgv_drecord_day.Rows[i].Cells.Count; j++)
                        {
                            sheet2.GetRow(i + 43).GetCell(j + 4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_drecord_day.Rows[i].Cells[j].Value == null ? "" : this.rgv_drecord_day.Rows[i].Cells[j].Value.ToString());
                            if (j == 0)
                            {
                                data += this.rgv_drecord_day.Rows[i].Cells[j].Value == null ? "" : this.rgv_drecord_day.Rows[i].Cells[j].Value.ToString();
                            }
                            else
                            {
                                data += this.rgv_drecord_day.Rows[i].Cells[j].Value == null ? "," : "," + this.rgv_drecord_day.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region 设置最后一层套管数据
                    td = new TableDetail();
                    td.TableName = rgv_laseCasing.Name;
                    for (int i = 0; i < this.rgv_laseCasing.Rows.Count; i++)
                    {
                        string data = "";
                        sheet2.GetRow(i + 3).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[0].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[0].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[0].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[0].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[1].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[1].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[1].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[1].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[2].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[2].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[2].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[2].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[3].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[3].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[3].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[3].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[4].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[4].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[4].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[4].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[5].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[5].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[5].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[5].Value.ToString();
                        sheet2.GetRow(i + 3).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_laseCasing.Rows[i].Cells[6].Value == null ? "" : this.rgv_laseCasing.Rows[i].Cells[6].Value.ToString());
                        data += this.rgv_laseCasing.Rows[i].Cells[6].Value == null ? "," : "," + this.rgv_laseCasing.Rows[i].Cells[6].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion

                    #region 设置作业内容详细资料 Night
                    td = new TableDetail();
                    td.TableName = rgv_deviation_night.Name;
                    for (int i = 0; i < rgv_deviation_night.Rows.Count; i++)
                    {
                        string data = "";
                        sheet2.GetRow(i + 17).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_night.Rows[i].Cells[0].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[0].Value.ToString());
                        data += this.rgv_deviation_night.Rows[i].Cells[0].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[0].Value.ToString();
                        sheet2.GetRow(i + 17).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_night.Rows[i].Cells[1].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[1].Value.ToString());
                        data += this.rgv_deviation_night.Rows[i].Cells[1].Value == null ? "," : "," + this.rgv_deviation_night.Rows[i].Cells[1].Value.ToString();
                        sheet2.GetRow(i + 17).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_night.Rows[i].Cells[2].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[2].Value.ToString());
                        data += this.rgv_deviation_night.Rows[i].Cells[2].Value == null ? "," : "," + this.rgv_deviation_night.Rows[i].Cells[2].Value.ToString();
                        sheet2.GetRow(i + 17).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_night.Rows[i].Cells[3].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[3].Value.ToString());
                        data += this.rgv_deviation_night.Rows[i].Cells[3].Value == null ? "," : "," + this.rgv_deviation_night.Rows[i].Cells[3].Value.ToString();
                        sheet2.GetRow(i + 17).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_night.Rows[i].Cells[4].Value == null ? "" : this.rgv_deviation_night.Rows[i].Cells[4].Value.ToString());
                        data += this.rgv_deviation_night.Rows[i].Cells[4].Value == null ? "," : "," + this.rgv_deviation_night.Rows[i].Cells[4].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #region 设置作业内容详细资料 Day
                    td = new TableDetail();
                    td.TableName = rgv_deviation_day.Name;
                    for (int i = 0; i < rgv_deviation_day.Rows.Count; i++)
                    {
                        string data = "";
                        sheet2.GetRow(i + 47).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_day.Rows[i].Cells[0].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[0].Value.ToString());
                        data += this.rgv_deviation_day.Rows[i].Cells[0].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[0].Value.ToString();
                        sheet2.GetRow(i + 47).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_day.Rows[i].Cells[1].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[1].Value.ToString());
                        data += this.rgv_deviation_day.Rows[i].Cells[1].Value == null ? "," : "," + this.rgv_deviation_day.Rows[i].Cells[1].Value.ToString();
                        sheet2.GetRow(i + 47).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_day.Rows[i].Cells[2].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[2].Value.ToString());
                        data += this.rgv_deviation_day.Rows[i].Cells[2].Value == null ? "," : "," + this.rgv_deviation_day.Rows[i].Cells[2].Value.ToString();
                        sheet2.GetRow(i + 47).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_day.Rows[i].Cells[3].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[3].Value.ToString());
                        data += this.rgv_deviation_day.Rows[i].Cells[3].Value == null ? "," : "," + this.rgv_deviation_day.Rows[i].Cells[3].Value.ToString();
                        sheet2.GetRow(i + 47).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(this.rgv_deviation_day.Rows[i].Cells[4].Value == null ? "" : this.rgv_deviation_day.Rows[i].Cells[4].Value.ToString());
                        data += this.rgv_deviation_day.Rows[i].Cells[4].Value == null ? "," : "," + this.rgv_deviation_day.Rows[i].Cells[4].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);
                    #endregion
                    #endregion
                    #region 第三张报表
                    ISheet sheet3 = workbook.GetSheetAt(2);

                    #region 顶部信息
                    sheet3.GetRow(1).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_dcpd.Text);
                    td = new TableDetail();
                    td.TableName = txt_dcpd.Name;
                    td.RowDatas.Add(txt_dcpd.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet3.GetRow(2).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rdtp_date.Value.ToString("yyyy-MM-dd"));
                    sheet3.GetRow(3).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_wnan.Text);
                    td = new TableDetail();
                    td.TableName = txt_wnan.Name;
                    td.RowDatas.Add(txt_wnan.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet3.GetRow(4).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_company.Text);
                    td = new TableDetail();
                    td.TableName = txt_company.Name;
                    td.RowDatas.Add(txt_company.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet3.GetRow(5).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_tool.Text);
                    td = new TableDetail();
                    td.TableName = txt_tool.Name;
                    td.RowDatas.Add(txt_tool.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    sheet3.GetRow(5).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_rigNo2.Text);
                    td = new TableDetail();
                    td.TableName = txt_rigNo2.Name;
                    td.RowDatas.Add(txt_rigNo2.Text);
                    report.TableDetails.Add(td);//添加一条数据

                    #endregion

                    #region WELL RIG INFORMATION
                    td = new TableDetail();
                    td.TableName = rgv_drillCrew_night.Name;
                    for (int i = 0; i < rgv_drillCrew_night.Rows.Count; i++)
                    {
                        string data = "";
                        //读取GrideView，并写入excel
                        sheet3.GetRow(i + 8).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[0].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[0].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[0].Value == null ? "" : ""+rgv_drillCrew_night.Rows[i].Cells[0].Value.ToString();
                        sheet3.GetRow(i + 8).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[1].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[1].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[1].Value == null ? "," : "," + rgv_drillCrew_night.Rows[i].Cells[1].Value.ToString();
                        sheet3.GetRow(i + 8).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[2].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[2].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[2].Value == null ? "," : "," + rgv_drillCrew_night.Rows[i].Cells[2].Value.ToString();
                        sheet3.GetRow(i + 8).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[3].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[3].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[3].Value == null ? "," : "," + rgv_drillCrew_night.Rows[i].Cells[3].Value.ToString();
                        sheet3.GetRow(i + 8).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[4].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[4].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[4].Value == null ? "," : "," + rgv_drillCrew_night.Rows[i].Cells[4].Value.ToString();
                        sheet3.GetRow(i + 8).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_night.Rows[i].Cells[5].Value == null ? "" : rgv_drillCrew_night.Rows[i].Cells[5].Value.ToString());
                        data += rgv_drillCrew_night.Rows[i].Cells[5].Value == null ? "," : "," + rgv_drillCrew_night.Rows[i].Cells[5].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//添加一条数据

                    td = new TableDetail();
                    td.TableName = rgv_drillCrew_day.Name;
                    for (int i = 0; i < rgv_drillCrew_day.Rows.Count; i++)
                    {
                        string data = "";
                        sheet3.GetRow(i + 36).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[0].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[0].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[0].Value == null ? "" : "" + rgv_drillCrew_day.Rows[i].Cells[0].Value.ToString();
                        sheet3.GetRow(i + 36).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[1].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[1].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[1].Value == null ? "," : "," + rgv_drillCrew_day.Rows[i].Cells[1].Value.ToString();
                        sheet3.GetRow(i + 36).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[2].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[2].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[2].Value == null ? "," : "," + rgv_drillCrew_day.Rows[i].Cells[2].Value.ToString();
                        sheet3.GetRow(i + 36).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[3].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[3].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[3].Value == null ? "," : "," + rgv_drillCrew_day.Rows[i].Cells[3].Value.ToString();
                        sheet3.GetRow(i + 36).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[4].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[4].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[4].Value == null ? "," : "," + rgv_drillCrew_day.Rows[i].Cells[4].Value.ToString();
                        sheet3.GetRow(i + 36).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(rgv_drillCrew_day.Rows[i].Cells[5].Value == null ? "" : rgv_drillCrew_day.Rows[i].Cells[5].Value.ToString());
                        data += rgv_drillCrew_day.Rows[i].Cells[5].Value == null ? "," : "," + rgv_drillCrew_day.Rows[i].Cells[5].Value.ToString();
                        td.RowDatas.Add(data);
                    }
                    report.TableDetails.Add(td);//添加一条数据
                    #endregion

                    #region 底部信息
                    sheet3.GetRow(33).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_accident_night.Text);
                    td = new TableDetail();
                    td.TableName = txt_accident_night.Name;
                    td.RowDatas.Add(txt_accident_night.Text);
                    report.TableDetails.Add(td);//添加一条数据
                    sheet3.GetRow(61).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(txt_accident_day.Text);
                    td = new TableDetail();
                    td.TableName = txt_accident_day.Name;
                    td.RowDatas.Add(txt_accident_day.Text);
                    report.TableDetails.Add(td);//添加一条数据
                    #endregion
                    #endregion
                    try
                    {
                        report.DrillId = AppDrill.DrillID;
                        report.UserName = AppDrill.username;
                        ReportData rd = ReportData.Where(o => o.Date == rdtp_date.Value.ToString("yyyy-MM-dd")).FirstOrDefault();
                        if (rd == null)
                        {
                            rd = new ReportData();
                            rd.DrillId = AppDrill.DrillID;
                            rd.Date = rdtp_date.Value.ToString("yyyy-MM-dd");
                            rd.UserName = AppDrill.username;
                            rd.dataMakePGM = "AddReportData";
                            rd.dataMakeTime = DateTime.Now;
                            rd.dataMakeUser = AppDrill.username;
                            rd.dataUpdPGM = "AddReportData";
                            rd.dataUpdTime = DateTime.Now;
                            rd.dataUpdUser = AppDrill.username;
                            rd.JsonData = new JavaScriptSerializer().Serialize(report);//序列化为json格式存入数据库
                            db.ReportData.Add(rd);
                            ReportData.Add(rd);
                        }
                        else
                        {
                            rd.JsonData = new JavaScriptSerializer().Serialize(report);
                            rd.dataUpdPGM = "AddReportData";
                            rd.dataUpdTime = DateTime.Now;
                            rd.dataUpdUser = AppDrill.username;
                        }
                        db.SaveChanges();
                        if(isSave)
                            MessageBox.Show(message_list[9]);
                    }
                    catch
                    {
                        MessageBox.Show(message_list[10]);
                    }
                }
            }
            catch { }
            #endregion
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                rbtn_save.Enabled = true ;
                radButton1.Enabled = true;
                if (isSave)
                {
                    return;//如果是保存操作，直接return
                }
                //路径选择
                SaveFileDialog path = new SaveFileDialog();
                path.Filter = "(*.xls)|*.xls";
                //非自动导出才弹出
                if (!isAuto1)
                {
                    path.FileName = "DAILY DRILLING REPORT" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    if (path.ShowDialog() == DialogResult.OK)
                    {
                        //生成文件流
                        FileStream file = new FileStream(path.FileName, FileMode.Create);
                        if (file != null)
                            workbook.Write(file);
                        file.Close();
                    }
                }
                else
                {
                    path.FileName = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\DAILY DRILLING REPORT" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                    //生成文件流
                    FileStream file = new FileStream(path.FileName, FileMode.Create);
                    if (file != null)
                        workbook.Write(file);
                    file.Close();
                }
                //释放Excel进程
                Process[] xlProcess = Process.GetProcessesByName("Excel");
                if (xlProcess == null)
                {
                    foreach (Process tProcess in xlProcess)
                    {
                        //用kill方法杀死进程
                        tProcess.Kill();
                    }
                }
            }
            catch { }
            backgroundWorker2.CancelAsync();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dNow = DateTime.Now;
            #region 晚上导出一次
            DateTime evening = new DateTime(dNow.Year, dNow.Month, dNow.Day, 5, 58, 0);//11点59分开始处理数据
            //提前1分钟刷新钻井活动
            if (dNow >= evening.AddMinutes(-1) && dNow <= evening)
            {
                backgroundWorker3.RunWorkerAsync();//刷新钻井活动
            }
            if (dNow >= evening && dNow <= evening.AddMinutes(5) && !isAutoExport)
            {
                try
                {
                    isAuto1 = true;
                    backgroundWorker2.RunWorkerAsync();
                    isAutoExport = true;
                }
                catch { }
            }
            else if (dNow < evening || dNow > evening.AddMinutes(5))
            {
                isAutoExport = false;
            }
            #endregion
        }

        private void rbtn_save_Click(object sender, EventArgs e)
        {
            try
            {
                isSave = true;
                rbtn_save.Enabled = false;
                backgroundWorker2.RunWorkerAsync();
            }
            catch { }
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //查询需要初始化的报表数据
                ReportData = db.ReportData.Where(o => o.DrillId == AppDrill.DrillID && o.UserName == AppDrill.username).ToList();
                report1 = new JavaScriptSerializer().Deserialize<Report>(ReportData.Where(o => o.Date == rdtp_date.Value.ToString("yyyy-MM-dd")).FirstOrDefault().JsonData);
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            rbtn_refresh.Enabled = true;
            try
            {
                SetGridViewData();
            }
            catch { }
            backgroundWorker3.CancelAsync();
        }

        private void rmv_operation_night_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //当单元格编辑结束时
            if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
            {
                string start = this.rmv_operation_night.Rows[e.RowIndex].Cells[0].Value == null ? "" : this.rmv_operation_night.Rows[e.RowIndex].Cells[0].Value.ToString();
                string end = this.rmv_operation_night.Rows[e.RowIndex].Cells[1].Value == null ? "" : this.rmv_operation_night.Rows[e.RowIndex].Cells[1].Value.ToString();
                try
                {
                    double from=0, to=0;
                    if (!string.IsNullOrEmpty(start))
                    {
                        from = double.Parse(start);//开始井深
                    }
                    if (!string.IsNullOrEmpty(end))
                    {
                        to = double.Parse(end);//结束井深
                    }
                    //如果存在有一个为空则取消比较
                    if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                    {
                        return;
                    }
                    //如果开始井深大于结束井深
                    if (from > to)
                    {
                        MessageBox.Show(message_list[4]);//格式不正确
                        this.rmv_operation_night.Rows[e.RowIndex].Cells[1].Value = "";//设置为""
                    }
                    //开始井绳和结束井深相差1000米
                    if ((to-from)>1000)
                    {
                        MessageBox.Show(message_list[5]);//范围不正确
                        this.rmv_operation_night.Rows[e.RowIndex].Cells[1].Value = "";//设置为""
                    }
                }
                catch
                {
                    MessageBox.Show(message_list[3]);//格式不正确
                    this.rmv_operation_night.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";//设置为""
                }
            }
        }
        /// <summary>
        /// 统计时效班次总时间
        /// </summary>
        /// <param name="shift"></param>
        private void SetSXTime(int shift)
        {
            double value = 0;
            //循环时效表第二列
            for (int i = 0; i < rgt_timeDistribution.Rows.Count; i++)
            {
                try
                {
                    value += double.Parse(rgt_timeDistribution.Rows[i].Cells[shift].Value.ToString());
                }
                catch { }
            }
            //循环完井信息第二列
            for (int i = 0; i < rgc_completion.Rows.Count-1; i++)
            {
                try
                {
                    value += double.Parse(rgc_completion.Rows[i].Cells[shift].Value.ToString());
                }
                catch { }
            }
            this.rgc_completion.Rows[rgc_completion.Rows.Count - 1].Cells[shift].Value = value;
        }


        private void rtxt_stands_night_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Night();
        }

        private void rtxt_singles_night_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Night();
        }

        private void rtxt_kelly_night_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Night();
        }
        /// <summary>
        /// 统计晚班钻具组件信息
        /// </summary>
        private void TotalDA_Night()
        {
            double value = 0;
            for (int i = 0; i < rgda_drillAss_night.Rows.Count; i++)
            {
                try
                {
                    value += double.Parse(rgda_drillAss_night.Rows[i].Cells[2].Value.ToString());
                }
                catch { }
            }
            try
            {
                value += double.Parse(rtxt_stands_night.Text);
            }
            catch { }
            try
            {
                value += double.Parse(rtxt_singles_night.Text);
            }
            catch { }
            try
            {
                value += double.Parse(rtxt_kelly_night.Text);
            }
            catch { }
            rtxt_total_night.Text = value.ToString();
        }


        private void rtxt_stands_day_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Day();
        }

        private void rtxt_singles_day_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Day();
        }

        private void rtxt_kelly_day_TextChanged(object sender, EventArgs e)
        {
            TotalDA_Day();
        }
        /// <summary>
        /// 统计早班钻具组件信息
        /// </summary>
        private void TotalDA_Day()
        {
            double value = 0;
            for (int i = 0; i < rgda_drillAss_day.Rows.Count; i++)
            {
                try
                {
                    value += double.Parse(rgda_drillAss_day.Rows[i].Cells[2].Value.ToString());
                }
                catch { }
            }
            try
            {
                value += double.Parse(rtxt_stands_day.Text);
            }
            catch { }
            try
            {
                value += double.Parse(rtxt_singles_day.Text);
            }
            catch { }
            try
            {
                value += double.Parse(rtxt_kelly_day.Text);
            }
            catch { }
            rtxt_total_day.Text = value.ToString();
        }

        private void rgds_daywork_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1||e.ColumnIndex==2)
            {
                double value = 0;
                for (int i = 0; i < this.rgds_daywork.Rows.Count-1; i++)
                {
                    try
                    {
                        value += double.Parse(this.rgds_daywork.Rows[i].Cells[e.ColumnIndex].Value.ToString());
                    }
                    catch { }
                }
                this.rgds_daywork.Rows[this.rgds_daywork.Rows.Count - 1].Cells[e.ColumnIndex].Value = value;
            }
        }

        private void btn_operation_night_Click(object sender, EventArgs e)
        {
            btn_operation_night.Enabled = false;
            backgroundWorker4.WorkerSupportsCancellation = true;
            if (!backgroundWorker4.IsBusy)
            {
                backgroundWorker4.RunWorkerAsync();
            }
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < rmv_operation_night.Rows.Count; i++)
            {
                var startCell = this.rmv_operation_night.Rows[i].Cells[0].Value;
                var endCell = this.rmv_operation_night.Rows[i].Cells[1].Value;
                if (startCell == null || endCell == null)
                {
                    // clearQueryTable(table_line);
                    continue;
                }
                var startDate = startCell.ToString();
                var endDate = endCell.ToString();
                if (startDate == "" || endDate == "")
                {
                    //  clearQueryTable(table_line);
                    continue;
                }
                //泵冲模型
                InputTagAS queryModel = new InputTagAS();
                //获取开始日期的时间戳
                queryModel.from = double.Parse(startDate);
                //获取结束日期的时间戳
                queryModel.to = double.Parse(endDate);
                //获取井号ID
                queryModel.DrillId = AppDrill.DrillID;

                //如果 井号 和 开始时间 结束时间 Tag都有数据 并且 结束时间 大于 开始时间 的话 开始从数据库里找数据并输出到表格
                if (queryModel.from < queryModel.to && queryModel.to > 0)
                {
                    List<string> SizeTags = new List<string>();  //需要求泵衬值的测点
                    List<string> AverageTags = new List<string>();//需要求平均值的测点
                    List<string> SumTags = new List<string>();      //需要求和的测点
                    List<string> OtherAvgTag = new List<string>();//需要单独求平均的测点

                    //添加泵衬值的测点
                    SizeTags.Add("var341");
                    SizeTags.Add("var342");
                    SizeTags.Add("var343");
                    SizeTags.Add("var344");

                    //添加求平均值的测点
                    AverageTags.Add("var59");//泵冲1
                    AverageTags.Add("var60");//泵冲2
                    AverageTags.Add("var61");//泵冲3
                    AverageTags.Add("var62");//泵冲4

                    //添加需要求和的测点
                    SumTags.Add("var59");
                    SumTags.Add("var60");
                    SumTags.Add("var61");
                    SumTags.Add("var62");

                    //需要单独求平均的测点
                    OtherAvgTag.Add("var16");
                    OtherAvgTag.Add("var6");
                    OtherAvgTag.Add("var58");

                    queryModel.AverageTags = AverageTags;
                    queryModel.SumTags = SumTags;
                    queryModel.SizeTags = SizeTags;
                    queryModel.OtherAverageTags = OtherAvgTag;

                    OutTagAS queryResultModel = GetTagData(queryModel);

                    try
                    {
                        //查询需要查询的前三列数据
                        for (int _col = 0; _col < queryResultModel.OtherAverages.Count; _col++)
                        {
                            string strLineCell = queryResultModel.OtherAverages[_col].ToString();
                            //写入excel
                            this.rmv_operation_night.Rows[i].Cells[5 + _col].Value = strLineCell;
                        }
                        //查询泵冲
                        for (int _col = 0; _col < queryResultModel.Averages.Count*2; _col += 2)
                        {
                            this.rmv_operation_night.Rows[i].Cells[9 + _col].Value = queryResultModel.Averages[_col / 2];
                        }
                        //查询泵衬
                        for (int _col = 0; _col < queryResultModel.Size.Count*2; _col += 2)
                        {
                            this.rmv_operation_night.Rows[i].Cells[8 + _col].Value = queryResultModel.Averages[_col / 2];
                        }
                        //求最后合计值的那一列 写入excel
                        var sums = queryResultModel.Sums.Sum().ToString();
                        this.rmv_operation_night.Rows[i].Cells[16].Value = sums;
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_operation_night.Enabled = true;
            backgroundWorker4.CancelAsync();
        }

        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < rmv_operation_night.Rows.Count; i++)
            {
                var startCell = this.rmv_operation_day.Rows[i].Cells[0].Value;
                var endCell = this.rmv_operation_day.Rows[i].Cells[1].Value;
                if (startCell == null || endCell == null)
                {
                    // clearQueryTable(table_line);
                    continue;
                }
                var startDate = startCell.ToString();
                var endDate = endCell.ToString();
                if (startDate == "" || endDate == "")
                {
                    //  clearQueryTable(table_line);
                    continue;
                }
                //泵冲模型
                InputTagAS queryModel = new InputTagAS();
                //获取开始日期的时间戳
                queryModel.from = double.Parse(startDate);
                //获取结束日期的时间戳
                queryModel.to = double.Parse(endDate);
                //获取井号ID
                queryModel.DrillId = AppDrill.DrillID;

                //如果 井号 和 开始时间 结束时间 Tag都有数据 并且 结束时间 大于 开始时间 的话 开始从数据库里找数据并输出到表格
                if (queryModel.from < queryModel.to && queryModel.to > 0)
                {
                    List<string> SizeTags = new List<string>();  //需要求泵衬值的测点
                    List<string> AverageTags = new List<string>();//需要求平均值的测点
                    List<string> SumTags = new List<string>();      //需要求和的测点
                    List<string> OtherAvgTag = new List<string>();//需要单独求平均的测点

                    //添加泵衬值的测点
                    SizeTags.Add("var341");
                    SizeTags.Add("var342");
                    SizeTags.Add("var343");
                    SizeTags.Add("var344");

                    //添加求平均值的测点
                    AverageTags.Add("var59");//泵冲1
                    AverageTags.Add("var60");//泵冲2
                    AverageTags.Add("var61");//泵冲3
                    AverageTags.Add("var62");//泵冲4

                    //添加需要求和的测点
                    SumTags.Add("var59");
                    SumTags.Add("var60");
                    SumTags.Add("var61");
                    SumTags.Add("var62");

                    //需要单独求平均的测点
                    OtherAvgTag.Add("var16");
                    OtherAvgTag.Add("var6");
                    OtherAvgTag.Add("var58");

                    queryModel.AverageTags = AverageTags;
                    queryModel.SumTags = SumTags;
                    queryModel.SizeTags = SizeTags;
                    queryModel.OtherAverageTags = OtherAvgTag;

                    OutTagAS queryResultModel = GetTagData(queryModel);

                    try
                    {
                        //查询需要查询的前三列数据
                        for (int _col = 0; _col < queryResultModel.OtherAverages.Count; _col++)
                        {
                            string strLineCell = queryResultModel.OtherAverages[_col].ToString();
                            //写入excel
                            this.rmv_operation_day.Rows[i].Cells[5 + _col].Value = strLineCell;
                        }
                        //查询泵冲
                        for (int _col = 0; _col < queryResultModel.Averages.Count * 2; _col += 2)
                        {
                            this.rmv_operation_day.Rows[i].Cells[9 + _col].Value = queryResultModel.Averages[_col / 2];
                        }
                        //查询泵衬
                        for (int _col = 0; _col < queryResultModel.Size.Count * 2; _col += 2)
                        {
                            this.rmv_operation_day.Rows[i].Cells[8 + _col].Value = queryResultModel.Averages[_col / 2];
                        }
                        //求最后合计值的那一列 写入excel
                        var sums = queryResultModel.Sums.Sum().ToString();
                        this.rmv_operation_day.Rows[i].Cells[16].Value = sums;
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_operation_day.Enabled = true;
            backgroundWorker5.CancelAsync();
        }

        private void btn_operation_day_Click(object sender, EventArgs e)
        {
            btn_operation_day.Enabled = false;
            backgroundWorker5.WorkerSupportsCancellation = true;
            if (!backgroundWorker5.IsBusy)
            {
                backgroundWorker5.RunWorkerAsync();
            }
        }
        //回填数据
        private void SetGridViewData()
        {
            try
            {
                txt_apiNumber.Text = report1.TableDetails.Where(o => o.TableName == txt_apiNumber.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_waterDepth.Text = report1.TableDetails.Where(o => o.TableName == txt_waterDepth.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_rigNo.Text = report1.TableDetails.Where(o => o.TableName == txt_rigNo.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_stands_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_stands_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_singles_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_singles_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_kelly_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_kelly_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_wos_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_wos_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_remarks_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_remarks_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_stands_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_stands_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_singles_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_singles_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_kelly_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_kelly_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_wos_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_wos_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_remarks_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_remarks_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_total_night.Text = report1.TableDetails.Where(o => o.TableName == rtxt_total_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                rtxt_total_day.Text = report1.TableDetails.Where(o => o.TableName == rtxt_total_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
            }
            catch { }
            try
            {
                txt_fod.Text = report1.TableDetails.Where(o => o.TableName == txt_fod.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_county.Text = report1.TableDetails.Where(o => o.TableName == txt_county.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_wlrrn.Text = report1.TableDetails.Where(o => o.TableName == txt_wlrrn.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_size.Text = report1.TableDetails.Where(o => o.TableName == txt_size.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_lines.Text = report1.TableDetails.Where(o => o.TableName == txt_lines.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_lslipped.Text = report1.TableDetails.Where(o => o.TableName == txt_lslipped.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_loo.Text = report1.TableDetails.Where(o => o.TableName == txt_loo.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_present.Text = report1.TableDetails.Where(o => o.TableName == txt_present.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_wotslc.Text = report1.TableDetails.Where(o => o.TableName == txt_wotslc.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_cwot.Text = report1.TableDetails.Where(o => o.TableName == txt_cwot.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_nodfs.Text = report1.TableDetails.Where(o => o.TableName == txt_nodfs.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_crh.Text = report1.TableDetails.Where(o => o.TableName == txt_crh.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_dmc.Text = report1.TableDetails.Where(o => o.TableName == txt_dmc.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_tmc.Text = report1.TableDetails.Where(o => o.TableName == txt_tmc.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
            }
            catch { }
            try
            {
                txt_dcpd.Text = report1.TableDetails.Where(o => o.TableName == txt_dcpd.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_rigNo2.Text = report1.TableDetails.Where(o => o.TableName == txt_rigNo2.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_accident_night.Text = report1.TableDetails.Where(o => o.TableName == txt_accident_night.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
                txt_accident_day.Text = report1.TableDetails.Where(o => o.TableName == txt_accident_day.Name).FirstOrDefault().RowDatas[0];//来自数据库最后一次保存数据
            }
            catch { }
            #region rgt_timeDistribution
            TableDetail td_rgt_timeDistribution = new TableDetail();
            if (report1 != null)
            {
                td_rgt_timeDistribution = report1.TableDetails.Find(o => o.TableName == rgt_timeDistribution.Name);//获取当前表格数据
            }
            for (int i = 0; i < rgt_timeDistribution.Rows.Count; i++)
            {
                try
                {
                    if (td_rgt_timeDistribution.TableName != null)
                    {
                        rgt_timeDistribution.Rows[i].Cells[1].Value = td_rgt_timeDistribution.RowDatas[i].Split(',')[0];//填入保存数据库的数据
                        rgt_timeDistribution.Rows[i].Cells[2].Value = td_rgt_timeDistribution.RowDatas[i].Split(',')[1];//填入保存数据库的数据
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_drillPipe
            TableDetail td_rgv_drillPipe = new TableDetail();
            if (report1 != null)
            {
                td_rgv_drillPipe = report1.TableDetails.Find(o => o.TableName == rgv_drillPipe.Name);//获取当前表格数据
            }
            for (int i = 0; i < rgv_drillPipe.Rows.Count; i++)
            {
                try
                {
                    if (td_rgv_drillPipe.TableName != null)
                    {
                        //循环添加每行的数据
                        string[] data = td_rgv_drillPipe.RowDatas[i].Split(',');//拆分
                        for (int j = 0; j < rgv_drillPipe.Rows[i].Cells.Count; j++)
                        {
                            rgv_drillPipe.Rows[i].Cells[j].Value = data[j];//单元格赋值
                        }
                    }
                }
                catch { }
            }
            #endregion

            #region  rgv_bitRecord
            //Night
            TableDetail td_rgv_bitRecord_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_bitRecord_night = report1.TableDetails.Find(o => o.TableName == rgv_bitRecord_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_bitRecord_night.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_bitRecord_night.TableName != null)
                    {
                        rgv_bitRecord_night.Rows[iLoop].Cells[1].Value = td_rgv_bitRecord_night.RowDatas[iLoop];//填入保存数据库的数据
                    }
                }
                catch { }
            }
            //Day
            TableDetail td_rgv_bitRecord_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_bitRecord_day = report1.TableDetails.Find(o => o.TableName == rgv_bitRecord_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_bitRecord_day.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_bitRecord_day.TableName != null)
                    {
                        rgv_bitRecord_day.Rows[iLoop].Cells[1].Value = td_rgv_bitRecord_day.RowDatas[iLoop];//填入保存数据库的数据
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_mudRecord
            //Night
            TableDetail td_rgv_mudRecord_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_mudRecord_night = report1.TableDetails.Find(o => o.TableName == rgv_mudRecord_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_mudRecord_night.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_mudRecord_night.TableName != null)
                    {
                        rgv_mudRecord_night.Rows[iLoop].Cells[1].Value = td_rgv_mudRecord_night.RowDatas[iLoop];//填入保存数据库的数据
                    }
                }
                catch { }
            }
            //Day
            TableDetail td_rgv_mudRecord_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_mudRecord_day = report1.TableDetails.Find(o => o.TableName == rgv_mudRecord_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_mudRecord_day.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_mudRecord_day.TableName != null)
                    {
                        rgv_mudRecord_day.Rows[iLoop].Cells[1].Value = td_rgv_mudRecord_day.RowDatas[iLoop];//填入保存数据库的数据
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_cuttingStruc
            //Night
            TableDetail td_rgv_cuttingStruc_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_cuttingStruc_night = report1.TableDetails.Find(o => o.TableName == rgv_cuttingStruc_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_cuttingStruc_night.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_cuttingStruc_night.TableName != null)
                    {
                        rgv_cuttingStruc_night.Rows[iLoop].Cells[1].Value = td_rgv_cuttingStruc_night.RowDatas[iLoop];
                    }
                }
                catch { }
            }
            //Day
            TableDetail td_rgv_cuttingStruc_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_cuttingStruc_day = report1.TableDetails.Find(o => o.TableName == rgv_cuttingStruc_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_cuttingStruc_day.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgv_cuttingStruc_day.TableName != null)
                    {
                        rgv_cuttingStruc_day.Rows[iLoop].Cells[1].Value = td_rgv_cuttingStruc_day.RowDatas[iLoop];
                    }
                }
                catch { }
            }
            #endregion

            #region rgc_completion
            TableDetail td_rgc_completion = new TableDetail();
            if (report1 != null)
            {
                td_rgc_completion = report1.TableDetails.Find(o => o.TableName == rgc_completion.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgc_completion.Rows.Count; iLoop++)
            {
                try
                {
                    if (td_rgc_completion.TableName != null)
                    {
                        rgc_completion.Rows[iLoop].Cells[1].Value = td_rgc_completion.RowDatas[iLoop].Split(',')[0];
                        rgc_completion.Rows[iLoop].Cells[2].Value = td_rgc_completion.RowDatas[iLoop].Split(',')[1];
                    }
                }
                catch { }
            }
            #endregion

            #region rgda_drillAss
            //Night
            TableDetail td_rgda_drillAss_night = new TableDetail();
            if (report1 != null)
            {
                td_rgda_drillAss_night = report1.TableDetails.Find(o => o.TableName == rgda_drillAss_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < 9; iLoop++)
            {
                try
                {
                    rgda_drillAss_night.Rows[iLoop].Cells[0].Value = td_rgda_drillAss_night.RowDatas[iLoop].Split(',')[0];
                    rgda_drillAss_night.Rows[iLoop].Cells[1].Value = td_rgda_drillAss_night.RowDatas[iLoop].Split(',')[1];
                    rgda_drillAss_night.Rows[iLoop].Cells[2].Value = td_rgda_drillAss_night.RowDatas[iLoop].Split(',')[2];
                }
                catch { }
            }
            //Day
            TableDetail td_rgda_drillAss_day = new TableDetail();
            if (report1 != null)
            {
                td_rgda_drillAss_day = report1.TableDetails.Find(o => o.TableName == rgda_drillAss_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < 9; iLoop++)
            {
                try
                {
                    rgda_drillAss_day.Rows[iLoop].Cells[0].Value = td_rgda_drillAss_day.RowDatas[iLoop].Split(',')[0];
                    rgda_drillAss_day.Rows[iLoop].Cells[1].Value = td_rgda_drillAss_day.RowDatas[iLoop].Split(',')[1];
                    rgda_drillAss_day.Rows[iLoop].Cells[2].Value = td_rgda_drillAss_day.RowDatas[iLoop].Split(',')[2];
                }
                catch { }
            }
            #endregion

            #region rgv_mudchemicals
            //Night
            TableDetail td_rgv_mudchemicals_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_mudchemicals_night = report1.TableDetails.Find(o => o.TableName == rgv_mudchemicals_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_mudchemicals_night.Rows.Count; iLoop++)
            {
                try
                {
                    rgv_mudchemicals_night.Rows[iLoop].Cells[0].Value = td_rgv_mudchemicals_night.RowDatas[iLoop].Split(',')[0];
                    rgv_mudchemicals_night.Rows[iLoop].Cells[1].Value = td_rgv_mudchemicals_night.RowDatas[iLoop].Split(',')[1];
                }
                catch { }
            }
            //Day
            TableDetail td_rgv_mudchemicals_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_mudchemicals_day = report1.TableDetails.Find(o => o.TableName == rgv_mudchemicals_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_mudchemicals_day.Rows.Count; iLoop++)
            {
                try
                {
                    rgv_mudchemicals_day.Rows[iLoop].Cells[0].Value = td_rgv_mudchemicals_day.RowDatas[iLoop].Split(',')[0];
                    rgv_mudchemicals_day.Rows[iLoop].Cells[1].Value = td_rgv_mudchemicals_day.RowDatas[iLoop].Split(',')[1];
                }
                catch { }
            }
            #endregion

            #region rgds_daywork
            TableDetail td_rgds_daywork = new TableDetail();
            if (report1 != null)
            {
                td_rgds_daywork = report1.TableDetails.Find(o => o.TableName == rgds_daywork.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgds_daywork.Rows.Count; iLoop++)
            {
                try
                {
                    rgds_daywork.Rows[iLoop].Cells[1].Value = td_rgds_daywork.RowDatas[iLoop].Split(',')[0];//读取数据
                    rgds_daywork.Rows[iLoop].Cells[2].Value = td_rgds_daywork.RowDatas[iLoop].Split(',')[1];//读取数据
                }
                catch { }
            }
            #endregion

            #region rgv_drillCrew
            TableDetail td_rgv_drillCrew_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_drillCrew_night = report1.TableDetails.Find(o => o.TableName == rgv_drillCrew_night.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_drillCrew_night.Rows.Count; iLoop++)
            {
                try
                {
                    rgv_drillCrew_night.Rows[iLoop].Cells[0].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[0];//数据通过','分割
                    rgv_drillCrew_night.Rows[iLoop].Cells[1].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[1];//数据通过','分割
                    rgv_drillCrew_night.Rows[iLoop].Cells[2].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[2];
                    rgv_drillCrew_night.Rows[iLoop].Cells[3].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[3];
                    rgv_drillCrew_night.Rows[iLoop].Cells[4].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[4];
                    rgv_drillCrew_night.Rows[iLoop].Cells[5].Value = td_rgv_drillCrew_night.RowDatas[iLoop].Split(',')[5];
                }
                catch { }
            }
            TableDetail td_rgv_drillCrew_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_drillCrew_day = report1.TableDetails.Find(o => o.TableName == rgv_drillCrew_day.Name);//获取当前表格数据
            }
            for (int iLoop = 0; iLoop < rgv_drillCrew_day.Rows.Count; iLoop++)
            {
                try
                {
                    rgv_drillCrew_day.Rows[iLoop].Cells[0].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[0];//数据通过'，'分割
                    rgv_drillCrew_day.Rows[iLoop].Cells[1].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[1];//数据通过'，'分割
                    rgv_drillCrew_day.Rows[iLoop].Cells[2].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[2];
                    rgv_drillCrew_day.Rows[iLoop].Cells[3].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[3];
                    rgv_drillCrew_day.Rows[iLoop].Cells[4].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[4];
                    rgv_drillCrew_day.Rows[iLoop].Cells[5].Value = td_rgv_drillCrew_day.RowDatas[iLoop].Split(',')[5];
                }
                catch { }
            }
            #endregion

            #region rmv_operation 早晚
            TableDetail td_operation_night = new TableDetail();
            if (report1 != null)
            {
                td_operation_night = report1.TableDetails.Find(o => o.TableName == rmv_operation_night.Name);//获取当前表格数据
            }
            for (int i = 0; i < rmv_operation_night.Rows.Count; i++)
            {
                try
                {
                    string[] data = td_operation_night.RowDatas[i].Split(',');
                    for (int j = 0; j < data.Length; j++)
                    {
                        rmv_operation_night.Rows[i].Cells[j].Value = data[j];
                    }
                }
                catch { }
            }

            TableDetail td_operation_day = new TableDetail();
            if (report1 != null)
            {
                td_operation_day = report1.TableDetails.Find(o => o.TableName == rmv_operation_day.Name);//获取当前表格数据
            }
            for (int i = 0; i < rmv_operation_day.Rows.Count; i++)
            {
                try
                {
                    string[] data = td_operation_day.RowDatas[i].Split(',');
                    for (int j = 0; j < data.Length; j++)
                    {
                        rmv_operation_day.Rows[i].Cells[j].Value = data[j];
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_drecord 早晚
            TableDetail td_rgv_drecord_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_drecord_night = report1.TableDetails.Find(o => o.TableName == rgv_drecord_night.Name);//获取当前表格数据
            }
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    if (td_rgv_drecord_night.TableName != null)
                    {
                        string[] data = td_rgv_drecord_night.RowDatas[i].Split(',');
                        for (int j = 0; j < rgv_drecord_night.Rows[i].Cells.Count; j++)
                        {
                            rgv_drecord_night.Rows[i].Cells[j].Value = data[j];
                        }
                    }
                }
                catch { }
            }
            TableDetail td_rgv_drecord_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_drecord_day = report1.TableDetails.Find(o => o.TableName == rgv_drecord_day.Name);//获取当前表格数据
            }
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    if (td_rgv_drecord_day.TableName != null)
                    {
                        string[] data = td_rgv_drecord_day.RowDatas[i].Split(',');
                        for (int j = 0; j < rgv_drecord_night.Rows[i].Cells.Count; j++)
                        {
                            rgv_drecord_day.Rows[i].Cells[j].Value = data[j];
                        }
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_laseCasing
            TableDetail td_rgv_laseCasing = new TableDetail();
            if (report1 != null)
            {
                td_rgv_laseCasing = report1.TableDetails.Find(o => o.TableName == rgv_laseCasing.Name);//获取当前表格数据
            }
            //最后一层套管数据
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (td_rgv_laseCasing.TableName != null)
                    {
                        string[] data = td_rgv_laseCasing.RowDatas[i].Split(',');
                        for (int j = 0; j < rgv_laseCasing.Rows[i].Cells.Count; j++)
                        {
                            rgv_laseCasing.Rows[i].Cells[j].Value = data[j];
                        }
                    }
                }
                catch { }
            }
            #endregion

            #region rgv_deviation 早晚
            TableDetail td_rgv_deviation_night = new TableDetail();
            if (report1 != null)
            {
                td_rgv_deviation_night = report1.TableDetails.Find(o => o.TableName == rgv_deviation_night.Name);//获取当前表格数据
            }
            for (int i = 0; i < 18; i++)
            {
                try
                {
                    if (td_rgv_deviation_night.TableName != null)
                    {
                        string[] data = td_rgv_deviation_night.RowDatas[i].Split(',');
                        for (int j = 0; j < rgv_deviation_night.Rows[i].Cells.Count; j++)
                        {
                            rgv_deviation_night.Rows[i].Cells[j].Value = data[j];
                        }
                    }
                }
                catch { }
            }
            TableDetail td_rgv_deviation_day = new TableDetail();
            if (report1 != null)
            {
                td_rgv_deviation_day = report1.TableDetails.Find(o => o.TableName == rgv_deviation_day.Name);//获取当前表格数据
            }
            for (int i = 0; i < 18; i++)
            {
                try
                {
                    if (td_rgv_deviation_day.TableName != null)
                    {
                        string[] data = td_rgv_deviation_day.RowDatas[i].Split(',');
                        for (int j = 0; j < rgv_deviation_day.Rows[i].Cells.Count; j++)
                        {
                            rgv_deviation_day.Rows[i].Cells[j].Value = data[j];
                        }
                    }
                }
                catch { }
            }
            #endregion
        }      

        /// <summary>
        /// 创建一个新的报表，清除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtn_create_Click(object sender, EventArgs e)
        {
            DateSelect ds = new DateSelect();
            ds.ReportData = ReportData;
            ds.message_list = message_list;
            ds.ShowDialog();
            if (ds.d_select != null)
            {
                rdtp_date.Value = ds.d_select;//设置日期
                #region 清除数据
                try
                {
                    txt_apiNumber.Text = "";
                    txt_waterDepth.Text = "";
                    txt_rigNo.Text = "";
                    rtxt_stands_night.Text = "";
                    rtxt_singles_night.Text = "";
                    rtxt_kelly_night.Text = "";
                    rtxt_wos_night.Text = "";
                    rtxt_remarks_night.Text = "";
                    rtxt_stands_day.Text = "";
                    rtxt_singles_day.Text = "";
                    rtxt_kelly_day.Text = "";
                    rtxt_wos_day.Text = "";
                    rtxt_remarks_day.Text = "";
                }
                catch { }
                try
                {
                    txt_fod.Text = "";
                    txt_county.Text = "";
                    txt_wlrrn.Text = "";
                    txt_size.Text = "";
                    txt_lines.Text = "";
                    txt_lslipped.Text = "";
                    txt_loo.Text = "";
                    txt_present.Text = "";
                    txt_wotslc.Text = "";
                    txt_cwot.Text = "";
                    txt_nodfs.Text = "";
                    txt_crh.Text = "";
                    txt_dmc.Text = "";
                    txt_tmc.Text = "";
                }
                catch { }
                try
                {
                    txt_dcpd.Text = "";
                    txt_rigNo2.Text = "";
                    txt_accident_night.Text = "";
                    txt_accident_day.Text = "";
                }
                catch { }

                rgt_timeDistribution.Rows.Clear();
                rgv_drillPipe.Rows.Clear();
                rgv_bitRecord_night.Rows.Clear();
                rgv_bitRecord_day.Rows.Clear();
                rgv_mudRecord_night.Rows.Clear();
                rgv_mudRecord_day.Rows.Clear();
                rgv_cuttingStruc_night.Rows.Clear();
                rgv_cuttingStruc_day.Rows.Clear();
                rgc_completion.Rows.Clear();
                rgda_drillAss_night.Rows.Clear();
                rgda_drillAss_day.Rows.Clear();
                rgv_mudchemicals_night.Rows.Clear();
                rgv_mudchemicals_day.Rows.Clear();
                rgds_daywork.Rows.Clear();
                rgv_drillCrew_night.Rows.Clear();
                rgv_drillCrew_day.Rows.Clear();
                rmv_operation_night.Rows.Clear();
                rmv_operation_day.Rows.Clear();
                rgv_drecord_night.Rows.Clear();
                rgv_drecord_day.Rows.Clear();
                rgv_laseCasing.Rows.Clear();
                rgv_deviation_night.Rows.Clear();
                rgv_deviation_day.Rows.Clear();
                //重新初始化
                InitRadGridTime();
                InitRadGridDrillPipe();
                InitRadGridBitRecord();
                InitRadGridMudRecord();
                IntiRadGridCuttingStructure();
                InitGridCompletion();
                IntiGridDrillAssembly();
                IntiGridDayworkSummary();
                InitDrillingParameters();
                IntiGridMCAdded();
                Report_Three_one();
                rtxt_total_night.Text = "";
                rtxt_total_day.Text = "";
                #endregion
            }
        }

        private void rbtn_delete_Click(object sender, EventArgs e)
        {
            //开启新的窗口选择需要删除的报表
            ReportSelect rs = new ReportSelect();
            rs.message_list = message_list;
            rs.Type = "delete";
            rs.ReportData = ReportData;
            rs.ShowDialog();
            if (rs.d_select != null)
            {
                try
                {
                    //执行删除操作
                    ReportData r = db.ReportData.Where(o => o.Date == rs.d_select).FirstOrDefault();
                    db.ReportData.Remove(r);
                    db.SaveChanges();
                    ReportData.Remove(r);//缓存中的也需要删除
                    MessageBox.Show(message_list[12]);
                }
                catch
                {
                    MessageBox.Show(message_list[13]);
                }
            }
        }

        private void rbtn_refresh_Click(object sender, EventArgs e)
        {
            //开启新的窗口选择需要删除的报表
            ReportSelect rs = new ReportSelect();
            rs.message_list = message_list;
            rs.Type = "load";
            rs.ReportData = ReportData;
            rs.ShowDialog();
            if (rs.d_select != null)
            {
                rdtp_date.Value = Convert.ToDateTime(rs.d_select);//设置日期
                //开始启动线程加载数据
                rbtn_refresh.Enabled = false;
                backgroundWorker3.RunWorkerAsync();
            }
        }
    }
}