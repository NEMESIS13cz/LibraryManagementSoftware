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
using LibrarySoftware.data;
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;
using LibrarySoftware.utils;
using LibrarySoftware.network;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerBook.xaml
    /// </summary>
    public partial class ClientScreenManagerBook : Window
    {
        int počet = 0; // zajišťuje, abychom nepřekročily množství knih a to z obou směrů
        string textbox = ""; // uchovává v sobě s čím porovnáváme při hledání
        byte searchType = 0; // podle čeho hledáme
        bool endOfList = false; // zda bylo ukázáno vše

        public ClientScreenManagerBook()
        {
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // smaže knihu z databáze
            try
            {
                ClientNetworkManager.sendPacketToServer(new DeleteBookPacket(booksListBox.SelectedItem as Book));
                booksListBox.Items.Remove((Book)booksListBox.SelectedItem);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře okno, kde můžeme přidat novou knihu
            AddBookWindow window = new AddBookWindow();
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            // máme možnost upravit informace o knize
            if (booksListBox.SelectedItem != null)
            {
                SharedInfo.currentlyEditingBook = booksListBox.SelectedItem as Book; // předá se jakou knihu upravujeme
                EditBookWindow window = new EditBookWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingBook = null;
            }
            else
            {
                MessageBox.Show("Nebyla zvolena žádná kniha", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // přidají se kritéria + výchozí stav a načtou se knihy do textboxu (10)
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
            searchButton_Click(searchButton, null);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // vyhledáváme knihy podle určitého klíče a textu
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
            // pošleme s dotazem serveru
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, 0));
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets(); // počkáme na odpověď
            switch (packet.getPacketID()) // jakého typu jsme obdrželi odpověď
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks: // správný typ
                    booksListBox.Items.Clear();
                    foreach (Book b in ((SearchBooksReplyPacket)packet).books) // načteme
                    {
                        booksListBox.Items.Add(b);
                    }
                    počet = 10; // počet knih, ikdyby jich bylo míň nijak to nevadí
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
            if (počet > 10) // zpět nemůžeme pokud jsme na začátku seznamu
            {
                počet -= 10;
                ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, počet - 10)); // dotaz po dalších knihách
                IPacket packet = ClientNetworkManager.pollSynchronizedPackets(); // odpověď
                switch (packet.getPacketID())
                {
                    case Registry.packet_bookData:
                        return;
                    case Registry.packet_readerData:
                        return;
                    case Registry.packet_searchReplyBooks: // správná odpověď
                        booksListBox.Items.Clear();
                        foreach (Book b in ((SearchBooksReplyPacket)packet).books) // načtení do seznamu
                        {
                            booksListBox.Items.Add(b);
                        }
                        endOfList = false;
                        if (((SearchBooksReplyPacket)packet).books.Count() < 10) // zjištění zda jsme na konci
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
            // zjistíme zda jsme na konci
            if (endOfList)
            {
                return;
            }
            // zobrazí se následující stránka
            ClientNetworkManager.sendPacketToServer(new SearchBooksPacket(textbox, searchType, 10, počet)); // pošleme svůj požadavek na další
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets(); // dostáváme odpověď
            switch (packet.getPacketID()) // jakého je typu?
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks: // správný typ
                    booksListBox.Items.Clear();
                    foreach (Book b in ((SearchBooksReplyPacket)packet).books) // načteme do seznamu
                    {
                        booksListBox.Items.Add(b);
                    }
                    if (((SearchBooksReplyPacket)packet).books.Count() < 10) // je to konec?
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
            // co se má udělat po zavření tohoto okna? - otevřít předchozí
            ClientScreenManagerMain window = new ClientScreenManagerMain();
            window.Show();
        }
    }
}
