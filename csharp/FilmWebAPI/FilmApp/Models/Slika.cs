namespace FilmRecenzijaApp.Models
{
    public class Slika : Entitet
    {
        public int Vrsta { get; set; }
        public int SifraVeze { get; set; }
        public byte[] Bytes { get; set; }
    }
}
