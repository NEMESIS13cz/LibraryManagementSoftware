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
using LibrarySoftware.network.client;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerMain.xaml
    /// </summary>
    public partial class ClientScreenManagerMain : Window
    {
        public ClientScreenManagerMain()
        {
            InitializeComponent();
        }

        private void ReadersButton_Click(object sender, RoutedEventArgs e)
        {
            //zavření tohoto okna a otevření okna, kde bude obsluha moc vidět seznam čtenářů/uživatelů, hledat, mazat a 
            //tlačítko na odkaz nového okna, kde vyplní údaje pro přidání nového čtenáře
            ClientScreenManagerReader window = new ClientScreenManagerReader();

            window.Show();
            this.Close();
        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            //v podstatě totéž jako horní, akorát na knihy
            ClientScreenManagerBook window = new ClientScreenManagerBook();

            window.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientNetworkManager.disconnect(); // po zavření okna se odpojí

            MainWindow window = new MainWindow();
            window.Show();
        }

        private void librariansButton_Click(object sender, RoutedEventArgs e)
        {
            // Otevře se okno na zaregistrování nového knihovníka nebo vymazání, či nějakou změnu
        }
    }
}
