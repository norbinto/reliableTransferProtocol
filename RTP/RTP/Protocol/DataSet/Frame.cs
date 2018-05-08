﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol.DataSet
{
    public class Frame
    {
        private Packet _actualPacket;
        private string _sourceMACAddress;
        private string _destinationMACAddress;


        public Packet ActualPacket { get { return _actualPacket; } private set { _actualPacket = value; } }
        public string SourceMACAddress { get { return _sourceMACAddress; } private set { _sourceMACAddress = value; } }
        public string DestinationMACAddress { get { return _destinationMACAddress; } private set { _destinationMACAddress = value; } }

        public Frame(Packet ActualPacket, string SourceMACAddress, string DestinationMACAddress)
        {
            this.ActualPacket = ActualPacket;
            this.SourceMACAddress = SourceMACAddress;
            this.DestinationMACAddress = DestinationMACAddress;
        }

        public Frame(Bitset bitset)
        {
            string[] tmp = bitset.GetDecodedData().Split(';');
            SourceMACAddress = tmp[0];
            DestinationMACAddress = tmp[1];
            StringBuilder raw =new StringBuilder() ;
            for(int i = 2; i<tmp.Length;i++)
            {
                raw.Append(tmp[i]+";");
            }

            ActualPacket =new Packet(raw.Remove(raw.Length-1,1).ToString());
        }

        public override string ToString()
        {
            return SourceMACAddress + ";" + DestinationMACAddress + ";" + ActualPacket.ToString();
        }
    }
}
