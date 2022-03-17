using MetalModels.Models;
using Microsoft.EntityFrameworkCore;

namespace MetalScraper.Contracts
{
    public interface IMetalDbContext
    {
        DbSet<Band> Bands { get; set; }
        DbSet<Album> Albums { get; set; }
        DbSet<Song> Songs { get; set; }
        DbSet<Genre> Genres { get; set; }
    }
}
