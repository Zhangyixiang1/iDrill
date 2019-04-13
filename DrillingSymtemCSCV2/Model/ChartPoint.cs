using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    public class ChartPoint
    {
      public  ChartPoint(long dt,double v) {
            dateTime = dt;value = v;

        }
        public ChartPoint()
        {
        }
        public long dateTime { get; set; }      //时间
        public double value { get;set; }        //值
    }
}
