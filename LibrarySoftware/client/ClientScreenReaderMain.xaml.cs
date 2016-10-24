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
using LibrarySoftware.allAboutBook;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenReaderMain.xaml
    /// </summary>
    public partial class ClientScreenReaderMain : Window
    {
        BookManager manager;
        public ClientScreenReaderMain()
        {
            InitializeComponent();
            manager = new BookManager();
            DataContext = manager;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reserveButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "Přejete si zarezervovat " + booksListBox.SelectedItem + "?";

            if(MessageBox.Show(message,"Dotaz",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Zde se nějak zarezervuje kniha - to se ještě musí domyslet !!!!!

                DateTime dnes = DateTime.Today;
                DateTime datumMaxZarezervování = dnes.AddDays(2);

                MessageBox.Show("Kniha byla úspěšně zarezervována, vyzvědněte si ji v knihovně do " + datumMaxZarezervování.Date.ToString(),
                    "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // načtou se data do manager.Přidej();
        }
    }
}
