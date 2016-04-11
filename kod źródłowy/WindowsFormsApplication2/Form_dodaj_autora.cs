using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_dodaj_autora : Form
    {
        string query;

        public Form_dodaj_autora()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                query = String.Format("INSERT INTO autor(imie,nazwisko) VALUES ('{0}','{1}');", textBox1.Text, textBox2.Text);
                Program.db.executeNonQuery(query);
                MessageBox.Show("Dodano");
            }
            catch
            {
                MessageBox.Show("Nie udało się dodać");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.oknoGlowne.load();
            this.Hide();
            Program.oknoGlowne.Show();
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
