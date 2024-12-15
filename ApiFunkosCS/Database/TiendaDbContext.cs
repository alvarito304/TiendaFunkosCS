
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiFunkosCS.Database;

public class TiendaDbContext : DbContext
{
    public TiendaDbContext(DbContextOptions<TiendaDbContext> options) : base(options) { }
    public DbSet<Funko> Funkos => Set<Funko>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Funko>(entity =>
        {
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .ValueGeneratedOnUpdate();
        });
    }
}
