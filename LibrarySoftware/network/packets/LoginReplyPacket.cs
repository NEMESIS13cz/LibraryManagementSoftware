using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;

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

        public LoginReplyPacket(byte status)
        {
            this.status = status;
        }

        public int getPacketID()
        {
            return Registry.packet_loginReply;
        }
    }
}
