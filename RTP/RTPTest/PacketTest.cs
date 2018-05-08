using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTP.Protocol.DataSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTPTest
{
    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void PacketRaw()
        {
            Packet smg = new Packet("192.168.1.1;192.168.1.2;80;99;10;message");
            Assert.AreEqual("192.168.1.1", smg.SourceIPAddress);
            Assert.AreEqual("192.168.1.2", smg.DestinationIPAddress);
            Assert.AreEqual("80;99;10;message", smg.ActualSegment.ToString());
        }

        [TestMethod]
        public void PacketToString()
        {
            Packet smg = new Packet("192.168.1.1;192.168.1.2;80;99;10;message");
            Assert.AreEqual("192.168.1.1;192.168.1.2;80;99;10;message", smg.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void PacketIPNotCorrect()
        {
            Packet smg = new Packet("192.168.1.1;192.168.1.2;80;99;message");

        }
    }
}
