using MetalModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MetalModels.Contracts
{
    public interface IMetalDbContext
    {
        DbSet<Band> Bands { get; set; }
        DbSet<Album> Albums { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<BandGenre> BandGenres { get; set; }
        DbSet<User> Users { get; set; }
    }
}
