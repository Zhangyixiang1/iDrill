//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrillingSymtemCSCV2.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class DrillTools
    {
        public int ID { get; set; }
        public int DrillId { get; set; }
        public string Name { get; set; }
        public decimal Length { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Unit { get; set; }
        public Nullable<decimal> Group { get; set; }
        public int Basic { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> dataMakeTime { get; set; }
        public string dataMakeUser { get; set; }
        public string dataMakePGM { get; set; }
        public Nullable<System.DateTime> dataUpdTime { get; set; }
        public string dataUpdUser { get; set; }
        public string dataUpdPGM { get; set; }
        public int order { get; set; }
    }
}
