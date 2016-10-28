using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.utils
{
    class Registry
    {
        public const int serverPort = 15410;
        public const string serverAddress = "localhost";
        public const string defaultAdminPass = "password1337";
        public static readonly byte[] endOfPacket = new byte[] {
            0x44, 0x6f, 0x62, 0x72, 0x79, 0x64, 0x65, 0x6e, 0x2c, 0x70, 0x6f,
            0x6b, 0x75, 0x64, 0x73, 0x69, 0x74, 0x6f, 0x74, 0x6f, 0x63, 0x74,
            0x65, 0x74, 0x65, 0x2c, 0x74, 0x61, 0x6b, 0x74, 0x75, 0x6e, 0x65,
            0x6d, 0x61, 0x74, 0x65, 0x63, 0x6f, 0x64, 0x65, 0x6c, 0x61, 0x74,
            0x2e, 0x2e, 0x2e
        };
        public const int packet_loginData = 1;
        public const int packet_loginReply = 2;
        public const int packet_bookData = 3;
        public const int packet_readerData = 4;
        public const int packet_deleteBook = 5;
        public const int packet_addBook = 6;
        public const int packet_modifyBook = 7;
        public const int packet_requestBook = 8;
        public const int packet_searchBooks = 9;
        public const int packet_searchReplyBooks = 10;
        public const int packet_addUser = 11;
        public const int packet_deleteUser = 12;
        public const int packet_modifyUser = 13;
        public const int packet_searchUsers = 14;
        public const int packet_searchReplyUsers = 15;
        public const int packet_requestUser = 16;
    }
}
