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
    class ModifyUserPacket : IPacket
    {
        public Reader reader { get; set; }
        public string ID { get; set; }

        public ModifyUserPacket(Reader reader, string ID)
        {
            this.reader = reader;
            this.ID = ID;
        }

        public int getPacketID()
        {
            return Registry.packet_modifyUser;
        }
    }
}
