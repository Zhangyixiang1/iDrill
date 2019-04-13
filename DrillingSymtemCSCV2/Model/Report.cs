using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrillingSymtemCSCV2.Model
{
    //报表数据
    public class Report
    {
        public Report()
        {
            TableDetails = new List<TableDetail>();//初始化
        }
        public int DrillId { get; set; }    //井号
        public string UserName { get; set; }    //用户
        public List<TableDetail> TableDetails { get; set; }  //报表中表的所有数据
    }
    //报表中表保存的数据
    public class TableDetail
    {
        public TableDetail()
        {
            RowDatas = new List<string>();//初始化
        }
        public string TableName { get; set; }   //对应表的名字
        public List<string> RowDatas { get; set; }  //表中保存的数据，每行数据用','隔开
    }
}
