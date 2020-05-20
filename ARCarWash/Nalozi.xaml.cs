using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for Nalozi.xaml
    /// </summary>
    public partial class Nalozi : Window
    {
        public Nalozi()
        {
            InitializeComponent();
            UcitajNaloge();
        }

        private void UcitajNaloge()
        {
            if (!File.Exists("Nalozi.txt"))
                return;

            string[] lines = File.ReadAllLines("Nalozi.txt");

            if (lines == null || lines.Length < 1)
                return;

            DataTable table = new DataTable();
            DataColumn col1 = new DataColumn("ID");
            DataColumn col2 = new DataColumn("KorisnickoIme", typeof(string));

            table.Columns.Add(col1);
            table.Columns.Add(col2);



            for (int i = 0; i < lines.Length; i++)
            {
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(lines[i]);

                DataRow row = table.NewRow();
                row["ID"] = k.ID;
                row["KorisnickoIme"] = k.UserName;
                table.Rows.Add(row);
            }



            dataGridView.ItemsSource = table.AsDataView();
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

        private void dataGridView_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void potvrdi_Click(object sender, RoutedEventArgs e)
        {
            string userName = input_korisnickoIme.Text;
            string pw = input_sifra.Text;

            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Morate uneti korisnicko ime!");
                return;
            }

            if (string.IsNullOrWhiteSpace(pw))
            {
                MessageBox.Show("Morate uneti sifru korisnika!");
                return;
            }

            if(Korisnik.Exist(userName))
            {
                MessageBox.Show("Korisnik sa datim imenom vec postoji!");
                return;
            }

            Korisnik k = new Korisnik();
            k.ID = Korisnik.GetMaxID() + 1;
            k.UserName = userName;
            k.Password = pw;

            File.AppendAllText("Nalozi.txt", JsonConvert.SerializeObject(k) + Environment.NewLine);

            UcitajNaloge();
        }

        private void stampaj_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
