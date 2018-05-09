using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTP.Protocol.DataSet;

namespace RTPTest
{
    [TestClass]
    public class SegmentTest
    {
        [TestMethod]
        public void SegmentRaw()
        {
            Segment smg = new Segment("80;433;10;666;message");
            Assert.AreEqual(80, smg.SourcePort);
            Assert.AreEqual(433, smg.DestinationPort);
            Assert.AreEqual("message", smg.ActualMessage.Data);
        }

        [TestMethod]
        public void SegmentToString()
        {
            Segment smg = new Segment("80;433;10;666;message");
            Assert.AreEqual("80;433;10;666;message", smg.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void SegmentPortOutOfRange()
        {
            Segment smg = new Segment("80;99999;10;666;message");
            
        }
    }
}
