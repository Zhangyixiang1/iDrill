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
using System.Xml;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class SelectNameForm : RadForm
    {
        private DrillOSEntities db;
        private List<Worker> list = new List<Worker>();
        private List<Button> btnlist=new List<Button>();
        private Worker worker;
        public string RotaName { get; set; } //用于传递用于设置的值班人员
        public int workType { get; set; }   //接收传递过来的当前需要的工作类型
        public List<WorkType> list_type { get; set; }//接收传递过来的工种类型
        private List<string> message_list = new List<string>();
        public SelectNameForm()
        {
            InitializeComponent();
        }

        private void SelectName_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            db = new DrillOSEntities();
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            setControlLanguage();
        }
        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                rlbl_selectRota.Text = list_type.Find(o => o.ID == workType).Type;
                list = db.Worker.Where(o => o.TypeWork == workType).ToList();
                btnlist = new List<Button>();
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //设置按钮相关属性并添加到TabPage上
            setButtons();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion
        //设置按钮属性
        private void setButtons()
        {
            int i = 0; 
            //循环设置按钮属性
            foreach (Worker u in list)
            {
                Button btn = new Button();
                btn.Location = new System.Drawing.Point(15 + 90 * (i % 6), 15 + 30 * (i / 6));
                btn.Size = new System.Drawing.Size(80, 25);
                btn.Text = u.Name;
                btn.Tag = u.ID;
                btn.BackColor = Color.Black;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                btn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.Click += new System.EventHandler(this.btn_Click);
                this.rpal_selectRota.Controls.Add(btn);
                btnlist.Add(btn);
                i++;
            }
        }
        #region 设置按钮监听事件
        private void btn_Click(object sender, EventArgs e)
        {
            int limitCount = 99;//获取当前工种限制人员数量
            int selectCount = btnlist.Where(o => o.BackColor == Color.Red).Count();//获取当前工种选中的人员数量
            
            try
            {
                Button b = (Button)sender;
                Color c = b.BackColor;//保存点击按钮的颜色
                worker = list.Where(o => o.ID == (int)b.Tag).FirstOrDefault();
                //取消所有的按钮选中状态
                if (c == Color.Black)
                {
                    //如果
                    if (selectCount >= limitCount)
                    {
                        MessageBox.Show(message_list[0].Replace("name", list_type.Find(o => o.ID == workType).Type).Replace("count", "99"));//工种替换name，数量替换number
                        return;
                    }
                    b.BackColor = Color.Red;
                }
                else
                {
                    b.BackColor = Color.Black;//如果重复点击了该按钮，则取消点击事件
                }
            }
            catch { }
        }
        #endregion

        #region 设置OK按钮点击事件
        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                bool AllBlack = true;//如果全黑，那么设置当前单元格内容为空
                foreach (Button b in btnlist)
                {
                    if (b.BackColor == Color.Red)
                    {
                        AllBlack = false;
                        RotaName += "," + b.Text;
                    }
                }
                if (AllBlack == true)
                {
                    RotaName = "";
                }
                else
                {
                    string result = RotaName.Substring(0, 1);
                    //判断第一个字符是不是“，”
                    if (result == ",")
                    {
                        RotaName = RotaName.Substring(1);//砍掉第一个字符
                    }
                }
                this.Close();
            }
            catch { }
        }
        #endregion

        #region 设置Cnacel按钮点击事件
        private void radButton2_Click(object sender, EventArgs e)
        {
            RotaName = null;
            this.Close();
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
                                continue;
                            }
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList xn_list2 = xe.ChildNodes;
                                foreach (XmlNode node2 in xn_list2)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    message_list.Add(xe2.GetAttribute("value"));
                                }
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
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
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
