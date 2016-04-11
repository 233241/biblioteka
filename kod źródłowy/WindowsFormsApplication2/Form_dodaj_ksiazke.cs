using Npgsql;
using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_dodaj_ksiazke : Form
    {
        NpgsqlDataReader dr;
        private string query;

        public Form_dodaj_ksiazke()
        {
            InitializeComponent();
        }

        private void Form_dodaj_ksiazke_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string query = "select imie||' '||nazwisko from autor";

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
                query = String.Format("INSERT INTO ksiazka(id_aut,tytul) VALUES ((select id_aut from autor where imie||' '||nazwisko='{0}' limit 1),'{1}');", comboBox1.GetItemText(comboBox1.SelectedItem), textBox1.Text);
                Program.db.executeNonQuery(query);
                MessageBox.Show("Dodano");
            }
            catch
            {
                MessageBox.Show("Nie udało się dodać");
            }
        }
    }
}
