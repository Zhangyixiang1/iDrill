using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    public class RealTimeData
    {
        public int BoxId {get; set;}
        public long Timestamp {get; set;}
        public List<RealTags> realTags {get; set;}

        public RealTimeData()
        {
            realTags = new List<RealTags>();
        }
    }

    public class RealTags
    {
        public string Tag { get; set; }
        public double Value { get; set; }
    }

    public enum DATA_SHOW
    {
        tagOne = 0,
        tagTwo,
        tagThree,
        tagFour,
        tagFive,
        tagSix,
        tagSeven,
        tagEight,
        tagNine,
        tagTen
    }
}
