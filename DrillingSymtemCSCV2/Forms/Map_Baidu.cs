using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CopyDataStruct;
using System.Xml;
namespace DrillingSymtemCSCV2.Forms
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Map_Baidu : Form
    {

        private DrillOSEntities _db;
        List<Drill> drillinfo;
        Dictionary<int, int> IDlist; //0702添加，数据库ID号和显示列表的对应关系
        private bool m_bIsHistory = false;

        public static ProcessStartInfo startInfo = new ProcessStartInfo();
        public static Process pro = new Process();

        public const int USER = 0x0400;
        public const int WM_VIDEO_TYPE = USER + 101;
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, StringBuilder lparam);

        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public Map_Baidu()
        {
            InitializeComponent();
            setControlLanguage();
            _db = new DrillOSEntities();
            drillinfo = new List<Drill>();
        }

        private void addDrillList()
        {
            var data = from p in drillinfo group p by p.Contractor into g select g;
            int index = 1;
            //190411修改，油服现场用户（权限为4）仅显示当前井的信息
            if (AppDrill.permissionId == 4)
            {
                var list = drillinfo.Where(o => o.isActive==true  && o.Contractor == AppDrill.realName).FirstOrDefault();
                if (list != null)
                {
                    IDlist.Add(index, list.ID);

                    listBox1.Items.Insert(index - 1, index + "." + list.Contractor + " " + list.DrillNo + "  " + list.Lease + "," + list.Country);
                }
                index++;
            }


            else
            {
                foreach (var item in data)
                {
                    //listBox1.Items.Add(index + "." + item.Key);
                    //在组里判断是否有激活的井，存在就添加到IDlist中,并更新listbox 
                    foreach (var drill in item)
                    {
                        if (drill.isActive==true)
                        {
                            IDlist.Add(index, drill.ID);
                            //listBox1.Items.RemoveAt(index - 1);
                            listBox1.Items.Insert(index - 1, index + "." + item.Key + " " + drill.DrillNo + "  " + drill.Lease + "," + drill.Country);
                        }
                    }
                    index++;
                }
            }
        }

        private void addDrillHisList()
        {
            int index = 1;

            if (AppDrill.permissionId == 4) drillinfo = drillinfo.Where(o => o.Contractor == AppDrill.realName).ToList();
            
            foreach (var item in drillinfo)
            {
                //listBox1.Items.Insert(index - 1, index + "." + item.Operator + " " + item.Lease + "  " + item.DateSpud + "," + item.DrillNo);
                listBox1.Items.Insert(index - 1, index + "." + item.Contractor + " " + item.DrillNo + "  " + item.Lease + "," + item.Country);
                IDlist.Add(index, item.ID);
                index++;
            }
        }

        private void Map_Baidu_Load(object sender, EventArgs e)
        {
            IDlist = new Dictionary<int, int>();
            try
            {
                drillinfo = _db.Drill.ToList();
                //以井队为字段生成组
                real_btn.Enabled = false;
                addDrillList();

                webBrowser1.Navigate(Application.StartupPath + "\\Map.html");
                webBrowser1.ObjectForScripting = this;
            }
            catch
            {
            }
        }


        private string[] unittrans(string str)
        {
            string[] redata = new string[2];
            try
            {
                string[] jingweidu = str.Split(',');
                string jingdu = jingweidu[1];
                string[] data = jingdu.Substring(1).Split('-');
                double x = Convert.ToDouble(data[0]) + Convert.ToDouble(data[1]) / 60;
                string weidu = jingweidu[0];
                string[] data1 = weidu.Substring(1).Split('-');
                double y = Convert.ToDouble(data1[0]) + Convert.ToDouble(data1[1]) / 60;
                redata[0] = x.ToString("#0.00");
                redata[1] = y.ToString("#0.00");
            }
            catch
            {

            }

            return redata;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //仅显示当前活动的井队
            foreach (Drill item in drillinfo)
            {
                if (item.isActive==true)
                {
                    string[] pt = unittrans(item.location);
                    webBrowser1.Document.InvokeScript("setLocation", new object[] { pt[0], pt[1], "  " + item.description });

                }

            }
            pictureBox1.Visible = false;
        }

        public static void changeVideo(string strValue, int iWellID)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is VideoForm)
                {
                    VideoForm videoForm = frm as VideoForm;
                    if (null != videoForm)
                    {
                        if (iWellID != videoForm.getWellID())
                        {
                            videoForm.setWellID(iWellID);
                            videoForm.stopVedio();
                            videoForm.setWellName(strValue);
                            videoForm.clearList();
                            videoForm.createThreadPlay();
                        }
                    }

                    break;
                }
            }
        }

        private void writeVideoConf(string strValue)
        {
            Configure conf = new Configure();
            conf.setFileName(AppDrill.videoConf);
            conf.WriteIniData("VideoCur", "wellName", ref strValue);
        }

        public static void sendMessgeToVideoProcess(string strValue, int iWellID, int iIndex)
        {
            PostMessage(pro.MainWindowHandle, WM_VIDEO_TYPE, iWellID, iIndex);
        }

        private void sendMessgeToVideoProcess(string strValue, int iWellID)
        {
            try
            {
                if (IntPtr.Zero == pro.MainWindowHandle)
                {
                    return;
                }

                writeVideoConf(strValue);
                PostMessage(pro.MainWindowHandle, WM_VIDEO_TYPE, iWellID, 0);
            }
            catch
            {
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("请选择一个数据源！");
                this.Show();
                return;
            }

            if (m_bIsHistory)
            {
                int iDrillID = 0;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm is HistoryDataForm)
                    {
                        HistoryDataForm history = (HistoryDataForm)frm;
                        iDrillID = IDlist[listBox1.SelectedIndex + 1];
                        history.selectHisDrill(iDrillID);
                        frm.BringToFront();
                        frm.WindowState = FormWindowState.Normal;
                        this.Hide();
                        this.Close();
                        return;
                    }
                }

                HistoryDataForm hisForm = new HistoryDataForm();
                hisForm.setHistoryValue(hisForm);
                hisForm.setBtnEnable(hisForm, false);
                iDrillID = IDlist[listBox1.SelectedIndex + 1];
                hisForm.selectHisDrill(iDrillID);
                hisForm.Size = new System.Drawing.Size(1920, 1080);
                hisForm.Tag = -1;
                hisForm.Show();
            }
            else
            {
                AppDrill.DrillID = IDlist[listBox1.SelectedIndex + 1];
                foreach (Form frm in Application.OpenForms)
                {
                    if (Convert.ToInt16(frm.Tag) == AppDrill.DrillID)
                    {
                        frm.BringToFront();
                        frm.WindowState = FormWindowState.Normal;
                        DrillForm drillForm = (DrillForm)frm;

                        if (null != drillForm)
                        {
                            //changeVideo(drillForm.getContractor(), AppDrill.DrillID);
                            sendMessgeToVideoProcess(drillForm.getContractor(), AppDrill.DrillID);
                        }

                        this.Hide();
                        this.Close();
                        return;
                    }
                }

                this.Hide();
                DrillForm drill = new DrillForm();
                drill.Size = new System.Drawing.Size(1920, 1080);
                drill.Location = new Point(0, 0);
                drill.Tag = AppDrill.DrillID;
                drill.m_iDrillID = AppDrill.DrillID;
                drill.setDrillID(AppDrill.DrillID);
                drill.Show();
                //changeVideo(drill.getContractor(), AppDrill.DrillID);
                sendMessgeToVideoProcess(drill.getContractor(), AppDrill.DrillID);
            }

            this.Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            //判断井是否被激活，没有置灰色
            e.DrawBackground();
            int index = e.Index + 1;

            if (IDlist.Keys.Contains(index))
            {
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(Color.White), e.Bounds);
            }
            else
            {
                e.Graphics.DrawString(((ListBox)sender).Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Gray), e.Bounds);
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;

            if (!IDlist.Keys.Contains(index + 1))
            {
                return;
            }

            var data = drillinfo.Where(p => p.ID == IDlist[index + 1]).FirstOrDefault();
            // string[] pt = unittrans(data.location);
            if (data != null)
            {
                webBrowser1.Document.InvokeScript("ChangeLocation", new object[] { index, data.description });
            }
        }

        private void real_btn_Click(object sender, EventArgs e)
        {
            m_bIsHistory = false;
            his_btn.Enabled = true;
            real_btn.Enabled = false;
            IDlist.Clear();
            listBox1.Items.Clear();
            addDrillList();
        }

        private void his_btn_Click(object sender, EventArgs e)
        {
            m_bIsHistory = true;
            his_btn.Enabled = false;
            real_btn.Enabled = true;
            listBox1.Items.Clear();
            IDlist.Clear();
            addDrillHisList();
        }

        private void cacel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Map_Baidu_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is DrillForm || frm is HistoryDataForm)
                {
                    return;
                }
            }

            BaseForm.modifyUserInfo(false);
            Process.GetCurrentProcess().Kill();
        }

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
    }
}
