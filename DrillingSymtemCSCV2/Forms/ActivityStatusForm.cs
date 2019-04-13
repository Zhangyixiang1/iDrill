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
namespace DrillingSymtemCSCV2.Forms
{
    public partial class ActivityStatusForm : RadForm
    {
        private static string OldSelectName = "";
        public String SelectActivityName;
        private ActivityStatus data;
        List<Button> btnList = new List<Button>();
        public BaseForm bsform;
        public string defaultString = "";
        //自己语言的字典
        //M1_翻译
        //M1，翻译
        private Dictionary<string, string> LanuangeDictionary = new Dictionary<string, string>();
        //翻译后的语言
        private List<string> textValueList = new List<string>();

        public ActivityStatusForm()
        {
            try
            {
                InitializeComponent();
                //初始化默认居中显示
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch { }
        }

        private void ActivityStatusForm_Load(object sender, EventArgs e)
        {
           
                //开始加载数据
                bkg_LoadData.RunWorkerAsync();

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dbContext = new DrillOSEntities())
                {
                    //是否存在被选中后的Tag
                    var data = dbContext.ActivityStatus.Where(o => o.IsSelect == true && o.DrillID == AppDrill.DrillID).FirstOrDefault();
                    //置为红色背景的按钮
                    var SelectBtn = btnList.Where(o => o.BackColor == Color.Red).FirstOrDefault();
                    
                    //如果用户取消了选中
                    if (SelectBtn == null)
                    {
                        if (data == null)
                        {
                            this.Close(); 
                            return;
                        } 
                        data.IsSelect = false;
                        data.SelectDateTo = DateTime.Now;
                        
                        data.dataUpdPGM = "DrillingSystemCSCV2";
                        data.dataUpdTime = DateTime.Now;
                        data.dataUpdUser = AppDrill.username;
                        
                        dbContext.SaveChanges();
                        bsform.setActivityTitle(defaultString);
                        OldSelectName = "";
                        this.Close();
                        return;
                    }

                    //重复选中无意义
                    if (OldSelectName == SelectBtn.Text)
                    {
                        this.Close();
                        return;
                    }

                    if (data == null)
                    {
                        //不存在被占用的按钮
                        //insert data
                        var insertData = new ActivityStatus();
                        
                        var strM = "";
                        var M = LanuangeDictionary.Where(o => o.Value == SelectBtn.Text).Select(o => o.Key).FirstOrDefault();  //get all keys  
                        if (M!=null){
                            strM =  M + "_" + SelectBtn.Text;
                        }

                        insertData.ActivityName = strM;
                        insertData.DrillID = AppDrill.DrillID;
                        insertData.SelectDateForm = DateTime.Now;
                        insertData.IsSelect = true;
                        
                        insertData.dataMakeTime = DateTime.Now;
                        insertData.dataMakeUser = AppDrill.username;
                        insertData.dataMakePGM = "DrillingSystemCSCV2";
                        
                        dbContext.ActivityStatus.Add(insertData);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        //存在被占用的按钮
                        if (data.SelectDateTo != null)
                        {
                            //如果没有SelectToTime
                            //update data

                            //把之前的东西选中项为 false
                            data.IsSelect = false;
                            data.dataUpdPGM = "DrillingSystemCSCV2";
                            data.dataUpdTime = DateTime.Now;
                            data.dataUpdUser = AppDrill.username;

                            //插入新的数据 并设置选中为true
                            var insertData = new ActivityStatus();


                            var strM = "";
                            var M = LanuangeDictionary.Where(o => o.Value == SelectBtn.Text).Select(o => o.Key).FirstOrDefault();  //get all keys  
                            if (M != null)
                            {
                                strM = M + "_" + SelectBtn.Text;
                            }

                            insertData.ActivityName = strM;
                            insertData.DrillID = AppDrill.DrillID;
                            insertData.SelectDateForm = DateTime.Now;
                            insertData.IsSelect = true;
                            
                            insertData.dataMakeTime = DateTime.Now;
                            insertData.dataMakeUser = AppDrill.username;
                            insertData.dataMakePGM = "DrillingSystemCSCV2";

                            dbContext.ActivityStatus.Add(insertData);

                            dbContext.SaveChanges();
                        }
                        else 
                        { 
                            //如果有SelectToTime
                            //insert data
                            data.IsSelect = false;
                            data.SelectDateTo = DateTime.Now;
                            data.dataUpdPGM = "DrillingSystemCSCV2";
                            data.dataUpdTime = DateTime.Now;
                            data.dataUpdUser = AppDrill.username;

                            var insertData = new ActivityStatus();


                            var strM = "";
                            var M = LanuangeDictionary.Where(o => o.Value == SelectBtn.Text).Select(o => o.Key).FirstOrDefault();  //get all keys  
                            if (M != null)
                            {
                                strM = M + "_" + SelectBtn.Text;
                            }

                            insertData.ActivityName = strM;
                            insertData.DrillID = AppDrill.DrillID;
                            insertData.SelectDateForm = DateTime.Now;
                            insertData.IsSelect = true;

                            insertData.dataMakeTime = DateTime.Now;
                            insertData.dataMakeUser = AppDrill.username;
                            insertData.dataMakePGM = "DrillingSystemCSCV2";

                            dbContext.ActivityStatus.Add(insertData);
                            dbContext.SaveChanges();
                        }
                    }
                    
                    if (SelectBtn == null)
                    {
                        bsform.setActivityTitle(defaultString);
                    }
                    else
                    {
                        bsform.setActivityTitle(SelectBtn.Text);
                    }
                    OldSelectName = SelectBtn.Text;
                }
            }
            catch {
            }

            try
            {
                this.Close();
            }
            catch { }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch { }
        }

        //开始加载 - -读取按钮
        private void bkg_LoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var dbContext = new DrillOSEntities())
                {
                    textValueList = getTranslateList(); 
                    //是否存在被选中后的Tag
                    data = dbContext.ActivityStatus.Where(o => o.IsSelect == true && o.DrillID == AppDrill.DrillID).FirstOrDefault();
                    if (data != null) { 
                        //翻译名字
                        data.ActivityName = LanuangeDictionary[data.ActivityName.Substring(0, data.ActivityName.IndexOf("_"))];
                        OldSelectName = data.ActivityName;
                    }
                }
            }
            catch { }
        }

        //获取按钮List文本
        private List<String> getTranslateList()
        {
            List<String> TranslateList = new List<string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == "ReportForm")//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //循环每个控件，设置当前语言应设置的值
                            if (xe.GetAttribute("key") == "Activity")
                            {
                                XmlNodeList m = node.ChildNodes;
                                int LanuangeCnt = 1;
                                foreach (XmlNode node2 in m)
                                {
                                    //这行是关键注意点 因为只取前27个
                                    if (LanuangeCnt>=28)
                                    {
                                        break;
                                    }
                                    XmlElement xe2 = (XmlElement)node2;
                                    TranslateList.Add(xe2.GetAttribute("value"));
                                    LanuangeDictionary.Add("M" + LanuangeCnt, xe2.GetAttribute("value"));
                                    ++LanuangeCnt;
                                }
                                continue;
                            }
                        }
                    }
                }

            }
            catch { }
            return TranslateList;
        }

        //绘画按钮被点击
        private void btn_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                Color c = b.BackColor;//保存点击按钮的颜色

                //取消所有的按钮选中状态
                for (int i = 0; i < btnList.Count; i++)
                {
                    if (btnList[i].BackColor == Color.Red)
                    {
                        btnList[i].BackColor = Color.Black;
                        btnList[i].ForeColor = Color.White;
                    }
                }
                
                if (c == Color.Black)
                {
                    b.BackColor = Color.Red;
                }
                else
                {
                    b.BackColor = Color.Black;//如果重复点击了该按钮，则取消点击事件
                }

            }
            catch { }
        }

        //加载完成
        private void bkg_LoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //翻译界面
                setControlLanguage();
                
                Button[] btns = new Button[textValueList.Count];  //声明对象
                for (int i = 0; i < textValueList.Count; i++)
                {
                    //设置按钮相关属性
                    btns[i] = new Button();
                    btns[i].Location = new System.Drawing.Point(6 + 205 * (i % 4), 6 + 80 * (i / 4));
                    btns[i].Size = new System.Drawing.Size(190, 70);
                    btns[i].Text = textValueList[i];
                    //btns[i].Tag = i;
                    btns[i].BackColor = Color.Black;
                    btns[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    btns[i].Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                    btns[i].ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                    btns[i].Click += new System.EventHandler(this.btn_Click);

                    ////判断当前测点是否被占用
                    //如果存在被占用的测点
                    if (data != null && textValueList[i] == data.ActivityName)
                    {
                        // btns[i].Enabled = false;//禁用当前按钮
                        btns[i].BackColor = Color.Red;
                        // btns[i].ForeColor = Color.White;
                    }

                    this.AcitvityStatusPage.Controls.Add(btns[i]);
                    btnList.Add(btns[i]);
                }
            }
            catch { }
        }

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
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    c.Text = xe.GetAttribute("value");//设置控件的Text
                                if (c.Name == "tab_tag")
                                {
                                    XmlNodeList xn_list3 = node.ChildNodes;
                                    foreach (XmlNode node3 in xn_list3)
                                    {
                                        XmlElement xe3 = (XmlElement)node3;
                                        AcitvityStatusPage.Text = xe3.GetAttribute("value");
                                    }

                                    foreach (var xe4 in node.ParentNode)
                                    {
                                        var i = xe4;
                                    }
                                }
                                break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("LanguageXML Config is error,Please check it!\n" + e.Message);
                this.Close();
            }
        }
        #endregion
    }
}
