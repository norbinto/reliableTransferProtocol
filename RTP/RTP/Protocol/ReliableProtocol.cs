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
        public static int READY = 103;
        public static int SEND = 200;
        public static int RECIE = 300;
        public static int REJECT = 500;

        public string IPAddress;
        public string MACAddress;
        public Int16 Port;

        public string DestinationIPAddress;
        public string DestinationMACAddress;
        public Int16 DestinationPort;

        public int status = REJECT;

        public ReliableProtocol(string IPAddress, string MACAddress, Int16 Port)
        {
            this.IPAddress = IPAddress;
            this.MACAddress = MACAddress;
            this.Port = Port;
        }

        public Bitset Send(Message message, Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Segment sgm = new Segment(message, Port, DestinationPort);
            Packet pkg = new Packet(sgm, IPAddress, DestinationIPAddress);
            Frame frm = new Frame(pkg, MACAddress, DestinationMacAddress);
            Bitset ret = new Bitset(frm);
            status = message.Statuscode;
            return ret;
        }

        public Bitset Send(Message message)
        {
            return Send(message, DestinationPort, DestinationIPAddress, DestinationMACAddress);
        }

        public Bitset Recieve(Bitset bitset)
        {
            Frame frm = new Frame(bitset);
            Packet pkg = frm.ActualPacket;
            Segment sgm = pkg.ActualSegment;
            Message msg = sgm.ActualMessage;

            if (msg.Statuscode == SYNC)
            {
                return SendSyncAck();
            }
            if (msg.Statuscode == SYNC_ACK)
            {
                if (status == REJECT)
                    return SendEstablished();
                else
                    return SendReject();
            }
            if (msg.Statuscode == ESTAB)
            {
                if (status == SYNC_ACK)
                    return SendReady();
                else
                    return SendReject();
            }
            if (msg.Statuscode == SEND)
            {
                if (status == READY)
                    return SendRecieved();
                else
                    return SendReject();
            }
            if (msg.Statuscode == REJECT)
            {
                status = REJECT;
            }
            if (msg.Statuscode == READY)
            {
                status = READY;
            }
            return null;
        }

        private Bitset SendReject()
        {
            Message msg = new Message(REJECT, "invalid state, connection rejected");

            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMACAddress);
        }

        public Bitset SendReady()
        {
            Message msg = new Message(READY, "ready for communication");
#warning modify a state for this class, and wait one stuff
            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMACAddress);

        }

        public Bitset SendRecieved()
        {
            Message msg = new Message(RECIE, "recieved a message");
#warning modify a state for this class, and wait one stuff
            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMACAddress);
        }

        public Bitset SendEstablished()
        {
            Message msg = new Message(ESTAB, "establish sync ack");
#warning modify a state for this class, and wait one stuff
            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMACAddress);
        }

        public Bitset SendSyncAck()
        {
            Message msg = new Message(SYNC_ACK, "reply sync ack");
#warning modify a state for this class, and wait one stuff
            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMACAddress);
        }

        public Bitset StartSync(Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Message msg = new Message(SYNC, "start sync");
#warning modify a state for this class, and wait one stuff
            return Send(msg, DestinationPort, DestinationIPAddress, DestinationMacAddress);
        }
    }
}
