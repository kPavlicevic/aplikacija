﻿using System.Text.RegularExpressions;

namespace FilmRecenzijaApp.Models
{
    public class Glumac : Entitet
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Drzavljanstvo { get; set; }
        public ICollection<Film> Filmovi { get; } = new List<Film>();
    }
}
