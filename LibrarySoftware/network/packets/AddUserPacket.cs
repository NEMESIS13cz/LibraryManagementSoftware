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
    class AddUserPacket : IPacket
    {
        public Reader reader { get; set; }

        public AddUserPacket(Reader reader)
        {
            this.reader = reader;
        }

        public int getPacketID()
        {
            return Registry.packet_addUser;
        }
    }
}
