using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.Charting;
//using Telerik.QuickStart.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Xml;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class ToolManageForm : BaseForm
    {
        private List<DrillTools> listDrill;
        private DrillOSEntities db;
        private int DrillID = -1;
        private string username = "";
        private List<string> list_hole = new List<string>();//装取gvw_hole标题字段
        private List<string> list_rack = new List<string>();//装取gvw_ravk标题字段
        private List<string> list_laydown = new List<string>();//装取gvw_laydown标题字段
        private List<string> lbl_message = new List<string>();//装翻译数据
        private List<DrillTools> list_tools=new List<DrillTools>();
        //构造函数
        public ToolManageForm()
        {
            InitializeComponent();
            listDrill = new List<DrillTools>();
            DrillID = AppDrill.DrillID;
            username = AppDrill.username;
            db = new DrillOSEntities();
            //初始化日期
            txt_date.Text = DateTime.Now.ToString("yyyy-MM-dd");
            setControlLanguage();

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
                            if (xe.GetAttribute("key") == "lbl_message")
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
                                    #region 单独针对panel
                                    if (c.Name == "rpnl_info" || c.Name == "pnl_SlipStatus" || c.Name == "pnl_Menu")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            foreach (Control ctl in c.Controls)
                                            {
                                                if (ctl.Name == xe2.GetAttribute("key"))
                                                {
                                                    ctl.Text = xe2.GetAttribute("value");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    else if (c.Name == "gvw_hole" || c.Name == "gvw_rack" || c.Name == "gvw_laydown")
                                    {
                                        switch (c.Name)
                                        {
                                            case "gvw_hole":
                                                XmlNodeList xn_hole = node.ChildNodes;//寻找control下面的control
                                                foreach (XmlNode node2 in xn_hole)
                                                {
                                                    XmlElement xe2 = (XmlElement)node2;
                                                    list_hole.Add(xe2.GetAttribute("value"));
                                                }
                                                break;
                                            case "gvw_rack":
                                                XmlNodeList xn_rack = node.ChildNodes;//寻找control下面的control
                                                foreach (XmlNode node2 in xn_rack)
                                                {
                                                    XmlElement xe2 = (XmlElement)node2;
                                                    list_rack.Add(xe2.GetAttribute("value"));
                                                } break;
                                            case "gvw_laydown":
                                                XmlNodeList xn_laydown = node.ChildNodes;//寻找control下面的control
                                                foreach (XmlNode node2 in xn_laydown)
                                                {
                                                    XmlElement xe2 = (XmlElement)node2;
                                                    list_laydown.Add(xe2.GetAttribute("value"));
                                                }
                                                break;
                                        }
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
            catch { }
        }
        #endregion

        public void ToolManageForm_Load(object sender, EventArgs e)
        {
            //设置控件的初始值
            setView();
            //设置GridView的显示
            setGridView();
            //设置箭头的显示
            setbtnVisible();
            InitPermission();
            //初始化基础数据
            InitBasicData();
        }
        //初始化基础数据
        private void InitBasicData()
        {
            try
            {                
                txt_bha_name.Text = AppDrill.BHAName;
                txt_bha_length.Text = AppDrill.BHALength;
                txt_length.Text = AppDrill.Length;
                txt_comment.Text = AppDrill.Comment;
            }
            catch { }
        }
        #region 初始化各gridview
        private void setView()
        {

            #region 初始化页面的文本框的显示
            try
            {
                var data = db.Drill.Where(o => o.ID == DrillID).FirstOrDefault();
                txt_well_name.Text = data.DrillNo;
            }
            catch { }
            #endregion

            #region 初始化gvw_hole
            this.gvw_hole.BackColor = Color.FromArgb(45, 45, 48);
            this.gvw_hole.ShowGroupPanel = false;
            this.gvw_hole.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvw_hole.MasterTemplate.AllowAddNewRow = false;
            this.gvw_hole.ReadOnly = true;
            //列设定初期化
            this.gvw_hole.DataSource = null;
            this.gvw_hole.TableElement.BeginUpdate();
            this.gvw_hole.MasterTemplate.Columns.Clear();
            //列设定
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[0], "gvwh_order"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[1], "gvwh_jl"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[2], "gvwh_standNo"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[3], "gvwh_standLength"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[4], "gvwh_pipeTotal"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[5], "gvwh_pipes"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[6], "gvwh_coment"));
            this.gvw_hole.Columns.Add(new GridViewTextBoxColumn(list_hole[7], "gvwh_time"));
            this.gvw_hole.Columns[0].Width = 100;
            this.gvw_hole.Columns[0].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_hole.Columns[1].Width = 100;
            this.gvw_hole.Columns[2].Width = 100;
            this.gvw_hole.Columns[3].Width = 100;
            this.gvw_hole.Columns[4].Width = 150;
            this.gvw_hole.Columns[5].Width = 100;
            this.gvw_hole.Columns[6].Width = 200;
            this.gvw_hole.Columns[7].Width = 250;
            #endregion

            #region 初始化gvw_rack
            this.gvw_rack.BackColor = Color.FromArgb(45, 45, 48);
            this.gvw_rack.ShowGroupPanel = false;
            this.gvw_rack.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvw_rack.MasterTemplate.AllowAddNewRow = false;
            this.gvw_rack.ReadOnly = true;
            //列设定初期化
            this.gvw_rack.DataSource = null;
            this.gvw_rack.TableElement.BeginUpdate();
            this.gvw_rack.MasterTemplate.Columns.Clear();
            //列设定
            this.gvw_rack.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_rack[0], "gvwr_order"));
            this.gvw_rack.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_rack[1], "gvwr_jl"));
            this.gvw_rack.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_rack[2], "gvwr_standNo"));
            this.gvw_rack.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_rack[3], "gvwr_standLength"));
            this.gvw_rack.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_rack[4], "gvwr_coment"));
            this.gvw_rack.MasterTemplate.Columns[0].Width = 80;
            this.gvw_rack.MasterTemplate.Columns[0].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_rack.MasterTemplate.Columns[1].Width = 80;
            this.gvw_rack.MasterTemplate.Columns[1].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_rack.MasterTemplate.Columns[2].Width = 80;
            this.gvw_rack.MasterTemplate.Columns[2].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_rack.MasterTemplate.Columns[3].Width = 120;
            this.gvw_rack.MasterTemplate.Columns[3].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_rack.MasterTemplate.Columns[4].Width = 120;
            this.gvw_rack.MasterTemplate.Columns[4].TextAlignment = ContentAlignment.MiddleCenter;
            #endregion

            #region 初始化gvw_laydown
            this.gvw_laydown.BackColor = Color.FromArgb(45, 45, 48);
            this.gvw_laydown.ShowGroupPanel = false;
            this.gvw_laydown.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvw_laydown.MasterTemplate.AllowAddNewRow = false;
            this.gvw_laydown.ReadOnly = true;
            //列设定初期化
            this.gvw_laydown.DataSource = null;
            this.gvw_laydown.TableElement.BeginUpdate();
            this.gvw_laydown.MasterTemplate.Columns.Clear();
            //列设定
            this.gvw_laydown.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_laydown[0], "gvwl_order"));
            this.gvw_laydown.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_laydown[1], "gvwl_jl"));
            this.gvw_laydown.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_laydown[2], "gvwl_standNo"));
            this.gvw_laydown.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_laydown[3], "gvwl_standLength"));
            this.gvw_laydown.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(list_laydown[4], "gvwl_coment"));
            this.gvw_laydown.MasterTemplate.Columns[0].Width = 80;
            this.gvw_laydown.MasterTemplate.Columns[0].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_laydown.MasterTemplate.Columns[1].Width = 80;
            this.gvw_laydown.MasterTemplate.Columns[1].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_laydown.MasterTemplate.Columns[2].Width = 80;
            this.gvw_laydown.MasterTemplate.Columns[2].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_laydown.MasterTemplate.Columns[3].Width = 120;
            this.gvw_laydown.MasterTemplate.Columns[3].TextAlignment = ContentAlignment.MiddleCenter;
            this.gvw_laydown.MasterTemplate.Columns[4].Width = 120;
            this.gvw_laydown.MasterTemplate.Columns[4].TextAlignment = ContentAlignment.MiddleCenter;
            #endregion
        }
        #endregion

        private void setGridView()
        {
            try
            {
                listDrill = db.DrillTools.Where(o => o.DrillId == DrillID).ToList();
                initGridView();
            }
            catch { }
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            try
            {
                //查询当前的laydown的数据
                var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 0).ToList();
                if (numdata.Count != 0)
                {

                    //获取选中的项的group值，转化为int形的
                    int num = (int)Decimal.Parse(gvw_laydown.Rows[gvw_laydown.CurrentRow.Index].Cells[2].Value.ToString());
                    //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                    var query = db.DrillTools.Where(o => (int)o.Group == num && o.status == 0 && o.DrillId == DrillID).ToList();

                    //修改两个界面的内容
                    DrillTools d = new DrillTools();
                    var data = db.DrillTools.Where(o => o.status == 1 && o.DrillId == DrillID).ToList();
                    //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                    for (int i = 0; i < 3; i++)
                    {
                        decimal number = (decimal)query[i].Group;
                        d = db.DrillTools.Where(o => o.Group == number && o.status == 0 && o.DrillId == DrillID).FirstOrDefault();
                        d.status = 1;
                        d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                        //修改为当前的时间
                        d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        db.SaveChanges();
                    }
                    //刷新页面
                    setGridViewdata(0);
                    ToolManageForm_Load(sender, e);
                }
            }
            catch { }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                //判断txt_length里面只能输入数字
                decimal a = Convert.ToDecimal(decimal.Parse(txt_length.Text));
                //查询当前的status = 0 的条数，好计算group的值
                var query = db.DrillTools.Where(o => o.status == 0 && o.DrillId == DrillID).ToList();
                //查询当前井号下的所有数据，来操作order数据
                var data = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic != 0).ToList();
                //点击这个按钮之后向这个控件里添加数据
                DrillTools d = new DrillTools();
                for (int i = 0; i < 3; i++)
                {
                    d.DrillId = DrillID;
                    d.Name = txt_well_name.Text.ToString();
                    d.Length = decimal.Parse(txt_length.Text);
                    d.Comment = txt_comment.Text;
                    //判断是否已经有基础数据了
                    //如果当前的数据大于1
                    int number = data.Count;
                    if (number > 1)
                    {
                        d.order = data[number - 1].order + i + 1;
                    }
                    else
                    {
                        d.order = data.Count + i + 1;
                    }
                    d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    d.Unit = txt_length_unit.Text;
                    d.Group = ((query.Count) / 3 + 1) + i % 3 * 0.1M;
                    d.Basic = 1;
                    d.status = 0;
                    d.dataMakeTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    d.dataMakePGM = "ToolManage";
                    d.dataMakeUser = username;
                    d.dataUpdTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    d.dataUpdPGM = "ToolManage";
                    d.dataUpdUser = username;

                    db.DrillTools.Add(d);
                    db.SaveChanges();
                }
                setGridViewdata(0);
                //刷新页面
                ToolManageForm_Load(sender, e);
            }
            catch
            {
                //弹出 在BHA length里面只能输入数字 的对话框
                MessageBox.Show(lbl_message[0], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            try
            {
                //删除laydrow的内容
                //查询当前的laydown的数据
                var data = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 0).ToList();
                //判断是否有数据再删除
                if (data.Count != 0)
                {
                    //获取选中的项
                    int num = (int)Decimal.Parse(gvw_laydown.Rows[gvw_laydown.CurrentRow.Index].Cells[2].Value.ToString());
                    //linq的模糊查询，查出这三天数据，然后放到下一个gridview里面去
                    var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 0 && o.DrillId == DrillID).ToList();
                    foreach (var r in datalist)
                    {
                        db.DrillTools.Remove(r);
                        db.SaveChanges();
                    }
                    setGridViewdata(0);
                    //更新界面
                    ToolManageForm_Load(sender, e);
                }
            }
            catch { }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            try
            {
                //查询当前的laydown的数据
                var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 1).ToList();
                //判断是否有数据再删除
                if (numdata.Count != 0)
                {
                    //获取选中的项的group值，转化为int形的
                    int num = (int)Decimal.Parse(gvw_rack.Rows[gvw_rack.CurrentRow.Index].Cells[2].Value.ToString());
                    //linq的模糊查询，查出这三天数据，然后放到下一个gridview里面去
                    var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 1 && o.DrillId == DrillID).ToList();
                    DrillTools d = new DrillTools();
                    var data = db.DrillTools.Where(o => o.status == 0 && o.DrillId == DrillID).ToList();
                    for (int i = 0; i < 3; i++)
                    {
                        //获取当前的group的查询的条件
                        decimal number = (decimal)datalist[i].Group;
                        d = db.DrillTools.Where(o => o.Group == number && o.status == 1 && o.DrillId == DrillID).FirstOrDefault();
                        d.status = 0;
                        d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                        //修改为当前的时间
                        d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        db.SaveChanges();
                    }
                    //刷新页面
                    setGridViewdata(1);
                    ToolManageForm_Load(sender, e);
                }
            }
            catch { }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断有没有基础数据
                var query = db.DrillTools.Where(o => o.Basic == 0 && o.DrillId == DrillID).ToList();
                //如果没有数据，就弹出提示框：没有基础数据，请先更新基础数据！！
                if (query.Count == 0)
                {
                    MessageBox.Show(lbl_message[2], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                //如果有基础数据就正常操作
                else
                {
                    //查询当前的laydown的数据
                    var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 1).ToList();
                    if (numdata.Count != 0)
                    {

                        //获取选中的项的group值，转化为int形的
                        int num = (int)Decimal.Parse(gvw_rack.Rows[gvw_rack.CurrentRow.Index].Cells[2].Value.ToString());
                        //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                        var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 1 && o.DrillId == DrillID).ToList();
                        DrillTools d = new DrillTools();
                        var data = db.DrillTools.Where(o => o.status == 2 && o.DrillId == DrillID).ToList();
                        for (int i = 0; i < 3; i++)
                        {
                            //获取当前的group的查询的条件
                            decimal number = (decimal)datalist[i].Group;
                            d = db.DrillTools.Where(o => o.Group == number && o.status == 1 && o.DrillId == DrillID).FirstOrDefault();//.ToList().Last();   
                            d.status = 2;
                            d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                            //修改为当前的时间
                            d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            db.SaveChanges();
                        }
                        //刷新页面
                        setGridViewdata(1);
                        ToolManageForm_Load(sender, e);
                    }
                }
            }
            catch { }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                //查询当前的laydown的数据
                var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 2).ToList();
                //判断是否有数据再删除
                if (numdata.Count != 0)
                {
                    //获取选中的项的group值，转化为int形的
                    int num = (int)Decimal.Parse(gvw_hole.Rows[gvw_hole.CurrentRow.Index].Cells[2].Value.ToString());
                    //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                    var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 2 && o.Basic == 1 && o.DrillId == DrillID).ToList();
                    //这里判断不能选择那条order=0的基础数据
                    if (datalist.Count >= 3)
                    {
                        DrillTools d = new DrillTools();
                        for (int i = 0; i < 3; i++)
                        {
                            //获取当前的group的查询的条件
                            decimal number = (decimal)datalist[i].Group;
                            d = db.DrillTools.Where(o => o.Group == number && o.status == 2 && o.DrillId == DrillID).FirstOrDefault();//.ToList().Last();   
                            var data = db.DrillTools.Where(o => o.status == 1 && o.DrillId == DrillID).ToList();
                            d.status = 1;
                            d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                            //修改为当前的时间
                            d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //弹出 不能移动该条数据 的对话框
                        MessageBox.Show(lbl_message[3], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    }
                    //刷新页面
                    setGridViewdata(2);
                    ToolManageForm_Load(sender, e);
                }
            }
            catch { }
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            try
            {
                //首先判断有没有基础数据
                var query = db.DrillTools.Where(o => o.Basic == 0 && o.DrillId == DrillID).ToList();
                //如果没有数据，就弹出提示框：没有基础数据，请先更新基础数据！！
                if (query.Count == 0)
                {
                    MessageBox.Show(lbl_message[2], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                //如果有基础数据就正常操作
                else
                {
                    //查询当前的laydown的数据
                    var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 0).ToList();
                    //判断是否有数据再删除
                    if (numdata.Count != 0)
                    {
                        //获取选中的项的group值，转化为int形的
                        int num = (int)Decimal.Parse(gvw_laydown.Rows[gvw_laydown.CurrentRow.Index].Cells[2].Value.ToString());
                        //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                        var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 0 && o.DrillId == DrillID).ToList();
                        DrillTools d = new DrillTools();
                        var data = db.DrillTools.Where(o => o.status == 2 && o.DrillId == DrillID).ToList();
                        for (int i = 0; i < 3; i++)
                        {
                            //获取当前的group的查询的条件
                            decimal number = (decimal)datalist[i].Group;
                            d = db.DrillTools.Where(o => o.Group == number && o.status == 0 && o.DrillId == DrillID).FirstOrDefault();
                            d.status = 2;
                            d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                            //修改为当前的时间
                            d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            db.SaveChanges();
                        }
                        setGridViewdata(0);
                        ToolManageForm_Load(sender, e);
                    }
                }
            }
            catch { }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            try
            {
                //查询当前的laydown的数据
                var numdata = db.DrillTools.Where(o => o.DrillId == DrillID && o.Basic == 1 && o.status == 2).ToList();
                //判断是否有数据再删除
                if (numdata.Count != 0)
                {
                    //获取选中的项的group值，转化为int形的
                    int num = (int)Decimal.Parse(gvw_hole.Rows[gvw_hole.CurrentRow.Index].Cells[2].Value.ToString());
                    //linq的模糊查询，查出这三条数据，然后放到下一个gridview里面去
                    var datalist = db.DrillTools.Where(o => (int)o.Group == num && o.status == 2 && o.Basic == 1 && o.DrillId == DrillID).ToList();
                    //这里判断是否选中的是那条order=0的基础数据
                    if (datalist.Count >= 3)
                    {
                        DrillTools d = new DrillTools();
                        var data = db.DrillTools.Where(o => o.status == 0 && o.DrillId == DrillID).ToList();
                        for (int i = 0; i < 3; i++)
                        {
                            //获取当前的group的查询的条件
                            decimal number = (decimal)datalist[i].Group;
                            d = db.DrillTools.Where(o => o.Group == number && o.status == 2 && o.DrillId == DrillID).FirstOrDefault();
                            d.status = 0;
                            d.Group = data.Count / 3 + 1 + i % 3 * 0.1M;
                            //修改为当前的时间
                            d.Date = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        //弹出 不能移动该条数据 的对话框
                        MessageBox.Show(lbl_message[3], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    }
                    //刷新页面
                    setGridViewdata(2);
                    ToolManageForm_Load(sender, e);
                }
            }
            catch { }
        }

        private void initGridView()
        {
            //装载gvw_laydown
            var list_laydown = listDrill.Where(O => O.status == 0).OrderBy(o => o.Group).ToList();
            int c = 0;
            for (int i = this.gvw_laydown.Rows.Count; i < list_laydown.Count; i++)
            {
                this.gvw_laydown.Rows.AddNew();
            }

            //修改order是3的倍数的数据的颜色
            for (int i = 0; i < list_laydown.ToList().Count; i++)
            {
                if (i % 3 == 0)
                {
                    for (int j = 0; j < gvw_laydown.Columns.Count; j++)
                    {
                        this.gvw_laydown.Rows[i].Cells[j].Style.ForeColor = Color.YellowGreen;
                    }
                }
            }
            for (int i = list_laydown.Count; i > 0; i--)
            {
                this.gvw_laydown.Rows[c].Cells[0].Value = list_laydown[i - 1].order;
                this.gvw_laydown.Rows[c].Cells[1].Value = list_laydown[i - 1].Length;
                this.gvw_laydown.Rows[c].Cells[2].Value = list_laydown[i - 1].Group;
                this.gvw_laydown.Rows[c].Cells[3].Value = list_laydown[i - 1].Length * 3;
                this.gvw_laydown.Rows[c].Cells[4].Value = list_laydown[i - 1].Comment;
                this.gvw_laydown.MasterTemplate.Rows[c].Height = gvw_laydown.Rows[c].Height - 1;
                c++;
            }
            //装载gvw_rack数据
            var list_rack = listDrill.Where(o => o.status == 1).OrderBy(o => o.Group).ToList();

            int a = 0;
            for (int i = this.gvw_rack.Rows.Count; i < list_rack.Count; i++)
            {
                this.gvw_rack.Rows.AddNew();
            }
            //修改order是3的倍数的数据的颜色
            for (int i = 0; i < list_rack.ToList().Count; i++)
            {
                if (i % 3 == 0)
                {
                    for (int j = 0; j < gvw_rack.Columns.Count; j++)
                    {
                        this.gvw_rack.Rows[i].Cells[j].Style.ForeColor = Color.YellowGreen;
                    }
                }
            }
            for (int i = list_rack.Count; i > 0; i--)
            {
                this.gvw_rack.Rows[a].Cells[0].Value = list_rack[i - 1].order;
                this.gvw_rack.Rows[a].Cells[1].Value = list_rack[i - 1].Length;
                this.gvw_rack.Rows[a].Cells[2].Value = list_rack[i - 1].Group;
                this.gvw_rack.Rows[a].Cells[3].Value = list_rack[i - 1].Length * 3;
                this.gvw_rack.Rows[a].Cells[4].Value = list_rack[i - 1].Comment;
                this.gvw_rack.MasterTemplate.Rows[a].Height = gvw_rack.Rows[a].Height - 1;
                a++;
            }

            //装载gvw_hole数据
            var list_hole = listDrill.Where(o => o.status == 2).OrderBy(o => o.Group).ToList();
            var basic = listDrill.Where(o => o.Basic == 0).FirstOrDefault();
            for (int i = this.gvw_hole.Rows.Count; i < list_hole.Count + 1; i++)
            {
                this.gvw_hole.Rows.AddNew();
            }
            //修改order是3的倍数的数据的颜色
            for (int i = 0; i < list_hole.ToList().Count; i++)
            {
                if (i % 3 == 0)
                {
                    for (int j = 0; j < gvw_hole.Columns.Count; j++)
                    {
                        this.gvw_hole.Rows[i].Cells[j].Style.ForeColor = Color.YellowGreen;
                    }
                }
            }
            for (int i = list_hole.Count - 1, b = 0; i >= 0; i--)
            {
                this.gvw_hole.Rows[i].Cells[0].Value = list_hole[b].order;
                this.gvw_hole.Rows[i].Cells[1].Value = list_hole[b].Length;
                this.gvw_hole.Rows[i].Cells[2].Value = list_hole[b].Group;
                if (i != list_hole.Count - 1)
                {
                    this.gvw_hole.Rows[i].Cells[3].Value = list_hole[b].Length * 3;
                }
                else
                {
                    this.gvw_hole.Rows[i].Cells[3].Value = list_hole[b].Length;
                }
                this.gvw_hole.Rows[i].Cells[4].Value = list_hole.Where(o => o.Basic != 0).Take(b).Select(o => o.Length).Sum();
                if (i != list_hole.Count - 1)
                    this.gvw_hole.Rows[i].Cells[5].Value = list_hole.Take(b + 1).Select(o => o.Length).Sum();
                else
                    this.gvw_hole.Rows[i].Cells[5].Value = basic.Length;
                this.gvw_hole.Rows[i].Cells[6].Value = list_hole[b].Comment;
                this.gvw_hole.Rows[i].Cells[7].Value = list_hole[b].Date;
                b++;
                this.gvw_hole.MasterTemplate.Rows[i].Height = gvw_hole.Rows[i].Height - 1;
            }
            this.gvw_hole.Refresh();
            this.gvw_laydown.Refresh();
            this.gvw_rack.Refresh();
        }

        //点击箭头时设置gridview的显示
        private void setGridViewdata(int numstatus)
        {
            try
            {
                //更新上一个gridView的显示的group
                var datafrom = db.DrillTools.Where(o => o.status == numstatus && o.Basic == 1 && o.DrillId == DrillID).ToList();
                //这里定义的两个值是作为处理standid的值的参数
                int numgroup = 0;
                int numstand = 1;
                for (int i = 0; i < datafrom.Count; i++)
                {
                    if (datafrom != null)
                    {
                        datafrom[i].Group = numstand + numgroup % 3 * 0.1M;
                        db.SaveChanges();
                    }
                    numgroup++;
                    if (numgroup >= 3)
                    {
                        numgroup = 0;
                        numstand++;
                    }
                }
            }
            catch { }
        }

        //点击这个按钮更新基础数据
        private void radButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    SetValue("BHAName", txt_bha_name.Text);
                    AppDrill.BHAName = txt_bha_name.Text;
                    SetValue("BHALength", txt_bha_length.Text);
                    AppDrill.BHALength = txt_bha_length.Text;
                    SetValue("Length", txt_length.Text);
                    AppDrill.Length = txt_length.Text;
                    SetValue("Comment", txt_comment.Text);
                    AppDrill.Comment = txt_comment.Text;
                }
                catch { }
                //判断输入框中输入的是设定的格式
                decimal a = Convert.ToDecimal(Decimal.Parse(txt_bha_length.Text));
                DrillTools data = new DrillTools();
                data = db.DrillTools.Where(o => o.Basic == 0 && o.DrillId == DrillID).FirstOrDefault();
                //如果查询到数据就做修改操作
                if (data != null)
                {
                    data.order = 0;
                    data.Name = txt_well_name.Text;
                    data.Length = Decimal.Parse(txt_bha_length.Text);
                    data.Date = DateTime.Parse(txt_date.Text);
                    data.Unit = txt_length_unit.Text;
                    db.SaveChanges();
                    ToolManageForm_Load(sender, e);
                }
                //如果没有查到数据就添加一条
                else
                {
                    DrillTools datalist = new DrillTools();
                    datalist.DrillId = DrillID;
                    datalist.Name = txt_well_name.Text.ToString();
                    datalist.Length = decimal.Parse(txt_bha_length.Text);
                    datalist.Comment = "BHA";
                    datalist.order = 0;
                    datalist.Date = DateTime.Parse(txt_date.Text);
                    datalist.Unit = txt_length_unit.Text;
                    datalist.Group = 0M;
                    datalist.Basic = 0;
                    datalist.status = 2;
                    datalist.dataMakeTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    datalist.dataMakePGM = "ToolManage";
                    datalist.dataMakeUser = username;
                    datalist.dataUpdTime = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    datalist.dataUpdPGM = "ToolManage";
                    datalist.dataUpdUser = username;

                    db.DrillTools.Add(datalist);
                    db.SaveChanges();

                    ToolManageForm_Load(sender, e);
                }
            }
            catch
            {
                //弹出 在BHA length里面只能输入数字 的对话框
                MessageBox.Show(lbl_message[0], lbl_message[1], MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
        }

        //点击时间的输入框，让输入框显示
        private void calendar_Click(object sender, EventArgs e)
        {
            radCalendar1.Visible = true;
        }

        //设置页面的txt_date的显示
        private void radCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            //当年份为1900 时， 不给txt_date赋值
            if (radCalendar1.SelectedDate.Year == 1900)
            {
            }
            else
            {
                //赋值
                txt_date.Text = radCalendar1.SelectedDate.ToString("yyyy-MM-dd");
            }
            //关闭日历
            radCalendar1.Visible = false;
        }

        //关闭日历
        private void radButton2_Click_1(object sender, EventArgs e)
        {
            radCalendar1.Visible = false;
        }

        //根据gridview的数据条数设置箭头的显示
        private void setbtnVisible()
        {
            try
            {
                //设置laydown的箭头的显示
                var dataone = db.DrillTools.Where(o => o.status == 0 && o.DrillId == DrillID).ToList();
                if (dataone.Count < 3)
                {
                    btn_top.Visible = false;
                    btn_left2.Visible = false;
                }
                else
                {
                    btn_left2.Visible = true;
                    btn_top.Visible = true;
                }
                //设置rack的箭头的显示
                var datatwo = db.DrillTools.Where(o => o.status == 1 && o.DrillId == DrillID).ToList();
                if (datatwo.Count < 3)
                {
                    btn_bottom.Visible = false;
                    btn_left.Visible = false;
                }
                else
                {
                    btn_bottom.Visible = true;
                    btn_left.Visible = true;
                }
                //设置hole的箭头的显示
                var datathree = db.DrillTools.Where(o => o.status == 2 && o.DrillId == DrillID).ToList();
                if (datathree.Count < 3)
                {
                    btn_right.Visible = false;
                    btn_right2.Visible = false;
                }
                else
                {
                    btn_right2.Visible = true;
                    btn_right.Visible = true;
                }
            }
            catch { }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            btn_export.Enabled = false;
            backgroundWorker2.WorkerSupportsCancellation = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                list_tools = db.DrillTools.ToList();
            }
            catch { }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btn_export.Enabled = true;
            try
            {
                if (list_tools.Count != 0)
                {
                    IWorkbook workbook = new XSSFWorkbook();//创建Workbook对象
                    ISheet sheet = workbook.CreateSheet("Hole");//创建工作表
                    ICell cell;
                    sheet.AutoSizeColumn(7, false);
                    //处理hole表
                    int i = 0;
                    IRow row = sheet.CreateRow(i);//在工作表中添加一行
                    i++;//创建一行++
                    for (int j = 0; j < list_hole.Count; j++)
                    {
                        cell = row.CreateCell(j);//在行中添加一列
                        cell.SetCellValue(list_hole[j]);//设置列的内容
                    }
                    List<DrillTools> Hole = list_tools.Where(o => o.status == 2).ToList();
                    for (int j = 0; j < Hole.Count; j++, i++)
                    {
                        row = sheet.CreateRow(i);
                        for (int k = 0; k < list_hole.Count; k++)
                        {
                            cell = row.CreateCell(k);
                            cell.SetCellValue(this.gvw_hole.Rows[j].Cells[k].Value == null ? "" : this.gvw_hole.Rows[j].Cells[k].Value.ToString());
                        }
                    }

                    //处理rack表
                    i = 0;
                    sheet = workbook.CreateSheet("Rack");//创建工作表
                    row = sheet.CreateRow(i);//在工作表中添加一行
                    i++;//创建一行++
                    for (int j = 0; j < list_rack.Count; j++)
                    {
                        cell = row.CreateCell(j);//在行中添加一列
                        cell.SetCellValue(list_rack[j]);//设置列的内容
                    }
                    List<DrillTools> Rack = list_tools.Where(o => o.status == 1).ToList();
                    for (int j = 0; j < Rack.Count; j++, i++)
                    {
                        row = sheet.CreateRow(i);
                        for (int k = 0; k < list_rack.Count; k++)
                        {
                            cell = row.CreateCell(k);
                            cell.SetCellValue(this.gvw_rack.Rows[j].Cells[k].Value == null ? "" : this.gvw_rack.Rows[j].Cells[k].Value.ToString());
                        }
                    }
                    //处理laydown表
                    i = 0;
                    sheet = workbook.CreateSheet("Laydown");//创建工作表
                    row = sheet.CreateRow(i);//在工作表中添加一行
                    i++;//创建一行++
                    for (int j = 0; j < list_laydown.Count; j++)
                    {
                        cell = row.CreateCell(j);//在行中添加一列
                        cell.SetCellValue(list_laydown[j]);//设置列的内容
                    }
                    List<DrillTools> Laydown = list_tools.Where(o => o.status == 0).ToList();
                    for (int j = 0; j < Laydown.Count; j++, i++)
                    {
                        row = sheet.CreateRow(i);
                        for (int k = 0; k < list_laydown.Count; k++)
                        {
                            cell = row.CreateCell(k);
                            cell.SetCellValue(this.gvw_laydown.Rows[j].Cells[k].Value == null ? "" : this.gvw_laydown.Rows[j].Cells[k].Value.ToString());
                        }
                    }

                    //路径选择
                    SaveFileDialog path = new SaveFileDialog();
                    path.FileName = "ToolManage_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    path.Filter = "(*.xlsx)|*.xlsx";
                    path.ShowDialog();
                    //生成文件流
                    FileStream file = new FileStream(path.FileName, FileMode.Create);
                    workbook.Write(file);
                    file.Close();
                }
            }
            catch { }
            backgroundWorker2.CancelAsync();
        }
        /// <summary>
        /// 权限配置
        /// </summary>
        private void InitPermission()
        {
            if (AppDrill.permissionId != 1 && AppDrill.permissionId != 2)
            {
                btn_add.Enabled = false;
                btn_remove.Enabled = false;
                btn_left.Enabled = false;
                btn_left2.Enabled = false;
                btn_right.Enabled = false;
                btn_right2.Enabled = false;
                btn_top.Enabled = false;
                btn_bottom.Enabled = false;
                rbtn_update.Enabled = false;
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
            if (xElem1 != null) xElem1.SetAttribute("value", AppValue);
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
    }
}