using HtmlAgilityPack;
using MetalScraper.Contracts;
using Microsoft.Extensions.Hosting;

namespace MetalScraper
{
    public class Scraper : BackgroundService
    {
        private IConfigHandler _config;

        public Scraper(IConfigHandler config)
        {
            _config = config;
        }

        public async Task ProcessAsync()
        {
            var stackSize = int.Parse(_config.GetSetting("EntryStackSize"));
            var maximumEntries = int.Parse(_config.GetSetting("MaximumEntries"));

            for(var currentStack = 0; currentStack < maximumEntries; currentStack += stackSize)
            {
                var url = $"{_config.GetSetting("BasicUrl")}{string.Format(_config.GetSetting("Query"), currentStack, maximumEntries)}";
                var rawCode = await new HtmlWeb().LoadFromWebAsync(url);
                var link = rawCode.DocumentNode.SelectNodes("//a[@href]");
            }
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
