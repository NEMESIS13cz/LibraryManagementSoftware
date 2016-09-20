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

namespace LibrarySoftware.client
{
    /// <summary>
    /// Interaction logic for ClientScreenManagerMain.xaml
    /// </summary>
    public partial class ClientScreenManagerMain : Window
    {
        public ClientScreenManagerMain()
        {
            InitializeComponent();
        }

        private void ReadersButton_Click(object sender, RoutedEventArgs e)
        {
            //zavření tohoto okna a otevření okna, kde bude obsluha moc vidět seznam čtenářů/uživatelů, hledat, mazat a 
            //tlačítko na odkaz nového okna, kde vyplní údaje pro přidání nového čtenáře
        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {
            //v podstatě totéž jako horní, akorát na knihy
        }
    }
}
