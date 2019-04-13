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
    public partial class UserManagement : RadForm
    {
        private DrillOSEntities db;//数据库连接对象
        private List<User> user_list = new List<User>();//获取用户列表
        private List<string> message = new List<string>();
        public UserManagement()
        {
            InitializeComponent();
        }

        private void UserManagement_Load(object sender, EventArgs e)
        {
            setControlLanguage();
            db = new DrillOSEntities();//初始化
            try
            {
                backgroundWorker1.WorkerSupportsCancellation = true;//设置当前异步操作可取消
                backgroundWorker1.RunWorkerAsync();//开始执行异步
            }
            catch { }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                user_list = db.User.Where(o=>o.PermissionId!=1).ToList();//取出所有的用户信息，一般使用的人数很少
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.rgv_users.Rows.Clear();
            //设置RadGridView信息及赋值
            for (int i = 0; i < user_list.Count; i++)
            {
                this.rgv_users.Rows.AddNew();
                this.rgv_users.Rows[i].Cells[0].Value = user_list[i].username;//用户名
                this.rgv_users.Rows[i].Cells[1].Value = user_list[i].realName;//真实姓名
                this.rgv_users.Rows[i].Cells[2].Value = user_list[i].PermissionId == 1 ? "工程师" : user_list[i].PermissionId == 2 ? "司钻" : "游客";//权限
            }
            //表格头设置
            try
            {
                this.rgv_users.Columns[0].HeaderText = message[0];
                this.rgv_users.Columns[1].HeaderText = message[1];
                this.rgv_users.Columns[2].HeaderText = message[2];
            }
            catch { }
            backgroundWorker1.CancelAsync();//执行完成
        }

        private void rbtn_deleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                int p = rgv_users.SelectedRows[0].Index;
                if (p < 0)
                    return;
                DialogResult dr = MessageBox.Show(message[4], message[3], MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    db.User.Remove(user_list[p]);//移除当前元素
                    db.SaveChanges();
                    user_list.RemoveAt(p);
                    this.rgv_users.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < user_list.Count; i++)
                    {
                        this.rgv_users.Rows.AddNew();
                        this.rgv_users.Rows[i].Cells[0].Value = user_list[i].username;//用户名
                        this.rgv_users.Rows[i].Cells[1].Value = user_list[i].realName;//真实姓名
                        this.rgv_users.Rows[i].Cells[2].Value = user_list[i].PermissionId == 1 ? "工程师" : user_list[i].PermissionId == 2 ? "司钻" : "游客";//权限
                    }
                }
            }
            catch { }
        }
        private void rbtn_addUser_Click(object sender, EventArgs e)
        {
            try
            {
                AddUserForm addUser = new AddUserForm();
                addUser.ShowDialog();
                foreach (var u in addUser.uList)
                {
                    db.User.Add(u);
                    user_list.Add(u);
                }
                if (addUser.uList.Count > 0)
                {
                    db.SaveChanges();
                    this.rgv_users.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < user_list.Count; i++)
                    {
                        this.rgv_users.Rows.AddNew();
                        this.rgv_users.Rows[i].Cells[0].Value = user_list[i].username;//用户名
                        this.rgv_users.Rows[i].Cells[1].Value = user_list[i].realName;//真实姓名
                        this.rgv_users.Rows[i].Cells[2].Value = user_list[i].PermissionId == 1 ? "工程师" : user_list[i].PermissionId == 2 ? "司钻" : "游客";//权限
                    }
                }
            }
            catch { }
        }

        private void rbtn_refresh_Click(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker1.WorkerSupportsCancellation = true;//设置当前异步操作可取消
                backgroundWorker1.RunWorkerAsync();//开始执行异步
            }
            catch { }
        }

        private void rbtn_editUser_Click(object sender, EventArgs e)
        {
            int p = rgv_users.SelectedRows[0].Index;
            if (p < 0)
                return;
            EditUserForm edit = new EditUserForm();
            edit.user = user_list[p];
            edit.ShowDialog();
            if (edit.isEdit)
            {
                user_list[p] = edit.user;//更改用户
                try
                {
                    db.SaveChanges();
                    this.rgv_users.Rows.Clear();
                    //设置RadGridView信息及赋值
                    for (int i = 0; i < user_list.Count; i++)
                    {
                        this.rgv_users.Rows.AddNew();
                        this.rgv_users.Rows[i].Cells[0].Value = user_list[i].username;//用户名
                        this.rgv_users.Rows[i].Cells[1].Value = user_list[i].realName;//真实姓名
                        this.rgv_users.Rows[i].Cells[2].Value = user_list[i].PermissionId == 1 ? "工程师" : user_list[i].PermissionId == 2 ? "司钻" : "游客";//权限
                    }
                }
                catch { }
            }
        }
        #region 读取xml文件设置语言
        private void setControlLanguage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(@"..\..\bin\Debug\DrillOS_" + AppDrill.language + ".xml");//加载XML文件
                XmlNode xn = doc.SelectSingleNode("Form");//获取根节点
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
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
                            if (xe.GetAttribute("key") == "message")
                            {
                                XmlNodeList xn_m = xe.ChildNodes;
                                foreach (XmlNode node_m in xn_m)
                                {
                                    XmlElement mess = (XmlElement)node_m;
                                    message.Add(mess.GetAttribute("value"));
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
