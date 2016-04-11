using Npgsql;
using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_dodaj_egzemplarz : Form
    {
        private NpgsqlDataReader dr;
        private string query;

        public Form_dodaj_egzemplarz()
        {
            InitializeComponent();
        }

        private void Form_dodaj_egzemplarz_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string query = "select tytul from ksiazka";

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
                query = String.Format("INSERT INTO egzemplarz(id_ks,nr_egz,wydanie,rok_wydania) VALUES ((select id_ks from ksiazka where tytul='{0}' limit 1), {1}, {2}, {3});", comboBox1.GetItemText(comboBox1.SelectedItem), textBox3.Text, textBox1.Text, textBox2.Text);
                Program.db.executeNonQuery(query);
                MessageBox.Show("Dodano");
            }
            catch
            {
                MessageBox.Show("Nie udało się dodać ");
            }
        }
    }
}
