using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class Film : Entitet
    {
        public string Naziv { get; set; }
        public int Godina { get; set; }
        public string Redatelj { get; set; }
        public string Zanr { get; set; }
        public List<Glumac> Glumci {get; set;}
        public List<Komentar> Komentari { get; set; }

        public List<Ocjena> Ocjene { get; set; }
        public Film() { 
            this.Komentari = new List<Komentar>();
            this.Ocjene = new List<Ocjena>();
        }


        public float izracunajOcjenu ()
        {
            float sum = 0;
            foreach (Ocjena o in Ocjene)
            {
                sum += o.Vrijednost;
            }
            return sum/Ocjene.Count;
        }

    }
}
