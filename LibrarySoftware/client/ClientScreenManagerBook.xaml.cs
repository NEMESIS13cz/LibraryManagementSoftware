﻿using System;
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
        int počet = 0;
        string textbox = "";
        byte searchType = 0;
        bool endOfList = false;

        public ClientScreenManagerBook()
        {
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
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
            AddBookWindow window = new AddBookWindow();
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (booksListBox.SelectedItem != null)
            {
                SharedInfo.currentlyEditingBook = booksListBox.SelectedItem as Book;
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
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("ISBN");
            sortComboBox.SelectedItem = "Název";
            searchButton_Click(searchButton, null);
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientScreenManagerMain window = new ClientScreenManagerMain();
            window.Show();
        }
    }
}
