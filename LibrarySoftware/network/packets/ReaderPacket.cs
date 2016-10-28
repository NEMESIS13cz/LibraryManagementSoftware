using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;
using LibrarySoftware.data;

namespace LibrarySoftware.network.packets
{
    [Serializable]
    class ReaderPacket : IPacket
    {
        public Reader reader { get; set; }

        public ReaderPacket(Reader reader)
        {
            this.reader = reader;
        }

        public int getPacketID()
        {
            return Registry.packet_readerData;
        }
    }
}
