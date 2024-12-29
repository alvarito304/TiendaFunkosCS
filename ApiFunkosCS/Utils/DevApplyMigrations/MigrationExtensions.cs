using ApiFunkosCS.Database;
using Microsoft.EntityFrameworkCore;

namespace ApiFunkosCS.Utils.DevApplyMigrations;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder context)
    {
        using IServiceScope scope = context.ApplicationServices.CreateScope();
        using TiendaDbContext dbContext = scope.ServiceProvider.GetRequiredService<TiendaDbContext>();
        dbContext.Database.Migrate(); // Aplica las migraciones si existen
    }
}