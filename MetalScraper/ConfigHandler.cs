using MetalScraper.Contracts;
using Microsoft.Extensions.Configuration;

namespace MetalScraper
{
    internal class ConfigHandler : IConfigHandler
    {
        public IConfiguration _configuration;

        public ConfigHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConfig(string key)
        {
            return _configuration[$"Config:{key}"];
        }
    }
}
