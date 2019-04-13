using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Apache.NMS;
using DrillingSymtemCSCV2.Model;
using Apache.NMS.ActiveMQ;
using DrillingSymtemCSCV2.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class chart : UserControl
    {
        private const int X = 46;
        private const int width = 40; //用于定义图标宽度
        private int range = 1000;//范围
        public string value { get; set; }
        public int HHValue = 90;//超过%高报警
        public int HValue = 80;//超过%预报警
        public int LValue = 20;//低于%预报警
        public int LLValue = 10;//低于%低报警
        public Color color_normal = Color.Blue;//正常颜色
        public Color color_alarm = Color.Red;//报警颜色
        public Color color_pre_alarm = Color.Orange;//预报警颜色
        public string TagValue;//传入测点
        public string TagStatus;//检测当前罐状态
        public int TagStatusRecord;//记录当前罐状态
        public int ClientHandle;//需要写入kepware的位置
        public List<DrillTag> listTag=new List<DrillTag>();
        #region 消息中间件
        private IMessageConsumer consumer1;//接收数据用的
        private IMessageConsumer consumer2;//判断当前罐的状态
        #endregion
        public chart()
        {
            InitializeComponent();
        }
        //设置图表的高度
        public void setValue(int value)
        {
            Color color = Color.Blue ;
            Value.Text = value.ToString();
            if (value > range)
            {
                value = range;//如果大于范围值取最大范围
            }
            int result = value * 100 / range;//计算百分比
            if (result >= LValue || result <= HValue)
                color = color_normal;
            if (result < LValue || result > HValue)
                color = color_pre_alarm;
            if (result < LLValue || result > HHValue)
                color = color_alarm;
            lbl_chart.Location = new Point(X, 107 - result);//设置位置
            lbl_chart.Size = new Size(width, result);//设置size大小
            lbl_chart.BackColor = color;//设置图标颜色
            rlbl_percent.Text = result+"%";
            rlbl_percent.Location = new Point(87, 98-result);
        }

        private void chart_Load(object sender, EventArgs e)
        {
            //初始化
            setValue(0);
        }
        public void setParameter(string TagValue, string TagStatus, List<DrillTag> listTag, int ClientHandle, string capital, int range, int HHValue, int HValue, int LValue, int LLValue, Color color_normal, Color color_alarm, Color color_pre_alarm)
        {
            this.TagValue = TagValue;//当前罐的测点值
            this.TagStatus = TagStatus;//当前罐的状态值，0|1
            this.listTag = listTag;//所有测点数据
            this.ClientHandle = ClientHandle;//写入kepware测点的位置
            Captial.Text = capital;//显示文本
            if (AppDrill.UnitFormat == "yz")
            {
                this.Unit.Text = AppData.UnitTransfer.Find(o => o.UnitFrom == "m3").UnitTo;
                this.range = (int)Comm.UnitConversion(listTag, TagValue, AppDrill.DrillID.ToString(), range);
            }
            else if (AppDrill.UnitFormat == "gz")
            {
                this.Unit.Text = "m3";
                this.range = range;
            }
            
            this.HHValue = HHValue;//高报警
            this.HValue = HValue;//高预报警
            this.LValue = LValue;//低报警
            this.LLValue = LLValue;//低预报警
            this.color_normal = color_normal;//正常颜色
            this.color_alarm = color_alarm;//报警颜色
            this.color_pre_alarm = color_pre_alarm;//预报警颜色
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AppData.factory == null)
                    AppData.factory = new ConnectionFactory(System.Configuration.ConfigurationManager.AppSettings["DataSourceAddress"].ToString());
                //开始连接消息中间件
                if (AppData.connection == null)
                    AppData.connection = AppData.factory.CreateConnection(System.Configuration.ConfigurationManager.AppSettings["DataSourceID"].ToString(), System.Configuration.ConfigurationManager.AppSettings["DataSourcePassWord"].ToString());
                AppData.connection.Start();
                if (AppData.session == null)
                    AppData.session = AppData.connection.CreateSession();
            }
            catch { }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (AppData.session != null)
                {
                    consumer1 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(TagValue + "_" + AppDrill.DrillID));
                    consumer2 = AppData.session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic(TagStatus + "_" + AppDrill.DrillID));
                }
                consumer1.Listener += new MessageListener(consumer1_Listener);
                consumer2.Listener += new MessageListener(consumer2_Listener);
            }
            catch { }
            backgroundWorker1.CancelAsync();
        }

        #region 消息中间件

        private delegate void ShowNoteMsgDelegate_New(Dictionary<string, string> map);

        private void consumer1_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage1), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }
        //显示数据
        private void ShowMessage1(Dictionary<string, string> map)
        {
            double value = 0;
            if (double.TryParse(map["Value"], out value))
            {
                var newValue = Comm.UnitConversion(listTag, TagValue,map["DrillId"], value);
            }
            setValue((int)value);
        }
        private void consumer2_Listener(IMessage message)
        {
            try
            {
                IObjectMessage msg = (IObjectMessage)message;
                if (this.InvokeRequired)
                    this.BeginInvoke(new ShowNoteMsgDelegate_New(ShowMessage2), (Dictionary<string, string>)msg.Body);
            }
            catch { }
        }
        //显示数据
        private void ShowMessage2(Dictionary<string, string> map)
        {
            int value = 0;
            if (int.TryParse(map["Value"], out value))
            {
                TagStatusRecord = value;
            }
            if (value >= 1)
            {
                lbl_status.ForeColor = Color.Lime;
            }
            else
            {
                lbl_status.ForeColor = Color.White;
            }
        }
        #endregion

        private void pnl_Memotext_Click(object sender, EventArgs e)
        {
            PitStatus pit = new PitStatus();
            pit.pitName = this.Captial.Text;//罐测点名
            pit.pitStatus = TagStatusRecord;//当前罐是否启用状态
            pit.ClientHandle = ClientHandle;//写入kepware的位置
            pit.Show();
        }
    }
}
