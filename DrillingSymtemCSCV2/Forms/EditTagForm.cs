using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Xml;
using System.Web.Script.Serialization;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class EditTagForm : RadForm
    {
        private DrillOSEntities db;
        private List<DrillTag> TagList;
        private List<UserTag> UserTag;
        private UserTagRef UserTagRef;
        //private DrillTag tag;
        private string FirstTag;//保存初始的测点ID
        public int order { get;set;}//设置控件排列顺序
        public string formName { get; set; }//设置窗体名称(如：DrillFrom，取Drill)
        public int group { get; set; }//控件属于第几组
        public bool dataShow2 { get; set; }//此控件用于判断是fourchart中的datashow还是datashow2控件
        //用于DataShow被修改之后更新界面的值
        public string Captial;
        public string Unit;
        public string From;
        public string To;
        public string LValue;
        public string HValue;
        public string Tags;
        public string DrillId;
        public bool remove = false;//是否点击了移除测点
        public List<Button> btnlist;//全部变量，用于定位用户当前所选择的测点
        private List<TagDictionary> list_tagdir=new List<TagDictionary>();
        private List<string> list_tag = new List<string>();  //设置表头的多语言
        private List<string> list_error=new List<string>();  //错误信息
        private List<WebRecordTag> WebRecordTag = new List<WebRecordTag>();
        private string str_error;//保存提示信息
        public EditTagForm()
        {
            InitializeComponent();
            TagList = new List<DrillTag>();
            UserTag = new List<UserTag>();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            try
            {
                str_error = "";
                btn_Update.Enabled = false;
                backgroundWorker2.WorkerSupportsCancellation = true;
                backgroundWorker2.RunWorkerAsync();
            }
            catch
            {
                MessageBox.Show(AppDrill.message[5]);//请勿重复点击
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            backgroundWorker3.WorkerSupportsCancellation = true;
            backgroundWorker3.RunWorkerAsync();
        }

        private void EditTagForm_Load(object sender, EventArgs e)
        {
            db = new DrillOSEntities();
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            //设置语言
            setControlLanguage();
            //多语言对应TabName
            initTabName();    
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                WebRecordTag = db.WebRecordTag.Where(o => o.DrillId == 1).ToList();
                int iDrillID = int.Parse(DrillId);
                UserTagRef = db.UserTagRef.Where(o => o.DrillId == iDrillID && o.Username == AppDrill.username).FirstOrDefault();
                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                    {
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                    }
                }
                else
                {
                    UserTagRef model = new UserTagRef();
                    DateTime dt = DateTime.Now;
                    model.DrillId = iDrillID;
                    model.Username = AppDrill.username;
                    model.dataMakePGM = "EditTagForm";
                    model.dataMakeTime = dt;
                    model.dataMakeUser = AppDrill.realName;
                    model.dataUpdPGM = "EditTagForm";
                    model.dataUpdTime = dt;
                    model.dataUpdUser = AppDrill.realName;
                    db.UserTagRef.Add(model);
                    db.SaveChanges();
                }
                //0703修改，drilltag用同一个
                TagList = db.DrillTag.Where(o => o.DrillId == 1).ToList();
             //   TagList = db.DrillTag.Where(o=>o.DrillId==AppDrill.DrillID).ToList();
                btnlist = new List<Button>();
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //设置TabPage页面
            switch (formName)
            {
                case "Drilling": this.tab_tag.SelectedTab = this.DrillPage;
                    break;
                case "Circulate": this.tab_tag.SelectedTab = this.DrillPage;
                    break;
                case "Directional": this.tab_tag.SelectedTab = this.DirectionalPage;
                    break;
                case "Gas in air": this.tab_tag.SelectedTab = this.tab_gia;
                    break;
                case "Gas from mud": this.tab_tag.SelectedTab = this.tab_gfm;
                    break;
                default: this.tab_tag.SelectedTab = this.DrillPage;
                    break;
            }
            //设置按钮相关属性并添加到TabPage上
            setButtons();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 设置按钮相关属性
        private void setButtons()
        {
            List<DrillTag> list_drill = TagList.Where(o => o.Type == "Drilling").ToList();
            List<DrillTag> list_circulate = TagList.Where(o => o.Type == "Circulate").ToList();
            List<DrillTag> list_ctrldrill = TagList.Where(o => o.Type == "Directional").ToList();
            List<DrillTag> list_gia = new List<DrillTag>();//TagList.Where(o => o.Type == "Gas in air").ToList();
            List<DrillTag> list_gfm = new List<DrillTag>();//TagList.Where(o => o.Type == "Gas from mud").ToList();
            //开始设置Button
            // 设置DrillForm相关测点
            if (list_drill.Count > 0)
            {
                Button[] btns = new Button[list_drill.Count];  //声明对象
                for (int i = 0; i < list_drill.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 40 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 30);
                    btns[i].Text = Transformation(list_drill[i].Tag);
                    btns[i].Tag = list_drill[i].Tag;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    //判断当前测点是否被占用
                    //var UTmodel = UserTag.Where(o => o.Tag == list_drill[i].Tag).FirstOrDefault();
                    //if (UTmodel != null)
                    //{
                    //    if (!string.IsNullOrEmpty(UTmodel.Form) && UTmodel.Group != null && UTmodel.Order != null)
                    //    {
                    //        btns[i].Enabled = false;//禁用当前按钮
                    //        btns[i].BackColor = Color.Gray;
                    //        btns[i].ForeColor = Color.White;
                    //    }
                    //}
                    this.DrillPage.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            // 设置Circulate相关测点
            if (list_circulate.Count > 0)
            {
                Button[] btns = new Button[list_circulate.Count];  //声明对象
                for (int i = 0; i < list_circulate.Count; i++)
                {
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 40 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 30);
                    btns[i].Text = Transformation(list_circulate[i].Tag);
                    btns[i].Tag = list_circulate[i].Tag;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    //判断当前测点是否被占用
                    //var UTmodel = UserTag.Where(o => o.Tag == list_circulate[i].Tag).FirstOrDefault();
                    //if (UTmodel != null)
                    //{
                    //    if (!string.IsNullOrEmpty(UTmodel.Form) && UTmodel.Group != null && UTmodel.Order != null)
                    //    {
                    //        btns[i].Enabled = false;//禁用当前按钮
                    //        btns[i].BackColor = Color.Gray;
                    //        btns[i].ForeColor = Color.White;
                    //    }
                    //}
                    this.CirculatePage.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            // 设置CtrlDrillForm相关测点
            if (list_ctrldrill.Count > 0)
            {
                Button[] btns = new Button[list_ctrldrill.Count];  //声明对象
                for (int i = 0; i < list_ctrldrill.Count; i++)
                {
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 40 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 30);
                    btns[i].Text = Transformation(list_ctrldrill[i].Tag);
                    btns[i].Tag = list_ctrldrill[i].Tag;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    //判断当前测点是否被占用
                    //var UTmodel = UserTag.Where(o => o.Tag == list_ctrldrill[i].Tag).FirstOrDefault();
                    //if (UTmodel != null)
                    //{
                    //    if (!string.IsNullOrEmpty(UTmodel.Form) && UTmodel.Group != null && UTmodel.Order != null)
                    //    {
                    //        btns[i].Enabled = false;//禁用当前按钮
                    //        btns[i].BackColor = Color.Gray;
                    //        btns[i].ForeColor = Color.White;
                    //    }
                    //}
                    this.DirectionalPage.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            // 设置Gasform相关测点
            if (list_gia.Count > 0)
            {
                Button[] btns = new Button[list_gia.Count];  //声明对象
                for (int i = 0; i < list_gia.Count; i++)
                {
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 40 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 30);
                    btns[i].Text = Transformation(list_gia[i].Tag);
                    btns[i].Tag = list_gia[i].Tag;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    //判断当前测点是否被占用
                    //var UTmodel = UserTag.Where(o => o.Tag == list_gas[i].Tag).FirstOrDefault();
                    //if (UTmodel != null)
                    //{
                    //    if (!string.IsNullOrEmpty(UTmodel.Form) && UTmodel.Group != null && UTmodel.Order != null)
                    //    {
                    //        btns[i].Enabled = false;//禁用当前按钮
                    //        btns[i].BackColor = Color.Gray;
                    //        btns[i].ForeColor = Color.White;
                    //    }
                    //}
                    this.tab_gia.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            if (list_gfm.Count > 0)
            {
                Button[] btns = new Button[list_gfm.Count];  //声明对象
                for (int i = 0; i < list_gfm.Count; i++)
                {
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 160 * (i % 5), 6 + 40 * (i / 5));
                    btns[i].Size = new System.Drawing.Size(150, 30);
                    btns[i].Text = Transformation(list_gfm[i].Tag);
                    btns[i].Tag = list_gfm[i].Tag;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);
                    //判断当前测点是否被占用
                    //var UTmodel = UserTag.Where(o => o.Tag == list_gas[i].Tag).FirstOrDefault();
                    //if (UTmodel != null)
                    //{
                    //    if (!string.IsNullOrEmpty(UTmodel.Form) && UTmodel.Group != null && UTmodel.Order != null)
                    //    {
                    //        btns[i].Enabled = false;//禁用当前按钮
                    //        btns[i].BackColor = Color.Gray;
                    //        btns[i].ForeColor = Color.White;
                    //    }
                    //}
                    this.tab_gfm.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            //判断当前控件的值是否已经设置
            var TagModel = UserTag.Where(o => o.Form == formName && o.Group == group && o.Order == order).FirstOrDefault();
            if (TagModel != null)
            {
                for (int i = 0; i < btnlist.Count; i++)
                {
                    if (btnlist[i].Tag.ToString() == TagModel.Tag)
                    {
                        FirstTag = btnlist[i].Tag.ToString();
                        btnlist[i].BackColor = Color.Red;
                        btnlist[i].Enabled = true;
                        txt_From.Text = TagModel.VFrom.ToString();
                        txt_TO.Text = TagModel.VTo.ToString();
                        break;
                    }
                }
            }

        }
        #endregion

        #region 设置按钮监听事件
        private void btn_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Color c = b.BackColor;//保存点击按钮的颜色
           // var tag = UserTag.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
            var tag = UserTag.Where(o => o.Order == order && o.Group == group && o.Form == formName && o.Tag == b.Tag.ToString()).FirstOrDefault();
            //取消所有的按钮选中状态
            for (int i = 0; i < btnlist.Count; i++)
            {
                if (btnlist[i].BackColor == Color.Red)
                {
                    btnlist[i].BackColor = Color.Black;
                }
            }
            if (c == Color.Black)
            {
                b.BackColor = Color.Red;
                //判断VFrom是否为空
                if (tag != null && !string.IsNullOrEmpty(tag.VFrom.ToString()))
                {
                    txt_From.Text = tag.VFrom.ToString();
                }
                else
                {
                    var tagModel = TagList.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
                    if (tagModel != null)
                    {
                        txt_From.Text = tagModel.DefaultFrom.ToString();
                    }
                    else
                    {
                        txt_From.Text = "1.0";
                    }
                }
                //判读VTo是否为空
                if (tag != null && !string.IsNullOrEmpty(tag.VTo.ToString()))
                {
                    txt_TO.Text = tag.VTo.ToString();
                }
                else
                {
                    var tagModel = TagList.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
                    if (tagModel != null)
                    {
                        txt_TO.Text = tagModel.DefaultTo.ToString();
                    }
                    else
                    {
                        txt_TO.Text = "1000.0";
                    }
                }

            }
            else
            {
                b.BackColor = Color.Black;//如果重复点击了该按钮，则取消点击事件
                txt_From.Text = "";
                txt_TO.Text = "";
            }
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
            catch 
            {
                return str;
            }
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
                                        if (c.Name == "tab_tag")
                                        {
                                            XmlNodeList xn_list3 = node.ChildNodes;
                                            foreach (XmlNode node3 in xn_list3)
                                            {
                                                XmlElement xe3 = (XmlElement)node3;
                                                list_tag.Add(xe3.GetAttribute("value"));
                                            }
                                        }
                                        if (c.Name == "rlbl_error")
                                        {
                                            XmlNodeList xn_list3 = node.ChildNodes;
                                            foreach (XmlNode node3 in xn_list3)
                                            {
                                                XmlElement xe3 = (XmlElement)node3;
                                                list_error.Add(xe3.GetAttribute("value"));
                                            }
                                        }
                                        c.Text = xe.GetAttribute("value");//设置控件的Text
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
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

        private void initTabName()
        {
            try
            {
                //设置选项卡名称
                this.DrillPage.Text = list_tag[0];
                this.CirculatePage.Text = list_tag[1];
                this.DirectionalPage.Text = list_tag[2];
                this.tab_gia.Text = list_tag[3];
                this.tab_gfm.Text = list_tag[4];
            }
            catch 
            { 
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //考虑到用户可能会什么都不操作直接update测点
                for (int i = 0; i < btnlist.Count; i++)
                {
                    if (btnlist[i].BackColor == Color.Red)
                    {
                        if (Decimal.Parse(txt_From.Text) > Decimal.Parse(txt_TO.Text))
                        {
                            rlbl_error.Text = list_error[1];
                            return;
                        }

                        try
                        {
                            //清除之前测点的信息
                            if (FirstTag != btnlist[i].Tag.ToString())
                            {
                                var oldTag = UserTag.Where(o => o.Tag == FirstTag).FirstOrDefault();
                                if (oldTag != null)
                                {
                                    oldTag.Form = null;
                                    oldTag.Group = null;
                                    oldTag.Order = null;

                                    if (UserTagRef != null)
                                    {
                                        UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(UserTag);
                                        UserTagRef.dataUpdPGM = "EditTag";
                                        UserTagRef.dataUpdTime = DateTime.Now;
                                        UserTagRef.dataUpdUser = AppDrill.username;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        catch 
                        {
                            rlbl_error.Text = AppDrill.message[4];//保存失败
                            return;
                        }

                        var tag = TagList.Where(o => o.Tag == btnlist[i].Tag.ToString()).FirstOrDefault();
                        if (tag != null)
                        {
                            //设置需要传递的数据
                            this.Captial = Transformation(tag.Tag);
                            this.HValue = tag.HValue.ToString();
                            this.LValue = tag.LValue.ToString();
                            this.Unit = tag.Unit;
                            this.From = txt_From.Text;
                            this.To = txt_TO.Text;
                            this.Tags = tag.Tag;
                            //this.DrillId = tag.DrillId.ToString();

                            if (AppDrill.username == "driller")
                            {
                                var web = WebRecordTag.Where(o => o.Tag == tag.Tag && o.Group <= 3);
                                foreach(WebRecordTag w in web)
                                if (web != null)
                                {
                                    w.From = Convert.ToDecimal(txt_From.Text);
                                    w.To = Convert.ToDecimal(txt_TO.Text);
                                    w.dataUpdPGM = "driller";
                                    w.dataUpdTime = DateTime.Now;
                                    w.dataUpdUser = "driller";
                                }
                            }

                            var model = UserTag.Where(o => o.Order == order && o.Group == group && o.Form == formName).FirstOrDefault();
                            if (model != null)
                            {
                                model.Tag = btnlist[i].Tag.ToString();
                                model.VFrom = Convert.ToDecimal(txt_From.Text);
                                model.VTo = Convert.ToDecimal(txt_TO.Text);
                            }
                            else
                            {
                                UserTag UTmodel = new UserTag();
                                UTmodel.Tag = tag.Tag;
                                UTmodel.VFrom = Convert.ToDecimal(txt_From.Text);
                                UTmodel.VTo = Convert.ToDecimal(txt_TO.Text);
                                UTmodel.Form = formName;
                                UTmodel.Group = group;
                                UTmodel.Order = order;
                                UserTag.Add(UTmodel);
                            }

                            var userTags = UserTag.Where(o => o.Order == null && o.Group == null && o.Form == null).ToList();

                            foreach (var item in userTags)
                            {
                                UserTag.Remove(item);
                            }

                            try
                            {
                                if (UserTagRef != null)
                                {
                                    UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(UserTag);
                                    UserTagRef.dataUpdPGM = "EditTagForm";
                                    UserTagRef.dataUpdTime = DateTime.Now;
                                    UserTagRef.dataUpdUser = AppDrill.username;
                                    db.SaveChanges();
                                }
                            }
                            catch 
                            {
                                str_error = AppDrill.message[4];//保存失败
                                return;
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                str_error = list_error[0];//量程格式不正确
            }
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                btn_Update.Enabled = true;
                if (!string.IsNullOrEmpty(str_error))
                {
                    rlbl_error.Text = str_error;
                    return;
                }
                this.Close();
            }
            catch { }
            backgroundWorker2.CancelAsync();
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                UserTag old = null;
                if (formName != null)
                {
                    old = UserTag.Where(o => o.Form == formName && o.Order == order && o.Group == group).FirstOrDefault();
                }

                if (old != null)
                {
                    UserTag.Remove(old);
                    old.Form = null;
                    old.Group = null;
                    old.Order = null;
                    if (UserTagRef != null)
                    {
                        UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(UserTag);
                        UserTagRef.dataUpdPGM = "EditTagForm";
                        UserTagRef.dataUpdTime = DateTime.Now;
                        UserTagRef.dataUpdUser = AppDrill.username;
                        db.SaveChanges();
                    }
                }

                this.remove = true;

                if (this.InvokeRequired) 
                {
                    this.Invoke(new Action(()=>this.Close()));
                }
               // this.Close();
            }
            catch { }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker3.CancelAsync();
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
