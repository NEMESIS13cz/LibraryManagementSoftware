using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using LibrarySoftware.utils;
using LibrarySoftware.allAboutBook;
using System.ComponentModel;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for BorrowBookWindow.xaml
    /// </summary>
    public partial class BorrowBookWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged; // snad lepší způsob, než do listboxu dávat data po jednom
        Reader reader;
        List<Book> temporalyBooks = new List<Book>();
        public BorrowBookWindow(Reader reader)
        {
            InitializeComponent();
            this.reader = reader;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            temporalyBooks.AddRange(reader.ReservedBooks.ToArray());
            MakeChange("booksListBox");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void borrowBook_Click(object sender, RoutedEventArgs e)
        {
            // na tohle potom vytvořím funkci ve tříde Reader
        }

        protected void MakeChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
