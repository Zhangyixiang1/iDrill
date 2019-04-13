using DrillingSymtemCSCV2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;

namespace DrillingSymtemCSCV2.Forms
{
    public partial class Frm_Welllist : Telerik.WinControls.UI.RadForm
    {
        DrillOSEntities db;

        public Frm_Welllist()
        {
            InitializeComponent();
        }


        private void Frm_Welllist_Load(object sender, EventArgs e)
        {
            db = new Model.DrillOSEntities();
            var welllist = db.Drill.Where(o => o.isActive == true);
            foreach (var item in welllist)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = index;                        //序号
                dataGridView1.Rows[index].Cells[1].Value = item.Contractor;              //队伍编号
                dataGridView1.Rows[index].Cells[2].Value = item.DrillNo;                 //井号
                dataGridView1.Rows[index].Cells[3].Value = item.RigNo;                   //钻机编号
                dataGridView1.Rows[index].Cells[4].Value = item.Operator;                //业主
                dataGridView1.Rows[index].Cells[5].Value = item.longitude;               //经度
                dataGridView1.Rows[index].Cells[6].Value = item.latitude;                //纬度
                dataGridView1.Rows[index].Cells[7].Value = item.Lease;                   //所在地
                dataGridView1.Rows[index].Cells[8].Value = item.design_depth;            //设计井深
                dataGridView1.Rows[index].Cells[9].Value = item.DateSpud;                //开钻日期
                dataGridView1.Rows[index].Cells[10].Value = item.period;                 //钻井周期

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            //修改事件
            if (e.ColumnIndex == 11 && e.RowIndex >= 0)
            {
                Frm_welllistcon frm = new Forms.Frm_welllistcon(false, dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                frm.ShowDialog();
               
            }
            //新建事件
            else if (e.ColumnIndex == 12 && e.RowIndex >= 0)
            {
                Frm_welllistcon frm = new Forms.Frm_welllistcon(true, dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                frm.ShowDialog();
            }

            //返回后刷新界面
            db = new Model.DrillOSEntities();
            var welllist = db.Drill.Where(o => o.isActive == true);
            dataGridView1.Rows.Clear();
            foreach (var item in welllist)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = index;                        //序号
                dataGridView1.Rows[index].Cells[1].Value = item.Contractor;              //队伍编号
                dataGridView1.Rows[index].Cells[2].Value = item.DrillNo;                 //井号
                dataGridView1.Rows[index].Cells[3].Value = item.RigNo;                   //钻机编号
                dataGridView1.Rows[index].Cells[4].Value = item.Operator;                //业主
                dataGridView1.Rows[index].Cells[5].Value = item.longitude;               //经度
                dataGridView1.Rows[index].Cells[6].Value = item.latitude;                //纬度
                dataGridView1.Rows[index].Cells[7].Value = item.Lease;                   //所在地
                dataGridView1.Rows[index].Cells[8].Value = item.design_depth;            //设计井深
                dataGridView1.Rows[index].Cells[9].Value = item.DateSpud;                //开钻日期
                dataGridView1.Rows[index].Cells[10].Value = item.period;                 //钻井周期

            }
        }
    }
}
