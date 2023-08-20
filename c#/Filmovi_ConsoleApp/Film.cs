using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
