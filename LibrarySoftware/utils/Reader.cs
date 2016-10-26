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
        public string Email { get; set; }
        public ObservableCollection<Book> ReservedBooks { get; set; }
        
        public Reader()
        {
            BorrowedBooks = new ObservableCollection<Book>();
        }
        public Reader(string name, string address, string birthNumber, DateTime dateOfBirth, ObservableCollection<Book> borrowedBooks, string password, string loginName, string email, ObservableCollection<Book> reservedBooks)
        {
            this.Name = name;
            this.Address = address;
            this.BirthNumber = birthNumber;
            this.DateOfBirth = dateOfBirth;
            this.BorrowedBooks = borrowedBooks;
            this.LoginName = loginName;
            this.LoginPassword = password;
            this.Email = email;
            this.ReservedBooks = reservedBooks;
        }

        public Reader(string name, string address, string birthNumber, DateTime dateOfBirth, ObservableCollection<Book> borrowedBooks, string password, string email, ObservableCollection<Book> reservedBooks)
        {
            this.Name = name;
            this.Address = address;
            this.BirthNumber = birthNumber;
            this.DateOfBirth = dateOfBirth;
            this.BorrowedBooks = borrowedBooks;
            this.LoginPassword = password;
            this.Email = email;
            this.ReservedBooks = reservedBooks;
        }

        public void AddNewBorrowedBook(Book book)
        {
            BorrowedBooks.Add(book);
        }

        public void AddReserveBook(Book book)
        {
            ReservedBooks.Add(book);
        }

        public void DeleteReservationOfBook(Book book)
        {
            if (!ReservedBooks.Contains(book))
                throw new ArgumentException("Tuto knihu nemáte rezervovanou.");

            ReservedBooks.Remove(book);
        }

        public void BorrowBook(Book book)
        {
            if (book == null)
                throw new Exception("Nebyla vybrána žádná kniha");
            if (book.Borrowed)
                throw new ArgumentException("Knihu si už někdo půjčil.");
            if (book.Reserved && !ReservedBooks.Contains(book))
                throw new ArgumentException("Knihu už má bohužel někdo zarezervovaný a ty to nejsi.");

            BorrowedBooks.Add(book);
            if (ReservedBooks.Contains(book))
            {
                ReservedBooks.Remove(book);
                book.Reserved = false;
            }
            book.ReaderOfBook = this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
