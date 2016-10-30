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
using LibrarySoftware.network.client;
using LibrarySoftware.utils;
using LibrarySoftware.network;
using LibrarySoftware.network.packets;
using LibrarySoftware.data;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenReaderMain.xaml
    /// </summary>
    public partial class ClientScreenReaderMain : Window
    {
        public ClientScreenReaderMain()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            byte searchType = 0;
            if (sortComboBox.SelectedItem.Equals("Žánr"))
            {
                searchType = 1;
            }
            else if (sortComboBox.SelectedItem.Equals("Autor"))
            {
                searchType = 2;
            }
            else if (sortComboBox.SelectedItem.Equals("ISBN"))
            {
                searchType = 3;
            }
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(searchTextBox.Text, searchType, 5, 0));
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets();
            switch (packet.getPacketID())
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks:
                    booksListBox.Items.Clear();
                    foreach (Book b in ((SearchBooksReplyPacket)packet).books)
                    {
                        booksListBox.Items.Add(b);
                    }
                    return;
                case Registry.packet_searchReplyUsers:
                    return;
            }
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reserveButton_Click(object sender, RoutedEventArgs e)
        {/*
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
            }*/
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {/*
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
            }*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect();

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
