using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace Client.BLE
{
    internal class BLEObserver
    {
        BluetoothLEAdvertisementWatcher watcher { get; set; }
        public List<String> devicesList { get; set; } = new List<String>();
        private bool isFindDevice { get; set; } = false;

        public async void Start()
        {

            watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            watcher.Received += Watcher_Received;
            watcher.Start();

        }

        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (isFindDevice)
                return;
            if (!devicesList.Contains(args.Advertisement.LocalName) && args.Advertisement.LocalName != "")
            {
                devicesList.Add(args.Advertisement.LocalName);
            }
            
        }
    }
}
