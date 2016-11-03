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
        int počet = 0;
        public ClientScreenManagerReader()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ClientScreenManagerMain window = new ClientScreenManagerMain();

            window.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            AddReaderWindow window = new AddReaderWindow();
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (readerListBox.SelectedItem != null)
            {
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader;
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
            if (readerListBox.SelectedItem != null)
            {
                string name = readerListBox.SelectedItem.ToString();
                
                if (MessageBox.Show("Přejete si vymazat " + name + "?", "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ClientNetworkManager.sendPacketToServer(new DeleteUserPacket(readerListBox.SelectedItem as Reader));
                    MessageBox.Show("Hotovo!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    readerListBox.Items.Remove(readerListBox.SelectedItem as Reader);
                }
            }
            else
                MessageBox.Show("Nebyl vybrán žádný čtenář!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            byte searchType = 0;
            if (sortComboBox.SelectedItem.Equals("Rodné číslo"))
            {
                searchType = 1;
            }
            else if (sortComboBox.SelectedItem.Equals("Email"))
            {
                searchType = 2;
            }
            ClientNetworkManager.sendPacketToServer(new SearchUsersPacket(searchTextBox.Text, searchType, 5, 0, false));
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
            ClientScreenManagerMain window = new ClientScreenManagerMain();
            window.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Jméno");
            sortComboBox.Items.Add("Rodné číslo");
            sortComboBox.Items.Add("Email");
            sortComboBox.SelectedItem = "Jméno";
        }

        private void borrowButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno
            if(readerListBox.SelectedItem != null)
            {
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader;
                BorrowBookWindow window = new BorrowBookWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingUser = null;
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno
            if(readerListBox.SelectedItem != null)
            {
                SharedInfo.currentlyEditingUser = readerListBox.SelectedItem as Reader;
                ReturnBookManagerWindow window = new ReturnBookManagerWindow();
                window.ShowDialog();
                SharedInfo.currentlyEditingUser = null;
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if(readerListBox.SelectedItem != null)
            {
                if(MessageBox.Show("Přejete si zrušit rezervaci(e) u "+readerListBox.SelectedItem.ToString(),"Dotaz",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Reader r = readerListBox.SelectedItem as Reader;
                    foreach (Book kniha in r.reservedBooks)
                    {
                        if (kniha != null)
                        {
                            Book b = kniha;
                            b.reserved = false;
                            b.reservedBy = null;
                            ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b)); 
                        }
                    }
                    r.reservedBooks = new Book[0];
                    ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, r.ID));
                }
            }
        }
    }
}
