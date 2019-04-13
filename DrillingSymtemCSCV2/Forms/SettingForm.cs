using DrillingSymtemCSCV2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Xml;
using System.Threading;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class SettingForm : RadForm
    {
        private List<Drill> listDrill = null;
        private DrillOSEntities _db;
        private List<RadListDataItem> list_language = new List<RadListDataItem>();//存放language键值对
        private List<RadListDataItem> list_sys = new List<RadListDataItem>();//存放公英制键值对
        private string selectDrill;//存放选择井号的翻译
        private List<string> list_error = new List<string>();//存放错误信息
        List<DrillTag> AlldataList = new List<DrillTag>();
        List<TagDictionary> TagDictionary = new List<TagDictionary>();
        private string[] FormSet;
        public SettingForm()
        {
            _db = new DrillOSEntities();
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            setControlLanguage();
            FormSet = System.Configuration.ConfigurationManager.AppSettings["FormSet"].ToString().Split(',');//获取界面设置启用情况
            #region 异步加载数据
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.WorkerSupportsCancellation = true;    //声明是否支持取消线程
            backgroundWorker1.RunWorkerAsync(); //开始
            #endregion
            //权限设置
            if (AppDrill.permissionId != 1)
            {
                rbtn_addUser.Enabled = false;
                //rddl_addTags.Enabled = false;
                //rbtn_addTags.Enabled = false;
                rbtn_addWell.Enabled = false;
            }
            //界面是否启用设置
            if (AppDrill.FormSet.Contains("DrillingGasForm"))
            {
                check_gas.Checked = true;
            }
            if (AppDrill.FormSet.Contains("DirectionalForm"))
            {
                check_dir.Checked = true;
            }
            if (AppDrill.FormSet.Contains("ToolManageForm"))
            {
                check_tool.Checked = true;
            }
            if (AppDrill.FormSet.Contains("RotaForm"))
            {
                check_rota.Checked = true;
            }
        }

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
                        rdp_wellNo.SelectedIndex = i+1;//设置井切换框为当前井
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

        private void Remove_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            bool reset = false;//定义是否重新启动页面
            #region 语言设置
            try
            {
                RadListDataItem selectLanguage = (RadListDataItem)rdp_language.SelectedItem.Value;
                if (selectLanguage.Value.ToString() != AppDrill.language)
                {
                    SetValue("Language", selectLanguage.Value.ToString());
                    AppDrill.language = selectLanguage.Value.ToString();
                    reset = true;
                }
            }
            catch
            { }
            #endregion

            #region 选择切换井
            RadListDataItem selectedItem = (RadListDataItem)rdp_wellNo.SelectedItem.Value;//必须取value然后转换再获取value
            var DrillId = -1;
            if (int.TryParse(selectedItem.Value.ToString(), out DrillId))
            {
                try
                {
                    if (DrillId != -1 && DrillId != AppDrill.DrillID)
                    {
                        SetValue("DrillID", DrillId.ToString());
                        AppDrill.DrillID = DrillId;
                        CurrentDrill c=_db.CurrentDrill.FirstOrDefault();
                        if (c != null&&AppDrill.permissionId==1)
                        {
                            c.CurrentDirllId = DrillId;//修改当前井号
                        }
                        //切换井之后进行自动导入测点操作
                        List<DrillTag> tag_list = _db.DrillTag.Where(o => o.DrillId == DrillId).ToList();
                        //如果查询到换井之后没数据，那么自动导入1号井的测点数据
                        if (tag_list.Count == 0)
                        {
                            _db.Database.ExecuteSqlCommand(@"insert into DrillTag SELECT [DrillId]={0}
                                                                                              ,[Tag]
                                                                                              ,[DefaultFrom]
                                                                                              ,[DefaultTo]
                                                                                              ,[Unit]
                                                                                              ,[HValue]
                                                                                              ,[LValue]
                                                                                              ,[Type]
                                                                                              ,[AlarmFlag]
                                                                                              ,[IsBool]
                                                                                              ,[dataMakeTime]={1}
                                                                                              ,[dataMakeUser]={2}
                                                                                              ,[dataMakePGM]={3}
                                                                                              ,[dataUpdTime]={4}
                                                                                              ,[dataUpdUser]={5}
                                                                                              ,[dataUpdPGM]={6}
                                                                                          FROM [DrillOS].[dbo].[DrillTag] where DrillId=1", new object[] { DrillId, DateTime.Now, AppDrill.username, "AddTag", DateTime.Now, AppDrill.username, "AddTag" });
                        }
                        _db.SaveChanges();
                        reset = true;
                    }
                }
                catch { }
            }
            #endregion

            #region 公英制选择
            try
            {
                RadListDataItem selectedType = (RadListDataItem)rdp_sys.SelectedItem.Value;//必须取value然后转换再获取value
                if (selectedType.Value.ToString() != "-1" && selectedType.Value.ToString() != AppDrill.UnitFormat)
                {
                    RadListDataItem selectSys = (RadListDataItem)rdp_sys.SelectedItem.Value;
                    SetValue("System", selectSys.Value.ToString());
                    AppDrill.UnitFormat = selectedType.Value.ToString();
                    reset = true;
                }
            }
            catch { }
            #endregion
            #region 界面设置启用
            try
            {
                string formSet = "";
                if (check_dir.Checked)
                {
                    formSet += "DirectionalForm";
                }
                if (check_gas.Checked)
                {
                    if (string.IsNullOrEmpty(formSet))
                        formSet += "DrillingGasForm";
                    else
                        formSet += ",DrillingGasForm";
                }
                if (check_tool.Checked)
                {
                    if (string.IsNullOrEmpty(formSet))
                        formSet += "ToolManageForm";
                    else
                        formSet += ",ToolManageForm";
                }
                if (check_rota.Checked)
                {
                    if (string.IsNullOrEmpty(formSet))
                        formSet += "RotaForm";
                    else
                        formSet += ",RotaForm";
                }
                string old_formSet="";
                for (int i = 0; i < AppDrill.FormSet.Length; i++)
                {
                    if (i == 0)
                        old_formSet += AppDrill.FormSet[i];
                    else
                        old_formSet += ","+AppDrill.FormSet[i];
                }
                if (formSet != old_formSet)
                {
                    SetValue("FormSet", formSet);
                    AppDrill.FormSet = formSet.Split(',');
                    reset = true;
                }
            }
            catch { }
            #endregion
            this.Close();
            //判断是否应该进行重启
            if (reset)
            {
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
                DrillForm main = new DrillForm();
                main.Size = new System.Drawing.Size(1920, 1080);
                main.Show();
                //CirculateForm circulate = new CirculateForm();
                //circulate.WindowState = FormWindowState.Minimized;
                //circulate.Show();
                //DrillPVTForm pvt = new DrillPVTForm();
                //pvt.WindowState = FormWindowState.Minimized;
                //pvt.Show();
                //if (AppDrill.FormSet.Contains("DrillingGasForm"))
                //{
                //    DrillingGasForm gas = new DrillingGasForm();
                //    gas.WindowState = FormWindowState.Minimized;
                //    gas.Show();
                //}
                //if (AppDrill.FormSet.Contains("DirectionalForm"))
                //{
                //    DirectionalForm direction = new DirectionalForm();
                //    direction.WindowState = FormWindowState.Minimized;
                //    direction.Show();
                //}
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            UserManagement form = new UserManagement();
            form.Show();
        }

        #region 测点导入 保留实现方法
        private void radButton3_Click(object sender, EventArgs e)
        {
            try
            {
                FileStream fs = null;
                IWorkbook workbook = null;
                ISheet sheet = null;
                IRow ExcelRow = null;//行
                ICell ExcelCell = null;//列

                //初始化一个OpenFileDialog类
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "(*.xlsx)|*.xlsx|(*.xls)|*.xls";
                //判断用户是否正确的选择了文件
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {   //文件路径
                    string Path = fileDialog.FileName;
                    try
                    {
                        using (fs = System.IO.File.OpenRead(Path))
                        {
                            #region 判断版本
                            //2007版本及以上
                            if (Path.IndexOf(".xlsx") > 0)
                                workbook = new XSSFWorkbook(fs);
                            //2003版本
                            else if (Path.IndexOf(".xls") > 0)
                                workbook = new HSSFWorkbook(fs);
                            #endregion

                            int i = 1;
                            int NodataFlg = 0; //跳出循环的Flag
                            if (workbook != null)
                            {   //读取第一个sheet，当然也可以循环读取每个sheet
                                sheet = workbook.GetSheetAt(0);

                                //遍历所有数据

                                //下拉框内容
                                //RadListDataItem selectedItem = (RadListDataItem)rddl_addTags.SelectedItem.Value;//必须取value然后转换再获取value
                                RadListDataItem selectedItem=null;
                                var DrillId = -1;
                                if (int.TryParse(selectedItem.Value.ToString(), out DrillId))
                                {
                                    if (DrillId != -1)
                                    {
                                        while (NodataFlg != 1)
                                        {

                                            #region 判断第i行是否为空
                                            ExcelRow = sheet.GetRow(i);     //第i行
                                            //如果为null，证明已经没有可以读取的行
                                            if (ExcelRow == null)
                                                break;
                                            ExcelCell = ExcelRow.GetCell(0);//读取excel第i行的第1列数据（测点）

                                            if ((ExcelCell == null) || (ExcelCell.ToString() == ""))
                                            {
                                                break;
                                            }
                                            #endregion

                                            #region 数据插入
                                            var TransferType = sheet.GetRow(0).GetCell(1).ToString().Trim();//语言
                                            var Tag = sheet.GetRow(i).GetCell(0).ToString().Trim();//基础测点值必须存在，不然导入失败
                                            var TransferName = sheet.GetRow(i).GetCell(1) == null ? "" : sheet.GetRow(i).GetCell(1).ToString().Trim();        //装翻译的结果
                                            var drillTag = AlldataList.Where(o => o.Tag == Tag && o.DrillId == DrillId).FirstOrDefault();//在DrillTag中查询当前测点是否存在
                                            var tagdictionary = TagDictionary.Where(o => o.Basic == Tag && o.TransferType == TransferType).FirstOrDefault();//检索当前测点及语言类型是否存在
                                            //声明一堆需要插入数据的变量
                                            string vfrom = sheet.GetRow(i).GetCell(2) == null ? "" : sheet.GetRow(i).GetCell(2).ToString().Trim();
                                            string vto = sheet.GetRow(i).GetCell(3) == null ? "" : sheet.GetRow(i).GetCell(3).ToString().Trim();
                                            string unit = sheet.GetRow(i).GetCell(4) == null ? "" : sheet.GetRow(i).GetCell(4).ToString().Trim();
                                            string hvalue = sheet.GetRow(i).GetCell(5) == null ? "" : sheet.GetRow(i).GetCell(5).ToString().Trim();
                                            string lvalue = sheet.GetRow(i).GetCell(6) == null ? "" : sheet.GetRow(i).GetCell(6).ToString().Trim();
                                            string type = sheet.GetRow(i).GetCell(7) == null ? "" : sheet.GetRow(i).GetCell(7).ToString().Trim();
                                            string isbool = sheet.GetRow(i).GetCell(8) == null ? "" : sheet.GetRow(i).GetCell(8).ToString().Trim();
                                            DrillTag model = new DrillTag();
                                            TagDictionary tagdic = new TagDictionary();
                                            if (drillTag == null)
                                            {
                                                //添加到DrillTag表
                                                model.DrillId = DrillId;
                                                if (string.IsNullOrEmpty(Tag))
                                                {
                                                    i++;
                                                    continue;
                                                }
                                                else
                                                {
                                                    model.Tag = Tag;
                                                }
                                                if (string.IsNullOrEmpty(TransferName))
                                                {
                                                    i++;
                                                    continue;
                                                }
                                                if (!string.IsNullOrEmpty(vfrom))//判断vfrom值是否为空
                                                {
                                                    model.DefaultFrom = Convert.ToDecimal(vfrom);
                                                }
                                                else
                                                {
                                                    model.DefaultFrom = 0;
                                                }
                                                if (!string.IsNullOrEmpty(vto))//判断vto值是否为空
                                                {
                                                    model.DefaultTo = Convert.ToDecimal(vto);
                                                }
                                                else
                                                {
                                                    model.DefaultTo = 1000;
                                                }
                                                if (!string.IsNullOrEmpty(unit))//判断unit是否为空
                                                {
                                                    model.Unit = unit;
                                                }
                                                if (!string.IsNullOrEmpty(hvalue))//判断hvalue是否为空
                                                {
                                                    model.HValue = Convert.ToDecimal(hvalue);
                                                }
                                                if (!string.IsNullOrEmpty(lvalue))//判断lvalue是否为空
                                                {
                                                    model.LValue = Convert.ToDecimal(lvalue);
                                                }
                                                if (!string.IsNullOrEmpty(type))//判断type是否为空
                                                {
                                                    model.Type = type;
                                                }
                                                if (!string.IsNullOrEmpty(isbool))//判断type是否为空
                                                {
                                                    model.IsBool = bool.Parse(isbool);
                                                }
                                                else
                                                {
                                                    model.IsBool = false;
                                                }
                                                model.dataMakePGM = "Setting";
                                                model.dataMakeTime = DateTime.Now;
                                                model.dataMakeUser = AppDrill.username;
                                                model.dataUpdPGM = "Setting";
                                                model.dataUpdTime = DateTime.Now;
                                                model.dataUpdUser = AppDrill.username;
                                                _db.DrillTag.Add(model);
                                                //因为这个基础测点是新增的，字典表肯定不存在，直接插入
                                                tagdic.Basic = Tag;//基础测点
                                                tagdic.TransferType = TransferType;//语言类型
                                                tagdic.TagShowName = TransferName;//翻译结果
                                                tagdic.dataMakePGM = "Setting";
                                                tagdic.dataMakeTime = DateTime.Now;
                                                tagdic.dataMakeUser = AppDrill.username;
                                                tagdic.dataUpdPGM = "Setting";
                                                tagdic.dataUpdTime = DateTime.Now;
                                                tagdic.dataUpdUser = AppDrill.username;
                                                _db.TagDictionary.Add(tagdic);
                                            }
                                            else
                                            {
                                                //更新操作
                                                if (!string.IsNullOrEmpty(vfrom))//判断vfrom值是否为空
                                                {
                                                    drillTag.DefaultFrom = Convert.ToDecimal(vfrom);
                                                }
                                                if (!string.IsNullOrEmpty(vto))//判断vto值是否为空
                                                {
                                                    drillTag.DefaultTo = Convert.ToDecimal(vto);
                                                }
                                                if (!string.IsNullOrEmpty(unit))//判断unit是否为空
                                                {
                                                    drillTag.Unit = unit;
                                                }
                                                if (!string.IsNullOrEmpty(hvalue))//判断hvalue是否为空
                                                {
                                                    drillTag.HValue = Convert.ToDecimal(hvalue);
                                                }
                                                if (!string.IsNullOrEmpty(lvalue))//判断lvalue是否为空
                                                {
                                                    drillTag.LValue = Convert.ToDecimal(lvalue);
                                                }
                                                if (!string.IsNullOrEmpty(type))//判断type是否为空
                                                {
                                                    drillTag.Type = type;
                                                }
                                                drillTag.dataUpdPGM = "Setting";
                                                drillTag.dataUpdTime = DateTime.Now;
                                                drillTag.dataUpdUser = AppDrill.username;
                                                //因为基础测点表中存在此测点，所以需要判断字典表中是否存在次测点的翻译
                                                if (tagdictionary == null)
                                                {
                                                    tagdic.Basic = Tag;//基础测点
                                                    tagdic.TransferType = TransferType;//语言类型
                                                    tagdic.TagShowName = TransferName;//翻译结果
                                                    tagdic.dataMakePGM = "Setting";
                                                    tagdic.dataMakeTime = DateTime.Now;
                                                    tagdic.dataMakeUser = AppDrill.username;
                                                    tagdic.dataUpdPGM = "Setting";
                                                    tagdic.dataUpdTime = DateTime.Now;
                                                    tagdic.dataUpdUser = AppDrill.username;
                                                    _db.TagDictionary.Add(tagdic);
                                                }
                                                else
                                                {
                                                    tagdictionary.TagShowName = TransferName;//修改只用修改翻译结果就行了
                                                    tagdictionary.dataUpdPGM = "Setting";
                                                    tagdictionary.dataUpdTime = DateTime.Now;
                                                    tagdictionary.dataUpdUser = AppDrill.username;
                                                }
                                            }
                                            #endregion
                                             i++;
                                        }
                                        try
                                        {
                                            lbl_error.Text = AppDrill.message[6];//等待中
                                            lbl_error.ForeColor = Color.White;
                                            backgroundWorker2.WorkerSupportsCancellation = true;
                                            backgroundWorker2.RunWorkerAsync();//进行保存操作
                                        }
                                        catch (Exception)
                                        {
                                            MessageBox.Show(AppDrill.message[5]);  
                                        }
                                        
                                    }
                                    else
                                    {
                                        lbl_error.Text = list_error[2];//井号未选择
                                        lbl_error.ForeColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        lbl_error.Text = list_error[1];//保存失败
                        lbl_error.ForeColor = Color.Red;
                    }
                }
            }
            catch
            {
                MessageBox.Show(AppDrill.message[5]);
            }
            
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
                XmlNodeList xnl = xn.ChildNodes;//得到根节点下的所有子节点(Form-xxxForm)
                foreach (XmlNode x in xnl)
                {
                    if (x.Name == this.Name)//比较当前节点的名称是否是当前Form名称
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
                                    if (c.Name == "radPanel1" || c.Name == "radPanel3" || c.Name == "radPanel2" || c.Name == "rpal_formset" || c.Name=="rpnl_wits")
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

        private void rbtn_addWell_Click(object sender, EventArgs e)
        {
            AddWellForm addwell = new AddWellForm();
            addwell.Show();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _db.SaveChanges();
            }
            catch 
            {
                
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lbl_error.Text = list_error[0];//保存成功
            lbl_error.ForeColor = Color.Green;
            backgroundWorker2.CancelAsync();
        }

        private void rbtn_send_Click(object sender, EventArgs e)
        {
            WITSForm wits = new WITSForm();
            wits.Show();
        }
        //禁止窗口移动
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0xA1 && m.WParam.ToInt32() == 2)
                return; base.WndProc(ref m);
        }
    }
}
