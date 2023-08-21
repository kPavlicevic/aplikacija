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
            Console.WriteLine("4. Povratak na glavni izbornik");
            switch (Pomocno.ucitajBrojRaspon ("Odaberite stavku izbornika filma: ", 
                "Odabir mora biti 1.-4.", 1, 4 ))
            {
                case 1:
                    Console.Clear();
                    PregledFilmova();
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
                    Pomocno.uspjesnaPoruka("Gotov rad s filmovima");
                    break;

            }
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
            while (Pomocno.ucitajBool("Želite li dodati glumca? (da ili bilo što za ne): ")) {
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
        }
    }
}
