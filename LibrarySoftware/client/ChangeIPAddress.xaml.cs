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
    /// Interaction logic for ChangeIPAddress.xaml
    /// </summary>
    public partial class ChangeIPAddress : Window
    {
        public ChangeIPAddress()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            string IPAddress = IPAddressTextBox.Text;
            string Port = portTextBox.Text;

            //Zde se to převede do registru a následně do config souboru

            this.Close();
        }
    }
}
