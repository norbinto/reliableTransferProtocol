using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTP.Protocol.DataSet;

namespace RTPTest
{
    [TestClass]
    public class MessageTest
    {
        [TestMethod]
        public void DataTest()
        {
            Message msg = new Message(100,"data");
            Assert.AreEqual("data", msg.Data);
        }

    }
}
