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
using LibrarySoftware.network.client;
using LibrarySoftware.network;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenGuestBook.xaml
    /// </summary>
    public partial class ClientScreenGuestBook : Window
    {
        public ClientScreenGuestBook()
        {
            InitializeComponent();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("Autor");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string podleČehoHledat = sortComboBox.SelectedItem.ToString();
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect(); // po zavření okna se odpojí

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
