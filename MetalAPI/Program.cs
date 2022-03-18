using AutoMapper;
using MetalAPI;
using MetalModels;
using MetalServices;
using MetalServices.Contracts;
using MetalServices.Mapping;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    services.AddDbContext<MetalDbContext>();
    services.AddTransient<IBandService, BandService>();
    services.AddTransient<IAlbumService, AlbumService>();
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new MetalProfile());
    });
    IMapper mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);
}
