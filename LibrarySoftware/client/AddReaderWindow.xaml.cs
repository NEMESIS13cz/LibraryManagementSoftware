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
using LibrarySoftware.data;
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for AddReaderWindow.xaml
    /// </summary>
    public partial class AddReaderWindow : Window
    {
        public AddReaderWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // vytvoří čtenáře, kterého pošleme serveru a ten do databáze;
            // pokud určitě části nejsou vyplněné nebo špatně vyhodí to běhovou chybu
            try
            {
                Reader r = new Reader();
                r.name = nameTextBox.Text;
                r.address = addressTextBox.Text;
                r.birthDate = dateOfBirthDataPicker.SelectedDate.Value.Ticks;
                r.birthNumber = birthNumberTextBox.Text;
                r.borrowedBooks = new Book[0];
                r.reservedBooks = new Book[0];
                r.email = emailTextBox.Text;
                r.password = passwordTextBox.Text;
                r.administrator = SharedInfo.admin;
                ClientNetworkManager.sendPacketToServer(new AddUserPacket(r));
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo k chybě, prosím zkontrolujte zadané údaje! Chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
