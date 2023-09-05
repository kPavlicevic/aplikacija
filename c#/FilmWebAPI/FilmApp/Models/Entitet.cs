using System.ComponentModel.DataAnnotations;

namespace FilmRecenzijaApp.Models
{
    public abstract class Entitet
    {
        [Key]
        public int Sifra { get; set; }
    }
}
