using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol.DataSet
{
    public class Packet
    {
        private Segment _actualSegment;
        private string _sourceIPAddress;
        private string _destinationIPAddress;
       
        public Segment ActualSegment { get { return _actualSegment; } private set { _actualSegment = value; } }
        public string SourceIPAddress { get { return _sourceIPAddress; } private set { _sourceIPAddress = value; } }
        public string DestinationIPAddress { get { return _destinationIPAddress; } private set { _destinationIPAddress = value; } }

        public Packet(Segment ActualSegment, string SourceIPAddress, string DestinationIPAddress)
        {
            this.ActualSegment = ActualSegment;
            this.SourceIPAddress = SourceIPAddress;
            this.DestinationIPAddress = DestinationIPAddress;
        }

        public Packet(string raw)
        {
            string[] tmp = raw.Split(';');
            SourceIPAddress = tmp[0];
            DestinationIPAddress = tmp[1];
            StringBuilder newRaw = new StringBuilder();
            for (int i = 2; i < tmp.Length; i++)
            {
                newRaw.Append(tmp[i] + ";");
            }

            ActualSegment = new Segment(newRaw.Remove(newRaw.Length - 1, 1).ToString());
        }

        public override string ToString()
        {
            return SourceIPAddress+";"+DestinationIPAddress+";"+ ActualSegment.ToString();
        }
    }
}
