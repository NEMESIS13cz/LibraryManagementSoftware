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
            AddReaderWindow window = new AddReaderWindow(); // možná předat nějaké parametry
            window.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            if (readerListBox.SelectedItem != null)
            {
                EditReaderWindow window = new EditReaderWindow(readerListBox.SelectedItem as Reader);
                window.ShowDialog();
            }
            else
                MessageBox.Show("Nebyl vybrán žádný čtenář!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (readerListBox.SelectedItem != null)
            {
                string name = readerListBox.SelectedItem.ToString();
                // nutno potom vyzkoušet!!
                if (MessageBox.Show("Přejete si vymazat " + name + "?", "Dotaz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    manager.DeleteReader(readerListBox.SelectedItem as Reader);
                    MessageBox.Show("Hotovo!", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("Nebyl vybrán žádný čtenář!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            sortComboBox.Items.Add("Datum narození");

            //načtou se data
        }

        private void borrowButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno
            if(readerListBox.SelectedItem != null)
            {
                BorrowBookWindow window = new BorrowBookWindow((Reader)readerListBox.SelectedItem);
                window.ShowDialog();
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            // otevře se nové okno
            if(readerListBox.SelectedItem != null)
            {
                ReturnBookManagerWindow window = new ReturnBookManagerWindow((Reader)readerListBox.SelectedItem);
                window.ShowDialog();
            }
        }

        private void deleteReserveButton_Click(object sender, RoutedEventArgs e)
        {
            if(readerListBox.SelectedItem != null)
            {
                if(MessageBox.Show("Přejete si zrušit rezervaci u "+readerListBox.SelectedItem.ToString(),"Dotaz",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    (readerListBox.SelectedItem as Reader).ReservedBooks = null;

                    //úprava se pošle do databáze
                }
            }
        }
    }
}
