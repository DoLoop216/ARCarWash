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
using System.IO;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;

namespace ARCarWash
{
    /// <summary>
    /// Interaction logic for Dash.xaml
    /// </summary>
    public partial class Dash : Window
    {
        private Korisnik this_Korisnik;
        public bool isMain = true;
        public Dash(Korisnik korisnik)
        {
            this_Korisnik = korisnik;
            InitializeComponent();
            dataGridView.IsReadOnly = true;
            ReloadData();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(isMain)
                System.Windows.Application.Current.Shutdown();
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isMain)
                System.Windows.Application.Current.Shutdown();
            else
                this.Close();
        }

        private void StampajIzvestaj(object sender, RoutedEventArgs e)
        {
            string[] lines = File.ReadAllLines("baza.txt");
            double ukupnaVrednost = 0;
            int danasAutomobila = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                WashItem item = JsonConvert.DeserializeObject<WashItem>(lines[i]);
                if (item.Time.Date != DateTime.Today)
                    continue;

                ukupnaVrednost += item.Price;
                danasAutomobila++;
            }

            List<string> izvestajLines = new List<string>();
            izvestajLines.Add("<style>body { margin: 0; padding: 0 } .ARCarWash-document { margin: 10px; max-width: 1100px; margin: 100px auto }</style>");
            izvestajLines.Add("<div class='ARCarWash-document'>");
            izvestajLines.Add("<h1 style='text-align: center'>ARCarWash Izvestaj</h1>");
            izvestajLines.Add("<h3 style='margin-bottom: 50px'>Poslovnica: Dorcol <br />Datum: " + DateTime.Now.ToString("dd.MM.yyyy") + "</h3>");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string perac = input_perac.Text;
            double cena = Convert.ToDouble(input_cena.Text);

            if(string.IsNullOrWhiteSpace(input_perac.Text))
            {
                MessageBox.Show("Morate uneti ime peraca!");
                return;
            }

            if(cena <= 0)
            {
                MessageBox.Show("Cena ne moze biti 0!");
                return;
            }

            WashItem wi = new WashItem();
            wi.Price = cena;
            wi.Staff = perac;

            File.AppendAllText("baza.txt", JsonConvert.SerializeObject(wi) + Environment.NewLine);

            ReloadData();
            MessageBox.Show("Uspesno ste dodalo novo pranje!");
        }

        private void input_cena_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            List<char> allow = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            if (allow.Contains(e.Text[0]))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void ReloadData()
        {
            string[] lines = File.ReadAllLines("baza.txt");
            double ukupnaVrednost = 0;
            int danasAutomobila = 0;

            DataTable table = new DataTable();
            DataColumn col1 = new DataColumn("Osoblje");
            DataColumn col2 = new DataColumn("Vreme", typeof(DateTime));
            DataColumn col3 = new DataColumn("Cena", typeof(decimal));

            table.Columns.Add(col1);
            table.Columns.Add(col3);
            table.Columns.Add(col2);



            for (int i = 0; i < lines.Length; i++)
            {
                WashItem item = JsonConvert.DeserializeObject<WashItem>(lines[i]);
                if (item.Time.Date != DateTime.Today)
                    continue;

                DataRow row = table.NewRow();
                row["Osoblje"] = item.Staff;
                row["Vreme"] = item.Time;
                row["Cena"] = item.Price;
                table.Rows.Add(row);
                ukupnaVrednost += item.Price;
                danasAutomobila++;
            }



            dataGridView.ItemsSource = table.AsDataView();



            UkupnoOprano.Content = "Ukupno oprano vozila: " + danasAutomobila;
            UkupnoOpranoVrednost.Content = "Vrednost: " + ukupnaVrednost.ToString("#,##0.00");
        }

        private void dataGridView_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.PropertyName == "Cena")
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "#,##0.00 rsd";
            }
            else if(e.PropertyName == "Vreme")
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = " HH : mm ";
            }
        }

        private void input_cena_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                Button_Click_1(potvrdi, null);
        }
    }
}
