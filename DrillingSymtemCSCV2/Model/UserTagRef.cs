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
    
    public partial class UserTagRef
    {
        public int ID { get; set; }
        public int DrillId { get; set; }
        public string Username { get; set; }
        public string JsonTag { get; set; }
        public Nullable<System.DateTime> dataMakeTime { get; set; }
        public string dataMakePGM { get; set; }
        public string dataMakeUser { get; set; }
        public Nullable<System.DateTime> dataUpdTime { get; set; }
        public string dataUpdPGM { get; set; }
        public string dataUpdUser { get; set; }
        public string JsonAlarm { get; set; }
    }
}