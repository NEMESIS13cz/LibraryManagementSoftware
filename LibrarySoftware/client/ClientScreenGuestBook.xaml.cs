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
        int počet = 0; // zajišťuje, abychom nepřekročily množství knih a to z obou směrů
        string textbox = ""; // uchovává v sobě s čím porovnáváme při hledání
        byte searchType = 0; // podle čeho hledáme
        bool endOfList = false; // zda bylo ukázáno vše

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
            searchButton_Click(searchButton, null);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // bude vyhledáno podle našich kritérií
            textbox = searchTextBox.Text;
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
            else
            {
                searchType = 0;
            }
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, 0));
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets();
            switch (packet.getPacketID()) // zkontrolujeme zda byl dodán správný packet/správná data
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks: // byly správná a zobrazí se v listboxu
                    booksListBox.Items.Clear();
                    foreach (Book b in ((SearchBooksReplyPacket)packet).books)
                    {
                        booksListBox.Items.Add(b);
                    }
                    počet = 10;
                    endOfList = false;
                    if (((SearchBooksReplyPacket)packet).books.Count() < 10) // zjistí zda bylo už ukázáno všechno z výběru
                    {
                        endOfList = true;
                    }
                    return;
                case Registry.packet_searchReplyUsers:
                    return;
            }
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka a to i pokud vyhledal a výsledků bylo víc
            if (počet > 10)
            {
                počet -= 10;
                ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, počet - 10));
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
                        endOfList = false;
                        if (((SearchBooksReplyPacket)packet).books.Count() < 10)
                        {
                            endOfList = true;
                        }
                        return;
                    case Registry.packet_searchReplyUsers:
                        return;
                }
            }
        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {
            if (endOfList)
            {
                return;
            }
            // zobrazí se následující stránka
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, počet));
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
                    if (((SearchBooksReplyPacket)packet).books.Count() < 10)
                    {
                        endOfList = true;
                    }
                    počet += 10;
                    return;
                case Registry.packet_searchReplyUsers:
                    return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect(); // po zavření okna se odpojí

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
