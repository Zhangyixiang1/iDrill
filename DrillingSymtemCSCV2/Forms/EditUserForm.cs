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

namespace DrillingSymtemCSCV2.Forms
{
    public partial class EditUserForm : RadForm
    {
        public bool isEdit = false;//默认未进行编辑操作
        public User user { get; set; }  //传过来的用户
        private List<string> message_list = new List<string>();
        public EditUserForm()
        {
            InitializeComponent();
        }

        private void EditUserForm_Load(object sender, EventArgs e)
        {
            try
            {
                //基础属性设置
                setControlLanguage();
                txt_userName.Text = user.username;
                txt_RealName.Text = user.realName;
                rbtn_p1.Text = message_list[0];
                rbtn_p2.Text = message_list[1];
            }
            catch { }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void btn_ok_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_RealName.Text))
            {
                MessageBox.Show(message_list[2]);
                return;
            }
            if (rbtn_p2.Checked)
            {
                if (string.IsNullOrEmpty(txt_pwd.Text))
                {
                    MessageBox.Show(message_list[3]);
                    return;
                }
            }
            user.realName = txt_RealName.Text;
            user.password = Encrypt.MD5(txt_pwd.Text);
            user.PermissionId = rbtn_p1.Checked ? 3 : 2;//2司钻，3游客
            user.dataUpdPGM = "UpdateUser";
            user.dataUpdTime = DateTime.Now;
            user.dataUpdUser = AppDrill.username;
            isEdit = true;//进行编辑
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
            catch { }
        }
        #endregion
    }
}
