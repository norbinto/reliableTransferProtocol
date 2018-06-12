using RTP.Protocol.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RTP.Protocol
{
    public class ReliableProtocol
    {
        public static int SYNC = 100;
        public static int SYNC_ACK = 101;
        public static int ESTAB = 102;
        public static int SEND = 200;
        public static int RECIE = 300;
        public static int REJECT = 500;

        public string IPAddress;
        public string MACAddress;
        public Int16 Port;

        public ReliableProtocol Destination;

        public int status = REJECT;

        public int id = -1;

        public Message lastMessage = new Message(REJECT, "initial status");
        public int lastlastMessageId;
        public bool SentAgain = false;

        public event EventHandler<MessageSendedEventArgs> MessageSended;

        protected virtual void OnMessageSended(MessageSendedEventArgs e)
        {
            MessageSended?.Invoke(this, e);
        }

        public ReliableProtocol(string IPAddress, string MACAddress, Int16 Port)
        {
            this.IPAddress = IPAddress;
            this.MACAddress = MACAddress;
            this.Port = Port;
        }

        public Bitset Send(Message message, Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Segment sgm = new Segment(message, Port, Destination.Port);
            Packet pkg = new Packet(sgm, IPAddress, DestinationIPAddress);
            Frame frm = new Frame(pkg, MACAddress, DestinationMacAddress);
            Bitset ret = new Bitset(frm);
            status = message.Statuscode;
            OnMessageSended(new MessageSendedEventArgs(ret));
            return ret;
        }

        public Bitset Send(Message message)
        {
            return Send(message, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset Recieve(Bitset bitset)
        {
            Frame frm = new Frame(bitset);
            Packet pkg = frm.ActualPacket;
            Segment sgm = pkg.ActualSegment;
            Message msg = sgm.ActualMessage;

            Console.WriteLine("RECIEVED id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            if (msg.Statuscode == SYNC)
            {
                return SendSyncAck();
            }
            if (msg.Statuscode == SYNC_ACK)
            {
                if (status == SYNC)
                    return SendEstablished();
                else
                    return SendReject();
            }
            if (msg.Statuscode == ESTAB)
            {
                if (status == SYNC_ACK)
                    status = ESTAB;
                else
                    return SendReject();
            }
            if (msg.Statuscode == SEND)
            {
                if (status == ESTAB)
                    return SendRecieved();
                else
                    return SendReject();
            }
            if (msg.Statuscode == REJECT)
            {
                status = REJECT;
            }
            

            Console.WriteLine("return null error");
            return null;
        }

        public Bitset SendReject()
        {
            Message msg = new Message(REJECT, "invalid state, connection rejected");
            Console.WriteLine("SENT id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }
        
        public Bitset SendRecieved()
        {
            Message msg = new Message(RECIE, "recieved a message");
            Console.WriteLine("SENT id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            lastMessage = msg;
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset SendEstablished()
        {
            Message msg = new Message(ESTAB, "establish sync ack");
            Console.WriteLine("SENT id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            
            lastMessage = msg;
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset SendSyncAck()
        {
            Message msg = new Message(SYNC_ACK, "reply sync ack");
            Console.WriteLine("SENT id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            lastMessage = msg;
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset StartSync(Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Message msg = new Message(SYNC, "start sync");
            Console.WriteLine("SENT id:" + id + " statuscode:" + msg.Statuscode + " msg data:" + msg.Data);
            lastMessage = msg;

            return Send(msg, Destination.Port, DestinationIPAddress, DestinationMacAddress);
        }
    }
}
