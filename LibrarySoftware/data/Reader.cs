using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.data
{
    [Serializable]
    class Reader
    {
        public string name;
        public string address;
        public string birthNumber;
        public long birthDate;
        public string email;
        public Book[] borrowedBooks;
        public Book[] reservedBooks;
        public string password;
        public bool administrator;
        public string ID;
    }
}
