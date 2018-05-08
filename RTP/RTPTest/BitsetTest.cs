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
    public class BitsetTest
    {

        [TestMethod]
        public void BitsetLoad()
        {
            Packet pck = new Packet("192.168.1.1;192.168.1.2;80;99;10;message");
            Frame frm = new Frame(pck, "bb-bb-bb-bb-bb-bb", "aa-aa-aa-aa-aa-aa");
            Bitset bs = new Bitset(frm);

            Assert.AreEqual("01100010011000100010110101100010011000100010110101100010011000100010110101100010011000100010110101100010011000100010110101100010011000100011101101100001011000010010110101100001011000010010110101100001011000010010110101100001011000010010110101100001011000010010110101100001011000010011101100110001001110010011001000101110001100010011011000111000001011100011000100101110001100010011101100110001001110010011001000101110001100010011011000111000001011100011000100101110001100100011101100111000001100000011101100111001001110010011101100110001001100000011101101101101011001010111001101110011011000010110011101100101", bs.Bits);
            Assert.AreEqual(frm.ToString().Length, bs.Bits.Length / 8);
        }

        [TestMethod]
        public void GetDecodedTest()
        {
            Packet pck = new Packet("192.168.1.1;192.168.1.2;80;99;10;message");
            Frame frm = new Frame(pck, "bb-bb-bb-bb-bb-bb", "aa-aa-aa-aa-aa-aa");
            Bitset bs = new Bitset(frm);

            Assert.AreEqual(frm.ToString(),bs.GetDecodedData());
        }

    }
}
