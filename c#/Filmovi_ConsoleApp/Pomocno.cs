using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class Pomocno
    {
        public static bool dev;
        public static int ucitajBrojRaspon (string poruka, string greska, 
            int poc, int kraj)
        {

            int b;
            while (true)
            {
                Console.Write (poruka);
                try
                {
                    b = int.Parse(Console.ReadLine());
                    if (b>=poc & b<=kraj)
                    {
                        return b;
                    }
                    Console.WriteLine(greska);
                }
                catch (Exception e)
                {
                    Console.WriteLine(greska);
                }
            }
        }

        public static int ucitajCijeliBroj(string poruka, string greska) {

            int b;
            while (true) { 
                
                Console.Write(poruka);
                try
                {
                    b = int.Parse(Console.ReadLine());
                    if (b > 0) {
                        return b;
                    }
                    Pomocno.neuspjesnaPoruka(greska);
                }
                catch (Exception e) {
                    Pomocno.neuspjesnaPoruka(greska);
                }
            }

        }

        public static string ucitajString(string poruka, string greska) {
            string s;
            while (true) {
                Console.Write(poruka);

                s = Console.ReadLine();
                if (s != null && s.Trim().Length > 0) {
                    return s;
                }
                Pomocno.neuspjesnaPoruka(greska);
            }
        }

        public static bool provjeraSifre(int sifra, Entitet[] Entiteti) {
            foreach (Entitet e in Entiteti) {
                if (e.Sifra == sifra) {
                    Pomocno.neuspjesnaPoruka("Unesena šifra već postoji!");
                    return true;
                }
            }
            return false;
        }

        public static bool ucitajBool(string poruka)
        {
            Console.Write(poruka);
            return Console.ReadLine().Trim().ToLower().Equals("da") ? true : false;
        }

        public static void uspjesnaPoruka(string poruka) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(poruka);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void neuspjesnaPoruka(string poruka) {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(poruka);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
