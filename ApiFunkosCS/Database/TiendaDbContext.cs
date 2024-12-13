
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiFunkosCS.Database;

public class TiendaDbContext(DbContextOptions<TiendaDbContext> options) : DbContext(options)
{
    public DbSet<Funko> Funkos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Funko>(entity =>
        {
            entity.Property(e => e.CreatedAt).IsRequired()
                .ValueGeneratedOnAdd(); 
            entity.Property(e => e.UpdatedAt).IsRequired()
                .ValueGeneratedOnUpdate(); 
        });
    }
}