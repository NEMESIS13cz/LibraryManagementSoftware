using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySoftware.utils;

namespace LibrarySoftware.allAboutBook
{
    public class Book
    {
        public string Author { get; private set; } // Později možná rozdělit podle jména a příjmení
        public string NameBook { get; private set; }
        public int AmountOfPages { get; set; }
        public string Genre { get; set; }
        public int DatePublishing { get; set; }
        public string ISBN { get; set; }
        public bool Borrowed { get; set; }
        public bool Reserved { get; set; }
        public Reader ReaderOfBook { get; set; }


        //Nezapomenout později přidat odkaz na čtenáře, který tu knihu má vypůjčenou
        
        public Book(string author, string nameOfBook, int amountOfPages, string genre, int dateOfPublishing, string ISBN, bool borrowed, bool reserved, Reader readerOfThisBook)
        {
            this.Author = author;
            this.NameBook = nameOfBook;
            this.AmountOfPages = amountOfPages;
            this.Genre = genre;
            this.DatePublishing = dateOfPublishing;
            this.ISBN = ISBN;
            this.Borrowed = borrowed;
            this.Reserved = reserved;
            this.ReaderOfBook = readerOfThisBook;
        }
        public Book()
        {

        }

        public void ReturnOfBook()
        {
            Borrowed = false;
            Reader reader = ReaderOfBook;
            reader.BorrowedBooks.Remove(this);
            ReaderOfBook = null;
        }

        // Pro vypsání třeba v listBoxu
        public override string ToString()
        {
            return NameBook;
        }
    }
}
