using FilmRecenzijaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRecenzijaApp.Data
{
    public class FilmRecenzijaContext : DbContext
    {
        public FilmRecenzijaContext(DbContextOptions<FilmRecenzijaContext>opcije)
            : base(opcije) {
        }

        public DbSet<Film>Film { get; set; }
    }
}
