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
using System.ComponentModel;
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;
using LibrarySoftware.network;
using LibrarySoftware.data;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for BorrowBookWindow.xaml
    /// </summary>
    public partial class BorrowBookWindow : Window
    {
        public BorrowBookWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "ISBN";
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

        private void borrowBook_Click(object sender, RoutedEventArgs e)
        {
            if(booksListBox.SelectedItem != null)
            {
                Book b = (Book)booksListBox.SelectedItem;
                if (b.borrowed || (b.reserved && b.reservedBy != SharedInfo.currentlyEditingUser.ID))
                {
                    MessageBox.Show("Kniha je vypůjčená nebo rezervována jiným čtenářem.", "Varování", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                // Změny pro knihu
                Book n = b;
                n.reserved = false;
                n.reservedBy = null;
                n.borrowed = true;
                n.borrowedBy = SharedInfo.currentlyEditingUser.ID;
                // Změny pro uživatele
                Reader r = SharedInfo.currentlyEditingUser;
                if (ObsahujeKnihu(r.reservedBooks, b))
                {
                    Book[] reserve = new Book[r.reservedBooks.Count() - 1];
                    for (int i = 0, j = 0; i < r.reservedBooks.Count(); i++, j++)
                    {
                        if (r.reservedBooks[i] != b)
                            reserve[j] = r.reservedBooks[i];
                        else
                            j--;
                    }
                    r.reservedBooks = reserve;
                }
                Book[] borrow = new Book[r.borrowedBooks.Count() + 1];
                Array.Copy(r.borrowedBooks, borrow, r.reservedBooks.Count());
                borrow[r.reservedBooks.Count()] = b;
                r.borrowedBooks = borrow;

                ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(b, n));
                ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentlyEditingUser.ID));

                MessageBox.Show("Úspěšně vypůjčeno", "Úspěch", MessageBoxButton.OK);
            }
        }

        private bool ObsahujeKnihu(Book[] pole, Book kniha)
        {
            foreach (Book b in pole)
            {
                if (b != null && b.ISBN.Equals(kniha.ISBN))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
