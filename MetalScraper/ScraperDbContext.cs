using MetalScraper.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MetalScraper
{
    public class ScraperDbContext : DbContext, IScraperDbContext
    {
        public ScraperDbContext()
        {

        }
    }
}
