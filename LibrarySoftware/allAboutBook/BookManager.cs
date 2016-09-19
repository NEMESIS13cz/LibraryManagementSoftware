using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace LibrarySoftware
{
    public class BookManager
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
            Books.Remove(book);
        }

        public void Odeber(int index)
        {
            Books.RemoveAt(index);
        }
    }
}
