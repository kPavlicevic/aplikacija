namespace FilmRecenzijaApp.Models
{
    public class Korisnik : Entitet
    {
        public string KorisnickoIme { get; set; }
        public string? Lozinka { get; set; }
        public List<Komentar> Komentari { get; set; } = new();
    }
}
