using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    public class TagDayMiddleNightModel
    {
        public TagDayMiddleNightModel() {
            Type = new List<int>();
            Value = new List<double>();
        }

        public List<int> Type { set; get; }// 0 Night 1 Day
        public List<double> Value { set; get; }//0 Night 1 Day
    }
}
