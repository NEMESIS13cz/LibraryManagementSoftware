using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.data
{
    [Serializable]
    class Book
    {
        public string author;
        public string name;
        public int pages;
        public string genre;
        public long datePublished;
        public string ISBN;
        public bool borrowed;
        public bool reserved;
        public string borrowedBy; // user ID
        public string reservedBy; // user ID
    }
}
