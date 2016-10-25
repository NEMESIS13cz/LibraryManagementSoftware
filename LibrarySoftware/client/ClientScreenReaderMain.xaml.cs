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
using LibrarySoftware.network.client;
using LibrarySoftware.utils;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenReaderMain.xaml
    /// </summary>
    public partial class ClientScreenReaderMain : Window
    {
        BookManager manager;
        Reader presentUser; // proměná uchovávající současného uživatele ve formě třídy Reader
        public ClientScreenReaderMain()
        {
            InitializeComponent();
            manager = new BookManager();
            DataContext = manager;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchString = searchTextBox.Text; // ten se následně pošle do databáze a ona pošle výsledky
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;
                if (kniha.Reserved == true)
                    return;

                string message = "Přejete si zarezervovat " + kniha + "?";

                if (MessageBox.Show(message, "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    kniha.Reserved = true;
                    presentUser.AddReserveBook(kniha);

                    DateTime dnes = DateTime.Today;
                    DateTime datumMaxZarezervování = dnes.AddDays(2);

                    //poslat změnu do databáze

                    MessageBox.Show("Kniha byla úspěšně zarezervována, vyzvědněte si ji v knihovně do " + datumMaxZarezervování.Date.ToString(),
                        "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if(booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;

                try
                {
                    presentUser.DeleteReservationOfBook(kniha);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //poslat do databáze změnu
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("Žánr");
            
            // načtou se data do manager.Přidej();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect();

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
