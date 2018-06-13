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


        public string IPAddress;
        public string MACAddress;
        public Int16 Port;

        public ReliableProtocol Destination;

        public int status = (int)Statuscode.REJECT;

        public int id = -1;

        public Message lastMessage = new Message((int)Statuscode.REJECT, "initial status");

        public bool CommunicationReady { get; private set; } = false;

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

            if (status == (int)Statuscode.RECIE)
            {
                status = (int)Statuscode.ESTAB;
            }

            lastMessage = message;

            Console.WriteLine("SENT id:" + id + " statuscode:" + message.Statuscode + " msg data:" + message.Data + " msg id:" + message.Id);
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

            Console.WriteLine("RECIEVED id:" + id + " statuscode:" + msg.Statuscode + " msg id:" + msg.Id);
            if (msg.Statuscode == (int)Statuscode.SYNC)
            {
                return SendSyncAck();
            }
            if (msg.Statuscode == (int)Statuscode.SYNC_ACK)
            {
                if (status == (int)Statuscode.SYNC)
                    return SendEstablished();
                else
                    return SendReject();
            }
            if (msg.Statuscode == (int)Statuscode.ESTAB)
            {
                if (status == (int)Statuscode.SYNC_ACK)
                    status = (int)Statuscode.ESTAB;
                else
                    return SendReject();
            }
            if (msg.Statuscode == (int)Statuscode.SEND)
            {
                if (status == (int)Statuscode.ESTAB)
                    return SendRecieved();
                else
                    return SendReject();
            }
            if (msg.Statuscode == (int)Statuscode.REJECT)
            {
                status = (int)Statuscode.REJECT;
            }
            if (msg.Statuscode == (int)Statuscode.RECIE)
            {
                status = (int)Statuscode.ESTAB;
            }
            CommunicationReady = true;
            Console.WriteLine("------------------------------------------------------");
            return null;
        }

        public Bitset SendReject()
        {
            Message msg = new Message((int)Statuscode.REJECT, "invalid state, connection rejected");
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset SendRecieved()
        {
            Message msg = new Message((int)Statuscode.RECIE, "recieved a message");
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset SendEstablished()
        {
            Message msg = new Message((int)Statuscode.ESTAB, "establish sync ack");
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset SendSyncAck()
        {
            Message msg = new Message((int)Statuscode.SYNC_ACK, "reply sync ack");
            return Send(msg, Destination.Port, Destination.IPAddress, Destination.MACAddress);
        }

        public Bitset StartSync(Int16 DestinationPort, string DestinationIPAddress, string DestinationMacAddress)
        {
            Message msg = new Message((int)Statuscode.SYNC, "start sync");
            return Send(msg, Destination.Port, DestinationIPAddress, DestinationMacAddress);
        }

        public Bitset SendAgain()
        {
            return Send(lastMessage);
        }
    }
}
