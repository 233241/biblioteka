using Npgsql;
using System;
using System.Windows.Forms;

namespace Biblioteka
{
    public partial class Form_okno_glowne : Form
    {
        private Form_Logowanie logowanie;
        private Form_dodaj_autora dodajAutora;
        private Form_dodaj_egzemplarz dodajEgzemplarz;
        private Form_dodaj_ksiazke dodajKsiazke;
        private Form_dodaj_czytelnika dodajCzytelnika;
        private Form_dodaj_bibliotekarza dodajBibliotekarza;
        private Form_zakoncz_wypozyczenie zakonczWypozyczenie;
        private NpgsqlDataReader dr;
        private ListViewItem lvi;
        private string rola;
        private string query;

        public Form_okno_glowne()
        {
            InitializeComponent();
            tabPage1.Text = "Książki";
            tabPage2.Text = "Moje dane";
            tabPage3.Text = "Moje wypożyczenia";
            tabPage4.Text = "Wypożycz";
            tabPage5.Text = "Usuń konto";
            tabPage6.Text = "Autorzy";
            tabPage7.Text = "Egzemplarze";
            tabPage8.Text = "Książki";
            tabPage9.Text = "Czytelnicy";
            tabPage10.Text = "Wypożyczenia";
            
            textBox8.UseSystemPasswordChar = true;
        }

        private void Form_okno_glowne_Load(object sender, EventArgs e)
        {
            load();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (rola == "gosc")
            {
                logowanie =  new Form_Logowanie();
                this.Hide();
                logowanie.Show();
            }
            else
            {
                Program.currentUser = new User();
                this.load();
            }
        }

        public void load()
        {
            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage2);
            tabControl1.TabPages.Remove(tabPage3);
            tabControl1.TabPages.Remove(tabPage4);
            tabControl1.TabPages.Remove(tabPage5);
            tabControl1.TabPages.Remove(tabPage6);
            tabControl1.TabPages.Remove(tabPage7);
            tabControl1.TabPages.Remove(tabPage8);
            tabControl1.TabPages.Remove(tabPage9);
            tabControl1.TabPages.Remove(tabPage10);

            rola = Program.currentUser.rola;

            switch (rola)
            {
                case "gosc":
                tabControl1.TabPages.Add(tabPage1);
                loadTabPage1();
                    break;
                case "czytelnik":
                    tabControl1.TabPages.Add(tabPage1);
                    loadTabPage1();
                    tabControl1.TabPages.Add(tabPage2);
                    loadTabPage2();
                    tabControl1.TabPages.Add(tabPage3);
                    loadTabPage3();
                    tabControl1.TabPages.Add(tabPage4);
                    loadTabPage4();
                    break;
                case "bibliotekarz":
                    tabControl1.TabPages.Add(tabPage2);
                    loadTabPage2();
                    tabControl1.TabPages.Add(tabPage5);
                    loadTabPage5();
                    tabControl1.TabPages.Add(tabPage6);
                    loadTabPage6();
                    tabControl1.TabPages.Add(tabPage7);
                    loadTabPage7();
                    tabControl1.TabPages.Add(tabPage8);
                    loadTabPage8();
                    tabControl1.TabPages.Add(tabPage9);
                    loadTabPage9();
                    tabControl1.TabPages.Add(tabPage10);
                    loadTabPage10();
                    break;
                case "administrator":
                    tabControl1.TabPages.Add(tabPage2);
                    loadTabPage2();
                    tabControl1.TabPages.Add(tabPage9);
                    loadTabPage9();
                    break;
            }

            if (rola == "gosc")
            {
                label1.Text = "Gość";
                button1.Text = "Zaloguj";
            }
            else
            {
                label1.Text = Program.currentUser.login;
                button1.Text = "Wyloguj";
            }
        }

        private void loadTabPage1()
        {
            string query = "select tytul, imie||' '||nazwisko, (select count(id_egz) from egzemplarz join ksiazka using(id_ks) where tytul=k.tytul) from ksiazka k join autor using(id_aut) order by tytul;";

            listView1.Items.Clear();
            ListViewItem lvi;
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                listView1.Items.Add(lvi);
            }
        }

        private void loadTabPage2()
        {
            textBox1.Text = Program.currentUser.imie;
            textBox2.Text = Program.currentUser.nazwisko;
            textBox3.Text = Program.currentUser.miasto;
            textBox4.Text = Program.currentUser.ulica;
            textBox5.Text = Program.currentUser.nr_domu;
            textBox6.Text = Program.currentUser.telefon;
            textBox8.Text = "";
        }

        private void loadTabPage3()
        {
            if (checkBox1.Checked)
                query = "select tytul, autor.imie||' '||autor.nazwisko, nr_egz, data_wyp, data_zwr from autor join ksiazka using(id_aut) join egzemplarz using(id_ks) join wypozyczenie using(id_egz) join uzytkownik using(id_uz) where id_uz=(select id_uz from uzytkownik where login='" + Program.currentUser.login + "') order by data_wyp;";
            else
                query = "select tytul, autor.imie||' '||autor.nazwisko, nr_egz, data_wyp, data_zwr from autor join ksiazka using(id_aut) join egzemplarz using(id_ks) join wypozyczenie using(id_egz) join uzytkownik using(id_uz) where id_uz=(select id_uz from uzytkownik where login='" + Program.currentUser.login + "') and data_zwr is null order by data_wyp;";

            listView2.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                lvi.SubItems.Add(dr[3].ToString().Substring(0,10));
                lvi.SubItems.Add(dr[4].ToString().Length == 0 ? "" : dr[4].ToString().Substring(0, 10));
                listView2.Items.Add(lvi);
            }
        }

        private void loadTabPage4()
        {
            query = "select distinct tytul from egzemplarz join ksiazka using(id_ks) where id_egz not in (select id_egz from egzemplarz join wypozyczenie using(id_egz) where data_zwr is null) order by tytul";

            comboBox1.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
                comboBox1.Items.Add(dr[0].ToString());
        }
        
        private void loadTabPage5()
        {
            query = "select distinct login from uzytkownik where rola='czytelnik';";

            comboBox2.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
                comboBox2.Items.Add(dr[0].ToString());            
        }

        private void loadTabPage6()
        {
            query = "select imie, nazwisko from autor";

            listView3.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                listView3.Items.Add(lvi);
            }
        }

        private void loadTabPage7()
        {
            string query = "select tytul, nr_egz, wydanie, rok_wydania from ksiazka join egzemplarz using(id_ks)";

            listView4.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                lvi.SubItems.Add(dr[3].ToString());
                listView4.Items.Add(lvi);
            }
        }
        
        private void loadTabPage8()
        {
            query = "select imie||' '||nazwisko, tytul from autor join ksiazka using(id_aut)";

            listView5.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                listView5.Items.Add(lvi);
            }
        }

        private void loadTabPage9()
        {
            if (rola == "administrator")
            {
                query = "select login, imie, nazwisko, miasto, ulica, nr_domu, telefon from uzytkownik";
                tabPage9.Text = "Użytkownicy";
                button11.Show();
            }
            else
            {
                query = "select login, imie, nazwisko, miasto, ulica, nr_domu, telefon from uzytkownik where rola='czytelnik'";
                tabPage9.Text = "Czytelnicy";
                button11.Hide();
            }

            listView6.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                lvi.SubItems.Add(dr[3].ToString());
                lvi.SubItems.Add(dr[4].ToString());
                lvi.SubItems.Add(dr[5].ToString());
                lvi.SubItems.Add(dr[6].ToString());
                listView6.Items.Add(lvi);
            }
        }

        private void loadTabPage10()
        {
            query = "select login, nr_egz, tytul, data_wyp from uzytkownik join wypozyczenie using(id_uz) join egzemplarz using(id_egz) join ksiazka using(id_ks) where data_zwr is null order by login, data_wyp";

            listView7.Items.Clear();
            dr = Program.db.execute(query);
            while (dr.Read())
            {
                lvi = new ListViewItem(dr[0].ToString());
                lvi.SubItems.Add(dr[1].ToString());
                lvi.SubItems.Add(dr[2].ToString());
                lvi.SubItems.Add(dr[3].ToString().Substring(0,10));
                listView7.Items.Add(lvi);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                query = String.Format("UPDATE uzytkownik SET imie='{0}', nazwisko='{1}', miasto='{2}', ulica='{3}', nr_domu='{4}',telefon='{5}', haslo='{6}' WHERE login='{7}';", textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, Program.md5(textBox8.Text), Program.currentUser.login);
                Program.db.executeNonQuery(query);
                MessageBox.Show("Zmieniono dane");
            }
            catch
            {
                MessageBox.Show("Nie udało się zmienić danych");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            loadTabPage3();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                query = String.Format("INSERT INTO wypozyczenie(id_uz,id_egz) VALUES ((select id_uz from uzytkownik where login='{0}'),(select id_egz from egzemplarz join ksiazka using(id_ks) where tytul='{1}' and id_egz not in (select id_egz from egzemplarz join ksiazka using(id_ks) join wypozyczenie using(id_egz) where data_zwr is null) limit 1));", Program.currentUser.login, comboBox1.GetItemText(comboBox1.SelectedItem));
                Program.db.executeNonQuery(query);
                MessageBox.Show("Wypożyczono");
                load();
            }
            catch
            {
                MessageBox.Show("Nie udało się wypożyczyć książki");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            query = String.Format("DELETE FROM uzytkownik where login='{0}';", Program.currentUser.login);
            Program.db.executeNonQuery(query);
            
            Program.currentUser = new User();
            load();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                query = String.Format("DELETE FROM uzytkownik where login='{0}'", comboBox2.GetItemText(comboBox2.SelectedItem));
                Program.db.executeNonQuery(query);
                MessageBox.Show("Usunięto użytkownika");
                load();
            }
            catch
            {
                MessageBox.Show("nie udało się usunąć użytkownika");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dodajAutora = new Form_dodaj_autora();
            this.Hide();
            dodajAutora.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dodajEgzemplarz = new Form_dodaj_egzemplarz();
            this.Hide();
            dodajEgzemplarz.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dodajKsiazke = new Form_dodaj_ksiazke();
            this.Hide();
            dodajKsiazke.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dodajCzytelnika = new Form_dodaj_czytelnika();
            this.Hide();
            dodajCzytelnika.Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            zakonczWypozyczenie = new Form_zakoncz_wypozyczenie();
            this.Hide();
            zakonczWypozyczenie.Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dodajBibliotekarza = new Form_dodaj_bibliotekarza();
            this.Hide();
            dodajBibliotekarza.Show();
        }
    }
}
