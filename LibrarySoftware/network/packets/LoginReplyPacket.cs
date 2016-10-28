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
    class LoginReplyPacket : IPacket
    {
        // 0 = chyba
        // 1 = úspěch - normální uživatel
        // 2 = úspěch - admin
        // 3 = špatné heslo
        // 4 = uživatel neexistuje
        public byte status;
        public Reader reader;

        public LoginReplyPacket(Reader reader, byte status)
        {
            this.status = status;
            this.reader = reader;
        }

        public int getPacketID()
        {
            return Registry.packet_loginReply;
        }
    }
}
