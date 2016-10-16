using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.network.packets
{
    [Serializable]
    class ConnectionPacket : IPacket
    {
        public int getPacketID()
        {
            return 1;
        }
    }
}
