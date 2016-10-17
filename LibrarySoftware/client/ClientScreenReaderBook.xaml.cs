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
    /// Interaction logic for ClientScreenReaderBook.xaml
    /// </summary>
    public partial class ClientScreenReaderBook : Window
    {
        private BookManager manager;
        public ClientScreenReaderBook()
        {
            InitializeComponent();
            manager = new BookManager();
            DataContext = manager;
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // načtou se data ...
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // vyhledávání
            int i = manager.Hledej(searchTextBox.Text);

            if (i != -1)
                booksListBox.SelectedIndex = i;
        }
    }
}
