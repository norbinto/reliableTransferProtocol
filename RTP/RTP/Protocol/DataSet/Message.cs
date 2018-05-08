using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol.DataSet
{
    public class Message
    {
        private string _data;
        private int _statuscode;

        public string Data { get { return _data; } private set { _data = value; } }
        public int Statuscode { get { return _statuscode; } set { _statuscode = value; } }

        public Message(int Statuscode, string Data)
        {
            this.Data = Data;
            this.Statuscode = Statuscode;
        }

        public Message(string RawData)
        {
            string[] tmp = RawData.Split(';');
            this.Statuscode = Convert.ToInt16(tmp[0]);
            StringBuilder newRaw = new StringBuilder();
            for (int i = 1; i < tmp.Length; i++)
            {
                newRaw.Append(tmp[i] + ";");
            }

            Data = newRaw.Remove(newRaw.Length - 1, 1).ToString();
        }

        public override string ToString()
        {
            return Statuscode + ";" + Data;
        }
    }
}
