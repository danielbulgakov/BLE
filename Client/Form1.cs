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

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        BLEObserver observer = new BLEObserver();

        private void Form1_Load(object sender, EventArgs e)
        {
            observer.Start();
        }


        private async void findButton_Click(object sender, EventArgs e)
        {
            // Add found device in observer to listbox 
            // Check if listbox already contains item
            if (observer.devicesList != null)
            {
                foreach(var item in observer.devicesList)
                {
                    if (!BLEList.Items.Contains(item))
                        BLEList.Items.Add(item);
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            // Client clicked on choosed device
            // and we tell to observer that needed device was found
            observer.DeviceFound(BLEList.SelectedItem.ToString());
        }
    }
}
