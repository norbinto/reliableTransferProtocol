using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTP.Protocol
{
    public enum Statuscode
    {
        SYNC = 100,
        SYNC_ACK = 101,
        ESTAB = 102,
        SEND = 200,
        RECIE = 300,
        REJECT = 500
    }
}
