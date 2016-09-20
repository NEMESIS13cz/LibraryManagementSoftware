using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySoftware
{
    public class ČtenářInfo
    {
        public string Jméno { get; set; }
        public string Příjmení { get; set; }
        public string RodnéČíslo { get; set; } //Myslím, že je lepší to nechat ve stringu
        public string Adresa { get; set; }
        public string MobilníČíslo { get; set; }

        //public Kniha[] VypůjčenéKnihy { get; set; } nebo nějak podobně

        public ČtenářInfo(string jméno, string příjmení, string rodnéČíslo, string adresa, string mobilníČíslo)
        {
            Jméno = jméno;
            Příjmení = příjmení;
            RodnéČíslo = rodnéČíslo;
            Adresa = adresa;
            MobilníČíslo = mobilníČíslo;
        }

        public ČtenářInfo()
        {
            // třeba se bude hodit
        }
    }
}
