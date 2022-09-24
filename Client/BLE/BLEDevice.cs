using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Storage.Streams;

namespace Client.BLE
{
    internal class BLEDevice
    {
        public ulong UUID { get; }
        public string Name { get; }

        private GattDeviceServicesResult properties;
        private BluetoothLEDevice device;  

        public BLEDevice(ulong uUID, string name = "")
        {
            UUID = uUID;
            Name = name;
            
        }

        public bool Equals(BLEDevice obj)
        {
            if (obj.UUID == this.UUID) return true;
            else return false;
        }

        public async void ConnectAsync()
        {
            device = await BluetoothLEDevice.FromBluetoothAddressAsync(this.UUID);
            properties = await device.GetGattServicesAsync();
        }

        public async Task<GattWriteResult> SubscribeToCharacteristicAsync(string serviceUUID, string characterUUID, TypedEventHandler<GattCharacteristic, GattValueChangedEventArgs> f)
        {
            if (properties == null) return null;
            var service = FindService(properties.Services, serviceUUID);
            if (service == null) return null;
            var charResult = await service.GetCharacteristicsAsync();
            
            if (charResult.Status == GattCommunicationStatus.Success )
            {
                var characteristic = FindCharacteristic(charResult.Characteristics, characterUUID);
                var Flags = characteristic.CharacteristicProperties;
                if (Flags.HasFlag(GattCharacteristicProperties.Notify))
                {
                    characteristic.ValueChanged += f;
                    GattWriteResult status = await characteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                    return status;
                }
            }
            return null;

        }

        public async Task<byte[]> ReadRawAsync(string serviceUUID, string characterUUID)
        {
            byte[] result;
            var service = FindService(properties.Services, serviceUUID);
            if (service == null) return null;
            var charResult = await service.GetCharacteristicsAsync();
            if (charResult.Status == GattCommunicationStatus.Success)
            {
                var characteristic = FindCharacteristic(charResult.Characteristics, characterUUID);
                result = await ReadBufferToByteArrayAsync(characteristic);
                return result;
            }   
            return null;
        }

        public async void WriteRawAsync(string serviceUUID, string characterUUID, byte[] data)
        {
            if (properties == null) return;
            var service = FindService(properties.Services, serviceUUID);
            if (service == null) return;
            var charResult = await service.GetCharacteristicsAsync();
            if (charResult.Status == GattCommunicationStatus.Success)
            {
                var characteristic = FindCharacteristic(charResult.Characteristics, characterUUID);
                var toSend = WriteToBuffer(data);
                await characteristic.WriteValueAsync(toSend);
            }
               
        }

        private GattDeviceService FindService(IReadOnlyList<GattDeviceService> list, string uuid)
        {
            foreach(var item in list)
            {
                if (item.Uuid.ToString().StartsWith(uuid)) return item;
            }
            return null;
            
        }

        private GattCharacteristic FindCharacteristic(IReadOnlyList<GattCharacteristic> list, string uuid)
        {
            foreach (var item in list)
            {
                if (item.Uuid.ToString().StartsWith(uuid)) return item;
            }
            return null;

        }

        private async Task<byte[]> ReadBufferToByteArrayAsync(GattCharacteristic characteristic)
        {
            var Flags = characteristic.CharacteristicProperties;
            if (Flags.HasFlag(GattCharacteristicProperties.Read))
            {
                GattReadResult gattResult = await characteristic.ReadValueAsync();
                if (gattResult.Status == GattCommunicationStatus.Success)
                {

                    var reader = DataReader.FromBuffer(gattResult.Value);
                    byte[] input = new byte[reader.UnconsumedBufferLength];
                    reader.ReadBytes(input);
                    return input;

                }
            }
            return null;
        }
 

        public static IBuffer WriteToBuffer(byte[] send)
        {
            
            DataWriter dataWriter = new DataWriter();
            dataWriter.WriteBytes(send);
            IBuffer buffer = dataWriter.DetachBuffer();
            return buffer;
        }

        public static byte[] ReadFromBuffer(IBuffer buff)
        {
            var reader = DataReader.FromBuffer(buff);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);
            return input;
        } 



    }

}
