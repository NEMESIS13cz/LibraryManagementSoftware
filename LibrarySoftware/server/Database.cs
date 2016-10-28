using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using LibrarySoftware.data;

namespace LibrarySoftware.server
{
    class Database
    {
        private static SQLiteConnection db;

        public static void connect()
        {
            db = new SQLiteConnection("Data source=database.sql;Version=3;");
            db.Open();
            
            SQLiteDataReader reader = query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = @name;",
                new SQLiteParameter[] { new SQLiteParameter("@name", "books") });

            if (!reader.HasRows)
            {
                Console.WriteLine("[Database]: Vytvářím SQL table 'books'");
                query("CREATE TABLE books (author VARCHAR(1024), name VARCHAR(1024), pages SMALLINT, genre VARCHAR(1024), " +
                    "date BIGINT, isbn VARCHAR(13), borrowed TINYINT, reserved TINYINT, borrowedBy VARCHAR(64), reservedBy VARCHAR(64))",
                    new SQLiteParameter[0]);
            }

            reader = query("SELECT name FROM sqlite_master WHERE type = 'table' AND name = @name;",
                new SQLiteParameter[] { new SQLiteParameter("@name", "users") });

            if (!reader.HasRows)
            {
                Console.WriteLine("[Database]: Vytvářím SQL table 'users'");
                query("CREATE TABLE users (name VARCHAR(512), address VARCHAR(512), birthNumber VARCHAR(10), birthDate BIGINT, " + 
                    "email VARCHAR(512), borrowedBooks VARCHAR(6656), reservedBooks VARCHAR(6656), admin TINYINT, password CHAR(64), id VARCHAR(64))",
                    new SQLiteParameter[0]);
            }
            Reader admin = getUser("0");
            if (admin == null)
            {
                Console.WriteLine("[Database]: Vytvářím administrátorský účet");
                admin = new Reader();
                admin.name = "Administrátor";
                admin.address = "---";
                admin.birthNumber = "---";
                admin.birthDate = 0;
                admin.email = "admin";
                admin.borrowedBooks = new Book[0];
                admin.reservedBooks = new Book[0];
                admin.password = Config.adminPassword;
                admin.administrator = true;
                admin.ID = "0";
                addUser(admin);
            }
        }

        public static void close()
        {
            db.Close();
        }

        public static void addBook(Book book)
        {
            query("INSERT INTO books (author, name, pages, genre, date, isbn, borrowed, " + 
                "reserved, borrowedBy, reservedBy) VALUES (@author, @name, @pages, @genre, " + 
                "@date, @isbn, @borrowed, @reserved, @borrowedBy, @reservedBy);", 
                new SQLiteParameter[] {new SQLiteParameter("@name", book.name), new SQLiteParameter("@author", book.author),
                new SQLiteParameter("@pages", book.pages), new SQLiteParameter("@genre", book.genre),
                new SQLiteParameter("@date", book.datePublished), new SQLiteParameter("@isbn", book.ISBN),
                new SQLiteParameter("@borrowed", book.borrowed ? 1 : 0), new SQLiteParameter("@reserved", book.reserved ? 1 : 0),
                new SQLiteParameter("@reservedBy", book.reservedBy), new SQLiteParameter("@borrowedBy", book.borrowedBy)});
        }

        public static List<Book> getBooks(string keyword, int category)
        {
            string searchValue = "";
            switch (category)
            {
                case 1:
                    searchValue = "genre";
                    break;
                case 2:
                    searchValue = "author";
                    break;
                case 3:
                    searchValue = "isbn";
                    break;
                default:
                    searchValue = "name";
                    break;
            }
            SQLiteDataReader reader = query("SELECT * FROM books WHERE " + searchValue + " = @search ORDER BY " + searchValue + " ASC;", new SQLiteParameter[] { new SQLiteParameter("@search", keyword)});
            List<Book> books = new List<Book>();
            while (reader.Read())
            {
                Book book = new Book();
                book.name = (string)reader["name"];
                book.author = (string)reader["author"];
                book.pages = (short)reader["pages"];
                book.genre = (string)reader["genre"];
                book.datePublished = (long)reader["date"];
                book.ISBN = (string)reader["isbn"];
                book.borrowed = (byte)reader["borrowed"] > 0;
                book.reserved = (byte)reader["reserved"] > 0;
                book.borrowedBy = (string)reader["borrowedBy"];
                book.reservedBy = (string)reader["reservedBy"];
                books.Add(book);
            }
            return books;
        }

        public static Book getBook(string ISBN)
        {
            SQLiteDataReader reader = query("SELECT * FROM books WHERE isbn = @isbn;", new SQLiteParameter[] {new SQLiteParameter("@isbn", ISBN)});
            Book book = null;
            while (reader.Read())
            {
                if (book != null)
                {
                    Console.WriteLine("[Database]: Více knih ze stejným ISBN v databázi (" + ISBN + ")");
                    break;
                }
                book = new Book();
                book.name = (string)reader["name"];
                book.author = (string)reader["author"];
                book.pages = (short)reader["pages"];
                book.genre = (string)reader["genre"];
                book.datePublished = (long)reader["date"];
                book.ISBN = (string)reader["isbn"];
                book.borrowed = (byte)reader["borrowed"] > 0;
                book.reserved = (byte)reader["reserved"] > 0;
                book.borrowedBy = (string)reader["borrowedBy"];
                book.reservedBy = (string)reader["reservedBy"];
            }
            return book;
        }

        public static void updateBook(string ISBN, Book book)
        {
            query("UPDATE books SET author = @author, name = @name, pages = @pages, date = @date, " + 
                "isbn = @isbn, borrowed = @borrowed, reserved = @reserved, borrowedBy = @borrowedBy, " + 
                "reservedBy = @reservedBy WHERE isbn = @toChange;", new SQLiteParameter[] { new SQLiteParameter("@name", book.name),
                new SQLiteParameter("@author", book.author), new SQLiteParameter("@pages", book.pages),
                new SQLiteParameter("@genre", book.genre), new SQLiteParameter("@date", book.datePublished),
                new SQLiteParameter("@isbn", book.ISBN), new SQLiteParameter("@borrowed", book.borrowed ? 1 : 0),
                new SQLiteParameter("@reserved", book.reserved ? 1 : 0), new SQLiteParameter("@reservedBy", book.reservedBy),
                new SQLiteParameter("@borrowedBy", book.borrowedBy), new SQLiteParameter("@toChange", ISBN) });
        }

        public static void deleteBook(string ISBN)
        {
            query("DELETE FROM books WHERE isbn = @isbn", new SQLiteParameter[] { new SQLiteParameter("@isbn", ISBN) });
        }

        public static void addUser(Reader reader)
        {
            string reserved = "";
            string borrowed = "";
            foreach (Book b in reader.reservedBooks)
            {
                reserved += b.ISBN + ":";
            }
            if (reserved.EndsWith(":"))
            {
                reserved = reserved.Substring(0, reserved.Length - 1);
            }
            foreach (Book b in reader.borrowedBooks)
            {
                borrowed += b.ISBN + ":";
            }
            if (borrowed.EndsWith(":"))
            {
                borrowed = borrowed.Substring(0, borrowed.Length - 1);
            }
            query("INSERT INTO users (name, address, birthNumber, birthDate, email, borrowedBooks, " + 
                "reservedBooks, admin, password, id) VALUES (@name, @address, @birthNumber, @birthDate, @email, " + 
                "@borrowedBooks, @reservedBooks, @admin, @password, @id);", new SQLiteParameter[] { new SQLiteParameter("@name", reader.name),
                new SQLiteParameter("@address", reader.address), new SQLiteParameter("@birthNumber", reader.birthNumber),
                new SQLiteParameter("@birthDate", reader.birthDate), new SQLiteParameter("@email", reader.email),
                new SQLiteParameter("@borrowedBooks", borrowed), new SQLiteParameter("@reservedBooks", reserved),
                new SQLiteParameter("@admin", reader.administrator ? 1 : 0), new SQLiteParameter("@password", reader.password), new SQLiteParameter("@id", reader.ID) });
        }

        public static void updateUser(string ID, Reader reader)
        {
            string reserved = "";
            string borrowed = "";
            foreach (Book b in reader.reservedBooks)
            {
                reserved += b.ISBN + ":";
            }
            if (reserved.EndsWith(":"))
            {
                reserved = reserved.Substring(0, reserved.Length - 1);
            }
            foreach (Book b in reader.borrowedBooks)
            {
                borrowed += b.ISBN + ":";
            }
            if (borrowed.EndsWith(":"))
            {
                borrowed = borrowed.Substring(0, borrowed.Length - 1);
            }
            query("UPDATE users SET name = @name, address = @address, birthNumber = @birthNumber, " + 
                "birthDate = @birthDate, email = @email, borrowedBooks = @borrowedBooks, " +
                "reservedBooks = @reservedBooks, admin = @admin, password = @password, id = @id WHERE id = @toChange;",
                new SQLiteParameter[] { new SQLiteParameter("@name", reader.name),
                new SQLiteParameter("@address", reader.address), new SQLiteParameter("@birthNumber", reader.birthNumber),
                new SQLiteParameter("@birthDate", reader.birthDate), new SQLiteParameter("@email", reader.email),
                new SQLiteParameter("@borrowedBooks", borrowed), new SQLiteParameter("@reservedBooks", reserved),
                new SQLiteParameter("@admin", reader.administrator ? 1 : 0), new SQLiteParameter("@id", reader.ID),
                new SQLiteParameter("@password", reader.password), new SQLiteParameter("@toChange", ID)});
        }

        public static Reader getUser(string ID)
        {
            SQLiteDataReader reader = query("SELECT * FROM users WHERE id = @id;", new SQLiteParameter[] { new SQLiteParameter("@id", ID) });
            Reader r = null;
            while (reader.Read())
            {
                if (r != null)
                {
                    Console.WriteLine("[Database]: Více uživatelů ze stejným ID v databázi (" + ID + ")");
                    r.ID = "-";
                    break;
                }
                r = new Reader();
                List<Book> books = new List<Book>();
                r.name = (string)reader["name"];
                r.address = (string)reader["address"];
                r.birthNumber = (string)reader["birthNumber"];
                r.birthDate = (long)reader["birthDate"];
                r.email = (string)reader["email"];
                r.ID = (string)reader["id"];
                r.administrator = (byte)reader["admin"] > 0;
                r.password = (string)reader["password"];
                // TODO join the queries into one
                foreach (string s in ((string)reader["borrowedBooks"]).Split(':'))
                {
                    books.Add(getBook(s));
                }
                r.borrowedBooks = new Book[books.Count];
                for (int i = 0; i < books.Count; i++)
                {
                    r.borrowedBooks[i] = books[i];
                }
                books.Clear();
                foreach (string s in ((string)reader["reservedBooks"]).Split(':'))
                {
                    books.Add(getBook(s));
                }
                r.reservedBooks = new Book[books.Count];
                for (int i = 0; i < books.Count; i++)
                {
                    r.reservedBooks[i] = books[i];
                }
                books.Clear();
            }
            return r;
        }

        public static List<Reader> getReaders(string keyword, int category)
        {
            string searchValue = "";
            switch (category)
            {
                case 1:
                    searchValue = "birthNumber";
                    break;
                case 2:
                    searchValue = "email";
                    break;
                default:
                    searchValue = "name";
                    break;
            }
            SQLiteDataReader reader = query("SELECT * FROM users WHERE " + searchValue + " = @search ORDER BY " + searchValue + " ASC;", new SQLiteParameter[] { new SQLiteParameter("@search", keyword) });
            List<Reader> readers = new List<Reader>();
            while (reader.Read())
            {
                Reader r = new Reader();
                List<Book> books = new List<Book>();
                r.name = (string)reader["name"];
                r.address = (string)reader["address"];
                r.birthNumber = (string)reader["birthNumber"];
                r.birthDate = (long)reader["birthDate"];
                r.email = (string)reader["email"];
                r.ID = (string)reader["id"];
                r.administrator = (byte)reader["admin"] > 0;
                r.password = (string)reader["password"];
                // TODO join the queries into one
                foreach (string s in ((string)reader["borrowedBooks"]).Split(':'))
                {
                    books.Add(getBook(s));
                }
                r.borrowedBooks = new Book[books.Count];
                for (int i = 0; i < books.Count; i++)
                {
                    r.borrowedBooks[i] = books[i];
                }
                books.Clear();
                foreach (string s in ((string)reader["reservedBooks"]).Split(':'))
                {
                    books.Add(getBook(s));
                }
                r.reservedBooks = new Book[books.Count];
                for (int i = 0; i < books.Count; i++)
                {
                    r.reservedBooks[i] = books[i];
                }
                books.Clear();
                readers.Add(r);
            }
            return readers;
        }

        public static void deleteUser(string ID)
        {
            query("DELETE FROM users WHERE id = @id;", new SQLiteParameter[] { new SQLiteParameter("@id", ID) });
        }

        public static string getUserID(string email)
        {
            SQLiteDataReader reader = query("SELECT id FROM users WHERE email = @email;", new SQLiteParameter[] { new SQLiteParameter("@email", email) });
            while (reader.Read())
            {
                return (string)reader["id"];
            }
            return "-";
        }

        public static string getUserPassword(string ID)
        {
            SQLiteDataReader reader = query("SELECT password FROM users WHERE id = @id;", new SQLiteParameter[] { new SQLiteParameter("@id", ID) });
            while (reader.Read())
            {
                return (string)reader["password"];
            }
            return null;
        }

        private static SQLiteDataReader query(string command, SQLiteParameter[] parameters)
        {
            SQLiteCommand cmd = new SQLiteCommand(command, db);
            cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteReader();
        }
    }
}
