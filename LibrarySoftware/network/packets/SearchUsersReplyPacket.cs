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
    class SearchUsersReplyPacket : IPacket
    {
        public Reader[] readers { get; set; }

        public SearchUsersReplyPacket(Reader[] readers)
        {
            this.readers = readers;
        }

        public int getPacketID()
        {
            return Registry.packet_searchReplyUsers;
        }
    }
}
