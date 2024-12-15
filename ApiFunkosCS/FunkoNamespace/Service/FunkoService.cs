using ApiFunkosCS.FunkoNamespace.Exception;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Repository;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.FunkoNamespace.Service;

public class FunkoService : IFunkoService
{
    private readonly ILogger<FunkoService> _logger;
    private readonly IFunkoRepository _funkoRepository;

    public FunkoService(ILogger<FunkoService> logger, IFunkoRepository funkoRepository)
    {
        _logger = logger;
        _funkoRepository = funkoRepository;
    }
    public async Task<List<Funko>> FindAllAsync()
    {
        _logger.LogInformation($"Finding all Funkos");
        return await _funkoRepository.GetAllAsync();
    }

    public async Task<Result<Funko, FunkoError.NotFound>> FindByIdAsync(int id)
    {
        _logger.LogInformation($"Finding Funko with id {id}");
        var funko = await _funkoRepository.GetByIdAsync(id);

        if (funko == null) return Result.Failure<Funko, FunkoError.NotFound>(new FunkoError.NotFound(id));
        
        return funko;
    }

    public async Task<Funko> CreateAsync(Funko funko)
    {
        _logger.LogInformation($"Creating Funko");
        await _funkoRepository.AddAsync(funko);
         return funko;
    }

    public async Task<Result<Funko, FunkoError.NotFound>> UpdateAsync(int id, Funko funko)
    {
        _logger.LogInformation($"Updating Funko with id {id}");
        var existingFunko = await _funkoRepository.GetByIdAsync(id);
        
        if (existingFunko == null)
        {
            return Result.Failure<Funko, FunkoError.NotFound>(new FunkoError.NotFound(id));
        }
        
        existingFunko.Update(funko);
        await _funkoRepository.UpdateAsync(existingFunko);
        return Result.Success<Funko, FunkoError.NotFound>(existingFunko);
    }

    public async Task<Result<Funko, FunkoError.NotFound>> DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting Funko with id {id}");
        var existingFunko = await _funkoRepository.GetByIdAsync(id);
        
        if (existingFunko == null)
        {
            return Result.Failure<Funko, FunkoError.NotFound>(new FunkoError.NotFound(id));
        }
        
        await _funkoRepository.DeleteAsync(id);
        return Result.Success<Funko, FunkoError.NotFound>(existingFunko);
    }

    public void DeleteAllAsync()
    {
        _logger.LogInformation($"Deleting all Funkos");
        _funkoRepository.DeleteAllAsync();
    }
}