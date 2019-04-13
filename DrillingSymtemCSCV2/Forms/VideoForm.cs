using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class VideoForm : RadForm
    {
        private List<string> message_list = new List<string>();
        private bool m_isFull = false;
        private static IntPtr[] handle = new IntPtr[4];//存放播放句柄
        private List<string> m_strCameraidList = new List<string>();//存放cameraid
        private List<IntPtr> m_SessionIdList = new List<IntPtr>();//存放申请的session
        private List<Point> m_picLocaList = new List<Point>();
        private Size m_picSize = new Size();
        private Size m_formSize = new Size();
        private string m_wellName = "宏华油服28队";  //井队描述，后续从数据库字段获取
        private int m_iWellID = 0;
        private  System.Windows.Forms.Timer timer = null;
        private static bool m_bIsclose = false;
        private bool m_bPlay = false;
        private bool m_bSizeChange = false;
        private bool m_bRefreshOver = true;
        string m_strContent = string.Empty;
        private System.Windows.Forms.PictureBox m_curPictureBox = null;

        public VideoForm()
        {
            InitializeComponent();

            //try
            //{
            //    InitString();
            //    Write(m_strContent);
            //    HkAction.MainForm = this;
            //    HkAction.Close();
            //    m_bPlay = HkAction.Start();//进入系统自动初始化库   
            //    if (m_bPlay)
            //    {
            //        Write("系统自动初始化库成功。");
            //    }
            //}
            //catch (Exception ex)//出现异常则提示
            //{
            //    //MessageBox.Show("异常！" + ex.ToString(), "提示", MessageBoxButtons.OK);
            //    m_strContent = "系统自动初始化库异常！" + ex.ToString();
            //    Write(m_strContent);
            //}
        }

        private void InitString()
        {
            for (int i = 0; i < 10; ++i)
            {
                m_strContent += "##########";
            }
        }

        public  static void Write(string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                string DayFileName = System.Windows.Forms.Application.StartupPath + "\\" + "videolog" + "\\" + DateTime.Now.ToString("yyyyMMdd") + "log.txt";
                try
                {
                    using (StreamWriter sw = new StreamWriter(DayFileName, true))
                    {
                        //如果客户端不存在Log文件则创建它		
                        if (!File.Exists(DayFileName))
                        {
                            using (FileStream fs = File.Create(DayFileName)) { }
                        }

                        sw.WriteLine(DateTime.Now.ToString() + "  " + content);
                        sw.Flush();
                        sw.Close();
                    }
                }
                catch (Exception)
                {
                }
            }

        }

        public void setWellName(string strWellName)
        {
            m_wellName = strWellName;
        }

        public void setWellID(int iWellID)
        {
            m_iWellID = iWellID;
        }

        public int getWellID()
        {
            return m_iWellID;
        }

        private void createTimer()
        {
            if (null == timer)
            {
                //timer = new System.Windows.Forms.Timer();
                //if (null != timer)
                //{
                //    timer.Enabled = true;
                //    timer.Interval = 20 * 60 * 1000;
                //    timer.Tick += new System.EventHandler(timer_Tick);
                //}
            }
        }

        private  void timer_Tick(object sender, EventArgs e)
        {
            //refresh();
        }

        public void createThreadPlay()
        {
            Thread thPlay = new Thread(playVedio);
            thPlay.IsBackground = true;
            thPlay.Start();
        }

        private void closeVideoForm()
        {
            if (this.InvokeRequired)
            {
                Action actionDelegate = () =>
                    {
                        this.Hide();
                        close();
                        this.Close();
                    };

                this.Invoke(actionDelegate);
            }
        }

        private void VedioForm_Load(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    InitString();
                    Write(m_strContent);
                    HkAction.MainForm = this;
                    HkAction.Close();
                    m_bPlay = HkAction.Start();//进入系统自动初始化库   
                    if (m_bPlay)
                    {
                        Write("系统自动初始化库成功。");
                    }
                }
                catch (Exception ex)//出现异常则提示
                {
                    //MessageBox.Show("异常！" + ex.ToString(), "提示", MessageBoxButtons.OK);
                    m_strContent = "系统自动初始化库异常！" + ex.ToString();
                    Write(m_strContent);
                }

                setControlLanguage();
                InitPictureBox();
                createThreadPlay();
                createTimer();

                m_formSize.Width = this.Width;
                m_formSize.Height = this.Height;
                label1.Visible = true;
                label1.BackColor = Color.Transparent;
            }
            catch 
            { 
            }
        }

        private void playVedio()
        {
            if (label1.InvokeRequired)
            {
                Action actionDelegate = () => { 
                                                label1.Visible = true; 
                                                label1.Text = "正在连接......";
                                              };

                label1.Invoke(actionDelegate);
            }

            regist();//注册
            play();//播放

            if (label1.InvokeRequired)
            {
                Action actionDelegate = () => { 
                                                label1.Visible = false; 
                                                label1.Hide(); 
                                              };

                label1.Invoke(actionDelegate);
            }

            m_bIsclose = true;
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
                                continue;
                            }
                            if (xe.GetAttribute("key") == "lbl_message")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                foreach (XmlNode node3 in xn_list2)
                                {
                                    XmlElement xe3 = (XmlElement)node3;
                                    message_list.Add(xe3.GetAttribute("value"));
                                }
                                continue;
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
            catch 
            { 
            }
        }
        #endregion

        public static void close()
        {
            try
            {
                if (m_bIsclose)
                {
                    //if (HkAction.Close())//反初始化库
                    //{
                    //    Write("用户退出成功！");
                    //}
                    //else
                    //{
                    //    Write("用户退出失败！");
                    //}
                }
            }
            catch
            {
                Write("用户退出异常！");
            }
        }

        protected override void WndProc(ref  Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                this.Hide();
                return;
            }

            base.WndProc(ref m);
        }

        private void Vedio_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void InitPictureBox()
        {
            m_picSize.Width = pictureBox1.Width;
            m_picSize.Height = pictureBox1.Height;

            pictureBox1.Tag = 0;
            m_picLocaList.Add(pictureBox1.Location);
          
            pictureBox2.Tag = 1;
            m_picLocaList.Add(pictureBox2.Location);

            pictureBox3.Tag = 2;
            m_picLocaList.Add(pictureBox3.Location);

            pictureBox4.Tag = 3;
            m_picLocaList.Add(pictureBox4.Location);

            handle[0] = pictureBox1.Handle;
            handle[1] = pictureBox2.Handle; 
            handle[2] = pictureBox4.Handle;
            handle[3] = pictureBox3.Handle;
        }

        public void jsonHandle(string str)
        {
            string jsonStr = str;
            JObject jsonObj = JObject.Parse(jsonStr);
            JArray jar = JArray.Parse(jsonObj["cameraList"].ToString());
            int iCount = Int32.Parse(jsonObj["count"].ToString());

            for (int i = 0; i < iCount; i++)
            {
                JObject jObj = JObject.Parse(jar[i].ToString());//取第几个摄像头信息放入对象j

                if (jObj["deviceName"].ToString() == m_wellName)
                {
                    m_strCameraidList.Add(jObj["cameraId"].ToString());
                }
            }

        }

        private void regist()
        {
            //登陆
            try
            {
                if (HkAction.GetAccessToken() != null)
                {
                    m_strContent = "用户登录成功！";
                    Write(m_strContent);
                }
            }
            catch (Exception ex)
            {
                m_strContent = "用户登录异常！" + ex.ToString();
                Write(m_strContent);
            }

            //获取摄像头列表ID
            try
            {
                string getList = HkAction.playList();

                jsonHandle(getList);

                m_strContent = "获取摄像头列表ID";

                for (int i = 0; i < m_strCameraidList.Count; ++i)
                {
                    m_strContent += "(" + m_strCameraidList[i] + ")";
                }

                if (m_strCameraidList.Count <= 0)
                {
                    m_strContent += "失败！";
                }

                Write(m_strContent);
            }
            catch (Exception ex)//出现异常则提示
            {
                m_strContent = "获取摄像头列表ID！" + ex.ToString();
                Write(m_strContent);
            }
        }

        private void createSessions()
        {
            if (m_SessionIdList.Count > 0)
            {
                return;
            }

            for (int i = 0; i < m_strCameraidList.Count; i++)
            {
                m_SessionIdList.Add(HkAction.AllocSession());//申请会话，保存到列表中
            }
        }

        private void play()
        {
            try
            {
                createSessions();
                m_strContent = "播放视频";
                for (int i = 0; i < m_strCameraidList.Count; i++)
                {
                    m_bPlay = HkAction.Play(handle[i], m_strCameraidList[i], m_SessionIdList[i]);
                    m_strContent += "(" + m_bPlay + "," + i + ")" ;
                }

                if (m_strCameraidList.Count <= 0)
                {
                    m_strContent += "失败！";
                }


                Write(m_strContent);
            }
            catch
            {
                m_strContent = "播放视频异常！";
                Write(m_strContent);
            }

        }

        public void stopVedio()
        {
            //停止所有播放
            foreach (IntPtr temp in m_SessionIdList)
            {
                try
                {
                    m_bPlay = HkAction.Stop(temp);
                    if (m_bPlay)
                    {
                        m_strContent = "停止视频成功！";
                        Write(m_strContent);
                    }
                }
                catch (Exception ex)
                {

                    //MessageBox.Show("异常" + ex.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    m_strContent = "停止视频异常！" + ex.ToString();
                    Write(m_strContent);
                }
            }
        }

        private void refresh()
        {
            m_strContent = "开始刷新视频！";
            Write(m_strContent);

            stopVedio();

            try
            {
                for (int i = 0; i < m_strCameraidList.Count; i++)
                {
                    HkAction.Play(handle[i], m_strCameraidList[i], m_SessionIdList[i]);
                }
            }
            catch (Exception ex)
            {
                m_strContent = "刷新视频异常！" + ex.ToString();
                Write(m_strContent);
            }
        }

        private void createThreadRefresh()
        {
            if (m_bRefreshOver)
            {  
                Thread thRefresh = new Thread(refreshAll);
                thRefresh.IsBackground = true;
                thRefresh.Start();
            }
          
        }

        private void refreshAll()
        {
            m_bRefreshOver = false;
            Write("重新进行视频刷新操作！");

            try
            {
                close();
                clearList();

                HkAction.MainForm = this;
                m_bPlay = HkAction.Start();//进入系统自动初始化库  
 
                if (m_bPlay)
                {
                    Write("重新进行系统自动初始化库成功！");
                }

                playVedio();
            }
            catch (Exception ex)//出现异常则提示
            {
                m_strContent = "重新进行系统自动初始化库异常！" + ex.ToString();
                Write(m_strContent);
            }

            m_bRefreshOver = true;
        }


        private void playOne(int iIndex)
        {
            stopVedio();

            //重新开始播放
            try
            {
                HkAction.Play(handle[iIndex], m_strCameraidList[iIndex], m_SessionIdList[iIndex]);
            }
            catch
            {
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseEvent = (MouseEventArgs)e;
            if (mouseEvent.Button.ToString() == "Right")
            {
                return;
            }

            PictureBox pic = sender as PictureBox;
            
            if (null == pic)
            {
                return;
            }

            m_curPictureBox = pic;

            int index = (int)pic.Tag;

            if (!m_isFull)
            {
                setPicTotalSize();
                m_curPictureBox.BringToFront();
                m_isFull = true;
            }
            else
            {
                if (m_bSizeChange)
                {
                    setFourPicSize();
                    m_bSizeChange = false;
                }
                else
                {
                    setPicSize(index);
                }

                m_isFull = false;
            }
        }

        private void setPicSize(int index)
        {
            m_curPictureBox.Size = m_picSize;

            switch (index)
            {
                case 0:
                    m_curPictureBox.Location = m_picLocaList[index];
                    break;
                case 1:
                    m_curPictureBox.Location = m_picLocaList[index];
                    break;
                case 2:
                    m_curPictureBox.Location = m_picLocaList[index];
                    break;
                case 3:
                    m_curPictureBox.Location = m_picLocaList[index];
                    break;
                default:
                    break;

            }
        }

        private void setPicTotalSize()
        {
            int iX = pictureBox1.Location.X;
            int iY = pictureBox1.Location.Y;

            m_curPictureBox.Location = pictureBox1.Location;
            m_curPictureBox.Width = this.Width - iX;
            m_curPictureBox.Height = this.Height - iY;
        }

        private void setFourPicSize()
        {
            int iWidth = (this.Width - m_formSize.Width) / 2;
            int iHeight = (this.Height - m_formSize.Height) / 2;

            pictureBox1.Width = m_picSize.Width + iWidth;
            pictureBox1.Height = m_picSize.Height + iHeight;


            pictureBox2.Location = new Point(pictureBox2.Location.X, m_picLocaList[(int)pictureBox2.Tag].Y + iHeight);
            pictureBox2.Width = m_picSize.Width + iWidth;
            pictureBox2.Height = m_picSize.Height + iHeight;


            pictureBox3.Location = new Point(m_picLocaList[(int)pictureBox3.Tag].X + iWidth, m_picLocaList[(int)pictureBox3.Tag].Y + iHeight);
            pictureBox3.Width = m_picSize.Width + iWidth;
            pictureBox3.Height = m_picSize.Height + iHeight;


            pictureBox4.Location = new Point(m_picLocaList[(int)pictureBox4.Tag].X + iWidth, pictureBox4.Location.Y);
            pictureBox4.Width = m_picSize.Width + iWidth;
            pictureBox4.Height = m_picSize.Height + iHeight;

            m_picSize.Width = pictureBox1.Width;
            m_picSize.Height = pictureBox1.Height;

            m_picLocaList[(int)pictureBox2.Tag] = pictureBox2.Location;
            m_picLocaList[(int)pictureBox3.Tag] = pictureBox3.Location;
            m_picLocaList[(int)pictureBox4.Tag] = pictureBox4.Location;

            m_formSize.Width = this.Width;
            m_formSize.Height = this.Height;
        }

        private void VedioForm_SizeChanged(object sender, EventArgs e)
        {
            if (m_picLocaList.Count <= 0 ||this.Width <= 0 || this.Height <= 0)
            {
                return;
            }

            if (this.Width != m_formSize.Width || this.Height != m_formSize.Height)
            {
                m_bSizeChange = true;
            }

            if (m_isFull)
            {
                setPicTotalSize();
            }
            else
            {
                setFourPicSize();
            }
        }

        private void operater(int iCommmand)
        {
            if (!m_bRefreshOver)
            {
                return;
            }

            switch (iCommmand)
            {
                case 1:
                    play();
                    break;
                case 2:
                    label1.Visible = true;
                    label1.BringToFront();
                    createThreadRefresh();
                    break;
                case 3:
                    stopVedio();
                    break;
                case 4:
                    Thread thClose = new Thread(closeVideoForm);
                    thClose.IsBackground = true;
                    thClose.Start();
                    break;
                default:
                    break;

            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                this.TopMost = false;

                MenuForm menu = new MenuForm();
                if (null != menu)
                {
                    //Point pt = new Point(this.Location.X + 10, this.Location.Y + 30);
                    //menu.Location = pt;
                    menu.ShowDialog();
                    operater(menu.getCommand());
                }

                if (m_bRefreshOver)
                {
                    this.TopMost = true;
                }
            }
        }

        public void clearList()
        {
            m_SessionIdList.Clear();
            m_strCameraidList.Clear();
        }
    }

}
