using RTP.Protocol.DataSet;

namespace RTP.Protocol
{
    public class MessageSendedEventArgs
    {
        public Bitset bitset;


        public MessageSendedEventArgs(Bitset bitset)
        {
            this.bitset = bitset;
        }
    }
}