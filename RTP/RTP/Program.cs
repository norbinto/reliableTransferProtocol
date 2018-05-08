using RTP.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP
{
    class Program
    {
        static void Main(string[] args)
        {
            ReliableProtocol pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);

            ReliableProtocol pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);

            pc1.Send("",2,"","");
        }
    }
}
