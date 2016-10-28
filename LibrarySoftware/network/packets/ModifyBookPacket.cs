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
    class ModifyBookPacket : IPacket
    {
        public Book book { get; set; }
        public string ISBN { get; set; }

        public ModifyBookPacket(Book oldBook, Book newBook)
        {
            this.book = newBook;
            this.ISBN = oldBook.ISBN;
        }

        public int getPacketID()
        {
            return Registry.packet_modifyBook;
        }
    }
}
