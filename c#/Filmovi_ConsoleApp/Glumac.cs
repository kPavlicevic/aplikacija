using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmovi_ConsoleApp
{
    internal class Glumac : Entitet
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Drzavljanstvo { get; set; }
        public List<Film> filmovi { get; set; }
    }
}
