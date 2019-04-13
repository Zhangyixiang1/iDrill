using DrillingSymtemCSCV2.Forms;
using DrillingSymtemCSCV2.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrillingSymtemCSCV2.Model
{
    public static class Alarms
    {
        //Alarms按钮状态
        public static bool isAlarm = false;
        public static List<AlarmModel> list_Alarms = new List<AlarmModel>();//实时记录报警测点信息
        //取消所有报警
        public static void CancelAlarm()
        {
            //取消Alarms按钮报警
            isAlarm = false;
        }
    }
    public class AlarmModel
    {
        public string Tag { get; set; }         //报警测点
        public int DrillId { get; set; }     //井号
        public decimal Value { get; set; }       //报警值
        public int status { get; set; }         //当前测点报警状态值，1未确认的正在报警，2已确认报警且不再继续报警，3已确认报警且继续报警
    }
}
