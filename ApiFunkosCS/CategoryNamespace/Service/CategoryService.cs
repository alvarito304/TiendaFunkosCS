using ApiFunkosCS.CategoryNamespace.Errors;
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.CategoryNamespace.Repository;
using ApiFunkosCS.Storage.Exceptions;
using ApiFunkosCS.Storage.ProcessingFile.Services;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.CategoryNamespace.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;
    private readonly ICategoryStorageCsv _categoryStorageCsv;
    
    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger, ICategoryStorageCsv categoryStorageCsv)
    {
        _repository = repository;
        _logger = logger;
        _categoryStorageCsv = categoryStorageCsv;
    }
    public async Task<List<Category>> FindAllAsync()
    {
        _logger.LogInformation("Finding all categories");
        return await _repository.GetAllAsync();
    }

    public async Task<Result<Category, CategoryError.NotFound>> FindByIdAsync(string id)
    {
        _logger.LogInformation($"Finding category by id: {id}");
        var category = await _repository.GetByIdAsync(id);
        return category is null
           ? Result.Failure<Category, CategoryError.NotFound>( new CategoryError.NotFound(id))
            : Result.Success<Category, CategoryError.NotFound>(category);
    }

    public async Task<Category> AddAsync(Category category)
    {
        _logger.LogInformation($"Adding new category: {category.Name}");
        return await _repository.AddAsync(category);
    }

    public async Task<Result<Category, CategoryError.NotFound>> UpdateAsync(string id, Category category)
    {
        _logger.LogInformation($"Updating category: {category.Id}");
        var existingCategory = await _repository.UpdateAsync(id, category);
        if (existingCategory == null)
        {
            throw new CategoryError.NotFound(id);
        }

        return existingCategory;
    }

    public async Task<Category> DeleteAsync(string id)
    {
        _logger.LogInformation($"Deleting category: {id}");
        var category = await _repository.DeleteAsync(id);
        if (category == null)
        {
            throw new CategoryError.NotFound(id);
        }
        return category;
    }

    public async Task<List<Category>> ImportByCsvAsync(IFormFile file)
    {
        _logger.LogInformation($"Importing categories from CSV: {file.FileName}");
    
        var categories = new List<Category>();

        try
        {
            if (file == null || file.Length == 0)
            {
                throw new MinFileSizeStorageException();
            }
            
            if (!file.ContentType.Contains("text/csv"))
            {
                throw new FileExtensionNotAllowedException(file.FileName);
            }
            
            await using var stream = file.OpenReadStream();
            await foreach (var category in _categoryStorageCsv.ImportAsync(stream))
            {
                // Procesar y agregar la categoría
                categories.Add(category);
                await _repository.AddAsync(category);
            }

            _logger.LogInformation($"Successfully imported {categories.Count} categories.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error importing categories from CSV: {ex.Message}", ex);
            throw;
        }

        return categories;
    }


}