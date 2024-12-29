
using ApiFunkosCS.CategoryNamespace.Model;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.CategoryNamespace.Repository;

public interface ICategoryRepository 
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(string id);
    Task<Category> AddAsync(Category category);
    Task<Category> UpdateAsync(string id, Category category);
    Task<Category> DeleteAsync(string id);
    void DeleteAllAsync();
}