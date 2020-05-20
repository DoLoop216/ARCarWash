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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ARCarWash
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(input_username.Text == "admin" && input_password.Password == "MALOM")
            {
                AdminPanel ap = new AdminPanel(Korisnik.arDevAdministrator());
                this.Hide();
                ap.ShowDialog();
            }
            else
            {
                Korisnik k = Korisnik.Verify(input_username.Text, input_password.Password);
                
                if(k == null)
                {
                    MessageBox.Show("Pogresno korisnicko ime ili lozinka!");
                    return;
                }

                if(k.Tip == Korisnik.KorisnikType.Admin)
                {
                    AdminPanel ap = new AdminPanel(k);
                    this.Hide();
                    ap.ShowDialog();
                }
                else
                {
                    Dash d = new Dash(k);
                    this.Hide();
                    d.ShowDialog();
                }
            }
        }
    }
}
