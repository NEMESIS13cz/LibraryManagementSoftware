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

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerReader.xaml
    /// </summary>
    public partial class ClientScreenManagerReader : Window
    {
        // !! udělat spojení se serverem + jak přidat vypůjčené knihy 

        ManagerReader manager;
        public ClientScreenManagerReader()
        {
            InitializeComponent();
            manager = new ManagerReader();
            DataContext = manager;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            //vrátí se zpět na menu
            ClientScreenManagerMain window = new ClientScreenManagerMain();

            window.Show();
            this.Close();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (readerListBox.SelectedItem != null)
            {
                string name = readerListBox.SelectedItem.ToString();
                if (MessageBox.Show("Přejete si vymazat " + name + "?", "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    manager.DeleteReader(readerListBox.SelectedItem as Reader);
                }
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchString = searchTextBox.Text;

            //pošle se serveru a ten pošle zpět vyběr, který se uloží do manager = manager(..pole výsledků ve verzi ObservableCollection<Reader>...)
        }

        private void backListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se předchozí packet/seznam/stránka
        }

        private void nextListButton_Click(object sender, RoutedEventArgs e)
        {
            // zobrazí se následující stránka
        }
    }
}
