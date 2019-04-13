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
    
    public partial class Drill
    {
        public int ID { get; set; }
        public string DrillNo { get; set; }
        public string Lease { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public string Contractor { get; set; }
        public string DateSpud { get; set; }
        public string ToolPusher { get; set; }
        public string DateRelease { get; set; }
        public string CompanyMan { get; set; }
        public Nullable<System.DateTime> dataMakeTime { get; set; }
        public string dataMakePGM { get; set; }
        public string dataMakeUser { get; set; }
        public Nullable<System.DateTime> dataUpdTime { get; set; }
        public string dataUpdPGM { get; set; }
        public string dataUpdUser { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string RigNo { get; set; }
        public string Operator { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string HoleDepth { get; set; }
        public string HeartBeat { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string design_depth { get; set; }
        public string period { get; set; }
    }
}