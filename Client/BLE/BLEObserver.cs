using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

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
            watcher.Received += WatcherReceived;
            watcher.Start();

        }

        private async void WatcherReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (isFindDevice)
                return;
            if (!devicesList.Contains(args.Advertisement.LocalName) && args.Advertisement.LocalName != "")
            {
                devicesList.Add(args.Advertisement.LocalName);
            }
            
        }

        public void DeviceFound(String name)
        {
            isFindDevice = true;

        }
    }
}
