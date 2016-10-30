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
using LibrarySoftware.network.client;
using LibrarySoftware.network.packets;
using LibrarySoftware.data;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for EditReaderWindow.xaml
    /// </summary>
    public partial class EditReaderWindow : Window
    {
        bool changePassword = false;
        public EditReaderWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reader r = new Reader();
                r.name = nameTextBox.Text;
                r.address = addressTextBox.Text;
                r.birthDate = dateOfBirthDataPicker.SelectedDate.Value.Ticks;
                r.birthNumber = birthNumberTextBox.Text;
                r.borrowedBooks = SharedInfo.currentlyEditingUser.borrowedBooks;
                r.reservedBooks = SharedInfo.currentlyEditingUser.reservedBooks;
                r.email = emailTextBox.Text;
                r.ID = SharedInfo.currentlyEditingUser.ID;
                if (changePassword)
                {
                    r.password = passwordTextBox.Text;
                    r.changedPassword = true;
                }
                else
                    r.password = SharedInfo.currentlyEditingUser.password;
                ClientNetworkManager.sendPacketToServer(new ModifyUserPacket(r, SharedInfo.currentlyEditingUser.ID));
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo k chybě, prosím zkontrolujte zadané údaje! Chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = SharedInfo.currentlyEditingUser.name;
            addressTextBox.Text = SharedInfo.currentlyEditingUser.address;
            dateOfBirthDataPicker.SelectedDate = new DateTime(SharedInfo.currentlyEditingUser.birthDate);
            birthNumberTextBox.Text = SharedInfo.currentlyEditingUser.birthNumber;
            emailTextBox.Text = SharedInfo.currentlyEditingUser.email;
            nameTextBox.Text = SharedInfo.currentlyEditingUser.name;
        }

        private void changePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Text != "")
                changePassword = true;
            else
                MessageBox.Show("Heslo nesmí být prázdné!", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
