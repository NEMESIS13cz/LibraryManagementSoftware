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

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ReturnBookManagerWindow.xaml
    /// </summary>
    public partial class ReturnBookManagerWindow : Window
    {
        public ReturnBookManagerWindow()
        {
            InitializeComponent();
            DataContext = SharedInfo.currentlyEditingUser;
        }

        // později ještě dodělat
        private void returnBookButton_Click(object sender, RoutedEventArgs e)
        {
            if(booksListBox.SelectedItem != null)
            {
                Book kniha = booksListBox.SelectedItem as Book;

                // pro knihu
                Book b = kniha;
                b.borrowed = false;
                b.borrowedBy = null;
                // pro uživatele
                Reader r = new Reader();
                r = SharedInfo.currentlyEditingUser;
                Book[] borrow;
                try
                {
                    borrow = new Book[r.borrowedBooks.Count() - 1];
                    for (int i = 0, j = 0; i < r.borrowedBooks.Count(); i++, j++)
                    {
                        if (r.borrowedBooks[i] != kniha)
                            borrow[j] = r.borrowedBooks[i];
                        else
                            j--;
                    }
                }
                catch
                {
                    borrow = new Book[0];
                }
                r.borrowedBooks = borrow;

                ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(kniha, b));
                ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentlyEditingUser.ID));
                
                MessageBox.Show("Hotovo", "Informace o stavu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(Book b in SharedInfo.currentlyEditingUser.borrowedBooks)
            {
                booksListBox.Items.Add(b);
            }
        }
    }
}
