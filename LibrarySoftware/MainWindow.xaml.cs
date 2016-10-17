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
            ClientNetworkManager.connectToServer(new utils.Address("localhost"));
        }
    }
}
