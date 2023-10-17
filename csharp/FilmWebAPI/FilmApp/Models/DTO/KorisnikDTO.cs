﻿using System.Text.Json.Serialization;

namespace FilmRecenzijaApp.Models.DTO
{
    public class KorisnikDTO
    {
        public int Sifra { get; set; }
        public string? KorisnickoIme { get; set; }
        [JsonIgnore]
        public string? Lozinka { get; set; }
    }
}
