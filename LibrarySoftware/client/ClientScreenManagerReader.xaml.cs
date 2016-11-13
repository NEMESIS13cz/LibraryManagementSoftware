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
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;
using LibrarySoftware.network;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerReader.xaml
    /// </summary>
    public partial class ClientScreenManagerReader : Window
    {
        int počet = 0; // zda jsme na konci nebo na začátku seznamu, abychom nepřekročili rozsah
        public ClientScreenManagerReader()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // pokud chceme zpět
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se okno pro přidání nového čtenáře
            AddReaderWindow window = new AddReaderWindow();
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            // upravíme stávájícího čtenáře
            if (readerListBox.SelectedItem != null) // máme nějakého zmáčknutého?
            {
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader; // zapamatujeme si, kterého jdeme upravovat
                EditReaderWindow window = new EditReaderWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingUser = null;
            }
            else
            {
                MessageBox.Show("Nebyl vybrán žádný čtenář!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // smažeme čtenáře
            if (readerListBox.SelectedItem != null) // označili jsme nějakého čtenáře, kterého chceme smazat?
            {
                string name = readerListBox.SelectedItem.ToString();
                
                // opravdu ho chceme smazat?
                if (MessageBox.Show("Přejete si vymazat " + name + "?", "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // ano
                    ClientNetworkManager.sendPacketToServer(new DeleteUserPacket(readerListBox.SelectedItem as Reader));
                    MessageBox.Show("Hotovo!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    readerListBox.Items.Remove(readerListBox.SelectedItem as Reader);
                }
            }
            else // nikoho jsme neoznačili
                MessageBox.Show("Nebyl vybrán žádný čtenář!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            // hledáme čtenáře v databázi, vyhovující našim kritériům
            byte searchType = 0; // podle jména
            if (sortComboBox.SelectedItem.Equals("Rodné číslo"))
            {
                searchType = 1;
            }
            else if (sortComboBox.SelectedItem.Equals("Email"))
            {
                searchType = 2;
            }
            ClientNetworkManager.sendPacketToServer(new SearchUsersPacket(searchTextBox.Text, searchType, 5, 0, false)); // dotaz pro server
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets(); // odpověď
            switch (packet.getPacketID()) // jakého typu je odpověď
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks:
                    return;
                case Registry.packet_searchReplyUsers: // správný typ
                    readerListBox.Items.Clear();
                    foreach (Reader r in ((SearchUsersReplyPacket)packet).readers) // načteme do seznamu
                    {
                        readerListBox.Items.Add(r);
                    }
                    return;
            }
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka
            if (počet >= 10)
            {
                ClientNetworkManager.sendPacketToServer(new SearchUsersPacket("", 0, 10, počet, false));
                IPacket packet = ClientNetworkManager.pollSynchronizedPackets();
                switch (packet.getPacketID())
                {
                    case Registry.packet_bookData:
                        return;
                    case Registry.packet_readerData:
                        return;
                    case Registry.packet_searchReplyBooks:
                        return;
                    case Registry.packet_searchReplyUsers:
                        readerListBox.Items.Clear();
                        foreach (Reader r in ((SearchUsersReplyPacket)packet).readers)
                        {
                            readerListBox.Items.Add(r);
                        }
                        počet -= 10;
                        return;
                }
            }
        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se následující stránka
            ClientNetworkManager.sendPacketToServer(new SearchUsersPacket("", 0, 10, počet, false));
            IPacket packet = ClientNetworkManager.pollSynchronizedPackets();
            switch (packet.getPacketID())
            {
                case Registry.packet_bookData:
                    return;
                case Registry.packet_readerData:
                    return;
                case Registry.packet_searchReplyBooks:
                    return;
                case Registry.packet_searchReplyUsers:
                    readerListBox.Items.Clear();
                    foreach (Reader r in ((SearchUsersReplyPacket)packet).readers)
                    {
                        readerListBox.Items.Add(r);
                    }
                    if (((SearchUsersReplyPacket)packet).readers.Count() == 10)
                        počet += 10;
                    return;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // co se má stát po zavření tohoto okna? - pouze zobrazit menu
            ClientScreenManagerMain window = new ClientScreenManagerMain();
            window.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // přidání našich možných kritérií hledání + výchozí a načtení knih
            sortComboBox.Items.Add("Jméno");
            sortComboBox.Items.Add("Rodné číslo");
            sortComboBox.Items.Add("Email");
            sortComboBox.SelectedItem = "Jméno";
            searchButton_Click(searchButton, null);
        }

        private void borrowButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno pro vypůjčení knihy
            if(readerListBox.SelectedItem != null) // vypůjčujeme někomu?
            {
                // Ano
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader; // komu vypůjčujeme, zapamatujeme si
                BorrowBookWindow window = new BorrowBookWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingUser = null;
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno pro vrácení knih(y)
            if(readerListBox.SelectedItem != null) // Označili jsme toho, kdo vrací knihu?
            {
                // ANO
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader; // zapamatujeme si ho
                ReturnBookManagerWindow window = new ReturnBookManagerWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingUser = null;
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            // Odstranění VŠECH rezervací, které dotyčný čtenář má (pokud jsme mu je popůjčovali, samy jsou odstraněny)
            if(readerListBox.SelectedItem != null)  // máme dotyčného označeného?
            {
                // ANO
                //Opravdu si přejeme je smazat?
                if(MessageBox.Show("Přejete si zrušit rezervaci(e) u "+readerListBox.SelectedItem.ToString(),"Dotaz",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // Ano smažou se
                    Reader r = readerListBox.SelectedItem as Reader;
                    foreach (Book kniha in r.reservedBooks) // postupné smazání všech
                    {
                        if (kniha != null)
                        {
                            Book b = kniha;
                            b.reserved = false;
                            b.reservedBy = null;
                            ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b)); // musí se smazat vztahy pro každou knihu
                        }
                    }
                    r.reservedBooks = new Book[0];
                    ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, r.ID)); // smazání vztahu i pro čtenáře
                }
            }
        }
    }
}
