using MetalScraper.Contracts;
using Microsoft.Extensions.Hosting;

namespace MetalScraper
{
    public class Scraper : BackgroundService
    {
        private IConfigHandler _configuration;

        public Scraper(IConfigHandler configuration)
        {
            _configuration = configuration;
        }

        public async Task ProcessAsync()
        {
            Console.WriteLine(_configuration.GetBasicUrl());
        }

        public void ScrapeBands()
        {

        }

        public void ScrapeAlbums()
        {

        }

        public void ScrapeSongs()
        {

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
