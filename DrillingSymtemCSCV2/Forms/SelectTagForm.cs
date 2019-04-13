using DrillingSymtemCSCV2.Model;
using DrillingSymtemCSCV2.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class SelectTagForm : RadForm
    {
        private DrillOSEntities db;
        private List<DrillTag> TagList = new List<DrillTag>();
        private List<UserTag> UserTag = new List<UserTag>();
        private UserTagRef UserTagRef;
        private List<Button> btnlist = new List<Button>();//全部变量，用于定位用户当前所选择的测点
        private List<TagDictionary> list_tagdir = new List<TagDictionary>();
        //private DrillTag tag;
        public bool RemoveFlag = false;
        public string ThisTag { get; set; }
        public string Captial { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public string Tags { get; set; }
        public string Unit { get; set; }
        private List<string> list_tag = new List<string>();  //设置表头的多语言
        private List<string> list_error = new List<string>();  //错误信息
        public SelectTagForm()
        {
            InitializeComponent();
        }

        private void SelectTagForm_Load(object sender, EventArgs e)
        {
            //设置语言
            setControlLanguage();

            db = new DrillOSEntities();

            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            try
            {
                initTabName();
            }
            catch { }
        }

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                UserTagRef = db.UserTagRef.Where(o => /*o.DrillId == AppDrill.DrillID &&*/ o.Username == AppDrill.username).FirstOrDefault();
                if (UserTagRef != null)
                {
                    if (UserTagRef.JsonTag != null)
                        UserTag = new JavaScriptSerializer().Deserialize<List<UserTag>>(UserTagRef.JsonTag); //反序列化
                }
                else
                {
                    UserTagRef model = new UserTagRef();
                    DateTime dt = DateTime.Now;
                    model.DrillId = AppDrill.DrillID;
                    model.Username = AppDrill.username;
                    model.dataMakePGM = "SelectTagForm";
                    model.dataMakeTime = dt;
                    model.dataMakeUser = AppDrill.realName;
                    model.dataUpdPGM = "SelectTagForm";
                    model.dataUpdTime = dt;
                    model.dataUpdUser = AppDrill.realName;
                    db.UserTagRef.Add(model);
                    db.SaveChanges();
                }
                TagList = db.DrillTag.Where(o => o.DrillId == 1).ToList();
                list_tagdir = db.TagDictionary.Where(o => o.TransferType == AppDrill.language).ToList();//取出Tag字典表中的数据
                btnlist = new List<Button>();
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //设置按钮相关属性并添加到TabPage上
            try
            {
                setButtons();
            }
            catch { }
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 设置按钮相关属性
        private void setButtons()
        {
            List<DrillTag> list_drill = TagList.Where(o => o.Type == "Drilling").ToList();
            List<DrillTag> list_circulate = new List<DrillTag>();//TagList.Where(o => o.Type == "Circulate").ToList();
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
                    ////判断当前测点是否被占用
                    //if (!string.IsNullOrEmpty(list_drill[i].Form) && list_drill[i].Group != null && list_drill[i].Order != null)
                    //{
                    //    btns[i].Enabled = false;//禁用当前按钮
                    //    btns[i].BackColor = Color.Gray;
                    //    btns[i].ForeColor = Color.White;
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
                    ////判断当前测点是否被占用
                    //if (!string.IsNullOrEmpty(list_circulate[i].Form) && list_circulate[i].Group != null && list_circulate[i].Order != null)
                    //{
                    //    btns[i].Enabled = false;//禁用当前按钮
                    //    btns[i].BackColor = Color.Gray;
                    //    btns[i].ForeColor = Color.White;
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
                    ////判断当前测点是否被占用
                    //if (!string.IsNullOrEmpty(list_ctrldrill[i].Form) && list_ctrldrill[i].Group != null && list_ctrldrill[i].Order != null)
                    //{
                    //    btns[i].Enabled = false;//禁用当前按钮
                    //    btns[i].BackColor = Color.Gray;
                    //    btns[i].ForeColor = Color.White;
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
                    ////判断当前测点是否被占用
                    //if (!string.IsNullOrEmpty(list_gas[i].Form) && list_gas[i].Group != null && list_gas[i].Order != null)
                    //{
                    //    btns[i].Enabled = false;//禁用当前按钮
                    //    btns[i].BackColor = Color.Gray;
                    //    btns[i].ForeColor = Color.White;
                    //}
                    this.tab_gia.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            // 设置Gasform相关测点
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
                    ////判断当前测点是否被占用
                    //if (!string.IsNullOrEmpty(list_gas[i].Form) && list_gas[i].Group != null && list_gas[i].Order != null)
                    //{
                    //    btns[i].Enabled = false;//禁用当前按钮
                    //    btns[i].BackColor = Color.Gray;
                    //    btns[i].ForeColor = Color.White;
                    //}
                    this.tab_gia.Controls.Add(btns[i]);
                    btnlist.Add(btns[i]);
                }
            }
            //判断当前控件的值是否已经设置
            var TagModel = TagList.Where(o => o.Tag == ThisTag).FirstOrDefault();
            if (TagModel != null)
            {
                for (int i = 0; i < btnlist.Count; i++)
                {
                    if (btnlist[i].Tag.ToString() == TagModel.Tag)
                    {
                        btnlist[i].BackColor = Color.Red;
                        btnlist[i].Enabled = true;
                        var UserTagModel = UserTag.Where(o => o.Tag == ThisTag).FirstOrDefault();
                        if (UserTagModel != null)
                        {
                            txt_From.Text = UserTagModel.VFrom.ToString();
                            txt_TO.Text = UserTagModel.VTo.ToString();
                        }
                        else
                        {
                            txt_From.Text = TagModel.DefaultFrom.ToString();
                            txt_TO.Text = TagModel.DefaultTo.ToString();
                        }
                        break;
                    }
                }
            }

            //foreach (var item in HistoryDataForm.SelectedTag)
            //{
            //    foreach (var btn in btnlist)
            //    {
            //        if (item.Tag == btn.Tag.ToString() && item.Tag != ThisTag)
            //        {
            //            btn.Enabled = false;    //禁用当前按钮
            //            btn.BackColor = Color.Gray;
            //            btn.ForeColor = Color.White;
            //            break;
            //        }
            //    }
            //}
        }
        #endregion

        #region 设置按钮监听事件
        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                Color c = b.BackColor;//保存点击按钮的颜色
                var tag = UserTag.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
                //取消所有的按钮选中状态
                for (int i = 0; i < btnlist.Count; i++)
                {
                    if (btnlist[i].BackColor == Color.Red)
                        btnlist[i].BackColor = Color.Black;
                }
                if (c == Color.Black)
                {
                    b.BackColor = Color.Red;
                    //判断VFrom是否为空
                    if (tag != null && !string.IsNullOrEmpty(tag.VFrom.ToString()))
                        txt_From.Text = tag.VFrom.ToString();
                    else
                    {
                        var tagModel = TagList.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
                        if (tagModel != null)
                            txt_From.Text = tagModel.DefaultFrom.ToString();
                        else
                            txt_From.Text = "1.0";
                    }
                    //判读VTo是否为空
                    if (tag != null && !string.IsNullOrEmpty(tag.VTo.ToString()))
                        txt_TO.Text = tag.VTo.ToString();
                    else
                    {
                        var tagModel = TagList.Where(o => o.Tag == b.Tag.ToString()).FirstOrDefault();
                        if (tagModel != null)
                            txt_TO.Text = tagModel.DefaultTo.ToString();
                        else
                            txt_TO.Text = "1000.0";
                    }
                }
                else
                {
                    b.BackColor = Color.Black;//如果重复点击了该按钮，则取消点击事件
                    txt_From.Text = "";
                    txt_TO.Text = "";
                }
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                //考虑到用户可能会什么都不操作直接update测点
                for (int i = 0; i < btnlist.Count; i++)
                {
                    if (btnlist[i].BackColor == Color.Red)
                    {
                        if (Decimal.Parse(txt_From.Text) >= Decimal.Parse(txt_TO.Text))
                        {
                            rlbl_error.Text = list_error[0];
                            return;
                        }
                        var tag = TagList.Where(o => o.Tag == btnlist[i].Tag.ToString()).FirstOrDefault();
                        if (tag != null)
                        {
                            //设置需要传递的数据
                            this.Captial = Transformation(tag.Tag);
                            this.From = Decimal.Parse(txt_From.Text);
                            this.To = Decimal.Parse(txt_TO.Text);
                            this.Tags = tag.Tag;
                            this.Unit = tag.Unit;

                            var model = UserTag.Where(o => o.Tag == btnlist[i].Tag.ToString()).FirstOrDefault();
                            if (model != null)
                            {
                                model.VFrom = this.From;
                                model.VTo = this.To;
                            }
                            else
                            {
                                UserTag UTModel = new UserTag();
                                UTModel.Tag = this.Tags;
                                UTModel.VFrom = this.From;
                                UTModel.VTo = this.To;
                                UserTag.Add(UTModel);
                            }
                            if (UserTagRef != null)
                            {
                                UserTagRef.JsonTag = new JavaScriptSerializer().Serialize(UserTag);
                                UserTagRef.dataUpdPGM = "SeleteTagForm";
                                UserTagRef.dataUpdTime = DateTime.Now;
                                UserTagRef.dataUpdUser = AppDrill.username;
                            }
                            db.SaveChanges();
                            break;
                        }
                    }
                }
                this.Close();
            }
            catch 
            {
                rlbl_error.Text = list_error[1];
            }
        }

        /// <summary>
        /// 移除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, EventArgs e)
        {
            RemoveFlag = true;
            this.Close();
        }

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
                    return t.TagShowName;//查询到返回转换结果
                else
                    return str;//查询不到直接返回
            }
            catch { return str; }
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

        //初始化测点类型
        private void initTabName()
        {
            this.DrillPage.Text = list_tag[0];
            this.CirculatePage.Text = list_tag[1];
            this.DirectionalPage.Text = list_tag[2];
            this.tab_gia.Text = list_tag[3];
            this.tab_gfm.Text = list_tag[4];
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
