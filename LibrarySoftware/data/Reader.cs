using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.data
{
    [Serializable]
    public class Reader
    {
        public string name { get; set; }
        public string address { get; set; }
        public string birthNumber { get; set; }
        public long birthDate { get; set; }
        public string email { get; set; }
        public Book[] borrowedBooks;
        public Book[] reservedBooks;
        public string password;
        public bool administrator;
        public string ID;

        // pro potřeby bindingu
        public override string ToString()
        {
            return name;
        }

        public DateTime narozeniny
        {
            get
            {
                DateTime narozky = new DateTime(birthDate);
                return narozky;
            }
            set { }
        }

        public string vypujceneKnihy
        {
            get
            {
                string vysledek = null;

                foreach(Book b in borrowedBooks)
                {
                    if (b != null)
                        vysledek += b.ToString() + ", ";
                }

                if (vysledek == null)
                    return null;

                return vysledek.Substring(0, vysledek.Length - 2);
            }
            set { }
        }

        public string rezervovaneKnihy
        {
            get
            {
                string vysledek = null;

                foreach (Book b in reservedBooks)
                {
                    if (b != null)
                        vysledek += b.ToString() + ", ";
                }

                if (vysledek == null)
                    return null;

                return vysledek.Substring(0, vysledek.Length - 2);
            }
            set { }
        }
    }
}
