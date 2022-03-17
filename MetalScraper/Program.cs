using MetalScraper;
using Microsoft.Extensions.DependencyInjection;
using MetalScraper.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var services = new ServiceCollection();
ConfigureServices(services);
await services.BuildServiceProvider()
    .GetService<Scraper>()
    .ProcessAsync();

static void ConfigureServices(IServiceCollection services)
{
    IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    services.AddSingleton(config);
    services.AddSingleton<IConfigHandler, ConfigHandler>();
    services.AddSingleton<Scraper, Scraper>();
    services.AddDbContext<MetalDbContext>(options => options.UseSqlServer());
}
