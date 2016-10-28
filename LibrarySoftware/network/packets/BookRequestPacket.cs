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
    class BookRequestPacket : IPacket
    {
        public string ISBN { get; set; }

        public BookRequestPacket(Book book)
        {
            this.ISBN = book.ISBN;
        }

        public int getPacketID()
        {
            return Registry.packet_requestBook;
        }
    }
}
