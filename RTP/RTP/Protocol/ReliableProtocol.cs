using RTP.Protocol.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol
{
    public class ReliableProtocol
    {
        public static int SYNC = 100;
        public static int SYNC_ACK = 101;
        public static int ESTAB = 102;
        public static int SEND = 200;
        public static int RECIE = 300;

        public string IPAddress;
        public string MACAddress;
        public Int16 Port;

        public ReliableProtocol(string IPAddress, string MACAddress, Int16 Port)
        {
            this.IPAddress = IPAddress;
            this.MACAddress = MACAddress;
            this.Port = Port;
        }

        public Bitset Send(string Data, Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Message msg = new Message(SEND, Data);
            Segment sgm = new Segment(msg, Port, DestinationPort);
            Packet pkg = new Packet(sgm, IPAddress, DestinationIPAddress);
            Frame frm = new Frame(pkg, MACAddress, DestinationMacAddress);
            Bitset ret = new Bitset(frm);
            return ret;
        }

        public void Recieve(Bitset bitset)
        {
            Frame frm = new Frame(bitset);
            Packet pkg = frm.ActualPacket;
            Segment sgm = pkg.ActualSegment;
            Message msg = sgm.ActualMessage;

            if (msg.Statuscode == SYNC)
            {
                SendSyncAck();
            }
            if (msg.Statuscode == SYNC_ACK)
            {
                SendEstablished();
            }
            if (msg.Statuscode == SEND)
            {
                SendRecieved();
            }
        }

        public void SendRecieved()
        {
            throw new NotImplementedException();
        }

        public void SendEstablished()
        {
            throw new NotImplementedException();
        }

        public void SendSyncAck()
        {
            throw new NotImplementedException();
        }

        public Bitset StartSync()
        {
            throw new NotImplementedException();
        }
    }
}
