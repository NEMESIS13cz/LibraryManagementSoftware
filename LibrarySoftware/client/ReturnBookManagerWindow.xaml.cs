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
using LibrarySoftware.data;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ReturnBookManagerWindow.xaml
    /// </summary>
    public partial class ReturnBookManagerWindow : Window
    {
        public ReturnBookManagerWindow()
        {
            InitializeComponent();
            DataContext = SharedInfo.currentlyEditingUser;
        }

        // později ještě dodělat
        private void returnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if(booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;
                
                //pošle se do databáze aktualizace
                MessageBox.Show("Hotovo", "Informace o stavu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
