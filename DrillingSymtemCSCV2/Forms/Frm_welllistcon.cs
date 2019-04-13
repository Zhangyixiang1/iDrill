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
    public partial class Frm_welllistcon : Telerik.WinControls.UI.RadForm
    {
        bool isNew; //0修改，1新建
        string contractor; //队伍编号
        DrillOSEntities db;
        public Frm_welllistcon(bool isNew, string Contractor)
        {
            InitializeComponent();
            this.isNew = isNew;
            this.contractor = Contractor;
        }

        private void Frm_welllistcon_Load(object sender, EventArgs e)
        {
            db = new Model.DrillOSEntities();
            lbl_contractor.Text = contractor;

            //新建命令
            if (isNew)
            {

            }
            //修改命令
            else
            {
                var item = db.Drill.Where(o => o.Contractor == contractor && o.isActive == true).FirstOrDefault();
                dateTimePicker1.Value = DateTime.Parse(item.DateSpud);
                txb_depth.Text = item.design_depth;
                txb_drillNo.Text = item.DrillNo;
                txb_lati.Text = item.latitude;
                txb_location.Text = item.Lease;
                txb_longi.Text = item.longitude;
                txb_operator.Text = item.Operator;
                txb_period.Text = item.period;
                txb_rigNo.Text = item.RigNo;
            }
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            //首先输入信息不能为空
            foreach (Control ctr in panel1.Controls)
            {
                if (ctr is TextBox)
                {
                    TextBox txb = ctr as TextBox;
                    if (string.IsNullOrEmpty(txb.Text))
                    {
                        lbl_warm.Visible = true;
                        return;
                    }
                }

            }
            //如果新建一口井
            if (isNew)
            {


                DialogResult result = MessageBox.Show("警告！，新建井后原有井口数据接口将关闭，单击确定按钮继续！", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    using (db = new Model.DrillOSEntities())
                    {
                        Drill item = new Model.Drill();
                        item.DateSpud = dateTimePicker1.Value.ToString();
                        item.design_depth = txb_depth.Text;
                        item.DrillNo = txb_drillNo.Text;
                        item.latitude = txb_lati.Text;
                        item.Lease = txb_location.Text;
                        item.longitude = txb_longi.Text;
                        item.Operator = txb_operator.Text;
                        item.period = txb_period.Text;
                        item.RigNo = txb_rigNo.Text;

                        //重要信息
                        var olditem = db.Drill.Where(o => o.Contractor == contractor && o.isActive == true).FirstOrDefault();
                        olditem.isActive = false;
                        item.Contractor = lbl_contractor.Text;
                        item.isActive = true;                    
                        db.Drill.Add(item);
                        db.SaveChanges();
                    }
                }
            }
            //更新信息
            else
            {
                using (db = new Model.DrillOSEntities())
                {
                    var item = db.Drill.Where(o => o.Contractor == contractor && o.isActive == true).FirstOrDefault();
                    item.DateSpud = dateTimePicker1.Value.ToString();
                    item.design_depth = txb_depth.Text;
                    item.DrillNo = txb_drillNo.Text;
                    item.latitude = txb_lati.Text;
                    item.Lease = txb_location.Text;
                    item.longitude = txb_longi.Text;
                    item.Operator = txb_operator.Text;
                    item.period = txb_period.Text;
                    item.RigNo = txb_rigNo.Text;
                    db.SaveChanges();
                }
            }
            lbl_warm.Visible = false;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
