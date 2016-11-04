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
        int počet = 0;
        string textbox = "";
        byte searchType = 0;
        bool endOfList = false;

        public ClientScreenReaderMain()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
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
                    počet = 10;
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

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka
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

        private void reserveButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;
                if (kniha.reserved || kniha.borrowed)
                {
                    MessageBox.Show("Kniha je už rezervována nebo půjčená, zkuste to prosím později.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }

                string message = "Přejete si zarezervovat " + kniha + "?";

                if (MessageBox.Show(message, "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // změny pro knihu
                    Book b = kniha;
                    b.reserved = true;
                    b.reservedBy = SharedInfo.currentUser.ID;
                    // změny pro uživatele
                    Reader r = new Reader();
                    r = SharedInfo.currentUser;
                    Book[] reserve = new Book[r.reservedBooks.Count() + 1];
                    Array.Copy(r.reservedBooks, reserve, r.reservedBooks.Count());
                    reserve[r.reservedBooks.Count()] = b;
                    r.reservedBooks = reserve;

                    ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b));
                    ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentUser.ID));

                    booksListBox.Items.Remove(kniha);
                    booksListBox.Items.Add(b);
                    SharedInfo.currentUser = r;

                    MessageBox.Show("Kniha byla úspěšně zarezervována, vyzvědněte si ji v knihovně.",
                        "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if(booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;
                if (!SharedInfo.currentUser.reservedBooks.Contains(kniha))
                {
                    MessageBox.Show("Tato kniha není rezervována nebo není k dispozici nebo rezervace nepatří vám.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    // pro knihu
                    Book b = kniha;
                    b.reserved = false;
                    b.reservedBy = null;
                    // pro uživatele
                    Reader r = new Reader();
                    r = SharedInfo.currentUser;
                    Book[] reserve;
                    try
                    {
                        reserve = new Book[r.reservedBooks.Count() - 1];
                        for (int i = 0, j = 0; i < r.reservedBooks.Count(); i++, j++)
                        {
                            if (r.reservedBooks[i] != kniha)
                                reserve[j] = r.reservedBooks[i];
                            else
                                j--;
                        }
                        r.reservedBooks = reserve;
                    }
                    catch 
                    {
                        reserve = new Book[0];
                    }

                    ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b));
                    ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentUser.ID));

                    MessageBox.Show("Rezervace odstraněna.", "Úspěch", MessageBoxButton.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
            searchButton_Click(searchButton, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect();

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
