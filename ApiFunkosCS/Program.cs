
using System.Text;
using ApiFunkosCS.CategoryNamespace.Database;
using ApiFunkosCS.CategoryNamespace.Repository;
using ApiFunkosCS.CategoryNamespace.Service;
using ApiFunkosCS.Database;
using ApiFunkosCS.FunkoNamespace.Repository;
using ApiFunkosCS.FunkoNamespace.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Serilog;

Console.OutputEncoding = Encoding.UTF8;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
Console.WriteLine($"Environment: {environment}");

// Configuramos Serilog
var configuration = new ConfigurationBuilder()
    .AddJsonFile($"logger.{environment}.json", optional: false, reloadOnChange: true)
    .Build();

// Creamos un logger con la configuración de Serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.ClearProviders(); // Limpia los proveedores de log por defecto
    logging.AddSerilog(logger, true); // Añade Serilog como un proveedor de log
});
logger.Debug("Serilog added as default logger");

builder.Services.AddDbContext<TiendaDbContext>(options =>
    {
        options.UseInMemoryDatabase("Funkos")
            // Disable log
            .EnableSensitiveDataLogging(); // Habilita el registro de datos sensibles
        logger.Debug("In-memory database added");
    }
);
logger.Debug("Funkos in-memory database added");

builder.Services.Configure<CategoryDatabaseSettings>(
    builder.Configuration.GetSection("CategoryStoreDatabase"));
    


/**************** INYECCION DE DEPENDENCIAS **************/

// FUNKO
builder.Services.AddScoped<IFunkoRepository, FunkoRepository>(); 
builder.Services.AddScoped<IFunkoService, FunkoService>();

// CATEGORIA
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

/*********************************************************/
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    // Otros metadatos de la API
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Tienda Funkos API",
        Description = "An API to perform CRUD operations on Funkos and Categories",
        Contact = new OpenApiContact
        {
            Name = "Álvaro Herrero Tamayo",
            Email = "alvaro.herrero11x@gmail.com",
            Url = new Uri("https://alvarito304github.io")
        },
    });
}); // Agrega SwaggerGen para generar documentación de la API
logger.Debug("Swagger/OpenAPI services added");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

logger.Information("🚀 Tienda API started 🟢");
Console.WriteLine("🚀 Tienda API started 🟢");

app.Run();