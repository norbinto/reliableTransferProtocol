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
            if (lastTimeSent == pc1.id)
            {
                if (lostMessage)
                    ResendCheck(pc1);
            }
            else
            {
                if (lostMessage)
                    ResendCheck(pc2);
            }
        }

        private static void ResendCheck(ReliableProtocol pc)
        {
            //Console.WriteLine( "pc.lastlastMessageId == pc.lastMessage.Id " + pc.lastlastMessageId +" "+ pc.lastMessage.Id);
            //if (pc.lastMessage != null && pc.status != ReliableProtocol.REJECT && pc.status != ReliableProtocol.ESTAB)
            if (pc.lastMessage != null)
            {
                Console.WriteLine("id:" + pc.id + "lastTimeSent: " + lastTimeSent + " Send again");
                pc.SendAgain();
            }

        }

        static ReliableProtocol pc1;
        static ReliableProtocol pc2;
        static int _lastTimeSent = -1;
        static int lastTimeSent { get { return _lastTimeSent; } set { _lastTimeSent = value; } }

        public static bool lostMessage = false;
        public static Random random = new Random();
        static System.Timers.Timer aTimer = new System.Timers.Timer();

        static void Main(string[] args)
        {
            //new thread for timer

            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 100;
            aTimer.Enabled = true;

            pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            pc1.id = 1;
            pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);
            pc2.id = 2;
            pc2.Destination = pc1;
            pc1.Destination = pc2;
            pc1.MessageSended += CommonMessageSended;
            pc2.MessageSended += CommonMessageSended;
            var pc1tmp = pc1.StartSync(pc2.Port, pc2.IPAddress, pc2.MACAddress);
            Thread.Sleep(5000);

            int message = 0;
            while (true)
            {
                lostMessage = random.Next() % 2 == 1;
                pc1.Send(new Message(ReliableProtocol.SEND, "successful message "));
                Thread.Sleep(1000);
                if (message > 2) { break; }
                message++;
            }
            Console.ReadKey();
        }

        private static void CommonMessageSended(object sender, MessageSendedEventArgs e)
        {
            lastTimeSent = ((ReliableProtocol)sender).id;
            //message losing ratio
            lostMessage = random.Next() % 10 < 5;
            if (!lostMessage)
            {
                aTimer.Stop();
                aTimer.Start();
                ((ReliableProtocol)sender).Destination.Recieve(e.bitset);

            }
            else
            {
                Console.WriteLine("message is lost! resend is required");
            }
        }
    }
}
