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

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerBook.xaml
    /// </summary>
    public partial class ClientScreenManagerBook : Window
    {
        public ClientScreenManagerBook()
        {
            InitializeComponent();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClientNetworkManager.sendPacketToServer(new DeleteBookPacket(booksListBox.SelectedItem as Book));
                //TODO možná refreshnout booksList?
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
            ClientScreenManagerMain window = new ClientScreenManagerMain();

            window.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sortComboBox.Items.Add("Název");
            sortComboBox.Items.Add("Žánr");
            sortComboBox.Items.Add("Autor");
            sortComboBox.Items.Add("ISBN");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchString = searchTextBox.Text;

            //Pošle se serveru a on pošle zpět vyhovující ve formátu ObservableCollection<Book>
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka
        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se následující stránka
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientScreenManagerMain window = new ClientScreenManagerMain();
            window.Show();
            this.Close();
        }
    }
}
