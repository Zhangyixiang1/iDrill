using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    public class HistoryDataModel
    {
        public int index { get; set; }
        public string Tag { get; set; }
        //public decimal from { get; set; }
        //public decimal to { get; set; }

        public double from { get; set; }
        public double to { get; set; }
    }

    public class getHistoryData
    {
        public getHistoryData()
        {
            datas = new List<HistoryData>();
            Depthdatas = new List<HistoryDepthData>();
        }

        public List<HistoryData> datas { get; set; }
        public List<HistoryDepthData> Depthdatas { get; set; }
    }

    public class HistoryDepthData
    {
        public int BoxId { get; set; }
        public string Tag { get; set; }
        public long Timestamp { get; set; }
        public double Value { get; set; }
    }

    public class HistoryData
    {
        public int BoxId { get; set; }
        public string Tag { get; set; }
        public List<TagValue> Datas { get; set; }
    }

    public class TagValue
    {
        public long Timestamp { get; set; }
        public double Value { get; set; }
    }

    public class QueryHistory
    {
        public int DrillId { get; set; }
        public long startTime { get; set; }
        public long endTime { get; set; }
        public List<string> Tag { get; set; }
        public string DepthTag { get; set; }
        public bool isHistoryData { get; set; }
    }

    public class CureValue
    {
        public double from { get; set; }
        public double to { get; set; }
    }
}
