using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class ObradaFilm
    {
        public List<Film> Filmovi { get; }

        private Izbornik Izbornik;

        public ObradaFilm(Izbornik izbornik)
        {
            this.Izbornik = izbornik;
            Filmovi = new List<Film>();
            if (Pomocno.dev)
            {
                TestniPodaci();
            }
        }

        public void PrikaziIzbornik()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("----- Izbornik za rad s filmovima -----");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. Pregled postojećih filmova");
            Console.WriteLine("2. Unos novog filma");
            Console.WriteLine("3. Promjena postojećeg filma");
            Console.WriteLine("4. Brisanje filma");
            Console.WriteLine("5. Povratak na glavni izbornik");
            switch (Pomocno.ucitajBrojRaspon ("Odaberite stavku izbornika filma: ", 
                "Odabir mora biti 1.-5.", 1, 5 ))
            {
                case 1:
                    Console.Clear();
                    DetaljniPregledFilmova();
                    Console.Clear();
                    PrikaziIzbornik();
                    break;
                case 2:
                    Console.Clear();
                    DodajNoviFilm();
                    Pomocno.uspjesnaPoruka("Unesen novi film!");
                    PrikaziIzbornik();
                    break;
                case 3:
                    Console.Clear();
                    PromjenaFilma();
                    Pomocno.uspjesnaPoruka("Film izmjenjen!");
                    PrikaziIzbornik();
                    break;
                case 4: 
                    Console.Clear();
                    BrisanjeFilma();
                    Pomocno.uspjesnaPoruka("Film uspješno obrisan!");
                    PrikaziIzbornik();
                    break;
                case 5:
                    Pomocno.uspjesnaPoruka("Gotov rad s filmovima");
                    break;

            }
        }

        private void BrisanjeFilma()
        {
            PregledFilmova();
            int index = Pomocno.ucitajBrojRaspon("Odaberi redni broj filma: ", "Nije dobar odabir!", 1, Filmovi.Count());
            Filmovi.RemoveAt(index - 1);
        }

        public void PregledFilmova()
        {
            Console.WriteLine("------------------");
            Console.WriteLine("----- Filmovi ----");
            Console.WriteLine("------------------");

            int b = 1;
            foreach (Film film in Filmovi)
            {
                Console.WriteLine("{0}. {1} ({2})", b++, film.Naziv, film.Godina);
            }
            Console.WriteLine("------------------");

        }

        public void DetaljniPregledFilmova()
        {
            PregledFilmova();
            int index = Pomocno.ucitajBrojRaspon("Odaberite film za pregled detalja i komentara:  ", "Nije dobar odabir!", 1, Filmovi.Count());
            Film f = Filmovi[index - 1];
            detaljniPrikazFilma(f);

        }

        public void detaljniPrikazFilma(Film f) {
            Console.Clear();
            Console.WriteLine("------------------");
            Console.WriteLine("----- {0} ----", f.Naziv);
            Console.WriteLine("------------------");
            Console.WriteLine("Godina: {0}", f.Godina);
            Console.WriteLine("Redatelj: {0}", f.Redatelj);
            Console.WriteLine("Žanr: {0}", f.Zanr);
            Console.Write("Ocjena: ");
            if (f.Ocjene != null && f.Ocjene.Count > 0)
            {
                Console.WriteLine(f.izracunajOcjenu());
            }
            else {
                Console.WriteLine("Još nitko nije ocijenio!");
            }
            Console.Write("Glumci: ");
            if (f.Glumci != null) { 
                foreach (Glumac g in f.Glumci) {
                    Console.Write("{0} {1}, ", g.Ime,g.Prezime);
                }
                Console.Write("\b\b");
            }
            Console.WriteLine();

            Console.WriteLine("--------- Komentari ---------");
            if (f.Komentari != null) { 
                foreach (Komentar k in f.Komentari) {
                    Console.WriteLine("{0}: {1}",k.Korisnik, k.Sadrzaj);
                }
            }

            IzbornikDetalja(f);
        }

        public void IzbornikDetalja(Film f)
        {
            
            Console.WriteLine("------------------");
            Console.WriteLine("----- akcije -----");
            Console.WriteLine("------------------");
            Console.WriteLine("1. Komentiraj");
            Console.WriteLine("2. Ocjeni");
            Console.WriteLine("3. Povratak");
            switch (Pomocno.ucitajBrojRaspon ("Odaberi stavku: ", "Odabir mora biti 1.-3.", 1, 3))
            {
                case 1:
                    Console.Clear();
                    KomentirajFilm(f);
                    detaljniPrikazFilma(f);
                    break;
                case 2:
                    Console.Clear();
                    OcjeniFilm(f);
                    detaljniPrikazFilma(f);
                    break;
                case 3:
                    Console.Clear();
                    break;
            }
        }

        private void OcjeniFilm(Film f)
        {
            while (Pomocno.ucitajBool("Ocjeni? (da ili bilo što za ne): "))
            {
                Ocjena o = new Ocjena();
                o.Vrijednost = Pomocno.ucitajBrojRaspon("Unesite ocjenu 1-5: ", "Odabir mora biti 1-5", 1, 5);
                f.Ocjene.Add(o);
            }
        }

        public void KomentirajFilm(Film f) {
            while (Pomocno.ucitajBool("Komentiraj? (da ili bilo što za ne): "))
            {
                Komentar k = new Komentar();
                k.Sadrzaj = Pomocno.ucitajString("Unesite komentar: ", "Unos obavezan!");
                if (this.Izbornik.trenutniKorisnik != null)
                {
                    k.Korisnik = this.Izbornik.trenutniKorisnik.KorisnickoIme;
                }
                else {
                    k.Korisnik = "gost";
                }
                f.Komentari.Add(k);
            }
        }

        public void DodajNoviFilm() {
            Film noviFilm = new Film();
            //Provjera postoji li već unesena sifra u ovoj vrsti entiteta
            do {
                noviFilm.Sifra = Pomocno.ucitajCijeliBroj("Unesite šifru filma: ", "Unos mora biti pozitivni cijeli broj!");
            } while (Pomocno.provjeraSifre(noviFilm.Sifra, Filmovi.ToArray()));

            noviFilm.Naziv = Pomocno.ucitajString("Unesite naziv filma: ","Unos obavezan!");
            noviFilm.Godina = Pomocno.ucitajCijeliBroj("Unesite godinu filma: ", "Unos mora biti pozitivni cijeli broj!");
            noviFilm.Redatelj = Pomocno.ucitajString("Unesite redatelja filma: ", "Unos obavezan!");
            noviFilm.Zanr = Pomocno.ucitajString("Unesite žanr filma: ", "Unos obavezan!");
            noviFilm.Glumci = DodajGlumce();
            Filmovi.Add(noviFilm);
        }

        public void PromjenaFilma() {

            PregledFilmova();
            int index = Pomocno.ucitajBrojRaspon("Odredite redni broj filma: ", "Nije dobar odabir", 1, Filmovi.Count());
            Film f = Filmovi[index - 1];
            Film promjene = new Film();


        
            promjene.Sifra = Pomocno.ucitajCijeliBroj("Unesite šifru filma (" + f.Sifra + "): ", "Unos mora biti pozitivni cijeli broj!");
            promjene.Naziv = Pomocno.ucitajString("Unesite naziv filma (" + f.Naziv + "): ", "Unos obavezan!");
            promjene.Godina = Pomocno.ucitajCijeliBroj("Unesite godinu filma (" + f.Godina + "): ", "Unos mora biti pozitivni cijeli broj!");
            promjene.Redatelj = Pomocno.ucitajString("Unesite redatelja filma (" + f.Redatelj + "): ", "Unos obavezan!");
            promjene.Zanr = Pomocno.ucitajString("Unesite žanr filma (" + f.Zanr + "): ", "Unos obavezan!");

            if(Pomocno.ucitajBool("Spremi promjene? (da ili bilo što drugo za ne): ")) {
                f.Sifra = promjene.Sifra;
                f.Naziv = promjene.Naziv;
                f.Zanr = promjene.Zanr;
                f.Redatelj = promjene.Redatelj;
                f.Godina = promjene.Godina;
            }

        }

        private List<Glumac> DodajGlumce() { 
            List<Glumac> glumci = new List<Glumac>();
            while (Pomocno.ucitajBool("Želite li dodati glumca? (da ili bilo što drugo za ne): ")) {
                glumci.Add(OdaberiGlumca());
            }

            return glumci;
        }

        private Glumac OdaberiGlumca() {
            Izbornik.ObradaGlumac.PregledGlumaca();
            int index = Pomocno.ucitajBrojRaspon("Odaberi redni broj glumca: ", "Nije dobar odabir", 1, Izbornik.ObradaGlumac.Glumci.Count());
            return Izbornik.ObradaGlumac.Glumci[index - 1];
        }

        private void TestniPodaci()
        {
            Filmovi.Add(new Film
            {
                Sifra = 1,
                Naziv = "The Lion King",
                Godina = 2019,
                Redatelj = "Jon Favreau",
                Zanr = "animacija"

            });

            Filmovi.Add(new Film
            {
                Sifra = 2,
                Naziv = "2 Hearts",
                Godina = 2020,
                Redatelj = "Lance Hool",
                Zanr = "romantika"
            });

            Filmovi.Add(new Film
            {
                Sifra = 3,
                Naziv = "Forrest Gump",
                Godina = 1994,
                Redatelj = "Robert Zemeckis",
                Zanr = "drama"
            });

            Filmovi.Add(new Film
            {
                Sifra = 4,
                Naziv = "Harry Potter and the Sorcerers Stone",
                Godina = 2001,
                Redatelj = "Chris Columbus",
                Zanr = "fantazija"
            });

            Filmovi.Add(new Film
            {
                Sifra = 5,
                Naziv = "Deadpool",
                Godina = 2016,
                Redatelj = "Tim Miller",
                Zanr = "komedija"
            });
        }
    }
}
