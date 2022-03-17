using MetalModels.Models;
using MetalScraper.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MetalScraper
{
    public class MetalDbContext : DbContext, IMetalDbContext
    {
        private readonly string _connectionString;
        private string DbPath { get; set; }

        public MetalDbContext(string connectionString)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "metal.db");
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Band>().HasMany(e => e.Albums).WithOne();
            modelBuilder.Entity<Band>().HasMany(e => e.Genres);
            modelBuilder.Entity<Album>().HasMany(e => e.Songs).WithOne();
            modelBuilder.Entity<BandGenres>().HasKey(e => new { e.BandId, e.GenreId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }

        public DbSet<Band> Bands { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
