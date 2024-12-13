using System.Text;
using ApiFunkosCS.CategoryNamespace.Database;
using ApiFunkosCS.CategoryNamespace.Repository;
using ApiFunkosCS.CategoryNamespace.Service;
using ApiFunkosCS.Database;
using ApiFunkosCS.FunkoNamespace.Repository;
using ApiFunkosCS.FunkoNamespace.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;

Console.OutputEncoding = Encoding.UTF8; // Configura la codificación de salida de la consola a UTF-8 para mostrar caracteres especiales.

var environment = InitLocalEnvironment(); // Inicializa y obtiene el entorno de ejecución actual de la aplicación.

var configuration = InitConfiguration(); // Construye y obtiene la configuración de la aplicación desde archivos JSON.

var logger = InitLogConfig(); // Inicializa y configura el logger de Serilog para registrar eventos y mensajes.

var builder = InitServices(); // Configura y obtiene un WebApplicationBuilder con servicios necesarios.

builder.Services.AddControllers(); // Agrega soporte para controladores, permitiendo manejar solicitudes HTTP.

builder.Services.AddEndpointsApiExplorer(); // Agrega servicios para explorar los endpoints de la API, necesario para Swagger.

var app = builder.Build(); // Construye la aplicación web a partir del WebApplicationBuilder.

if (app.Environment.IsDevelopment()) // Verifica si el entorno es de desarrollo.
{
    app.UseSwagger(); // Habilita Swagger para generar documentación de la API.
    app.UseSwaggerUI(); // Habilita Swagger UI para explorar y probar la API visualmente.
}

app.UseHttpsRedirection(); // Redirige automáticamente las solicitudes HTTP a HTTPS para mejorar la seguridad.

app.UseRouting(); // Habilita el enrutamiento para dirigir las solicitudes HTTP a los controladores correspondientes.

app.UseAuthorization(); // Habilita la autorización para asegurar el acceso a recursos protegidos.

app.MapControllers(); // Mapea las rutas de los controladores a los endpoints de la aplicación.

logger.Information("🚀 Tienda API started 🟢"); // Registra un mensaje informativo indicando que la API ha iniciado.
Console.WriteLine("🚀 Tienda API started 🟢"); // Muestra un mensaje en la consola indicando que la API ha iniciado.

app.Run(); // Inicia la aplicación y comienza a escuchar las solicitudes HTTP entrantes.

string InitLocalEnvironment()
{
    Console.OutputEncoding = Encoding.UTF8; // Necesario para mostrar emojis
    var myEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
    Console.WriteLine($"Environment: {myEnvironment}");
    return myEnvironment;
}

IConfiguration InitConfiguration()
{
    var myConfiguration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{environment}.json", true)
        .Build();
    return myConfiguration;
}

Logger InitLogConfig()
{
    // Creamos un logger con la configuración de Serilog
    return new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

WebApplicationBuilder InitServices()
{
    
    var myBuilder = WebApplication.CreateBuilder(args);
    
    
    myBuilder.Services.AddLogging(logging =>
    {
        logging.ClearProviders(); // Limpia los proveedores de log por defecto
        logging.AddSerilog(logger, true); // Añade Serilog como un proveedor de log
    });
    logger.Debug("Serilog added as default logger");

    
    /**************** FUNKOS DATABASE SETTINGS **************/
    myBuilder.Services.AddDbContext<TiendaDbContext>(options =>
        {
            options.UseInMemoryDatabase("Funkos")
                // Disable log
                .EnableSensitiveDataLogging(); // Habilita el registro de datos sensibles
            logger.Debug("In-memory database added");
        }
    );
    /*********************************************************/
    
    /**************** CATEGORY DATABASE SETTINGS **************/
    myBuilder.Services.Configure<CategoryDatabaseSettings>(
        myBuilder.Configuration.GetSection("CategoryStoreDatabase"));
    /*********************************************************/
    


/**************** INYECCION DE DEPENDENCIAS **************/
// REPOSITORIO Y SERVICIOS

// FUNKO
    myBuilder.Services.AddScoped<IFunkoRepository, FunkoRepository>(); 
    myBuilder.Services.AddScoped<IFunkoService, FunkoService>();

// CATEGORIA
    myBuilder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    myBuilder.Services.AddScoped<ICategoryService, CategoryService>();

/*********************************************************/

/****************  DOCUMENTACION DE SWAGGER **************/
    myBuilder.Services.AddSwaggerGen(c =>
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
/*********************************************************/

return myBuilder;
}