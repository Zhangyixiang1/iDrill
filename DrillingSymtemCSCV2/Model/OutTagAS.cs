using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrillingSymtemCSCV2.Model
{

    //输出测点对应的平均值、数据集合
    public class OutTagAS
    {
        public OutTagAS()
        {
            Averages = new List<decimal>();
            Sums = new List<double>();
            Size = new List<double>();
            OtherAverages = new List<decimal>();
        }
        public List<double> Size { get; set; }      //泵衬值
        public List<decimal> OtherAverages { get; set; } //平均值
        public List<decimal> Averages { get; set; } //平均值
        public List<double> Sums { get; set; }      //合计值
    }

    public class InputTagAS
    {
        public InputTagAS()
        {
            AverageTags = new List<string>();
            SumTags = new List<string>();
            SizeTags = new List<string>();
        }
        public int DrillId { get; set; }//井号
        public double from { get; set; }//开始日期
        public double to { get; set; }//结束日期
        public List<string> OtherAverageTags { get; set; }   //需要求泵衬值的测点
        public List<string> SizeTags { get; set; }   //需要求泵衬值的测点
        public List<string> AverageTags { get; set; }//需要求平均值的测点
        public List<string> SumTags { get; set; }    //需要求和的测点
    }
    //单独求平均值的数据
    public class InputAverageData
    {
        public InputAverageData() {
            OtherAverageTags = new List<string>();
        }
        public int DrillId { get; set; }
        public long from { get; set; }//开始日期
        public long to { get; set; }//结束日期
        public List<string> OtherAverageTags { get; set; }   //需要求泵衬值的测点
    }
    //获取求平均值的数据
    public class OutputAverageData
    {
        public OutputAverageData() {
            Value = new List<decimal>();
        }
        public List<decimal> Value { get; set; } //平均值
    }

}
