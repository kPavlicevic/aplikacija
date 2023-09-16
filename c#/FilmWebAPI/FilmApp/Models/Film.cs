

namespace FilmRecenzijaApp.Models
{
    public class Film : Entitet
    {
        public string? Naziv { get; set; }
        public int Godina { get; set; }
        public string? Redatelj { get; set; }
        public string? Zanr { get; set; }
        public List<Glumac> Glumci { get; set; } = new();
        public List<Komentar> Komentari { get; set; } = new();
        public List<Ocjena> Ocjene { get; set; } = new();
    }
}
