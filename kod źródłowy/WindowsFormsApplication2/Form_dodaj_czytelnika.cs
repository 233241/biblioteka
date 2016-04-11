using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_dodaj_czytelnika : Form
    {
        private string query;

        public Form_dodaj_czytelnika()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form_dodaj_czytelnika_Load(object sender, EventArgs e)
        {
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
                query = String.Format("INSERT INTO uzytkownik(login,haslo,imie,nazwisko,miasto,ulica,nr_domu,telefon,rola) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','czytelnik');", textBox1.Text, Program.md5(textBox2.Text), textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text);
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
