

using System.ComponentModel.DataAnnotations;

namespace FilmRecenzijaApp.Models
{
    public class Film : Entitet
    {
        [Required(ErrorMessage = "Naziv obavezno")]
        public string? Naziv { get; set; }

        [Required]
        [Range(1890, 2024, ErrorMessage = "{0} mora biti između {1} i {2}")]
        public int Godina { get; set; }
        public string? Redatelj { get; set; }
        public string? Zanr { get; set; }
        public List<Glumac> Glumci { get; set; } = new();
        public List<Komentar> Komentari { get; set; } = new();
        public List<Ocjena> Ocjene { get; set; } = new();
    }
}
