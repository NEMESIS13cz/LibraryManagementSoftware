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
    class SearchBooksPacket : IPacket
    {
        public int amountOfBooks { get; set; }
        public int offsetOfBooks { get; set; }
        public string keyword { get; set; }
        // 0 = jméno
        // 1 = žánr
        // 2 = autor
        // 3 = ISBN
        public byte category { get; set; }

        public SearchBooksPacket(string keyword, byte searchBy, int amount, int offset)
        {
            this.keyword = keyword;
            this.category = searchBy;
            this.amountOfBooks = amount;
            this.offsetOfBooks = offset;
        }

        public int getPacketID()
        {
            return Registry.packet_searchBooks;
        }
    }
}
