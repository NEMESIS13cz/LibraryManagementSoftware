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
        int počet = 0; // zajišťuje, abychom nepřekročily množství knih a to z obou směrů
        string textbox = ""; // uchovává v sobě s čím porovnáváme při hledání
        byte searchType = 0; // podle čeho hledáme
        bool endOfList = false; // zda bylo ukázáno vše

        public ClientScreenReaderMain()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // hledání knih podle zadaných kritérií
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
                searchType = 0; // podle názvu
            }
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, 0)); // požadavek pošleme serveru
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets(); // počkáme na odpověď
            switch (packet.getPacketID()) // kterého typu přišla odpověď?
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks: // dostali jsme správný typ odpovědi a zpracujeme
                    booksListBox.Items.Clear();
                    foreach (Book b in ((SearchBooksReplyPacket)packet).books) // synchronizujeme se seznamem
                    {
                        booksListBox.Items.Add(b);
                    }
                    počet = 10;
                    endOfList = false;
                    if (((SearchBooksReplyPacket)packet).books.Count() < 10) // zobrazili jsme všechny?
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
            // rezervujeme si knihu pro pozdější vypůjčení
            if (booksListBox.SelectedItem != null) // označili jsme nějakou knihu?
            {
                Book kniha = booksListBox.SelectedItem as Book;
                if (kniha.reserved || kniha.borrowed) // není kniha už rezervována nebo půjčena?
                {
                    MessageBox.Show("Kniha je už rezervována nebo půjčená, zkuste to prosím později.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }

                string message = "Přejete si zarezervovat " + kniha + "?";

                // Dotaz zda si ji chci opravdu rezervovat
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

                    // musí se upravit vztahy jak pro knihu tak pro čtenáře
                    ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b));
                    ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentUser.ID));

                    booksListBox.Items.Remove(kniha); // odstraníme ji a přidáme aktualizovanou
                    booksListBox.Items.Add(b);
                    SharedInfo.currentUser = r;

                    MessageBox.Show("Kniha byla úspěšně zarezervována, vyzvědněte si ji v knihovně.",
                        "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            // Pokud jsme si rezervaci rozmysleli, můžeme ji zde odstranit
            if(booksListBox.SelectedItem != null) // je nějaká kniha označená?
            {
                Book kniha = booksListBox.SelectedItem as Book;
                if (!SharedInfo.currentUser.reservedBooks.Contains(kniha)) // Máme ji opravdu rezervovanou MY?
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
                    // zrušení rezervace
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

                    // informování databáze
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
            // přidání kritérií a nastavení výchozí + načtení knih
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
            searchButton_Click(searchButton, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // co se stane po zavření?
            ClientNetworkManager.disconnect();

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
