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
        private string ServiceUUID = "17b5290c-2e8b-11ed-a261-0242ac120002";

        private string xUUID = "17b52c72-2e8b-11eb-a261-0242ac120002";
        private string yUUID = "17b52c72-2e8b-11ef-a261-0242ac120002";
        private string wideUUID = "17b52ea2-2e8b-11ed-a261-0242ac120002";
        private string heightUUID = "17b52fc4-2e8b-11ed-a261-0242ac120002";


        BLEDevice esp32;
        

        public float wide { get; private set; }
        public float height { get; private set; }

        public float[] x { get; private set; } = new float[5];
        public float[] y { get; private set; } = new float[5];

        public Timer UpdateTimer { get; }

        public BLEesp32(ulong uuid)
        {
            esp32 = new BLEDevice(uuid);
            esp32.ConnectAsync();
            
            UpdateTimer = new System.Windows.Forms.Timer();
            UpdateTimer.Interval = 2000;
            UpdateTimer.Enabled = true;
            UpdateTimer.Tick += new EventHandler(Start);
        }

        private async void Start(Object myObject,
                                            EventArgs myEventArgs)
        {
            GattWriteResult res, res1;
            res = await esp32.SubscribeToCharacteristicAsync(ServiceUUID, xUUID, XChanged);
            res1 = await esp32.SubscribeToCharacteristicAsync(ServiceUUID, yUUID, YChanged);
            if (res != null && res1 != null) UpdateTimer.Enabled = false;
        }

        public async void UpdateWideAsync()
        {
            wide = ToFloat(await esp32.ReadRawAsync(ServiceUUID, wideUUID));
            Console.WriteLine("Wide = " + wide.ToString());
        }

        public async void UpdateHeightAsync()
        {
            height = ToFloat(await esp32.ReadRawAsync(ServiceUUID, heightUUID));
            Console.WriteLine("Height = " + height.ToString());
        }

        public void WriteWide(float iwide)
        {
            esp32.WriteRawAsync(ServiceUUID, wideUUID, BitConverter.GetBytes(iwide));
        }

        public void WriteHeight(float iheight)
        {
            esp32.WriteRawAsync(ServiceUUID, heightUUID, BitConverter.GetBytes(iheight));
        }

        private void XChanged (GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            var raw = ReadFromBuffer(args.CharacteristicValue);
            x = ToFloatArray(raw);
        }

        private void YChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            var raw = ReadFromBuffer(args.CharacteristicValue);
            y = ToFloatArray(raw);
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

        public float[] ToFloatArray(byte[] input)
        {
            var len = input.Length * sizeof(byte) / sizeof(float);
            float[] result = new float[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = BitConverter.ToSingle(input, (sizeof(float)*i));
            }
            return result;

        }

        public float ToFloat(byte[] input)
        {
            if (input == null) return 0.0f;
            float result ;
            result = BitConverter.ToSingle(input, 0);
            return result;

        }

        //public async void Connect(ulong uuid)
        //{
        //    esp32 = await BluetoothLEDevice.FromBluetoothAddressAsync(uuid);
        //    props = await esp32.GetGattServicesAsync();

        //    //this.Refresh();

        //    //Debug();
        //}

        //private GattDeviceService FindService(string uuid)
        //{
        //    if (props == null) return null;
        //    if (props.Status == GattCommunicationStatus.Success)
        //    {
        //        var services = props.Services;
        //        foreach (var service in services)
        //        {
        //            if (!service.Uuid.ToString().StartsWith(uuid))
        //            {
        //                continue;
        //            }
        //            else return service;
        //        }
        //    }
        //    return null;
        //}

        //        public async void WriteWide(float val)
        //        {
        //            byte[] send = BitConverter.GetBytes(val);
        //            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
        //            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();
        //            if (characteristicsResult.Status == GattCommunicationStatus.Success)
        //            {
        //var characteristics = characteristicsResult.Characteristics;
        //                foreach (var characteristic in characteristics)
        //                {
        //                    if (characteristic.Uuid.ToString().StartsWith("17b52ea2-2e8b-11ed-a261-0242ac120002"))
        //                    {
        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
        //                        DataWriter dataWriter = new DataWriter();
        //                        dataWriter.WriteBytes(send);
        //                        IBuffer buffer = dataWriter.DetachBuffer();
        //                        GattCommunicationStatus status = await characteristic.WriteValueAsync(buffer);

        //                    }
        //                }
        //            }
        //        }

        //        public async void WriteHeight(float val)
        //        {
        //            byte[] send = BitConverter.GetBytes(val);
        //            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
        //            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();
        //            if (characteristicsResult.Status == GattCommunicationStatus.Success)
        //            {
        //                var characteristics = characteristicsResult.Characteristics;
        //                foreach (var characteristic in characteristics)
        //                {
        //                    if (characteristic.Uuid.ToString().StartsWith("17b52fc4-2e8b-11ed-a261-0242ac120002"))
        //                    {
        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;
        //                        DataWriter dataWriter = new DataWriter();
        //                        dataWriter.WriteBytes(send);
        //                        IBuffer buffer = dataWriter.DetachBuffer();
        //                        GattCommunicationStatus status = await characteristic.WriteValueAsync(buffer);

        //                    }
        //                }
        //            }

        //        }


        //        public async void Refresh()
        //        {
        //            props = await esp32.GetGattServicesAsync();
        //            var s = FindService("17b5290c-2e8b-11ed-a261-0242ac120002");
        //            if (s == null) return;
        //            GattCharacteristicsResult characteristicsResult = await s.GetCharacteristicsAsync();

        //            if (characteristicsResult.Status == GattCommunicationStatus.Success)
        //            {
        //                var characteristics = characteristicsResult.Characteristics;
        //                foreach (var characteristic in characteristics)
        //                {
        //                    if (characteristic.Uuid.ToString().StartsWith("17b52c72-2e8b-11eb-a261-0242ac120002"))
        //                    {
        //                        //Console.WriteLine("1");
        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

        //                        if (properties.HasFlag(GattCharacteristicProperties.Read))
        //                        {
        //                            GattReadResult gattResult = await characteristic.ReadValueAsync();


        //                            if (gattResult.Status == GattCommunicationStatus.Success)
        //                            {
        //                                var reader = DataReader.FromBuffer(gattResult.Value);
        //                                byte[] input = new byte[reader.UnconsumedBufferLength];
        //                                reader.ReadBytes(input);

        //                                x[0] = BitConverter.ToSingle(input, 0);
        //                                x[1] = BitConverter.ToSingle(input, 4);
        //                                x[2] = BitConverter.ToSingle(input, 8);
        //                                x[3] = BitConverter.ToSingle(input, 12);
        //                                x[4] = BitConverter.ToSingle(input, 16);

        //                                    Console.WriteLine("argx");
        //                                for (int i = 0; i < 5; i++)
        //                                {
        //                                    Console.WriteLine(x[i]);
        //                                }


        //                            }
        //                        }
        //                    }

        //                    if (characteristic.Uuid.ToString().StartsWith("17b52c72-2e8b-11ef-a261-0242ac120002"))
        //                    {
        //                        //Console.WriteLine("2");

        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

        //                        if (properties.HasFlag(GattCharacteristicProperties.Read))
        //                        {
        //                            GattReadResult gattResult = await characteristic.ReadValueAsync();
        //                            if (gattResult.Status == GattCommunicationStatus.Success)
        //                            {
        //                                Console.WriteLine("im here y");

        //                                var reader = DataReader.FromBuffer(gattResult.Value);
        //                                byte[] input = new byte[reader.UnconsumedBufferLength];
        //                                reader.ReadBytes(input);

        //                                y[0] = BitConverter.ToSingle(input, 0);
        //                                y[1] = BitConverter.ToSingle(input, 4);
        //                                y[2] = BitConverter.ToSingle(input, 8);
        //                                y[3] = BitConverter.ToSingle(input, 12);
        //                                y[4] = BitConverter.ToSingle(input, 16);

        //                                    Console.WriteLine("argy");
        //                                for (int i = 0; i < 5; i++)
        //                                {
        //                                    Console.WriteLine(y[i]);
        //                                }

        ///*                                System.Buffer.BlockCopy(input, 0, y, 0, input.Length);
        //                                for (int i = 0; i < y.Length; i++)
        //                                {
        //                                    //y[i] = (float)Math.Round(y[i], 2);
        //                                    Console.WriteLine(y[i]);
        //                                }*//*                                System.Buffer.BlockCopy(input, 0, y, 0, input.Length);
        //                                for (int i = 0; i < y.Length; i++)
        //                                {
        //                                    //y[i] = (float)Math.Round(y[i], 2);
        //                                    Console.WriteLine(y[i]);
        //                                }*/

        //                            }
        //                        }
        //                    }

        //                    if (characteristic.Uuid.ToString().StartsWith("17b52ea2-2e8b-11ed-a261-0242ac120002"))
        //                    {
        //                        //Console.WriteLine("3");
        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

        //                        if (properties.HasFlag(GattCharacteristicProperties.Read))
        //                        {
        //                            GattReadResult gattResult = await characteristic.ReadValueAsync();
        //                            if (gattResult.Status == GattCommunicationStatus.Success)
        //                            {
        //                                //Console.WriteLine("im here wide");
        //                                var reader = DataReader.FromBuffer(gattResult.Value);
        //                                byte[] input = new byte[reader.UnconsumedBufferLength];
        //                                reader.ReadBytes(input);
        //                                if (input.Length == 1)
        //                                {
        //                                    wide = (float)(input[0] - '0');
        //                                }
        //                                else
        //                                {
        //                                    wide = BitConverter.ToSingle(input, 0);
        //                                }
        //                                //Console.WriteLine("wide");
        //                                //Console.WriteLine(wide);


        //                            }
        //                        }
        //                    }

        //                    if (characteristic.Uuid.ToString().StartsWith("17b52fc4-2e8b-11ed-a261-0242ac120002"))
        //                    {
        //                        //Console.WriteLine("4");
        //                        GattCharacteristicProperties properties = characteristic.CharacteristicProperties;

        //                        if (properties.HasFlag(GattCharacteristicProperties.Read))
        //                        {
        //                            GattReadResult gattResult = await characteristic.ReadValueAsync();
        //                            if (gattResult.Status == GattCommunicationStatus.Success)
        //                            {
        //                                //Console.WriteLine("im here wide");
        //                                var reader = DataReader.FromBuffer(gattResult.Value);
        //                                byte[] input = new byte[reader.UnconsumedBufferLength];
        //                                reader.ReadBytes(input);
        //                                if (input.Length == 1)
        //                                {
        //                                    height = (float)(input[0] - '0');
        //                                }
        //                                else
        //                                {
        //                                    height = BitConverter.ToSingle(input, 0);
        //                                }
        //                                //Console.WriteLine("wide");
        //                                //Console.WriteLine(height);


        //                            }
        //                        }
        //                    }

        //                }

        //            }
        //        }


        //        private async void Debug()
        //        {
        //            GattDeviceServicesResult result = await esp32.GetGattServicesAsync();
        //            if (result.Status == GattCommunicationStatus.Success)
        //            {
        //                var services = result.Services;
        //                foreach (var service in services)
        //                {
        //                    Console.Write("Service Information");
        //                    Console.WriteLine(" UUID :" + service.Uuid.ToString());
        //                    GattCharacteristicsResult characteristicsResult = await service.GetCharacteristicsAsync();
        //                    if (characteristicsResult.Status == GattCommunicationStatus.Success)
        //                    {
        //                        var characteristics = characteristicsResult.Characteristics;
        //                        foreach (var characteristic in characteristics)
        //                        {
        //                            Console.Write("Characteristic Information");
        //                            Console.WriteLine(" UUID :" + characteristic.Uuid.ToString());
        //                        }

        //                    }
        //                }
        //            }
        //        }

    }
}
