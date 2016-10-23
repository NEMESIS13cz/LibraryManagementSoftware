using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.allAboutBook;
using System.Collections.ObjectModel;

namespace LibrarySoftware.utils
{
    public class Reader
    {
        // zde bude vše o čtenáři
        public string Name { get; set; }
        public string Address { get; set; }
        public string BirthNumber { get; set; }
        public DateTime DateOfBirth { get; set; } // možná později změnit na jiný typ
        public ObservableCollection<Book> BorrowedBooks { get; set; } // Odkaz na vypůjčené knihy
        public string LoginName { get; set; }
        public string LoginPassword { get; set; }
        
        public Reader()
        {
            BorrowedBooks = new ObservableCollection<Book>();
        }
        public Reader(string name, string address, string birthNumber, DateTime dateOfBirth, ObservableCollection<Book> borrowedBooks, string password, string loginName)
        {
            this.Name = name;
            this.Address = address;
            this.BirthNumber = birthNumber;
            this.DateOfBirth = dateOfBirth;
            this.BorrowedBooks = borrowedBooks;
            this.LoginName = loginName;
            this.LoginPassword = password;
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
