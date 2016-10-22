using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;

namespace LibrarySoftware.network.packets
{
    [Serializable]
    class LoginDataPacket : IPacket
    {
        public string username;
        public string password;

        public LoginDataPacket(string user, string pass)
        {
            this.username = user;
            this.password = pass;
        }

        public int getPacketID()
        {
            return Registry.packet_loginData;
        }
    }
}
