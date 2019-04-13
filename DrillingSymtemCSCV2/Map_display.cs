using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;

namespace DrillingSymtemCSCV2
{
    public partial class Map_display : Form
    {

        GeckoWebBrowser geckoWebBrowser;
        public Map_display()
        {
            InitializeComponent();
            Xpcom.Initialize("Firefox");
            geckoWebBrowser = new GeckoWebBrowser { };
            geckoWebBrowser.Size = new Size(1920, 1080);
            this.Controls.Add(geckoWebBrowser);
        }

        private void Map_display_Load(object sender, EventArgs e)
        {

            geckoWebBrowser.Navigate(Application.StartupPath + @"\Map_resource\index.html");

        }

    }
}
