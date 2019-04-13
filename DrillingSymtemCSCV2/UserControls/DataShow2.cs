using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    /// <summary>
    /// UserControl   Add by ZAY in 2017.5.4
    /// </summary>
    public partial class DataShow2 : UserControl
    {
        public string DSCaptial { get; set; }
        public string DSValue { get; set; }
        public string DSLValue { get; set; }
        public string DSHValue { get; set; }
        public string DSUnit { get; set; }
        public string DTag { get; set; }

        public DataShow2()
        {
            InitializeComponent();
        }

        private void DataShow2_Load(object sender, EventArgs e)
        {

        }
        
        //设置控件值
        public void SetTag()
        {
            this.Captial.Text = DSCaptial;
            this.Value.Text = DSValue == null ?  "###" : DSValue;           
            this.Unit.Text = DSUnit;
        }

        public void setTagValue(string strValue)
        {
            if (!string.IsNullOrEmpty(this.Captial.Text))
            {
                this.Value.Text = strValue;
            }
        }
    }
}
