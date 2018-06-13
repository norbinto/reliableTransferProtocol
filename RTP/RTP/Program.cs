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

        static void Main(string[] args)
        {
            var pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            pc1.id = 1;
            var pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);
            pc2.id = 2;
           
            var simulator = new Simulator(pc1, pc2);

            simulator.SetIteration(2).SetLosingMessageRatio(0.5).SetMessageWaitingInterval(100);

            simulator.Simulate();

        }


    }
}
