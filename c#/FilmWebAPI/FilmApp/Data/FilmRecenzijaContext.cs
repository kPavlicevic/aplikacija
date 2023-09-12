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
        public DbSet<Glumac> Glumac { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>()
                .HasMany(f => f.Glumci)
                .WithMany(g => g.Filmovi)
                .UsingEntity<Dictionary<string, object>>("uloga",
                u => u.HasOne<Glumac>().WithMany().HasForeignKey("glumac"),
                u => u.HasOne<Film>().WithMany().HasForeignKey("film"),
                u => u.ToTable("uloga") 
                );
        }
    }
}
