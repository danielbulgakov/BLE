using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BLE
{
    internal class ParsePack
    {
        public string Head { get; private set; }
        public int Num { get; private set; }
        public int Mcu { get; private set; }
        public float[] Data { get; private set; } = new float[100];

        public void Parse(byte[] buff)
        {
            this.Head = Encoding.Default.GetString(buff).Substring(0,8);
            var b = buff.Skip(8).ToArray(); 
            this.Num = BitConverter.ToInt32(b, 0);
            b = buff.Skip(4).ToArray();
            this.Mcu = BitConverter.ToInt16(b, 0);
            b = buff.Skip(3).ToArray();
            b = b.Take(400).ToArray();
            this.Data = ToFloatArray(b);
        }

        public string toLog()
        {
            if (Head == null && Num == null && Mcu == null && Data == null) return "";
            StringBuilder sb = new StringBuilder();
            sb.Append(Head);
            sb.Append(' ');
            sb.Append(Num);
            sb.Append(' ');
            sb.Append(Mcu);
            sb.Append(' ');
            for (int i = 0; i < Data.Length; i++)
            {
                sb.Append(Data[i]);
                sb.Append(' ');
            }
            return sb.ToString();

        }

        private float[] ToFloatArray(byte[] input)
        {
            var len = input.Length * sizeof(byte) / sizeof(float);
            float[] result = new float[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = BitConverter.ToSingle(input, (sizeof(float) * i));
            }
            return result;

        }
    }
}
