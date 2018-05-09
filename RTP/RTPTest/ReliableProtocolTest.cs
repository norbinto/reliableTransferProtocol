using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTP.Protocol;
using RTP.Protocol.DataSet;

namespace RTPTest
{
    [TestClass]
    public class ReliableProtocolTest
    {
        [TestMethod]
        public void Connect()
        {
            ReliableProtocol pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            ReliableProtocol pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);

            var pc1tmp = pc1.SendSyncAck();
            Bitset pc2tmp = pc1tmp;
            bool one = true;
            while (pc1tmp != null && pc2tmp != null)
            {
                if (one)
                {
                    one = false;
                    pc2tmp = pc2.Recieve(pc1tmp);
                }
                else
                {
                    one = true;
                    pc1tmp = pc1.Recieve(pc2tmp);
                }

            }
            Assert.AreEqual(ReliableProtocol.READY, pc1.status);

            Assert.AreEqual(ReliableProtocol.READY, pc2.status);
        }

        [TestMethod]
        public void Reject()
        {
            ReliableProtocol pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            ReliableProtocol pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);

            var pc1tmp = pc1.SendSyncAck();
           pc1.Recieve( pc2.Send(new Message(ReliableProtocol.REJECT,"rejtected")));
            
            Assert.AreEqual(ReliableProtocol.REJECT, pc1.status);

            Assert.AreEqual(ReliableProtocol.REJECT, pc2.status);
        }

        [TestMethod]
        public void SendWithoutConnection()
        {
            ReliableProtocol pc1 = new ReliableProtocol("10.0.0.2", "00-11-22-33-44-55", 80);
            ReliableProtocol pc2 = new ReliableProtocol("192.168.1.2", "FF-DD-EE-CC-BB-AA", 443);

            var pc1tmp = pc1.Send(new Message(ReliableProtocol.SEND,"message"));
            Bitset pc2tmp = pc1tmp;
            bool one = true;
            while (pc1tmp != null && pc2tmp != null)
            {
                if (one)
                {
                    one = false;
                    pc2tmp = pc2.Recieve(pc1tmp);
                }
                else
                {
                    one = true;
                    pc1tmp = pc1.Recieve(pc2tmp);
                }

            }

            Assert.AreEqual(ReliableProtocol.REJECT, pc1.status);

            Assert.AreEqual(ReliableProtocol.REJECT, pc2.status);
        }
    }
}
