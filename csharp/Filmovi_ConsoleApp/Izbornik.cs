using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class Izbornik
    {
        public ObradaFilm ObradaFilm { get; }
        public ObradaGlumac ObradaGlumac { get; }
        public ObradaKorisnik ObradaKorisnik { get; }

        public Korisnik trenutniKorisnik { get; set; }

        public Izbornik() 
        {
            ObradaFilm = new ObradaFilm(this);
            ObradaGlumac = new ObradaGlumac(this);
            ObradaKorisnik = new ObradaKorisnik();
            PrikaziIzbornik();
        }


        private void PozdravnaPoruka()
        {
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("----- RecenzijaNaFilm Console v 1.0 -----");
            Console.WriteLine("-----------------------------------------");
        }

        private void PrikaziIzbornik()
        {
            PozdravnaPoruka();
            Console.WriteLine("Glavni izbornik");
            Console.WriteLine("1. Filmovi");
            Console.WriteLine("2. Glumci");
            if (trenutniKorisnik != null)
            {
                Console.WriteLine("3. Odjava");
                Console.Write("Prijavljen kao: ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(trenutniKorisnik.KorisnickoIme); 
                Console.ForegroundColor = ConsoleColor.White;
            }else {
                Console.WriteLine("3. Prijava");
            }

            switch (Pomocno.ucitajBrojRaspon("Odaberi stavku izbornika: ", "Odabir mora biti 1.-3.", 1, 3))
            {
                case 1:
                    Console.Clear();
                    ObradaFilm.PrikaziIzbornik();
                    PrikaziIzbornik();
                    break;
                case 2:
                    Console.Clear();
                    ObradaGlumac.PrikaziIzbornik();
                    PrikaziIzbornik();
                    break;
                case 3:
                    Console.Clear();
                    if (trenutniKorisnik != null)
                    {
                        trenutniKorisnik = null;
                        Pomocno.uspjesnaPoruka("Odjava uspješna!");
                    }
                    else { 
                        this.trenutniKorisnik = ObradaKorisnik.Prijava();
                    }
                    PrikaziIzbornik();
                    break;
               
            }
        }
    }

}


