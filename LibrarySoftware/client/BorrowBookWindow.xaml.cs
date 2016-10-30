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

        private void borrowBook_Click(object sender, RoutedEventArgs e)
        {/*
            try
            {
                reader.AddNewBorrowedBook(booksListBox.SelectedItem as Book);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }*/
        }
    }
}
