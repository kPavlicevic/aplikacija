namespace FilmApp.Models
{
    public class Film : Entitet
    {
        public string? Naziv { get; set; }
        public int Godina { get; set; }
        public string Redatelj { get; set; }
        public string Zanr { get; set; }
    }
}
