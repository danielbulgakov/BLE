using Client.BLE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Windows.System;

namespace Client
{
    public partial class DeviceFrame : Form
    {
        BLEesp32 esp32;
        public DeviceFrame(ulong uuid)
        {
            InitializeComponent();
            esp32 = new BLEesp32(uuid);



            chart1.Series.Add("Line");
        }


        private void DeviceFrame_Load(object sender, EventArgs e)
        {
            var UpdateTimer = new System.Windows.Forms.Timer();
            UpdateTimer.Interval = 5000;
            UpdateTimer.Enabled = true;
            UpdateTimer.Tick += new EventHandler(UpdateValues);
            hScrollBar1.Value = (int)esp32.wide;
            hScrollBar2.Value = (int)esp32.height;

        }

        private async void UpdateValues(Object myObject,
                                            EventArgs myEventArgs)
        {
            Console.Write("x = {");
            Array.ForEach(esp32.x, Console.Write);
            Console.Write("} \ny = {");
            Array.ForEach(esp32.y, Console.Write);
            Console.WriteLine("}");

            esp32.UpdateHeightAsync();
            esp32.UpdateWideAsync();

            hScrollBar1.Value = (int)esp32.wide;
            hScrollBar2.Value = (int)esp32.height;

            try { chart1.Series["Line"].Points.Clear(); }
            finally { };
            for (int i = 0; i < 5; i++)
            {

                chart1.Series["Line"].Points.AddXY(esp32.x[i], esp32.y[i]);
                chart1.Series["Line"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            }

        }

        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            esp32.WriteWide(hScrollBar1.Value);
            //esp32.WriteWide(hScrollBar1.Value);
            
        }

        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            esp32.WriteHeight(hScrollBar2.Value);
            //esp32.WriteHeight(hScrollBar2.Value);
        }
    }
}
