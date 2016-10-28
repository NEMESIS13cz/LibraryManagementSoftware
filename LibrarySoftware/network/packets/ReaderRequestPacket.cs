using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.data;
using LibrarySoftware.utils;

namespace LibrarySoftware.network.packets
{
    [Serializable]
    class ReaderRequestPacket : IPacket
    {
        public string ID { get; set; }

        public ReaderRequestPacket(Reader reader)
        {
            this.ID = reader.ID;
        }

        public int getPacketID()
        {
            return Registry.packet_requestUser;
        }
    }
}
