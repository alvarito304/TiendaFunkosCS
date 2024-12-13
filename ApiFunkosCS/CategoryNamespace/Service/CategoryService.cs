using ApiFunkosCS.CategoryNamespace.Errors;
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.CategoryNamespace.Repository;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.CategoryNamespace.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;
    
    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger)
    {
        _repository = repository;
        _logger = logger;
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
}