using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class ObradaKorisnik
    {
        private List<Korisnik> Korisnici;
        public ObradaKorisnik()
        {
            this.Korisnici = new List<Korisnik>();
            if (Pomocno.dev)
            {
                TestniPodaci();
            }
        }


        public Korisnik Prijava()
        {
            Korisnik trenutniKorisnik = unosKorisnik();
            if (Korisnici != null && Korisnici.Count > 0)
            {
                foreach (Korisnik korisnik in Korisnici)
                {
                    if (trenutniKorisnik.Jednako(korisnik))
                    {
                        if (trenutniKorisnik.Lozinka != korisnik.Lozinka)
                        {
                            do{
                                Pomocno.neuspjesnaPoruka("Neispravna lozinka!");
                                trenutniKorisnik.Lozinka = Pomocno.ucitajString("Lozinka: ", "Unos obavezan!");
                            } while (trenutniKorisnik.Lozinka != korisnik.Lozinka);
                        }
                        return trenutniKorisnik;

                    }
                }
            }
            Pomocno.neuspjesnaPoruka("Korisnk ne postoji!");
            return Akcija();
            
        }

        public Korisnik Akcija ()
        {
            Console.WriteLine("1. Registriraj se");
            Console.WriteLine("2. Nastavi kao gost");
            switch (Pomocno.ucitajBrojRaspon("Odaberite stavku: ", "Odabbir mora biti 1-2!", 1, 2)) {
                case 1:
                    return Registracija();
                default:
                    return new Korisnik("gost", "");
            }
        }

        private Korisnik Registracija()
        {
            Korisnik noviKorisnik;
            bool postoji;
            do {
                postoji = false;
                noviKorisnik = unosKorisnik();
                if (Korisnici != null && Korisnici.Count > 0) { 
                    foreach (Korisnik korisnik in Korisnici)
                    {
                        if (noviKorisnik.Jednako(korisnik))
                        {
                            postoji = true;
                            Pomocno.neuspjesnaPoruka("Korisničko ime se već koristi!");
                            break;
                        }
                    }
                }
            } while (postoji);
            Korisnici.Add(noviKorisnik);
            Pomocno.uspjesnaPoruka("Registracija uspješna!");
            return noviKorisnik;
        }

        private Korisnik unosKorisnik() {
            string kIme = Pomocno.ucitajString("Korisničko ime: ", "Unos obavezan!");
            string lozinka = Pomocno.ucitajString("Lozinka: ", "Unos obavezan!");
            return new Korisnik(kIme, lozinka);
        }
        private void TestniPodaci()
        {
            this.Korisnici.Add(new Korisnik
            {
                KorisnickoIme = "koko",
                Lozinka = "koko"
            });
                
        }
    }
}
