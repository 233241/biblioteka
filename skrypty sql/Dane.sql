INSERT INTO autor(imie, nazwisko)
	VALUES 	('John', 'Tolkien'),
			('Boles³aw', 'Prus'),
			('George', 'Orwell');
SELECT * from autor;
			
INSERT INTO ksiazka(tytul, id_aut)
	VALUES	('Hobbit',1),
			('W³adca pierœcieni - tom 1',1),
			('Lalka',2),
			('Rok 1984',3),
			('Folwark zwierzêcy',3);
SELECT * from ksiazka;
						
INSERT INTO egzemplarz(id_ks,nr_egz, wydanie, rok_wydania)
	VALUES	(1,1,5, 2012),
			(1,2,6, 2013),
			(2,3,12,2000),
			(3,4,3,2000),
			(4,5,4,1984),
			(5,6,5,1998),
			(1,7,NULL,2015),
			(3,8,NULL,2000),
			(3,9,NULL,2000),
			(3,10,NULL,2000),
			(3,11,NULL,2000),
			(3,12,NULL,2000);
SELECT * from egzemplarz;

INSERT INTO uzytkownik(login, haslo, imie, nazwisko, miasto, ulica, nr_domu, telefon, rola)
	VALUES	('czytelnik', 'cd81536c245e62f9db678b240e1b0085', 'Krzysiek', 'Kowalski', 'Lublin', 'Lipowa', '12', '123456789', 'czytelnik'),
			('bibliotekarz', 'b075ac2d999f6df83591b44b04aa08cd', 'Magda', 'Korzeniowska', 'Sochaczew', 'Elsnera', '39/2', '987654321', 'bibliotekarz'),
			('admin', '21232f297a57a5a743894a0e4a801fc3', 'Leopold', 'Robak', 'Soplicowo', 'Grunwaldzka', '2a', '111222333', 'administrator');
SELECT * from uzytkownik;

INSERT INTO wypozyczenie(id_uz, id_egz, data_wyp)
	VALUES 	(1, 2, '2014-07-16'),
			(1, 5, '2015-02-02'),
			(1, 9, default);
SELECT * from wypozyczenie;
			
UPDATE wypozyczenie SET data_zwr = CURRENT_DATE WHERE id_wyp = 2;