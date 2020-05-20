using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for Izvestaj.xaml
    /// </summary>
    public partial class Izvestaj : Window
    {
        public Izvestaj()
        {
            InitializeComponent();

            List<string> k = new List<string>() { "Svi korisnici" };
            korisnici_cmb.ItemsSource = k;
            korisnici_cmb.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void izvuciIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines("baza.txt");
            DateTime? _od = datumOD.SelectedDate;
            DateTime? _do = datumDO.SelectedDate;

            if(_od == null || _do == null)
            {
                MessageBox.Show("Niste izabrali datume!");
                return;
            }

            if(_do < _od)
            {
                MessageBox.Show("Vremenski period je neispravan!");
                return;
            }

            double ukupnaVrednost = 0;
            int danasAutomobila = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                WashItem item = JsonConvert.DeserializeObject<WashItem>(lines[i]);
                if (item.Time.Date > _do || item.Time.Date < _od)
                    continue;

                ukupnaVrednost += item.Price;
                danasAutomobila++;
            }

            List<string> izvestajLines = new List<string>();
            izvestajLines.Add("<style>body { margin: 0; padding: 0 } .ARCarWash-document { margin: 10px; max-width: 1100px; margin: 100px auto }</style>");
            izvestajLines.Add("<div class='ARCarWash-document'>");
            izvestajLines.Add("<h1 style='text-align: center'>ARCarWash Izvestaj</h1>");
            izvestajLines.Add("<h3 style='margin-bottom: 50px'>Poslovnica: Dorcol <br />Za period: " + ((DateTime)_od).ToString("dd.MM.yyyy") + " - " + ((DateTime)_do).ToString("dd.MM.yyyy") + "</h3>");
            izvestajLines.Add("<h4>Oprano automobila: " + danasAutomobila.ToString());
            izvestajLines.Add("<h4>Vrednosno: " + ukupnaVrednost.ToString("#,##0.00"));
            izvestajLines.Add("<hr />");

            for (int i = 0; i < lines.Length; i++)
            {
                WashItem item = JsonConvert.DeserializeObject<WashItem>(lines[i]);
                if (item.Time.Date != DateTime.Today)
                    continue;

                izvestajLines.Add($"<p>[ {item.Time.ToString("HH:mm")} ] {item.Staff} - {item.Price}</p>");
            }

            izvestajLines.Add("</div>");

            File.WriteAllLines("Temp_Izvestaj.html", izvestajLines);

            Process.Start(new ProcessStartInfo(@"Temp_Izvestaj.html") { UseShellExecute = true });

        }
    }
}
