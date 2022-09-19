using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BLE
{
    internal class BLEDevice : IEquatable<BLEDevice>
    {
        public string Name { set; get; }
        public string MetaData { set; get; }

        public ulong UUID { get; }

        public BLEDevice(string name, ulong uUID, string metaData = "")
        {
            Name = name;
            MetaData = metaData;
            UUID = uUID;
        }

        public bool Equals(BLEDevice obj)
        {
            if (obj.UUID == this.UUID) return true;
            else return false;
        }
    }

    internal class DeviceServices
    {
        string UUID;
        
    }

    internal class DeviceCharacteristics
    {
        string UUID;

    }
}
