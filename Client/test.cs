using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace Client
{
    public class BluetoothObserver
    {
        BluetoothLEAdvertisementWatcher Watcher { get; set; }
        public void Start()
        {
            Watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active
            };
            Watcher.Received += Watcher_Received;
            Watcher.Stopped += Watcher_Stopped;
            Watcher.Start();
        }
        private bool isFindDevice { get; set; } = false;
        private async void Watcher_Received(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args)
        {
            if (isFindDevice)
                return;
            if (args.Advertisement.LocalName.Contains("TEST_ESP32"))
            {
                isFindDevice = true;
                BluetoothLEDevice bluetoothLeDevice = await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);
                GattDeviceServicesResult result = await bluetoothLeDevice.GetGattServicesAsync();
                if (result.Status == GattCommunicationStatus.Success)
                {
                    var services = result.Services;
                    foreach (var service in services)
                    {
                        if (!service.Uuid.ToString().StartsWith("17b5290c-2e8b-11ed-a261-0242ac120002"))
                        {
                            continue;
                        }
                        GattCharacteristicsResult characteristicsResult = await service.GetCharacteristicsAsync();
                        if (characteristicsResult.Status == GattCommunicationStatus.Success)
                        {
                            var characteristics = characteristicsResult.Characteristics;
                            foreach (var characteristic in characteristics)
                            {
                                if (!characteristic.Uuid.ToString().StartsWith("17b52c72-2e8b-11eb-a261-0242ac120002"))
                                {
                                    continue;
                                }
                                GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
                                if (properties.HasFlag(GattCharacteristicProperties.Indicate))
                                {
                                    characteristic.ValueChanged += Characteristic_ValueChanged;
                                    GattWriteResult status = await characteristic.WriteClientCharacteristicConfigurationDescriptorWithResultAsync(GattClientCharacteristicConfigurationDescriptorValue.Indicate);
                                    return;
                                }
                                if (properties.HasFlag(GattCharacteristicProperties.Read))
                                {
                                    GattReadResult gattResult = await characteristic.ReadValueAsync();
                                    if (gattResult.Status == GattCommunicationStatus.Success)
                                    {
                                        Console.WriteLine("im here");
                                        var reader = DataReader.FromBuffer(gattResult.Value);
                                        byte[] input = new byte[reader.UnconsumedBufferLength];
                                        reader.ReadBytes(input);
                                        var floatArray2 = new float[input.Length / 4];
                                        System.Buffer.BlockCopy(input, 0, floatArray2, 0, input.Length);
                                        foreach (var item in floatArray2)
                                        {
                                            Console.WriteLine(item.ToString());
                                        }
                                        //Читаем input
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            var reader = DataReader.FromBuffer(args.CharacteristicValue);
            byte[] input = new byte[reader.UnconsumedBufferLength];
            reader.ReadBytes(input);
            //Читаем input
        }
        private void Watcher_Stopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            ;
        }
    }
}
