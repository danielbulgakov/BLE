using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Client.BLE
{
    public class BLEList
    {
        BLEObserver BLEObserver;
        public ulong ConnectDevice { set; get; } 
        
        public void ObserveStart() 
        {
            if (BLEObserver != null) return;
            BLEObserver = new BLEObserver();
            BLEObserver.Start();
        }

        public List<Tuple<string, ulong>> GetDeviceNames()
        {
            List<Tuple<string, ulong>> deviceNames = new List<Tuple<string, ulong>>();
            if (BLEObserver.DevicesList != null)
            foreach (var item in BLEObserver.DevicesList)
            {
                deviceNames.Add(new Tuple<string, ulong> (item.Name, item.UUID));
            }
            return deviceNames;
        }

        
    }
    
}
