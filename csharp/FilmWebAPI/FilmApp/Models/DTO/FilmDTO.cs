using System.Text.Json.Serialization;

namespace FilmRecenzijaApp.Models.DTO
{
    public class FilmDTO
    {
        public int Sifra { get; set; }
        public string? Naziv { get; set; }
        public int Godina { get; set; }
        public string? Redatelj { get; set; }
        public string? Zanr { get; set; }
    }
}
