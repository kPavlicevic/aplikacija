namespace FilmRecenzijaApp.Models.DTO
{
    public class KomentarDTO
    {
        public int Sifra { get; set; }
        public Korisnik? Korisnik { get; set; }
        public string? Sadrzaj { get; set; }
    }
}
