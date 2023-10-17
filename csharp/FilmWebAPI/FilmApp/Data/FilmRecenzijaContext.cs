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

        public DbSet<Komentar> Komentar { get; set; }

        public DbSet<Korisnik> Korisnik { get; set; }
        public DbSet<Ocjena> Ocjena { get; set; }

        public DbSet<Slika> Slika { get; set; }
        

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

            modelBuilder.Entity<Komentar>().HasOne(k => k.Korisnik);
            modelBuilder.Entity<Komentar>().HasOne(k => k.Film);

            modelBuilder.Entity<Ocjena>().HasOne(o => o.Korisnik);
            modelBuilder.Entity<Ocjena>().HasOne(o => o.Film);


        }
    }
}
