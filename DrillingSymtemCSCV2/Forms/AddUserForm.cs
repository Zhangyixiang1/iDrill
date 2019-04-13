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
    public partial class AddUserForm : RadForm
    {
        public List<User> uList = new List<User>();
        private List<string> lbl_message = new List<string>();//语言翻译Message
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            //设置语言
            setControlLanguage();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            this.Close();
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
                            if (xe.GetAttribute("key") == "lbl_Message")
                            {
                                XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                foreach (XmlNode node3 in xn_list2)
                                {
                                    XmlElement xe3 = (XmlElement)node3;
                                    lbl_message.Add(xe3.GetAttribute("value"));
                                }
                                continue;
                            }
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    if (c.Name == "radPanel")
                                    {
                                        XmlNodeList xn_list3 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node3 in xn_list3)
                                        {
                                            XmlElement xe3 = (XmlElement)node3;
                                            foreach (Control ctl1 in c.Controls)
                                            {
                                                #region 单独针对panel
                                                if (ctl1.Name == "panel")
                                                {
                                                    XmlNodeList xn_list2 = node3.ChildNodes;//寻找control下面的control
                                                    foreach (XmlNode node2 in xn_list2)
                                                    {
                                                        XmlElement xe2 = (XmlElement)node2;
                                                        foreach (Control ctl in ctl1.Controls)
                                                        {
                                                            if (ctl.Name == xe2.GetAttribute("key"))
                                                            {
                                                                if (ctl.Name == "gbx_permission")
                                                                {
                                                                    XmlNodeList xn_list4 = node2.ChildNodes;//寻找control下面的control
                                                                    foreach (XmlNode node4 in xn_list4)
                                                                    {
                                                                        XmlElement xe4 = (XmlElement)node4;
                                                                        foreach (Control cl in ctl.Controls)
                                                                        {
                                                                            if (cl.Name == xe4.GetAttribute("key"))
                                                                            {
                                                                                cl.Text = xe4.GetAttribute("value");
                                                                                continue;
                                                                            }
                                                                        }
                                                                    }
                                                                    break;
                                                                }
                                                                ctl.Text = xe2.GetAttribute("value");
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                    c.Text = xe.GetAttribute("value");//设置控件的Text
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch {  }
        }
        #endregion

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            try
            {
                var UserName = txt_LoginName.Text.Trim();
                var RealName = txt_RealName.Text.Trim();
                var PassWord = txt_PassWord.Text.Trim();
                var PassWord2 = txt_PassWordAgain.Text.Trim();
                if (string.IsNullOrEmpty(UserName))
                {
                    label1.Text = lbl_message[0];
                    return;
                }
                this.Cursor = Cursors.WaitCursor;//鼠标等待
                using (var dbContext = new DrillOSEntities())
                {
                    var user = dbContext.User.Where(o => o.username == UserName).FirstOrDefault();
                    if (user != null)
                    {
                        label1.Text = lbl_message[1];
                        this.Cursor = Cursors.Default;//鼠标正常状态
                        return;
                    }
                    if (string.IsNullOrEmpty(PassWord) || string.IsNullOrEmpty(PassWord2))
                    {
                        label1.Text = lbl_message[2];
                        this.Cursor = Cursors.Default;//鼠标正常状态
                        return;
                    }
                    if (PassWord != PassWord2)
                    {
                        label1.Text = lbl_message[3];
                        this.Cursor = Cursors.Default;//鼠标正常状态
                        return;
                    }
                    DateTime dt = DateTime.Now;
                    User UserModel = new User();
                    UserModel.username = UserName;
                    UserModel.password = Encrypt.MD5(PassWord);  //对密码进行Md5加密
                    UserModel.realName = RealName;
                    if (rbtn_p2.Checked)
                    {
                        UserModel.PermissionId = 2;
                    }
                    else
                    {
                        UserModel.PermissionId = 3;
                    }
                    UserModel.dataMakeTime = dt;
                    UserModel.dataMakePGM = "AddUserForm";
                    UserModel.dataMakeUser = AppDrill.username;
                    UserModel.dataUpdTime = dt;
                    UserModel.dataUpdPGM = "AddUserForm";
                    UserModel.dataUpdUser = AppDrill.username; ;
                    uList.Add(UserModel);
                }
                label1.Text = lbl_message[4];
                this.Cursor = Cursors.Default;//鼠标正常状态
            }
            catch { label1.Text = lbl_message[5]; }
        }
    }
}
