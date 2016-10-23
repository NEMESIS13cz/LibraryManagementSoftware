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
    /// Interaction logic for EditReaderWindow.xaml
    /// </summary>
    public partial class EditReaderWindow : Window
    {
        Reader reader;
        public EditReaderWindow(Reader reader)
        {
            InitializeComponent();
            this.reader = reader;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // místo null dát pak něco jiného
                reader = new Reader(nameTextBox.Text, addressTextBox.Text, birthNumberTextBox.Text, dateOfBirthDataPicker.SelectedDate.Value.Date, null, passwordTextBox.Text, loginNameTextBox.Text);

                // odeslat změny do databáze!!



                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo k chybě, prosím zkontrolujte zadané údaje! Chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = reader.Name;
            addressTextBox.Text = reader.Address;
            dateOfBirthDataPicker.SelectedDate = reader.DateOfBirth;
            birthNumberTextBox.Text = reader.BirthNumber;
            //borrowed books
            loginNameTextBox.Text = reader.LoginName;
            passwordTextBox.Text = reader.LoginPassword;
        }
    }
}
