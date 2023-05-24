use master;
drop database if exists recenzijaNaFilm;
go
create database recenzijaNaFilm;
go
use recenzijaNaFilm;

create table film(
	sifra int not null primary key identity (1,1),
	naziv varchar (50) not null,
	godina varchar (5),
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

alter table uloga add foreign key (film) references film(sifra);
alter table uloga add foreign key (glumac) references glumac(sifra);
alter table recenzija add foreign key (korisnik) references korisnik(sifra);
alter table recenzija add foreign key (film) references film(sifra);
alter table ocjena add foreign key (korisnik) references korisnik(sifra);
alter table ocjena add foreign key (film) references film(sifra);