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
using LibrarySoftware.network.client;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerMain.xaml
    /// </summary>
    public partial class ClientScreenManagerMain : Window
    {
        public static bool windowClosing = false;

        public ClientScreenManagerMain()
        {
            InitializeComponent();
        }

        private void ReadersButton_Click(object sender, RoutedEventArgs e)
        {
            ClientScreenManagerReader window = new ClientScreenManagerReader();

            window.Show();
            windowClosing = true;
            this.Close();
        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            ClientScreenManagerBook window = new ClientScreenManagerBook();

            window.Show();
            windowClosing = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!windowClosing)
            {
                ClientNetworkManager.disconnect();

                MainWindow window = new MainWindow();
                window.Show();
            }
            windowClosing = false;
        }

        private void librariansButton_Click(object sender, RoutedEventArgs e)
        {
            // Otevře se okno na zaregistrování nového knihovníka nebo vymazání, či nějakou změnu
            ClientScreenManagerAdmin window = new ClientScreenManagerAdmin();
            window.Show();
            windowClosing = true;
            this.Close();
        }
    }
}
