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
using LibrarySoftware.allAboutBook;

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerBook.xaml
    /// </summary>
    public partial class ClientScreenManagerBook : Window
    {
        BookManager manager;
        public ClientScreenManagerBook()
        {
            InitializeComponent();
            manager = new BookManager();
            DataContext = manager;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.Odeber(booksListBox.SelectedItem as Book); // nevím jestli na to listBox automaticky zareaguje, pokud ne, tak implementuju INotifyPropertyChanged do BookManager
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //Bude odkazovat na další okno, kde bude formulář na vyplnění
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            //Otevře se okno, kde se bude moc opravit jakákoli vlastnost té knihy a následně se uloží změny
        }

        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            // vrátí se zpět na hlavní okno
            ClientScreenManagerMain window = new ClientScreenManagerMain();

            window.Show();
            this.Close();
        }
    }
}
