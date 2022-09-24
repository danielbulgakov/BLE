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
        BluetoothLEAdvertisementWatcher Watcher { get; set; }
        public List<BLEDevice> DevicesList { get; set; } = new List<BLEDevice>();
        private bool isFindDevice { get; set; } = false;
        public bool isActive { get; set; } = false; 

        public void Start()
        {
            this.isActive = true;
            Watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            Watcher.Received += WatcherReceived;
            Watcher.Stopped += WatcherStopped;
            Watcher.Start();

        }

        private void WatcherReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (isFindDevice) return;

            
            if ((DevicesList.Count == 0 || !DevicesList.Any(var => var.Equals(args.BluetoothAddress)))
                    && args.Advertisement.LocalName != "")
            {
                DevicesList.Add(new BLEDevice(args.BluetoothAddress, args.Advertisement.LocalName));
            }
            
        }

        private void WatcherStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            DevicesList.Remove(DevicesList.Find(var => args.Equals(var)));
        }

    }
}
