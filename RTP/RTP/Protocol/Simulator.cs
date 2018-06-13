using RTP.Protocol.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace RTP.Protocol
{
    public class Simulator
    {
        public ReliableProtocol pc1;
        public ReliableProtocol pc2;

        private static int _lastTimeSent = -1;
        public static int LastTimeSent { get => _lastTimeSent; set => _lastTimeSent = value; }

        public bool lostMessage = false;
        public Random random = new Random();
        static System.Timers.Timer aTimer = new System.Timers.Timer();

        public Simulator(ReliableProtocol pc1, ReliableProtocol pc2)
        {
            this.pc1 = pc1;
            this.pc2 = pc2;
            pc2.Destination = pc1;
            pc1.Destination = pc2;
        }

        private int _iteration = 0;
        public int Iteration { get => _iteration; set => _iteration = value; }

        public Simulator SetIteration(int iteration)
        {
            this.Iteration = iteration;
            return this;
        }

        public double _losingMessageRatio = 0.0;
        public double LosingMessageRatio { get => _losingMessageRatio; set { if (value >= 1.0) while (value / 10.0 > 1.0) { value /= 10.0; } _losingMessageRatio = value; } }


        public Simulator SetLosingMessageRatio(double LosingMessageRatio)
        {
            this.LosingMessageRatio = LosingMessageRatio;
            return this;
        }

        private int _messageWaitingInterval = 100;
        public int MessageWaitingInterval { get => _messageWaitingInterval; set => _messageWaitingInterval = value; }

        /// <summary>
        /// Message waiting interval in milisecs
        /// </summary>
        /// <param name="MessageWaitingInterval">milisec interval</param>
        /// <returns></returns>
        public Simulator SetMessageWaitingInterval(int MessageWaitingInterval)
        {

            return this;
        }

        public void Simulate()
        {
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = MessageWaitingInterval;
            aTimer.Enabled = true;

            pc1.MessageSended += CommonMessageSended;
            pc2.MessageSended += CommonMessageSended;

            //build the connection
            pc1.StartSync(pc2.Port, pc2.IPAddress, pc2.MACAddress);
            while (!pc1.CommunicationReady || !pc2.CommunicationReady)
            {
                Thread.Sleep(100);
            }
            int message = 0;
            while (true)
            {
                lostMessage = IsMessageLost();
                pc1.Send(new Message((int)Statuscode.SEND, "successful message "));
                Thread.Sleep(1000);
                if (message > Iteration) { break; }
                message++;
            }
            Console.ReadKey();
        }

        private void CommonMessageSended(object sender, MessageSendedEventArgs e)
        {
            LastTimeSent = ((ReliableProtocol)sender).id;
            //message losing ratio
            lostMessage = IsMessageLost();
            if (!lostMessage)
            {
                //one successful message, clock restart
                aTimer.Stop();
                aTimer.Start();

                //send the
                ((ReliableProtocol)sender).Destination.Recieve(e.bitset);
            }
            else
            {
                Console.WriteLine("message is lost! resend is required");
            }
        }

        private bool IsMessageLost()
        {
            return random.NextDouble()  < LosingMessageRatio;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (LastTimeSent == pc1.id)
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

        private void ResendCheck(ReliableProtocol pc)
        {
            if (pc.lastMessage != null)
            {
                Console.WriteLine("id:" + pc.id + "lastTimeSent: " + LastTimeSent + " Send again");
                pc.SendAgain();
            }

        }

    }
}
