using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol.DataSet
{
    public class Bitset
    {
        private string _bits;

        public string Bits { get { return _bits; } private set { _bits = value; } }

        public Bitset(Frame frame)
        {
            _bits = ConvertToBinary(frame.ToString());
        }

        private string ConvertToBinary(string text)
        {

            StringBuilder ret = new StringBuilder();
            bool useful = true;
            foreach (byte b in Encoding.Unicode.GetBytes(text))
            {
                if (!useful)
                {
                    useful = true;
                    continue;
                }
                useful = false;
                string tmp = Convert.ToString(b, 2);
                while (tmp.Length < 8)
                {
                    tmp = "0" + tmp;
                }
                ret.Append(tmp);
            }
            return ret.ToString();
        }

        public override string ToString()
        {
            return Bits;
        }

        public string GetDecodedData()
        {
            StringBuilder ret = new StringBuilder();

            for (int i = 0; i < Bits.Length; i = i + 8)
            {
                byte[] tmpBits = GetBytes(Bits.Substring(i,8));
                ret.Append(System.Text.Encoding.UTF8.GetString(tmpBits));
            }

            return ret.ToString();
        }

        private static byte[] GetBytes(string bitString)
        {
            byte[] result = Enumerable.Range(0, bitString.Length / 8).
                Select(pos => Convert.ToByte(
                    bitString.Substring(pos * 8, 8),
                    2)
                ).ToArray();

            List<byte> mahByteArray = new List<byte>();
            for (int i = result.Length - 1; i >= 0; i--)
            {
                mahByteArray.Add(result[i]);
            }

            return mahByteArray.ToArray();
        }

        private static String ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = bits.Count - 1; i >= 0; i--)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
