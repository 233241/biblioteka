using Npgsql;
using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_Logowanie : Form
    {
        private NpgsqlDataReader dr;
        private string query;

        public Form_Logowanie()
        {
            InitializeComponent();
            textBox2.UseSystemPasswordChar = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            query = "select * from uzytkownik where login='" + textBox1.Text + "' and haslo='" + Program.md5(textBox2.Text) + "';";   
            dr = Program.db.execute(query);
                        
            if (!dr.Read())
                MessageBox.Show("Niepoprawny login lub hasło");
            else
            {
                Program.currentUser = new User(Int32.Parse(dr[0].ToString()),dr[1].ToString(),dr[2].ToString(),dr[3].ToString(),dr[4].ToString(),dr[5].ToString(),dr[6].ToString(),dr[7].ToString(),dr[8].ToString(),dr[9].ToString());
                dr.Close();

                Program.disconnect();
                Program.connect();

                Program.oknoGlowne.load();
                this.Hide();
                Program.oknoGlowne.Show();
                this.Close();
            }        
        }

        private void Form_Logowanie_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.oknoGlowne.load();
            this.Hide();
            Program.oknoGlowne.Show();
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
