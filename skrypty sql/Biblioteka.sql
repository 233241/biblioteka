drop domain rola_dziedzina CASCADE;

drop table Uzytkownik CASCADE;
drop table autor CASCADE;
drop table Ksiazka CASCADE;
drop table Egzemplarz CASCADE;
drop table Wypozyczenie CASCADE;

create domain rola_dziedzina as varchar
	check(VALUE IN ('gosc', 'czytelnik', 'bibliotekarz', 'administrator'));

create table Uzytkownik(
id_uz serial primary key,
login varchar NOT NULL UNIQUE,
haslo varchar NOT NULL,
imie varchar NOT NULL,
nazwisko varchar NOT NULL,
miasto varchar NOT NULL,
ulica varchar NOT NULL,
nr_domu varchar NOT NULL,
telefon varchar NOT NULL,
rola rola_dziedzina DEFAULT 'Czytelnik' NOT NULL
);

create table autor(
id_aut serial primary key,
imie varchar NOT NULL,
nazwisko varchar NOT NULL
);

create table Ksiazka(
id_ks serial primary key,
id_aut int NOT NULL references autor,
tytul varchar NOT NULL
);

create table Egzemplarz(
id_egz serial primary key,
id_ks int NOT NULL references Ksiazka,
nr_egz int NOT NULL unique,
wydanie int,
rok_wydania int
);

create table Wypozyczenie(
id_wyp serial primary key,
id_uz int NOT NULL references Uzytkownik ON DELETE CASCADE,
id_egz int NOT NULL references Egzemplarz,
data_wyp timestamp with time zone NOT NULL default now(),
data_zwr timestamp with time zone default NULL
);



REVOKE ALL ON Uzytkownik FROM PUBLIC, Gosc, Czytelnik, Bibliotekarz, Administrator;
REVOKE ALL ON autor FROM PUBLIC, Gosc, Czytelnik, Bibliotekarz, Administrator;
REVOKE ALL ON Ksiazka FROM PUBLIC, Gosc, Czytelnik, Bibliotekarz, Administrator;
REVOKE ALL ON Egzemplarz FROM PUBLIC, Gosc, Czytelnik, Bibliotekarz, Administrator;
REVOKE ALL ON Wypozyczenie FROM PUBLIC, Gosc, Czytelnik, Bibliotekarz, Administrator;

DROP ROLE IF EXISTS GOSC;
CREATE ROLE gosc LOGIN ENCRYPTED PASSWORD 'gosc';
GRANT SELECT ON Egzemplarz, Ksiazka, autor, uzytkownik TO gosc;

DROP ROLE IF EXISTS CZYTELNIK;
CREATE ROLE CZYTELNIK LOGIN ENCRYPTED PASSWORD 'czytelnik';
GRANT SELECT ON Egzemplarz, Ksiazka, autor, uzytkownik, wypozyczenie TO czytelnik;
GRANT INSERT ON wypozyczenie TO czytelnik;
GRANT UPDATE ON wypozyczenie_id_wyp_seq, uzytkownik TO czytelnik;

DROP ROLE IF EXISTS BIBLIOTEKARZ;
CREATE ROLE bibliotekarz LOGIN ENCRYPTED PASSWORD 'bibliotekarz';
GRANT SELECT ON Egzemplarz, Ksiazka, autor, uzytkownik, wypozyczenie TO bibliotekarz;
GRANT INSERT ON uzytkownik, autor, egzemplarz, ksiazka TO bibliotekarz;
GRANT UPDATE ON uzytkownik, wypozyczenie, uzytkownik_id_uz_seq, autor_id_aut_seq, egzemplarz_id_egz_seq, ksiazka_id_ks_seq, wypozyczenie_id_wyp_seq TO bibliotekarz;
GRANT DELETE ON uzytkownik TO bibliotekarz;

DROP ROLE IF EXISTS ADMINISTRATOR;
CREATE ROLE ADMINISTRATOR LOGIN ENCRYPTED PASSWORD 'administrator';
GRANT ALL ON  Egzemplarz, Ksiazka, autor, uzytkownik, wypozyczenie, uzytkownik_id_uz_seq, autor_id_aut_seq, egzemplarz_id_egz_seq, ksiazka_id_ks_seq, wypozyczenie_id_wyp_seq TO administrator;

DROP FUNCTION IF EXISTS nie_wiecej_niz_5_funkcja();
CREATE OR REPLACE FUNCTION nie_wiecej_niz_5_funkcja() RETURNS TRIGGER
AS
$X$
BEGIN
	IF (select count(id_wyp) from wypozyczenie where id_uz=new.id_uz and data_zwr is null) > 5 THEN
		delete from wypozyczenie where id_uz=new.id_uz and id_egz=new.id_egz and data_zwr is null;
	END IF;
	RETURN NEW;
END
$X$
LANGUAGE PLpgSQL;

CREATE TRIGGER nie_wiecej_niz_5_wyzwalacz
after insert on wypozyczenie
FOR EACH ROW
EXECUTE PROCEDURE nie_wiecej_niz_5_funkcja();
