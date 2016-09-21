using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.allAboutBook;
using System.Collections.ObjectModel;

namespace LibrarySoftware.utils
{
    class Reader
    {
        // zde bude vše o čtenáři
        public string Name { get; set; }
        public string Address { get; set; }
        public string BirthNumber { get; set; }
        public DateTime DateOfBirth { get; set; } // možná později změnit na jiný typ
        public ObservableCollection<Book> BorrowedBooks { get; set; } // Odkaz na vypůjčené knihy
        
        public Reader() { }
        public Reader(string name, string address, string birthNumber, DateTime dateOfBirth, ObservableCollection<Book> borrowedBooks)
        {
            this.Name = name;
            this.Address = address;
            this.BirthNumber = birthNumber;
            this.DateOfBirth = dateOfBirth;
            this.BorrowedBooks = borrowedBooks;
        }

        public void AddNewBorrowedBook(Book book)
        {
            BorrowedBooks.Add(book);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
