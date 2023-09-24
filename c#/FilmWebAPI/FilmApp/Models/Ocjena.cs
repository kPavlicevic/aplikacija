using System.ComponentModel.DataAnnotations.Schema;

namespace FilmRecenzijaApp.Models
{
    public class Ocjena : Entitet
    {
        [ForeignKey("korisnik")]
        public Korisnik? Korisnik { get; set; }

        [ForeignKey("film")]
        public Film? Film { get; set; }

        public decimal Vrijednost { get; set; }
    }
}
