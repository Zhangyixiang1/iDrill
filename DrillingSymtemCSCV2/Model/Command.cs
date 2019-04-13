using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    public class Command
    {
        public int Group { get; set; }  //分组
        public string Tag { get; set; } //测点
        public string TagName { get; set; } //测点名称
        public int hand { get; set; }   //写入kepware的位置
        public string Unit { get; set; }    //单位
    }
}
