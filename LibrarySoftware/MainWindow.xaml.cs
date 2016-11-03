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
using LibrarySoftware.utils;

namespace LibrarySoftware
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool windowClosing = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ClientNetworkManager.connectToServer(new Address(Registry.serverAddress)))
                MessageBox.Show("Nepodařilo se připojit k serveru", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (passwordBox.Password.Length == 0 || usernameTextBox.Text.Length == 0)
                {
                    MessageBox.Show("Heslo nebo jméno nemůže být prázdné.", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ClientNetworkManager.sendPacketToServer(new LoginDataPacket(usernameTextBox.Text, passwordBox.Password));
                    ClientNetworkManager.pollSynchronizedPackets();
                    switch (SharedInfo.userType)
                    {
                        case 1:
                            ClientScreenReaderMain newWindowR = new ClientScreenReaderMain();
                            newWindowR.Show();
                            windowClosing = true;
                            this.Close();
                            break;
                        case 2:
                            ClientScreenManagerMain newWindowA = new ClientScreenManagerMain();
                            newWindowA.Show();
                            windowClosing = true;
                            this.Close();
                            break;
                        case 3:
                            MessageBox.Show("Špatné heslo pro uživatele '" + usernameTextBox.Text + "'.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        case 4:
                            MessageBox.Show("Uživatel '" + usernameTextBox.Text + "' neexistuje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                        default:
                            MessageBox.Show("Neznámá chyba při přihašování.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
        }

        private void guestButton_Click(object sender, RoutedEventArgs e)
        {
            // otevření okna pro hosta
            if (!ClientNetworkManager.connectToServer(new Address(Registry.serverAddress)))
                MessageBox.Show("Nepodařilo se připojit k serveru, zkuste to prosím později", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                ClientScreenGuestBook window = new ClientScreenGuestBook();
                window.Show();
                windowClosing = true;
                this.Close();
            }
        }

        private void windowClose(object sender, EventArgs e)
        {
            if (!windowClosing)
            {
                ClientNetworkManager.disconnect();
                SharedInfo.reset();
            }
            windowClosing = false;
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(LoginButton, null);
        }
    }
}
