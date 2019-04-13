using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.Forms;
using System.Web.Script.Serialization;

namespace DrillingSymtemCSCV2.UserControls
{
    /// <summary>
    /// UserControl   Add by ZAY in 2017.5.23
    /// </summary>
    public partial class DataShowControl : UserControl
    {
        private DrillOSEntities _db;
        public string fname { get; set; }   //当前form名称
        public int group { get; set; }      //第几组
        public int drillID { get; set; }      //第几组
        public static List<DrillTag> listTag;
        private List<UserTag> UserTag;
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();//定义测点字典表
        private Dictionary<DATA_SHOW, UserControls.DataShow2> m_dicTags = new Dictionary<DATA_SHOW, UserControls.DataShow2>();

        public DataShowControl()
        {
            InitializeComponent();
        }

        private void DataShowControl_Load(object sender, EventArgs e)
        {
            _db = new DrillOSEntities();
            if (listTag == null)
            {
                listTag = new List<DrillTag>();
            }

            UserTag = new List<UserTag>();

            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion

            //订阅点击事件
            SetDataShowClick();
        }

        public void resetDataShow()
        {
            InitDataShow(true);
        }

        public void setTagValue(string strValue)
        {
            dataShow21.setTagValue(strValue);
            dataShow22.setTagValue(strValue);
            dataShow23.setTagValue(strValue);
            dataShow24.setTagValue(strValue);
            dataShow25.setTagValue(strValue);
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                InitDictionaryTags();
                //取出tag数据
                if (listTag.Count == 0)
                {
                    listTag = _db.DrillTag.Where(o => o.DrillId == 1).ToList();
                }

                var UserTagRef = _db.UserTagRef.Where(o => o.DrillId == drillID && o.Username == AppDrill.username).FirstOrDefault();
                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }

                list_tagdir = _db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
            }
            catch 
            { 
            }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InitDataShow();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 订阅点击事件

        public void SetDataShowClick()
        {
            dataShow21.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Captial.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Value.MouseClick += new MouseEventHandler(dataShow1_MouseClick);
            dataShow21.Unit.MouseClick += new MouseEventHandler(dataShow1_MouseClick);

            dataShow22.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Captial.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Value.MouseClick += new MouseEventHandler(dataShow2_MouseClick);
            dataShow22.Unit.MouseClick += new MouseEventHandler(dataShow2_MouseClick);

            dataShow23.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Captial.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Value.MouseClick += new MouseEventHandler(dataShow3_MouseClick);
            dataShow23.Unit.MouseClick += new MouseEventHandler(dataShow3_MouseClick);

            dataShow24.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Captial.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Value.MouseClick += new MouseEventHandler(dataShow4_MouseClick);
            dataShow24.Unit.MouseClick += new MouseEventHandler(dataShow4_MouseClick);

            dataShow25.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Captial.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Value.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
            dataShow25.Unit.MouseClick += new MouseEventHandler(dataShow5_MouseClick);
        }

        private void dataShow1_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = group;
                form.order = 1;
                form.formName = fname;
                form.DrillId = drillID.ToString();
                form.ShowDialog();

                //在界面上删除该点
                if (form.remove) 
                {
                    setDataShowEmpty(dataShow21);
                }
                else
                {
                    setDataShow(dataShow21, form);
                }
            }
        }

        private void dataShow2_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = group;
                form.order = 2;
                form.formName = fname;
                form.DrillId = drillID.ToString();
                form.ShowDialog();

                //在界面上删除该点
                if (form.remove)
                {
                    setDataShowEmpty(dataShow22);
                }
                else
                {
                    setDataShow(dataShow22, form);
                }
            }
        }

        private void dataShow3_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = group;
                form.order = 3;
                form.formName = fname;
                form.DrillId = drillID.ToString();
                form.ShowDialog();

                //在界面上删除该点
                if (form.remove) 
                {
                    setDataShowEmpty(dataShow23);
                }
                else
                {
                    setDataShow(dataShow23, form);
                }
            }
        }

        private void dataShow4_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = group;
                form.order = 4;
                form.formName = fname;
                form.DrillId = drillID.ToString();
                form.ShowDialog();

                //在界面上删除该点
                if (form.remove)
                {
                    setDataShowEmpty(dataShow24);
                }
                else
                {
                    setDataShow(dataShow24, form);
                }
            }
        }

        private void dataShow5_MouseClick(object sender, MouseEventArgs e)
        {
            EditTagForm form = new EditTagForm();
            if (e.Button.ToString() != "Right")
            {
                form.dataShow2 = true;
                form.group = group;
                form.order = 5;
                form.formName = fname;
                form.DrillId = drillID.ToString();
                form.ShowDialog();

                //在界面上删除该点
                if (form.remove) 
                {
                    setDataShowEmpty(dataShow25);
                }
                else
                {
                    setDataShow(dataShow25, form);
                }
            }
        }
        #endregion

        #region 报警闪烁  ADD by ZAY in 2017.5.25

        /// <summary>
        /// 判断是否报警
        /// </summary>
        private void isAlarm(string Tag, string DrillId, string Value, int Index)
        {
            try
            {
                var Dirrl = 0;
                if (!string.IsNullOrEmpty(DrillId))
                {
                    Dirrl = int.Parse(DrillId);
                }

                var model = listTag.Find(o => o.Tag == Tag && o.DrillId == Dirrl && o.AlarmFlag == 1);
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(Value))
                    {
                        AlarmModel old_am = Alarms.list_Alarms.Find(o => o.Tag == Tag);
                        if (model.HValue <= Decimal.Parse(Value) || model.LValue >= Decimal.Parse(Value))
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
                            }

                            setDataShowBackColor(Index, 0);
                        }
                        else
                        {
                            if (old_am != null)
                            {
                                if (old_am.status != 1)
                                {
                                    Alarms.list_Alarms.Remove(old_am);//已经不报警了,移除
                                    //状态不等于1的操作数据库
                                    var alarm = _db.AlarmHistory.Where(o => o.Tag == old_am.Tag && o.RecoveryTime == null);
                                    if (alarm != null)
                                    {
                                        foreach (var a in alarm)
                                        {
                                            a.RecoveryTime = DateTime.Now;
                                        }
                                        backgroundWorker2.WorkerSupportsCancellation = true;
                                        backgroundWorker2.RunWorkerAsync();//开启异步保存
                                    }
                                }
                            }

                            setDataShowBackColor(Index, 1);
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
            TagDictionary t=list_tagdir.Where(o => o.Basic == str).FirstOrDefault();
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
            backgroundWorker2.RunWorkerAsync();
        }

        private double UnitConversion(string Tag, string DrillId, double value)
        {
            try
            {
                if (AppDrill.UnitFormat == "yz")
                {
                    var Dirrl = 0;
                    if (!string.IsNullOrEmpty(DrillId))
                    {
                        Dirrl = int.Parse(DrillId);
                    }
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

        //设置DataShow的背景色（报警或者不报警）
        private void setDataShowBackColor(int iIndex, int iSetColor)
        {
            switch (iIndex)
            {
                case 0:
                    dataShow21.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 1:
                    dataShow22.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 2:
                    dataShow23.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 3:
                    dataShow24.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                case 4:
                    dataShow25.BackColor = 0 == iSetColor ? Color.Red : Color.Black;
                    break;
                default:
                    break;
            }
        }

        //初始化字典
        private void InitDictionaryTags()
        {
            m_dicTags.Add(DATA_SHOW.tagOne, dataShow21);
            m_dicTags.Add(DATA_SHOW.tagTwo, dataShow22);
            m_dicTags.Add(DATA_SHOW.tagThree, dataShow23);
            m_dicTags.Add(DATA_SHOW.tagFour, dataShow24);
            m_dicTags.Add(DATA_SHOW.tagFive, dataShow25);
        }

        //根据数据库保存的默认值，设置控件DataShow2的值
        private void resetTag(UserControls.DataShow2 dataShow, int iIndex)
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
                        dataShow.DTag = data.Tag;
                        dataShow.DSCaptial = Transformation(data.Tag);
                        setDataShowUnit(dataShow, taglist.Unit);
                        dataShow.Value.Text = "###";
                        dataShow.SetTag();
                    }
                }
            }
            catch
            {
            }
        }

        //根据数据库保存datashow的数据，设置控件DataShow2的值
        private void setDataShow(UserControls.DataShow2 dataShow, int iIndex)
        {
            var userTag = UserTag.Find(o => o.Form == fname && o.Group == 4 && o.Order == iIndex);
            if (null != userTag)
            {
                var tag = listTag.Where(o => o.Tag == userTag.Tag).FirstOrDefault();
                if (null != tag)
                {
                    dataShow.DTag = userTag.Tag;
                    dataShow.DSCaptial = Transformation(userTag.Tag);
                    setDataShowUnit(dataShow, tag.Unit);
                    dataShow.Value.Text = "###";
                    dataShow.SetTag();
                }
                else
                {
                    dataShow.DTag = "";
                    dataShow.Captial.Text = "";
                    dataShow.Unit.Text = "";
                    dataShow.Value.Text = "";
                }
            }
            else
            {
                dataShow.DTag = "";
                dataShow.Captial.Text = "";
                dataShow.Unit.Text = "";
                dataShow.Value.Text = "";
            }
        }

        //根据选择的测点值来更新要显示的DataShow2对象值
        private void setDataShow(UserControls.DataShow2 dataShow, EditTagForm form)
        {
            if (null != form && null != dataShow)
            {
                if (!string.IsNullOrEmpty(form.Captial))
                {
                    dataShow.DTag = form.Tags;
                    dataShow.DSCaptial = form.Captial;
                    dataShow.DSLValue = "L:" + form.LValue;
                    dataShow.DSHValue = "H:" + form.HValue;
                    setDataShowUnit(dataShow, form.Unit);
                    dataShow.SetTag();
                }
            }
        }

        //将DataShow2对象的值设置为空
        private void setDataShowEmpty(UserControls.DataShow2 dataShow)
        {
            dataShow.DSCaptial = "";
            dataShow.DSValue = "";
            dataShow.DSLValue = "";
            dataShow.DSHValue = "";
            dataShow.DSUnit = "";
            dataShow.DTag = "";
            dataShow.SetTag();
            dataShow.BackColor = Color.Black;
        }

        //初始化控件DataShow2的值
        private void InitDataShow(bool bIsReset = false)
        {
            foreach (var data in m_dicTags)
            {
                var dataShow = data.Value;
                int iIndex = (int)data.Key + 1;
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
        }

        //设置DataShow的单位
        private void setDataShowUnit(DataShow2 dataShow, string strUnit)
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
                    dataShow.DSUnit = UnitModel.UnitTo;
                }
                else
                {
                    dataShow.DSUnit = strUnit;
                }
            }
            else if (AppDrill.UnitFormat == "gz")
            {
                dataShow.DSUnit = strUnit;
            }
        }

        //显示DataShow接收到的实时数据
        public void ShowMessage(RealTimeData realTimeData)
        {
            if (null == realTimeData)
            {
                return;
            }

            foreach (var data in m_dicTags)
            {
                var dataShow = data.Value;
                if (null != dataShow)
                {
                    int iIndex = (int)data.Key;
                    var tag = realTimeData.realTags.Where(o => o.Tag == dataShow.DTag).FirstOrDefault();
                    if (null != tag)
                    {
                        double value = 0;
                        if (double.TryParse(tag.Value.ToString(), out value))
                        {
                            string strBoxID = realTimeData.BoxId.ToString();
                            var newValue = Comm.UnitConversion(listTag, dataShow.DTag, strBoxID, value);
                            if ((bool)listTag.Where(o => o.Tag == dataShow.DTag).FirstOrDefault().IsBool)
                            {
                                dataShow.Value.Text = newValue > 0 ? "Yes" : "No";
                            }
                            else
                            {
                                dataShow.Value.Text = newValue.ToString("#0.00");
                            }

                            isAlarm(dataShow.DTag, strBoxID, newValue.ToString(), iIndex);
                        }
                    }
                }
            }

        }
    }
}
