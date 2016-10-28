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
using LibrarySoftware.data;
using LibrarySoftware.network.packets;
using LibrarySoftware.network.client;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for EditBookWindow.xaml
    /// </summary>
    public partial class EditBookWindow : Window
    {
        public EditBookWindow()
        {
            InitializeComponent();

            nameTextBox.Text = SharedInfo.currentlyEditingBook.name;
            authorTextBox.Text = SharedInfo.currentlyEditingBook.author;
            genreTextBox.Text = SharedInfo.currentlyEditingBook.genre;
            amountOfPagesTextBox.Text = SharedInfo.currentlyEditingBook.pages + "";
            ISBNTextBox.Text = SharedInfo.currentlyEditingBook.ISBN;
            YesRadioButton.IsChecked = SharedInfo.currentlyEditingBook.borrowed;
            NoRadioButton.IsChecked = !SharedInfo.currentlyEditingBook.borrowed;
            dateOfPublishingTextBox.SelectedDate = new DateTime(SharedInfo.currentlyEditingBook.datePublished);
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
            book.borrowed = (bool)YesRadioButton.IsChecked;
            if (book.borrowed)
            {
                book.borrowedBy = SharedInfo.currentlyEditingBook.borrowedBy;
            }

            ClientNetworkManager.sendPacketToServer(new ModifyBookPacket(SharedInfo.currentlyEditingBook, book));
        }
    }
}
