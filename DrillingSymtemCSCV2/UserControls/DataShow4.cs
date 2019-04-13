using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class DataShow4 : UserControl
    {
       

        public DataShow4()
        {
            InitializeComponent();
        }

        private void DataShow4_Load(object sender, EventArgs e)
        {

        }   

        private void AdjustmentSize(object sender, EventArgs e)
        {        
            //自动调整控件 by 钟越

            //Value标签 根据大小 缩放字体
            Value.Font = new Font(Value.Font.FontFamily,(int)(this.Size.Width /8), Value.Font.Style);

            //Value标签 上下左右居中           
            Value.Location = new Point((int)(this.Size.Width*0.25)+5,(int)(this.Size.Height*0.125)-5);

            //Cube标签 根据大小 缩放字体
            Cube.Font = new Font(Cube.Font.FontFamily, (int)(this.Size.Width / 17), Cube.Font.Style);

            //Cube标签 右下角固定 //右下角预留5px
            Cube.Location = new Point((int)(this.Size.Width - Cube.Size.Width - Encoding.Default.GetBytes(Cube.Text).Length*9)+5, (int)(this.Size.Height - Cube.Size.Height));            
          
        }

        public void SetValue(double dValue) {//值
            Value.Text = dValue.ToString("0.00");
        }

        public void SetDepart(String sDepart)//单位（% m³）
        {
            Cube.Text = sDepart;
        }



    }
}
