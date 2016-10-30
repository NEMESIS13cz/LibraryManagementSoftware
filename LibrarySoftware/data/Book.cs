using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware.data
{
    [Serializable]
    public class Book
    {
        public string author { get; set; }
        public string name { get; set; }
        public int pages { get; set; }
        public string genre { get; set; }
        public long datePublished;
        public string ISBN { get; set; }
        public bool borrowed { get; set; }
        public bool reserved { get; set; }
        public string borrowedBy; // user ID
        public string reservedBy; // user ID

        //pro binding a ukázku dat
        public override string ToString()
        {
            return name;
        }

        public DateTime datumVydani
        {
            get
            {
                DateTime vydani = new DateTime(datePublished);
                return vydani;
            }
            set { }
        }
    }
}
