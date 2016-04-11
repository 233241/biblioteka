using Npgsql;
using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_zakoncz_wypozyczenie : Form
    {
        private NpgsqlDataReader dr;
        private string query;

        public Form_zakoncz_wypozyczenie()
        {
            InitializeComponent();
        }

        private void Form_zakoncz_wypozyczenie_Load(object sender, EventArgs e)
        {
            load();
        }

        private void load()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            query = "SELECT login from uzytkownik where rola='czytelnik'";
            dr = Program.db.execute(query);
            while (dr.Read())
                comboBox1.Items.Add(dr[0].ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.oknoGlowne.load();
            this.Hide();
            Program.oknoGlowne.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                query = String.Format("UPDATE wypozyczenie SET data_zwr=now() where id_uz=(select id_uz from uzytkownik where login='{0}') and id_egz=(select id_egz from egzemplarz where nr_egz={1} limit 1) and data_zwr is null;", comboBox1.GetItemText(comboBox1.SelectedItem), comboBox2.GetItemText(comboBox2.SelectedItem));
                Program.db.executeNonQuery(query);
                MessageBox.Show("Zakończono");
            }
            catch
            {
                MessageBox.Show("Nie udało się zakończyć");
            }
            load();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();

            query = String.Format("select nr_egz from egzemplarz join wypozyczenie using(id_egz) join uzytkownik using(id_uz) where login='{0}' and data_zwr is null;", comboBox1.GetItemText(comboBox1.SelectedItem));
            dr = Program.db.execute(query);
            while (dr.Read())
                comboBox2.Items.Add(dr[0].ToString());
        }
    }
}
