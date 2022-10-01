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
        Timer UpdateTimer = new System.Windows.Forms.Timer();

        public DeviceFrame(ulong uuid)
        {
            InitializeComponent();
            esp32 = new BLEesp32(uuid);



            chart1.Series.Add("Sin");
            chart1.Series.Add("Cos");
        }


        private void DeviceFrame_Load(object sender, EventArgs e)
        {
            
            UpdateTimer.Interval = 5000;
            UpdateTimer.Enabled = true;
            UpdateTimer.Tick += new EventHandler(UpdateValues);
            //hScrollBar1.Value = (int)esp32.wide;
            //hScrollBar2.Value = (int)esp32.height;

        }

        private async void UpdateValues(Object myObject,
                                            EventArgs myEventArgs)
        {
            Console.Write("x = {");
            Array.ForEach(esp32.sinx, Console.Write);
            Console.Write("} \ny = {");
            Array.ForEach(esp32.cosx, Console.Write);
            Console.WriteLine("}");

            esp32.UpdateStepCosAsync();
            esp32.UpdateStepSinAsync();



            //hScrollBar1.Value = (int)esp32.wide;
            //hScrollBar2.Value = (int)esp32.height;
            try
            {
                chart1.Series["Sin"].Points.Clear();
                chart1.Series["Cos"].Points.Clear();
            }
            catch (System.NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            float x1 = 0, x2 = 0;

            for (int i = 0; i < esp32.GetPackSize() / 10; i++)
            {
                chart1.Series["Sin"].Points.AddXY(x1, esp32.sinx[i]);
                chart1.Series["Cos"].Points.AddXY(x2, esp32.cosx[i]);
                x1 += esp32.stepSin; x2 += esp32.stepCos;
                chart1.Series["Sin"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                chart1.Series["Cos"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            }

        }





        private void costrackBarChanged(object sender, EventArgs e)
        {
            float val = 1 + (float)( (costrackBar.Value - 1) / 2f);
            cosLabel.Text = "Cos Step = " + val.ToString("0.0");
            esp32.WriteStepCos(val);
        }

        private void sintrackBarChanged(object sender, EventArgs e)
        {
            float val = 1 + (float)((sintrackBar.Value - 1) / 2f);
            sinLabel.Text = "Sin Step = " + val.ToString("0.0");
            esp32.WriteStepSin(val);
        }

        private void onClosing(object sender, FormClosingEventArgs e)
        {
            UpdateTimer.Enabled = false;
        }

        private void onClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
