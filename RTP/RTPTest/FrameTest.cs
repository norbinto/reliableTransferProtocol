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
    public class FrameTest
    {
        [TestMethod]
        public void FrameRaw()
        {
            Packet pck = new Packet("192.168.1.1;192.168.1.2;80;99;10;666;message");
            Frame frm = new Frame(pck, "bb-bb-bb-bb-bb-bb", "aa-aa-aa-aa-aa-aa");

            Assert.AreEqual("bb-bb-bb-bb-bb-bb", frm.SourceMACAddress);
            Assert.AreEqual("aa-aa-aa-aa-aa-aa", frm.DestinationMACAddress);
            Assert.AreEqual("192.168.1.1;192.168.1.2;80;99;10;666;message", frm.ActualPacket.ToString());
        }

        [TestMethod]
        public void FrameToString()
        {
            Packet pck = new Packet("192.168.1.1;192.168.1.2;80;99;10;666;message");
            Frame frm = new Frame(pck, "bb-bb-bb-bb-bb-bb", "aa-aa-aa-aa-aa-aa");
            Assert.AreEqual("bb-bb-bb-bb-bb-bb;aa-aa-aa-aa-aa-aa;192.168.1.1;192.168.1.2;80;99;10;666;message", frm.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void FrameMACNotCorrect()
        {
            Packet smg = new Packet("192.168.1.1;192.168.1.2;80;99;10;666;message");
            Frame frm = new Frame(smg, "zg-aa-aa-aa-aa-aa", "zg-aa-aa-aa-aa-aa");
        }

    }
}
