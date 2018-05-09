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
        private int _id;

        public string Data { get { return _data; } private set { _data = value; } }
        public int Statuscode { get { return _statuscode; } set { _statuscode = value; } }
        public int Id { get { return _id; } private set { _id = value; } }

        public Message(int Statuscode, string Data)
        {
            this.Data = Data;
            this.Statuscode = Statuscode;
            Id = new Random().Next();
        }

        public Message(string RawData)
        {
            string[] tmp = RawData.Split(';');
            this.Statuscode = Convert.ToInt32(tmp[0]);
            this.Id = Convert.ToInt32(tmp[1]);
            StringBuilder newRaw = new StringBuilder();
            for (int i = 2; i < tmp.Length; i++)
            {
                newRaw.Append(tmp[i] + ";");
            }

            Data = newRaw.Remove(newRaw.Length - 1, 1).ToString();
        }

        public override string ToString()
        {
            return Statuscode + ";" + Id + ";" + Data;
        }
    }
}
