using ApiFunkosCS.Database;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.Utils;
using ApiFunkosCS.Utils.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiFunkosCS.FunkoNamespace.Repository;

public class FunkoRepository : GenericRepository<Funko>, IFunkoRepository
{
    public FunkoRepository(TiendaDbContext context, ILogger<FunkoRepository> logger) 
        : base(context, logger)
    {
    }
}