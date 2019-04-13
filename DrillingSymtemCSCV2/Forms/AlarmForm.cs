using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Charting;
//using Telerik.QuickStart.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Xml;
using DrillingSymtemCSCV2.UserControls;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class AlarmForm : BaseForm
    {
        private DrillOSEntities _db;
        private List<AlarmHis> AlarmList;   //报警数据
        private List<AlarmHistoryTJ> AlarmTJList;   //报警统计数据
        private List<DrillTag> AlarmtagList;  //报警列表(Alarmflg=1)
        private List<JsonAlarm> JsonAlarmList;  //用户报警模型
        private Dictionary<int, JsonAlarm> TempAlarmList;   //缓存用户修改的结果
        int current_index; //当前选中的标签序号

        private bool GetDataList = false;   //获取数据是否成功
        private bool SaveStatus = false;    //保存数据是否成功
        private string strURL = "";
        private TagSimpleModel TagModel;
        private List<string> list_status = new List<string>();//存放status信息
        private List<string> list_rcv = new List<string>();//存放图表的信息
        private List<string> list_grv = new List<string>();//存放测点报警信息
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        private string save;
        public AlarmForm()
        {
            InitializeComponent();
        }

        private void AlarmForm_Load(object sender, EventArgs e)
        {
            
            try
            {

                //设置语言
                setControlLanguage();
                _db = new DrillOSEntities();
                //TagList = new List<DrillTag>();
                // AlarmList = new List<AlarmHistory>();
                //190409修改，alarm统计与用户绑定，用新的
                AlarmList = new List<AlarmHis>();
                AlarmTJList = new List<AlarmHistoryTJ>();
                AlarmtagList = new List<DrillTag>();
                TempAlarmList = new Dictionary<int, JsonAlarm>();
                JsonAlarmList = new List<JsonAlarm>();
                //报警Grid数据
                getAlarmDatas();
                //radChartView1.
                this.rcv_percent.View.Margin = new Padding(20);
                this.rcv_percent.Title = list_rcv[0];
                this.rcv_percent.ShowTitle = true;
                this.rcv_percent.ChartElement.TitlePosition = TitlePosition.Top;
                //radChartView2
                this.rcv_top.View.Margin = new Padding(20);
                this.rcv_top.Title = list_rcv[1];
                this.rcv_top.ShowTitle = true;
                this.rcv_top.ChartElement.TitlePosition = TitlePosition.Top;

                if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
                {
                    //   btn_Save.Enabled = false;
                    btn_Default.Enabled = false;
                }
                //不可多选
                lst_channel.SelectionMode = SelectionMode.One;
                //条目点击事件
                lst_channel.SelectedIndexChanged += new EventHandler(list_channel_SelectedIndexChanged);

                //设置输入框只能输入正负数字和小数点
                setTextBoxEvent();
            }
            catch { }
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion

            #region Save异步提交
            backgroundWorker2.DoWork += new DoWorkEventHandler(backgroundWorker2_DoWork);
            backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker2_RunWorkerCompleted);
            backgroundWorker2.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            #endregion

            #region 设置报警后，重新获取其他界面的dirlltag
            backgroundWorker3.DoWork += new DoWorkEventHandler(backgroundWorker3_DoWork);
            backgroundWorker3.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker3_RunWorkerCompleted);
            backgroundWorker3.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            #endregion
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_tagdir = _db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
                //TagList = _db.DrillTag.Where(o => o.DrillId == AppDrill.DrillID).ToList();
                // AlarmList = _db.AlarmHistory.Where(o => o.DrillId == AppDrill.DrillID).OrderByDescending(o => o.ID).Take(100).ToList();//报警

                //190409修改，显示当前报警(用户与井号为查找条件)
                AlarmList = _db.AlarmHis.Where(o => o.DrillID == m_iDrillID && o.UserName == AppDrill.username).OrderByDescending(o => o.Time).Take(100).ToList();//报警


                // AlarmTJList=_db.AlarmHistory.Where(o => o.DrillId == AppDrill.DrillID).GroupBy(p=>p.Tag)
                //    AlarmTJList = _db.AlarmHistoryTJ.Where(o => o.DrillId == AppDrill.DrillID).OrderByDescending(o => o.AlarmCount).Take(10).ToList();//报警统计
                GetDataList = true;

                //0710修改，报警设置跟用户绑定 
                var data = _db.UserTagRef.Where(o => o.Username == AppDrill.username).FirstOrDefault();
                if (data != null)
                {
                    if (data.JsonAlarm != null)
                    {
                        JsonAlarmList = new JavaScriptSerializer().Deserialize<List<JsonAlarm>>(data.JsonAlarm);
                        AppDrill.JsonAlarmList = JsonAlarmList;
                    }

                }
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (!GetDataList)    //获取数据失败
                {
                    this.lbl_Status.ForeColor = Color.Red;
                    this.lbl_Status.Text = list_status[2];
                }
                else    //获取数据成功
                {
                    //绑定tag数据
                    List<string> list_channel_data = new List<string>();
                    AlarmtagList = listTag.Where(o => o.AlarmFlag == 1).ToList();
                    foreach (var item in AlarmtagList)
                    {
                        list_channel_data.Add(Transformation(item.Tag));
                    }
                    lst_channel.DataSource = list_channel_data;
                    //绑定报警数据
                    List<AlarmData> radGridView1_data = new List<AlarmData>();
                    foreach (var item in AlarmList)
                    {
                        radGridView1_data.Add(new AlarmData(Transformation(item.Code), Transformation(item.Code,true), item.Time.ToString()));
                    }
                    this.gvw_RecentlyAlarms.DataSource = radGridView1_data;


                    //     this.gvw_RecentlyAlarms.MasterTemplate.Columns[0].Width = 100;

                    //绑定图表数据
                    List<KeyValuePair<double, object>> PieData = new List<KeyValuePair<double, object>>();
                    List<KeyValuePair<double, object>> BarData = new List<KeyValuePair<double, object>>();
                    //0704修改，修改报警统计的算法
                    //var TJalarm = _db.AlarmHistory.Where(o => o.DrillId == m_iDrillID).OrderByDescending(p => p.Count).Take(10);

                    //190409修改，报警统计的算法用code字段直接统计出来
                    var TJalarm = _db.AlarmHis.Where(o => o.DrillID == m_iDrillID && o.UserName == AppDrill.username).GroupBy(o => o.Code).Select(k => new { Code = k.Key, ItemCount = k.Count() }).OrderByDescending(c => c.ItemCount).Take(10);
                    if (TJalarm == null) return;
                    foreach (var item in TJalarm)
                    {
                        PieData.Add(new KeyValuePair<double, object>(item.ItemCount, Transformation(item.Code, true)));
                        BarData.Add(new KeyValuePair<double, object>(item.ItemCount, Transformation(item.Code, true)));
                    }
                    //加载图
                    InitializePie(PieData.Take(5).ToList());   //测试用，取前5条
                    InitializeBar(BarData);
                }
            }
            catch { }
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region Save异步提交
        //添加DoWork事件请求数据
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (TagModel != null)
                {
                    var data = new JavaScriptSerializer().Serialize(TagModel);
                    //读取配置文件
                    if (string.IsNullOrEmpty(strURL))
                    {
                        strURL = System.Configuration.ConfigurationManager.AppSettings["AlarmForm_Save"].ToString();
                    }

                    System.Net.HttpWebRequest request;
                    // 创建一个HTTP请求
                    request = (System.Net.HttpWebRequest)WebRequest.Create(strURL + data);
                    request.Method = "get";
                    System.Net.HttpWebResponse response;
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string responseText = myreader.ReadToEnd();

                    if (responseText.IndexOf("OK") != -1)
                    {
                        SaveStatus = true;
                    }
                    else
                    {
                        SaveStatus = false;
                    }

                    myreader.Close();

                }


            }
            catch
            {
            }
        }

        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (SaveStatus)
                {
                    this.lbl_Status.ForeColor = Color.Lime;
                    this.lbl_Status.Text = list_status[0];

                    if (TagModel != null)
                    {
                        var model = listTag.Where(o => o.Tag == TagModel.Tag && o.DrillId == 1).FirstOrDefault();
                        model.HValue = Math.Round(TagModel.HValue, 4);
                        model.LValue = Math.Round(TagModel.LValue, 4);
                    }

                    backgroundWorker3.RunWorkerAsync(); //为其他界面重新获取数据
                }
                else
                {
                    this.lbl_Status.ForeColor = Color.Red;
                    this.lbl_Status.Text = list_status[1];
                }

                btn_Save.Enabled = true;
                btn_Save.Text = save;
            }
            catch
            {
            }

            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 异步为其他界面重新获取drilltag
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //0703修改，drilltag只有一个数据源
                //  var drilltag = _db.DrillTag.Where(o => o.DrillId == AppDrill.DrillID).ToList();
                var drilltag = _db.DrillTag.Where(o => o.DrillId == 1).ToList();
                listTag = drilltag;
                //fourchart drilltag重新获取
                FourChart.listTag = drilltag;
                //datashowcontrol drilltag重新获取
                DataShowControl.listTag = drilltag;
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker3.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 设置输入框只能输入正负数字和小数点

        private void setTextBoxEvent()
        {
            this.txt_HighAlarm.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
            this.txt_LowAlarm.KeyPress += new KeyPressEventHandler(textBox_KeyPress);
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 45 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
            //输入为负号时，只能输入一次且只能输入一次
            if (e.KeyChar == 45 && (((TextBox)sender).SelectionStart != 0 || ((TextBox)sender).Text.IndexOf("-") >= 0))
                e.Handled = true;
            //输入为小数点时，只能输入一次且只能输入一次
            if (e.KeyChar == 46 && ((TextBox)sender).Text.IndexOf(".") >= 0)
                e.Handled = true;
        }

        #endregion

        /// <summary>
        /// list_channel 条目点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void list_channel_SelectedIndexChanged(object sender, EventArgs e)
        {

            //先判值赋给上一次的index
            if (TempAlarmList.ContainsKey(current_index))
            {
                TempAlarmList[current_index].H = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : double.Parse(txt_HighAlarm.Text);
                TempAlarmList[current_index].L = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : double.Parse(txt_LowAlarm.Text);
                TempAlarmList[current_index].HIsActive = chb_H.Checked;
                TempAlarmList[current_index].LIsActive = chb_L.Checked;
                TempAlarmList[current_index].Tag = AlarmtagList[current_index].Tag;

            }
            else if (!TempAlarmList.ContainsKey(current_index) && (lst_channel.SelectedIndex != current_index))
            {
                JsonAlarm tag = new JsonAlarm();
                tag.H = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : double.Parse(txt_HighAlarm.Text);
                tag.L = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : double.Parse(txt_LowAlarm.Text);
                tag.HIsActive = chb_H.Checked;
                tag.LIsActive = chb_L.Checked;
                tag.Tag = AlarmtagList[current_index].Tag;
                TempAlarmList.Add(current_index, tag);
            }

            int index = lst_channel.SelectedIndex;
            this.lbl_Status.Text = "";
            txt_HighAlarm.Text = "";
            txt_LowAlarm.Text = "";
            lbl_Tags.Text = "";
            lbl_TagId.Text = "";
            lbl_DrillId.Text = "";
            chb_H.Checked = false;
            chb_L.Checked = false;
            //0714修改，先从缓存判断有没有值
            if (TempAlarmList.ContainsKey(index))
            {
                txt_HighAlarm.Text = TempAlarmList[index].H.ToString();
                txt_LowAlarm.Text = TempAlarmList[index].L.ToString();
                lbl_TagId.Text = TempAlarmList[index].Tag;
                lbl_Tags.Text = Transformation(TempAlarmList[index].Tag);
                lbl_DrillId.Text = "1";
                chb_H.Checked = TempAlarmList[index].HIsActive;
                chb_L.Checked = TempAlarmList[index].LIsActive;

            }


            //0710修改，用JsonAlarm更新
            else if (JsonAlarmList.Count != 0)
            {
                JsonAlarm tag = JsonAlarmList.Where(o => o.Tag == AlarmtagList[lst_channel.SelectedIndex].Tag).FirstOrDefault();
                if (tag != null)
                {
                    txt_HighAlarm.Text = tag.H.ToString();
                    txt_LowAlarm.Text = tag.L.ToString();
                    lbl_TagId.Text = tag.Tag;
                    lbl_Tags.Text = Transformation(tag.Tag);
                    lbl_DrillId.Text = "1";
                    chb_H.Checked = tag.HIsActive;
                    chb_L.Checked = tag.LIsActive;
                }
                else
                {
                    DrillTag tag2 = AlarmtagList[lst_channel.SelectedIndex];
                    txt_HighAlarm.Text = tag2 == null ? "" : (tag2.HValue >= 99999 ? "" : tag2.HValue.ToString());
                    txt_LowAlarm.Text = tag2 == null ? "" : (tag2.LValue <= -99999 ? "" : tag2.LValue.ToString());
                    lbl_Tags.Text = tag2 == null ? "" : Transformation(tag2.Tag);
                    lbl_TagId.Text = tag2 == null ? "" : tag2.Tag;
                    lbl_DrillId.Text = tag2 == null ? "" : tag2.DrillId.ToString();
                    chb_H.Checked = tag2.HisActive == null ? false : (bool)tag2.HisActive;
                    chb_L.Checked = tag2.LisActive == null ? false : (bool)tag2.LisActive;
                }
            }
            else
            {
                if (AlarmtagList.Count != 0)
                {
                    DrillTag tag = AlarmtagList[lst_channel.SelectedIndex];
                    txt_HighAlarm.Text = tag == null ? "" : (tag.HValue >= 99999 ? "" : tag.HValue.ToString());
                    txt_LowAlarm.Text = tag == null ? "" : (tag.LValue <= -99999 ? "" : tag.LValue.ToString());
                    lbl_Tags.Text = tag == null ? "" : Transformation(tag.Tag);
                    lbl_TagId.Text = tag == null ? "" : tag.Tag;
                    lbl_DrillId.Text = tag == null ? "" : tag.DrillId.ToString();
                    chb_H.Checked = tag.HisActive == null ? false : (bool)tag.HisActive;
                    chb_L.Checked = tag.LisActive == null ? false : (bool)tag.LisActive;
                }

            }

            current_index = lst_channel.SelectedIndex;
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setJsonAlarm(JsonAlarm source_jsonAlarm, JsonAlarm target_jsonAlarm)
        {
            source_jsonAlarm.H = target_jsonAlarm.H;
            source_jsonAlarm.L = target_jsonAlarm.L;
            source_jsonAlarm.HIsActive = target_jsonAlarm.HIsActive;
            source_jsonAlarm.LIsActive = target_jsonAlarm.LIsActive;
            source_jsonAlarm.Tag = target_jsonAlarm.Tag;
        }

        private void save_Click(object sender, EventArgs e)
        {
            this.lbl_Status.Text = "";
            TagModel = new TagSimpleModel();
            TagModel.HValue = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : decimal.Parse(txt_HighAlarm.Text);
            TagModel.LValue = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : decimal.Parse(txt_LowAlarm.Text);

            if (TagModel.LValue >= TagModel.HValue)
            {
                this.lbl_Status.ForeColor = Color.Red;
                this.lbl_Status.Text = list_status[3];
                return;
            }

            //string Tag = list_tagdir.Where(o => o.TagShowName == lbl_TagId.Text).Select(o => o.Basic).FirstOrDefault();
            TagModel.Tag = string.IsNullOrEmpty(lbl_TagId.Text) ? "" : lbl_TagId.Text;
            TagModel.DrillId = string.IsNullOrEmpty(lbl_DrillId.Text) ? 0 : int.Parse(lbl_DrillId.Text);
            btn_Save.Enabled = false;
            btn_Save.Text = "Wait...";

            try
            {

                //if (AppDrill.username == "admin")
                //{
                //    backgroundWorker2.RunWorkerAsync(); //开始
                //}

                ////0711修改，将上下限报警是否启用保存到drilltag
                //_db.DrillTag.Where(o => o.DrillId == 1 && o.Tag == TagModel.Tag).FirstOrDefault().HisActive = chb_H.Checked;
                //_db.DrillTag.Where(o => o.DrillId == 1 && o.Tag == TagModel.Tag).FirstOrDefault().LisActive = chb_L.Checked;
                //0710保存到 jsonalarm
                //JsonAlarm model = new JsonAlarm();
                //model.H = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : double.Parse(txt_HighAlarm.Text);
                //model.L = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : double.Parse(txt_LowAlarm.Text);
                //model.HIsActive = chb_H.Checked;
                //model.LIsActive = chb_L.Checked;
                //model.Tag = AlarmtagList[lst_channel.SelectedIndex].Tag;

                //0714批量修改
                //因为templist只会在切换的时候添加，所以最后一条记录要先修改到templist
                if (TempAlarmList.ContainsKey(lst_channel.SelectedIndex))
                {

                    TempAlarmList[lst_channel.SelectedIndex].H = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : double.Parse(txt_HighAlarm.Text);
                    TempAlarmList[lst_channel.SelectedIndex].L = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : double.Parse(txt_LowAlarm.Text);
                    TempAlarmList[lst_channel.SelectedIndex].HIsActive = chb_H.Checked;
                    TempAlarmList[lst_channel.SelectedIndex].LIsActive = chb_L.Checked;
                    TempAlarmList[lst_channel.SelectedIndex].Tag = AlarmtagList[current_index].Tag;

                }
                else
                {
                    JsonAlarm tag = new JsonAlarm();
                    tag.H = string.IsNullOrEmpty(txt_HighAlarm.Text) ? 99999 : double.Parse(txt_HighAlarm.Text);
                    tag.L = string.IsNullOrEmpty(txt_LowAlarm.Text) ? -99999 : double.Parse(txt_LowAlarm.Text);
                    tag.HIsActive = chb_H.Checked;
                    tag.LIsActive = chb_L.Checked;
                    tag.Tag = AlarmtagList[current_index].Tag;
                    TempAlarmList.Add(current_index, tag);

                }

                if (TempAlarmList.Count != 0)
                {
                    foreach (JsonAlarm item in TempAlarmList.Values)
                    {
                        var data = JsonAlarmList.Where(o => o.Tag == item.Tag).FirstOrDefault();

                        if (data == null)
                        {
                            //  JsonAlarm model = new JsonAlarm();
                            //   setJsonAlarm(model);
                            JsonAlarmList.Add(item);
                        }
                        else
                        {
                            setJsonAlarm(data, item);
                        }
                    }
                }
                //0716修改，如果是管理员权限会修改报警测点的上下限，影响统计结果，取消http请求的方式
                if (AppDrill.username == "admin")
                {
                    if (TempAlarmList.Count != 0)
                    {
                        foreach (JsonAlarm item in TempAlarmList.Values)
                        {
                            var data = _db.DrillTag.Where(o => o.DrillId == 1 && o.Tag == item.Tag).FirstOrDefault();
                            if (data != null)
                            {
                                data.HisActive = item.HIsActive;
                                data.LisActive = item.LIsActive;
                                data.HValue = (decimal)item.H;
                                data.LValue = (decimal)item.L;

                            }

                        }
                    }

                }


                AppDrill.JsonAlarmList = JsonAlarmList;
                string json = new JavaScriptSerializer().Serialize(JsonAlarmList);
                _db.UserTagRef.Where(o => o.Username == AppDrill.username).FirstOrDefault().JsonAlarm = json;
                _db.SaveChanges();

                this.lbl_Status.ForeColor = Color.Lime;
                this.lbl_Status.Text = list_status[0];
                btn_Save.Enabled = true;
                btn_Save.Text = save;
            }
            catch (Exception)
            {
                this.lbl_Status.ForeColor = Color.Red;
                this.lbl_Status.Text = list_status[1];
                //    MessageBox.Show(AppDrill.message[5]);//请勿重复点击
            }
        }

        /// <summary>
        /// 设置默认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Default_Click(object sender, EventArgs e)
        {
            this.lbl_Status.Text = "";

            txt_HighAlarm.Text = "";
            txt_LowAlarm.Text = "";

            TagModel = new TagSimpleModel();
            TagModel.HValue = 99999;
            TagModel.LValue = -99999;
            TagModel.Tag = string.IsNullOrEmpty(lbl_TagId.Text) ? "" : lbl_TagId.Text;
            TagModel.DrillId = string.IsNullOrEmpty(lbl_DrillId.Text) ? 0 : int.Parse(lbl_DrillId.Text);
            btn_Save.Enabled = false;
            btn_Save.Text = "Wait...";
            try
            {
                backgroundWorker2.RunWorkerAsync(); //开始
            }
            catch (Exception)
            {
                MessageBox.Show(AppDrill.message[5]);//请勿重复点击
            }

        }

        /// <summary>
        /// 设置列表
        /// </summary>
        public void getAlarmDatas()
        {
            //基本属性
            this.gvw_RecentlyAlarms.BackColor = Color.FromArgb(45, 45, 48);
            this.gvw_RecentlyAlarms.ShowGroupPanel = false;
            this.gvw_RecentlyAlarms.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvw_RecentlyAlarms.MasterTemplate.AllowAddNewRow = false;
            this.gvw_RecentlyAlarms.ThemeName = visualStudio2012DarkTheme1.ThemeName;
            this.gvw_RecentlyAlarms.ReadOnly = true;
            //列设定初期化
            this.gvw_RecentlyAlarms.DataSource = null;
            //  this.gvw_RecentlyAlarms.TableElement.BeginUpdate();
            this.gvw_RecentlyAlarms.MasterTemplate.Columns.Clear();
            //列设定
            this.gvw_RecentlyAlarms.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_grv[0], "Channel"));
            this.gvw_RecentlyAlarms.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_grv[1], "Message"));
            this.gvw_RecentlyAlarms.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_grv[2], "Date"));
            this.gvw_RecentlyAlarms.MasterTemplate.Columns[0].Width = 100;
            this.gvw_RecentlyAlarms.MasterTemplate.Columns[1].Width = 100;
            this.gvw_RecentlyAlarms.MasterTemplate.Columns[2].Width = 220;


            //this.radGridView1.BorderStyle = BorderStyle.Fixed3D;
            //this.radGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            //this.radGridView1.GridColor = Color.FromArgb(45, 45, 48);

            //DataGridViewCellStyle style = new DataGridViewCellStyle();
            //style.ForeColor = Color.White;
            //style.BackColor = Color.FromArgb(45, 45, 48);
            //foreach (DataGridViewColumn col in this.radGridView1.Columns)
            //{
            //    col.HeaderCell.Style = style;
            //}
            //this.radGridView1.EnableHeadersVisualStyles = false;

            //for (int i = 0; i < 20; i++)
            //{
            //    int index = radGridView1.Rows.Add();
            //    radGridView1.Rows[index].Cells[0].Value = "Ann Pressure";
            //    radGridView1.Rows[index].Cells[1].Value = "High High Value has happened";
            //    radGridView1.Rows[index].Cells[2].Value = "2017-03-18 09:12:46:20";
            //    //背景色
            //    radGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.White;
            //    radGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            //}
        }

        #region 饼图和柱状图

        /// <summary>
        /// 初始化pie
        /// </summary>
        private void InitializePie(List<KeyValuePair<double, object>> data)
        {
            this.rcv_percent.AreaType = ChartAreaType.Pie;
            this.rcv_percent.ChartElement.View.ForeColor = Color.White;

            PieSeries pie = new PieSeries();
            pie.ValueMember = "Value";
            pie.LabelMode = PieLabelModes.Horizontal;
            pie.ShowLabels = true;
            pie.ForeColor = Color.White;

            if (data.Count == 0)
                data.Add(new KeyValuePair<double, object>(0, ""));
            foreach (KeyValuePair<double, object> dataItem in data)
            {
                PieDataPoint point = new PieDataPoint(dataItem.Key, dataItem.Value.ToString());
                point.Label = Transformation(dataItem.Value.ToString()) + "(" + dataItem.Key.ToString() + ")";
                pie.DataPoints.Add(point);
            }
            this.rcv_percent.Series.Add(pie);
            this.rcv_percent.ShowLegend = false;
        }

        /// <summary>
        /// 初始化Bar
        /// </summary>
        private void InitializeBar(List<KeyValuePair<double, object>> data)
        {
            this.rcv_top.AreaType = ChartAreaType.Cartesian;
            this.rcv_top.ShowGrid = true;
            this.rcv_top.ChartElement.View.ForeColor = Color.White;
            if (data.Count == 0)
                data.Add(new KeyValuePair<double, object>(0, ""));
            foreach (KeyValuePair<double, object> dataItem in data)
            {
                BarSeries bar = new BarSeries();
                //bar.ShowLabels = true;
                bar.LegendTitle = Transformation(dataItem.Value.ToString());
                bar.ForeColor = Color.White;

                CategoricalDataPoint point = new CategoricalDataPoint(dataItem.Key, dataItem.Value);
                point.Label = string.Format("{0:P2} - {1}", point.Value / 100, point.Category);
                bar.DataPoints.Add(point);

                this.rcv_top.Series.Add(bar);
            }
            this.rcv_top.Axes[0].LabelFitMode = AxisLabelFitMode.MultiLine;
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
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "rpnl_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu" || c.Name == "gbx_AlarmSetting")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            if (xe2.GetAttribute("key") == "lbl_Status")
                                            {
                                                XmlNodeList xn_list3 = node2.ChildNodes;
                                                foreach (XmlNode node3 in xn_list3)
                                                {
                                                    XmlElement xe3 = (XmlElement)node3;
                                                    list_status.Add(xe3.GetAttribute("value"));
                                                }
                                            }
                                            else
                                            {
                                                foreach (Control ctl in c.Controls)
                                                {
                                                    if (ctl.Name == "btn_Save")
                                                    {
                                                        save = xe2.GetAttribute("value");
                                                    }
                                                    if (ctl.Name == xe2.GetAttribute("key"))
                                                    {
                                                        ctl.Text = xe2.GetAttribute("value");
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (c.Name == "gvw_RecentlyAlarms")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            list_grv.Add(xe2.GetAttribute("value"));
                                        }
                                    }
                                    if (c.Name == "rcv_percent")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            list_rcv.Add(xe2.GetAttribute("value"));
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

        #region 多语言转换
        /// <summary>
        /// 转换Tag
        /// </summary>
        /// <param name="str">需要转换的Tag</param>
        /// <returns>转换后的结果</returns>
        private string Transformation(string str)
        {
            try
            {

                string[] temp = str.Split('_');
                string varname = temp[0];
                TagDictionary t = list_tagdir.Where(o => o.Basic == varname).FirstOrDefault();
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
            catch { return str; }       //异常原路返回

        }


        /// <summary>
        /// 转换Tag
        /// </summary>
        /// <param name="str">原始code编码</param>
        /// <param name="needinfo">是否需要补充信息</param>
        /// <returns></returns>
        private string Transformation(string str, bool needinfo)
        {
            try
            {

                string[] temp = str.Split('_');
                string varname = temp[0];
                string msg = "";
                if (temp[1] == "1") msg = "高报警";
                else if (temp[1] == "0") msg = "低报警";
                TagDictionary t = list_tagdir.Where(o => o.Basic == varname).FirstOrDefault();
                //判断当前测点在字典表里面是否可以查询到
                if (t != null)
                {
                    return t.TagShowName+" "+msg;//查询到返回转换结果
                }
                else
                {
                    return str;//查询不到直接返回
                }
            }
            catch { return str; }       //异常原路返回
        }
        #endregion


    }


    public class AlarmData
    {
        public AlarmData(string Channel, string Message, string Date)
        {
            this.Channel = Channel;
            this.Message = Message;
            this.Date = Date;
        }

        public string Channel { get; set; }
        public string Message { get; set; }
        public string Date { get; set; }
    }

    public class TagSimpleModel
    {
        public string Tag { get; set; }
        public decimal HValue { get; set; }
        public decimal LValue { get; set; }
        public int DrillId { get; set; }
    }


}
