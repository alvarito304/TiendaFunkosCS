using ApiFunkosCS.CategoryNamespace.Database;
using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.Database;
using ApiFunkosCS.Utils;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApiFunkosCS.CategoryNamespace.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _collection;
    private readonly ILogger<CategoryRepository> _logger;
    
    public CategoryRepository(IOptions<CategoryDatabaseSettings> categoryDatabaseSettings, ILogger<CategoryRepository> logger)
    {
        var mongoClient = new MongoClient(
            categoryDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            categoryDatabaseSettings.Value.DatabaseName);

        _collection = mongoDatabase.GetCollection<Category>(
            categoryDatabaseSettings.Value.CategoryCollectionName);
    }
    public async Task<List<Category>> GetAllAsync() => await _collection.Find(_ => true).ToListAsync();

    public async Task<Category> GetByIdAsync(string id) => await _collection.Find(category => category.Id == id).FirstOrDefaultAsync();

    public async Task<Category> AddAsync(Category category)
    {
        await _collection.InsertOneAsync(category);
        return category;
    }
    public async Task<Category> UpdateAsync(string id, Category category)
    {
        var updatedCategory = await _collection.FindOneAndReplaceAsync(
            document => document.Id == id,
            category,
            new FindOneAndReplaceOptions<Category> { ReturnDocument = ReturnDocument.After }
        );
        return updatedCategory;
    }

    public async Task<Category> DeleteAsync(string id)
    {
        var deleteCategory = await _collection.FindOneAndDeleteAsync(c => c.Id == id);
        return deleteCategory;
    }

    public void DeleteAllAsync()
    {
        _collection.DeleteMany(new BsonDocument());
    }
}