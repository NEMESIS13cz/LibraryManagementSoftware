using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LibrarySoftware.allAboutBook
{
    class BookManager
    {
        public ObservableCollection<Book> Books;

        public BookManager()
        {
            Books = new ObservableCollection<Book>();
        }

        //Dále přidat ještě další metody jak inicializovat tuhle třídu


        //Způsoby pracování s tím, později přidat další
        public void Odeber(Book book)
        {
            if (book == null)
                throw new ArgumentException("Není vybrána žádná položka. Prosím nejdříve označte položku, kterou chcete smazat.");

            Books.Remove(book);
        }

        public void Odeber(int index)
        {
            if (index < 0)
                throw new ArgumentException("Není vybrána žádná položka. Prosím nejdříve označte položku, kterou chcete smazat.");

            Books.RemoveAt(index);
        }

        public void Přidej(Book book)
        {
            Books.Add(book);
        }

        public int Hledej(string searchingString)
        {
            // pomocí toho najdeme, který to je a vrátí to index, na kterém se nachází a selectedIndex nastavíme na něj
        }

        public Book Hledej(string searchingString)
        {
            //tady totéž, ale jiná varianta
        }
    }
}
