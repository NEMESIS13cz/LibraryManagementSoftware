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
    class SearchBooksReplyPacket : IPacket
    {
        public Book[] books { get; set; }

        public SearchBooksReplyPacket(Book[] books)
        {
            this.books = books;
        }

        public int getPacketID()
        {
            return Registry.packet_searchReplyBooks;
        }
    }
}
