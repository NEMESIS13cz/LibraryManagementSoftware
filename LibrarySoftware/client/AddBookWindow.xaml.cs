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
using LibrarySoftware.network.packets;
using LibrarySoftware.data;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Book book = new Book();

            book.name = nameTextBox.Text;
            book.author = authorTextBox.Text;
            book.genre = genreTextBox.Text;
            book.datePublished = dateOfPublishingTextBox.SelectedDate.Value.Ticks;
            book.pages = Convert.ToInt32(amountOfPagesTextBox.Text);
            book.ISBN = ISBNTextBox.Text;

            ClientNetworkManager.sendPacketToServer(new AddBookPacket(book));
            MessageBox.Show("Hotovo", "informace o stavu", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
