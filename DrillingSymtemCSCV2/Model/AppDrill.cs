using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System.Net.Sockets;
using ZedGraph;

namespace DrillingSymtemCSCV2.Model
{
    public static class AppDrill
    {
        public static string username { get; set; }//保存用户名
        public static int permissionId { get; set; }    //保存用户权限
        public static string realName { get; set; }//保存真实姓名
        public static string DrillNo { get; set; } //保存当前井号Drill #3
        public static int DrillID = -1;
        public static string UnitFormat = "-1";//单位制式
        public static string language = "CN";//保存系统语言
        public static List<string> message = new List<string>();    //保存通用多语言信息
        public static string ActivityName { get; set; } //保存当前的活动状态
        //钻具管理基础属性
        public static string BHAName { get; set; }
        public static string BHALength { get; set; }
        public static string Length { get;set; }
        public static string Comment { get; set; }

        public static List<string> slip_list = new List<string>();
        public static List<Command> Command = new List<Command>();
        public static Socket client;
        public static List<TextMemo> MessageList = new List<TextMemo>();
        public static string[] FormSet { get; set; }    //保存界面设置启用状态
        public static List<JsonAlarm> JsonAlarmList = null;
        public static string videoFileName = string.Empty;
        public static string videoConf = string.Empty;
        public static List<UserTag> UserTag;
        public static bool bUserTagFirst = true;
    }

    public static class AppData
    {
        public static IConnectionFactory factory = null;
        public static IConnection connection = null;
        public static ISession session = null;
        public static List<UnitTransfer> UnitTransfer { get; set; }
    }

    public class ListItem : System.Object  //创建一个继承自Object的类，用于下拉框数据绑定
    {
        private string _Value = string.Empty;
        private string _Text = string.Empty;
        //值
        public string Value
        {
            get { return this._Value; }
            set { this._Value = value; }
        }
        //显示的文本
        public string Text
        {
            get { return this._Text; }
            set { this._Text = value; }
        }
        public ListItem(string value, string text)
        {
            this._Value = value;
            this._Text = text;
        }
        public override string ToString()
        {
            return this._Text;
        }
    }
    public class DrtillToolsData
    {
        public DrtillToolsData()
        {
            Date = new DateTime();
        }

        public int DrillId { get; set; }                //井号
        public string Name { get; set; }                //井的名字
        public decimal Length { get; set; }             //每根的长度
        public string Comment { get; set; }             //说明
        public System.DateTime Date { get; set; }       //日期
        public string Unit { get; set; }                //单位
        public Nullable<int> GroupId { get; set; }      //每一柱的ID
        public int Basic { get; set; }                  //0-最初的一条数据，1-之后的数据      
        public Nullable<System.DateTime> dataMakeTime { get; set; }
        public string dataMakeUser { get; set; }
        public string dataMakePGM { get; set; }
        public Nullable<System.DateTime> dataUpdTime { get; set; }
        public string dataUpdUser { get; set; }
        public string dataUpdPGM { get; set; }

    }

    public class WorkerType
    {
        public int ID { get ; set; } ///工种ID
        public string Name { get; set; } //工种名称
        public int Count { get; set; }  //允许数量
    }

    public class UserTag
    {
        public string Tag { get; set; }
        public string Form { get; set; }
        public Nullable<int> Group { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<decimal> VFrom { get; set; }
        public Nullable<decimal> VTo { get; set; }
    }

    //public class SendInfo
    //{
    //    public string Tag { get; set; }
    //    public int DrillId { get; set; }
    //    public long Timestamp { get; set; }
    //    public decimal Value { get; set; }
    //}
    public class TextMemo
    {
        public string Text { get; set; }    //显示的消息
        public double unix { get; set; }    //时间戳
    }
    /// <summary>
    /// 用于存取时效信息
    /// </summary>
    public class SXCode
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
    /// <summary>
    /// 存放报警的Json模型
    /// </summary>
    public class JsonAlarm {
        public string Tag { get; set; }
        public double L { get; set; }
        public double H { get; set; }
        public bool HIsActive {   get; set; }
        public bool LIsActive { get; set; }
    }
}
