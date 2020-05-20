using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ARCarWash
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private Korisnik this_Korisnik;
        public AdminPanel(Korisnik k)
        {
            this_Korisnik = k;
            InitializeComponent();
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Dash d = new Dash(this_Korisnik);
            d.isMain = false;
            d.Show();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Nalozi n = new Nalozi();
            n.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Izvestaj i = new Izvestaj();
            i.Show();
        }
    }
}
