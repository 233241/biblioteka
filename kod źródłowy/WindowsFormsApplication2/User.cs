using System;

namespace Biblioteka
{
    class User
    {
        public int id;
        public string login;
        public string haslo;
        public string imie;
        public string nazwisko;
        public string miasto;
        public string ulica;
        public string nr_domu;
        public string telefon;
        public string rola;

        public User()
        {
            this.rola = "gosc";
        }

        public User(int id, string login, string haslo, string imie, string nazwisko, string miasto, string ulica, string nr_domu, string telefon, string rola)
        {
            this.id = id;
            this.login = login;
            this.haslo = haslo;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.miasto = miasto;
            this.ulica = ulica;
            this.nr_domu = nr_domu;
            this.telefon = telefon;
            this.rola = rola;
        }
    }
}
