using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Windows.Storage.Streams;
using Windows.Media.Capture;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Forms;

namespace Client.BLE
{
    internal class BLEesp32
    {
        BluetoothLEDevice esp32;
        GattDeviceServicesResult props;
        public float wide { get; set; }
        public float height { get; set; }

        public float[] x { get; private set; } = new float[5];
        public float[] y { get; private set; } = new float[5];

        public async void Connect(ulong uuid)
        {
            esp32 = await BluetoothLEDevice.FromBluetoothAddressAsync(uuid);
            props = await esp32.GetGattServicesAsync();
            
            this.Refresh();
         
            //Debug();
        }

        private GattDeviceService FindService(string uuid)
        {
            if (props.Status == GattCommunicationStatus.Success)
            {
                var services = props.Services;
                foreach (var service in services)
                {
                    if (!service.Uuid.ToString().StartsWith(uuid))
                    {
                        continue;
                    }
                    else return service;
                }
            }
            return null;
        }

        public async void WriteWide(float val)
        {
            byte[] send = BitConverter.GetBytes(val);
            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();
            if (characteristicsResult.Status == GattCommunicationStatus.Success)
            {
                var characteristics = characteristicsResult.Characteristics;
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.Uuid.ToString().StartsWith("17b52ea2-2e8b-11ed-a261-0242ac120002"))
                    {
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
                        DataWriter dataWriter = new DataWriter();
                        dataWriter.WriteBytes(send);
                        IBuffer buffer = dataWriter.DetachBuffer();
                        GattCommunicationStatus status = await characteristic.WriteValueAsync(buffer);

                    }
                }
            }
        }

        public async void WriteHeight(float val)
        {
            byte[] send = BitConverter.GetBytes(val);
            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();
            if (characteristicsResult.Status == GattCommunicationStatus.Success)
            {
                var characteristics = characteristicsResult.Characteristics;
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.Uuid.ToString().StartsWith("17b52fc4-2e8b-11ed-a261-0242ac120002"))
                    {
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
                        DataWriter dataWriter = new DataWriter();
                        dataWriter.WriteBytes(send);
                        IBuffer buffer = dataWriter.DetachBuffer();
                        GattCommunicationStatus status = await characteristic.WriteValueAsync(buffer);

                    }
                }
            }

        }


        public async void Refresh()
        {
            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();
            if (characteristicsResult.Status == GattCommunicationStatus.Success)
            {
                var characteristics = characteristicsResult.Characteristics;
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.Uuid.ToString().StartsWith("17b52c72-2e8b-11eb-a261-0242ac120002"))
                    {
                        Console.WriteLine("1");
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            GattReadResult gattResult = await characteristic.ReadValueAsync();
                            if (gattResult.Status == GattCommunicationStatus.Success)
                            {
                                Console.WriteLine("im here x");
                                var reader = DataReader.FromBuffer(gattResult.Value);
                                byte[] input = new byte[reader.UnconsumedBufferLength];
                                reader.ReadBytes(input);

                                System.Buffer.BlockCopy(input, 0, x, 0, input.Length);
                               
                                for (int i = 0; i < x.Length; i++)
                                {
                                    x[i] = (float)Math.Round(x[i], 2);
                                }
                                

                            }
                        }
                    }

                    if (characteristic.Uuid.ToString().StartsWith("17b52c72-2e8b-11ef-a261-0242ac120002"))
                    {
                        Console.WriteLine("2");

                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            GattReadResult gattResult = await characteristic.ReadValueAsync();
                            if (gattResult.Status == GattCommunicationStatus.Success)
                            {
                                Console.WriteLine("im here y");
                                var reader = DataReader.FromBuffer(gattResult.Value);
                                byte[] input = new byte[reader.UnconsumedBufferLength];
                                reader.ReadBytes(input);
                                Console.WriteLine(input);
                                System.Buffer.BlockCopy(input, 0, y, 0, input.Length);
                                for (int i = 0; i < y.Length; i++)
                                {
                                    y[i] = (float)Math.Round(y[i], 2);
                                }

                            }
                        }
                    }

                    if (characteristic.Uuid.ToString().StartsWith("17b52ea2-2e8b-11ed-a261-0242ac120002"))
                    {
                        Console.WriteLine("3");
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            GattReadResult gattResult = await characteristic.ReadValueAsync();
                            if (gattResult.Status == GattCommunicationStatus.Success)
                            {
                                Console.WriteLine("im here wide");
                                var reader = DataReader.FromBuffer(gattResult.Value);
                                byte[] input = new byte[reader.UnconsumedBufferLength];
                                reader.ReadBytes(input);
                                if (input.Length == 1)
                                {
                                    wide = (float)(input[0] - '0');
                                }
                                else
                                {
                                    wide = BitConverter.ToSingle(input, 0);
                                }
                                Console.WriteLine("wide");
                                Console.WriteLine(wide);


                            }
                        }
                    }

                    if (characteristic.Uuid.ToString().StartsWith("17b52fc4-2e8b-11ed-a261-0242ac120002"))
                    {
                        Console.WriteLine("4");
                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

                        if (properties.HasFlag(GattCharacteristicProperties.Read))
                        {
                            GattReadResult gattResult = await characteristic.ReadValueAsync();
                            if (gattResult.Status == GattCommunicationStatus.Success)
                            {
                                Console.WriteLine("im here wide");
                                var reader = DataReader.FromBuffer(gattResult.Value);
                                byte[] input = new byte[reader.UnconsumedBufferLength];
                                reader.ReadBytes(input);
                                if (input.Length == 1)
                                {
                                    height = (float)(input[0] - '0');
                                }
                                else
                                {
                                    height = BitConverter.ToSingle(input, 0);
                                }
                                Console.WriteLine("wide");
                                Console.WriteLine(height);


                            }
                        }
                    }

                }

            }
        }
        
        
        private async void Debug()
        {
            GattDeviceServicesResult result = await esp32.GetGattServicesAsync();
            if (result.Status == GattCommunicationStatus.Success)
            {
                var services = result.Services;
                foreach (var service in services)
                {
                    Console.Write("Service Information");
                    Console.WriteLine(" UUID :" + service.Uuid.ToString());
                    GattCharacteristicsResult characteristicsResult = await service.GetCharacteristicsAsync();
                    if (characteristicsResult.Status == GattCommunicationStatus.Success)
                    {
                        var characteristics = characteristicsResult.Characteristics;
                        foreach (var characteristic in characteristics)
                        {
                            Console.Write("Characteristic Information");
                            Console.WriteLine(" UUID :" + characteristic.Uuid.ToString());
                        }

                    }
                }
            }
        }

    }
}
