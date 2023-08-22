using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class ObradaGlumac
    {

        public List<Glumac> Glumci { get; }
        private Izbornik Izbornik;

        public ObradaGlumac(Izbornik izbornik) 
        {
            this.Izbornik= izbornik;
            Glumci = new List<Glumac>();
            if(Pomocno.dev)
            {
                TestniPodaci();
            }

        }

      

        public void PrikaziIzbornik ()
        {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("------ Izbornik za rad s glumcima -----");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("1. Pregled postojećih glumaca");
            Console.WriteLine("2. Unos novog glumca");
            Console.WriteLine("3. Promjena postojećeg glumca");
            Console.WriteLine("4. Povratak na glavni izbornik");
            switch (Pomocno.ucitajBrojRaspon ("Odaberite stavku izbornika glumca: ", 
                "Odabir mora biti 1.-4.", 1, 4))
            {
                case 1:
                    Console.Clear();
                    PregledGlumaca();
                    PrikaziIzbornik();
                    break;
                case 2:
                    Console.Clear();
                    DodajNovogGlumca();
                    Pomocno.uspjesnaPoruka("Unesen novi glumac!");
                    PrikaziIzbornik();
                    break;
                case 3:
                    Console.Clear();
                    PromjenaGlumca();
                    Pomocno.uspjesnaPoruka("Glumac izmjenjen!");
                    PrikaziIzbornik();
                    break;
                case 4:
                    Console.WriteLine("Gotov rad sa glumcima");
                    break;
            }
        }

       

        public void PregledGlumaca() {

            Console.WriteLine("------------------");
            Console.WriteLine("----- Glumci -----");
            Console.WriteLine("------------------");

            int b = 1;
            foreach (Glumac glumac in Glumci)
            {
                Console.WriteLine("{0}. {1} {2}", b++, glumac.Ime, glumac.Prezime);
            }
            Console.WriteLine("------------------");

        }

        private void DodajNovogGlumca()
        {
            Glumac noviGlumac = new Glumac();
            do
            {
                noviGlumac.Sifra = Pomocno.ucitajCijeliBroj("Unesite šifru glumca: ", "Unos mora biti pozitivan cijeli broj!");
            } while (Pomocno.provjeraSifre(noviGlumac.Sifra, Glumci.ToArray()));

            noviGlumac.Ime = Pomocno.ucitajString("Unesite ime glumca: ", "Unos mora biti obavezan!");
            noviGlumac.Prezime = Pomocno.ucitajString("Unesite prezime glumca: ", "Unos mora biti obavezan!");
            noviGlumac.Drzavljanstvo = Pomocno.ucitajString("Unesite državljantsvo: ", "Unos mora biti obavezan!");
            noviGlumac.filmovi = DodajFilmove();
            Glumci.Add(noviGlumac);

        }

        private void PromjenaGlumca()
        {
            PregledGlumaca();
            int index = Pomocno.ucitajBrojRaspon("Odredi redni broj glumca: ", "Nije dobar odabir", 1, Glumci.Count());
            Glumac g = Glumci[index - 1];
            Glumac promjene = new Glumac();


            promjene.Sifra = Pomocno.ucitajCijeliBroj("Unesite šifru glumca (" + g.Sifra + "): ", "Unos mora biti pozitivni cijeli broj!");
            promjene.Ime = Pomocno.ucitajString("Unesite ime glumca (" + g.Ime + "): ", "Unos obavezan!");
            promjene.Prezime = Pomocno.ucitajString("Unesite prezime glumca (" + g.Prezime + "): ", "Unos obavezan!");
            promjene.Drzavljanstvo = Pomocno.ucitajString("Unesite državljanstvo (" + g.Drzavljanstvo + "): ", "Unos obavezan!");

            if (Pomocno.ucitajBool("Spremi promjene? (da ili bilo što drugo za ne): "))
            {
                g.Sifra = promjene.Sifra;
                g.Ime = promjene.Ime;
                g.Prezime = promjene.Prezime;
                g.Drzavljanstvo=promjene.Drzavljanstvo;
            }
        }

        private List<Film> DodajFilmove()
        {
            List<Film> filmovi= new List<Film>();
            while (Pomocno.ucitajBool("Želite li dodati film? (da ili bilo što za ne): "))
            {
                filmovi.Add(OdaberiFilm());
            }

            return filmovi;
        }

        private Film OdaberiFilm()
        {
            Izbornik.ObradaFilm.PregledFilmova();
            int index = Pomocno.ucitajBrojRaspon("Odaberi redni broj filma: ", "Nije dobar odabir", 1, Izbornik.ObradaFilm.Filmovi.Count());
            return Izbornik.ObradaFilm.Filmovi[index - 1];
        }

        private void TestniPodaci()
        {
            Glumci.Add(new Glumac 
            {
                Sifra = 1,
                Ime = "Donald",
                Prezime = "Glover",
                Drzavljanstvo = "američko"
            });

            Glumci.Add(new Glumac
            {
                Sifra=2,
                Ime = "Jacob",
                Prezime = "Elordi",
                Drzavljanstvo = "australsko"
            });
        }


    }
}
