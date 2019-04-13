using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using DrillingSymtemCSCV2.Model;
using Telerik.WinControls.UI;
using ZedGraph;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class HistoryDataPrtsc : RadForm
    {
        //数据传递
        public string drillNo;
        public string lease;
        public string company;
        public string country;
        public string contractor;
        public string dateSpud;
        public string toolPusher;
        public string daterelease;
        public string companyman;
        public string startTime;
        public string endTime;
        public string tag1;
        public string tag2;
        public string tag3;
        public string tag4;
        public double from1,from2,from3,from4;//量程
        public double to1,to2,to3,to4;//量程
        public double time_from;
        public double time_to;
        public IPointListEdit iList1;
        public IPointListEdit iList2;
        public IPointListEdit iList3;
        public IPointListEdit iList4;
        public IPointListEdit iList5;
        public IPointListEdit iList6;
        public IPointListEdit iList7;
        public IPointListEdit iList8;
        public IPointListEdit iList9;
        public IPointListEdit iList10;
        public IPointListEdit iList11;
        public IPointListEdit iList12;
        public IPointListEdit iList13;
        public IPointListEdit iList14;
        public IPointListEdit iList15;
        public IPointListEdit iList16;
        public GraphObjList listText;
        public Bitmap bit;
        public bool clickOK = false;
        public HistoryDataPrtsc()
        {
            InitializeComponent();
        }

        private void setButtonColor()
        {
            SelectTag1.BackColor = Color.White;
            SelectTag2.BackColor = Color.White;
            SelectTag3.BackColor = Color.White;
            SelectTag4.BackColor = Color.White;
            SelectTag5.BackColor = Color.White;
            SelectTag6.BackColor = Color.White;
            SelectTag7.BackColor = Color.White;
            SelectTag8.BackColor = Color.White;
            SelectTag9.BackColor = Color.White;
            SelectTag10.BackColor = Color.White;
            SelectTag11.BackColor = Color.White;
            SelectTag12.BackColor = Color.White;
            SelectTag13.BackColor = Color.White;
            SelectTag14.BackColor = Color.White;
            SelectTag15.BackColor = Color.White;
            SelectTag16.BackColor = Color.White;

            rad_left_Label1.BackColor = Color.White;
            rad_right_Label1.BackColor = Color.White;

            rad_left_Label2.BackColor = Color.White;
            rad_right_Label2.BackColor = Color.White;

            rad_left_Label3.BackColor = Color.White;
            rad_right_Label3.BackColor = Color.White;

            rad_left_Label4.BackColor = Color.White;
            rad_right_Label4.BackColor = Color.White;

            rad_left_Label5.BackColor = Color.White;
            rad_right_Label5.BackColor = Color.White;

            rad_left_Label6.BackColor = Color.White;
            rad_right_Label6.BackColor = Color.White;

            rad_left_Label7.BackColor = Color.White;
            rad_right_Label7.BackColor = Color.White;

            rad_left_Label8.BackColor = Color.White;
            rad_right_Label8.BackColor = Color.White;

            rad_left_Label9.BackColor = Color.White;
            rad_right_Label9.BackColor = Color.White;

            rad_left_Label10.BackColor = Color.White;
            rad_right_Label10.BackColor = Color.White;

            rad_left_Label11.BackColor = Color.White;
            rad_right_Label11.BackColor = Color.White;

            rad_left_Label12.BackColor = Color.White;
            rad_right_Label12.BackColor = Color.White;

            rad_left_Label13.BackColor = Color.White;
            rad_right_Label13.BackColor = Color.White;

            rad_left_Label14.BackColor = Color.White;
            rad_right_Label14.BackColor = Color.White;

            rad_left_Label15.BackColor = Color.White;
            rad_right_Label15.BackColor = Color.White;

            rad_left_Label16.BackColor = Color.White;
            rad_right_Label16.BackColor = Color.White;
        }

        private void setLableColor()
        {
            rad_left_Label1.ForeColor = Color.Black;
            rad_right_Label1.ForeColor = Color.Black;

            rad_left_Label2.ForeColor = Color.Black;
            rad_right_Label2.ForeColor = Color.Black;

            rad_left_Label3.ForeColor = Color.Black;
            rad_right_Label3.ForeColor = Color.Black;

            rad_left_Label4.ForeColor = Color.Black;
            rad_right_Label4.ForeColor = Color.Black;

            rad_left_Label5.ForeColor = Color.Black;
            rad_right_Label5.ForeColor = Color.Black;

            rad_left_Label6.ForeColor = Color.Black;
            rad_right_Label6.ForeColor = Color.Black;

            rad_left_Label7.ForeColor = Color.Black;
            rad_right_Label7.ForeColor = Color.Black;

            rad_left_Label8.ForeColor = Color.Black;
            rad_right_Label8.ForeColor = Color.Black;

            rad_left_Label9.ForeColor = Color.Black;
            rad_right_Label9.ForeColor = Color.Black;

            rad_left_Label10.ForeColor = Color.Black;
            rad_right_Label10.ForeColor = Color.Black;

            rad_left_Label11.ForeColor = Color.Black;
            rad_right_Label11.ForeColor = Color.Black;

            rad_left_Label12.ForeColor = Color.Black;
            rad_right_Label12.ForeColor = Color.Black;

            rad_left_Label13.ForeColor = Color.Black;
            rad_right_Label13.ForeColor = Color.Black;

            rad_left_Label14.ForeColor = Color.Black;
            rad_right_Label14.ForeColor = Color.Black;

            rad_left_Label15.ForeColor = Color.Black;
            rad_right_Label15.ForeColor = Color.Black;

            rad_left_Label16.ForeColor = Color.Black;
            rad_right_Label16.ForeColor = Color.Black;
        }

        public void setText(string strButtonText, string strLeftLableText, string strRightLableText, int iIndex)
        {
            switch(iIndex)
            {
                case 1:
                    SelectTag1.Text = strButtonText;
                    rad_left_Label1.Text = strLeftLableText;
                    rad_right_Label1.Text = strRightLableText;
                    break;
                case 2:
                    SelectTag2.Text = strButtonText;
                    rad_left_Label2.Text = strLeftLableText;
                    rad_right_Label2.Text = strRightLableText;
                    break;
                case 3:
                    SelectTag3.Text = strButtonText;
                    rad_left_Label3.Text = strLeftLableText;
                    rad_right_Label3.Text = strRightLableText;
                    break;
                case 4:
                    SelectTag4.Text = strButtonText;
                    rad_left_Label4.Text = strLeftLableText;
                    rad_right_Label4.Text = strRightLableText;
                    break;
                case 5:
                    SelectTag5.Text = strButtonText;
                    rad_left_Label5.Text = strLeftLableText;
                    rad_right_Label5.Text = strRightLableText;
                    break;
                case 6:
                    SelectTag6.Text = strButtonText;
                    rad_left_Label6.Text = strLeftLableText;
                    rad_right_Label6.Text = strRightLableText;
                    break;
                case 7:
                    SelectTag7.Text = strButtonText;
                    rad_left_Label7.Text = strLeftLableText;
                    rad_right_Label7.Text = strRightLableText;
                    break;
                case 8:
                    SelectTag8.Text = strButtonText;
                    rad_left_Label8.Text = strLeftLableText;
                    rad_right_Label8.Text = strRightLableText;
                    break;
                case 9:
                    SelectTag9.Text = strButtonText;
                    rad_left_Label9.Text = strLeftLableText;
                    rad_right_Label9.Text = strRightLableText;
                    break;
                case 10:
                    SelectTag10.Text = strButtonText;
                    rad_left_Label10.Text = strLeftLableText;
                    rad_right_Label10.Text = strRightLableText;
                    break;
                case 11:
                    SelectTag11.Text = strButtonText;
                    rad_left_Label11.Text = strLeftLableText;
                    rad_right_Label11.Text = strRightLableText;
                    break;
                case 12:
                    SelectTag12.Text = strButtonText;
                    rad_left_Label12.Text = strLeftLableText;
                    rad_right_Label12.Text = strRightLableText;
                    break;
                case 13:
                    SelectTag13.Text = strButtonText;
                    rad_left_Label13.Text = strLeftLableText;
                    rad_right_Label13.Text = strRightLableText;
                    break;
                case 14:
                    SelectTag14.Text = strButtonText;
                    rad_left_Label14.Text = strLeftLableText;
                    rad_right_Label14.Text = strRightLableText;
                    break;
                case 15:
                    SelectTag15.Text = strButtonText;
                    rad_left_Label15.Text = strLeftLableText;
                    rad_right_Label15.Text = strRightLableText;
                    break;
                case 16:
                    SelectTag16.Text = strButtonText;
                    rad_left_Label16.Text = strLeftLableText;
                    rad_right_Label16.Text = strRightLableText;
                    break;
                default :
                    break;
            }
        }

        private void HistoryDataPrtsc_Load(object sender, EventArgs e)
        {
            setControlLanguage();//多语言对应
            SetBasic();
            setButtonColor();
            setLableColor();
        }
        /// <summary>
        /// 基础数据设置
        /// </summary>
        private void setPointList()
        {
            historyDataShow1.SetPointList(1, iList1);
            historyDataShow1.SetPointList(2, iList2);
            historyDataShow1.SetPointList(3, iList3);
            historyDataShow1.SetPointList(4, iList4);

            historyDataShow2.SetPointList(1, iList5);
            historyDataShow2.SetPointList(2, iList6);
            historyDataShow2.SetPointList(3, iList7);
            historyDataShow2.SetPointList(4, iList8);

            historyDataShow3.SetPointList(1, iList9);
            historyDataShow3.SetPointList(2, iList10);
            historyDataShow3.SetPointList(3, iList11);
            historyDataShow3.SetPointList(4, iList12);

            historyDataShow4.SetPointList(1, iList13);
            historyDataShow4.SetPointList(2, iList14);
            historyDataShow4.SetPointList(3, iList15);
            historyDataShow4.SetPointList(4, iList16);
        }

        public void SetBasic()
        {
            //黑白风格设置
            historyDataDepthShow1.SetStyle();
            historyDataShow1.SetStyle();
            historyDataShow2.SetStyle();
            historyDataShow3.SetStyle();
            historyDataShow4.SetStyle();
            //基础数据设置
            txt_company.Text = company==null?"":company;
            txt_companyman.Text = companyman == null ? "" : companyman;
            txt_contractor.Text = contractor == null ? "" : contractor;
            txt_country.Text = country == null ? "" : country;
            txt_daterelease.Text = daterelease == null ? "" : daterelease;
            txt_datespud.Text = dateSpud == null ? "" : dateSpud;
            txt_end.Text = endTime == null ? "" : endTime;
            txt_lease.Text = lease == null ? "" : lease;
            txt_start.Text = startTime == null ? "" : startTime;
            txt_tool.Text = toolPusher == null ? "" : toolPusher;
            txt_well.Text = drillNo == null ? "" : drillNo;
            lbl_tag1.Text = tag1;
            lbl_tag2.Text = tag2;
            lbl_tag3.Text = tag3;
            lbl_tag4.Text = tag4;
            historyDataDepthShow1.SetRange(time_from,time_to);
            historyDataDepthShow1.SetTextObj(listText);
            historyDataShow1.SetRange(from1, to1);
            historyDataShow2.SetRange(from2, to2);
            historyDataShow3.SetRange(from3, to3);
            historyDataShow4.SetRange(from4, to4);
            historyDataShow1.SetTimeFT(time_from, time_to);
            historyDataShow2.SetTimeFT(time_from, time_to);
            historyDataShow3.SetTimeFT(time_from, time_to);
            historyDataShow4.SetTimeFT(time_from, time_to);

            setPointList();

            //historyDataShow1.SetPointList(0, iList1);
            //historyDataShow2.SetPointList(1, iList2);
            //historyDataShow3.SetPointList(2, iList3);
            //historyDataShow4.SetPointList(3, iList4);
            tag_from1.Text = from1.ToString();
            tag_from2.Text = from2.ToString();
            tag_from3.Text = from3.ToString();
            tag_from4.Text = from4.ToString();
            tag_to1.Text = to1.ToString();
            tag_to2.Text = to2.ToString();
            tag_to3.Text = to3.ToString();
            tag_to4.Text = to4.ToString();
            txt_start.Text = Comm.ConvertIntDateTime(time_from * 1000).ToString();
            txt_end.Text = Comm.ConvertIntDateTime(time_to * 1000).ToString();
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
                            string sss = xe.GetAttribute("key");
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
            catch { }
        }
        #endregion

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                clickOK = true;
                bit = new Bitmap(1700, 900);//实例化一个和窗体一样大的bitmap
                Graphics g = Graphics.FromImage(bit);
                g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                g.CopyFromScreen(this.Left, this.Top+25, 0, 0, new Size(1700, 900));//截取需要的数据
                this.Close();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
