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

        public ObradaGlumac() { 
            Glumci = new List<Glumac>();
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

    }
}
