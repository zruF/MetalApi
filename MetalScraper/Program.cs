using MetalScraper;
using Microsoft.Extensions.DependencyInjection;
using MetalScraper.Contracts;
using Microsoft.Extensions.Configuration;
using MetalModels;
using ConfigurationManager = System.Configuration.ConfigurationManager;

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
    services.AddSingleton(new MetalDbContext("Server=tcp:metal-database.database.windows.net,1433;Initial Catalog=MetalDatabase;Persist Security Info=False;User ID=metal-db-user;Password=b4jn0vJXON1my4XU;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
}
