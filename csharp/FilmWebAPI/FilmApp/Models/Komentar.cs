using System.ComponentModel.DataAnnotations.Schema;

namespace FilmRecenzijaApp.Models
{
    [Table("Recenzija")]
    public class Komentar : Entitet
    {
        [ForeignKey("korisnik")]
        public Korisnik? Korisnik { get; set; }
        [ForeignKey("film")]
        public Film? Film { get; set; }
        public string? Sadrzaj { get; set; }
    }
}
