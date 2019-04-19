using Apache.NMS.ActiveMQ;
using DrillingSymtemCSCV2.Forms;
using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2
{
    public partial class LoginForm : RadForm
    {
        DrillOSEntities _db;
        List<User> u_list;
        Configuration config;
        string dis2, dis3, dis4, cycletime; //显示配置信息
        List<DrillForm> displaylist;// 需要循环的窗体
        List<Point> Tvlist;// 活动的电视墙坐标
        private List<string> error_info = new List<string>();//定义错误提交信息
        public LoginForm()
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string cachePath = Path.Combine(appPath, "cache");
            if (!Directory.Exists(cachePath))
            {
                Directory.CreateDirectory(cachePath);
            }

            InitializeComponent();

            //ThemeResolutionService.LoadPackageFile("TMDBTheme.tssp");
            //ThemeResolutionService.ApplicationThemeName = "TMDBTheme";
            try
            {
                if (AppDrill.DrillID < 0)
                {
                    AppDrill.DrillID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DrillID"].ToString());
                }

                AppDrill.language = System.Configuration.ConfigurationManager.AppSettings["Language"].ToString();//初始化系统语言
                AppDrill.FormSet = System.Configuration.ConfigurationManager.AppSettings["FormSet"].ToString().Split(',');
                //读取config文件
                AppDrill.BHAName = System.Configuration.ConfigurationManager.AppSettings["BHAName"].ToString();
                AppDrill.BHALength = System.Configuration.ConfigurationManager.AppSettings["BHALength"].ToString();
                AppDrill.Length = System.Configuration.ConfigurationManager.AppSettings["Length"].ToString();
                AppDrill.Comment = System.Configuration.ConfigurationManager.AppSettings["Comment"].ToString();
                AppDrill.videoFileName = Application.StartupPath + "\\video\\CCTV.exe";//@"D:\myWork\Program\video\VideoWinFormsApp\VideoWinFormsApp\bin\Debug\VideoWinFormsApp.exe";//Application.StartupPath + "\\video\\VideoWinFormsApp.exe";
                AppDrill.videoConf = Application.StartupPath + "\\video\\VideoConfig.ini";//@"D:\myWork\Program\video\VideoWinFormsApp\VideoWinFormsApp\bin\Debug\VideoConfig.ini";//Application.StartupPath + "\\video\\VideoConfig.ini";
            }
            catch
            {
            }
        }

        /// <summary>
        /// 登录框border
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                                this.panelLogin.ClientRectangle,
                                Color.LightSeaGreen,//7f9db9
                                1,
                                ButtonBorderStyle.Solid,
                                Color.LightSeaGreen,
                                1,
                                ButtonBorderStyle.Solid,
                                Color.LightSeaGreen,
                                1,
                                ButtonBorderStyle.Solid,
                                Color.LightSeaGreen,
                                1,
                                ButtonBorderStyle.Solid);
        }

        /// <summary>
        /// 登录按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                btn_login.Enabled = false;
                backgroundWorker1.RunWorkerAsync(); //开始
            }
        }

        private bool isActiveUser(User user, out int ilogTimestamp)
        {
            int localTime = (int)(DateTime.Now - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
            ilogTimestamp = localTime;

            if (null == user.isActive || null == user.timeStamp)
            {
                return false;
            }

            if (user.isActive == true && (localTime - user.timeStamp) < 120)
            {
                return true;
            }

            return false;
        }

        private void modifyUserInfo(User user, int ilogTimestamp)
        {
            user.isActive = true;
            user.timeStamp = ilogTimestamp;
            _db.SaveChanges();
        }

        //登录方法
        public User Login(string loginName, string password)
        {
            User userinfo = new User();
            password = Encrypt.MD5(password);
            loginName = loginName.Trim();

            try
            {
                var user = u_list.Where(u => u.username.ToUpper() == loginName.ToUpper()).FirstOrDefault();

                if (password == user.password)
                {
                    return user;
                }
                else
                {
                    return null;
                }

                //只有admin和司钻才需要密码
                /*if (user.PermissionId == 1 || user.PermissionId == 2)
                {
                    if (password == user.password)
                    {
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (user != null)
                    {
                        //userinfo.username = user.username;
                        //userinfo.password = user.password;
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }*/
            }
            catch
            {
            }

            return null;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //0613修改，记忆帐户信息
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["Account"]))
            {
                txtUsername.Text = System.Configuration.ConfigurationManager.AppSettings["Account"];
            }

            //多语言对应
            setControlLanguage();
            _db = new DrillOSEntities();
            //190415新增，读取显示配置文件信息
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            dis2 = config.AppSettings.Settings["display2"].Value;
            dis3 = config.AppSettings.Settings["display3"].Value;
            dis4 = config.AppSettings.Settings["display4"].Value;
            cycletime = config.AppSettings.Settings["cycletime"].Value;
            displaylist = new List<DrillForm>();
            Tvlist = new List<Point>();
            if (dis2 == "循环") Tvlist.Add(new Point(1920 * 2, 0));
            if (dis3 == "循环") Tvlist.Add(new Point(1920, 1080));
            if (dis4 == "循环") Tvlist.Add(new Point(1920 * 2, 1080));


            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            #endregion
        }

        #region 异步加载数据

        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                u_list = _db.User.ToList();
            }
            catch { }
        }
        //添加RunWorkerCompleted事件，数据加载完后，操作控件赋值：
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_login.Enabled = true;
            if (u_list == null)
            {
                this.txt_loginfailed.Text = error_info[0];
                return;
            }

            string username = txtUsername.Text;
            string password = txtPassword.Text;
            User user = Login(username, password);
            if (user != null)
            {
                //该用户是否已经登录
                int ilogTimestamp = 0;
                if (isActiveUser(user, out ilogTimestamp))
                {
                    this.txt_loginfailed.Text = error_info[2];
                    return;
                }

                modifyUserInfo(user, ilogTimestamp);

                //登录成功将用户名保存在全局变量
                AppDrill.username = user.username;
                AppDrill.permissionId = (int)user.PermissionId;
                AppDrill.realName = user.realName;
                SetValue("Account", user.username);

                Map_Baidu frm = new Map_Baidu();
                frm.Size = new System.Drawing.Size(1920, 1080);
                frm.Show();

                //190411新增，permission为2的用户，电视上投影大数据界面和钻井界面的轮询
                if (AppDrill.permissionId == 2)
                {
                    Map_display frm_dis = new Map_display();
                    frm_dis.Location = new Point(1920, 0); //电视墙1号屏幕
                    frm_dis.Show();
                    //从数据获得当前井队的状态，并与配置文件中的显示设置比对，确定form的显示位置
                    var wellist = _db.Drill.Where(o => o.isActive == true).ToList();



                    foreach (var item in wellist)
                    {
                        DrillForm drill = new DrillForm();
                        drill.Size = new System.Drawing.Size(1920, 1080);
                        drill.Location = new Point(-1920, -1920); //默认的话给一个负坐标
                        drill.Tag = item.ID;
                        drill.m_iDrillID = item.ID;
                        drill.setDrillID(item.ID);
                        if (item.Contractor == dis2) drill.Location = new Point(1920 * 2, 0);//电视墙2号屏幕
                        else if (item.Contractor == dis3) drill.Location = new Point(1920, 1080);//电视墙3号屏幕
                        else if (item.Contractor == dis4) drill.Location = new Point(1920 * 2, 1080);//电视墙4号屏幕
                        else displaylist.Add(drill);
                        drill.Show();
                    }

                    //启动触发

                    timer_display.Interval = int.Parse(cycletime) * 1000;
                    timer_display.Enabled = true;


                }





                this.Hide();

                #region 界面是否自动启动

                #endregion
            }
            else
            {
                try
                {
                    this.txt_loginfailed.Text = error_info[1];
                }
                catch { }
                return;
            }
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();  //杀死进程
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
                            if (xe.GetAttribute("key") == "pnl_SlipStatus")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//到达节点Form-xxxForm-Control-Control
                                foreach (XmlNode node2 in xn_list2)
                                {
                                    XmlElement xe2 = (XmlElement)node2;
                                    AppDrill.slip_list.Add(xe2.GetAttribute("value"));
                                }
                                break;
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    //判断当前Node的key是否是当前需要设置的控件名称
                                    if (c.Name == "txt_loginfailed")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//到达节点Form-xxxForm-Control-Control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            error_info.Add(xe2.GetAttribute("value"));
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        c.Text = xe.GetAttribute("value");//设置控件的Text
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("LanguageXML Config is error,Please check it!\n" + e.Message);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        #endregion
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
            {
                return;
            }

            base.WndProc(ref m);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe");
            }
            catch
            {
            }
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
            if (xElem1 != null)
            {
                xElem1.SetAttribute("value", AppValue);
            }
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

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (_db = new DrillOSEntities())
            {
                var item = _db.User.Where(o => o.username == AppDrill.username).FirstOrDefault();
                item.isActive = false;
                _db.SaveChanges();
            }

        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {

        }
        int temp = 0; //控制井队循环的中间变量
        private void timer_display_Tick(object sender, EventArgs e)
        {


            foreach (var item in displaylist)
            {
                item.Location = new Point(-1920, -1920);
            }

            for (int i = 0; i < Tvlist.Count; i++)
            {
                if (temp * Tvlist.Count + i < displaylist.Count)
                    displaylist[temp * Tvlist.Count + i].Location = Tvlist[i];

            }

            temp++;
            if (temp > displaylist.Count / Tvlist.Count) temp = 0;

        }
    }
}
