using ApiFunkosCS.CategoryNamespace.Errors;
using ApiFunkosCS.CategoryNamespace.Model;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.CategoryNamespace.Service;

public interface ICategoryService
{
    Task<List<Category>> FindAllAsync();
    Task<Result<Category, CategoryError.NotFound>> FindByIdAsync(string id);
    Task<Category> AddAsync(Category category);
    Task<Result<Category, CategoryError.NotFound>> UpdateAsync(string id, Category category);
    Task<Category> DeleteAsync(string id);
    Task<List<Category>> ImportByCsvAsync(IFormFile file);   
    Task<FileStream> ExportCsvAsync();
    
    Task<List<Category>> ImportByJsonAsync(IFormFile file); 
    
    Task<FileStream> ExportJsonAsync();
}