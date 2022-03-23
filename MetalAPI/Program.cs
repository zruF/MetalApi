using AutoMapper;
using MetalAPI;
using MetalModels;
using MetalServices;
using MetalServices.Contracts;
using MetalServices.Mapping;
using ConfigurationManager = System.Configuration.ConfigurationManager;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    services.AddSingleton(config);
    services.AddSingleton(new MetalDbContext(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString));
    services.AddTransient<IBandService, BandService>();
    services.AddTransient<IAlbumService, AlbumService>();
    services.AddTransient<IGenreService, GenreService>();
    services.AddTransient<ISearchService, SearchService>();
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MetalProfile());
    });
    IMapper mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);
}
