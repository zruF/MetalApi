using MetalModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MetalModels
{
    public class MetalDbContext : DbContext
    {
        private string DbPath { get; set; }

        public MetalDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "metal.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Band>().HasMany(e => e.Albums).WithOne().HasForeignKey(a => a.BandId);
            modelBuilder.Entity<Album>().HasMany(e => e.Songs).WithOne().HasForeignKey(s => s.AlbumId);
            modelBuilder.Entity<BandGenre>().HasKey(e => new { e.BandId, e.GenreId });
            modelBuilder.Entity<BandGenre>().HasOne(bg => bg.Band).WithMany(b => b.BandGenres).HasForeignKey(bg => bg.GenreId);
            modelBuilder.Entity<BandGenre>().HasOne(bg => bg.Genre).WithMany(b => b.BandGenres).HasForeignKey(bg => bg.BandId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
            options.EnableSensitiveDataLogging();
        }

        public DbSet<Band> Bands { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BandGenre> BandGenres { get; set; }
    }
}
