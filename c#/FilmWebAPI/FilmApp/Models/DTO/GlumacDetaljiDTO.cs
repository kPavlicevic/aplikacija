namespace FilmRecenzijaApp.Models.DTO
{
    public class GlumacDetaljiDTO
    {
        public int Sifra { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Drzavljanstvo { get; set; }
        public List<FilmDTO> Filmovi { get; set; }

    }
}
