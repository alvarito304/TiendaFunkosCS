using ApiFunkosCS.CategoryNamespace.Service;
using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Exception;
using ApiFunkosCS.FunkoNamespace.Mapper;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Repository;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.FunkoNamespace.Service;

public class FunkoService : IFunkoService
{
    private readonly ILogger<FunkoService> _logger;
    private readonly IFunkoRepository _funkoRepository;
    private readonly ICategoryService _categoryService;

    public FunkoService(ILogger<FunkoService> logger, IFunkoRepository funkoRepository, ICategoryService categoryService)
    {
        _logger = logger;
        _funkoRepository = funkoRepository;
        _categoryService = categoryService;
    }
    public async Task<List<FunkoDtoResponse>> FindAllAsync()
    {
        _logger.LogInformation($"Finding all Funkos");
        var funkos = await _funkoRepository.GetAllAsync();
        var result = new List<FunkoDtoResponse>();

        /*foreach (var funko in funkos)
        {
            var category = await _categoryService.FindByIdAsync(funko.CategoryID);
            if (category.IsSuccess)
            {
                result.Add(funko.MapToDto(category.Value));
            }
            else
            {
                _logger.LogError($"Failed to find category for Funko with id {funko.Name}");
            }
        }
        
        return result;*/
        return funkos.Select(f => f.MapToDto(_categoryService.FindByIdAsync(f.CategoryID).Result.Value)).ToList();

    }

    public async Task<Result<FunkoDtoResponse, FunkoError.NotFound>> FindByIdAsync(int id)
    {
        _logger.LogInformation($"Finding Funko with id {id}");
        var funko = await _funkoRepository.GetByIdAsync(id);

        if (funko == null) return Result.Failure<FunkoDtoResponse, FunkoError.NotFound>(new FunkoError.NotFound(id));
        var category = await _categoryService.FindByIdAsync(funko.CategoryID);
        return funko.MapToDto(category.Value);
    }

    public async Task<FunkoDtoResponse> CreateAsync(FunkoDtoSaveRequest funko)
    {
        _logger.LogInformation($"Creating Funko");
        await _funkoRepository.AddAsync(funko.MapToEntity());
        var category = await _categoryService.FindByIdAsync(funko.CategoryID);
         return funko.MapToEntity().MapToDto(category.Value);
    }

    public async Task<Result<FunkoDtoResponse, FunkoError.NotFound>> UpdateAsync(int id, FunkoDtoUpdateRequest funko)
    {
        _logger.LogInformation($"Updating Funko with id {id}");
        var existingFunko = await _funkoRepository.GetByIdAsync(id);
        
        if (existingFunko == null)
        {
            return Result.Failure<FunkoDtoResponse, FunkoError.NotFound>(new FunkoError.NotFound(id));
        }
        
        existingFunko.ImageUrl = funko.ImageUrl;
        existingFunko.Description = funko.Description;
        existingFunko.Price = funko.Price;
        existingFunko.Stock = funko.Stock;
        existingFunko.UpdatedAt = DateTime.Now;
        await _funkoRepository.UpdateAsync(existingFunko);
        var category = await _categoryService.FindByIdAsync(existingFunko.CategoryID);
        return Result.Success<FunkoDtoResponse, FunkoError.NotFound>(existingFunko.MapToDto(category.Value));
    }

    public async Task<Result<FunkoDtoResponse, FunkoError.NotFound>> DeleteAsync(int id)
    {
        _logger.LogInformation($"Deleting Funko with id {id}");
        var existingFunko = await _funkoRepository.GetByIdAsync(id);
        
        if (existingFunko == null)
        {
            return Result.Failure<FunkoDtoResponse, FunkoError.NotFound>(new FunkoError.NotFound(id));
        }
        
        await _funkoRepository.DeleteAsync(id);
        var category = await _categoryService.FindByIdAsync(existingFunko.CategoryID);
        return Result.Success<FunkoDtoResponse, FunkoError.NotFound>(existingFunko.MapToDto(category.Value));
    }

    public void DeleteAllAsync()
    {
        _logger.LogInformation($"Deleting all Funkos");
        _funkoRepository.DeleteAllAsync();
    }
}