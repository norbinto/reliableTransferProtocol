using RTP.Protocol;
using RTP.Protocol.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RTP
{
    class Program
    {
        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine("id:" + pc1.id + " timer checked");
            if (pc1.lastMessage != null && pc1.lastlastMessageId != pc1.lastMessage.Id && pc1.status != ReliableProtocol.REJECT && pc1.status != ReliableProtocol.ESTAB)
            {
                Console.WriteLine("id:" + pc1.id + " Send again");

                if (pc1.SentAgain == true)
                    pc2.Recieve(pc1.SendReject());
                pc2.Recieve(pc1.Send(pc1.lastMessage));
                pc1.SentAgain = true;
            }
            if (pc1.lastMessage != null)
                pc1.lastlastMessageId = pc1.lastMessage.Id;
            pc1.SentAgain = false;

            //Console.WriteLine("id:" + pc2.id + " timer checked");
            if (pc2.lastMessage != null && pc2.lastlastMessageId == pc2.lastMessage.Id && pc2.status != ReliableProtocol.REJECT && pc2.status != ReliableProtocol.ESTAB)
            {

                Console.WriteLine("id:" + pc2.id + " Send again");
                if (pc2.SentAgain == true)
                    pc1.Recieve(pc2.SendReject());
                pc2.Recieve(pc2.Send(pc2.lastMessage));
                pc1.SentAgain = true;
            }
            if (pc2.lastMessage != null)
                pc2.lastlastMessageId = pc2.lastMessage.Id;
            pc2.SentAgain = false;
        }

        static ReliableProtocol pc1;
        static ReliableProtocol pc2;

        static void Main(string[] args)
        {
            //new thread for timer
            //System.Timers.Timer aTimer = new System.Timers.Timer();
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //aTimer.Interval = 1000;
            //aTimer.Enabled = true;

            pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            pc1.id = 1;
            pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);
            pc2.id = 2;
            pc2.Destination = pc1;
            pc1.Destination = pc2;
            pc1.MessageSended += CommonMessageSended;
            pc2.MessageSended += CommonMessageSended;
            var pc1tmp = pc1.StartSync(pc2.Port, pc2.IPAddress, pc2.MACAddress);

            pc1.Send(new Message(ReliableProtocol.SEND, "sikeres üzenet"));

            Console.ReadKey();
        }

        private static void CommonMessageSended(object sender, MessageSendedEventArgs e)
        {
            ((ReliableProtocol)sender).Destination.Recieve(e.bitset);

        }
    }
}
