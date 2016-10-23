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
using System.Threading;
using LibrarySoftware.allAboutBook;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenGuestBook.xaml
    /// </summary>
    public partial class ClientScreenGuestBook : Window
    {
        private BookManager manager;
        public ClientScreenGuestBook()
        {
            InitializeComponent();
            manager = new BookManager();
            DataContext = manager;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // načtou se data ...
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchString = searchTextBox.Text;

            //Pošle se serveru a on pošle zpět vyhovující ve formátu ObservableCollection<Book>
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka
        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se následující stránka
        }
    }
}
