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

            SharedInfo.Port = Port;
            SharedInfo.ServerAddress = IPAddress;

            this.Close();
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IPAddressTextBox.Text = SharedInfo.ServerAddress;
            portTextBox.Text = SharedInfo.Port.ToString();
        }
    }
}
