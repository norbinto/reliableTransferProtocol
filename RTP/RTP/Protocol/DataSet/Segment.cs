using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol.DataSet
{
    public class Segment
    {
        private Message _actualMessage;
        private Int16 _sourcePort;
        private Int16 _destinationPort;


        public Message ActualMessage { get { return _actualMessage; } private set { _actualMessage = value; } }
        public Int16 SourcePort { get { return _sourcePort; } private set { _sourcePort = value; } }
        public Int16 DestinationPort { get { return _destinationPort; } private set { _destinationPort = value; } }

        public Segment(Message ActualMessage,Int16 SourcePort,Int16 DestinationPort)
        {
            this.ActualMessage = ActualMessage;
            this.SourcePort =SourcePort;
            this.DestinationPort = DestinationPort;
        }

        public Segment(string raw)
        {
            string[] tmp = raw.Split(';');
            SourcePort = Convert.ToInt16(tmp[0]);
            DestinationPort = Convert.ToInt16(tmp[1]);
            StringBuilder newRaw = new StringBuilder();
            for (int i = 2; i < tmp.Length; i++)
            {
                newRaw.Append(tmp[i] + ";");
            }

            ActualMessage = new Message(newRaw.Remove(newRaw.Length - 1, 1).ToString());
        }

        public override string ToString()
        {
            return SourcePort+";"+DestinationPort+";"+ActualMessage.ToString();
        }
    }
}
