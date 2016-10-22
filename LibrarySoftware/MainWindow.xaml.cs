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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibrarySoftware.client;
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;

namespace LibrarySoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Zde nějak zjistit jestli to je client nebo správce



            /* Podle toho, co zjistil, že je zač (správce/čtenář)
             * otevře příslušné okno*/

            /*
             * if(Reader){ /// Okno pro čtenáře
             * ClientScreenReader ahoj = new ClientScreenReader();
             * ahoj.Show();
             * this.Close(); }
             * else{
             *  //// otevře se okno pro správce
             *  }
             */

            // stáhnutí aktualizací databáze a tudíž vytvoření dočasné nebo stálé kopie
            if (!ClientNetworkManager.connectToServer(new utils.Address("localhost")))
                MessageBox.Show("Nepodařilo se připojit k serveru", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ClientScreenManagerMain window = new ClientScreenManagerMain();
                window.Show();
                this.Close();

                // TODO bleh
                ClientNetworkManager.sendPacketToServer(new LoginDataPacket("username", "password"));
            }
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            // otevření okna pro hosta
            if (!ClientNetworkManager.connectToServer(new utils.Address("localhost")))
                MessageBox.Show("Nepodařilo se připojit k serveru, zkuste to prosím později", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ClientScreenReaderBook window = new ClientScreenReaderBook();
                window.Show();
                this.Close();
            }
        }
    }
}
