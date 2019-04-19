using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using DrillingSymtemCSCV2.Model;

using Newtonsoft.Json;

namespace DrillingSymtemCSCV2
{
    public partial class Map_display : Form
    {
        DrillOSEntities db;
        GeckoWebBrowser geckoWebBrowser;
        bool isini = false; // 是否刷新十二点的统计量
        int msgindex = 1; //刷新消息的变量
        string url_single = "http://122.112.221.34:6688/api/HisSingleData";
        List<string> msgcon = new List<string>();
        Dictionary<int, List<double>> holehis = new Dictionary<int, List<double>>();
        public Map_display()
        {
            InitializeComponent();
            Xpcom.Initialize("Firefox");
            geckoWebBrowser = new GeckoWebBrowser { };
            geckoWebBrowser.Size = new Size(1920, 1080);
            this.Controls.Add(geckoWebBrowser);
            db = new DrillOSEntities();
        }

        private void Map_display_Load(object sender, EventArgs e)
        {


            geckoWebBrowser.Navigate(Application.StartupPath + @"\Map_resource\index.html");
            timer1.Interval = 5000;
            timer1.Enabled = true;

        }

        /// <summary>
        /// 返回10位时间戳
        /// </summary>
        /// <param name="dt">传入时间</param>
        /// <returns></returns>
        private long Gettimestamp(string dt)
        {
            DateTime ts = DateTime.Parse(dt);
            return (long)(ts - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int sum_design = 0; //设计周期
            int sum_act = 0;    //实际周期
            Dictionary<int, string[]> fooagelist;  //进尺

            //执行js函数刷新界面   
            var executor = new Gecko.JQuery.JQueryExecutor(geckoWebBrowser.Window);  //先获取到jquery对象
            using (db = new DrillOSEntities())
            {
                //每日12点更新钻井周期和周进尺
                if (DateTime.Now.Hour == 12 || !isini)
                {



                    fooagelist = new Dictionary<int, string[]>();
                    string url = "http://122.112.221.34:6688/api/HoleDepthData";     
                    var drill = db.Drill.Where(o => o.isActive == true);

                    foreach (var item in drill)
                    {

                        //初始化，取出在线井的标定井深，以计算进尺
                        if (item.HeartBeat == "True")
                        {
                            DateTime now = DateTime.Now.Date;
                            SingleData a_now = new SingleData();
                            a_now.drillID = item.ID;
                            a_now.ts = Gettimestamp(now.ToString());
                            string result_hole_now = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_now)));

                            SingleData a_m = new SingleData();
                            a_m.drillID = item.ID;
                            a_m.ts = Gettimestamp(now.AddMonths(-1).ToString());
                            string result_hole_m = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_m)));

                            SingleData a_y = new SingleData();
                            a_y.drillID = item.ID;
                            a_y.ts = Gettimestamp(now.AddYears(-1).ToString());
                            string result_hole_y = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_y)));

                            List<double> holetemp = new List<double>();
                            holetemp.Add(double.Parse(result_hole_now));
                            holetemp.Add(double.Parse(result_hole_m));
                            holetemp.Add(double.Parse(result_hole_y));
                            holehis.Add(item.ID, holetemp);

                            int temp1 = int.Parse(item.period);
                            sum_design += temp1;
                            int temp2 = Convert.ToInt16((DateTime.Now - DateTime.Parse(item.DateSpud)).TotalDays);
                            sum_act += temp2;
                            //请求api，返回当前井号的前8天的井深
                            string result = Comm.HttpPost(url, item.ID.ToString());
                            string[] temp = result.Substring(2, result.Length - 4).Split(',');
                            fooagelist.Add(item.ID, temp);

                        }

                        //初始化，显示地图标注
                        executor.ExecuteJQuery(string.Format("Addmarker({0},{1},{2},{3},{4},{5})", item.longitude, item.latitude, item.xoff, item.yoff, "'" + item.Contractor + "'", "'" + item.Lease + "'"));
                    }



                    //刷新进尺列表
                    //解析进尺，取出前7天的数据
                    double[] hole_de = new double[8];
                    double[] foot_ag = new double[7];
                    for (int i = 0; i < hole_de.Length; i++)
                    {
                        foreach (var item in fooagelist)
                        {
                            hole_de[i] += double.Parse(item.Value[i]);
                        }

                    }

                    for (int i = 0; i < foot_ag.Length; i++)
                    {
                        foot_ag[i] = hole_de[i + 1] - hole_de[i];
                    }




                    StringBuilder str = new StringBuilder();
                    str.Append("[");
                    for (int i = 0; i < foot_ag.Length; i++)
                    {
                        str.Append(foot_ag[i].ToString("#0.00") + ",");
                    }
                    str.Remove(str.Length - 1, 1);
                    str.Append("]");

                    executor.ExecuteJQuery(string.Format("drillrefresh({0},{1})", sum_design, sum_act));
                    executor.ExecuteJQuery(string.Format("footagerefresh({0})", str.ToString()));
                    isini = true;
                }
                //在线情况
                int online = db.Drill.Where(o => o.HeartBeat == "True").Count();
                int offline = db.Drill.Where(o => o.isActive == true).Count() - online;


                //消息记录
                if (msgindex == 1)
                {
                    var msg_or = db.Memo.OrderByDescending(o => o.CreateTime).Take(18);
                    msgcon = new List<string>();
                    foreach (var item in msg_or)
                    {
                        string wellname = db.Drill.Where(o => o.ID == item.DrillID).First().Contractor;
                        StringBuilder temp = new StringBuilder();
                        DateTime dt = Convert.ToDateTime(item.CreateTime);
                        temp.Append(dt.ToString("hh:mm:ss") + " " + wellname + item.Text);
                        msgcon.Add(temp.ToString());
                    }
                }
                StringBuilder msg = new StringBuilder();
                msg.Append("[");
                for (int j = 0; j < 6; j++)
                {
                    msg.Append("'" + msgcon[6 * msgindex + j] + "',");
                }
                msg.Remove(msg.Length - 1, 1);
                msg.Append("]");
                msgindex++;
                if (msgindex >= 3) msgindex = 1;

                executor.ExecuteJQuery(string.Format("onlinerefresh({0},{1})", online, offline));//然后执行jquery的代码
                executor.ExecuteJQuery(string.Format("msgrefresh({0})", msg.ToString()));



                //刷新表格
                int rowindex = 0; //表格刷新标记
                foreach (var item in db.Drill.Where(o => o.isActive == true))
                {
                    if (item.HeartBeat == "True")
                    {

                        //封装输入参数
                        StringBuilder ss1 = new StringBuilder();
                        ss1.Append("[");
                        ss1.Append("'" + item.Operator + "'" + ",");
                        ss1.Append("'" + item.Contractor + "'" + ",");
                        ss1.Append("'" + item.DrillNo + "'" + ",");
                        ss1.Append("'" + DateTime.Parse(item.DateSpud).ToShortDateString() + "'" + ",");
                        ss1.Append("'" + item.period + "'" + ",");
                        ss1.Append("'" + item.design_depth + "'");
                        ss1.Append("]");

                        //请求api接口，获得数据
                        SingleData a = new SingleData();
                        a.drillID = item.ID;
                        a.ts = Gettimestamp(DateTime.Now.ToString());
                        string input = JsonConvert.SerializeObject(a);
                        string temp = Comm.HttpPost(url_single, input);
                        string value = process_value(item.ID, temp);
                        executor.ExecuteJQuery(string.Format("tablerefresh({0},{1},{2})", rowindex, ss1.ToString(), value));
                        executor.ExecuteJQuery(string.Format("setonline({0},{1})", 1, "'" + item.Contractor + "'"));
                        rowindex++;
                    }



                    else
                    {
                        executor.ExecuteJQuery(string.Format("setonline({0},{1})", 0, "'" + item.Contractor + "'"));
                        executor.ExecuteJQuery(string.Format("tableoffline({0})", "'" + item.Contractor + "'"));

                    }
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (db = new DrillOSEntities())
            {
                foreach (var item in db.Drill.Where(o => o.HeartBeat == "True"))
                {
                    //请求api接口，获得数据
                    SingleData a = new SingleData();
                    a.drillID = item.ID;
                    a.ts = Gettimestamp(DateTime.Now.ToString());
                    string input = JsonConvert.SerializeObject(a);
                    string temp = Comm.HttpPost(url_single, input);
                    string value = process_value(item.ID, temp);
                    //执行js函数刷新界面   
                    var executor = new Gecko.JQuery.JQueryExecutor(geckoWebBrowser.Window);  //先获取到jquery对象
                    executor.ExecuteJQuery(string.Format("tablerefresh({0},{1})", "'" + item.Contractor + "'", value));
                }
            }

        }

        string process_value(int drillid, string value)
        {
            //处理http返回的字符串，获得参数
            string[] temp = value.Split('#');
            double hole_depth = 0, ROP = 0, footage, foot_m, foot_y, wob = 0, diffp = 0, flow = 0, spp = 0, rpm = 0, tor = 0;
            foreach (var item in temp)
            {
                if (item.Contains("var2,")) hole_depth = Math.Round(Convert.ToDouble(item.Substring(5)), 2);
                if (item.Contains("var13,")) ROP = Math.Round(Convert.ToDouble(item.Substring(6)), 2);
                if (item.Contains("var6,")) wob = Math.Round(Convert.ToDouble(item.Substring(5)), 2);
                if (item.Contains("var8,")) diffp = Math.Round(Convert.ToDouble(item.Substring(5)), 2);
                if (item.Contains("var25,")) flow = Math.Round(Convert.ToDouble(item.Substring(6)), 2);
                if (item.Contains("var7,")) spp = Math.Round(Convert.ToDouble(item.Substring(5)), 2);
                if (item.Contains("var16,")) rpm = Math.Round(Convert.ToDouble(item.Substring(6)), 2);
                if (item.Contains("var16,")) tor = Math.Round(Convert.ToDouble(item.Substring(6)), 2);
            }

            //判断列表里是否有进尺的初始值
            if (holehis.ContainsKey(drillid))
            {
                footage = Math.Round(hole_depth - holehis[drillid][0], 2);
                foot_m = Math.Round(hole_depth - holehis[drillid][1], 2);
                foot_y = Math.Round(hole_depth - holehis[drillid][2], 2);
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append(hole_depth + "," + ROP + "," + footage + "," + foot_m + "," + foot_y + "," + wob + "," + diffp + "," + flow + "," + spp + "," + rpm + "," + tor + "]");
                return str.ToString();
            }
            //若没有生成进尺的参考值
            else
            {
                DateTime now = DateTime.Now.Date;
                SingleData a_now = new SingleData();
                a_now.drillID = drillid;
                a_now.ts = Gettimestamp(now.ToString());
                string result_hole_now = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_now)));

                SingleData a_m = new SingleData();
                a_m.drillID = drillid;
                a_m.ts = Gettimestamp(now.AddMonths(-1).ToString());
                string result_hole_m = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_m)));

                SingleData a_y = new SingleData();
                a_y.drillID = drillid;
                a_y.ts = Gettimestamp(now.AddYears(-1).ToString());
                string result_hole_y = getHoledepth(Comm.HttpPost(url_single, JsonConvert.SerializeObject(a_y)));

                List<double> holetemp = new List<double>();
                holetemp.Add(double.Parse(result_hole_now));
                holetemp.Add(double.Parse(result_hole_m));
                holetemp.Add(double.Parse(result_hole_y));
                holehis.Add(drillid, holetemp);

                footage = hole_depth - holehis[drillid][0];
                foot_m = hole_depth - holehis[drillid][1];
                foot_y = hole_depth - holehis[drillid][2];
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append(hole_depth + "," + ROP + "," + footage + "," + foot_m + "," + foot_y + "," + wob + "," + diffp + "," + flow + "," + spp + "," + rpm + "," + tor + "]");
                return str.ToString();
            }

        }

        string getHoledepth(string value)
        {
            string[] temp = value.Split('#');
            foreach (var item in temp)
            {
                if (item.Contains("var2,")) return Convert.ToDouble(item.Substring(5)).ToString("#0.00");

            }
            return "0.00";
        }

        public class SingleData
        {
            public int drillID;
            public long ts;
        }


    }
}
