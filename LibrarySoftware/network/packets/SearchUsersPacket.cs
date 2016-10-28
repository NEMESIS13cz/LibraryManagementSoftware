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
    class SearchUsersPacket : IPacket
    {
        public int amountOfUsers { get; set; }
        public int offsetOfUsers { get; set; }
        public string keyword { get; set; }
        // 0 = jméno
        // 1 = rodné číslo
        // 2 = e-mail
        public byte category { get; set; }

        public SearchUsersPacket(string keyword, byte searchBy, int amount, int offset)
        {
            this.keyword = keyword;
            this.category = searchBy;
            this.amountOfUsers = amount;
            this.offsetOfUsers = offset;
        }

        public int getPacketID()
        {
            return Registry.packet_searchUsers;
        }
    }
}
