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
using LibrarySoftware.network.packets;
using LibrarySoftware.utils;
using LibrarySoftware.data;

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
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
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
