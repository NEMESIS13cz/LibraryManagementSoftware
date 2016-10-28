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
    class DeleteBookPacket : IPacket
    {
        public string ISBN { get; set; }

        public DeleteBookPacket(Book book)
        {
            this.ISBN = book.ISBN;
        }

        public int getPacketID()
        {
            return Registry.packet_deleteBook;
        }
    }
}
