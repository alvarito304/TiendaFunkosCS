
using ApiFunkosCS.Database;
using ApiFunkosCS.Repository;
using Microsoft.EntityFrameworkCore;

namespace ApiFunkosCS.Utils;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly TiendaDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly ILogger _logger;

    public GenericRepository(TiendaDbContext context, ILogger logger)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _logger = logger;
    }

    public async Task<List<T>> GetAllAsync()
    {
        _logger.LogInformation($"Getting all {typeof(T).Name}s");
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Getting {typeof(T).Name} with id {id}");
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        _logger.LogInformation($"Adding {typeof(T).Name}");
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _logger.LogInformation($"Updating {typeof(T).Name} entity: {entity}");
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting {typeof(T).Name} with id {id}");
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAllAsync()
    {
        _logger.LogInformation($"Deleting all {typeof(T).Name}s");
        var entities = _dbSet.ToList();
        _dbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
}
