using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Linq;
using System.Collections;
namespace DrillingSymtemCSCV2.Forms
{
    public partial class SettingForm2 : Telerik.WinControls.UI.RadForm
    {
        private List<Drill> listDrill = null;
        private DrillOSEntities _db;
        private List<RadListDataItem> list_language = new List<RadListDataItem>();//存放language键值对
        private List<RadListDataItem> list_sys = new List<RadListDataItem>();//存放公英制键值对
        private string selectDrill;//存放选择井号的翻译
        private List<string> list_error = new List<string>();//存放错误信息
        List<DrillTag> AlldataList = new List<DrillTag>();
        List<TagDictionary> TagDictionary = new List<TagDictionary>();
        private bool m_bReset = false;
        private bool m_bResetAll = false;
        private int m_iDrillID = 0;

        public SettingForm2()
        {
            _db = new DrillOSEntities();
            InitializeComponent();
        }

        public bool getResetMark()
        {
            return m_bReset;
        }

        public bool getResetAll()
        {
            return m_bResetAll;
        }

        public int getDrillID()
        {
            return m_iDrillID;
        }

        public void setDrillID(int iDrillID)
        {
            m_iDrillID = iDrillID;
        }

        private void SettingForm2_Load(object sender, EventArgs e)
        {
            setControlLanguage();
      
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            //权限设置
            if (AppDrill.permissionId != 1)
            {
                rdp_wellNo.Enabled = false;
                txt_contractor.Enabled = false;
                txt_country.Enabled = false;
                txt_lade.Enabled = false;
                txt_lamin.Enabled = false;
                txt_lase.Enabled = false;
                txt_lease.Enabled = false;
                txt_longde.Enabled = false;
                txt_longmin.Enabled = false;
                txt_longse.Enabled = false;
                txt_operator.Enabled = false;
                txt_rignum.Enabled = false;
                rbtn_laN.Enabled = false;
                rbtn_laS.Enabled = false;
                rbtn_longE.Enabled = false;
                rbtn_longW.Enabled = false;
                datetimepickerrelease.Enabled = false;
                datetimepickerspud.Enabled = false;
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
                    if (x.Name == "SettingForm")//比较当前节点的名称是否是当前Form名称
                    {
                        XmlNodeList xn_list = x.ChildNodes;//得到根节点下的所有子节点
                        foreach (XmlNode node in xn_list)
                        {
                            XmlElement xe = (XmlElement)node;//将节点转换为元素
                            //循环每个控件，设置当前语言应设置的值
                            foreach (Control c in this.Controls)
                            {
                                //判断当前Node的key是否是当前需要设置的控件名称
                                if (c.Name == xe.GetAttribute("key"))
                                {
                                    #region 单独针对panel
                                    if (c.Name == "radPanel1" || c.Name == "radPanel3" || c.Name == "radPanel2" || c.Name == "rpal_formset" || c.Name == "rpnl_wits")
                                    {
                                        XmlNodeList xn_list2 = node.ChildNodes;//寻找control下面的control
                                        foreach (XmlNode node2 in xn_list2)
                                        {
                                            XmlElement xe2 = (XmlElement)node2;
                                            foreach (Control ctl in c.Controls)
                                            {
                                                if (ctl.Name == xe2.GetAttribute("key"))
                                                {
                                                    switch (ctl.Name)
                                                    {
                                                        case "rdp_language":
                                                            XmlNodeList xn_list_language = node2.ChildNodes;
                                                            foreach (XmlNode node3 in xn_list_language)
                                                            {
                                                                XmlElement xe3 = (XmlElement)node3;
                                                                list_language.Add(new RadListDataItem(xe3.GetAttribute("value"), xe3.GetAttribute("key")));
                                                            }
                                                            break;
                                                        case "rdp_sys":
                                                            XmlNodeList xn_list_sys = node2.ChildNodes;
                                                            foreach (XmlNode node3 in xn_list_sys)
                                                            {
                                                                XmlElement xe3 = (XmlElement)node3;
                                                                list_sys.Add(new RadListDataItem(xe3.GetAttribute("value"), xe3.GetAttribute("key")));
                                                            }
                                                            break;
                                                        case "rdp_wellNo":
                                                            selectDrill = xe2.GetAttribute("value");
                                                            break;
                                                        default:
                                                            ctl.Text = xe2.GetAttribute("value");
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    if (c.Name == "lbl_error")
                                    {
                                        XmlNodeList xn_list_error = node.ChildNodes;
                                        foreach (XmlNode node3 in xn_list_error)
                                        {
                                            XmlElement xe3 = (XmlElement)node3;
                                            list_error.Add(xe3.GetAttribute("value"));
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
            catch { }
        }
        #endregion

        #region 异步加载数据
        //添加DoWork事件请求数据
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            listDrill = new List<Drill>();
            try
            {
                listDrill = _db.Drill.ToList();
                AlldataList = _db.DrillTag.ToList();
                TagDictionary = _db.TagDictionary.ToList();
            }
            catch { }
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SelectList();
            backgroundWorker1.CancelAsync();    //取消挂起的后台操作。
        }
        #endregion

        #region 钻井井号选择框

        private void SelectList()
        {
            try
            {
                var selectdata = new ArrayList();
                var selectdata2 = new ArrayList();
                selectdata.Add(new RadListDataItem(selectDrill, "-1"));
                foreach (var item in listDrill)
                {
                    selectdata.Add(new RadListDataItem(item.DrillNo, item.ID.ToString()));
                    selectdata2.Add(new RadListDataItem(item.DrillNo, item.ID.ToString()));
                }
                rdp_wellNo.DataSource = selectdata;//井号数据
                //rddl_addTags.DataSource = selectdata2;//测点数据
                rdp_language.DataSource = list_language;//语言数据
                for (int i = 0; i < list_language.Count; i++)
                {
                    if (list_language[i].Value.ToString() == AppDrill.language)
                    {
                        rdp_language.SelectedIndex = i;//设置语言设置框中的语言为当前语言
                    }
                }
                for (int i = 0; i < listDrill.Count; i++)
                {
                    if (listDrill[i].ID == AppDrill.DrillID)
                    {
                        rdp_wellNo.SelectedIndex = i + 1;//设置井切换框为当前井
                    }
                }
                rdp_sys.DataSource = list_sys;//公英制数据
                for (int i = 0; i < list_sys.Count; i++)
                {
                    if (list_sys[i].Value.ToString() == AppDrill.UnitFormat)
                    {
                        rdp_sys.SelectedIndex = i;//设置公英制为当前所在制
                    }
                }
            }
            catch { }
        }

        #endregion

        private void rdp_wellNo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            var data = _db.Drill.Where(p => p.DrillNo == rdp_wellNo.SelectedItem.Text).FirstOrDefault();
            if (data != null)
            {
                txt_lease.Text = data.Lease;
                txt_contractor.Text = data.Contractor;
                txt_operator.Text = data.Operator;
                txt_rignum.Text = data.RigNo;
                txt_country.Text = data.Country;
                datetimepickerspud.Value = DateTime.Parse(data.DateSpud);
                if (!string.IsNullOrEmpty(data.DateRelease)) datetimepickerrelease.Value = DateTime.Parse(data.DateRelease);
                //处理经纬度
                string location = data.location;
                if (!string.IsNullOrEmpty(location))
                {

                    string[] jingweidu = location.Split(',');
                    //纬度
                    string weidu = jingweidu[0];
                    string weidusymbol = weidu.Substring(0, 1);
                    if (weidusymbol == "N") rbtn_laN.Checked = true;
                    else if (weidusymbol == "S") rbtn_laS.Checked = true;
                    string[] weiduvalue = weidu.Split('-');
                    txt_lade.Text = weiduvalue[0].Substring(1);
                    txt_lamin.Text = weiduvalue[1];
                    txt_lase.Text = weiduvalue[2];
                    //经度
                    string jingdu = jingweidu[1];
                    string jindudusymbol = jingdu.Substring(0, 1);
                    if (jindudusymbol == "E") rbtn_longE.Checked = true;
                    else if (jindudusymbol == "W") rbtn_longW.Checked = true;
                    string[] jingduvalue = jingdu.Split('-');
                    txt_longde.Text = jingduvalue[0].Substring(1);
                    txt_longmin.Text = jingduvalue[1];
                    txt_longse.Text = jingduvalue[2];
                }

            }
        }

        private void saveDrill()
        {
            #region 保存信息
            try
            {
                var data = _db.Drill.Where(p => p.DrillNo == rdp_wellNo.SelectedItem.Text).FirstOrDefault();
                data.Lease = txt_lease.Text;
                data.Contractor = txt_contractor.Text;
                data.Operator = txt_operator.Text;
                data.RigNo = txt_rignum.Text;
                data.Country = txt_country.Text;
                data.DateSpud = datetimepickerspud.Value.ToString();
                data.dataUpdTime = DateTime.Now;

                if (string.IsNullOrEmpty(data.DateRelease))
                {
                    data.DateRelease = datetimepickerrelease.Value.ToString();
                }

                //构建经纬度字符串
                string weidusymbol, jingdusymbol;
                if (rbtn_laN.Checked)
                {
                    weidusymbol = "N";
                }
                else
                {
                    weidusymbol = "S";
                }
                if (rbtn_longW.Checked)
                {
                    jingdusymbol = "W";
                }
                else
                {
                    jingdusymbol = "E";
                }

                string weidu = weidusymbol + txt_lade.Text + "-" + txt_lamin.Text + "-" + txt_lase.Text;
                string jingdu = jingdusymbol + txt_longde.Text + "-" + txt_longmin.Text + "-" + txt_longse.Text;
                string jingweidu = weidu + "," + jingdu;
                data.location = jingweidu;
                _db.SaveChanges();
            }
            catch
            {
            }
            #endregion
        }

        private void rbtn_OK_Click(object sender, EventArgs e)
        {
            bool reset = false;//定义是否重新启动页面
            #region 语言设置
            try
            {
                RadListDataItem selectLanguage = (RadListDataItem)rdp_language.SelectedItem.Value;
                if (selectLanguage.Value.ToString() != AppDrill.language)
                {
                    SetValue("Language", selectLanguage.Value.ToString());
                    System.Configuration.ConfigurationManager.AppSettings["Language"] = selectLanguage.Value.ToString();
                    AppDrill.language = selectLanguage.Value.ToString();
                    reset = true;
                }
            }
            catch
            { }
            #endregion
            #region 公英制选择
            try
            {
                RadListDataItem selectedType = (RadListDataItem)rdp_sys.SelectedItem.Value;//必须取value然后转换再获取value
                if (selectedType.Value.ToString() != "-1" && selectedType.Value.ToString() != AppDrill.UnitFormat)
                {
                    RadListDataItem selectSys = (RadListDataItem)rdp_sys.SelectedItem.Value;
                    SetValue("System", selectSys.Value.ToString());
                    System.Configuration.ConfigurationManager.AppSettings["System"] = selectSys.Value.ToString();
                    AppDrill.UnitFormat = selectedType.Value.ToString();
                    reset = true;
                }
            }
            catch { }
            #endregion

            saveDrill();



            this.Close();
            //判断是否应该进行重启
            if (reset)
            {
                BaseForm.modifyUserInfo(false);
                List<Form> disForm = new List<Form>();
                int num = Application.OpenForms.Count;
                for (int i = 0; i < num; i++)
                {
                    Form f = Application.OpenForms[i];
                    if (f.Name != "LoginForm")
                    {
                        disForm.Add(f);
                    }
                }
                foreach (var item in disForm)
                {
                    item.Dispose();
                }
                AppDrill.message.Clear();
                AppDrill.Command.Clear();
                //开启全部带有数据显示的界面
                LoginForm frm = new LoginForm();
                 frm.Show();
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

        private void rbtn_Cancel_Click(object sender, EventArgs e)
        {
            m_bReset = false;
            this.Close();
        }

        private void rbtn_reset_Click(object sender, EventArgs e)
        {
            m_bReset = true;
            ResetForm resetForm = new ResetForm();
            resetForm.ShowDialog();

            m_bResetAll = resetForm.getResetValue();
            DialogResult dr = MessageBox.Show("是否要重置测点！", "提示", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
            {
                return;
            }
            else
            {
                this.Hide();
                this.Close();
            }
        }
    }
}
