using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }

        public Korisnik(string korisnickoIme, string lozinka) 
        {
            this.KorisnickoIme = korisnickoIme;
            this.Lozinka = lozinka;
        }
        
        public Korisnik()
        {

        }

        public bool Jednako(Korisnik k1) {
            if (k1.KorisnickoIme == this.KorisnickoIme)
            {
                return true;
            }
            return false;
        }
    }
}
