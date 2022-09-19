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

namespace Client
{
    public partial class StartFrame : Form
    {
        public StartFrame()
        {
            InitializeComponent();
        }

        public BLEController bLEController = new BLEController();
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        
        private Timer UpdateTimer = new System.Windows.Forms.Timer();


        private void findButton_Click(object sender, EventArgs e)
        {
            // Add found device in observer to listbox 
            // Check if listbox already contains item
            UpdateTimer.Interval = 200;
            UpdateTimer.Enabled = true;
            UpdateTimer.Tick += new EventHandler(UpdateValues);

            
        }

        private void UpdateValues(Object myObject,
                                            EventArgs myEventArgs)
        {
            bLEController.ObserveStart();
            
            var devicesList = bLEController.GetDeviceNames();
            if (devicesList != null)
            {
                foreach (var item in devicesList)
                {
                    if (!BLEList.Items.Contains(item.Item1 + " UUID : " + item.Item2))
                        BLEList.Items.Add(item.Item1 + " UUID : " + item.Item2);
                }
            }

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Client clicked on choosed device
            // and we tell to observer that needed device was found
            //observer.DeviceFound(BLEList.SelectedItem.ToString());
            UpdateTimer.Enabled = false;
            string item = BLEList.SelectedItem.ToString();
            bLEController.ConnectDevice = (ulong.Parse(item.Substring(item.LastIndexOf(" ") + 1)));
            DeviceFrame frame = new DeviceFrame(bLEController.ConnectDevice);
            frame.Show();
            this.Hide();
        }
    }
}
