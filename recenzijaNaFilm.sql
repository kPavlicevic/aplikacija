use master;
drop database if exists recenzijaNaFilm;
go
create database recenzijaNaFilm;
go
use recenzijaNaFilm;

create table film(
	sifra int not null primary key identity (1,1),
	naziv varchar (50) not null,
	godina int not null,
	redatelj varchar (50) not null,
	zanr varchar (50) not null
);

create table glumac(
	sifra int not null primary key identity (1,1),
	ime varchar (50) not null,
	prezime varchar (50) not null,
	drzavljanstvo varchar (50)
);

create table uloga(
	film int not null,
	glumac int not null
);

create table korisnik(
	sifra int not null primary key identity (1,1),
	korisnickoIme varchar (50) not null,
	lozinka varchar (50)
);

create table recenzija(
	sifra int not null primary key identity (1,1),
	korisnik int not null,
	film int not null,
	sadrzaj varchar (500)
);

create table ocjena(
	sifra int not null primary key identity (1,1),
	korisnik int not null,
	film int not null,
	vrijednost decimal (3,2)
);

create table slika(
	sifra int not null primary key identity (1,1),
	vrsta int not null,
	sifraVeze int not null,
	bytes varbinary(MAX)
);

alter table uloga add foreign key (film) references film(sifra) on delete cascade;
alter table uloga add foreign key (glumac) references glumac(sifra) on delete cascade;
alter table recenzija add foreign key (korisnik) references korisnik(sifra) on delete cascade;
alter table recenzija add foreign key (film) references film(sifra) on delete cascade;
alter table ocjena add foreign key (korisnik) references korisnik(sifra) on delete cascade;
alter table ocjena add foreign key (film) references film(sifra) on delete cascade;


 select * from film;

 insert into film (naziv,godina,redatelj,zanr)
 values
	('The Lion King',2019,'Jon Favreau','animacija'),
	('2 Hearts',2020,'Lance Hool','romantika'),
	('Forrest Gump',1994,'Robert Zemeckis','drama'),
	('Harry Potter and the Sorcerers Stone',2001,'Chris Columbus','fantazija'),
	('Deadpool',2016,'Tim Miller','komedija');

select * from glumac;

insert into glumac (ime,prezime,drzavljanstvo)
values
	('Donald','Glover','amerièko'),
	('Jacob','Elordi','australsko'),
	('Tom','Hanks','amerièko'),
	('Daniel','Radcliffe','britansko'),
	('Ryan','Reynolds','amerièko,kanadsko');

select * from uloga;

insert into uloga (film,glumac)
values
	(1,1),
	(2,2),
	(3,3),
	(4,3),
	(5,5);

select * from korisnik;

insert into korisnik (korisnickoIme,lozinka)
values
	('maiija','lioo234'),
	('lucky','srca22'),
	('inspectors','munja78'),
	('marija49','magija743'),
	('dominik96','hr4dt5');

select * from recenzija;

insert into recenzija (korisnik,film,sadrzaj)
values
	(1,1,'savršeno'),
	(2,2,'dobra izvedba'),
	(3,3,'nevjerojatan film'),
	(4,4,'èaroban i zabavan film'),
	(5,5,'urnebesan');

select * from ocjena;

insert into ocjena (korisnik,film,vrijednost)
values
	(1,1,'3.40'),
	(2,2,'3.15'),
	(3,3,'4.40'),
	(4,4,'3.80'),
	(5,5,'4.00');




-- izlistaj sve nazive filmove u kojima glumi Tom Hanks

select *
from film a
inner join uloga b on a.sifra=b.film
inner join glumac c on c.sifra=b.glumac 
where c.ime='Tom' and c.prezime='Hanks';


-- izlistaj sve komedije koje je režisirao Tim Miller ili Lance Hool

select * from film where zanr='komedija' and (redatelj='Tim Miller' or redatelj='Lance Hool');


-- izlistaj sve akcijske filmove koje su izašle prije 2010.

select * from film where zanr='Drama' and godina<2010;


-- izlistaj sve flmove kojima je ocjena viša od 3.50 i koji si izašli poslije 2010.

select b.vrijednost as ocjena, a.godina
from film a
inner join ocjena b on a.sifra=b.film
where b.vrijednost>'3.50' and godina>2010;


--izlistaj sve recenzije komedija, korisnika sa sifrom 3;

select *
from recenzija a
inner join film b on b.sifra=a.film
inner join korisnik c on c.sifra=a.korisnik
where zanr='Komedija' and c.sifra=3;

insert into recenzija (korisnik,film,sadrzaj) 
values (3,5,'chimichangas!')


--izlistaj sve filmove koje je korisnik sa sifrom 3 ocijeno sa 5;

select *
from film a
inner join ocjena b on b.film=a.sifra
inner join korisnik c on c.sifra=b.korisnik
where c.sifra=3 and b.vrijednost=5;

insert into ocjena (korisnik,film,vrijednost) 
values (1,2,5),(2,2,5);