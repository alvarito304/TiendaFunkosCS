using ApiFunkosCS.CategoryNamespace.Errors;
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.CategoryNamespace.Repository;
using ApiFunkosCS.Storage.Exceptions;
using ApiFunkosCS.Storage.ProcessingFile.Services;
using ApiFunkosCS.Storage.ProcessingFile.Services.Categories;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.CategoryNamespace.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly ILogger<CategoryService> _logger;
    private readonly ICategoryStorageCsv _categoryStorageCsv;
    private readonly ICategoryStorageJson _categoryStorageJson;
    
    public CategoryService(ICategoryRepository repository, ILogger<CategoryService> logger, ICategoryStorageCsv categoryStorageCsv, ICategoryStorageJson categoryStorageJson)
    {
        _repository = repository;
        _logger = logger;
        _categoryStorageCsv = categoryStorageCsv;
        _categoryStorageJson = categoryStorageJson;
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
        category.Id = id;
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

    public async Task<FileStream> ExportCsvAsync()
    {
        var filePath = $"categories-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.csv";
        _logger.LogInformation($"Exporting categories to CSV: {filePath}");

        var categories = await _repository.GetAllAsync();

        // Crear el archivo y escribir los datos
        await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await _categoryStorageCsv.ExportAsync(categories, fileStream);
        }

        // Abrir un nuevo flujo para lectura
        var readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return readStream;
    }

    public async Task<List<Category>> ImportByJsonAsync(IFormFile file)
    {
        _logger.LogInformation($"Importing categories from JSON: {file.FileName}");
        
        var categories = new List<Category>();
        
        if (file == null || file.Length == 0)
        {
            throw new MinFileSizeStorageException();
        }
            
        if (!file.ContentType.Contains("application/json"))
        {
            throw new FileExtensionNotAllowedException(file.FileName);
        }
        
        await using var stream = file.OpenReadStream();
        await foreach (var category in _categoryStorageJson.ImportAsync(stream))
        {
            // Procesar y agregar la categoría
            categories.Add(category);
            await _repository.AddAsync(category);
        }
        _logger.LogInformation($"Successfully imported {categories.Count} categories.");
        return categories;
    }

    public async Task<FileStream> ExportJsonAsync()
    {
        var filePath = $"categories-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json";
        _logger.LogInformation($"Exporting categories to JSON: {filePath}");

        var categories = await _repository.GetAllAsync();

        // Crear el archivo y escribir los datos
        await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await _categoryStorageJson.ExportAsync(categories, fileStream);
        }

        // Abrir un nuevo flujo para lectura
        var readStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return readStream;
    }
}