using MetalModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MetalModels
{
    public class MetalDbContext : DbContext
    {
        private readonly string _connectionString;
        public MetalDbContext()
        {
        }

        public MetalDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*
             * Server=tcp:metal-database.database.windows.net,1433;Initial Catalog=MetalDatabase;Persist Security Info=False;User ID=metal-db-user;Password=b4jn0vJXON1my4XU;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
             * */
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Band>().HasMany(e => e.Albums).WithOne(a => a.Band).HasForeignKey(a => a.BandId);
            modelBuilder.Entity<Album>().HasOne(e => e.Band).WithMany().HasForeignKey(a => a.AlbumId);
            modelBuilder.Entity<BandGenre>().HasKey(e => new { e.BandId, e.GenreId });
            modelBuilder.Entity<BandGenre>().HasOne(bg => bg.Band).WithMany(b => b.BandGenres).HasForeignKey(bg => bg.GenreId);
            modelBuilder.Entity<BandGenre>().HasOne(bg => bg.Genre).WithMany(b => b.BandGenres).HasForeignKey(bg => bg.BandId);

            modelBuilder.Entity<BandFavorites>().HasKey(e => new { e.UserId, e.BandId });
            modelBuilder.Entity<BandFavorites>().HasOne(bg => bg.User).WithMany(b => b.BandFavorites).HasForeignKey(bg => bg.BandId);
            modelBuilder.Entity<BandFavorites>().HasOne(bg => bg.Band).WithMany(b => b.Favorites).HasForeignKey(bg => bg.UserId);

            modelBuilder.Entity<GenreFavorites>().HasKey(e => new { e.UserId, e.GenreId });
            modelBuilder.Entity<GenreFavorites>().HasOne(bg => bg.User).WithMany(b => b.GenreFavorites).HasForeignKey(bg => bg.GenreId);
            modelBuilder.Entity<GenreFavorites>().HasOne(bg => bg.Genre).WithMany(b => b.Favorites).HasForeignKey(bg => bg.UserId);
        }

        public DbSet<Band> Bands { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BandGenre> BandGenres { get; set; }
    }
}
