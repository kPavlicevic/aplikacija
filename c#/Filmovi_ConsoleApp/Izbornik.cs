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

        public Izbornik() 
        { 
            ObradaFilm = new ObradaFilm(this);
            ObradaGlumac = new ObradaGlumac();
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

            switch (Pomocno.ucitajBrojRaspon("Odaberi stavku izbornika: ", "Odabir mora biti 1.-4.", 1, 2 ))
            {
                case 1:
                    Console.Clear();
                    ObradaFilm.PrikaziIzbornik();
                    PrikaziIzbornik();
                    break;
               
            }
        }
    }

}


