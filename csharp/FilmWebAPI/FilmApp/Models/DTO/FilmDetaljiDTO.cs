namespace FilmRecenzijaApp.Models.DTO
{
    public class FilmDetaljiDTO
    {
        public int Sifra { get; set; }
        public string? Naziv { get; set; }
        public int Godina { get; set; }
        public string? Redatelj { get; set; }
        public string? Zanr { get; set; }
        public List<GlumacDTO> Glumci { get; set; } = new();
        public List<KomentarDTO> Komentari { get; set; } = new();
        public List<OcjenaDTO> Ocjene { get; set; } = new();
    }
}
