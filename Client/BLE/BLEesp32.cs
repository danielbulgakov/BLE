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
using System.IO;

namespace Client.BLE
{
    public class BLEesp32
    {
        private string ServiceUUID = "17b5290c-2e8b-11ed-a261-0242ac120002";

        private string xUUID = "17b52c72-2e8b-11eb-a261-0242ac120002";
        private string yUUID = "17b52c72-2e8b-11ef-a261-0242ac120002";
        private string wideUUID = "17b52ea2-2e8b-11ed-a261-0242ac120002";
        private string heightUUID = "17b52fc4-2e8b-11ed-a261-0242ac120002";

        BLEDevice esp32;

        public const int packSize = 100;
        public int GetPackSize() { return packSize; }

        public float stepSin { get; private set; }
        public float stepCos { get; private set; }

        public float[] sinx { get; private set; } = new float[packSize];
        public float[] cosx { get; private set; } = new float[packSize];
        
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

        public async void UpdateStepSinAsync()
        {
            //stepSin = ToFloat(await esp32.ReadRawAsync(ServiceUUID, wideUUID));
            //Console.WriteLine("Wide = " + stepSin.ToString());
        }

        public async void UpdateStepCosAsync()
        {
            //stepCos = ToFloat(await esp32.ReadRawAsync(ServiceUUID, heightUUID));
            //Console.WriteLine("Height = " + stepCos.ToString());
        }

        public void WriteStepSin(float stepSin)
        {
            this.stepSin = stepSin;
            esp32.WriteRawAsync(ServiceUUID, wideUUID, BitConverter.GetBytes(stepSin));
        }

        public void WriteStepCos(float stepCos)
        {
            this.stepCos = stepCos;
            esp32.WriteRawAsync(ServiceUUID, heightUUID, BitConverter.GetBytes(stepCos));
        }

        private void XChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            var raw = ReadFromBuffer(args.CharacteristicValue);
            sinx = ToFloatArray(raw);

            toLog("..\\..\\logs\\Xlog.txt", "X", sinx);

        }

        private void YChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {

            var raw = ReadFromBuffer(args.CharacteristicValue);
            cosx = ToFloatArray(raw);



            toLog("..\\..\\logs\\Ylog.txt", "Y", cosx);
        }


        private void toLog(string path, string className, float[] items)
        {
            string log = "[" + DateTime.Now.ToString("hh.mm.ss.ffffff") + "]";
            log += " BLE Client " + className + " class recieved data [";
            foreach (var elem in items) log += elem.ToString() + " ";
            log += "]\n";
            File.AppendAllText(path, log);
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
                result[i] = BitConverter.ToSingle(input, (sizeof(float) * i));
            }
            return result;

        }

        public float ToFloat(byte[] input)
        {
            if (input == null) return 0.0f;
            float result;
            result = BitConverter.ToSingle(input, 0);
            return result;

        }



    }
    //internal class BLEesp32
    //{
    //    private string ServiceUUID = "17b5290c-2e8b-11ed-a261-0242ac120002";

    //    private string xUUID = "17b52c72-2e8b-11eb-a261-0242ac120002";
    //    private string yUUID = "17b52c72-2e8b-11ef-a261-0242ac120002";
    //    private string wideUUID = "17b52ea2-2e8b-11ed-a261-0242ac120002";
    //    private string heightUUID = "17b52fc4-2e8b-11ed-a261-0242ac120002";


    //    BLEDevice esp32;


    //    public float wide { get; private set; }
    //    public float height { get; private set; }

    //    public float[] x { get; private set; } = new float[5];
    //    public float[] y { get; private set; } = new float[5];

    //    public Timer UpdateTimer { get; }

    //    public BLEesp32(ulong uuid)
    //    {
    //        esp32 = new BLEDevice(uuid);
    //        esp32.ConnectAsync();

    //        UpdateTimer = new System.Windows.Forms.Timer();
    //        UpdateTimer.Interval = 2000;
    //        UpdateTimer.Enabled = true;
    //        UpdateTimer.Tick += new EventHandler(Start);
    //    }

    //    private async void Start(Object myObject,
    //                                        EventArgs myEventArgs)
    //    {
    //        GattWriteResult res, res1;
    //        res = await esp32.SubscribeToCharacteristicAsync(ServiceUUID, xUUID, XChanged);
    //        res1 = await esp32.SubscribeToCharacteristicAsync(ServiceUUID, yUUID, YChanged);
    //        if (res != null && res1 != null) UpdateTimer.Enabled = false;
    //    }

    //    public async void UpdateWideAsync()
    //    {
    //        wide = ToFloat(await esp32.ReadRawAsync(ServiceUUID, wideUUID));
    //        Console.WriteLine("Wide = " + wide.ToString());
    //    }

    //    public async void UpdateHeightAsync()
    //    {
    //        height = ToFloat(await esp32.ReadRawAsync(ServiceUUID, heightUUID));
    //        Console.WriteLine("Height = " + height.ToString());
    //    }

    //    public void WriteWide(float iwide)
    //    {
    //        wide = iwide;
    //        esp32.WriteRawAsync(ServiceUUID, wideUUID, BitConverter.GetBytes(iwide));
    //    }

    //    public void WriteHeight(float iheight)
    //    {
    //        height = iheight;
    //        esp32.WriteRawAsync(ServiceUUID, heightUUID, BitConverter.GetBytes(iheight));
    //    }

    //    private void XChanged (GattCharacteristic sender, GattValueChangedEventArgs args)
    //    {

    //        var raw = ReadFromBuffer(args.CharacteristicValue);
    //        x = ToFloatArray(raw);
    //    }

    //    private void YChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
    //    {

    //        var raw = ReadFromBuffer(args.CharacteristicValue);
    //        y = ToFloatArray(raw);
    //    }



    //    public static IBuffer WriteToBuffer(byte[] send)
    //    {

    //        DataWriter dataWriter = new DataWriter();
    //        dataWriter.WriteBytes(send);
    //        IBuffer buffer = dataWriter.DetachBuffer();
    //        return buffer;
    //    }

    //    public static byte[] ReadFromBuffer(IBuffer buff)
    //    {
    //        var reader = DataReader.FromBuffer(buff);
    //        byte[] input = new byte[reader.UnconsumedBufferLength];
    //        reader.ReadBytes(input);
    //        return input;
    //    }

    //    public float[] ToFloatArray(byte[] input)
    //    {
    //        var len = input.Length * sizeof(byte) / sizeof(float);
    //        float[] result = new float[len];
    //        for (int i = 0; i < len; i++)
    //        {
    //            result[i] = BitConverter.ToSingle(input, (sizeof(float)*i));
    //        }
    //        return result;

    //    }

    //    public float ToFloat(byte[] input)
    //    {
    //        if (input == null) return 0.0f;
    //        float result ;
    //        result = BitConverter.ToSingle(input, 0);
    //        return result;

    //    }


}

