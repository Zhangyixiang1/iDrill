using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace DrillingSymtemCSCV2.UserControls
{
    public partial class DataShow3 : UserControl
    {

        private int charNumber = 0;//双字节数 和 单字节数 总和 这是用于统计中文和字母数字的总长度 方便DescribeLabel控件居中
        

        public DataShow3()
        {
            InitializeComponent();
        }

        private void DataShow3_Load(object sender, EventArgs e)
        {
            

        }


        //设置描述
        public void SetDescribe(String sDescribe) {
            Describe.Text = sDescribe;

            setCenter(Describe);
        }

        //设置Title
        public void SetTitle(String sTitle)
        {
            Title.Text = sTitle;

            setCenter(Title,-5);

          
        }

        //设置Value
        public void SetValue(double dValue)
        {        
            Value.Text = dValue.ToString("0.00");

            charNumber = Value.Text.Length;//获取Describe标签中 包括双字节数的字符个数         

            
            Value.Location = new Point(((this.Size.Width - charNumber * 9)-Cube.Size.Width) / 2,12);
        }



        #region 完美居中 By 钟越
        //<summary>
        //每个字符大概9个像素
        //(总宽度- 字符数*9px ) / 2 = Label左边距
        //         当前Label宽度
        //第一个参数是待居中的目标标签 第二个参数是目标标签的高度,若无则默认放置最底部
        //</summary>
        #endregion
        private void setCenter(RadLabel radLabel, int height=0)    
        {
            charNumber = Encoding.Default.GetBytes(radLabel.Text).Length;//获取Describe标签中 包括双字节数的字符个数           
            radLabel.Location = new Point((this.Size.Width - (charNumber * 9)) / 2, height==0?this.Size.Height - radLabel.Height:height);
        }

        private void Value_Click(object sender, EventArgs e)
        {

        }

        

    }
}
