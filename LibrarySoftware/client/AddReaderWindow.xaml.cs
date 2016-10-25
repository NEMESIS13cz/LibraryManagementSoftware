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
            try
            {
                Reader newReader = new Reader(nameTextBox.Text, addressTextBox.Text, birthNumberTextBox.Text, dateOfBirthDataPicker.SelectedDate.Value.Date, null, passwordTextBox.Text, emailTextBox.Text, null);

                //poslat do databáze


                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Došlo k chybě, prosím zkontrolujte zadané údaje! Chyba: " + ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
