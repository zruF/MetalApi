using MetalScraper;
using Microsoft.Extensions.DependencyInjection;
using MetalScraper.Contracts;
using Microsoft.Extensions.Configuration;

var services = new ServiceCollection();
ConfigureServices(services);
await services.AddSingleton<Scraper, Scraper>()
    .BuildServiceProvider()
    .GetService<Scraper>()
    .ProcessAsync();

static void ConfigureServices(IServiceCollection services)
{
    IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    services.AddSingleton(config);
    services.AddSingleton<IConfigHandler, ConfigHandler>();
}
