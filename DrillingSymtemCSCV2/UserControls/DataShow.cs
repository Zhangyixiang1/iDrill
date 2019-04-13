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
    public partial class DataShow : UserControl
    {
        public string DSCaptal { get; set; }
        public string DSunit { get; set; }
        public string DSvalue { get; set; }
        public string DSRangeFrom { get; set; }
        public string DSRangeTo { get; set; }
        public Color DSValueColor { get; set; }
        public string DSLValue { get; set; }
        public string DSHValue { get; set; }
        public string DTag { get; set; }

        public DataShow()
        {
            InitializeComponent();
        }

        private void DataShow_Load(object sender, EventArgs e)
        {
       
        }


        //设置控件值
        public void SetTag()
        {            
            this.Captial.Text = DSCaptal;            
            this.Unit.Text = DSunit;
            if (!string.IsNullOrEmpty(DSvalue))
            {
                double value = double.Parse(DSvalue);
                this.Value.Text = value.ToString("#0.0.");
            }
            else
            {
                if (string.IsNullOrEmpty(DSCaptal))
                {
                    this.Value.Text = DSvalue;
                }
                else
                {
                    this.Value.Text = DSvalue == null ? "###" : DSvalue;
                }
            }

            this.From.Text = DSRangeFrom;          
            this.To.Text = DSRangeTo;
            this.Value.ForeColor = DSValueColor;
        }

        public void setTagValue(string strValue)
        {
            if (!string.IsNullOrEmpty(DSCaptal))
            {
                this.Value.Text = strValue;
            }
        }
    }
}
